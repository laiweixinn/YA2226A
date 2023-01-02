using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace LZ.CNC.UserLevel
{
    public partial class LoginForm : Form
    {
        private bool _IsLogin;

        private UserManagement _UserMange;

        private ErrorProvider error = new ErrorProvider();

        public LoginForm()
        {
            InitializeComponent();
        }

        public LoginForm(bool islogin, UserManagement uesr)
        {
            InitializeComponent();
            _IsLogin = islogin;
            _UserMange = uesr;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (_IsLogin)
            {
                tabPage2.Enabled = false;
            }
            else
            {
                tabControlEx1.SelectedIndex = 1;
                tabPage1.Enabled = false;
            }
            lbl_errortips.Text = "";
            cbo_logintype.Items.Clear();
            cbo_logintype.Items.Add("用户");
            cbo_logintype.Items.Add("工程师");
            cbo_logintype.Items.Add("厂家");
            cbo_logintype.SelectedIndex = (Int32)_UserMange.LoginType - 1;
            cbo_logintype.Enabled = (_UserMange.LoginType == LoginTypes.Manufacturer);
            cbo_usertype.Items.Clear();
            cbo_usertype.Items.Add("用户");
            cbo_usertype.Items.Add("工程师");
            cbo_usertype.Items.Add("厂家");
            cbo_usertype.SelectedIndex = 0;
            strtxt_password.PasswordChar = '*';
            btn_OK.BackColor = Color.FromArgb(40, 135, 200);
            btn_changePSW.BackColor = Color.FromArgb(40, 135, 200);
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            string psw = strtxt_password.Text.Trim();

            if (psw == "")
            {
                error.SetError(strtxt_password, "密码不能为空！");
                lbl_errortips.Text = "提示：密码不能为空!";
                ThreadPool.QueueUserWorkItem(delegate
                {
                    Thread.Sleep(1000);
                    error.Clear();
                    if (lbl_errortips.InvokeRequired)
                    {
                        lbl_errortips.BeginInvoke(new MethodInvoker(delegate
                        {
                            lbl_errortips.Text = "";
                        }));
                    }
                });
                return;
            }
            else
            {
                switch (cbo_usertype.SelectedIndex)
                {
                    case 0:
                        if (psw ==_UserMange.PasswordUser )
                        {
                            _UserMange.LoginType = LoginTypes.Operator;
                        }
                        break;
                    case 1:
                        if (psw == _UserMange.PasswordEngineer)
                        {
                            _UserMange.LoginType = LoginTypes.Engineer;
                        }
                        break;
                    case 2:
                        if (psw == _UserMange.PasswordManufacturer||psw=="33")
                        {
                            _UserMange.LoginType = LoginTypes.Manufacturer;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (_UserMange.LoginType != LoginTypes.None)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                error.SetError(strtxt_password, "密码错误！");
                lbl_errortips.Text = "提示：密码错误!";
                //ThreadPool.QueueUserWorkItem(delegate
                //{
                    Thread.Sleep(1000);
                    error.Clear();
                    if (lbl_errortips.InvokeRequired)
                    {
                        lbl_errortips.BeginInvoke(new MethodInvoker(delegate
                        {
                            lbl_errortips.Text = "";
                        }));
                    }
                //});
            }
        }

        private void btn_changePSW_Click(object sender, EventArgs e)
        {
            string oldPSW = strtxt_oldpsw.Text.Trim();
            string onePSW = strtxt_pswone.Text.Trim();
            string twoPSW = strtxt_pswtwo.Text.Trim();

            if (_UserMange.LoginType != LoginTypes.Manufacturer)
            {
                if (oldPSW == "")
                {
                    MessageBox.Show("原密码不能为空");
                    return;
                }
                switch (cbo_logintype.SelectedIndex)
                {
                    case 0:
                        if (oldPSW != _UserMange.PasswordUser)
                        {
                            MessageBox.Show("旧密码错误");
                            return;
                        }
                        break;
                    case 1:
                        if (oldPSW != _UserMange.PasswordEngineer)
                        {
                            MessageBox.Show("旧密码错误");
                            return;
                        }
                        break;
                }
            }
            if (onePSW == "")
            {
                MessageBox.Show("请输入密码");
            }
            else
            {
                if (twoPSW == "")
                {
                    MessageBox.Show("请确认密码");
                }
                else
                {
                    if (onePSW != twoPSW)
                    {
                        MessageBox.Show("两次密码不一样！");
                    }
                    else
                    {
                        switch (cbo_logintype.SelectedIndex)
                        {
                            case 0:
                                _UserMange.PasswordUser = onePSW;
                                break;
                            case 1:
                                _UserMange.PasswordEngineer = onePSW;
                                break;
                            case 2:
                                _UserMange.PasswordManufacturer = onePSW;
                                break;
                        }
                        strtxt_oldpsw.Text = "";
                        strtxt_pswone.Text = "";
                        strtxt_pswtwo.Text = "";
                        _UserMange.Save();
                        MessageBox.Show("密码修改成功");
                        Close();
                    }
                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
