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
            Link startLink = new Link
            {
                Address = "http://example.com/"
            };
            List<Link> links = new List<Link>();

            DataStorage storage = new DataStorage(links);

            Link link = new Link
            {
                FoundLinks = new System.Collections.Generic.List<string>()
            };
            List<string> list = new List<string>();
            Crawler crawler = new Crawler(new HttpClient(), new HtmlDocument(), storage, list);

            crawler.Start("http://example.com/", link);
            System.Console.ReadLine();
        }
    }
}
