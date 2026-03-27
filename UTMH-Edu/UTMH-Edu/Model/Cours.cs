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

namespace UTMH_Edu.Model
{
    public class Cours
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        public int idProf { get; set; }
        private string code;
        private string nom;
        private string description;
        private string heureDebut;
        private string heureFin;
        private string duree;
        private string statut;
        private string coefficient;

        public Cours( int idProf, string code, string nom, string description, string heureDebut, string heureFin, string duree, string statut, string coefficient)
        {
            this.idProf = idProf;
            this.code = code;
            this.nom = nom;
            this.description = description;
            this.heureDebut = heureDebut;
            this.heureFin = heureFin;
            this.duree = duree;
            this.statut = statut;
            this.coefficient = coefficient;
        }
        public Cours() : this(0, null, null, null, null, null, null, null, null) { }

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
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public string HeureDebut
        {
            get { return this.heureDebut; }
            set { this.heureDebut = value; }
        }
        public string HeureFin
        {
            get { return this.heureFin; }
            set { this.heureFin = value; }
        }
        public string Duree
        {
            get { return this.duree; }
            set { this.duree = value; }
        }
        public string Statut
        {
            get { return this.statut; }
            set { this.statut = value; }
        }
        public string Coefficient
        {
            get { return this.coefficient; }
            set { this.coefficient = value; }
        }

        public int creerCours(
            int idProf,
            string code,
            string nom,
            string description,
            string heureDebut,
            string heureFin,
            string duree,
            string statut,
            string coefficient)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO cours
            (idProf, code, nom, description, heureDebut, heureFin, duree, statut, coefficient)
            OUTPUT INSERTED.idCours
            VALUES
            (@idProf, @code, @nom, @description, @heureDebut, @heureFin, @duree, @statut, @coefficient)
        ", con);

                cmd.Parameters.AddWithValue("@idProf", idProf);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@heureDebut", heureDebut);
                cmd.Parameters.AddWithValue("@heureFin", heureFin);
                cmd.Parameters.AddWithValue("@duree", duree);
                cmd.Parameters.AddWithValue("@statut", statut);
                cmd.Parameters.AddWithValue("@coefficient", coefficient);

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());

            }
        }

        public static bool professeurDejaAssigne(int idProf)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                string query = "SELECT COUNT(*) FROM cours WHERE idProf = @idProf";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@idProf", idProf);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0; // true si le professeur a déjà un cours
            }
        }

        public void lierCoursOption(int idCours, int idOption)
        {
            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO OptionChoisieCours (idCours, idOption)
            VALUES (@idCours, @idOption)
        ", con);

                cmd.Parameters.AddWithValue("@idCours", idCours);
                cmd.Parameters.AddWithValue("@idOption", idOption);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public static string supprimerCours(string codeASupprimer)
        {
            string message = "";
            // preparer la requet
            string requete = "DELETE FROM cours WHERE code = @code";
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
        public static bool nomCoursExisteDansSysteme(string nom)
        {
            string query = @"
        SELECT 1 FROM cours 
        WHERE UPPER(nom) = UPPER(@nom)";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@nom", nom);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    return dr.HasRows; // TRUE = nom déjà utilisé
                }
            }
        }

        public int nombreCours()
        {
            int total = 0;

            string chReq = "SELECT COUNT(*) FROM cours";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(chReq, con))
            {
                con.Open();
                total = (int)cmd.ExecuteScalar();
            }

            return total;
        }

        public int nombreOption()
        {
            int total = 0;

            string chReq = "SELECT COUNT(*) FROM optionChoisie";

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