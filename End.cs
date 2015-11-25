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
    public partial class End : Form
    {
        public End()
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
        
        public static DataGridViewRow mydat = new DataGridViewRow();
        string No = null;
        private void Form5_Load(object sender, EventArgs e)
        {
            No = mydat.Cells[0].Value.ToString().Trim();
            string bxtime = mydat.Cells[8].Value.ToString().Trim();
            string xuehao = mydat.Cells[1].Value.ToString().Trim();
            string name = mydat.Cells[2].Value.ToString().Trim();
            string tel = mydat.Cells[4].Value.ToString().Trim();
            string room = mydat.Cells[5].Value.ToString().Trim();
            string lx = mydat.Cells[6].Value.ToString().Trim();
            string problem = mydat.Cells[7].Value.ToString().Trim();
            zdname3.Text = mydat.Cells[10].Value.ToString().Trim();
            zdtime3.Text = mydat.Cells[9].Value.ToString().Trim();
            No3.Text = No;
            bxtime3.Text = bxtime;
            khinfo3.Text = xuehao + "（学号） " + name + "（姓名） " + tel + "（联系电话）";
            gzinfo.Text = room + "   " + problem;
            lxinfo.Text = lx;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rproblem, wxfangshi, wxresult, wxname, wxfee;
            wxfangshi = null;
            rproblem = null;
            wxresult = null;
            wxname = null;
            if (sj1.Checked == true)
            {
                rproblem = sj1.Text;
            }
            else if (sj2.Checked == true)
            {
                rproblem = sj2.Text;
            }
            else if (sj3.Checked == true)
            {
                rproblem = sj3.Text;
            }
            else if (sj4.Checked == true)
            {
                rproblem = sj4.Text;
            }
            else if (sj5.Checked == true)
            {
                rproblem = "NONE";
            }
            else
            {
                MessageBox.Show("请选择实际问题！");
                return;
            }
            if (fs1.Checked == true)
            {
                wxfangshi = fs1.Text;
            }
            else if (fs2.Checked == true)
            {
                wxfangshi = fs2.Text;
            }
            else if (fs3.Checked == true)
            {
                wxfangshi = fs3.Text;
            }
            else
            {
                MessageBox.Show("请选择维修方式！");
                return;
            }
            if (jg1.Checked == true)
            {
                wxresult = jg1.Text;
            }
            else if (jg2.Checked == true)
            {
                wxresult = jg2.Text;
            }
            else
            {
                MessageBox.Show("请选择维修结果！");
                return;
            }
            if (sf1.Checked == true)
            {
                wxfee = "0";
            }
            else if (sf2.Checked == true)
            {
                if (fee.Text == null)
                {
                    MessageBox.Show("请输入金额！");
                    return;
                }
                wxfee = fee.Text;
            }
            else
            {
                MessageBox.Show("请选择维修费用！");
                return;
            }
            try
            {
                wxname = wxr.Text;
                string[] args = new string[9];
                args[0] = "jiedan";
                args[1] = myuser.ErrInfo;
                args[2] = No;
                args[3] = myuser.name;
                args[4] = rproblem;
                args[5] = wxfangshi;
                args[6] = wxname;
                args[7] = wxresult;
                args[8] = wxfee;
                object result = myuser.InvokeWebService(myuser.ServerPage, "GuanliRepair", args);
                MessageBox.Show(result.ToString());
                myuser.shuaxin = "yes";

            }
            catch (Exception)
            {
                MessageBox.Show("系统异常！");
            }
        }
        private void sj1_CheckedChanged(object sender, EventArgs e)
        {
            if (sj1.Checked)
            {
                sj1.Checked = true;
                sj2.Checked = false;
                sj3.Checked = false;
                sj4.Checked = false;
                sj5.Checked = false;
            }
        }

        private void sj2_CheckedChanged(object sender, EventArgs e)
        {
            if (sj2.Checked)
            {
                sj2.Checked = true;
                sj1.Checked = false;
                sj3.Checked = false;
                sj4.Checked = false;
                sj5.Checked = false;
            }
        }

        private void sj3_CheckedChanged(object sender, EventArgs e)
        {
            if (sj3.Checked)
            {
                sj3.Checked = true;
                sj2.Checked = false;
                sj1.Checked = false;
                sj4.Checked = false;
                sj5.Checked = false;
            }
        }

        private void sj4_CheckedChanged(object sender, EventArgs e)
        {
            if (sj4.Checked)
            {
                sj4.Checked = true;
                sj1.Checked = false;
                sj2.Checked = false;
                sj3.Checked = false;
                sj5.Checked = false;
            }
        }
        private void sj5_CheckedChanged(object sender, EventArgs e)
        {
            if (sj5.Checked)
            {
                sj5.Checked = true;
                sj1.Checked = false;
                sj2.Checked = false;
                sj3.Checked = false;
                sj4.Checked = false;
            }
        }

        private void fs1_CheckedChanged(object sender, EventArgs e)
        {
            if (fs1.Checked)
            {
                fs1.Checked = true;
                fs2.Checked = false;
                fs3.Checked = false;
            }
        }

        private void fs2_CheckedChanged(object sender, EventArgs e)
        {
            if (fs2.Checked)
            {
                fs2.Checked = true;
                fs1.Checked = false;
                fs3.Checked = false;
            }
        }
        private void fs3_CheckedChanged(object sender, EventArgs e)
        {
            if (fs3.Checked)
            {
                fs3.Checked = true;
                fs1.Checked = false;
                fs2.Checked = false;
            }
        }

        private void jg1_CheckedChanged(object sender, EventArgs e)
        {
            if (jg1.Checked)
            {
                jg1.Checked = true;
                jg2.Checked = false;
            }
        }

        private void jg2_CheckedChanged(object sender, EventArgs e)
        {
            if (jg2.Checked)
            {
                jg2.Checked = true;
                jg1.Checked = false;
            }
        }

        private void sf1_CheckedChanged(object sender, EventArgs e)
        {
            if (sf1.Checked)
            {
                sf1.Checked = true;
                sf2.Checked = false;
            }
        }

        private void sf2_CheckedChanged(object sender, EventArgs e)
        {
            if (sf1.Checked)
            {
                sf2.Checked = true;
                sf1.Checked = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form5_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }
    }
}
