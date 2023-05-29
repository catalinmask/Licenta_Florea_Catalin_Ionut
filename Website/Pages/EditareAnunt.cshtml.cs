using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
	public class EditareAnuntModel : PageModel
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

        public int idAnunt { get; set; }

        public EditareAnuntModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task OnGetAsync(int id)
        {
            idAnunt = id;

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

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                var Query = "select id_utilizator, TitluAnunt, NumeProdus, Pret from Anunturi where id_anunturi=@idAnunt";

                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@idAnunt", idAnunt);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idUtilizator = reader.GetInt32(0);
                            TitluAnunt = reader.GetString(1);
                            NumeProdus = reader.GetString(2);
                            Pret = reader.GetInt32(3);
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


        public async Task<IActionResult> OnPostAsync(int id)
        {
            idAnunt = id;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.Parameters.AddWithValue("@idAnunt", idAnunt);
                    command.CommandText = "UPDATE Anunturi SET TitluAnunt = @TitluAnunt,NumeProdus = @NumeProdus, Pret = @Pret, id_subcategorie = @IdSubcategorii WHERE id_anunturi=@idAnunt";
                    command.Parameters.AddWithValue("@TitluAnunt", TitluAnunt);
                    command.Parameters.AddWithValue("@NumeProdus", NumeProdus);
                    command.Parameters.AddWithValue("@Pret", Pret);
                    command.Parameters.AddWithValue("@IdSubcategorii", IdSubcategorii);
                    await command.ExecuteNonQueryAsync();
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
                            command.Parameters.Clear();
                            command.CommandText = "UPDATE Imagini SET URLImagine = @URLImagine WHERE id_anunt = @idAnunt;";
                            command.Parameters.AddWithValue("@URLImagine", URLImagine);
                            command.Parameters.AddWithValue("@idAnunt", idAnunt);
                            await command.ExecuteNonQueryAsync();
                            connection.Close();
                        }
                    }

                }

                return RedirectToPage("Index");
            }
        }
       
    }
}
