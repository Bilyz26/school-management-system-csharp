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
    public partial class frmGestionNiveaumatier : Form
    {
        public frmGestionNiveaumatier()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void frmGestionNiveaumatier_Load(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from matier");
            cmbmatier.DataSource = dt;
            cmbmatier.ValueMember = "idmat";
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from niveauMat where codeNiv=@codeNiv",
                new SqlParameter("@codeNiv", txtcodeniveau.Text));

            if (dt.Rows.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(txtcodeniveau.Text))
                {
                    bool b = MemberGlobal.Insert_Edit_Delete("insert into niveauMat values(@codeNiv,@idmat,@nomMat)",
                        new SqlParameter("@codeNiv", txtcodeniveau.Text),
                        new SqlParameter("@idmat", cmbmatier.Text),
                        new SqlParameter("@nomMat", txtnomniv.Text));

                    if (b)
                    {
                        MemberGlobal.messageBox(new frmMssageboxSucces(), "Ajouter avec succées!");
                    }
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
            DataTable dt = MemberGlobal.rechercher("select * from niveauMat where codeNiv=@codeNiv",
                new SqlParameter("@codeNiv", txtcodeniveau.Text));

            if (dt.Rows.Count != 0)
            {
                txtcodeniveau.Text = dt.Rows[0][0].ToString();
                cmbmatier.Text = dt.Rows[0][1].ToString();
                txtnomniv.Text = dt.Rows[0][2].ToString();
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), " N'Existe Pas!");
            }
        }
    }
}
