using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Website.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string Nume { get; set; }

        [BindProperty]
        public string Prenume { get; set; }

        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Parola { get; set; }

        [BindProperty]
        public string NumarTelefon { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime DataNasterii { get; set; }

        [BindProperty]
        public string Adresa { get; set; }

        [BindProperty]
        public string Oras { get; set; }

        [BindProperty]
        public int Inregistrari { get; set; }
        private readonly string connectionString = "Server=localhost;Database=Licența;Uid=root;";

        public int GetOrasId(string Oras)
        {
            int id_oras = 0;
            string query = "SELECT id_oras FROM Orase WHERE NumeOras = @Oras";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Oras", Oras);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                id_oras = reader.GetInt32(0);
            }
            reader.Close();
            return id_oras;
        }

        public IActionResult OnPost()
        {
            Inregistrari = 0;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string connectionString = "Server=localhost;Database=Licența;Uid=root;";
            string QueryVerificare = "SELECT COUNT(*) FROM utilizatori WHERE Email=@Email OR Parola=@Parola;";


            string query = "INSERT INTO utilizatori (Nume, Prenume, Email, Parola, NumarTelefon, DataNasterii, Adresa, id_oras) " +
                "VALUES (@Nume, @Prenume, @Email, @Parola, @NumarTelefon, @DataNasterii, @Adresa, @id_oras)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(QueryVerificare, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Parola", Parola);


                    Inregistrari = Convert.ToInt32(command.ExecuteScalar());
                }

                if(Inregistrari == 0)
                { 
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    int id_oras = GetOrasId(Oras);

                    if (id_oras == 0)
                    {
                        string queryOras = "INSERT INTO Orase (NumeOras) VALUES (@Oras)";
                        MySqlCommand cmd = new MySqlCommand(queryOras, connection);
                        cmd.Parameters.AddWithValue("@Oras", Oras);
                        cmd.ExecuteNonQuery();

                        id_oras = (int)cmd.LastInsertedId;
                    }

                    command.Parameters.AddWithValue("@Nume", Nume);
                    command.Parameters.AddWithValue("@Prenume", Prenume);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Parola", Parola);
                    command.Parameters.AddWithValue("@NumarTelefon", NumarTelefon);
                    command.Parameters.AddWithValue("@DataNasterii", DataNasterii);
                    command.Parameters.AddWithValue("@Adresa", Adresa);
                    command.Parameters.AddWithValue("@id_oras", id_oras);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        return Page();
                    }
                }
                }
                else
                {
                    return Page();
                }
            }
        }
    }
}
