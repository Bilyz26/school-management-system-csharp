using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace prj_ForYou
{
    public partial class frmImprimer : Form
    {
        public static string groupe;
        public static string Matiere;
        public static string Niveau;
        public static string prof;
        public static string Annee;

        public frmImprimer()
        {
            InitializeComponent();
        }

        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmImprimer_Load(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select idmat from matier");
            if (dt.Rows.Count != 0)
            {
                cmbmatier.DataSource = dt;
                cmbmatier.ValueMember = "idmat";
            }

            DataTable dtAnnee = MemberGlobal.rechercher("select * from Annee");
            if (dtAnnee != null && dtAnnee.Rows.Count != 0)
            {
                cmbannee.DataSource = dtAnnee;
                cmbannee.ValueMember = "annee";
            }
        }

        private void cmbmatier_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtt = MemberGlobal.rechercher("select * from niveauMat where #idmat=@idmat",
                new SqlParameter("@idmat", cmbmatier.Text));

            if (dtt.Rows.Count != 0)
            {
                cmbniveau.DataSource = dtt;
                cmbniveau.ValueMember = "nomMat";
            }

            DataTable dt = MemberGlobal.rechercher("select * from prof where #idmat=@idmat",
                new SqlParameter("@idmat", cmbmatier.Text));

            if (dt.Rows.Count != 0)
            {
                cmbnomprof.DataSource = dt;
                cmbnomprof.ValueMember = "nomprof";
            }
        }

        private void cmbniveau_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select codegrp from grp where #idmat=@idmat and #codeNiv=@codeNiv",
                new SqlParameter("@idmat", cmbmatier.Text),
                new SqlParameter("@codeNiv", cmbniveau.Text));

            if (dt.Rows.Count != 0)
            {
                cmbgroup.DataSource = dt;
                cmbgroup.ValueMember = "codegrp";
            }
        }

        private void btnimprimer_Click(object sender, EventArgs e)
        {
            groupe = cmbgroup.Text;
            Matiere = cmbmatier.Text;
            Niveau = cmbniveau.Text;
            prof = cmbnomprof.Text;
            Annee = cmbannee.Text;

            if (string.IsNullOrWhiteSpace(groupe) || string.IsNullOrWhiteSpace(prof) || string.IsNullOrWhiteSpace(Annee))
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "Veuillez sélectionner tous les critères du rapport.");
                return;
            }

            frmAbsence f = new frmAbsence();
            f.Show();
        }
    }
}
