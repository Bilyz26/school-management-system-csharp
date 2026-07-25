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
    public partial class frm_Inscription_dun_eleve : Form
    {
        public frm_Inscription_dun_eleve()
        {
            InitializeComponent();
        }

        private void frm_Inscription_dun_eleve_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
            dtpdatedebut.Format = DateTimePickerFormat.Custom;
            dtpdatedebut.CustomFormat = "yyyy/MM/dd";
            cbcin.Checked = false;
        }

        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gbeleve_Enter(object sender, EventArgs e)
        {
        }

        decimal fri;
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtcineleve.Text) &&
                !string.IsNullOrWhiteSpace(qui) &&
                !string.IsNullOrWhiteSpace(txtnomprenomeleve.Text) &&
                !string.IsNullOrWhiteSpace(mtxtteleleve.Text))
            {
                DataTable b = MemberGlobal.rechercher("select * from inscStd where nom=@nom",
                    new SqlParameter("@nom", txtnomprenomeleve.Text));

                if (b.Rows.Count == 0)
                {
                    if (string.IsNullOrWhiteSpace(txtfrinscr.Text))
                    {
                        fri = 0;
                    }
                    else
                    {
                        decimal.TryParse(txtfrinscr.Text, out fri);
                    }

                    string query = "insert into inscStd values(@cin, @qui, @nom, @tele, @frinsc, @dateD)";
                    bool i = MemberGlobal.Insert_Edit_Delete(query,
                        new SqlParameter("@cin", txtcineleve.Text),
                        new SqlParameter("@qui", qui),
                        new SqlParameter("@nom", txtnomprenomeleve.Text),
                        new SqlParameter("@tele", mtxtteleleve.Text),
                        new SqlParameter("@frinsc", fri),
                        new SqlParameter("@dateD", dtpdatedebut.Value));

                    if (i)
                    {
                        MemberGlobal.messageBox(new frmMssageboxSucces(), " Ajouter avec succée");
                        MemberGlobal.vider(this);
                    }
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "Le Nom d'éléve éxist Deja");
                }
            }
        }

        string qui = "élève";
        private void cbcin_CheckedChanged(object sender, EventArgs e)
        {
            if (cbcin.Checked == false)
            {
                qui = "élève";
            }
            else
            {
                qui = "Parent";
            }
        }

        DataTable dt;
        private void btnrechercher_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtrech.Text))
            {
                dt = MemberGlobal.rechercher(
                    "select nom as 'Nom et Prenom',#cin as 'CIN', qui as 'Le propriétaire de Cin',tele as 'numéro de telephone',frinsc as 'Frais d inscription',dateD as 'date d inscription' from inscStd where #cin=@searchCin or nom like @searchNom",
                    new SqlParameter("@searchCin", txtrech.Text),
                    new SqlParameter("@searchNom", txtrech.Text + "%"));

                if (dt != null && dt.Rows.Count != 0)
                {
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "y'a aucun élève avec les données vous saisir");
                }
            }
        }

        private void btnmodifier_Click(object sender, EventArgs e)
        {
            try
            {
                if (dt != null && dt.Rows.Count != 0 && pos >= 0 && pos < dt.Rows.Count)
                {
                    decimal f = 0;
                    if (!string.IsNullOrWhiteSpace(txtfrinscr.Text))
                    {
                        decimal.TryParse(txtfrinscr.Text, out f);
                    }

                    string oldNom = dt.Rows[pos][0].ToString();
                    string query = "update inscStd set #cin=@cin, nom=@nom, tele=@tele, frinsc=@frinsc, dateD=@dateD, qui=@qui where nom=@oldNom";

                    bool i = MemberGlobal.Insert_Edit_Delete(query,
                        new SqlParameter("@cin", txtcineleve.Text),
                        new SqlParameter("@nom", txtnomprenomeleve.Text),
                        new SqlParameter("@tele", mtxtteleleve.Text),
                        new SqlParameter("@frinsc", f),
                        new SqlParameter("@dateD", dtpdatedebut.Value),
                        new SqlParameter("@qui", qui),
                        new SqlParameter("@oldNom", oldNom));

                    if (i)
                    {
                        MemberGlobal.messageBox(new frmMssageboxSucces(), "modifier avec succées ");
                    }
                    else
                    {
                        MemberGlobal.messageBox(new frmMessagboxFaile(), "y'a aucun élève avec les données vous saisir");
                    }
                }
            }
            catch
            {
            }
        }

        int pos;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dt != null && dt.Rows.Count != 0 && dataGridView1.CurrentRow != null)
                {
                    pos = dataGridView1.CurrentRow.Index;
                    if (pos >= dt.Rows.Count)
                    {
                        pos = dt.Rows.Count - 1;
                    }

                    if (pos >= 0)
                    {
                        txtcineleve.Text = dt.Rows[pos][1].ToString();
                        txtnomprenomeleve.Text = dt.Rows[pos][0].ToString();
                        mtxtteleleve.Text = dt.Rows[pos][3].ToString();
                        txtfrinscr.Text = dt.Rows[pos][4].ToString();
                        DateTime parsedDate;
                        if (DateTime.TryParse(dt.Rows[pos][5].ToString(), out parsedDate))
                        {
                            dtpdatedebut.Value = parsedDate;
                        }

                        if (dt.Rows[pos][2].ToString() == "Parent")
                        {
                            cbcin.Checked = true;
                        }
                        else
                        {
                            cbcin.Checked = false;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void frm_Inscription_dun_eleve_MaximumSizeChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void cbcin_CheckedChanged_1(object sender, EventArgs e)
        {
            if (cbcin.Checked == false)
            {
                qui = "élève";
            }
            else
            {
                qui = "Parent";
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            txtcineleve.Text = string.Empty;
            txtfrinscr.Text = string.Empty;
            txtnomprenomeleve.Text = "";
            mtxtteleleve.Text = string.Empty;
            cbcin.Checked = false;
            dtpdatedebut.Text = string.Empty;
            txtrech.Text = string.Empty;
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns.Clear();
            }
        }

        private void txtrech_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnrechercher_Click(sender, e);
            }
        }
    }
}
