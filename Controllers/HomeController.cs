using CustomIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Diagnostics;

namespace CustomIdentity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult food()
        {
            return View();
        }

        public IActionResult Portal()
        {
            return View();
        }

        public IActionResult Client()
        {
            var clientInfo = new Client_Db();
            clientInfo.OnGet(); // Assuming OnGet() retrieves client information
            return View(clientInfo);
        }

        public IActionResult Create_Client()
        {
            var createDb = new Create_Db();
            createDb.OnGet(); // Assuming OnGet() retrieves data needed for creating a client
            return View(createDb);
        }

        [HttpPost]
        public IActionResult Create_Client(Create_Db createDb)
        {
            createDb.OnPost(HttpContext);

            if (!string.IsNullOrEmpty(createDb.ErrorMessage))
            {
                ModelState.AddModelError("", createDb.ErrorMessage);
                return View(createDb); // Return the view with errors
            }

            TempData["SuccessMessage"] = createDb.SuccessMessage;
            return RedirectToAction("Client"); // Redirect to client list after successful creation
        }

        public IActionResult Edit_Client(string id)
        {
            var editDb = new Edit_Db();
            editDb.OnGet(id); // Load client information for editing based on id
            return View(editDb);
        }

        [HttpPost]
        public IActionResult Edit_Client(Edit_Db editDb)
        {
            editDb.OnPost(HttpContext);

            if (!string.IsNullOrEmpty(editDb.errorMessage))
            {
                ModelState.AddModelError("", editDb.errorMessage);
                return View(editDb); // Return the view with errors
            }

            TempData["SuccessMessage"] = editDb.successMessage;
            return RedirectToAction("Client"); // Redirect to client list after successful edit
        }
        public IActionResult Delete_Client(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound("Client ID is required.");
                }

                string connectionString = "Data Source=LAPTOP-S9QPQDAD\\MSSQLSERVER01;Initial Catalog=CustomIdentityDB;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM clients WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Client deleted successfully.";
                        }
                        else
                        {
                            return NotFound("Client not found.");
                        }
                    }
                }

                return RedirectToAction("Client");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
