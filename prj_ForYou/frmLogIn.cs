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
    public partial class frmLogIn : Form
    {
        public frmLogIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = MemberGlobal.rechercher(
                "select * from emp where username=@username and pw=@pw",
                new SqlParameter("@username", txtusername.Text),
                new SqlParameter("@pw", txtpassword.Text));

            if (dt.Rows.Count != 0)
            {
                this.Hide();
                new FrmMenu().Show();
                dt.Clear();
            }
            else
            {
                MemberGlobal.messageBox(new frmMessagboxFaile(), "le nom d'utilisateur ou le mot de passe est incorrect Réessayer");
                txtpassword.Clear();
                txtusername.Clear();
                txtusername.Focus();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            txtpassword.Clear();
            txtusername.Clear();
            txtusername.Focus();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {
            txtpassword.UseSystemPasswordChar = false;
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                txtpassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtpassword.UseSystemPasswordChar = true;
            }
        }

        private void frmLogIn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
