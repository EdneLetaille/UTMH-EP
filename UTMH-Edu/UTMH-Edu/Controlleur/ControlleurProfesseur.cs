using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTMH_Edu.Model;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace UTMH_Edu.Controlleur
{
    public class ControlleurProfesseur
    {
        private Professeur Prof;
        public ControlleurProfesseur()
        {
            Prof = new Professeur();
        }

        public void enregistrerProfesseur(string code, string nom, string prenom,
            int idVille, string departement,
           string email,string motPasse, string cv, string statut,
            string dateEmbauche, string salaire,
            string cin, string role)
        {
            // Construire l’objet
            this.Prof = new Professeur(code, nom, prenom,  idVille, departement,
            email, motPasse, cv, statut,
           dateEmbauche, salaire, cin, role);

            // Enregistrer dans la base
            Prof.enregistrerProfesseur();

        }

        public void modifierMotPasseProfesseur(int idProf, string motPasse)
        {

            Professeur.modifierMotPasseProfesseur(idProf, motPasse);
        }
        public bool verifierMotPasseProfesseur(int idProf, string motPasse)
        {

            return Prof.verifierMotPasseProfesseur(idProf, motPasse);
        }


        public bool emailExisteDansSysteme(string email)
        {
            return Professeur.emailExisteDansSysteme(email);
        }
        public bool cinExisteDansSysteme(string cin)
        {
            return Professeur.cinExisteDansSysteme(cin);
        }

        public string supprimerProfesseur(string codeASupprimer)
        {
            return Professeur.supprimerProfesseur(codeASupprimer);
        }


        public string GetCode()
        {
            return Prof?.Code;
        }

    }
}