using UTMH_Edu.Model;

public static class UtilisateurService
{
    public static bool TrouverParEmail(
        string email,
        out int idUtilisateur,
        out string typeUtilisateur)
    {
        // Étudiant
        int idEtudiant = Etudiant.GetIdByEmail(email);
        if (idEtudiant > 0)
        {
            idUtilisateur = idEtudiant;
            typeUtilisateur = "Etudiant";
            return true;
        }

        // Professeur
        int idProf = Professeur.GetIdByEmail(email);
        if (idProf > 0)
        {
            idUtilisateur = idProf;
            typeUtilisateur = "Professeur";
            return true;
        }

        // Administrateur
        int idAdmin = Administrateur.GetIdByEmail(email);
        if (idAdmin > 0)
        {
            idUtilisateur = idAdmin;
            typeUtilisateur = "Administrateur";
            return true;
        }

        idUtilisateur = 0;
        typeUtilisateur = null;
        return false;
    }
}
