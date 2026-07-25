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
    public partial class frmExplorerpaiment : Form
    {
        public frmExplorerpaiment()
        {
            InitializeComponent();
            rbpeleve.Checked = true;
        }

        private void rbpeleve_CheckedChanged(object sender, EventArgs e)
        {
            grppG.Hide();
            grppE.Location = new Point(128, 40);
            grppE.Show();
            gro.Show();
        }

        private void rbpgroupe_CheckedChanged(object sender, EventArgs e)
        {
            grppE.Hide();
            grppG.Show();
            gro.Hide();
        }

        private void frmExplorerpaiment_Load(object sender, EventArgs e)
        {
            this.Size = new Size(605, 489);
            DataTable dt = MemberGlobal.rechercher("select idmat from matier");
            if (dt.Rows.Count != 0)
            {
                cmbmatpG.SelectedIndexChanged -= new EventHandler(cmbmatpG_SelectedIndexChanged);
                cmbmatpG.DataSource = dt;
                cmbmatpG.ValueMember = "idmat";
                cmbmatpG.SelectedIndexChanged += new EventHandler(cmbmatpG_SelectedIndexChanged);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void cmbmatpG_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtt = MemberGlobal.rechercher("select * from niveauMat where #idmat=@idmat",
                new SqlParameter("@idmat", cmbmatpG.Text));
            if (dtt.Rows.Count != 0)
            {
                cmbnivpG.DataSource = dtt;
                cmbnivpG.ValueMember = "nomMat";
            }

            DataTable dt = MemberGlobal.rechercher("select * from prof where #idmat=@idmat",
                new SqlParameter("@idmat", cmbmatpG.Text));
            if (dt.Rows.Count != 0)
            {
                cmbpropG.SelectedIndexChanged -= new EventHandler(cmbpropG_SelectedIndexChanged);
                cmbpropG.DataSource = dt;
                cmbpropG.ValueMember = "nomprof";
                cmbpropG.SelectedIndexChanged += new EventHandler(cmbpropG_SelectedIndexChanged);
            }
        }

        private void cmbnivpG_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupCombo();
        }

        private void cmbpropG_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupCombo();
        }

        private void cmbAnnpG_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupCombo();
        }

        private void LoadGroupCombo()
        {
            DataTable dt = MemberGlobal.rechercher(
                "select distinct grp.codegrp from grp inner join Raff on codegrp=#codegrp where #idmat=@idmat and #codeNiv=@codeNiv and Raff.#nomprof=@prof and Raff.annee=@annee",
                new SqlParameter("@idmat", cmbmatpG.Text),
                new SqlParameter("@codeNiv", cmbnivpG.Text),
                new SqlParameter("@prof", cmbpropG.Text),
                new SqlParameter("@annee", cmbAnnpG.Text));

            if (dt.Rows.Count != 0)
            {
                cmbgrppG.DataSource = dt;
                cmbgrppG.ValueMember = "codegrp";
            }
        }

        private void cmbannepE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select distinct codegrp from grp inner join Raff on codegrp=Raff.#codegrp where Raff.#nom=@nom and Raff.annee=@annee",
                new SqlParameter("@nom", txtnompE.Text),
                new SqlParameter("@annee", cmbannepE.Text));

            if (dt.Rows.Count != 0)
            {
                cmbEGpE.DataSource = dt;
                cmbEGpE.ValueMember = "codegrp";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select nom as 'Nom et Prenom',#cin as 'CIN' from inscStd where #cin=@cin or nom like @nomLike",
                new SqlParameter("@cin", txtcin_nom.Text),
                new SqlParameter("@nomLike", txtcin_nom.Text + "%"));

            if (dt.Rows.Count != 0)
            {
                dgvpE.DataSource = dt;
            }
        }

        private void dgvpE_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvpE.CurrentRow != null && dgvpE.CurrentRow.Cells.Count > 0)
            {
                txtnompE.Text = Convert.ToString(dgvpE.CurrentRow.Cells[0].Value);
            }
        }

        private void btnrechpG_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select distinct Raff.#nom as 'Nom et Prenom',grp.#idmat as 'Matière',Raff.#codegrp as 'Group',grp.#codeNiv as 'Niveau',Raff.#nomprof as 'Nom et Prenom de Formateur',Raff.annee as 'année d Etude' " +
                "from Raff inner join grp on #codegrp=codegrp " +
                "where Raff.#codegrp=@grp and Raff.#nomprof=@prof and grp.#idmat=@idmat and grp.#codeNiv=@codeNiv " +
                "and Raff.#nom not in (select pay.#nom from pay where pay.#codegrp=@grp and pay.#codeNiv=@codeNiv and pay.#idmat=@idmat and pay.#nomprof=@prof and pay.#annee=@annee and monthp=@monthp)",
                new SqlParameter("@grp", cmbgrppG.Text),
                new SqlParameter("@prof", cmbpropG.Text),
                new SqlParameter("@idmat", cmbmatpG.Text),
                new SqlParameter("@codeNiv", cmbnivpG.Text),
                new SqlParameter("@annee", cmbAnnpG.Text),
                new SqlParameter("@monthp", cmbmnthpG.Text));

            if (dt.Rows.Count != 0)
            {
                MemberGlobal.dt = dt;
                Frmafichage f = new Frmafichage();
                f.Show();
            }
            else
            {
                MemberGlobal.messageBox(new frmMssageboxSucces(), "Tout a payé");
            }
        }

        private void btnrechpE_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select distinct Raff.#nom as 'Nom et Prenom',grp.#idmat as 'Matière',Raff.#codegrp as 'Group',grp.#codeNiv as 'Niveau',Raff.#nomprof as 'Nom et Prenom de Formateur',Raff.annee as 'année d Etude' " +
                "from Raff inner join grp on #codegrp=codegrp " +
                "where Raff.#nom=@nom " +
                "and Raff.#nom not in (select #nom from pay where #nom=@nom and #annee=@annee and monthp=@monthp)",
                new SqlParameter("@nom", txtnompE.Text),
                new SqlParameter("@annee", cmbannepE.Text),
                new SqlParameter("@monthp", cmbmonthpE.Text));

            if (dt.Rows.Count != 0)
            {
                MemberGlobal.dt = dt;
                Frmafichage f = new Frmafichage();
                f.Show();
            }
            else
            {
                MemberGlobal.messageBox(new frmMssageboxSucces(), "Tout a payé");
            }
        }

        private void txtcin_nom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button3_Click(sender, e);
            }
        }
    }
}
