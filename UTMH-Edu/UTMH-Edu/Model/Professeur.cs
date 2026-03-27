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

    public class Professeur
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        private string code;
        private string nom;
        private string prenom; 
        private int idVille { get; set; }
        private string departement;
        private string email;    
        private string motPasse;
        private string cv;
        private string statut;
        private string dateEmbauche;
        private string salaire;
        private string cin;
        private string role;


        public Professeur(string code, string nom, string prenom,
            int idVille, string departement,
             string email, string motPasse,
             string cv, string statut,
            string dateEmbauche, string salaire,
            string cin, string role)
        {
            this.code = code;
            this.nom = nom;
            this.prenom = prenom;
            this.idVille = idVille;
            this.departement = departement;
            this.email = email;
            this.motPasse = motPasse;
            this.cv = cv;
            this.statut = statut;
            this.dateEmbauche = dateEmbauche;
            this.salaire = salaire;
            this.cin = cin;
            this.role = role;
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }

        public Professeur() : this( null, null, null, 0, null, null, null, null,null, null, null, null, null) { }


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
            get { return this.Prenom; }
            set { this.prenom = value; }
        }
      
        public string Departement
        {
            get { return this.departement; }
            set { this.departement = value; }
        }

        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        public string MotPasse
        {
            get { return this.motPasse; }
            set { this.motPasse = value; }
        }
        public string Cv
        {
            get { return this.cv; }
            set { this.cv = value; }
        }
        public string Statut
        {
            get { return this.statut; }
            set { this.statut = value; }
        }
      

        public string DateEmbauche
        {
            get { return this.dateEmbauche; }
            set { this.dateEmbauche = value; }
        }
        public string Salaire
        {
            get { return this.salaire; }
            set { this.salaire = value; }
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

        public static int GetIdByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return 0;

            string query = "SELECT idProf FROM professeur WHERE email = @Email";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                object result = cmd.ExecuteScalar();

                return (result != null) ? Convert.ToInt32(result) : 0;
            }
        }

        public static bool emailExisteDansSysteme(string email)
        {
            string query = @"
        SELECT 1 FROM professeur WHERE email = @email
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
        SELECT 1 FROM professeur WHERE cin = @cin
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

        public void enregistrerProfesseur()
        {
            string query = @"
    INSERT INTO professeur
    ( code, nom,prenom, idVille, departement,
    email,motPasse, cv, statut,
     dateEmbauche, salaire,  cin, role)
    VALUES
    ( @code, @nom,@prenom, @idVille, @departement,
      @email, @motPasse, @cv, @statut,
     @dateEmbauche, @salaire,  @cin, @role)
     ";
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@idVille", idVille);
                cmd.Parameters.AddWithValue("@departement", departement);             
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@motPasse", motPasse);
                cmd.Parameters.AddWithValue("@cv", cv);
                cmd.Parameters.AddWithValue("@statut", statut);
                cmd.Parameters.AddWithValue("@dateEmbauche", dateEmbauche);
                cmd.Parameters.AddWithValue("@salaire", salaire);
                cmd.Parameters.AddWithValue("@cin", cin);
                cmd.Parameters.AddWithValue("@role", role);

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

        public static string supprimerProfesseur(string codeASupprimer)
        {
            string message = "";
            // preparer la requet
            string requete = "DELETE FROM professeur WHERE code = @code";
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
            catch (SqlException)
            {
                message = "Suppresion non reussir";
            }
            return message;

        }

        // debut / modifie mot de passe professeur
        // -----------------------------

        public static void modifierMotPasseProfesseur(int idProf, string motPasse)
        {
            string motPasseHash = HashPassword(motPasse);

            string query = @"UPDATE professeur 
                     SET motPasse = @motPasse 
                     WHERE idProf = @idProf";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@motPasse", motPasseHash);
                cmd.Parameters.AddWithValue("@idProf", idProf);

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

        // méthode pour vérifier l'ancien mot de passe
        public bool verifierMotPasseProfesseur(int idProf, string motPasse)
        {
            string motPasseHash = HashPassword(motPasse);

            string query = @"SELECT COUNT(*) 
                     FROM professeur 
                     WHERE idProf = @idProf
                     AND motPasse = @motPasse";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idProf", idProf);
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

        public int nombreProfesseur()
        {
            int total = 0;

            string chReq = "SELECT COUNT(*) FROM professeur";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(chReq, con))
            {
                con.Open();
                total = (int)cmd.ExecuteScalar();
            }

            return total;
        }
    }
}