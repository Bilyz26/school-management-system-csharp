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
    public partial class frmfinannce : Form
    {
        public frmfinannce()
        {
            InitializeComponent();
            rdbS.Checked = true;
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
                cmbnomprof.SelectedIndexChanged -= new EventHandler(cmbnomprof_SelectedIndexChanged);
                cmbnomprof.DataSource = dtProf;
                cmbnomprof.ValueMember = "nomprof";
                cmbnomprof.SelectedIndexChanged += new EventHandler(cmbnomprof_SelectedIndexChanged);
            }
        }

        private void cmbniveau_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupsForSelectedCriteria();
        }

        private void cmbnomprof_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupsForSelectedCriteria();
        }

        private void LoadGroupsForSelectedCriteria()
        {
            DataTable dtGrp = MemberGlobal.rechercher(
                "select distinct codegrp from grp inner join Raff on codegrp=#codegrp where #idmat=@idmat and #codeNiv=@codeNiv and Raff.#nomprof=@prof and Raff.annee=@annee",
                new SqlParameter("@idmat", cmbmatier.Text),
                new SqlParameter("@codeNiv", cmbniveau.Text),
                new SqlParameter("@prof", cmbnomprof.Text),
                new SqlParameter("@annee", cmbannee.Text));

            if (dtGrp.Rows.Count != 0)
            {
                cmbnomgrp.SelectedIndexChanged -= new EventHandler(cmbnomgrp_SelectedIndexChanged);
                cmbnomgrp.DataSource = dtGrp;
                cmbnomgrp.ValueMember = "codegrp";
                cmbnomgrp.SelectedIndexChanged += new EventHandler(cmbnomgrp_SelectedIndexChanged);
            }
        }

        private void cmbnomgrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select count(#nom) from Raff where #nomprof=@prof and #codegrp=@grp and annee=@annee",
                new SqlParameter("@prof", cmbnomprof.Text),
                new SqlParameter("@grp", cmbnomgrp.Text),
                new SqlParameter("@annee", cmbannee.Text));

            if (dt.Rows.Count != 0)
            {
                lblnombrEleve.Text = dt.Rows[0][0].ToString();
            }
        }

        private void cmbannee_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroupsForSelectedCriteria();
        }

        private void cmbmnth_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void frmfinannce_Load(object sender, EventArgs e)
        {
            DataTable dtMatier = MemberGlobal.rechercher("select * from matier");
            cmbmatier.SelectedIndexChanged -= new EventHandler(cmbmatier_SelectedIndexChanged);
            cmbmatier.DataSource = dtMatier;
            cmbmatier.ValueMember = "idmat";
            cmbmatier.SelectedIndexChanged += new EventHandler(cmbmatier_SelectedIndexChanged);

            DataTable dtAnnee = MemberGlobal.rechercher("select * from Annee");
            cmbannee.SelectedIndexChanged -= new EventHandler(cmbannee_SelectedIndexChanged);
            cmbannee.DataSource = dtAnnee;
            cmbannee.ValueMember = "annee";
            cmbannee.SelectedIndexChanged += new EventHandler(cmbannee_SelectedIndexChanged);

            cmbMatT.SelectedIndexChanged -= new EventHandler(cmbMatT_SelectedIndexChanged);
            cmbMatT.DataSource = dtMatier.Copy();
            cmbMatT.ValueMember = "idmat";
            cmbMatT.SelectedIndexChanged += new EventHandler(cmbMatT_SelectedIndexChanged);

            cmbAnneeT.SelectedIndexChanged -= new EventHandler(cmbAnneeT_SelectedIndexChanged);
            cmbAnneeT.DataSource = dtAnnee.Copy();
            cmbAnneeT.ValueMember = "annee";
            cmbAnneeT.SelectedIndexChanged += new EventHandler(cmbAnneeT_SelectedIndexChanged);

            gbT.Location = new Point(12, 25);
            this.Size = new Size(430, 439);
        }

        private void cmbMatT_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtProfT = MemberGlobal.rechercher("select * from prof where #idmat=@idmat",
                new SqlParameter("@idmat", cmbMatT.Text));

            if (dtProfT.Rows.Count != 0)
            {
                cmbProT.SelectedIndexChanged -= new EventHandler(cmbProT_SelectedIndexChanged);
                cmbProT.DataSource = dtProfT;
                cmbProT.ValueMember = "nomprof";
                cmbProT.SelectedIndexChanged += new EventHandler(cmbProT_SelectedIndexChanged);
            }
        }

        private void cmbProT_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmbAnneeT_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void rdbS_CheckedChanged(object sender, EventArgs e)
        {
            gbS.Show();
            gbT.Hide();
        }

        private void rdbT_CheckedChanged(object sender, EventArgs e)
        {
            gbS.Hide();
            gbT.Show();
        }

        private void btnCmnc_Click(object sender, EventArgs e)
        {
            if (rdbS.Checked)
            {
                DataTable dtt = MemberGlobal.rechercher(
                    "select sum(prix) from pay where #codegrp=@grp and #nomprof=@prof and #idmat=@idmat and #codeNiv=@codeNiv and monthp=@monthp and #annee=@annee",
                    new SqlParameter("@grp", cmbnomgrp.Text),
                    new SqlParameter("@prof", cmbnomprof.Text),
                    new SqlParameter("@idmat", cmbmatier.Text),
                    new SqlParameter("@codeNiv", cmbniveau.Text),
                    new SqlParameter("@monthp", cmbmnth.Text),
                    new SqlParameter("@annee", cmbannee.Text));

                if (dtt.Rows.Count != 0)
                {
                    txtmT.Text = dtt.Rows[0][0].ToString();
                }
            }
            else if (rdbT.Checked)
            {
                DataTable dtt1 = MemberGlobal.rechercher(
                    "select sum(prix) from pay where #nomprof=@prof and monthp=@monthp and #annee=@annee",
                    new SqlParameter("@prof", cmbProT.Text),
                    new SqlParameter("@monthp", cmbmonthpT.Text),
                    new SqlParameter("@annee", cmbAnneeT.Text));

                if (dtt1.Rows.Count != 0)
                {
                    txtmT.Text = dtt1.Rows[0][0].ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtmT.Text))
            {
                double m, p;
                if (double.TryParse(txtmT.Text, out m) && double.TryParse(mtxt100.Text, out p))
                {
                    double t = (m / 100) * p;
                    txtm.Text = t.ToString("F2");
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MemberGlobal.vider(this);
        }
    }
}
