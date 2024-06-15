using System.Data.SqlClient;

namespace CustomIdentity.Models
{
    public class Client_Db
    {
        public List<ClientInfo> ListClients { get; private set; } = new List<ClientInfo>();

        public List<ClientInfo>  OnGet()
        {
            try
            {
                string connectionString = "Data Source=LAPTOP-S9QPQDAD\\MSSQLSERVER01;Initial Catalog=CustomIdentityDB;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo client = new ClientInfo
                                {
                                    Id = reader["id"].ToString(),
                                    Name = reader["name"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Phone = reader["phone"].ToString(),
                                    Address = reader["address"].ToString()
                                };

                                ListClients.Add(client);
                            }
                        }
                    }
                }
            }
            
            catch (Exception ex)
            {
                // Handle the error (e.g., log it, display an error message, etc.)
                Console.WriteLine(ex.Message);
            }
            return ListClients;
        }
        public class ClientInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
        }
    }
}
