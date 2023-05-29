using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
	public class HomePageModel : PageModel
    {
        [BindProperty]
        public List<(int IdAnunt, string TitluAnunt, string NumeProdus, decimal Pret, string URLImagine)> AnunturiPromovate { get; set; }
        [BindProperty]
        public List<(int IdCategorie, string Nume)> Categorii { get; set; }

        public void OnGet()
        {
            string connectionString = "Server=localhost;Database=Licența;Uid=root;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            connection.Open();

            string queryAnunturiPromovate = "SELECT a.id_anunturi, a.TitluAnunt, a.NumeProdus, a.Pret, " +
                "(SELECT i.URLImagine FROM imagini i WHERE i.id_anunt = a.id_anunturi LIMIT 1) AS URLImagine FROM anunturi a " +
                "WHERE a.Promovare = 1";

            MySqlCommand commandAnunturiPromovate = new MySqlCommand(queryAnunturiPromovate, connection);
            MySqlDataReader readerAnunturiPromovate = commandAnunturiPromovate.ExecuteReader();

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

            string queryCategorii = "SELECT * FROM categorii";
            MySqlCommand commandCategorii = new MySqlCommand(queryCategorii, connection);
            MySqlDataReader readerCategorii = commandCategorii.ExecuteReader();

            Categorii = new List<(int, string)>();
            while (readerCategorii.Read())
            {
                var categorie = (
                    readerCategorii.GetInt32("id_categorie"),
                    readerCategorii.GetString("Nume")
                );
                Categorii.Add(categorie);
            }
            readerCategorii.Close();

            connection.Close();
        }
    }
}

