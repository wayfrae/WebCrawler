using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using CrawlerApp.DataStore;

namespace CrawlerApp.Console
{
    public class Crawler
    {
        private Uri _currentAddress;
        private HttpClient _httpClient;
        private HtmlDocument _htmlDocument;
        private IDataStorage<Link> _storage;

        public List<Link> LinksToCrawl { get; private set; }
        public bool IsCrawling { get; private set; }

        public Crawler(HttpClient client, HtmlDocument document, IDataStorage<Link> storage, List<Link> list)
        {
            _httpClient = client;
            _htmlDocument = document;
            _storage = storage;
            LinksToCrawl = list;
        }

        public async Task<bool> Start(Uri urlToCrawl)
        {
            IsCrawling = true;
            _currentAddress = urlToCrawl;

            await StartCrawlerAsync(urlToCrawl, new Link(new List<AssociatedLink>()));

            return false;
        }

        private async Task StartCrawlerAsync(Uri startingUrlToCrawl, Link crawlerObjective)
        {
            Uri urlToCrawl = startingUrlToCrawl;

            while (IsCrawling)
            {
                await Crawl(urlToCrawl, crawlerObjective);

                foreach(Link link in _storage.GetAll())
                {
                    if (!link.IsCrawled && LinksToCrawl.Find(x=>x.Address.Equals(link.Address)) == null)
                    {
                        LinksToCrawl.Add(link);
                    }
                }

                if (LinksToCrawl.Count > 0)
                {
                    urlToCrawl = StringToUri(LinksToCrawl[0].Address);
                    LinksToCrawl.RemoveAt(0);
                }
                else
                {
                    IsCrawling = false;
                }
            }                    
        }

        private Uri StringToUri(string address)
        {
            return new Uri(address);
        }

        private async Task Crawl(Uri urlToCrawl, Link crawlerObjective)
        {
            try
            {                
                using (var html = await _httpClient.GetAsync(urlToCrawl))
                {                  
                    if (html.IsSuccessStatusCode)
                    {
                        using (var content = html.Content)
                        {                            
                            var page = await content.ReadAsStringAsync();
                            _htmlDocument.LoadHtml(page);

                            ParseAndSave(urlToCrawl, _htmlDocument, crawlerObjective, new AssociatedLink());                            
                        }
                    }
                    crawlerObjective.Address = urlToCrawl.AbsoluteUri;
                    crawlerObjective.Response = html.ToString();
                    crawlerObjective.IsCrawled = true;
                    _storage.Create(crawlerObjective);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ParseAndSave(Uri urlToCrawl, HtmlDocument htmlDocument, Link linkObject, AssociatedLink associatedLink)
        {            
            foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
            {
                associatedLink.Address = link.GetAttributeValue("href", string.Empty);
                linkObject.AssociatedLinks.Add(associatedLink);
                _storage.Create(CreateObject(GetAbsolutePath(urlToCrawl, link), new Link(new List<AssociatedLink>())));
            }
                       
        }        

        private Link CreateObject(string url, Link definition)
        {
            definition.Address = url;
            definition.IsCrawled = false;

            return definition;
        }

        private string GetAbsolutePath(Uri baseUrl, HtmlNode link)
        {
            return new Uri(baseUrl, link.GetAttributeValue("href", string.Empty)).AbsoluteUri;
        }                
    }
}
