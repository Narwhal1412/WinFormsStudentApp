using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class LoginForm : Form
    {
        private readonly UserService _userService = new UserService();
        private readonly TextBox txtUsername = new TextBox();
        private readonly TextBox txtPassword = new TextBox();

        public LoginForm()
        {
            Text = "Login";
            ClientSize = new Size(300, 190);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var lblUsername = new Label { Text = "Username", Left = 20, Top = 20, AutoSize = true };
            txtUsername.SetBounds(100, 17, 170, 23);

            var lblPassword = new Label { Text = "Password", Left = 20, Top = 60, AutoSize = true };
            txtPassword.SetBounds(100, 57, 170, 23);
            txtPassword.PasswordChar = '*';

            var btnLogin = new Button { Text = "Login", Left = 100, Top = 100, Width = 80 };
            var btnCancel = new Button { Text = "Cancel", Left = 190, Top = 100, Width = 80 };
            btnLogin.Click += btnLogin_Click;
            btnCancel.Click += btnCancel_Click;
            AcceptButton = btnLogin;

            var lblCopyright = new Label { Text = "Developed by [NGUYEN GIA HUY]", Left = 20, Top = 150, AutoSize = true };

            Controls.AddRange(new Control[] { lblUsername, txtUsername, lblPassword, txtPassword, btnLogin, btnCancel, lblCopyright });
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (_userService.Login(txtUsername.Text, txtPassword.Text))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Login failed. Please check your username and password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.GetBaseException().Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Exit App", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}
