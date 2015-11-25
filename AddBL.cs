using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BX
{
    public partial class AddBL : Form
    {
        public AddBL()
        {
            InitializeComponent();
        }

        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 0x10;
        const int HTBOTTOMRIGHT = 17;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0201://鼠标左键按下的消息 
                    m.Msg = 0x00A1;//更改消息为非客户区按下鼠标 
                    m.LParam = IntPtr.Zero;//默认值 
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内 
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

      
        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (xuehao.Text == "" || reason.Text == "" || xingming.Text == "")
            {
                MessageBox.Show("学号、姓名和原因都必须填写！");
                return;
            }
            else
            {
                if (MessageBox.Show("确定把【" + xingming.Text + "】加入黑名单？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    string xh = xuehao.Text.Trim();
                    string xm = xingming.Text.Trim();
                    string rs = reason.Text.Trim();
                    string[] args = new string[6];
                    args[0] = myuser.ErrInfo;
                    args[1] = xh;
                    args[2] = xm;
                    args[3] = rs;
                    args[4] = myuser.name;
                    args[5] = "add";
                    object result = myuser.InvokeWebService(myuser.ServerPage, "GuanliBlack", args);
                    MessageBox.Show(result.ToString());
                }
            }
        }

        private void Form7_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }
    }
}
