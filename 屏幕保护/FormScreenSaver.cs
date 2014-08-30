using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 屏幕保护
{
    public partial class FormScreenSaver : Form
    {
        private string password;//屏保密码

        public string Password
        {
            get { return password; }
        }
        private ScreenKeyHook myScreenKeyHook;

        public ScreenKeyHook MyScreenKeyHook
        {
            get { return myScreenKeyHook; }
        }

    
        public FormScreenSaver()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            password = textBoxPassword.Text.Trim();
            if (password == string.Empty)
                MessageBox.Show("您未设置屏保密码,以后双击屏保屏幕可直接关闭屏保.", "温馨提示!");
            else
                MessageBox.Show("请务必记住您所设置的屏保密码,\r\n用来以后双击屏保屏幕关闭屏保.", "温馨提示!");
            this.Visible = false;
            myScreenKeyHook = new ScreenKeyHook(this);
            myScreenKeyHook.KeyBoardHookStart();
            new FormDoor(this).ShowDialog();
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonOK.PerformClick();
        }
    }
}
