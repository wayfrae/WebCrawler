using CrawlerApp.DataStore;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrawlerApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var links = new List<Link>();
            //var storage = new DataStorageMySql(links);
            //var storage = new DataStorageLocal(links);
            var storage = new DataStorageAPI(links);
            Task start;
            var list = new List<Link>();
            var scheduler = new Scheduler(new List<Link>(), new object());
            var crawler = new Crawler(new HttpClient(), storage, scheduler, 16, new object());
            var url = Environment.GetCommandLineArgs();
            if (url.Length > 1)
            {
                System.Console.WriteLine(url);
                start = crawler.Start(new Uri(url[1]));
            }
            else
            {
                System.Console.WriteLine("____________________________________");
                System.Console.WriteLine("|****Welcome to the web crawler****|");
                System.Console.WriteLine("|****     By: Zach Frazier     ****|");
                System.Console.WriteLine("Instructions:");
                System.Console.WriteLine("start [web address] (begins crawling at specified address)");
                System.Console.WriteLine("stop (exits the program immediately)");

                while (true)
                {
                    System.Console.Write(">");
                    var input = System.Console.ReadLine();
                    if (input.Length > 4 && input.Substring(0, 5).ToLower().Equals("start"))
                    {
                        var inputSplit = input.Split(' ');
                        try
                        {
                            var crawlAddress = new Uri(inputSplit[1]);
                            Task.Run(() => crawler.Start(crawlAddress));
                        }
                        catch(Exception e)
                        {
                            System.Console.WriteLine("Program.cs:  " + e.Message);
                            continue;
                        }
                        
                        break;
                    }
                    else
                    {
                        if (input.Length == 0) continue;
                        System.Console.WriteLine("Invalid command. Use: 'start [web address]' to start the crawler.");
                    }
                }
            }
            
            System.Console.WriteLine("The crawler is running. Type 'stop' to stop...");

            while (true)
            {
                System.Console.Write(">");
                var input = System.Console.ReadLine();
                if (input.ToLower().Equals("stop"))
                {
                    crawler.Stop();
                    System.Console.WriteLine("The crawler has stopped.");
                    break;
                }
                else
                {
                    if (input.Length == 0) continue;
                    System.Console.WriteLine("Invalid command.");
                }
            }
        }
    }
}
