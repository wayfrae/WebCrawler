using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CrawlerApp.DataStore
{
    public class DataStorageMySql : IDataStorage<Link>
    {

        private string _connectionString = "server=school-projects.mysql.database.azure.com;uid=wayfrae@school-projects;pwd=Password1;database=webcrawler";
        private readonly List<Link> _links;
        private readonly object _lock = new object();

        public DataStorageMySql(List<Link> list)
        {
            _links = list;

        }
        
        public void Create(Link obj)
        {
                try
                {
                    using(var connection = new MySqlConnection(_connectionString))
                    {
                        connection.Open();
                        MySqlCommand cmd = connection.CreateCommand();
                        cmd.CommandText = "INSERT INTO links(Address, Response, IsCrawled, `Date`, `FoundOn`) VALUES(@address, @response, @isCrawled, @date, @foundOn) ON DUPLICATE KEY UPDATE Address=@address, Response=@response, IsCrawled=@isCrawled, Date=@date, FoundOn=@foundOn;";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("@address", MySqlDbType.LongText).Value = obj.Address;
                        cmd.Parameters.Add("@response", MySqlDbType.LongText).Value = obj.Response;
                        cmd.Parameters.Add("@isCrawled", MySqlDbType.Bit).Value = obj.IsCrawled;
                        cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = obj.Date;
                        cmd.Parameters.Add("@foundOn", MySqlDbType.LongText).Value = obj.FoundOn;
                        cmd.ExecuteNonQuery();
                        obj.ID = cmd.LastInsertedId;
                    }
                                        
                }
            catch (Exception ex)
                {
                    Console.WriteLine("MySQL Create:" + ex);
                }       

        }

        public void Delete(Link obj)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    var sql = $"DELETE FROM links WHERE ID = {obj.ID};";
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        public IEnumerable<Link> GetAll()
        {
            lock (_lock)
            {
                _links.Clear();

                try
                {
                    using (var connection = new MySqlConnection(_connectionString))
                    {
                        MySqlCommand cmd = connection.CreateCommand();
                        cmd.CommandText = "SELECT * FROM links";
                        connection.Open();
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                Link link = CreateLink(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4]);
                                _links.Add(link);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Get all: " + ex);
                }

                return _links;
            }
        }

        public int CountRows()
        {
            
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM links";
                    connection.Open();
                    return int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return 0;
        }

        public Link GetByID(int id)
        {
            if(FindLink(id) == null)
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

        public bool Update(Link obj)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"UPDATE links SET Address=@address, Response=@response, IsCrawled=@isCrawled, Date=@date, FoundOn=@foundOn WHERE id={obj.ID};";
                cmd.Parameters.Add("@address", MySqlDbType.LongText).Value = obj.Address;
                cmd.Parameters.Add("@response", MySqlDbType.LongText).Value = obj.Response;
                cmd.Parameters.Add("@isCrawled", MySqlDbType.Bit).Value = obj.IsCrawled;
                cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = obj.Date;
                cmd.Parameters.Add("@foundOn", MySqlDbType.LongText).Value = obj.FoundOn;
                var rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected != 0;
            }

        }
        
        private static Link CreateLink(object v1, object v2, object v3, object v4, object v5)
        {
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
