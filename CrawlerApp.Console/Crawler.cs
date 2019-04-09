using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using CrawlerApp.DataStore;
using RobotsTxt;
using System.IO;

namespace CrawlerApp.Console
{
    public class Crawler
    {
        private Uri _currentAddress;
        private HttpClient _httpClient;
        private HtmlDocument _htmlDocument;
        private IDataStorage<Link> _storage;
        private string _userAgent;

        private Robots robot { get; set; }

        public List<Link> LinksToCrawl { get; private set; }
        public bool IsCrawling { get; private set; }

        public Crawler(HttpClient client, HtmlDocument document, IDataStorage<Link> storage, List<Link> list)
        {
            _httpClient = client;
            _userAgent = "WeberStateUniversityWebCrawler / 1.0 wayfrae";
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);

            _htmlDocument = document;
            _storage = storage;
            LinksToCrawl = list;
        }

        public async Task<bool> Start(Uri urlToCrawl)
        {
            IsCrawling = true;
            _currentAddress = urlToCrawl;

            await StartCrawlerAsync(urlToCrawl, new Link());

            return false;
        }

        private async Task StartCrawlerAsync(Uri startingUrlToCrawl, Link crawlerObjective)
        {
            Uri urlToCrawl = startingUrlToCrawl;

            while (IsCrawling)
            {
                await Crawl(urlToCrawl, crawlerObjective);

                foreach(Link link  in _storage.GetAll())
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
                /////******************************************************************//////
                //figure out how to check for download files and other types of responses
                ////*********************************************************************////
                using (var robotsTxt = await _httpClient.GetAsync(urlToCrawl.AbsoluteUri + "/robots.txt"))
                {
                    if (robotsTxt.IsSuccessStatusCode)
                    {
                        robot = Robots.Load(await robotsTxt.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        robot = null;
                    }
                }

                if(robot == null || robot.IsPathAllowed(_userAgent, urlToCrawl.AbsoluteUri))
                {
                    using (var html = await _httpClient.GetAsync(urlToCrawl))
                    {
                        crawlerObjective.Date = DateTime.Now;
                        crawlerObjective.Address = urlToCrawl.AbsoluteUri;
                        crawlerObjective.Response = html.ToString();
                        crawlerObjective.IsCrawled = true;
                        if (string.IsNullOrEmpty(crawlerObjective.FoundOn))
                        {
                            crawlerObjective.FoundOn = urlToCrawl.AbsoluteUri;
                        }

                        _storage.Create(crawlerObjective);

                        if (html.IsSuccessStatusCode)
                        {
                            using (var content = html.Content)
                            {
                                var page = await content.ReadAsStringAsync();
                                _htmlDocument.LoadHtml(page);
                                ParseHtml(urlToCrawl, _htmlDocument);
                            }
                        }
                    }
                }                
             }
            catch (Exception e )
            {

                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.StackTrace);
            }
        }

        private void ParseHtml(Uri urlToCrawl, HtmlDocument htmlDocument)
        {
            
                foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
                {
                    var linkToSave = CreateLink(GetAbsolutePath(urlToCrawl, link), new Link());
                    if (!linkToSave.Address.Contains("javascript"))
                    {
                        LinksToCrawl.Add(linkToSave);
                        linkToSave.FoundOn = urlToCrawl.AbsoluteUri;
                        _storage.Create(linkToSave);
                    }
                }
                  
        }        

        private Link CreateLink(string url, Link definition)
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
