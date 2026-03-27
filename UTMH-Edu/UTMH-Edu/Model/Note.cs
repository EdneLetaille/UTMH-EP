using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UTMH_Edu.Model
{
    public class Note
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        public int idCours { get; set; }
        public int idEtudiant { get; set; }
        public string noteObtenue { get; set; }
        public string dateNote { get; set; }
        public string typeNote { get; set; }


        public Note(int idCours,int idEtudiant,
            string noteObtenue, string dateNote, string typeNote)
        {
            this.idCours = idCours;
            this.idEtudiant = idEtudiant;
            this.noteObtenue = noteObtenue;
            this.dateNote = dateNote;
            this.typeNote = typeNote;
        }


        public Note(): this(0, 0, null, null, null) { }

     
        public void enregistrerNote()
        {
            string query = @" INSERT into note(idCours, idEtudiant, noteObtenue, dateNote, typeNote)
                           VALUES(@idCours,@idEtudiant,@noteObtenue, @dateNote, @typeNote)";
            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idCours",idCours);
                cmd.Parameters.AddWithValue("@idEtudiant", idEtudiant);
                cmd.Parameters.AddWithValue("@noteObtenue", noteObtenue);
                cmd.Parameters.AddWithValue("@dateNote", dateNote );
                cmd.Parameters.AddWithValue("@typeNote", typeNote);          

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

        public static bool SupprimerNote(int idNote)
        {
            string requete = "DELETE FROM note WHERE idNote = @idNote";

            using (SqlConnection con = new SqlConnection(strCon))
            using (SqlCommand cmd = new SqlCommand(requete, con))
            {
                cmd.Parameters.Add("@idNote", SqlDbType.Int).Value = idNote;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }




    }
    }