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
    public partial class frmGestiondesProfs : Form
    {
        string b;
        public frmGestiondesProfs()
        {
            InitializeComponent();
        }

        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from prof where nomprof=@nom",
                new SqlParameter("@nom", txtnomprenomprof.Text));

            if (dt.Rows.Count != 0)
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), " Existe Déja");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtnomprenomprof.Text))
                {
                    MemberGlobal.Insert_Edit_Delete("insert into prof values(@nom,@tele,@idmat)",
                        new SqlParameter("@nom", txtnomprenomprof.Text),
                        new SqlParameter("@tele", mtxtteleprof.Text),
                        new SqlParameter("@idmat", cmbmatierprof.Text));

                    MemberGlobal.messageBox(new frmMssageboxSucces(), "Ajouter avec succées!");
                }
            }
            MemberGlobal.vider(this);
        }

        private void btnmodifier_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from prof where nomprof=@oldNom",
                new SqlParameter("@oldNom", b));

            if (dt.Rows.Count != 0)
            {
                MemberGlobal.Insert_Edit_Delete(
                    "update prof set nomprof=@nom,teleprof=@tele,#idmat=@idmat where nomprof=@oldNom",
                    new SqlParameter("@nom", txtnomprenomprof.Text),
                    new SqlParameter("@tele", mtxtteleprof.Text),
                    new SqlParameter("@idmat", cmbmatierprof.Text),
                    new SqlParameter("@oldNom", b));

                btnmodifier.Enabled = false;
                MemberGlobal.messageBox(new frmMssageboxSucces(), "Modifier avec succées!");
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), " N'Existe Pas!");
            }
            MemberGlobal.vider(this);
        }

        private void frmGestiondesProfs_Load(object sender, EventArgs e)
        {
            btnmodifier.Enabled = false;
            DataTable dt = MemberGlobal.rechercher("select * from matier");
            cmbmatierprof.DataSource = dt;
            cmbmatierprof.ValueMember = "idmat";
            txtnomprenomprof.Text = "";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void btnrechercher_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher("select * from prof where nomprof=@nom",
                new SqlParameter("@nom", txtnomprenomprof.Text));

            if (dt.Rows.Count != 0)
            {
                b = dt.Rows[0][0].ToString();
                txtnomprenomprof.Text = dt.Rows[0][0].ToString();
                mtxtteleprof.Text = dt.Rows[0][1].ToString();
                cmbmatierprof.Text = dt.Rows[0][2].ToString();
                btnmodifier.Enabled = true;
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), " N'Existe Pas!");
            }
        }
    }
}
