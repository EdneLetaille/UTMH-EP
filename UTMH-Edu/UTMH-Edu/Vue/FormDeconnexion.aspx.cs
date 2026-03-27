using System;
using System.Web;
using System.Web.UI;

namespace UTMH_Edu.Vue
{
    public partial class FormDeconnexion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔒 Désactiver totalement le cache
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            Response.ClearHeaders();
            Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");

            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            // 🔁 Redirection propre
            Response.Redirect("FormConnexion.aspx");
        }
    }
}
