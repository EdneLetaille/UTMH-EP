using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTMH_Edu.Model;

namespace UTMH_Edu.Vue
{
    public partial class FormJournalisation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("FormDeconnexion.aspx");
                    return;
                }
                ChargerLogs();
            }
        }




        private void ChargerLogs()
        {
            Log log = new Log();

            DateTime? date = null;
            if (!string.IsNullOrEmpty(txtDate.Text))
                date = Convert.ToDateTime(txtDate.Text);

            gvLogs.DataSource = log.GetLogs(
                txtCode.Text.Trim(),
                date
            );
            gvLogs.DataBind();
        }
        protected void gvLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLogs.PageIndex = e.NewPageIndex;
            ChargerLogs();
        }
        protected void btnReinitialiser_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtDate.Text = "";

            gvLogs.PageIndex = 0;
            ChargerLogs();
        }

        protected void FiltrerLogs(object sender, EventArgs e)
        {
            gvLogs.PageIndex = 0; // reset pagination
            ChargerLogs();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Log log = new Log();

            DateTime? date = null;
            if (!string.IsNullOrEmpty(txtDate.Text))
                date = Convert.ToDateTime(txtDate.Text);

            DataTable dt = log.GetLogs(txtCode.Text.Trim(), date);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=Journalisation.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    GridView gv = new GridView();
                    gv.DataSource = dt;
                    gv.DataBind();
                    gv.RenderControl(hw);

                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }


    }
}