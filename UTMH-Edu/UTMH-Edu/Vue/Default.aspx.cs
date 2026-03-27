using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Controlleur;

namespace UTMH_Edu.Vue
{
    public partial class Default : System.Web.UI.Page
    {
       
        public static string strCon = ConfigurationManager.ConnectionStrings["dbConnect"].ConnectionString;
        ControlleurContact Cont = new ControlleurContact();
        ControlleurEtudiant Etu = new ControlleurEtudiant();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {             
                this.listeNomOption();
                this.ChargerDepartements();
                this.RemplirAnneeAcademique();
                txtDateInscription.Text = DateTime.Now.ToString("yyyy-MM-dd");
                if (Session["SuccessMessage"] != null)
                {
                    ShowAlert(Session["SuccessMessage"].ToString());
                    Session.Remove("SuccessMessage");
                   this.viderChamps();
                }

                DateTime hier = DateTime.Today.AddDays(-1);
                txtDateNaissance.Attributes["max"] = hier.ToString("yyyy-MM-dd");

            }

          


        }

        protected void ddlModePaiement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlModePaiement.SelectedValue == "MonCash")
            {
                btnMonCash.Visible = true;
                
            }
            ScriptManager.RegisterStartupScript(
       this,
       GetType(),
       "ReopenModal",
       "$('#exampleModal').modal('show');",
       true
   );

        }


        public bool EmailValide(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var adr = new MailAddress(email.Trim());
                string domaine = adr.Host;

                return domaine.Contains("."); // ex: gmail.com obligatoire
            }
            catch
            {
                return false;
            }
        }
        void viderChamps()
        {
            txtCode.Text = "";
            txtNom.Text = "";
            txtPrenom.Text = "";
            txtDateNaissance.Text = "";
            txtDateInscription.Text = "";
            ddlDepartement.SelectedIndex = -1;
            ddlVille.SelectedIndex = -1;
            txtAdresse.Text = "";
            txtPhone.Text = "";
            txtTelephoneRes.Text = "";
            txtPersonneRes.Text = "";
            txtCin.Text = "";
            txtMail.Text = "";
            ddlSexe.Text = "";
            ddlOption.SelectedIndex = -1;

        }


        protected void btnMonCash_Click(object sender, EventArgs e)
        {
            // 1) Valider champs
            this.codeEtudiant();

            if (string.IsNullOrWhiteSpace(txtCode.Text) ||
                string.IsNullOrWhiteSpace(txtNom.Text) ||
                string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                string.IsNullOrWhiteSpace(txtMail.Text))
            {
                ShowAlert("Certains champs sont vides !");
                return;
            }
            string prenom = txtPrenom.Text;
            if (!Regex.IsMatch(prenom, @"^[A-Za-zÀ-ÿ]+([ _-][A-Za-zÀ-ÿ]+)*$"))
            {
                ShowAlert("Format de prénom invalide.");
                return;
            }
            string nom = txtNom.Text;
            if (!Regex.IsMatch(nom, @"^[A-Za-zÀ-ÿ]+([ _-][A-Za-zÀ-ÿ]+)*$"))
            {
                ShowAlert("Format de nom invalide.");
                return;
            }

            string nomPersonnel = txtPersonneRes.Text;
            if (!Regex.IsMatch(nomPersonnel, @"^[A-Za-zÀ-ÿ]+([ _-][A-Za-zÀ-ÿ]+)*$"))
            {
                ShowAlert("Format de nom du personnel invalide.");
                return;
            }

            DateTime dateNaissance;

            if (!DateTime.TryParse(txtDateNaissance.Text, out dateNaissance))
            {
                ShowAlert("Date de naissance invalide.");
                return;
            }        

            int age = DateTime.Today.Year - dateNaissance.Year;
            if (dateNaissance > DateTime.Today.AddYears(-age)) age--;

            if (age < 12)
            {
                ShowAlert("L'étudiant doit avoir au moins 12 ans.");
                return;
            }

            if (!EmailValide(txtMail.Text))
            {
                ShowAlert("Email non valide.");
                return;
            }

            if (Etu.emailExisteDansSysteme(txtMail.Text))
            {
                ShowAlert("L'email existe déjà dans le système.");
                return;
            }

            if (Etu.cinExisteDansSysteme(txtCin.Text))
            {
                ShowAlert("Le CIN existe déjà dans le système.");
                return;
            }


            if (ddlOption.SelectedValue == "0")
            {
                ShowAlert("Veuillez sélectionner une option valide !");
                return;
            }

            if (string.IsNullOrWhiteSpace(ddlDepartement.SelectedValue))
            {
                ShowAlert("Veuillez sélectionner un departement.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ddlVille.SelectedValue))
            {
                ShowAlert("Veuillez sélectionner une ville.");
                return;
            }


            // ✅ IMPORTANT: si tu veux garder le fichier upload après redirect, il faut le sauvegarder ici
            // (Sinon FileUpload perd le fichier après redirect)
            // string photoPath = SavePhoto(); 
            // Session["INS_photo"] = photoPath;

            // 2) Stocker les infos en Session (✅ types cohérents)
            // idOption = INT => on peut stocker en string aussi, puis Convert.ToInt32 plus tard
            Session["INS_idOption"] = ddlOption.SelectedValue;     // "3"

            Session["INS_code"] = txtCode.Text.Trim();
            Session["INS_nom"] = txtNom.Text.Trim();
            Session["INS_prenom"] = txtPrenom.Text.Trim();
            Session["INS_dateNaissance"] = txtDateNaissance.Text.Trim();
            Session["INS_sexe"] = ddlSexe.Text;

            // ✅ idVille dans ta table etudiant = VARCHAR => stocker string
            Session["INS_idVille"] = ddlVille.SelectedValue;       // "5"

            Session["INS_departement"] = ddlDepartement.Text;
            Session["INS_adresse"] = txtAdresse.Text.Trim();
            Session["INS_telephone"] = txtPhone.Text.Trim();
            Session["INS_email"] = txtMail.Text.Trim();
            Session["INS_personneRes"] = txtPersonneRes.Text.Trim();
            Session["INS_telephoneRes"] = txtTelephoneRes.Text.Trim();

            // dateInscription reste string (car ta table utilise varchar)
            Session["INS_dateInscription"] = txtDateInscription.Text.Trim();

            Session["INS_cin"] = txtCin.Text.Trim();

            // infos paiement inscription
            Session["INS_anneeAcademique"] = ddlAnneeAcademique.Text;
            Session["INS_montantInscription"] = "1500"; 

            // 3) Redirection vers page paiement MonCash
            Response.Redirect("FormPaiementEtudiantMoncash.aspx");
        }

        void RemplirAnneeAcademique()
        {
            ddlAnneeAcademique.Items.Clear();

            int annee = DateTime.Now.Year;
            int mois = DateTime.Now.Month;

            int debut;
            int fin;

            // Si on est avant septembre
            if (mois < 9)
            {
                debut = annee - 1;
                fin = annee;
            }
            else
            {
                debut = annee;
                fin = annee + 1;
            }

            string anneeAcademique = debut + "/" + fin;

            ddlAnneeAcademique.Items.Add(
                new ListItem(anneeAcademique, anneeAcademique)
            );

            ddlAnneeAcademique.SelectedIndex = 0;
        }



        protected void btnAnnuler_Click(object sender, EventArgs e)
        {
            this.viderChamps();
        }
        public void listeNomOption()
        {
            string query = "SELECT * FROM optionChoisie";
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                da.Fill(ds);
            }

            ddlOption.Items.Clear();
            ddlOption.Items.Add(new ListItem("-- Sélectionnez --", "0"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlOption.Items.Add(new ListItem(
                    row["nom"].ToString(),
                    row["idOption"].ToString()
                ));
            }
        }

        private void ChargerDepartements()
        {
            ddlDepartement.Items.Clear();
            ddlDepartement.Items.Add(new ListItem("-- Sélectionner département --", ""));

          

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT idDepartement, nom FROM departement", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlDepartement.Items.Add(
                        new ListItem(dr["nom"].ToString(), dr["idDepartement"].ToString()));
                }
            }
        }

        protected void ddlDepartement_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlVille.Items.Clear();
            ddlVille.Items.Add(new ListItem("-- Sélectionner ville --", ""));

            if (string.IsNullOrEmpty(ddlDepartement.SelectedValue))
                return;

           

            using (SqlConnection con = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand(@"
            SELECT idVille, nom
            FROM ville
            WHERE idDepartement = @idDepartement
        ", con);

                cmd.Parameters.AddWithValue("@idDepartement", ddlDepartement.SelectedValue);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ddlVille.Items.Add(
                        new ListItem(dr["nom"].ToString(), dr["idVille"].ToString()));
                }
            }
            ScriptManager.RegisterStartupScript(
        this,
        GetType(),
        "ReopenModal",
        "$('#exampleModal').modal('show');",
        true
    );
        }

        public void codeEtudiant()
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text))
            {
                return;
            }

            string nom = txtNom.Text.Trim();
            string prenom = txtPrenom.Text.Trim();

            string codeNom = nom.Length >= 2 ? nom.Substring(0, 2).ToUpper() : nom.ToUpper();
            string codePrenom = prenom.Length >= 2 ? prenom.Substring(0, 2).ToUpper() : prenom.ToUpper();

            Random rnd = new Random();
            int randomNumber = rnd.Next(10, 99);

            txtCode.Text = codeNom + "-" + codePrenom + "-" + randomNumber;
        }



        //protected void btnEnvoyerEtudiant_Click(object sender, EventArgs e)
        //{
        //    this.codeEtudiant();

        //    if (string.IsNullOrWhiteSpace(txtCode.Text) ||
        //       string.IsNullOrWhiteSpace(txtNom.Text) ||
        //       string.IsNullOrWhiteSpace(txtPrenom.Text) ||
        //       string.IsNullOrWhiteSpace(txtMail.Text)||
        //       string.IsNullOrWhiteSpace(txtcin.Text))
        //    {
        //        ShowAlert("Certains champs sont vides !");
        //        return;
        //    }

        //    if (!EmailValide(txtMail.Text))
        //    {
        //        ShowAlert("Email non valide.");
        //        return;
        //    }
        //    if (Etu.emailExisteDansSysteme(txtMail.Text))
        //    {
        //        ShowAlert("l'email existe déjà dans le systeme.");
        //        return;
        //    }

        //    if (Etu.cinExisteDansSysteme(txtcin.Text))
        //    {
        //        ShowAlert("la cin existe déjà dans le systeme.");
        //        return;
        //    }
        //    if (!Regex.IsMatch(txtcin.Text, @"^\d{3}-\d{3}-\d{3}-\d{1}$"))
        //    {
        //        ShowAlert("cin invalide");
        //        return;
        //    }

           

        //    if (ddlOption.SelectedValue == "0")
        //    {
        //        ShowAlert("Veuillez sélectionner une option valide !");
        //        return;
        //    }

        //    int idOption = int.Parse(ddlOption.SelectedValue);
        //    int idVille = int.Parse(ddlVille.SelectedValue);


        //    // -----------------------------
        //    // GESTION PHOTO PROFIL
        //    // -----------------------------
        //    string photoPath = null;

           

        //    string extProfil = Path.GetExtension(fuPhotoProfil.FileName).ToLower();
        //    if (extProfil != ".jpg" && extProfil != ".jpeg" && extProfil != ".png")
        //    {

        //    }

        //    string dossierProfil = Server.MapPath("~/Uploads/PhotoProfil/");
        //    if (!Directory.Exists(dossierProfil))
        //        Directory.CreateDirectory(dossierProfil);

        //    string nomProfil = txtCode.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extProfil;
        //    string cheminProfil = Path.Combine(dossierProfil, nomProfil);

        //    fuPhotoProfil.SaveAs(cheminProfil);
        //    photoPath = "~/Uploads/PhotoProfil/" + nomProfil;


        //    // -----------------------------
        //    // Enregistrement en base
        //    // -----------------------------

        //    string statutPaiement = "Non payé";
        //    string anneeAcademique = "";
        //    string modePaiement = "Aucun";
        //    string statut = "En attente";
        //    string role = "Etudiant";

        //    Etu.inscrireEtudiant(
        //        idOption,
        //        txtCode.Text,
        //        txtNom.Text,
        //        txtPrenom.Text,
        //        txtDateNaissance.Text,
        //        ddlSexe.Text,
        //       idVille,
        //        ddlDepartement.Text,
        //        txtAdresse.Text,
        //        txtPhone.Text,
        //        txtMail.Text,
        //        statut,
        //        txtPersonneRes.Text,
        //        txtTelephoneRes.Text,
        //        txtDateInscription.Text,
        //        photoPath,
        //        txtcin.Text,
        //        statutPaiement,
        //        anneeAcademique,
        //        modePaiement,
        //        role
        //    );
        //    // Envoi email après inscription étudiant
        //    ServiceTechnique.SendEmail(
        //        txtMail.Text.Trim(),
        //        "Validation d'inscription - UTMH",
        //        $"Salut {txtPrenom.Text.Trim()} {txtNom.Text.Trim()},\n\n" +
        //        "Votre inscription à l'UTMH a été effectuée avec succès.\n\n" +
        //        "Pour finaliser et valider votre inscription, veuillez vous présenter " +
        //        "à l’établissement avec les documents suivants :\n" +
        //        "- Une preuve de paiement des frais d'inscription\n" +
        //        "- Votre code d'inscription\n\n" +
        //        $"Code d'inscription : {txtCode.Text}\n\n" +
        //        "Sans ces éléments, l’inscription ne pourra pas être validée.\n\n" +
        //        "Cordialement,\n" +
        //        "Administration UTMH"
        //    );



        //    ShowAlert("Inscription effectuée avec succès.");
        //    this.viderChamps();

        //}


        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
                $"document.getElementById('msgText').innerText = '{msg.Replace("'", "\\'")}'; " +
                "new bootstrap.Modal(document.getElementById('msgModal')).show();", true);
        }

        protected void btnEnvoyer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
           string.IsNullOrWhiteSpace(txtNom1.Text) || string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                ShowAlert("Certains champs sont vides !");
                return;
            }
            else
            {
                // -----------------------------
                // Enregistrement en base
                // -----------------------------

                Cont.enregistrerContact(txtNom1.Text,txtTelephone1.Text,txtEmail.Text,txtMessage.Text);
                ShowAlert("Inscription effectuée avec succès.");
            }

        }
    }
}