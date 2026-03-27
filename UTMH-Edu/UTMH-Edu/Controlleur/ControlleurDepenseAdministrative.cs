using System;
using System.Data;
using UTMH_Edu.Model;

namespace UTMH_Edu.Controlleur
{
    public class ControlleurDepenseAdministrative
    {
        public int Ajouter(int idAdm, string motif, string description, DateTime dateDepense,
                           int quantite, string modePaiement, decimal montant)
        {
            if (idAdm <= 0) throw new Exception("Administrateur invalide.");
            if (string.IsNullOrWhiteSpace(motif)) throw new Exception("Le motif est obligatoire.");
            if (quantite <= 0) throw new Exception("La quantité doit être supérieure à zéro.");
            if (string.IsNullOrWhiteSpace(modePaiement)) throw new Exception("Le mode de paiement est obligatoire.");
            if (montant <= 0) throw new Exception("Le montant doit être supérieur à zéro.");

            return DepenseAdministrative.Insert(idAdm, motif.Trim(), description?.Trim(), dateDepense, quantite, modePaiement.Trim(), montant);
        }

        public void Modifier(int idDepense, int idAdm, string motif, string description, DateTime dateDepense,
                             int quantite, string modePaiement, decimal montant)
        {
            if (idDepense <= 0) throw new Exception("Dépense invalide.");
            if (idAdm <= 0) throw new Exception("Administrateur invalide.");
            if (string.IsNullOrWhiteSpace(motif)) throw new Exception("Le motif est obligatoire.");
            if (quantite <= 0) throw new Exception("La quantité doit être supérieure à zéro.");
            if (string.IsNullOrWhiteSpace(modePaiement)) throw new Exception("Le mode de paiement est obligatoire.");
            if (montant <= 0) throw new Exception("Le montant doit être supérieur à zéro.");

            DepenseAdministrative.Update(idDepense, idAdm, motif.Trim(), description?.Trim(), dateDepense, quantite, modePaiement.Trim(), montant);
        }

        public void Supprimer(int idDepense)
        {
            if (idDepense <= 0) throw new Exception("Dépense invalide.");
            DepenseAdministrative.Delete(idDepense);
        }

        public DataRow Lire(int idDepense)
        {
            if (idDepense <= 0) throw new Exception("Dépense invalide.");
            return DepenseAdministrative.GetById(idDepense);
        }

        public DataTable Lister(DateTime? du, DateTime? au)
        {
            return DepenseAdministrative.Lister(du, au);
        }
    }
}