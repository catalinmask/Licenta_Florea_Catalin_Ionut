using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
	public class PaginaAnunturiModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=Licența;Uid=root;";

        [BindProperty]
        public List<(int IdAnunt, string TitluAnunt, string NumeProdus, decimal Pret, string URLImagine)> AnunturiPromovate { get; set; }


        public void OnGet(int id)
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);

            string queryAnunturi = "SELECT a.id_anunturi, a.TitluAnunt, a.NumeProdus, a.Pret,(SELECT i.URLImagine FROM imagini i WHERE i.id_anunt = a.id_anunturi LIMIT 1) AS URLImagine FROM anunturi a " +
                "JOIN Subcategorii s ON a.id_subcategorie = s.id_subcategorie " +
                "JOIN Categorii c ON s.id_categorie = c.id_categorie " +
                "WHERE c.id_categorie = @id;";

            connection.Open();

            MySqlCommand commandAnunturi = new MySqlCommand(queryAnunturi, connection);
            commandAnunturi.Parameters.AddWithValue("@id", id);

            MySqlDataReader readerAnunturiPromovate = commandAnunturi.ExecuteReader();

            AnunturiPromovate = new List<(int, string, string, decimal, string)>();
            while (readerAnunturiPromovate.Read())
            {
                var anunt = (
                    readerAnunturiPromovate.GetInt32("id_anunturi"),
                    readerAnunturiPromovate.GetString("TitluAnunt"),
                    readerAnunturiPromovate.GetString("NumeProdus"),
                    readerAnunturiPromovate.GetDecimal("Pret"),
                    readerAnunturiPromovate.GetString("URLImagine")
                );
                AnunturiPromovate.Add(anunt);
            }
            readerAnunturiPromovate.Close();
        }
    }
}
