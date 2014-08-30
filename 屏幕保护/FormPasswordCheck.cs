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
    public partial class FormPasswordCheck : Form
    {
        private FormScreenSaver formScreenSaver;
        private FormDoor formDoor;
        public FormPasswordCheck(FormScreenSaver formScreenSaver, FormDoor formDoor)
        {
            InitializeComponent();
            this.formScreenSaver = formScreenSaver;
            this.formDoor = formDoor;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.Trim() == formScreenSaver.Password)
            {
                formScreenSaver.MyScreenKeyHook.KeyBoardHookStop();
                this.Close();
                formDoor.Close();
                formScreenSaver.Close();
            }
            else
            {
                if (textBoxPassword.Text.Trim().Length != 0)
                {
                    textBoxPassword.Clear();
                    MessageBox.Show("输入密码不正确!");
                }
            }
        }

        private void textBoxPassword_MouseClick(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).Clear();
        }

        public void SetTextBoxTex(string text)
        {
            textBoxPassword.Text = text;
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonOK.PerformClick();
        }
    }
}
