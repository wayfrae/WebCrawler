using CrawlerApp.DataStore;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CrawlerApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Link startLink = new Link(new List<string>(), "http://example.com");
            List<Link> links = new List<Link>();

            DataStorage storage = new DataStorage(links);

            Link link = new Link(new System.Collections.Generic.List<string>());
            
            List<Link> list = new List<Link>();
            Crawler crawler = new Crawler(new HttpClient(), new HtmlDocument(), storage, list);

            while(crawler.Start(new Uri("http://example.com/")).Result);
        }
    }
}
