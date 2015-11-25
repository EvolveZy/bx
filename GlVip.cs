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
    public partial class GLVip : Form
    {
        public GLVip()
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
        public void load(string number, string xuehao)
        {
            string[] args = new string[3];
            args[0] = number;
            args[1] = myuser.ErrInfo;
            args[2] = xuehao;
            object result = myuser.InvokeWebService(myuser.ServerPage, "ReaderVip", args);
            DataSet DSRe = (DataSet)result;
            this.dataGridView1.AutoGenerateColumns = true;

            //将返回的DSRe绑定到datagridview上
            this.dataGridView1.DataSource = DSRe;

            //指定显示的datatable
            this.dataGridView1.DataMember = "vip";
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
        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string xm = dataGridView1.CurrentRow.Cells[2].Value.ToString().Trim();
            string xh = dataGridView1.CurrentRow.Cells[1].Value.ToString().Trim();
            if (MessageBox.Show("确定把【" + xm + "】从VIP名单中删除？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                string[] args = new string[5];
                args[0] = myuser.ErrInfo;
                args[1] = xh;
                args[2] = "";
                args[3] = "";
                args[4] = "del";
                object result = myuser.InvokeWebService(myuser.ServerPage, "GuanliVip", args);
                MessageBox.Show(result.ToString());
                load("1", "");
            }
        }

        private void GLVip_Load(object sender, EventArgs e)
        {
            load("1","");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (xuehao.Text.Trim() == "")
            {
                load("1", "");
            }
            else
            {
                load("2", xuehao.Text.Trim());
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Indigo;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UserName.Text == "" || TrueName.Text == "")
            {
                MessageBox.Show("学号、姓名都必须填写！");
                return;
            }
            else
            {
                if (MessageBox.Show("确定把【" + TrueName.Text + "】加入VIP名单？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    string xh = UserName.Text.Trim();
                    string xm = TrueName.Text.Trim();
                    string[] args = new string[5];
                    args[0] = myuser.ErrInfo;
                    args[1] = xh;
                    args[2] = xm;
                    args[3] = myuser.name;
                    args[4] = "add";
                    object result = myuser.InvokeWebService(myuser.ServerPage, "GuanliVip", args);
                    MessageBox.Show(result.ToString());
                    load("1", "");
                }
            }
        }

        private void GLVip_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }
    }
}
