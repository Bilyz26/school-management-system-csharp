using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prj_ForYou
{
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
           
        }
        private void hidepanel()
        {
            panelAdministration.Visible = false;
            panelGeneral.Visible = false;


        }
        private void showpanel(Panel panel)
        {
            if (panel.Visible == false)
            {
                hidepanel();
                panel.Visible = true;

            }
            else { panel.Visible = false; }
        }

        private void btnAdministration_Click(object sender, EventArgs e)
        {
            hidepanel();
            showpanel(panelAdministration);

        }

        private void btnGeneral_Click(object sender, EventArgs e)
        {
            hidepanel();
            showpanel(panelGeneral);
        }

        private void btnFrmIncription_Click(object sender, EventArgs e)
        {
            hidepanel();
            //btnFrmIncription.BackColor =
            openChildFormInPanelFormes(new frm_Inscription_dun_eleve());
            makebtnbleu();
            makebtnwhite(sender);

        }

        private void btnFrmRaff_Click(object sender, EventArgs e)
        {
            hidepanel();
            openChildFormInPanelFormes(new frmraaf());
            makebtnbleu();
            makebtnwhite(sender);

        }

        private void btnFrmpay_Click(object sender, EventArgs e)
        {
            hidepanel();
            openChildFormInPanelFormes(new frmPaiement());
            makebtnbleu();
            makebtnwhite(sender);
        }
       
        private void FrmMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
          
        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {
            hidepanel();
           
        }
        private Form activeform = null;
        private void openChildFormInPanelFormes(Form childForm)
        {
            if (activeform != null)
            {
                activeform.Close();

            }
            activeform = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelFormes.Controls.Add(childForm);
            panelFormes.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();


        }


        private void panelFormes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnfrmprof_Click(object sender, EventArgs e)
        {
            openChildFormInPanelFormes(new frmGestiondesProfs());
            makebtnbleu();
            makebtnwhite(sender);
        }

        private void btnfrmemp_Click(object sender, EventArgs e)
        {
            openChildFormInPanelFormes(new frmGestiondesEmployees());
            makebtnbleu();
            makebtnwhite(sender);
        }

        private void btngrp_Click(object sender, EventArgs e)
        {
            openChildFormInPanelFormes(new frmCreationNouveauGroupe());
            makebtnbleu();
            makebtnwhite(sender);
        }

        private void btnmatier_Click(object sender, EventArgs e)
        {
            openChildFormInPanelFormes(new frmGestiondesMatier());
            makebtnbleu();
            makebtnwhite(sender);
        }

        private void btnseance_Click(object sender, EventArgs e)
        {
            openChildFormInPanelFormes(new frmGestiondes_seances());
            makebtnbleu();
            makebtnwhite(sender);
        }

        private void btnExplorer_Click(object sender, EventArgs e)
        {
            openChildFormInPanelFormes(new frmExplorer());
            hidepanel();
            makebtnbleu();
            makebtnwhite(sender);
        }

        private void btnimpression_Click(object sender, EventArgs e)
        {
            hidepanel();
            makebtnbleu();
            makebtnwhite(sender);
            openChildFormInPanelFormes(new frmImprimer());
        }

        private void btnaide_Click(object sender, EventArgs e)
        {
            hidepanel();
            makebtnbleu();
            makebtnwhite(sender);
            frmAide f = new frmAide();
            f.Show();
        }
        private void makebtnbleu()
        {
            btnAdministration.BackColor = Color.FromArgb(51, 51, 76);
            btnseance.BackColor = Color.FromArgb(75, 75, 112);
            btnmatier.BackColor = Color.FromArgb(75, 75, 112);
            btnimpression.BackColor = Color.FromArgb(51, 51, 76);
            btngrp.BackColor = Color.FromArgb(75, 75, 112);
            btnGeneral.BackColor = Color.FromArgb(51, 51, 76);
            btnFrmRaff.BackColor = Color.FromArgb(51, 51, 76);
            btnfrmprof.BackColor = Color.FromArgb(75, 75, 112);
            btnFrmpay.BackColor = Color.FromArgb(51, 51, 76);
            btnFrmIncription.BackColor = Color.FromArgb(51, 51, 76);
            btnfrmemp.BackColor = Color.FromArgb(75, 75, 112);
            btnExplorer.BackColor = Color.FromArgb(51, 51, 76);
            btnaide.BackColor = Color.FromArgb(51, 51, 76);
            //--------------------------------------------------------------------------------------------------------
            btnAdministration.ForeColor = Color.White;
            btnseance.ForeColor = Color.White;
            btnmatier.ForeColor = Color.White;
            btnimpression.ForeColor = Color.White;
            btngrp.ForeColor = Color.White;
            btnGeneral.ForeColor = Color.White;
            btnFrmRaff.ForeColor = Color.White;
            btnfrmprof.ForeColor = Color.White;
            btnFrmpay.ForeColor = Color.White;
            btnFrmIncription.ForeColor = Color.White;
            btnfrmemp.ForeColor = Color.White;
            btnExplorer.ForeColor = Color.White;
            btnaide.ForeColor = Color.White;

            

        }
        private void makebtnwhite(object sender)
        {
            
            Button b = (Button)sender;
           
            b.BackColor = Color.White;
            b.ForeColor=Color.FromArgb(51, 51, 76);
          
        }
        //btnmousehover
        private void btnFrmIncription_MouseHover(object sender, EventArgs e)
        {
            if(btnFrmIncription.BackColor==Color.White)
            {
                btnFrmIncription.ForeColor = Color.White;
            }
        }

     

        private void btnFrmRaff_MouseHover(object sender, EventArgs e)
        {
            if (btnFrmRaff.BackColor == Color.White)
            {
                btnFrmRaff.ForeColor = Color.White;
            }
        }



        private void btnFrmpay_MouseHover(object sender, EventArgs e)
        {
            if (btnFrmpay.BackColor == Color.White)
            {
                btnFrmpay.ForeColor = Color.White;
            }
        }

        private void btnAdministration_MouseHover(object sender, EventArgs e)
        {
            if (btnAdministration.BackColor == Color.White)
            {
                btnAdministration.ForeColor = Color.White;
            }
        }

        private void btnfrmprof_MouseHover(object sender, EventArgs e)
        {
            if (btnfrmprof.BackColor == Color.White)
            {
                btnfrmprof.ForeColor = Color.White;
            }
        }

        private void btnfrmemp_MouseHover(object sender, EventArgs e)
        {
            if (btnfrmemp.BackColor == Color.White)
            {
                btnfrmemp.ForeColor = Color.White;
            }
        }

        private void btnGeneral_MouseHover(object sender, EventArgs e)
        {
            if (btnGeneral.BackColor == Color.White)
            {
                btnGeneral.ForeColor = Color.White;
            }
        }

        private void btngrp_MouseHover(object sender, EventArgs e)
        {
            if (btngrp.BackColor == Color.White)
            {
                btngrp.ForeColor = Color.White;
            }
        }

        private void btnmatier_MouseHover(object sender, EventArgs e)
        {
            if (btnmatier.BackColor == Color.White)
            {
                btnmatier.ForeColor = Color.White;
            }
        }

        private void btnseance_MouseHover(object sender, EventArgs e)
        {
            if (btnseance.BackColor == Color.White)
            {
                btnseance.ForeColor = Color.White;
            }
        }

        private void btnExplorer_MouseHover(object sender, EventArgs e)
        {
            if (btnExplorer.BackColor == Color.White)
            {
                btnExplorer.ForeColor = Color.White;
            }
        }
        
        private void btnimpression_MouseHover(object sender, EventArgs e)
        {
            
            if (btnimpression.BackColor == Color.White)
            {
                btnimpression.ForeColor = Color.White;
            }
        }

        private void btnaide_MouseHover(object sender, EventArgs e)
        {
            if (btnaide.BackColor == Color.White)
            {
                btnaide.ForeColor = Color.White;
            }
        }

        private void btnaide_MouseLeave(object sender, EventArgs e)
        {
            if (btnaide.BackColor == Color.White)
                btnaide.ForeColor = Color.FromArgb(51, 51, 76);
        }
        //btnmouseleave------------------------------------------------------------------------------------------------

        private void btnFrmRaff_MouseLeave(object sender, EventArgs e)
        {
            if (btnFrmRaff.BackColor == Color.White)
                btnFrmRaff.ForeColor = Color.FromArgb(51, 51, 76);

        }
        private void btnFrmIncription_MouseLeave(object sender, EventArgs e)
        {
            if (btnFrmIncription.BackColor == Color.White)
                btnFrmIncription.ForeColor = Color.FromArgb(51, 51, 76);
        }

        private void btnFrmpay_MouseLeave(object sender, EventArgs e)
        {
                if (btnFrmpay.BackColor == Color.White)
                    btnFrmpay.ForeColor = Color.FromArgb(51, 51, 76);
        }

        private void btnAdministration_MouseLeave(object sender, EventArgs e)
        {
            if (btnAdministration.BackColor == Color.White)
                btnAdministration.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void btnfrmprof_MouseLeave(object sender, EventArgs e)
        {
            if (btnfrmprof.BackColor == Color.White)
                btnfrmprof.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void btnfrmemp_MouseLeave(object sender, EventArgs e)
        {
            if (btnfrmemp.BackColor == Color.White)
                btnfrmemp.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void btnGeneral_MouseLeave(object sender, EventArgs e)
        {
            if (btnGeneral.BackColor == Color.White)
                btnGeneral.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void btngrp_MouseLeave(object sender, EventArgs e)
        {
            if (btngrp.BackColor == Color.White)
                btngrp.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void btnmatier_MouseLeave(object sender, EventArgs e)
        {
            if (btnmatier.BackColor == Color.White)
                btnmatier.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void btnseance_MouseLeave(object sender, EventArgs e)
        {
            if (btnseance.BackColor == Color.White)
                btnseance.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void btnExplorer_MouseLeave(object sender, EventArgs e)
        {
            if (btnExplorer.BackColor == Color.White)
                btnExplorer.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void btnimpression_MouseLeave(object sender, EventArgs e)
        {
            if (btnimpression.BackColor == Color.White)
                btnimpression.ForeColor = Color.FromArgb(51, 51, 76);

        }

        private void FrmMenu_MaximumSizeChanged(object sender, EventArgs e)
        {
           
        }
    }
    
}
