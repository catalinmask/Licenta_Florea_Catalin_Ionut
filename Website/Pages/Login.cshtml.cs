using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Parola { get; set; }
        [BindProperty]
        public long id { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            string connectionString = "Server=localhost;Database=Licența;Uid=root;";
            string query = "SELECT COUNT(*),id_utilizator FROM utilizatori WHERE Email = @Email AND Parola = @Parola;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Parola", Parola);

                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync() && reader.GetInt64(0) == 1)
                        {
                            id = reader.GetInt32(1);
                            Response.Cookies.Append("UserId", id.ToString());
                            Response.Cookies.Append("userEmail", Email);
                            return RedirectToPage("/HomePage");
                        }
                        else
                        {
                            return Page();
                        }
                    }
                }
            }

        }
    }

}
