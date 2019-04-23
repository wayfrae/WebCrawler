using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace CrawlerApp.DataStore
{
    public class DataStorageAPI: IDataStorage<Link>
    {

        private string _baseAddress = "https://localhost:44307/links";
        private List<Link> _links;
        private readonly object _lock = new object();

        public DataStorageAPI(List<Link> list)
        {
            ServicePointManager.DefaultConnectionLimit = 16;
            _links = list;
        }        

        public async void Create(Link obj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var jsonLink = JsonConvert.SerializeObject(obj);
                    client.DefaultRequestHeaders.ConnectionClose = false;
                    client.BaseAddress = new Uri(_baseAddress);
                    var content = new StringContent(jsonLink.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage result = await client.PostAsync(_baseAddress, content);
                    if (!result.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"{result.StatusCode}: {result.ReasonPhrase}");
                        if (result.StatusCode == HttpStatusCode.Conflict)
                        {
                            Update(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Delete(Link obj)
        {
            
        }

        public async Task<IEnumerable<Link>> GetAll()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    HttpResponseMessage result = await client.GetAsync(_baseAddress);
                    result.EnsureSuccessStatusCode();
                    var str = await result.Content.ReadAsStringAsync();
                    lock (_lock)
                    {
                        _links = JsonConvert.DeserializeObject<List<Link>>(str);
                    }
                    return _links;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return _links;
        }

        public int CountRows()
        {
            return _links.Count;
        }

        public Link GetByID(int id)
        {
            if (FindLink(id) == null)
            {
                GetAll();
            }
            return FindLink(id);
        }

        private Link FindLink(int id)
        {
            Link link = _links.Find(x => x.ID == id);
            return link;
        }

        public async void Update(Link obj)
        {
            using (var client = new HttpClient())
            {
                var jsonLink = JsonConvert.SerializeObject(obj);
                    client.DefaultRequestHeaders.ConnectionClose = false;
                client.BaseAddress = new Uri(_baseAddress);
                var content = new StringContent(jsonLink.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage result = await client.PutAsync($"{_baseAddress}/{obj.ID}", content);
                if (!result.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{result.StatusCode}: {result.ReasonPhrase}");
                }
            }
        }

        private Link CreateLink(object v1, object v2, object v3, object v4, object v5)
        {
            var i = v4.ToString();
            return new Link
            {
                ID = long.Parse(v1.ToString()),
                Address = v2.ToString(),
                Response = v3.ToString(),
                IsCrawled = !v4.ToString().Equals("0"),
                Date = (DateTime)v5,
                FoundOn = v5.ToString()
            };
        }
    }
}
