using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBPROJECT
{
    public partial class frmUserProfile : Form
    {
        long iduser;
        String loginname;
        public frmUserProfile(long liduser, String lname)
        {
            InitializeComponent();
            this.iduser = liduser;
            this.loginname = lname;
        }

        private frmChangePassword ChangePasswordfrm;
        private void ChangePasswordfrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChangePasswordfrm.Dispose();
        }

        private void btnChangePwd_Click(object sender, EventArgs e)
        {
            ChangePasswordfrm = new frmChangePassword(this.iduser, this.loginname);
            ChangePasswordfrm.FormClosed += ChangePasswordfrm_FormClosed;
            //ChangePasswordfrm.MdiParent = this;
            ChangePasswordfrm.ShowDialog(); //show a modal
        }

        private void frmUserProfile_LoadUserData()
        {
            String uname = "", uemail = "", ugender = "MALE", usmtphost = "", usmtpport = "";
            DateTime ubirthdate = Convert.ToDateTime("01/01/1940");

        }
    }
}
