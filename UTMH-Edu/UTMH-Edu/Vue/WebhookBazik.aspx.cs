using System;
using System.IO;

public partial class WebhookBazik : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string json;
        using (var reader = new StreamReader(Request.InputStream))
        {
            json = reader.ReadToEnd();
        }

        // TODO: vérifier la signature X-Bazik-Signature

        // Désérialiser et traiter
        // Mettre à jour la DB (status = PAID)

        Response.StatusCode = 200;
    }
}
