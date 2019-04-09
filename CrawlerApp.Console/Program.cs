using CrawlerApp.DataStore;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrawlerApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Link> links = new List<Link>();
            DataStorageMySql storage = new DataStorageMySql(links, new MySql.Data.MySqlClient.MySqlConnection());

            List<Link> list = new List<Link>();
            Crawler crawler = new Crawler(new HttpClient(), new HtmlDocument(), storage, list);


            Task.Run(async () => { await crawler.Start(new Uri("http://example.com/")); }).GetAwaiter();
            System.Console.WriteLine("The crawler is running. Press any key to stop...");
            System.Console.ReadLine();
        }
    }
}
