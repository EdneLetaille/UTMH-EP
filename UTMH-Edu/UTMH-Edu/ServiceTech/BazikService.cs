using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BazikService
{
    private static readonly string USER_ID = "bzk_sandbox_688785ea_1770682246";
    private static readonly string SECRET_KEY = "sk_sandbox_9432f7aa25d8e4ba894445c93cda4467";

    public static string GetBazikToken()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        var payload = new
        {
            userID = USER_ID,
            secretKey = SECRET_KEY
        };

        var request = (HttpWebRequest)WebRequest.Create("https://api.bazik.io/token");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Accept = "application/json";
        request.Timeout = 30000;

        string json = JsonConvert.SerializeObject(payload);
        byte[] data = Encoding.UTF8.GetBytes(json);

        using (var stream = request.GetRequestStream())
            stream.Write(data, 0, data.Length);

        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string resp = reader.ReadToEnd();
                var obj = JObject.Parse(resp);

                var token = obj["token"]?.ToString();
                if (string.IsNullOrWhiteSpace(token))
                    throw new Exception("Token absent : " + resp);

                return token;
            }
        }
        catch (WebException ex)
        {
            string error = ReadWebException(ex);
            throw new Exception("Erreur Bazik TOKEN : " + error);
        }
    }

    public static string CreerPaiement(decimal montant, string reference)
    {
        string token = GetBazikToken();

        var payload = new
        {
            gdes = montant,
            userID = USER_ID,
            successUrl = "https://unwaved-jennefer-frazzledly.ngrok-free.dev/Vue/Default.aspx",
            errorUrl = "https://unwaved-jennefer-frazzledly.ngrok-free.dev/Vue/Default.aspx",
            webhookUrl = "https://unwaved-jennefer-frazzledly.ngrok-free.dev/Vue/WebhookBazik.aspx",
            referenceId = reference,
            description = "Paiement ASP.NET Bazik"
        };

        var request = (HttpWebRequest)WebRequest.Create("https://api.bazik.io/moncash/token");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Accept = "application/json";
        request.Timeout = 30000;
        request.Headers["Authorization"] = "Bearer " + token;

        string json = JsonConvert.SerializeObject(payload);
        byte[] data = Encoding.UTF8.GetBytes(json);

        using (var stream = request.GetRequestStream())
            stream.Write(data, 0, data.Length);

        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                string responseJson = reader.ReadToEnd();
                var obj = JObject.Parse(responseJson);

                // ✅ Fallback sur plizyè non posib
                string redirect =
                    obj["redirectUrl"]?.ToString()
                    ?? obj["redirect_url"]?.ToString()
                    ?? obj["paymentUrl"]?.ToString()
                    ?? obj["url"]?.ToString()
                    ?? obj["link"]?.ToString();

                if (string.IsNullOrWhiteSpace(redirect))
                    throw new Exception("Aucune URL de redirection trouvée : " + responseJson);

                return redirect;
            }
        }
        catch (WebException ex)
        {
            string error = ReadWebException(ex);
            throw new Exception("Erreur Bazik Paiement : " + error);
        }
    }

    private static string ReadWebException(WebException ex)
    {
        if (ex.Response == null) return ex.Message;

        using (var reader = new StreamReader(ex.Response.GetResponseStream()))
            return reader.ReadToEnd();
    }
}
