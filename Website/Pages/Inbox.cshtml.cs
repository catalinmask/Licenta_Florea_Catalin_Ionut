using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace Website.Pages
{
	public class InboxModel : PageModel
    {
        private readonly string connectionString = "Server=localhost;Database=Licența;Uid=root;";

        [BindProperty]
        public List<(string Nume,string Prenume, DateTime DataMesaj, string Subiect, string Mesaj,int idExpeditor, int idAnunt,int idMesaj)> Mesaje { get; set; }

        public int id { get; set; }

        public IActionResult OnGet()
        {
            Mesaje = new List<(string,string, DateTime, string, string,int,int,int)>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                id = Int32.Parse(HttpContext.Request.Cookies["UserId"]);
                connection.Open();
                var queryMesaje = "SELECT Utilizatori.Nume, Utilizatori.Prenume, DataMesaj, Subiect, Continut, id_utilizator_expeditor,id_anunt,id_mesaj FROM Mesaje " +
                                   "JOIN Utilizatori ON Utilizatori.id_utilizator = Mesaje.id_utilizator_expeditor " +
                                   "WHERE id_utilizator_destinatar = @id";

                using (var command = new MySqlCommand(queryMesaje, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            var detaliiInbox=(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetDateTime(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6),
                            reader.GetInt32(7)
                            );
                            Mesaje.Add(detaliiInbox);
                        }
                    }

                }
            }
            return Page();
        }
        public IActionResult OnPostDelete(int messageId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var queryDelete = "DELETE FROM `Mesaje` WHERE id_mesaj=@messageId";
                connection.Open();
                using (var command = new MySqlCommand(queryDelete, connection))
                {
                    command.Parameters.AddWithValue("@messageId", messageId);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index", "Inbox");
        }
    }
}
