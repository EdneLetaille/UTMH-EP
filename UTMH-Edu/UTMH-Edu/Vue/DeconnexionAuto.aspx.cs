using System;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class DeconnexionAuto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["code"] != null)
            {
                Log log = new Log();
                log.MettreAJourDeconnexion(Session["code"].ToString());
            }
        }
    }
}