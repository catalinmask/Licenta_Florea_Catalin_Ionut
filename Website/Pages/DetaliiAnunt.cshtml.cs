using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;

namespace Website.Pages
{
    public class DetaliiAnuntModel : PageModel
    {
        [BindProperty(SupportsGet = true)]

        public int id { get; set; }

        private readonly string _connectionString = "Server=localhost;Database=Licența;Uid=root;";
        public List<(int IdAnunt, int IdUtilizator,string TitluAnunt, string NumeProdus,DateTime DataPostareAnunt, decimal Pret,string StareAnunt,int IdSubcategorie)> Anunt { get; set; }
        public List<(int IdImagine,int IdAnunt,string UrlImagine)> Imagini { get; set; }
        public string Mesaj { get; set; }
        public int idDestinatar { get; set; }
        public int idExpeditor { get; set; }

        public IActionResult OnGet(int id)
        {
            Anunt = new List<(int, int, string, string, DateTime, decimal, string, int)>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Anunturi WHERE id_anunturi=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var detaliiAnunt=(
                                reader.GetInt32("id_anunturi"),
                                reader.GetInt32("id_utilizator"),
                                reader.GetString("TitluAnunt"),
                                reader.GetString("NumeProdus"),
                                reader.GetDateTime("DataPostareAnunt"),
                                reader.GetDecimal("Pret"),
                                reader.GetString("StareAnunt"),
                                reader.GetInt32("id_subcategorie")
                            );
                            Anunt.Add(detaliiAnunt);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                Imagini = new List<(int, int, string)>();
                using (var command = new MySqlCommand("SELECT * FROM imagini WHERE id_anunt=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var imagine =(
                            
                                reader.GetInt32("id_imagini"),
                                reader.GetInt32("id_anunt"),
                                reader.GetString("URLImagine")
                            );
                            Imagini.Add(imagine);
                        }
                    }
                }
                connection.Close();
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var numeExpeditor = Request.Form["nume"].ToString();
            var emailExpeditor = Request.Form["email"].ToString();
            var mesajExpeditor = Request.Form["mesaj"].ToString();

            using (var connection = new MySqlConnection("connection_string"))
            {
                var idUtilizatorDestinatar = 0;
                connection.Open();
                using (var command = new MySqlCommand("SELECT id_utilizator FROM Anunturi WHERE id_anunturi=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idUtilizatorDestinatar = reader.GetInt32("id_utilizator");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            return RedirectToPage("/DetaliiAnunt", new { id = id });
        }

        public IActionResult OnPostTrimiteMesaj(int idAnunt, string subiect, string continut)
        {
            idExpeditor = Int32.Parse(HttpContext.Request.Cookies["UserId"]);

            string queryIdUtilizator = "SELECT id_utilizator FROM Anunturi WHERE id_anunturi = @idAnunt";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                MySqlCommand cmdIdUtilizator = new MySqlCommand(queryIdUtilizator, connection);
                { 
                    cmdIdUtilizator.Parameters.AddWithValue("@idAnunt", idAnunt);

                    idDestinatar = Convert.ToInt32(cmdIdUtilizator.ExecuteScalar());
                }
                string queryInsertMesaj = "INSERT INTO Mesaje (id_utilizator_expeditor, id_utilizator_destinatar, id_anunt, DataMesaj, Subiect, Continut) " +
                    "VALUES (@idExpeditor, @idDestinatar, @idAnunt, NOW(), @subiect, @continut)";
                MySqlCommand cmdInsertMesaj = new MySqlCommand(queryInsertMesaj, connection);
                { 
                cmdInsertMesaj.Parameters.AddWithValue("@idExpeditor", idExpeditor);
                cmdInsertMesaj.Parameters.AddWithValue("@idDestinatar", idDestinatar);
                cmdInsertMesaj.Parameters.AddWithValue("@idAnunt", idAnunt);
                cmdInsertMesaj.Parameters.AddWithValue("@subiect", subiect);
                cmdInsertMesaj.Parameters.AddWithValue("@continut", continut);
                cmdInsertMesaj.ExecuteNonQuery();
                }
                return RedirectToPage();
            }
        }

    }
}
