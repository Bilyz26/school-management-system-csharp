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
    public partial class frmCreationNouveauGroupe : Form
    {
        public frmCreationNouveauGroupe()
        {
            InitializeComponent();
        }

        private void txtnomgroup_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblnomgrp_Click(object sender, EventArgs e)
        {
        }

        private void btnQuiter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCreationNouveauGroupe_Load(object sender, EventArgs e)
        {
            DataTable dtMatier = MemberGlobal.rechercher("select * from matier");
            cmbmatier.SelectedIndexChanged -= new EventHandler(cmbmatier_SelectedIndexChanged);
            cmbmatier.DataSource = dtMatier;
            cmbmatier.ValueMember = "idmat";
            cmbmatier.SelectedIndexChanged += new EventHandler(cmbmatier_SelectedIndexChanged);
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
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = MemberGlobal.rechercher("select * from grp where codegrp=@codegrp",
                    new SqlParameter("@codegrp", txtnomgroup.Text));

                if (dt.Rows.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(txtnomgroup.Text) &&
                        !string.IsNullOrWhiteSpace(cmbmatier.Text) &&
                        !string.IsNullOrWhiteSpace(cmbniveau.Text))
                    {
                        bool i = MemberGlobal.Insert_Edit_Delete("insert into grp values(@codegrp,@idmat,@codeNiv)",
                            new SqlParameter("@codegrp", txtnomgroup.Text),
                            new SqlParameter("@idmat", cmbmatier.Text),
                            new SqlParameter("@codeNiv", cmbniveau.Text));

                        if (i)
                        {
                            MemberGlobal.messageBox(new frmMssageboxSucces(), "le groupe été crée avec succée");
                        }
                    }
                    else
                    {
                        MemberGlobal.messageBox(new frmMessagboxFaile(), "Vous avez oublié un ou plus champs vide ");
                    }
                }
                else
                {
                    MemberGlobal.messageBox(new frmMessagboxFaile(), "Exist Déja ");
                }
            }
            catch
            {
            }
        }

        private void cmbniveau_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
