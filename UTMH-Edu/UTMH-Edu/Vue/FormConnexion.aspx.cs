using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using System.Web.UI;
using UTMH_Edu.Model;
using System.Security.Cryptography;

namespace UTMH_Edu.Vue
{

    // Objet pour retourner le résultat de connexion
    public class LoginResult
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nom { get; set; }
        public string Role { get; set; }
        public string Prenom { get; set; }
        public string Code { get; set; }
        public string Sexe { get; set; }
        public string Telephone { get; set; }
        public string Option { get; set; }
        public string Date { get; set; }
        public string Annee { get; set; }
        public string MotPasse { get; set; }
        public string Photo { get; set; }
        public string Salaire { get; set; }
        public string DateEmbauche { get; set; }
        public string Statut { get; set; }
    }

    public partial class FormConnexion : System.Web.UI.Page
    {
        Log log = new Log();
        private static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
           

          

        }

        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }

        // Fonction de vérification de l’utilisateur
        private LoginResult verifierUtilisateur(string email, string motPasseHash)
        {
            string query = "";

            using (SqlConnection con = new SqlConnection(strCon))
            {
                con.Open();

                // ===== ETUDIANT =====
                query = @"SELECT idEtudiant, code, nom, prenom, email, sexe, telephone,
                         idOption, dateInscription, anneeAcademique, motPasse, photo, statut
                  FROM etudiant
                  WHERE email = @email AND motPasse = @motPasse";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@motPasse", System.Data.SqlDbType.VarChar).Value = motPasseHash;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new LoginResult
                            {
                                Id = Convert.ToInt32(dr["idEtudiant"]),
                                Nom = dr["nom"].ToString(),
                                Prenom = dr["prenom"].ToString(),
                                Email = dr["email"].ToString(),
                                Code = dr["code"].ToString(),
                                Sexe = dr["sexe"].ToString(),
                                Telephone = dr["telephone"].ToString(),
                                Option = dr["idOption"].ToString(),
                                Date = dr["dateInscription"].ToString(),
                                Annee = dr["anneeAcademique"].ToString(),
                                MotPasse = dr["motPasse"].ToString(),
                                Photo = dr["photo"].ToString(),
                                Statut = dr["statut"].ToString(),
                                Role = "Etudiant"
                            };
                        }
                    }
                }

                // ===== PROFESSEUR =====
                query = @"SELECT idProf, code, nom, prenom, email,dateEmbauche, salaire, motPasse,statut
                  FROM professeur
                  WHERE email = @email AND motPasse = @motPasse";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@motPasse", System.Data.SqlDbType.VarChar).Value = motPasseHash;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new LoginResult
                            {
                                Id = Convert.ToInt32(dr["idProf"]),
                                Nom = dr["nom"].ToString(),
                                Prenom = dr["prenom"].ToString(),
                                Email = dr["email"].ToString(),
                                Code = dr["code"].ToString(),
                                Salaire = dr["salaire"].ToString(),
                                DateEmbauche = dr["dateEmbauche"].ToString(),
                                Statut = dr["statut"].ToString(),
                                Role = "Professeur"
                            };
                        }
                    }
                }

                // ===== ADMIN / COMPTABLE =====
                query = @"SELECT idAdm, code, nom, prenom, email, telephone, motPasse,dateEmbauche, statut, role
                  FROM administrateur
                  WHERE email = @email AND motPasse = @motPasse";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = email;
                    cmd.Parameters.Add("@motPasse", System.Data.SqlDbType.VarChar).Value = motPasseHash;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new LoginResult
                            {
                                Id = Convert.ToInt32(dr["idAdm"]),
                                Nom = dr["nom"].ToString(),
                                Prenom = dr["prenom"].ToString(),
                                Email = dr["email"].ToString(),
                                Code = dr["code"].ToString(),
                                MotPasse = dr["motPasse"].ToString(),
                                DateEmbauche = dr["dateEmbauche"].ToString(),
                                Telephone = dr["telephone"].ToString(),
                                Statut = dr["statut"].ToString(),
                                Role = dr["role"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }


        public bool EmailValide(string email)
        {
            try
            {
                var adr = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }

        private string GetUserIP()
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ip))
            {
                string[] addresses = ip.Split(',');
                if (addresses.Length != 0)
                    return addresses[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        private string GetBrowser()
        {
            return Request.Browser.Browser + " " + Request.Browser.Version;
        }

        private string GetOS()
        {
            return Request.Browser.Platform;
        }

        private bool AppareilExiste(int userId, string ip, string navigateur, string os)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"SELECT COUNT(*) FROM AppareilUtilisateur 
                         WHERE userId=@id AND ip=@ip 
                         AND navigateur=@nav AND os=@os";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.Parameters.AddWithValue("@nav", navigateur);
                cmd.Parameters.AddWithValue("@os", os);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        private void EnregistrerAppareil(int userId, string ip, string navigateur, string os)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"INSERT INTO AppareilUtilisateur(userId, ip, navigateur, os)
                         VALUES(@id, @ip, @nav, @os)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.Parameters.AddWithValue("@nav", navigateur);
                cmd.Parameters.AddWithValue("@os", os);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void EnvoyerEmailAlerte(string email, string ip, string navigateur, string os)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ednerletaille19@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Nouvelle connexion détectée";

            mail.Body = $"Nouvelle connexion détectée :\n\nIP: {ip}\nNavigateur: {navigateur}\nOS: {os}";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //tsun qbzw zqlt ptfu
               // gldf tjax exrh rpum
            smtp.Credentials = new System.Net.NetworkCredential("ednerletaille19@gmail.com", "tsun qbzw zqlt ptfu");
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }


        protected void btnConnexion_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string motPasse = txtMotPasse.Text.Trim();

            if (email == "" || motPasse == "")
            {
                ShowAlert("Certains champs sont vides.");
                return;
            }

            string motPasseHash = HashPassword(motPasse);

            // Vérifier utilisateur
            LoginResult result = verifierUtilisateur(email, motPasseHash);
            if (result == null)
            {
                ShowAlert("Email ou mot de passe incorrect.");
                return;
            }

            if (result.Statut == null || result.Statut.ToLower() != "actif")
            {
                ShowAlert("Votre compte est inactif. Veuillez contacter l'administration.");
                return;
            }
            string ip = GetUserIP();
            string navigateur = GetBrowser();
            string os = GetOS();

            if (!AppareilExiste(result.Id, ip, navigateur, os))
            {
                // Nouveau appareil détecté
                EnvoyerEmailAlerte(result.Email, ip, navigateur, os);

                // Enregistrer appareil
                EnregistrerAppareil(result.Id, ip, navigateur, os);
            }

           

            //  Enregistrer les informations dans la session
            Session["userId"] = result.Id;         
            Session["email"] = result.Email;
            Session["role"] = result.Role;
            Session["nom"] = result.Nom;
            Session["prenom"] = result.Prenom;
            Session["code"] = result.Code;
            Session["sexe"] = result.Sexe;
            Session["telephone"] = result.Telephone;
            Session["idOption"] = result.Option;
            Session["dateInscription"] = result.Date;
            Session["anneeAcademique"] = result.Annee;
            Session["motPasse"] = result.MotPasse;
            Session["photo"] = result.Photo;
            Session["dateEmbauche"] = result.DateEmbauche;
            Session["salaire"] = result.Salaire;


                log.AjouterLog(
                    Session["code"].ToString(),
                    "Login.aspx",
                    "Connexion",
                    Session["role"].ToString()
                );
            



            //  Redirection selon rôle
            switch (result.Role)
            {
                case "Etudiant":
                    Response.Redirect("FormAccueilEtudiant.aspx");
                    break;

                case "Professeur":
                    Response.Redirect("FormAccueilProfesseur.aspx");
                    break;

                case "Comptable":
                    Response.Redirect("FormAccueilAdm.aspx");
                    break;

                case "Responsable":
                    Response.Redirect("FormAccueilAdm.aspx");
                    break;

                case "Secretaire":
                    Response.Redirect("FormAccueilAdm.aspx");
                    break;
                case "Administrateur":
                    Response.Redirect("FormAccueilAdm.aspx");
                    break;

                default:
                    ShowAlert("Rôle non reconnu.");
                    break;
            }

        }
    }
}
