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
        private string _currentAddress;
        private HttpClient _httpClient;
        private HtmlDocument _htmlDocument;
        private IDataStorage<Link> _storage;

        public List<string> LinksToCrawl { get; private set; }
        public bool IsCrawling { get; private set; }

        public Crawler(HttpClient client, HtmlDocument document, IDataStorage<Link> storage, List<string> list)
        {
            _httpClient = client;
            _htmlDocument = document;
            _storage = storage;
            LinksToCrawl = list;
        }

        public async Task Start(string urlToCrawl, Link emptyLink)
        {
            IsCrawling = true;
            _currentAddress = urlToCrawl;

            await StartCrawlerAsync(emptyLink);
        }

        private async Task StartCrawlerAsync(Link linkToSave)
        {
            while (IsCrawling)
            {
                using (var html = await _httpClient.GetAsync(_currentAddress))
                {
                    using (var content = html.Content)
                    {
                        var baseUrl = new Uri(_currentAddress);
                        linkToSave.Address = _currentAddress;
                        linkToSave.Response = await content.ReadAsStringAsync();
                        _htmlDocument.LoadHtml(linkToSave.Response);

                        foreach (HtmlNode link in _htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
                        {
                            var url = new Uri(baseUrl, link.GetAttributeValue("href", string.Empty));
                            linkToSave.FoundLinks.Add(url.AbsoluteUri);
                            LinksToCrawl.Add(url.AbsoluteUri);
                        }
                        linkToSave.IsCrawled = true;
                        _storage.Create(linkToSave);
                        LinksToCrawl.Remove(_currentAddress);                        
                        if(LinksToCrawl.Count > 0)
                        {
                            _currentAddress = LinksToCrawl[0];
                        }
                        else
                        {
                            IsCrawling = false;
                        }
                    }
                }
            }
                    
        }
    }
}
