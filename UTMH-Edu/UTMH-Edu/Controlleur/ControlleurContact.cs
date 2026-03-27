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
    public class ControlleurContact
    {
        private Contact Cont;
        public ControlleurContact()
        {
            Cont = new Contact();
        }

        public void enregistrerContact(string nomComplet, string telephone, string email, string message)
        {
            this.Cont = new Contact(nomComplet, telephone, email, message);
            Cont.enregistrerContact(nomComplet, telephone, email, message);
        }
        

    }
}