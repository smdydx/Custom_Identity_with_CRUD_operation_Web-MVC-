using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace CustomIdentity.Models
{
    public class Edit_Db
    {
        public ClientInfo clientInfo { get; set; } = new ClientInfo();
        public string errorMessage { get; set; }
        public string successMessage { get; set; }

        public void OnGet(string id)
        {
            try
            {
                string connectionString = "Data Source=LAPTOP-S9QPQDAD\\MSSQLSERVER01;Initial Catalog=CustomIdentityDB;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.Id = reader["id"].ToString();
                                clientInfo.Name = reader["name"].ToString();
                                clientInfo.Email = reader["email"].ToString();
                                clientInfo.Phone = reader["phone"].ToString();
                                clientInfo.Address = reader["address"].ToString();
                            }
                            else
                            {
                                errorMessage = "Client not found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost(HttpContext context)
        {
            clientInfo.Id = context.Request.Form["id"];
            clientInfo.Name = context.Request.Form["name"];
            clientInfo.Email = context.Request.Form["email"];
            clientInfo.Phone = context.Request.Form["phone"];
            clientInfo.Address = context.Request.Form["address"];

            if (string.IsNullOrEmpty(clientInfo.Id) || string.IsNullOrEmpty(clientInfo.Name) || string.IsNullOrEmpty(clientInfo.Email) || string.IsNullOrEmpty(clientInfo.Phone) || string.IsNullOrEmpty(clientInfo.Address))
            {
                errorMessage = "All fields are required.";
                return;
            }

            try
            {
                string connectionString = "Data Source=LAPTOP-S9QPQDAD\\MSSQLSERVER01;Initial Catalog=CustomIdentityDB;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients " +
                                 "SET name=@name, email=@email, phone=@phone, address=@address " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.Name);
                        command.Parameters.AddWithValue("@email", clientInfo.Email);
                        command.Parameters.AddWithValue("@phone", clientInfo.Phone);
                        command.Parameters.AddWithValue("@address", clientInfo.Address);
                        command.Parameters.AddWithValue("@id", clientInfo.Id);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Client updated successfully!";
                        }
                        else
                        {
                            errorMessage = "Failed to update client.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
