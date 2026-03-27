using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class FormAccueilAdm : System.Web.UI.Page
    {
        public int totalEtudiants;
        public int totalProfesseurs;
        public int totalCours;
        public int totalOptions;

        public string labelsOptions;
        public string dataOptions;

        protected int etuActifs = 0;
        protected int etuInactifs = 0;
        protected int etuEnAttente = 0;
         
        Log log = new Log();
        Etudiant etu = new Etudiant();
        Professeur prof = new Professeur();
        Cours cr = new Cours();

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoServerCaching();

            if (!IsPostBack)
            {
                //lbTotalEtu.Text = etu.nombreEtudiant().ToString();
                //lbTotalPro.Text = prof.nombreProfesseur().ToString();
                //lbTotalCours.Text = cr.nombreCours().ToString();
                //lbTotalFiliere.Text = cr.nombreOption().ToString();

                totalEtudiants = etu.nombreEtudiant();
                totalProfesseurs = prof.nombreProfesseur();
                totalCours = cr.nombreCours();
                totalOptions = cr.nombreOption();

                ChargerGrapheStatut();

                // 🔥 IMPORTANT : charger le graphe
                ChargerGrapheOptions();

                if (Session["userId"] == null)
                {
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                    Response.Redirect("Default.aspx");

                }

                log.AjouterLog(
                   Session["code"].ToString(),
                   "FormAccueilAdm.aspx",
                   "Navigation sur la page ADM",
                   Session["role"].ToString()
               );

            }
        }

        private void ChargerGrapheOptions()
        {
            DataTable dt = etu.GetEtudiantsParOption();

            List<string> labels = new List<string>();
            List<int> valeurs = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                labels.Add(row["OptionNom"].ToString());
                valeurs.Add(Convert.ToInt32(row["TotalEtudiants"]));
            }

            labelsOptions = "['" + string.Join("','", labels) + "']";
            dataOptions = "[" + string.Join(",", valeurs) + "]";
        }



        private void ChargerGrapheStatut()
        {
            DataTable dt = etu.GetStatutEtudiants();

            foreach (DataRow row in dt.Rows)
            {
                string statut = row["statut"].ToString();
                int total = Convert.ToInt32(row["total"]);

                if (statut == "Actif") etuActifs = total;
                else if (statut == "Inactif") etuInactifs = total;
                else if (statut == "En attente") etuEnAttente = total;
            }
        }

    }
}
