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
    public partial class LoginLog : Form
    {
        public LoginLog()
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

        private void Form8_Load(object sender, EventArgs e)
        {
            string[] args = new string[2];
            args[0] = myuser.ErrInfo;
            args[1] = myuser.username;
            object result = myuser.InvokeWebService(myuser.ServerPage, "LoginLog", args);
            DataSet DSRe = (DataSet)result;
            this.dataGridView1.AutoGenerateColumns = true;
            //将返回的DSRe绑定到datagridview上
            this.dataGridView1.DataSource = DSRe;
            //指定显示的datatable
            this.dataGridView1.DataMember = "LoginLog";
            dataGridView1.ScrollBars = ScrollBars.Vertical;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].Width = 60;
            int i;
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i % 2 == 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Moccasin;
                }
            }
        }

        private void Form8_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }
    }
}
