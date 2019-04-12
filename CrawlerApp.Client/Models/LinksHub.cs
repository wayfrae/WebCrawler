using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrawlerApp.Client.Services;
using CrawlerApp.DataStore;
using System.Timers;

namespace CrawlerApp.Client.Models
{
    public class LinksHub : Hub
    {
        DataStorageMySql db;
        int previousCount = 0;

        public override Task OnConnectedAsync()
        {
            db = new DataStorageMySql(new List<DataStore.Link>());
            Start();
            return base.OnConnectedAsync();
        }

        string connectionString = "server=school-projects.mysql.database.azure.com;uid=wayfrae@school-projects;pwd=Password1;database=webcrawler";

        void Start()
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            int value = db.CountRows();
            if (previousCount != value)
            {
                NotifyChange();
            }
            previousCount = value;
        }

        void Stop()
        {
        }

        public async Task NotifyChange()
        {
            await Clients.All.SendAsync("NotifyChange");
        }
    }
}
