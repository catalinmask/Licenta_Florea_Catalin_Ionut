using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
	public class RaspunsMesajModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=Licența;Uid=root;";
        private readonly string Query = "INSERT INTO Mesaje (id_utilizator_expeditor, " +
                                        "id_utilizator_destinatar, id_anunt, DataMesaj, Subiect, Continut) " +
                                        "VALUES (@idExpeditor, @idDestinatar, @idAnunt, NOW(), @subiect, @continut);";

        [BindProperty]
        public string Subiect { get; set; }
        [BindProperty]
        public string Continut { get; set; }
        public DateTime DataMesaj = DateTime.Now;
        [BindProperty(Name = "id", SupportsGet = true)]
        public int idDestinatar { get; set; }
        [BindProperty(Name = "id2", SupportsGet = true)]
        public int idAnunt { get; set; }


        public void OnGet()
        {
  

        }

        [HttpPost("{id}/{id2}")]
        public IActionResult OnPostTrimiteMesaj()
        {
            idDestinatar = Convert.ToInt32(Request.RouteValues["id"]);
            idAnunt = Convert.ToInt32(Request.RouteValues["id2"]);

            using (var connection = new MySqlConnection(_connectionString))
            {
                var idExpeditor = Int32.Parse(HttpContext.Request.Cookies["UserId"]);

                connection.Open();
                MySqlCommand cmd = new MySqlCommand(Query, connection);
                {
                    cmd.Parameters.AddWithValue("@id_utilizator_expeditor", idExpeditor);
                    cmd.Parameters.AddWithValue("@id_utilizator_destinatar", idDestinatar);
                    cmd.Parameters.AddWithValue("@id_anunt", idAnunt);
                    cmd.Parameters.AddWithValue("@DataMesaj", DataMesaj);
                    cmd.Parameters.AddWithValue("@Subiect", Subiect);
                    cmd.Parameters.AddWithValue("@Continut", Continut);
                    cmd.Parameters.AddWithValue("@idExpeditor", idExpeditor);
                    cmd.Parameters.AddWithValue("@idDestinatar", idDestinatar);
                    cmd.Parameters.AddWithValue("@idAnunt", idAnunt);
                    cmd.ExecuteNonQuery();
                }

            }
            return Page();
        }
    }
}
