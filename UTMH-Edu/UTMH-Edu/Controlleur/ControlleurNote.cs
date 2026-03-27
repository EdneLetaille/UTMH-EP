using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UTMH_Edu.Model;

namespace UTMH_Edu.Controlleur
{
    public class ControlleurNote
    {
        private Note note;

        public ControlleurNote()
        {
            note = new Note();
        }

        public void EnregistrerNote(int idCours, int idEtudiant, string noteObtenue, string dateNote, string typeNote)
        {
            this.note = new Note(idCours,idEtudiant,noteObtenue, dateNote, typeNote);
            note.enregistrerNote();
        }


        // =========================
        // SUPPRIMER NOTE
        // =========================
        public bool SupprimerNote(int idNote)
        {
            return Note.SupprimerNote(idNote);
        }
    }
}