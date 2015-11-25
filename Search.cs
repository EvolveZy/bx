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
    public partial class Search : Form
    {
        public Search()
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

        private void Search_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (xh.Text == "" && xm.Text == "" && qs.Text == "")
            {
                MessageBox.Show("条件不能为空！");
            }
            else
            {
                string[] args = new string[3];
                if (xh.Text != "")
                {
                    args[0] = "学号";
                    args[1] = xh.Text.Trim();
                }
                if (xm.Text != "")
                {
                    args[0] = "姓名";
                    args[1] = xm.Text.Trim();
                }
                if (qs.Text != "")
                {
                    args[0] = "寝室";
                    args[1] = qs.Text.Trim();
                }
                args[2] = myuser.ErrInfo;
                object result = myuser.InvokeWebService(myuser.ServerPage, "SearchRepair", args);
                DataSet DSRe = (DataSet)result;
                this.dataGridView1.AutoGenerateColumns = true;

                //将返回的DSRe绑定到datagridview上
                this.dataGridView1.DataSource = DSRe;

                //指定显示的datatable
                this.dataGridView1.DataMember = "net";
                dataGridView1.ScrollBars = ScrollBars.Vertical;
                dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.Columns[0].Width = 55;
                dataGridView1.Columns[2].Width = 70;
                dataGridView1.Columns[6].Width = 55;
                dataGridView1.Columns[8].Width = 110;
                dataGridView1.Columns[9].Width = 110;
                dataGridView1.Columns[10].Width = 70;
                dataGridView1.Columns[14].Width = 110;
                dataGridView1.Columns[15].Width = 70;
                // dataGridView1.Columns[13].Width = 110;
                int i;
                for (i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[9].Value.ToString() == "")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightSalmon;
                    }
                    else if (dataGridView1.Rows[i].Cells[9].Value.ToString() != "" && dataGridView1.Rows[i].Cells[14].Value.ToString() == "")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    else if (dataGridView1.Rows[i].Cells[14].Value.ToString() != "")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    }
                    if (dataGridView1.Rows[i].Cells[6].Value.ToString() == "Vip" && dataGridView1.Rows[i].Cells[9].Value.ToString() == "")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
             
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (myuser.bm == "CFO")
            {
                End.mydat = dataGridView1.CurrentRow;
                End jd = new End();
                jd.ShowDialog();
            }
            else
            {
                MessageBox.Show("你无权结单，NOT IS CFO");
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (myuser.bm == "Teacher" || myuser.bm == "CEO" || myuser.bm == "CTO" || myuser.bm == "CTOA")
            {
                if (MessageBox.Show("确认拒单？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    try
                    {
                        int index = dataGridView1.CurrentRow.Index;
                        string No = dataGridView1.Rows[index].Cells[0].Value.ToString().Trim();
                        string[] args = new string[9];
                        args[0] = "judan";
                        args[1] = myuser.ErrInfo;
                        args[2] = No;
                        args[3] = myuser.name;
                        args[4] = "";
                        args[5] = "";
                        args[6] = "";
                        args[7] = "";
                        object result = myuser.InvokeWebService(myuser.ServerPage, "GuanliRepair", args);
                        MessageBox.Show(result.ToString());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("拒单失败！");
                    }
                }
            }
            else
            {
                MessageBox.Show("你无权结单，NOT IS Teacher CEO CTO CTOA");
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int i;
            for (i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[8].Value.ToString() == "")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightSalmon;
                }
                else if (dataGridView1.Rows[i].Cells[8].Value.ToString() != "" && dataGridView1.Rows[i].Cells[11].Value.ToString() == "")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (dataGridView1.Rows[i].Cells[12].Value.ToString() != "")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            Print.mydat = dataGridView1.CurrentRow;
            Print dy = new Print();
            dy.ShowDialog();
        }
    }
}
