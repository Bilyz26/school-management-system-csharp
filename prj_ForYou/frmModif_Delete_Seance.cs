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
    public partial class frmModif_Delete_Seance : Form
    {
        public frmModif_Delete_Seance()
        {
            InitializeComponent();
        }

        private void frmModif_Delete_Seance_Load(object sender, EventArgs e)
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
            LoadGroups();
        }

        private void cmbnomprof_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroups();
        }

        private void cmbannee_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGroups();
        }

        private void LoadGroups()
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

        int pos;
        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null && dgv.CurrentRow.Cells.Count >= 3)
            {
                txtjour.Text = Convert.ToString(dgv.CurrentRow.Cells[0].Value);
                mtxtHD.Text = Convert.ToString(dgv.CurrentRow.Cells[1].Value);
                mtxtHF.Text = Convert.ToString(dgv.CurrentRow.Cells[2].Value);
                pos = dgv.CurrentRow.Index;
            }
        }

        private void btnrechercher_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbannee.Text) &&
                !string.IsNullOrWhiteSpace(cmbmatier.Text) &&
                !string.IsNullOrWhiteSpace(cmbniveau.Text) &&
                !string.IsNullOrWhiteSpace(cmbnomgrp.Text) &&
                !string.IsNullOrWhiteSpace(cmbnomprof.Text))
            {
                DataTable dt = MemberGlobal.rechercher(
                    "select dayy,heureD,heureF from seance where #codegrp=@grp and #annee=@annee and #nomprof=@prof",
                    new SqlParameter("@grp", cmbnomgrp.Text),
                    new SqlParameter("@annee", cmbannee.Text),
                    new SqlParameter("@prof", cmbnomprof.Text));

                if (dt.Rows.Count != 0)
                {
                    dgv.DataSource = dt;
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "N'Exist pas");
                }
            }
        }

        private void btnmodifier_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbannee.Text) &&
                !string.IsNullOrWhiteSpace(cmbmatier.Text) &&
                !string.IsNullOrWhiteSpace(cmbniveau.Text) &&
                !string.IsNullOrWhiteSpace(cmbnomgrp.Text) &&
                !string.IsNullOrWhiteSpace(cmbnomprof.Text))
            {
                if (dgv.CurrentRow != null && dgv.Rows.Count != 0)
                {
                    string dayyVal = Convert.ToString(dgv.CurrentRow.Cells[0].Value);
                    bool i = MemberGlobal.Insert_Edit_Delete(
                        "update seance set heureD=@hd, heureF=@hf where #codegrp=@grp and #nomprof=@prof and #annee=@annee and dayy=@dayy",
                        new SqlParameter("@hd", mtxtHD.Text),
                        new SqlParameter("@hf", mtxtHF.Text),
                        new SqlParameter("@grp", cmbnomgrp.Text),
                        new SqlParameter("@prof", cmbnomprof.Text),
                        new SqlParameter("@annee", cmbannee.Text),
                        new SqlParameter("@dayy", dayyVal));

                    if (i)
                    {
                        MemberGlobal.messageBox(new frmMssageboxSucces(), "modifié avec succèe");
                    }
                    else
                    {
                        MemberGlobal.messageBox(new frmMessagboxFaile(), "modification echoué ressayé");
                    }
                }
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnsupprimer_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbannee.Text) &&
                !string.IsNullOrWhiteSpace(cmbmatier.Text) &&
                !string.IsNullOrWhiteSpace(cmbniveau.Text) &&
                !string.IsNullOrWhiteSpace(cmbnomgrp.Text) &&
                !string.IsNullOrWhiteSpace(cmbnomprof.Text))
            {
                if (dgv.CurrentRow != null && dgv.Rows.Count != 0)
                {
                    string dayyVal = Convert.ToString(dgv.CurrentRow.Cells[0].Value);
                    bool i = MemberGlobal.Insert_Edit_Delete(
                        "delete from seance where #codegrp=@grp and #nomprof=@prof and #annee=@annee and dayy=@dayy",
                        new SqlParameter("@grp", cmbnomgrp.Text),
                        new SqlParameter("@prof", cmbnomprof.Text),
                        new SqlParameter("@annee", cmbannee.Text),
                        new SqlParameter("@dayy", dayyVal));

                    if (i)
                    {
                        MemberGlobal.messageBox(new frmMssageboxSucces(), "Supprimé avec succèe");
                    }
                    else
                    {
                        MemberGlobal.messageBox(new frmMessagboxFaile(), "supprission echoué ressayé");
                    }
                }
            }
        }
    }
}
