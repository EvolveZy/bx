using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BX
{
    public partial class MainForm : Form
    {
        public MainForm()
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

        private void Form2_Load(object sender, EventArgs e)
        {
            this.label1.Focus();
            load("1");
            string[] args = new string[1];
            args[0] = myuser.ErrInfo;
            object result = myuser.InvokeWebService(myuser.ServerPage, "ZhuangTai", args);
            myuser.serverstatus = result.ToString();
            if (myuser.serverstatus == "on")
            {
                button4.Text = "关闭报修系统";

            }
            else if (myuser.serverstatus == "off")
            {
                button4.Text = "开启报修系统";
            }
            else
            {
                button4.Text = myuser.serverstatus;
            }
            if (myuser.bm == "CEO" || myuser.bm == "CTO" ||myuser.bm == "CTOA" || myuser.bm == "Teacher")
            {
                button4.Enabled = true;
            }
            if(myuser.bm=="Teacher")
            {
                Menu.Items[1].Visible = true;
            }
        }


        int wz = 0, wj = 0;
        public void load(string number)
        {
            wz = 0;
            wj = 0;
            string[] args = new string[2];
            args[0] = number;
            args[1] = myuser.ErrInfo;
            object result = myuser.InvokeWebService(myuser.ServerPage, "ReaderRepair", args);
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
                    wz++;
                }
                else if (dataGridView1.Rows[i].Cells[9].Value.ToString() != "" && dataGridView1.Rows[i].Cells[14].Value.ToString() == "")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    wj++;
                }
                else if (dataGridView1.Rows[i].Cells[14].Value.ToString() != "")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
                if(dataGridView1.Rows[i].Cells[6].Value.ToString() == "Vip" && dataGridView1.Rows[i].Cells[9].Value.ToString() == "")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
            label1.Text = "共计" + dataGridView1.Rows.Count.ToString() + "条报修记录";
            label2.Text = "共计" + wz.ToString() + "条未接单";
            label3.Text = "共计" + wj.ToString() + "条未结单";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BlackList blacklist = new BlackList();
            blacklist.Show();
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
            if (myuser.bm == "CEO" || myuser.bm == "CTO"||myuser.bm == "CTOA")
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
                    load("1");
                }
            }
            else
            {
                MessageBox.Show("你无权结单，NOT IS CEO CTO CTOA");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "开启报修系统")
            {
                if (MessageBox.Show("确定开启报修系统？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    string[] args = new string[2];
                    args[0] = "on";
                    args[1] = myuser.ErrInfo;
                    object result = myuser.InvokeWebService(myuser.ServerPage, "GuanliZt", args);
                    if (result.ToString() == "报修系统开启成功")
                    {
                        button4.Text = "关闭报修系统";
                        MessageBox.Show("报修系统开启成功！");
                    }
                    else
                    {
                        MessageBox.Show(result.ToString());
                    }
                }
            }
            else if (button4.Text == "关闭报修系统")
            {
                if (MessageBox.Show("确认关闭报修系统？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    string[] args = new string[2];
                    args[0] = "off";
                    args[1] = myuser.ErrInfo;
                    object result = myuser.InvokeWebService(myuser.ServerPage, "GuanliZt", args);
                    if (result.ToString() == "报修系统关闭成功")
                    {
                        button4.Text = "开启报修系统";
                        MessageBox.Show("报修系统关闭成功！");
                    }
                    else
                    {
                        MessageBox.Show(result.ToString());
                    }
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
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

        private void button5_Click(object sender, EventArgs e)
        {
            load("2");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            load("3");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            load("4");
        }

        int j = 0,k = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (myuser.shuaxin == "yes")
            {
                load("1");
                myuser.shuaxin = "no";
            }
            j++;
            if(j==10)
            {
                j = 0;
                k++;
                string[] args = new string[2];
                myuser.KeepMd5= myuser.myMD5(myuser.KeepMd5);
                args[0] = myuser.KeepMd5;
                args[1] = myuser.username;
                try {
                    object result = myuser.InvokeWebService(myuser.ServerPage, "KeepSession", args);
                    if (result.ToString() == "False")
                    {
                        timer1.Enabled = false;
                        if (MessageBox.Show("保持在线失败！请重登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            Application.Exit();
                            Application.Restart();
                            Environment.Exit(0);
                        }
                    }
                }
                catch(Exception)
                {
                    timer1.Enabled = false;
                    if (MessageBox.Show("保持在线失败！请重登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        Application.Exit();
                        Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Menu.Show((sender as Button),(sender as Button).PointToClient(Cursor.Position),ToolStripDropDownDirection.Default);
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeP xgmm = new ChangeP();
            xgmm.ShowDialog();
        }

        private void 更新日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("2015-11-22 加入电脑维护。\r\n2015-11-21 全面更新，加入VIP机制。\r\n2015-10-27 修改已知的搜索bug。\r\n2015-10-25 升级更新程序，逼格增加。\r\n2015-10-23 新增Teacher的账号管理、后台更新。\r\n2015-10-20 新增保持会话、后台更新。\r\n2015-10-19 新增搜索功能、记住用户名。\r\n2015-10-16 不知道更新了什么。\r\n2015-10-15 1836更新已知bug。\r\n2015-10-15 1624后台更新、增加查询登录日志。\r\n2015-10-15 1223窗体停靠功能。\r\n2015-10-15 1216美化界面，添加切换账号。\r\n2015-10-14 2232修改黑名单详细记录。\r\n2015-10-14 新增修改密码和后台改进。\r\n2015-10-13 更新后台处理能力。\r\n前面还有无数次更新.....");
        }

        private void 切换账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Application.Restart();
            Environment.Exit(0);
        }

        private void 添加黑名单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddBL lh = new AddBL();
            lh.ShowDialog();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 查询登录日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginLog ll = new LoginLog();
            ll.ShowDialog();
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            GLLogin gl = new GLLogin();
            gl.ShowDialog();
        }

        private void toolStripMenuItem4_Click_1(object sender, EventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GLVip gl = new GLVip();
            gl.ShowDialog();
        }

        private void toolStripMenuItem4_Click_2(object sender, EventArgs e)
        {
            Pc pcwh = new Pc();
            pcwh.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Search ss = new Search();
            ss.ShowDialog();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            Print.mydat = dataGridView1.CurrentRow;
            Print dy = new Print();
            dy.ShowDialog();
        }


    }
}
