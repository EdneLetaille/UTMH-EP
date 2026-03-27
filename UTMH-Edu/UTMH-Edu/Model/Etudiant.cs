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
    public class Etudiant
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        public int idOption { get; set; }
        public int idVille { get; set; }
        private string code;
        private string nom;
        private string photo;
        private string prenom;
        private string dateNaissance;
        private string sexe;
        private string departement;
        private string adresse;
        private string telephone;
        private string email;
        private string statut;
        private string personneRes;
        private string telephoneRes;
        private string dateInscription;      
        private string cin;
        private string statutPaiement;
        private string anneeAcademique;
        private string modePaiement;
        private string motPasse;
        private string role;

        public Etudiant(int idOption, string code, string nom, string prenom, string dateNaissance, string sexe, int idVille, string departement, string adresse, string telephone, string email, string statut, string personneRes, string telephoneRes, string dateInscription, string photo, string cin, string statutPaiement, string anneeAcademique, string modePaiement, string motPasse, string role)
        {
            this.idOption = idOption;
            this.code = code;
            this.nom = nom;
            this.prenom = prenom;
            this.dateNaissance = dateNaissance;
            this.sexe = sexe;
            this.idVille = idVille;
            this.departement = departement;
            this.adresse = adresse;
            this.telephone = telephone;
            this.email = email;
            this.statut = statut;
            this.personneRes = personneRes;
            this.telephoneRes = telephoneRes;
            this.dateInscription = dateInscription;
            this.photo = photo;
            this.cin = cin;
            this.statutPaiement = statutPaiement;
            this.anneeAcademique = anneeAcademique;
            this.modePaiement = modePaiement;
            this.motPasse = motPasse;
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



        public Etudiant() : this(0,null,  null, null, null, null, 0, null,  null, null,null, null, null, null, null, null, null, null, null, null, null, null) { }

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

        public string Departement
        {
            get { return this.departement; }
            set { this.departement = value; }
        }

        public string Adresse
        {
            get { return this.adresse; }
            set { this.adresse = value; }
        }

        public string Telephone
        {
            get { return this.telephone; }
            set { this.telephone = value; }
        }

        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        public string Statut
        {
            get { return this.statut; }
            set { this.statut = value; }
        }

        public string PersonneRes
        {
            get { return this.personneRes; }
            set { this.personneRes = value; }
        }

        public string TelephoneRes
        {
            get { return this.telephoneRes; }
            set { this.telephoneRes = value; }
        }

        public string DateInscription
        {
            get { return this.dateInscription; }
            set { this.dateInscription = value; }
        }
        public string Photo
        {
            get { return this.photo; }
            set { this.photo = value; }
        }



        public string Cin
        {
            get { return this.cin; }
            set { this.cin = value; }
        }
        public string StatutPaiement
        {
            get { return this.statutPaiement; }
            set { this.statutPaiement = value; }
        }


        public string AnneeAcademique
        {
            get { return this.anneeAcademique; }
            set { this.anneeAcademique = value; }
        }
        public string ModePaiement
        {
            get { return this.modePaiement; }
            set { this.modePaiement = value; }
        }
        public string MotPasse
        {
            get { return this.motPasse; }
            set { this.motPasse = value; }
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

            string query = "SELECT idEtudiant FROM etudiant WHERE email = @Email";

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
            // Si le CIN est vide ou seulement des espaces, on considère qu'il n'existe pas
            if (string.IsNullOrWhiteSpace(cin))
                return false;

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
                    return dr.HasRows; // TRUE = CIN déjà utilisé
                }
            }
        }




        // -----------------------------
        // debut / enregistrement etudiant
        // -----------------------------

        public void inscrireEtudiant()
        {




            string query = @"
    INSERT INTO etudiant 
    (idOption, code, nom, prenom, dateNaissance, sexe, idVille, departement, adresse,
    telephone, email, statut, personneRes, telephoneRes, dateInscription, photo,
    cin, statutPaiement, anneeAcademique,modePaiement, motPasse, role)
    VALUES
    (@idOption, @code, @nom, @prenom, @dateNaissance, @sexe, @idVille, @departement, 
    @adresse, @telephone, @email, @statut, @personneRes, @telephoneRes, 
    @dateInscription,@photo, @cin, @statutPaiement, @anneeAcademique,@modePaiement, @motPasse, @role)";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idOption", idOption);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);
                cmd.Parameters.AddWithValue("@dateNaissance", dateNaissance);
                cmd.Parameters.AddWithValue("@sexe", sexe);
                cmd.Parameters.AddWithValue("@idVille", idVille);
                cmd.Parameters.AddWithValue("@departement", departement);
                cmd.Parameters.AddWithValue("@adresse", adresse);
                cmd.Parameters.AddWithValue("@telephone", telephone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@statut", statut);
                cmd.Parameters.AddWithValue("@personneRes", personneRes);
                cmd.Parameters.AddWithValue("@telephoneRes", telephoneRes);
                cmd.Parameters.AddWithValue("@dateInscription", dateInscription);
                cmd.Parameters.AddWithValue("@photo", photo);
                cmd.Parameters.AddWithValue("@cin", cin);
                cmd.Parameters.AddWithValue("@statutPaiement", statutPaiement);
                cmd.Parameters.AddWithValue("@anneeAcademique", anneeAcademique);
                cmd.Parameters.AddWithValue("@modePaiement", modePaiement);
                cmd.Parameters.AddWithValue("@motPasse", motPasse);
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

        public static string supprimerEtudiant(string codeASupprimer)
        {
            string message = "";
            // preparer la requet
            string requete = "DELETE FROM etudiant WHERE code = @code";
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
        // debut / modifie mot de passe etudiant
        // -----------------------------

        public static void modifierMotPasseEtudiant(int idEtudiant, string motPasse)
        {
            string motPasseHash = HashPassword(motPasse);

            string query = @"UPDATE etudiant 
                     SET motPasse = @motPasse 
                     WHERE idEtudiant = @idEtudiant";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@motPasse", motPasseHash);
                cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);

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
        public bool verifierMotPasseEtudiant(int idEtudiant, string motPasse)
        {
            string motPasseHash = HashPassword(motPasse);

            string query = @"SELECT COUNT(*) 
                     FROM etudiant 
                     WHERE idEtudiant = @idEtudiant 
                     AND motPasse = @motPasse";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
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

        public int nombreEtudiant()
        {
            int total = 0;

            string chReq = "SELECT COUNT(*) FROM etudiant";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(chReq, con))
            {
                con.Open();
                total = (int)cmd.ExecuteScalar();
            }

            return total;
        }

        public DataTable GetEtudiantsParOption()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
            SELECT o.nom AS OptionNom,
                   COUNT(e.idEtudiant) AS TotalEtudiants
            FROM optionChoisie o
            LEFT JOIN etudiant e ON e.idOption = o.idOption
            GROUP BY o.nom";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable GetStatutEtudiants()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = @"
            SELECT statut, COUNT(*) AS total
            FROM etudiant
            GROUP BY statut";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            return dt;
        }





    }
}