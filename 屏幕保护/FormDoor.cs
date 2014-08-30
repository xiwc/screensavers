using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace 屏幕保护
{
    public partial class FormDoor : Form
    {
        private FormScreenSaver formScreenSaver;
        private FormPasswordCheck formPasswordCheck;
        private List<string> filePathList = new List<string>();
        private ListAllFile listAllFile;
        private Random random = new Random();
        private List<Image> imageList = new List<Image>();
        public FormDoor(FormScreenSaver formScreenSaver)
        {
            InitializeComponent();
            this.formScreenSaver = formScreenSaver;
            this.MouseWheel += new MouseEventHandler(FormDoor_MouseWheel);

            List<string> suffixList=new List<string>();
            suffixList.AddRange(new string[] { ".jpg", ".bmp", "jpeg", ".gif", ".png", ".ico" });
            listAllFile = new ListAllFile(filePathList, suffixList);
            Thread myThread = new Thread(ListAllNeedFile);
            myThread.IsBackground = true;
            myThread.Start();
        }
        private void ListAllNeedFile()
        {
            foreach (string drive in listAllFile.GetAllDrive())
            {
                listAllFile.StartFind(drive);
            }
            //MessageBox.Show("Over" + filePathList.Count);
        }
        private void FormDoor_MouseWheel(object sender, MouseEventArgs e)
        {
            double speed = ((double)e.Delta) / 120 / 50;
            if (speed > 0 && this.Opacity < 1)
            {
                this.Opacity += speed;
            }
            if (speed < 0 && this.Opacity > 0.1)
            {
                this.Opacity += speed;
            }
        }

        private void FormDoor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (formScreenSaver.Password == String.Empty)
            {
                formScreenSaver.MyScreenKeyHook.KeyBoardHookStop();
                this.Close();
                formScreenSaver.Close();
            }
            else
            {
                if (formPasswordCheck == null)
                {
                    formPasswordCheck = new FormPasswordCheck(formScreenSaver, this);
                    formPasswordCheck.Show();
                }
                else
                {
                    formPasswordCheck.Visible = true;
                }
            }
        }

        private void FormDoor_MouseClick(object sender, MouseEventArgs e)
        {
            if (formPasswordCheck != null)
                formPasswordCheck.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int countFile = filePathList.Count;
            if (countFile != 0)
            {
                string selectedFilePath = filePathList[random.Next(countFile)];
                try
                {
                    using (Image image = Image.FromFile(selectedFilePath))
                    {
                        int x = random.Next(this.Width - 100);
                        int y = random.Next(this.Height - 100);
                        Graphics g = this.CreateGraphics();
                        g.TranslateTransform(x, y);
                        g.RotateTransform(random.Next(360));
                        g.DrawImage(image, 0, 0, image.Width, image.Height);
                    }
                }
                catch { }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Interval = random.Next(1000, 60000);
            Graphics g = this.CreateGraphics();
            g.Clear(this.BackColor);
        }
    }
}
