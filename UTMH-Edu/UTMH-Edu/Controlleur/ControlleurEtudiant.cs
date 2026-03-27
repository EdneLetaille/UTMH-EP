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
    public class ControlleurEtudiant
    {
        private Etudiant Etu;

        public ControlleurEtudiant()
        {
            Etu = new Etudiant();
        }

        public void inscrireEtudiant(
            int idOption,
            string code,
            string nom,
            string prenom,
            string dateNaissance,
            string sexe,
            int idVille,
            string departement,
            string adresse,
            string telephone,
            string email,
            string statut,
            string personneRes,
            string telephoneRes,
            string dateInscription,
            string photo,
            string cin,
            string statutPaiement,
            string anneeAcademique,
            string modePaiement,
            string motPasse,
            string role)
        {
            // Construire l’objet étudiant
            this.Etu = new Etudiant(
                idOption, code, nom, prenom, dateNaissance, sexe, idVille, departement,
                adresse, telephone, email, statut, personneRes, telephoneRes,
                dateInscription, photo, cin, statutPaiement, anneeAcademique,
                modePaiement, motPasse, role
            );

            // Appel de la méthode d’insertion (NOTE : plus de mot de passe ici)
            Etu.inscrireEtudiant();
        }



        public void modifierMotPasseEtudiant(int idEtudiant, string motPasse)
        {

            Etudiant.modifierMotPasseEtudiant(idEtudiant, motPasse);
        }
        public bool verifierMotPasseEtudiant(int idEtudiant, string motPasse)
        {

            return Etu.verifierMotPasseEtudiant(idEtudiant, motPasse);
        }


        public bool emailExisteDansSysteme(string email)
        {
            return Etudiant.emailExisteDansSysteme(email);
        }
        public bool cinExisteDansSysteme(string cin)
        {
            return Etudiant.cinExisteDansSysteme(cin);
        }

        public string supprimerEtudiant(string codeASupprimer)
        {
            return Etudiant.supprimerEtudiant(codeASupprimer);
        }

        public int nombreEtudiant()
        {
            return Etu.nombreEtudiant();
        }

        public string GetCode()
        {
            return Etu?.Code;
        }
    }
}
