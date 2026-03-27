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
    public class ControleurSalaire
    {
        public void Enregistrer(string typePersonne, int idPersonne, decimal montantMensuel, bool actif = true)
        {
            if (string.IsNullOrWhiteSpace(typePersonne))
                throw new Exception("Type personne obligatoire.");

            typePersonne = typePersonne.Trim().ToUpperInvariant();

            if (typePersonne != "PROF" && typePersonne != "ADM")
                throw new Exception("Type personne invalide.");

            if (idPersonne <= 0)
                throw new Exception("Id personne invalide.");

            if (montantMensuel <= 0)
                throw new Exception("Le montant du salaire doit être supérieur à zéro.");

            string actifDb = actif ? "1" : "0";

            Salaire.UpsertSalaire(typePersonne, idPersonne, montantMensuel, actifDb);
        }

        public decimal LireSalaire(string typePersonne, int idPersonne)
        {
            if (idPersonne <= 0)
                throw new Exception("Id personne invalide.");

            return Salaire.GetMontantMensuel(typePersonne, idPersonne);
        }
    }
}