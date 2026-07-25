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
    public partial class frmraaf : Form
    {
        public frmraaf()
        {
            InitializeComponent();
        }

        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btncreategroup_Click_1(object sender, EventArgs e)
        {
            frmCreationNouveauGroupe g = new frmCreationNouveauGroupe();
            g.Show();
            g.MaximumSize = new Size(574, 286);
            foreach (Control c in g.Controls)
            {
                if (c is Button && c.Name == "btnQuiter")
                {
                    c.Hide();
                }
            }
        }

        private void frmraaf_Load(object sender, EventArgs e)
        {
            DataTable dtMatier = MemberGlobal.rechercher("select * from matier");
            cmbmatier.SelectedIndexChanged -= new EventHandler(cmbmatier_SelectedIndexChanged);
            cmbmatier.DataSource = dtMatier;
            cmbmatier.ValueMember = "idmat";
            cmbmatier.SelectedIndexChanged += new EventHandler(cmbmatier_SelectedIndexChanged);

            DataTable dtAnnee = MemberGlobal.rechercher("select * from Annee");
            cmbAnnee.DataSource = dtAnnee;
            cmbAnnee.ValueMember = "annee";

            dgvelevegrp.Hide();
            dgv.SelectionChanged += new EventHandler(dgv_SelectionChanged);
        }

        private void cmbmatier_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtNiv = MemberGlobal.rechercher("select * from niveauMat where #idmat=@idmat",
                new SqlParameter("@idmat", cmbmatier.Text));

            if (dtNiv.Rows.Count != 0)
            {
                cmbniveau.SelectedIndexChanged -= new EventHandler(cmbniveau_SelectedIndexChanged);
                cmbniveau.DataSource = dtNiv;
                cmbniveau.ValueMember = "nomMat";
                cmbniveau.SelectedIndexChanged += new EventHandler(cmbniveau_SelectedIndexChanged);
            }

            DataTable dtProf = MemberGlobal.rechercher("select * from prof where #idmat=@idmat",
                new SqlParameter("@idmat", cmbmatier.Text));

            if (dtProf.Rows.Count != 0)
            {
                cmbprof.DataSource = dtProf;
                cmbprof.ValueMember = "nomprof";
            }
        }

        private void cmbGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select count(#nom) from Raff where #nomprof=@prof and #codegrp=@grp and annee=@annee",
                new SqlParameter("@prof", cmbprof.Text),
                new SqlParameter("@grp", cmbGrp.Text),
                new SqlParameter("@annee", cmbAnnee.Text));

            if (dt.Rows.Count != 0)
            {
                lblnombreeleve.Text = dt.Rows[0][0].ToString();
            }
        }

        private void cmbniveau_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtGrp = MemberGlobal.rechercher("select * from grp where #idmat=@idmat and #codeNiv=@codeNiv",
                new SqlParameter("@idmat", cmbmatier.Text),
                new SqlParameter("@codeNiv", cmbniveau.Text));

            if (dtGrp.Rows.Count != 0)
            {
                cmbGrp.SelectedIndexChanged -= new EventHandler(cmbGrp_SelectedIndexChanged);
                cmbGrp.DataSource = dtGrp;
                cmbGrp.ValueMember = "codegrp";
                cmbGrp.SelectedIndexChanged += new EventHandler(cmbGrp_SelectedIndexChanged);
            }
        }

        private void btnrechercher_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select #nom as 'Nom d élève',#codegrp as 'Group',annee as 'Année d étude',#nomprof as 'Nom de Professeur' from Raff where #nom=@nom",
                new SqlParameter("@nom", txtnomprenomeleve.Text));

            if (dt.Rows.Count != 0)
            {
                dgv.Hide();
                dgvelevegrp.SelectionChanged -= new EventHandler(dgvelevegrp_SelectionChanged);
                dgvelevegrp.DataSource = dt;
                dgvelevegrp.SelectionChanged += new EventHandler(dgvelevegrp_SelectionChanged);

                dgvelevegrp.Size = new Size(622, 150);
                dgvelevegrp.Location = new Point(32, 296);
                dgvelevegrp.Show();
            }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null && dgv.CurrentRow.Cells.Count > 0)
            {
                txtnomprenomeleve.Text = Convert.ToString(dgv.CurrentRow.Cells[0].Value);
            }
        }

        private void btnRchercherEleve_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select nom as 'Nom et Prenom',#cin as 'CIN', qui as 'Le propriétaire de Cin',tele as 'numéro de telephone',frinsc as 'Frais d inscription',dateD as 'date d inscription' from inscStd where #cin=@cin or nom like @nomLike",
                new SqlParameter("@cin", txtcin.Text),
                new SqlParameter("@nomLike", txtcin.Text + "%"));

            if (dt.Rows.Count != 0)
            {
                dgvelevegrp.Hide();
                dgv.Show();
                dgv.DataSource = dt;
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "y'a aucun élève avec les donnée vous saisire");
            }
        }

        private void dgvelevegrp_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvelevegrp.CurrentRow == null || dgvelevegrp.CurrentRow.Cells.Count < 4) return;

            string grpCode = Convert.ToString(dgvelevegrp.CurrentRow.Cells[1].Value);
            DataTable dt = MemberGlobal.rechercher("select * from grp where codegrp=@codegrp",
                new SqlParameter("@codegrp", grpCode));

            if (dt.Rows.Count != 0)
            {
                txtnomprenomeleve.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[0].Value);
                cmbmatier.Text = dt.Rows[0][1].ToString();
                cmbniveau.Text = dt.Rows[0][2].ToString();
                cmbGrp.Text = grpCode;
                cmbprof.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[3].Value);
                cmbAnnee.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[2].Value);

                DataTable dtcount = MemberGlobal.rechercher(
                    "select count(#nom) from Raff where #nomprof=@prof and #codegrp=@grp",
                    new SqlParameter("@prof", cmbprof.Text),
                    new SqlParameter("@grp", cmbGrp.Text));

                if (dtcount.Rows.Count != 0)
                {
                    lblnombreeleve.Text = dtcount.Rows[0][0].ToString();
                }
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select * from Raff where #nomprof=@prof and #codegrp=@grp and #nom=@nom",
                new SqlParameter("@prof", cmbprof.Text),
                new SqlParameter("@grp", cmbGrp.Text),
                new SqlParameter("@nom", txtnomprenomeleve.Text));

            if (dt.Rows.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(cmbGrp.Text) &&
                    !string.IsNullOrWhiteSpace(cmbprof.Text) &&
                    !string.IsNullOrWhiteSpace(txtnomprenomeleve.Text))
                {
                    bool i = MemberGlobal.Insert_Edit_Delete(
                        "insert into Raff values(@nom,@grp,@annee,@prof)",
                        new SqlParameter("@nom", txtnomprenomeleve.Text),
                        new SqlParameter("@grp", cmbGrp.Text),
                        new SqlParameter("@annee", cmbAnnee.Text),
                        new SqlParameter("@prof", cmbprof.Text));

                    if (i)
                    {
                        MemberGlobal.messageBox(new frmMssageboxSucces(), "Ajouter avec succée");
                    }
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "Vous avez oublié un ou plus champs vide");
                }
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "il existe deja");
            }
        }

        private void btnmodifier_Click(object sender, EventArgs e)
        {
            if (dgvelevegrp.CurrentRow == null) return;

            string oldNom = Convert.ToString(dgvelevegrp.CurrentRow.Cells[0].Value);
            string oldGrp = Convert.ToString(dgvelevegrp.CurrentRow.Cells[1].Value);
            string oldProf = Convert.ToString(dgvelevegrp.CurrentRow.Cells[3].Value);

            DataTable dt = MemberGlobal.rechercher(
                "select * from Raff where #nom=@nom and #codegrp=@grp and #nomprof=@prof",
                new SqlParameter("@nom", oldNom),
                new SqlParameter("@grp", oldGrp),
                new SqlParameter("@prof", oldProf));

            if (dt.Rows.Count != 0)
            {
                bool i = MemberGlobal.Insert_Edit_Delete(
                    "update Raff set #nom=@nom, #codegrp=@grp, annee=@annee, #nomprof=@prof " +
                    "where #nom=@oldNom and #codegrp=@oldGrp and #nomprof=@oldProf",
                    new SqlParameter("@nom", txtnomprenomeleve.Text),
                    new SqlParameter("@grp", cmbGrp.Text),
                    new SqlParameter("@annee", cmbAnnee.Text),
                    new SqlParameter("@prof", cmbprof.Text),
                    new SqlParameter("@oldNom", oldNom),
                    new SqlParameter("@oldGrp", oldGrp),
                    new SqlParameter("@oldProf", oldProf));

                if (i)
                {
                    MemberGlobal.messageBox(new frmMssageboxSucces(), "modifier avec succée");
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "modification echoué ressayé");
                }
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "y'a aucun élève avec les donnée vous saisire");
            }
        }

        private void btnsupprimer_Click(object sender, EventArgs e)
        {
            if (dgvelevegrp.CurrentRow == null) return;

            string oldNom = Convert.ToString(dgvelevegrp.CurrentRow.Cells[0].Value);
            string oldGrp = Convert.ToString(dgvelevegrp.CurrentRow.Cells[1].Value);
            string oldProf = Convert.ToString(dgvelevegrp.CurrentRow.Cells[3].Value);

            DataTable dt = MemberGlobal.rechercher(
                "select * from Raff where #nom=@nom and #codegrp=@grp and #nomprof=@prof",
                new SqlParameter("@nom", oldNom),
                new SqlParameter("@grp", oldGrp),
                new SqlParameter("@prof", oldProf));

            if (dt.Rows.Count != 0)
            {
                bool i = MemberGlobal.Insert_Edit_Delete(
                    "delete from Raff where #nom=@oldNom and #codegrp=@oldGrp and #nomprof=@oldProf",
                    new SqlParameter("@oldNom", oldNom),
                    new SqlParameter("@oldGrp", oldGrp),
                    new SqlParameter("@oldProf", oldProf));

                if (i)
                {
                    MemberGlobal.messageBox(new frmMssageboxSucces(), "supprimé avec succée");
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "supprission echoué ressayé");
                }
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "y'a aucun élève avec les donnée vous saisire");
            }
        }

        private void txtcin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnRchercherEleve_Click(sender, e);
            }
        }
    }
}
