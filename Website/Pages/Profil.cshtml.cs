using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
	public class ProfilModel : PageModel
    {
        [BindProperty]
        public string Nume { get; set; }
        [BindProperty]
        public string Prenume { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Parola { get; set; }
        [BindProperty]
        public string NumarTelefon { get; set; }
        [BindProperty]
        public DateTime DataNasterii { get; set; }
        [BindProperty]
        public string Adresa { get; set; }
        [BindProperty]
        public string Oras { get; set; }
        [BindProperty]
        public List<(string TitluAnunt, DateTime DataAnunt,int IdAnunt)> Anunturi { get; set; }

        private readonly string connectionString = "Server=localhost;Database=Licența;Uid=root;";
        

        public IActionResult OnGet(int Id)
        {

            using(var connection= new MySqlConnection(connectionString))
            {
                connection.Open();
                var utilizatorQuery= "SELECT Utilizatori.id_utilizator, Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email," +
                    " Utilizatori.NumarTelefon, Utilizatori.DataNasterii, Utilizatori.Adresa, Utilizatori.Parola, Orase.NumeOras AS Oras " +
                                     "FROM Utilizatori " +
                                     "JOIN Orase ON Utilizatori.id_oras = Orase.id_oras " +
                                     "WHERE Utilizatori.id_utilizator = @Id";
                using (var command = new MySqlCommand(utilizatorQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Nume = reader.GetString(1);
                            Prenume = reader.GetString(2);
                            Email = reader.GetString(3);
                            NumarTelefon = reader.GetString(4);
                            DataNasterii = reader.GetDateTime(5);
                            Adresa = reader.GetString(6);
                            Oras = reader.GetString(8);
                            Parola = reader.GetString(7);
                        }
                        else
                        {
                            return RedirectToPage("/Index");
                        }
                    }
                }
                var anunturiQuery = "SELECT TitluAnunt, DataPostareAnunt, id_anunturi " +
                                    "FROM Anunturi " +
                                    "WHERE id_utilizator = @Id " +
                                    "ORDER BY DataPostareAnunt DESC";

                Anunturi = new List<(string, DateTime,int)>();
                using (var command = new MySqlCommand(anunturiQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var anunt = (
                            reader.GetString(0),
                            reader.GetDateTime(1),
                            reader.GetInt32(2)
                            );
                            Anunturi.Add(anunt);
                        }
                    }
                }
            }
            return Page();
        }
        public IActionResult OnPostDelete([FromForm] string id)
        {
            var idAnunt = Convert.ToInt32(id);
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var queryDeleteAnunt = "DELETE FROM `Anunturi` WHERE id_anunturi=@idAnunt";

                var queryDeleteImg = "DELETE FROM `Imagini` WHERE id_anunt=@idAnunt";

                var queryDeleteMesaje = "DELETE FROM `Mesaje` WHERE id_anunt=@idAnunt";

                connection.Open();

                using (var command = new MySqlCommand(queryDeleteMesaje, connection))
                {
                    command.Parameters.AddWithValue("@idAnunt", idAnunt);
                    command.ExecuteNonQuery();
                }

                using (var command = new MySqlCommand(queryDeleteImg, connection))
                {
                    command.Parameters.AddWithValue("@idAnunt", idAnunt);
                    command.ExecuteNonQuery();
                }

                using (var command = new MySqlCommand(queryDeleteAnunt, connection))
                {
                    command.Parameters.AddWithValue("@idAnunt", idAnunt);
                    command.ExecuteNonQuery();
                }

            

            }
            return RedirectToAction("Index", "Profil");
        }
    }
}
