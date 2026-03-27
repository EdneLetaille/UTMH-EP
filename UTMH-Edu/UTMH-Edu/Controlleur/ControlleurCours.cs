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
    public class ControlleurCours
    {
        private Cours Cour;

        public ControlleurCours()
        {
            Cour = new Cours();
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
            return Cour.creerCours(
                idProf, code, nom, description,
                heureDebut, heureFin, duree, statut, coefficient
            );
        }

        public void lierCoursOption(int idCours, int idOption)
        {
            Cour.lierCoursOption(idCours, idOption);
        }

        public   bool professeurDejaAssigne(int idProf)
        {
            return Cours.professeurDejaAssigne(idProf);
        }

        public string supprimerCours(string codeASupprimer)
        {
            return Cours.supprimerCours(codeASupprimer);
        }
        public bool nomCoursExisteDansSysteme(string nom)
        {
            return Cours.nomCoursExisteDansSysteme(nom);
        }

        public string GetCode()
        {
            return Cour?.Code;
        }
    }
}
