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
              

        public async Task NotifyChange()
        {
            await Clients.All.SendAsync("NotifyChange");
        }
    }
}
