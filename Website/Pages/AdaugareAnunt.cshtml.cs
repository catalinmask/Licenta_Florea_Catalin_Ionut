using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
    public class AdaugareAnuntModel : PageModel
    {
        private readonly string _connectionString = "Server=localhost;Database=Licența;Uid=root;";

        [BindProperty]
        public string TitluAnunt { get; set; }

        [BindProperty]
        public string NumeProdus { get; set; }

        public DateTime DataPostareAnunt = DateTime.Now;

        [BindProperty]
        public decimal Pret { get; set; }

        [BindProperty]
        public string StareAnunt { get; set; }

        [BindProperty]
        public string Promovare { get; set; }

        public int idPromovare { get; set; }

        public int IdSubcategorii = 1;

        [BindProperty]
        public List<IFormFile> URLImagini { get; set; }

        public int idUtilizator { get; set; }

        [BindProperty]
        public List<string> Subcategorii { get; set; }

        [BindProperty]
        public List<string> Categorii { get; set; }

        [BindProperty]
        public string CategorieSelectata { get; set; }

        public int idCategorie { get; set; }

        private readonly IWebHostEnvironment _environment;
        public long idAnunt { get; set; }

        public AdaugareAnuntModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        public async Task OnGetAsync()
        {
            Subcategorii = new List<string>();
            Categorii = new List<string>();
            string queryCategorii = "SELECT Nume from Categorii";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new MySqlCommand(queryCategorii, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Categorii.Add(reader.GetString("Nume"));
                        }
                    }
                }
            }
        }

        public void OnPostSubcategorii(string CategorieSelectata)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select id_categorie from Categorii where Nume=@CategorieSelectata";
                    command.Parameters.AddWithValue("@CategorieSelectata", CategorieSelectata);
                    var _CategorieId = command.ExecuteScalar();
                    if (_CategorieId != null)
                    {
                        idCategorie = Convert.ToInt32(_CategorieId);
                    }

                    command.CommandText = "SELECT Nume FROM Subcategorii where id_categorie=@idCategorie";
                    command.Parameters.AddWithValue("@idCategorie", idCategorie);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Subcategorii.Add(reader.GetString("Nume"));
                        }
                    }
                }
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(Request.Cookies.ContainsKey("userEmail")))
            {
                return Unauthorized();

            }

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {

                    var emailUtilizator = Request.Cookies["userEmail"];
                    command.CommandText = "select id_utilizator from Utilizatori WHERE Email=@emailUtilizator";
                    command.Parameters.AddWithValue("@emailUtilizator", emailUtilizator);
                    var _UtilizatorId = command.ExecuteScalar();

                    if (_UtilizatorId != null)
                    {
                        idUtilizator = Convert.ToInt32(_UtilizatorId);
                    }

                    if (Promovare == "Da")
                    {
                        idPromovare = 1;
                    }
                    command.CommandText = "INSERT INTO Anunturi (id_utilizator, TitluAnunt, NumeProdus, DataPostareAnunt, Pret, StareAnunt, Promovare, id_subcategorie) VALUES (@IdUtilizator, @TitluAnunt, @NumeProdus, @DataPostareAnunt, @Pret, 'Activ', @idPromovare, @IdSubcategorii); SELECT LAST_INSERT_ID();";
                    command.Parameters.AddWithValue("@IdUtilizator", idUtilizator);
                    command.Parameters.AddWithValue("@TitluAnunt", TitluAnunt);
                    command.Parameters.AddWithValue("@NumeProdus", NumeProdus);
                    command.Parameters.AddWithValue("@DataPostareAnunt", DataPostareAnunt);
                    command.Parameters.AddWithValue("@Pret", Pret);
                    command.Parameters.AddWithValue("@StareAnunt", "Activ");
                    command.Parameters.AddWithValue("@idPromovare", idPromovare);
                    command.Parameters.AddWithValue("@IdSubcategorii", IdSubcategorii);
                    await command.ExecuteNonQueryAsync();
                    idAnunt = (long)command.LastInsertedId;
                    connection.Close();
                }
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    foreach (var image in URLImagini)
                    {
                        if (image.Length > 0)
                        {
                            var fileName = Path.GetFileName(image.FileName);
                            var index = _environment.WebRootPath.IndexOf("/Licenta/Website/Website/wwwroot");
                            var URLImagine = Path.Combine(_environment.WebRootPath.Remove(index), fileName);
                            var newFilePath = Path.Combine("/Users/catalinflorea/Desktop/Licenta/Website/Website/wwwroot/images/", fileName);
                            System.IO.File.Move(URLImagine, newFilePath);
                            URLImagine = newFilePath;
                            command.CommandText = "INSERT INTO Imagini (id_anunt, URLImagine) VALUES (@IdAnunt, @URLImagine);";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@IdAnunt", idAnunt);
                            command.Parameters.AddWithValue("@URLImagine", URLImagine);
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                }
                return RedirectToPage("HomePage");
            }
        }
    }
}
