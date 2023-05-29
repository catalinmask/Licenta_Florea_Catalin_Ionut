using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
	public class RapoarteModel : PageModel
    {
        private readonly string connectionString = "Server=localhost;Database=Licența;Uid=root;";

        [BindProperty]
        public List<Dictionary<string, object>>? rezultate { get; set; }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            var query = "SELECT * from Utilizatori";

            var variable = "";

            var where = " ";

            var AnunturiVanduteCheck = Request.Form["Vandute"];
            var AnunturiNevanduteCheck = Request.Form["Nevandute"];
            var CategoriiCheck = Request.Form["Categorii"];
            var SubcategoriiCheck = Request.Form["Subcategorii"];
            var OraseCheck = Request.Form["Orase"];
            var PromovariCheck = Request.Form["Promovari"];
            var ListaCheck = Request.Form["Lista Neagra"];

            if (!string.IsNullOrEmpty(AnunturiVanduteCheck))
            {
                query += " INNER JOIN Anunturi ON Anunturi.id_utilizator = Utilizatori.id_utilizator";
                where += " WHERE Anunturi.StareAnunt='inactiv'";
                variable += "Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email, Utilizatori.NumarTelefon, Utilizatori.Adresa," +
                    " Utilizatori.DataNasterii, Anunturi.TitluAnunt, Anunturi.NumeProdus, " +
                    "Anunturi.DataPostareAnunt, Anunturi.Pret, Anunturi.StareAnunt, Anunturi.Promovare";
            }

            if (!string.IsNullOrEmpty(AnunturiNevanduteCheck) && !string.IsNullOrEmpty(AnunturiVanduteCheck))
            {
                query += " OR Anunturi.StareAnunt = 'activ'";
            }

            if(!string.IsNullOrEmpty(AnunturiNevanduteCheck) && string.IsNullOrEmpty(AnunturiVanduteCheck))
            {
                query += " INNER JOIN Anunturi ON Anunturi.id_utilizator = Utilizatori.id_utilizator";
                where += " WHERE Anunturi.StareAnunt='activ'";
                variable += " Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email, Utilizatori.NumarTelefon, Utilizatori.Adresa," +
                            " Utilizatori.DataNasterii, Anunturi.TitluAnunt, Anunturi.NumeProdus, " +
                            " Anunturi.DataPostareAnunt, Anunturi.Pret, Anunturi.StareAnunt, Anunturi.Promovare";
            }

            if (!string.IsNullOrEmpty(CategoriiCheck) && string.IsNullOrEmpty(AnunturiNevanduteCheck) && string.IsNullOrEmpty(AnunturiVanduteCheck))
            {
                query += " INNER JOIN Anunturi ON Anunturi.id_utilizator=Utilizatori.id_utilizator" +
                    " INNER JOIN Subcategorii ON Subcategorii.id_subcategorie=Anunturi.id_subcategorie" +
                    " INNER JOIN Categorii ON Categorii.id_categorie = Subcategorii.id_categorie";

                variable += "Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email, Utilizatori.NumarTelefon, Utilizatori.Adresa," +
                                " Utilizatori.DataNasterii, Categorii.Nume AS NumeCategorie";
            }
            if (!string.IsNullOrEmpty(CategoriiCheck) && !string.IsNullOrEmpty(AnunturiNevanduteCheck) && !string.IsNullOrEmpty(AnunturiVanduteCheck))
            {
                query +=" INNER JOIN Subcategorii ON Subcategorii.id_subcategorie=Anunturi.id_subcategorie" +
                    " INNER JOIN Categorii ON Categorii.id_categorie = Subcategorii.id_categorie";

                variable += "Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email, Utilizatori.NumarTelefon, Utilizatori.Adresa," +
                                " Utilizatori.DataNasterii, Categorii.Nume AS NumeCategorie,";
            }


            if (!string.IsNullOrEmpty(SubcategoriiCheck))
            {
                query += " INNER JOIN Subcategorii ON Subcategorii.id_subcategorie=Anunturi.id_subcategorie";
            }

            if (!string.IsNullOrEmpty(OraseCheck))
            {
                query += " INNER JOIN Orase ON Orase.id_Oras = Utilizatori.id_oras";
                variable += "Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email, Utilizatori.NumarTelefon, Utilizatori.Adresa," +
                                  " Utilizatori.DataNasterii, Orase.NumeOras ";
            }

            if (!string.IsNullOrEmpty(PromovariCheck) && string.IsNullOrEmpty(AnunturiVanduteCheck) && string.IsNullOrEmpty(CategoriiCheck))
            {
                query += " INNER JOIN Anunturi ON Anunturi.id_utilizator = Utilizatori.id_utilizator" +
                         " INNER JOIN Promovari ON Promovari.id_anunt = Anunturi.id_anunturi";

                variable += "Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email, Utilizatori.NumarTelefon, Utilizatori.Adresa," +
                                  " Utilizatori.DataNasterii, Promovari.TipPromovare, Promovari.Durata, Promovari.Suma ";
            }
            if (!string.IsNullOrEmpty(PromovariCheck) && !string.IsNullOrEmpty(AnunturiVanduteCheck) && !string.IsNullOrEmpty(CategoriiCheck))
            {
                query += " INNER JOIN Promovari ON Promovari.id_anunt = Anunturi.id_anunturi";

                variable += "Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email, Utilizatori.NumarTelefon, Utilizatori.Adresa," +
                                  " Utilizatori.DataNasterii, Promovari.TipPromovare, Promovari.Durata, Promovari.Suma ";
            }

                if (!string.IsNullOrEmpty(ListaCheck))
            {
                query += " INNER JOIN ListaNeagra ON ListaNeagra.id_utilizator = Utilizatori.id_utilizator";
                variable += "Utilizatori.Nume, Utilizatori.Prenume, Utilizatori.Email, Utilizatori.NumarTelefon, Utilizatori.Adresa," +
                                  " Utilizatori.DataNasterii,ListaNeagra.Motiv  ";
            }

            List<Dictionary<string, object>> rezultate = new List<Dictionary<string, object>>();

            if(variable.Length>2)
            {
                string[] words = variable.Split(' ');
                string[] distinct = words.Distinct().ToArray();
                string final = string.Join(" ", distinct);

                query = query.Contains("*") ? query.Replace("*", final) : query;
            }
            query = query + where;
            MySqlConnection connection = new MySqlConnection(connectionString);
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    {
                        while(reader.Read())
                        {
                            Dictionary<string, object> rezultat = new Dictionary<string, object>();

                            for (int i=0;i<reader.FieldCount;i++)
                            {
                                string coloana = reader.GetName(i);
                                object valoare = reader.GetValue(i);
                                rezultat[coloana] = valoare;
                            }
                            rezultate.Add(rezultat);

                        }
                        reader.Close();
                    }

                }
                
            }
            connection.Close();
            TempData["rezultate"] = JsonSerializer.Serialize(rezultate);

            return RedirectToPage();
        }
    }
}
