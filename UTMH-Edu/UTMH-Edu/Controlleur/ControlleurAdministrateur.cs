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
    public class ControlleurAdministrateur
    {
        private Administrateur Adm;
        public ControlleurAdministrateur()
        {
            Adm = new Administrateur();
        }
        public void enregistrerAdministrateur(string code, string nom, string prenom, string email, string dateNaissance, string sexe, string telephone,string motPasse, string statut, string cin, string role, string dateEmbauche)
        {
            this.Adm = new Administrateur(code, nom, prenom, email, dateNaissance, sexe, telephone, motPasse, statut,cin, role, dateEmbauche);
            Adm.enregistrerAdministrateur();
        }

        public void modifierMotPasseAdministrateur(int idAdm, string motPasse)
        {

            Administrateur.modifierMotPasseAdministrateur(idAdm, motPasse);
        }

        public bool verifierMotPasseAdministrateur(int idAdm, string motPasse)
        {

            return Adm.verifierMotPasseAdministrateur(idAdm, motPasse);
        }

        public bool emailExisteDansSysteme(string email)
        {
            return Administrateur.emailExisteDansSysteme(email);
        }
        public bool cinExisteDansSysteme(string cin)
        {
            return Administrateur.cinExisteDansSysteme(cin);
        }

        public string supprimerAdministrateur(string codeASupprimer)
        {
            return Administrateur.supprimerAdministrateur(codeASupprimer);
        }

        public string GetCode()
        {
            return Adm?.Code;
        }
    }
}

