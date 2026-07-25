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
    public partial class frmGestiondesMatier : Form
    {
        public frmGestiondesMatier()
        {
            InitializeComponent();
        }

        private void frmGestiondesMatier_Load(object sender, EventArgs e)
        {
        }

        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmGestionNiveaumatier f = new frmGestionNiveaumatier();
            f.Show();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from matier where idmat=@idmat",
                new SqlParameter("@idmat", txtidmat.Text));

            if (dt.Rows.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(txtidmat.Text))
                {
                    MemberGlobal.Insert_Edit_Delete("insert into matier values(@idmat,@nomMat)",
                        new SqlParameter("@idmat", txtidmat.Text),
                        new SqlParameter("@nomMat", txtnommat.Text));

                    MemberGlobal.messageBox(new frmMssageboxSucces(), "Ajouter Avec Succées!");
                }
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "Existe Déja!");
            }
            MemberGlobal.vider(this);
        }

        private void btnrechercher_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from matier where idmat=@idmat",
                new SqlParameter("@idmat", txtidmat.Text));

            if (dt.Rows.Count != 0)
            {
                txtidmat.Text = dt.Rows[0][0].ToString();
                txtnommat.Text = dt.Rows[0][1].ToString();
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), " N'Existe Pas!");
            }
        }

        private void btnsupprimer_Click(object sender, EventArgs e)
        {
        }
    }
}
