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
        private Link _currentLink;
        private HttpClient _httpClient;
        private HtmlDocument _htmlDocument;
        private IDataStorage<Link> _storage;
        public List<string> Links { get; private set; }

        public Crawler(Link startingLink, HttpClient client, HtmlDocument document, IDataStorage<Link> storage)
        {
            _currentLink = startingLink;
            _httpClient = client;
            _htmlDocument = document;
            _storage = storage;
        }

        private async Task StartCrawlerAsync(Link emptyLink)
        {
            var html = await _httpClient.GetStringAsync(_currentLink.Address);            
            _currentLink.Response = html;
            _htmlDocument.LoadHtml(html);
            foreach (HtmlNode link in _htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
            {
                // Get the value of the HREF attribute
                string href = link.GetAttributeValue("href", string.Empty);
                emptyLink.Address = href;
                _storage.Create(emptyLink);  
            }
            _currentLink.IsCrawled = true;
        }
    }
}
