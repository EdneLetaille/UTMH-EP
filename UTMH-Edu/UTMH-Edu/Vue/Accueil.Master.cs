using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class Accueil : System.Web.UI.MasterPage
    {
        Etudiant etu = new Etudiant();
        Professeur prof = new Professeur();
        Cours cr = new Cours();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null)
            {
                Response.Redirect("FormDeconnexion.aspx");
                return;
            }

            lbTotalEtu.Text = etu.nombreEtudiant().ToString();
            lbTotalPro.Text = prof.nombreProfesseur().ToString();
            lbTotalCours.Text = cr.nombreCours().ToString();
            lbTotalFiliere.Text = cr.nombreOption().ToString();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoServerCaching();
            if (!IsPostBack)
            {
                if (Session["userId"] != null && Session["role"] != null &&
                (Session["role"].ToString() == "Responsable" ||
                Session["role"].ToString() == "Secretaire" ||
                Session["role"].ToString() == "Administrateur" ||
                Session["role"].ToString() == "Comptable"))
                {
                    string prenom = Session["prenom"].ToString();
                    string role = Session["role"].ToString();
                    lbUser.Text = Session["prenom"].ToString();


                  

                    // Tout cacher d'abord
                    menuAdministration.Visible = false;
                    menuCours.Visible = false;
                    menuEtudiant.Visible = false;
                    menuProfesseur.Visible = false;
                    menuPresence.Visible = false;
                    menuNote.Visible = false;
                    menuComptable.Visible = false;
                    menuRapport.Visible = false;
                    menuJournalisation.Visible = false;
                    menuEnvoyerMessage.Visible = false;
                    menuGenererBadge.Visible = false;
                    menuMessageSupport.Visible = false;

                    // GESTION DES ROLES

                    if (role == "Administrateur")
                    {
                        menuAdministration.Visible = true;
                        menuJournalisation.Visible = true;
                    }
                    else if (role == "Responsable")
                    {
                        menuAdministration.Visible = true;
                        menuCours.Visible = true;
                        menuEtudiant.Visible = true;
                        menuProfesseur.Visible = true;
                        menuPresence.Visible = true;
                        menuNote.Visible = true;
                        menuComptable.Visible = true;
                        menuRapport.Visible = true;
                        menuJournalisation.Visible = true;
                        menuEnvoyerMessage.Visible = true;
                        menuGenererBadge.Visible = true;
                        menuMessageSupport.Visible = true;
                    }
                    else if (role == "Secretaire")
                    {
                        menuEtudiant.Visible = true;
                        menuComptable.Visible = true; // paiement étudiant
                        menuEnvoyerMessage.Visible = true;
                        menuGenererBadge.Visible = true;
                    }
                    else if (role == "Comptable") // ✅ NOUVEAU ROLE
                    {
                        menuComptable.Visible = true;
                        menuEtudiant.Visible = true;
                        menuRapport.Visible = true;

                    }
                }
                else
                {
                    Response.Redirect("FormDeconnexion.aspx");
                }
            }


        }



        protected void btnModifierMotPasse_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnSeDeconnecter_Click(object sender, EventArgs e)
        {
            if (Session["code"] != null)
            {
                Log log = new Log();
                log.MettreAJourDeconnexion(Session["code"].ToString());
            }
            Session.Abandon();
            Session.Clear();
            Session["userId"] = null;
            Session["email"] = null;
            Session["role"] = null;
            Session["nom"] = null;
            Session.RemoveAll();
            Response.Redirect("FormConnexion.aspx");

        }
    }
}