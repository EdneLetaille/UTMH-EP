using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using UTMH_Edu.Model;
using UTMH_Edu.ServiceTech;
using System.Security.Cryptography;

namespace UTMH_Edu.Model
{
    public class Administrateur
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;

        private string code;
        private string nom;
        private string prenom;
        private string email;
        private string dateNaissance;
        private string sexe;
        private string telephone;
        private string motPasse;
        private string statut;
        string cin;
        private string role;
        private string dateEmbauche;

        public Administrateur(string code, string nom, string prenom, string email, string dateNaissance,string sexe, string telephone, string motPasse, string statut, string cin, string role, string dateEmbauche)
        {
            this.code = code;
            this.nom = nom;
            this.prenom = prenom;
            this.email = email;
            this.dateNaissance = dateNaissance;
            this.sexe = sexe;
            this.telephone = telephone;
            this.motPasse = motPasse;
            this.cin = cin;
            this.statut = statut;
            this.role = role;
            this.dateEmbauche = dateEmbauche;

        }


        public Administrateur() : this( null,null, null,null, null, null, null, null, null, null, null, null) { }


        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        public string Prenom
        {
            get { return this.prenom; }
            set { this.prenom = value; }
        }
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }
        public string DateNaissance
        {
            get { return this.dateNaissance; }
            set { this.dateNaissance = value; }
        }
        public string Sexe
        {
            get { return this.sexe; }
            set { this.sexe = value; }
        }
        public string Telephone
        {
            get { return this.telephone; }
            set { this.telephone = value; }
        }

        public string MotPasse
        {
            get { return this.motPasse; }
            set { this.motPasse = value; }
        }
        public string Statut
        {
            get { return this.statut; }
            set { this.statut = value; }
        }
        public string Cin
        {
            get { return this.cin; }
            set { this.cin = value; }
        }
        public string Role
        {
            get { return this.role; }
            set { this.role = value; }
        }
        public string DateEmbauche
        {
            get { return this.dateEmbauche; }
            set { this.dateEmbauche = value; }
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }
        public static int GetIdByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return 0;

            string query = "SELECT idAdm FROM administrateur WHERE email = @Email";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                object result = cmd.ExecuteScalar();

                return (result != null) ? Convert.ToInt32(result) : 0;
            }
        }

        public void enregistrerAdministrateur()
        {
            string query = @"Insert into administrateur(code,nom,prenom,email,dateNaissance, sexe, telephone,motPasse,statut,cin,role,dateEmbauche) 
                            values(@code,@nom,@prenom,@email,@dateNaissance,@sexe,@telephone,@motPasse,@statut,@cin,@role,@dateEmbauche)";
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@dateNaissance", dateNaissance);
                cmd.Parameters.AddWithValue("@sexe", sexe);
                cmd.Parameters.AddWithValue("@telephone", telephone);
                cmd.Parameters.AddWithValue("@motPasse", motPasse);
                cmd.Parameters.AddWithValue("@statut", statut);
                cmd.Parameters.AddWithValue("@cin", cin);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@dateEmbauche", dateEmbauche);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur SQL : " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

        }

        public static string supprimerAdministrateur(string codeASupprimer)
        {
            string message = "";
            // preparer la requet
            string requete = "DELETE FROM administrateur WHERE code = @code";
            try
            {
                SqlConnection con = new SqlConnection(strCon);
                //Ouvrire la connexion
                con.Open();
                //Creation de la commande
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.Parameters.AddWithValue("@code", codeASupprimer);
                //Execution de la requete
                if (cmd.ExecuteNonQuery() != 0)
                {
                    message = "Suppresion reussir";
                }
            }
            catch (SqlException )
            {
                message = "Suppresion non reussir";
            }
            return message;

        }

        // -----------------------------
        // debut / modifie mot de passe administrateur
        // -----------------------------

        public static void modifierMotPasseAdministrateur(int idAdm, string motPasse)
        {
            string motPasseHash = HashPassword(motPasse);

            string query = @"UPDATE administrateur 
                     SET motPasse = @motPasse 
                     WHERE idAdm = @idAdm";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@motPasse", motPasseHash);
                cmd.Parameters.AddWithValue("@idAdm", idAdm);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur SQL : " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public static bool emailExisteDansSysteme(string email)
        {
            string query = @"
        SELECT 1 FROM etudiant WHERE email = @email
        UNION
        SELECT 1 FROM professeur WHERE email = @email
        UNION
        SELECT 1 FROM administrateur WHERE email = @email";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@email", email.Trim().ToLower());
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    return dr.HasRows; // TRUE = email déjà utilisé
                }
            }
        }
        public static bool cinExisteDansSysteme(string cin)
        {
            string query = @"
        SELECT 1 FROM etudiant WHERE cin = @cin
        UNION
        SELECT 1 FROM professeur WHERE cin = @cin
        UNION
        SELECT 1 FROM administrateur WHERE cin = @cin";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@cin", cin.Trim());
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    return dr.HasRows; // TRUE = email déjà utilisé
                }
            }
        }

        // méthode pour vérifier l'ancien mot de passe
        public bool verifierMotPasseAdministrateur(int idAdm, string motPasse)
        {
            string motPasseHash = HashPassword(motPasse);

            string query = @"SELECT COUNT(*) 
                     FROM administrateur 
                     WHERE idAdm = @idAdm
                     AND motPasse = @motPasse";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idAdm", idAdm);
                cmd.Parameters.AddWithValue("@motPasse", motPasseHash);

                try
                {
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur SQL : " + ex.Message);
                }
            }
        }
    }
}