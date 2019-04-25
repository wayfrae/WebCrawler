using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using CrawlerApp.DataStore;
using RobotsTxt;
using System.Threading;

namespace CrawlerApp.Console
{
    public class Crawler : ICrawler
    {
        private readonly HttpClient _httpClient;
        private readonly IDataStorage<Link> _storage;
        private readonly string _userAgent;
        private int _threadsAvailable;
        private readonly IScheduler _scheduler;
        private int _threadLimit;
        private readonly object _lock;

        public bool IsCrawling { get; private set; }

        public Crawler(HttpClient client, IDataStorage<Link> storage, IScheduler scheduler, int numberOfThreads, object @lock)
        {
            _httpClient = client;
            _userAgent = "WeberStateUniversityWebCrawler / 1.0 wayfrae";
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);

            _scheduler = scheduler;
            _storage = storage;
            _threadLimit = numberOfThreads;
            _lock = @lock;
        }

        public async Task<bool> Start(Uri urlToCrawl)
        {
            IsCrawling = true;
            await FillScheduler();
            var tasks = new List<Task>();
            ThreadPool.GetAvailableThreads(out _threadsAvailable, out _);

            while (IsCrawling && _threadsAvailable > _threadLimit)
            {
                for (var i = _threadLimit; i > 0; i--)
                {
                    Uri crawl = urlToCrawl;
                    tasks.Add(Task.Run(() => StartCrawlerAsync(crawl, new Link())));
                    _threadLimit--;
                    if (_scheduler.HasNext())
                    {
                        urlToCrawl = StringToUri(_scheduler.GetNext().Address);
                    }
                }
            }

            //Single-threaded crawler
            //while (true) 
            //{
            //    Uri crawl = urlToCrawl;
            //    //tasks.Add(Task.Run(() => StartCrawlerAsync(crawl, new Link())));
            //    await StartCrawlerAsync(crawl, new Link());
            //    _threadLimit--;
            //    if (_scheduler.HasNext())
            //    {
            //        urlToCrawl = StringToUri(_scheduler.GetNext().Address);
            //    }
            //}
            await Task.WhenAll(tasks);
            return false;
        }

        private async Task FillScheduler()
        {
            foreach (Link link in await _storage.GetAll())
            {
                if (!link.IsCrawled)
                {
                    _scheduler.Push(link);
                }
            }
        }

        private async Task StartCrawlerAsync(Uri startingUrlToCrawl, Link crawlerObjective)
        {
            Uri urlToCrawl = startingUrlToCrawl;

            while (IsCrawling)
            {
                await Crawl(urlToCrawl, crawlerObjective);
                var linksToCrawl = await _storage.GetAll();
                var toCrawl = linksToCrawl.ToList();
                foreach (Link link in toCrawl)
                {
                    if (link.IsCrawled || toCrawl.Find(x => x.Address.Equals(link.Address)) != null) continue;
                        
                    toCrawl.Add(link); 
                }

                if (_scheduler.HasNext())
                {
                    urlToCrawl = StringToUri(_scheduler.GetNext().Address);
                    IsCrawling = true;
                }
                else
                {
                    _threadLimit++;
                    IsCrawling = false;
                }
            }                    
        }

        private static Uri StringToUri(string address)
        {
            return new Uri(address);
        }

        private async Task Crawl(Uri urlToCrawl, Link crawlerObjective)
        {
            try
            {
                Robots robot = await GetRobotTxt(urlToCrawl);

                if (robot == null || robot.IsPathAllowed(_userAgent, urlToCrawl.AbsoluteUri))
                {
                    using (HttpResponseMessage html = await _httpClient.GetAsync(urlToCrawl))
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
                            using (HttpContent content = html.Content)
                            {
                                var page = await content.ReadAsStringAsync();
                                HtmlDocument htmlDocument = GetHtmlDocument();
                                htmlDocument.LoadHtml(page);
                                ParseHtml(urlToCrawl, htmlDocument);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                System.Console.Write("\nCrawl: ");
                System.Console.Write(e.Message);
            }
        }

        private static HtmlDocument GetHtmlDocument()
        {
            return new HtmlDocument();
        }

        private async Task<Robots> GetRobotTxt(Uri urlToCrawl)
        {
            try
            {
                using (HttpResponseMessage robotsTxt = await _httpClient.GetAsync(urlToCrawl.AbsoluteUri + "/robots.txt"))
                {
                    if (robotsTxt.IsSuccessStatusCode)
                    {
                        return Robots.Load(await robotsTxt.Content.ReadAsStringAsync());
                    }                    
                }
            }
            catch (Exception e)
            {

                System.Console.Write("\nGetRobot: ");
                System.Console.Write(e.Message);
            }
            return null;
        }

        private void ParseHtml(Uri urlToCrawl, HtmlDocument htmlDocument)
        {
            try
            {
                //check that there is html to parse
                if (htmlDocument.DocumentNode.SelectSingleNode("html") == null) return;

                foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
                {
                    var absolutePath = GetAbsolutePath(urlToCrawl, link);
                    if (absolutePath == null) return;
                    
                    Link linkToSave = CreateLink(GetAbsolutePath(urlToCrawl, link));
                    if (!linkToSave.Address.Substring(0, 4).ToLower().Equals("http")) continue;
                    _scheduler.Push(linkToSave);
                    linkToSave.FoundOn = urlToCrawl.AbsoluteUri;
                    _storage.Create(linkToSave);
                }
            }
            catch (Exception e)
            {

                System.Console.Write("\nParseHtml: ");
                System.Console.Write(e.Message);
            }
        }        

        private static Link CreateLink(string url)
        {
            var definition = new Link
            {
                Address = url,
                IsCrawled = false
            };

            return definition;
        }

        private static string GetAbsolutePath(Uri baseUrl, HtmlNode link)
        {
            var relativePath = link.GetAttributeValue("href", string.Empty);
            return new Uri(baseUrl, relativePath).AbsoluteUri;
        }

        public void Stop()
        {
            Environment.Exit(Environment.ExitCode);
        }
                
    }
}
