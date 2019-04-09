using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CrawlerApp.DataStore
{
    public class DataStorageMySql : IDataStorage<Link>
    {

        private string _connectionString = "server=school-projects.mysql.database.azure.com;uid=wayfrae@school-projects;pwd=Password1;database=webcrawler";
        private List<Link> _list;
        private MySqlConnection _connection;

        public DataStorageMySql(List<Link> list, MySqlConnection connection)
        {
            _list = list;
            _connection = connection;
            _connection.ConnectionString = _connectionString;

        }

        public void Create(Link obj)
        {
                try
                {
                    _connection.Open();
                    MySqlCommand cmd = _connection.CreateCommand();
                    //cmd.CommandText = "INSERT INTO links(Address, Response, IsCrawled) SELECT * FROM (SELECT @address, @response, @isCrawled) AS tmp WHERE NOT EXISTS(SELECT Address FROM links WHERE Address = @address); ";
                    cmd.CommandText = "INSERT INTO links(Address, Response, IsCrawled, `Date`, `FoundOn`) VALUES(@address, @response, @isCrawled, @date, @foundOn) ON DUPLICATE KEY UPDATE Address=@address, Response=@response, IsCrawled=@isCrawled, Date=@date, FoundOn=@foundOn;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@address", MySqlDbType.LongText).Value = obj.Address;
                    cmd.Parameters.Add("@response", MySqlDbType.LongText).Value = obj.Response;
                    cmd.Parameters.Add("@isCrawled", MySqlDbType.Bit).Value = obj.IsCrawled;
                    cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = obj.Date;
                    cmd.Parameters.Add("@foundOn", MySqlDbType.LongText).Value = obj.FoundOn;
                    var rowsAffected = cmd.ExecuteNonQuery();
                    obj.ID = cmd.LastInsertedId;                    
                }
            catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }       
                _connection.Close();

        }

        public void Delete(Link obj)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    string sql = $"DELETE FROM links WHERE ID = {obj.ID};";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public IEnumerable<Link> GetAll()
        {
            _list.Clear();
            
                try
                {
                    MySqlCommand cmd = _connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM links";
                    _connection.Open();
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Link link = CreateLink(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4]);
                            _list.Add(link);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            _connection.Close();

            return _list;
        }        

        public Link GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Link obj)
        {
            throw new NotImplementedException();
        }
        
        private Link CreateLink(object v1, object v2, object v3, object v4, object v5)
        {
            var i = v4.ToString();
            return new Link
            {
                ID = long.Parse(v1.ToString()),
                Address = v2.ToString(),
                Response = v3.ToString(),
                IsCrawled = v4.ToString().Equals("0") ? false : true,
                Date = (DateTime)v5,
                FoundOn = v5.ToString()
            };
        }
    }
}
