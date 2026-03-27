using System;
using System.Security.Cryptography;
using System.Text;
using UTMH_Edu.Model;

namespace UTMH_Edu.Controlleur
{
    public class ControlleurReinitialiserMotPasse
    {
        // TOKEN
        public string GenererToken()
        {
            byte[] bytes = new byte[64];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return Convert.ToBase64String(bytes)
                .Replace("+", "")
                .Replace("/", "")
                .Replace("=", "");
        }

       


        // CRÉER TOKEN
        public string CreerToken(int idUtilisateur, string typeUtilisateur)
        {
            string token = GenererToken();
            DateTime expiration = DateTime.UtcNow.AddMinutes(30);

            ReinitialiserMotPasse reset =
                new ReinitialiserMotPasse(
                    idUtilisateur,
                    token,
                    expiration,
                    typeUtilisateur
                );

            reset.InsererToken();
            return token;
        }

        public bool VerifierToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            return ReinitialiserMotPasse.TokenValide(token);
        }

        // RESET MOT DE PASSE
        public bool ReinitialiserMotDePasse(string token, string nouveauMotPasse)
        {
            if (!ReinitialiserMotPasse.TokenValide(token))
                return false;

            int id = ReinitialiserMotPasse.GetIdUtilisateurByToken(token);
            string type = ReinitialiserMotPasse.GetTypeUtilisateurByToken(token);

            string hash = nouveauMotPasse;

            switch (type)
            {
                case "Etudiant":
                    Etudiant.modifierMotPasseEtudiant(id, hash);
                    break;

                case "Professeur":
                    Professeur.modifierMotPasseProfesseur(id, hash);
                    break;

                case "Administrateur":
                    Administrateur.modifierMotPasseAdministrateur(id, hash);
                    break;

                default:
                    return false;
            }

            ReinitialiserMotPasse.MarquerTokenUtilise(token);
            return true;
        }

        

       
    }
}
