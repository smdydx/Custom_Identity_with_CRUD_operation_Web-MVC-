using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace CustomIdentity.Models
{
    public class Create_Db
    {
        public ClientInfo ClientInfo { get; set; } = new ClientInfo();
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        // Method to handle GET operation (display form)
        public void OnGet()
        {
            // Optionally initialize any data needed for the form
        }

        // Method to handle POST operation (process form submission)
        public void OnPost(HttpContext context)
        {
            ClientInfo.Name = context.Request.Form["ClientInfo.Name"];
            ClientInfo.Email = context.Request.Form["ClientInfo.Email"];
            ClientInfo.Phone = context.Request.Form["ClientInfo.Phone"];
            ClientInfo.Address = context.Request.Form["ClientInfo.Address"];

            if (string.IsNullOrEmpty(ClientInfo.Name) || string.IsNullOrEmpty(ClientInfo.Email) || string.IsNullOrEmpty(ClientInfo.Phone) || string.IsNullOrEmpty(ClientInfo.Address))
            {
                ErrorMessage = "All fields are required.";
                return; // Exit the method if any field is empty
            }

            try
            {
                string connectionString = "Data Source=LAPTOP-S9QPQDAD\\MSSQLSERVER01;Initial Catalog=CustomIdentityDB;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Clients (Name, Email, Phone, Address) VALUES (@Name, @Email, @Phone, @Address)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", ClientInfo.Name);
                        command.Parameters.AddWithValue("@Email", ClientInfo.Email);
                        command.Parameters.AddWithValue("@Phone", ClientInfo.Phone);
                        command.Parameters.AddWithValue("@Address", ClientInfo.Address);
                        command.ExecuteNonQuery();
                    }
                }

                // Clear the form fields after successful submission
                ClientInfo.Name = "";
                ClientInfo.Email = "";
                ClientInfo.Phone = "";
                ClientInfo.Address = "";

                SuccessMessage = "Client information saved successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
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
