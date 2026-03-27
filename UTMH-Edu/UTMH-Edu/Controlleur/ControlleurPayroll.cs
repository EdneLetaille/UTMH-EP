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
using System.Configuration;
using UTMH_Edu.ServiceTech;


namespace UTMH_Edu.Controlleur
{
    public class ControlleurPayroll
    {
        public void GenererPayroll(string typePersonne, int idPersonne, int mois, int annee)
        {
            Payroll.GenererPayroll(typePersonne, idPersonne, mois, annee);
        }

        // type: "TOUS" / "ADM" / "PROF"
        public int GenererPayrollPourTous(int mois, int annee, string type = "TOUS")
        {
            DataTable dt = Salaire.ListerPersonnesAvecSalaire(type);

            int nb = 0;
            foreach (DataRow r in dt.Rows)
            {
                string t = r["typePersonne"].ToString();
                int id = Convert.ToInt32(r["idPersonne"]); // <- idAdm ou idProf, menm jan

                try
                {
                    Payroll.GenererPayroll(t, id, mois, annee);
                    nb++;
                }
                catch
                {
                    // ignore doublons / salaire non défini...
                }
            }

            return nb;
        }
    }
}