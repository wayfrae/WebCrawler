using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using CrawlerApp.DataStore;
using RobotsTxt;
using System.IO;
using System.Threading;

namespace CrawlerApp.Console
{
    public class Crawler : ICrawler
    {
        private Uri _currentAddress;
        private HttpClient _httpClient;
        private HtmlDocument _htmlDocument;
        private IDataStorage<Link> _storage;
        private string _userAgent;
        private int _threadsAvailable;
        private int _IOthreadsAvailable;
        private Robots robot { get; set; }
        private IScheduler _scheduler;
        private int _threadLimit;

        public bool IsCrawling { get; private set; }

        public Crawler(HttpClient client, HtmlDocument document, IDataStorage<Link> storage, IScheduler scheduler, int numberOfThreads)
        {
            _httpClient = client;
            _userAgent = "WeberStateUniversityWebCrawler / 1.0 wayfrae";
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);

            _scheduler = scheduler;
            _htmlDocument = document;
            _storage = storage;
            _threadLimit = numberOfThreads;
            
        }

        public async Task<bool> Start(Uri urlToCrawl)
        {
            IsCrawling = true;
            _currentAddress = urlToCrawl;
            foreach(var link in _storage.GetAll())
            {
                if (!link.IsCrawled)
                {
                    _scheduler.Push(link);
                }
            }
            List<Task> tasks = new List<Task>();
            ThreadPool.GetAvailableThreads(out _threadsAvailable, out _IOthreadsAvailable);
            while (IsCrawling && _threadsAvailable > _threadLimit)
            {
                for (var i = _threadLimit; i > 0; i--)
                {
                    tasks.Add(Task.Run(()=>StartCrawlerAsync(urlToCrawl, new Link())));
                    _threadLimit--;
                    if (_scheduler.HasNext())
                    {
                        urlToCrawl = StringToUri(_scheduler.GetNext().Address);
                    }
                }
            }
            await Task.WhenAll(tasks);
            return false;
        }

        private async Task StartCrawlerAsync(Uri startingUrlToCrawl, Link crawlerObjective)
        {
            Uri urlToCrawl = startingUrlToCrawl;

            while (IsCrawling)
            {
                
                await Crawl(urlToCrawl, crawlerObjective);
                List<Link> linksToCrawl = _storage.GetAll() as List<Link>;
                foreach(Link link  in linksToCrawl)
                {
                    if (!link.IsCrawled && linksToCrawl.Find(x=>x.Address.Equals(link.Address)) == null)
                    {
                        linksToCrawl.Add(link);
                    }
                }
                
                if (_scheduler.HasNext())
                {
                    urlToCrawl = StringToUri(_scheduler.GetNext().Address);                    
                }
                else
                {
                    _threadLimit++;
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
            //check that there is html to parse
            if(htmlDocument.DocumentNode.SelectSingleNode("html") != null)
            {
                foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
                {
                    var linkToSave = CreateLink(GetAbsolutePath(urlToCrawl, link), new Link());
                    if (!linkToSave.Address.Contains("javascript"))
                    {
                        _scheduler.Push(linkToSave);
                        linkToSave.FoundOn = urlToCrawl.AbsoluteUri;
                        _storage.Create(linkToSave);
                    }
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

        public void Stop()
        {
            throw new NotImplementedException();
        }
                
    }
}
