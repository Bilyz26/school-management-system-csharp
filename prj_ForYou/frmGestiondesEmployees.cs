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
    public partial class frmGestiondesEmployees : Form
    {
        string b;
        public frmGestiondesEmployees()
        {
            InitializeComponent();
        }

        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGestiondesEmployees_Load(object sender, EventArgs e)
        {
            btnmodifier.Enabled = false;
            txtmodepasseemp.UseSystemPasswordChar = true;
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from emp where nomemp=@nom",
                new SqlParameter("@nom", txtnomprenomemp.Text));

            if (dt.Rows.Count != 0)
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "Existe déja!");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtnomprenomemp.Text) &&
                    !string.IsNullOrWhiteSpace(txtusernameemp.Text) &&
                    !string.IsNullOrWhiteSpace(txtmodepasseemp.Text))
                {
                    MemberGlobal.Insert_Edit_Delete(
                        "insert into emp values(@nom,@tele,@fonction,@username,@pw)",
                        new SqlParameter("@nom", txtnomprenomemp.Text),
                        new SqlParameter("@tele", mtxttelemp.Text),
                        new SqlParameter("@fonction", cmbfonctionemp.Text),
                        new SqlParameter("@username", txtusernameemp.Text),
                        new SqlParameter("@pw", txtmodepasseemp.Text));

                    MemberGlobal.messageBox(new frmMssageboxSucces(), "Ajouter avec succées!");
                }
            }

            MemberGlobal.vider(this);
        }

        private void btnrechercher_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from emp where nomemp like @nom",
                new SqlParameter("@nom", txtnomprenomemp.Text + "%"));

            if (dt.Rows.Count != 0)
            {
                b = dt.Rows[0][0].ToString();
                txtnomprenomemp.Text = dt.Rows[0][0].ToString();
                mtxttelemp.Text = dt.Rows[0][1].ToString();
                cmbfonctionemp.Text = dt.Rows[0][2].ToString();
                txtusernameemp.Text = dt.Rows[0][3].ToString();
                txtmodepasseemp.Text = dt.Rows[0][4].ToString();

                btnmodifier.Enabled = true;
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "L'employe N'Existe Pas!");
            }
        }

        private void btnmodifier_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from emp where nomemp=@oldNom",
                new SqlParameter("@oldNom", b));

            if (dt.Rows.Count != 0)
            {
                MemberGlobal.Insert_Edit_Delete(
                    "update emp set nomemp=@nom,tele=@tele,fonction=@fonction,username=@username,pw=@pw where nomemp=@oldNom",
                    new SqlParameter("@nom", txtnomprenomemp.Text),
                    new SqlParameter("@tele", mtxttelemp.Text),
                    new SqlParameter("@fonction", cmbfonctionemp.Text),
                    new SqlParameter("@username", txtusernameemp.Text),
                    new SqlParameter("@pw", txtmodepasseemp.Text),
                    new SqlParameter("@oldNom", b));

                MemberGlobal.messageBox(new frmMssageboxSucces(), "Modifier Avec Succées!");
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "L'employe N'Existe Pas!");
            }

            btnmodifier.Enabled = false;
            MemberGlobal.vider(this);
        }

        private void btnsupprimer_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from emp where nomemp=@nom",
                new SqlParameter("@nom", txtnomprenomemp.Text));

            if (dt.Rows.Count != 0)
            {
                MemberGlobal.Insert_Edit_Delete("delete from emp where nomemp=@nom",
                    new SqlParameter("@nom", txtnomprenomemp.Text));

                MemberGlobal.messageBox(new frmMssageboxSucces(), "Supprimer Avec Succées!");
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "L'employe N'Existe Pas!");
            }

            MemberGlobal.vider(this);
        }

        private void cbmotdepasse_CheckedChanged(object sender, EventArgs e)
        {
            if (cbmotdepasse.Checked == false)
            {
                txtmodepasseemp.UseSystemPasswordChar = true;
            }
            else
            {
                txtmodepasseemp.UseSystemPasswordChar = false;
            }
        }
    }
}
