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
    public partial class frmPaiement : Form
    {
        public frmPaiement()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtcin_nom.Text))
            {
                DataTable dt = MemberGlobal.rechercher(
                    "select nom as 'Nom et Prenom',#cin as 'CIN',Raff.#codegrp as 'Nom groupe',Raff.#nomprof as 'Professeur',grp.#idmat as 'Matiére',grp.#codeNiv as 'Niveau' " +
                    "from inscStd inner join Raff on nom =#nom inner join grp on #codegrp=codegrp where #cin=@searchVal or #nom like @searchLike",
                    new SqlParameter("@searchVal", txtcin_nom.Text),
                    new SqlParameter("@searchLike", "%" + txtcin_nom.Text + "%"));

                if (dt != null && dt.Rows.Count != 0)
                {
                    dgvelevegrp.Hide();
                    dgv.Show();
                    dgv.DataSource = dt;
                    btnrechercherPaiement.Enabled = true;
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "y'a aucun élève avec les donnée vous saisire");
                }
            }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null && dgv.CurrentRow.Cells.Count >= 6)
            {
                txtnomeleve.Text = Convert.ToString(dgv.CurrentRow.Cells[0].Value);
                txtgrp.Text = Convert.ToString(dgv.CurrentRow.Cells[2].Value);
                txtMat.Text = Convert.ToString(dgv.CurrentRow.Cells[4].Value);
                txtNiv.Text = Convert.ToString(dgv.CurrentRow.Cells[5].Value);
                txtprof.Text = Convert.ToString(dgv.CurrentRow.Cells[3].Value);
                btnAjouter.Enabled = true;
            }
        }

        private void frmPaiement_Load(object sender, EventArgs e)
        {
            dtpp.Format = DateTimePickerFormat.Custom;
            dtpp.CustomFormat = "yyyy/MM/dd";

            DataTable dtAnneeList = MemberGlobal.rechercher("select * from Annee");
            if (dtAnneeList != null && dtAnneeList.Rows.Count > 0)
            {
                cmbAnnee.DataSource = dtAnneeList;
                cmbAnnee.ValueMember = "annee";
            }

            btnrechercherPaiement.Enabled = false;
            btnAjouter.Enabled = false;
            btnmodifier.Enabled = false;
            dgvelevegrp.Hide();
        }

        private void btnrechercherPaiement_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select #nom as 'Nom et Prenom',#codegrp as 'Nom groupe',#nomprof as 'Professeur',#idmat as 'Matiére',#codeNiv as 'Niveau',datep as 'Date de Paiement',monthp as 'Mois Payé',prix as 'Prix Payé' " +
                "from pay where #nom=@nom and #codegrp=@codegrp and #codeNiv=@codeNiv and #idmat=@idmat and #nomprof=@nomprof and #annee=@annee",
                new SqlParameter("@nom", txtnomeleve.Text),
                new SqlParameter("@codegrp", txtgrp.Text),
                new SqlParameter("@codeNiv", txtNiv.Text),
                new SqlParameter("@idmat", txtMat.Text),
                new SqlParameter("@nomprof", txtprof.Text),
                new SqlParameter("@annee", cmbAnnee.Text));

            if (dt != null && dt.Rows.Count != 0)
            {
                dgv.Hide();
                dgvelevegrp.SelectionChanged -= new EventHandler(dgvelevegrp_SelectionChanged);
                dgvelevegrp.DataSource = dt;
                dgvelevegrp.SelectionChanged += new EventHandler(dgvelevegrp_SelectionChanged);

                dgvelevegrp.Size = new Size(605, 150);
                dgvelevegrp.Location = new Point(45, 330);
                dgvelevegrp.Show();
                btnmodifier.Enabled = true;
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "Paiement introuvable");
            }
        }

        private void dgvelevegrp_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvelevegrp.CurrentRow != null && dgvelevegrp.CurrentRow.Cells.Count >= 8)
            {
                txtnomeleve.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[0].Value);
                txtgrp.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[1].Value);
                txtprof.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[2].Value);
                txtMat.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[3].Value);
                txtNiv.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[4].Value);

                DateTime pDate;
                if (DateTime.TryParse(Convert.ToString(dgvelevegrp.CurrentRow.Cells[5].Value), out pDate))
                {
                    dtpp.Value = pDate;
                }
                cmbmnth.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[6].Value);
                mtxtPrix.Text = Convert.ToString(dgvelevegrp.CurrentRow.Cells[7].Value);
                btnmodifier.Enabled = true;
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select * from pay where #nom=@nom and #codegrp=@codegrp and #codeNiv=@codeNiv and #idmat=@idmat and #nomprof=@nomprof and monthp=@monthp and #annee=@annee",
                new SqlParameter("@nom", txtnomeleve.Text),
                new SqlParameter("@codegrp", txtgrp.Text),
                new SqlParameter("@codeNiv", txtNiv.Text),
                new SqlParameter("@idmat", txtMat.Text),
                new SqlParameter("@nomprof", txtprof.Text),
                new SqlParameter("@monthp", cmbmnth.Text),
                new SqlParameter("@annee", cmbAnnee.Text));

            if (dt.Rows.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(cmbmnth.Text) && !string.IsNullOrWhiteSpace(mtxtPrix.Text))
                {
                    decimal prixValue;
                    decimal.TryParse(mtxtPrix.Text, out prixValue);
                    bool i = MemberGlobal.Insert_Edit_Delete(
                        "insert into pay values(@nom,@codegrp,@nomprof,@idmat,@annee,@codeNiv,@datep,@monthp,@prix)",
                        new SqlParameter("@nom", txtnomeleve.Text),
                        new SqlParameter("@codegrp", txtgrp.Text),
                        new SqlParameter("@nomprof", txtprof.Text),
                        new SqlParameter("@idmat", txtMat.Text),
                        new SqlParameter("@annee", cmbAnnee.Text),
                        new SqlParameter("@codeNiv", txtNiv.Text),
                        new SqlParameter("@datep", dtpp.Value),
                        new SqlParameter("@monthp", cmbmnth.Text),
                        new SqlParameter("@prix", prixValue));

                    if (i)
                    {
                        MemberGlobal.messageBox(new frmMssageboxSucces(), "Payé avec succée");
                    }
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "Vous avez oublié un ou plus champs vide");
                }
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "Paiement deja payé");
            }
        }

        private void btnmodifier_Click(object sender, EventArgs e)
        {
            if (dgvelevegrp.CurrentRow == null) return;

            string targetMonth = Convert.ToString(dgvelevegrp.CurrentRow.Cells[6].Value);
            DataTable dt = MemberGlobal.rechercher(
                "select * from pay where #nom=@nom and #codegrp=@codegrp and #codeNiv=@codeNiv and #idmat=@idmat and #nomprof=@nomprof and monthp=@monthp and #annee=@annee",
                new SqlParameter("@nom", txtnomeleve.Text),
                new SqlParameter("@codegrp", txtgrp.Text),
                new SqlParameter("@codeNiv", txtNiv.Text),
                new SqlParameter("@idmat", txtMat.Text),
                new SqlParameter("@nomprof", txtprof.Text),
                new SqlParameter("@monthp", targetMonth),
                new SqlParameter("@annee", cmbAnnee.Text));

            if (dt.Rows.Count != 0)
            {
                decimal prixVal;
                decimal.TryParse(mtxtPrix.Text, out prixVal);
                bool i = MemberGlobal.Insert_Edit_Delete(
                    "update pay set #nom=@nom, #codegrp=@codegrp, #codeNiv=@codeNiv, #nomprof=@nomprof, monthp=@monthp, #idmat=@idmat, datep=@datep, prix=@prix, #annee=@annee " +
                    "where #nom=@oldNom and #codegrp=@oldGrp and #nomprof=@oldProf and #idmat=@oldMat and monthp=@oldMonth and #annee=@oldAnnee",
                    new SqlParameter("@nom", txtnomeleve.Text),
                    new SqlParameter("@codegrp", txtgrp.Text),
                    new SqlParameter("@codeNiv", txtNiv.Text),
                    new SqlParameter("@nomprof", txtprof.Text),
                    new SqlParameter("@monthp", cmbmnth.Text),
                    new SqlParameter("@idmat", txtMat.Text),
                    new SqlParameter("@datep", dtpp.Value),
                    new SqlParameter("@prix", prixVal),
                    new SqlParameter("@annee", cmbAnnee.Text),
                    new SqlParameter("@oldNom", dt.Rows[0][0].ToString()),
                    new SqlParameter("@oldGrp", dt.Rows[0][1].ToString()),
                    new SqlParameter("@oldProf", dt.Rows[0][2].ToString()),
                    new SqlParameter("@oldMat", dt.Rows[0][3].ToString()),
                    new SqlParameter("@oldMonth", dt.Rows[0][7].ToString()),
                    new SqlParameter("@oldAnnee", dt.Rows[0][4].ToString()));

                if (i)
                {
                    MemberGlobal.messageBox(new frmMssageboxSucces(), "modifier avec succée");
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "modification echoué");
                }
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "y'a aucun élève avec les donnée vous saisire");
            }
        }

        private void btnFinan_Click(object sender, EventArgs e)
        {
            frmfinannce f = new frmfinannce();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmExplorerpaiment f = new frmExplorerpaiment();
            f.Show();
        }

        private void txtcin_nom_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void txtcin_nom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
