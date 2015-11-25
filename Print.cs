using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Web.Services.Description;
using System.Drawing.Drawing2D;

namespace BX
{
    public partial class Print : Form
    {
        public Print()
        {
            InitializeComponent();
        }
        string No;
        string bxtime;
        string xuehao;
        string name;
        string tel;
        string lx;
        string room;
        string problem;
        string zdtime;
        public static DataGridViewRow mydat = new DataGridViewRow();
        private void Form1_Load(object sender, EventArgs e)
        {
            No = mydat.Cells[0].Value.ToString().Trim();
            bxtime = mydat.Cells[8].Value.ToString().Trim();
            xuehao = mydat.Cells[1].Value.ToString().Trim();
            name = mydat.Cells[2].Value.ToString().Trim();
            tel = mydat.Cells[4].Value.ToString().Trim();
            lx = mydat.Cells[6].Value.ToString().Trim();
            room = mydat.Cells[5].Value.ToString().Trim();
            problem = mydat.Cells[7].Value.ToString().Trim();
            zdtime = mydat.Cells[9].Value.ToString().Trim();
            initializeBMP();
            if(zdtime.Trim()=="")
            {
                button2.Visible = false;
                button2.Enabled = false;
                button1.Location = new Point(672, 0);
            }
        }
        public void initializeBMP()
        {

            string loginUser = myuser.name;
            string text = DateTime.Now.ToString();
            string text2 = "No：" + No;
            string str = bxtime;
            string s = ("").PadLeft(30, ' ');
            string str2 = string.Concat(new string[]
				{
					xuehao+"(学号)   ",
					name+"(姓名)   ",
					tel+"(联系电话)   ",
                    lx+"(客户类型)"
				});
            string text4 = room + "     " + problem;
            Bitmap image = new Bitmap(780, 1140);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            Font font = new Font("微软雅黑", 64f, FontStyle.Bold);
            Font font2 = new Font("微软雅黑", 16f);
            Font font3 = new Font("微软雅黑", 11f);
            Font font4 = new Font("微软雅黑", 12f);
            Font font5 = new Font("华文彩云", 28f);
            graphics.DrawImage(this.PBLOGO.Image, 10, 0, 150, 65);
            // graphics.DrawString("CLOVER", font5, Brushes.Black, 7f, 10f);
            graphics.DrawString("四叶草网络技术公司\u3000BPO-公寓网络维护 专用票据\u3000\u3000\u3000①", font2, Brushes.Black, 165f, 0f);
            graphics.DrawLine(Pens.Black, 170, 30, 770, 30);
            graphics.DrawString(string.Concat(new string[]
				{
					"制单人：",
					loginUser,
					"      制单时间：",
					text,
					"                ",text2
				}), font3, Brushes.Black, 168f, 35f);
            graphics.DrawString("❶ 存根联", font, Brushes.Yellow, 385f, 45f, stringFormat);
            graphics.DrawString("报修时间：" + str, font3, Brushes.Black, 10f, 60f);
            graphics.DrawString("客户信息：" + str2, font3, Brushes.Black, 10f, 80f);
            graphics.DrawString("故障信息：" + text4, font3, Brushes.Black, 10f, 100f);
            graphics.DrawString("派单时间：" + DateTime.Today.ToString("yyyy/MM/dd") + " ______________    接单人签字：___________________    授权人签字：___________________", font3, Brushes.Black, 10f, 155f);
            graphics.DrawString(string.Concat(new string[]
				{
					"裁剪线",
                    s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线"
				}), font3, Brushes.Orange, 10f, 195f);
            graphics.DrawLine(Pens.Orange, 0, 205, 780, 205);
            graphics.DrawImage(this.PBLOGO.Image, 10, 225, 150, 65);
            // graphics.DrawString("CLOVER", font5, Brushes.Black, 7f, 235f);
            graphics.DrawString("四叶草网络技术公司\u3000BPO-公寓网络维护 专用票据\u3000\u3000\u3000②", font2, Brushes.Black, 165f, 225f);
            graphics.DrawLine(Pens.Black, 170, 255, 770, 255);
            graphics.DrawString(string.Concat(new string[]
				{
					"制单人：",
					loginUser,
					"      制单时间：",
					text,
					"                ",text2
				}), font3, Brushes.Black, 168f, 260f);
            graphics.DrawString("❷ 工程部留存", font, Brushes.Yellow, 385f, 270f, stringFormat);
            graphics.DrawString("报修时间：" + str, font3, Brushes.Black, 10f, 285f);
            graphics.DrawString("客户信息：" + str2, font3, Brushes.Black, 10f, 305f);

            graphics.DrawString("故障信息：" + text4, font3, Brushes.Black, 10f, 325f);
            graphics.DrawString("接单时间：" + DateTime.Now.ToString() + "        接单人签字：___________________", font3, Brushes.Black, 10f, 350f);
            graphics.DrawString("实际故障：□面板或模块故障   □讯井或交换机故障    □墙线或接触故障   □用户自身原因   □无", font3, Brushes.Black, 10f, 380f);
            graphics.DrawString("维修耗材：水晶头__个  模块__个  面板__个  网线__米\u3000\u3000\u3000\u3000□未收费    □基本5元   材料：________________", font3, Brushes.Black, 10f, 410f);
            graphics.DrawString(string.Concat(new string[]
				{
					"维修时间：________________  ",s,s," 维修员签字：_________________"
				}), font3, Brushes.Black, 10f, 440f);
            graphics.DrawString(string.Concat(new string[]
				{
					
					"裁剪线",
                    s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线"
				}), font3, Brushes.Orange, 10f, 480f);
            graphics.DrawLine(Pens.Orange, 0, 490, 780, 490);
            graphics.DrawImage(this.PBLOGO.Image, 10, 510, 150, 65);
            // graphics.DrawString("CLOVER", font5, Brushes.Black, 7f, 520f);
            graphics.DrawString("四叶草网络技术公司\u3000BPO-公寓网络维护 专用票据\u3000\u3000\u3000③", font2, Brushes.Black, 165f, 510f);
            graphics.DrawLine(Pens.Black, 170, 540, 770, 540);
            graphics.DrawString(string.Concat(new string[]
				{
					"制单人：",
					loginUser,
					"      制单时间：",
					text,
					"                ",text2
				}), font3, Brushes.Black, 168f, 545f);
            graphics.DrawString("❸ 财务记账", font, Brushes.Yellow, 385f, 555f, stringFormat);
            graphics.DrawString("报修时间：" + str, font3, Brushes.Black, 10f, 570f);
            graphics.DrawString("客户信息：" + str2, font3, Brushes.Black, 10f, 590f);
            graphics.DrawString("故障信息：" + text4, font3, Brushes.Black, 10f, 610f);
            graphics.DrawString("实际故障：□面板或模块故障   □讯井或交换机故障    □墙线或接触故障   □用户自身原因   □无", font3, Brushes.Black, 10f, 635f);
            graphics.DrawString("处理方式：□维修   □不维修   □更换\u3000__________________________________\u3000\u3000\u3000\u3000维修结果：□成功    □失败", font3, Brushes.Black, 10f, 670f);
            graphics.DrawString("维修耗材：水晶头__个  模块__个  面板__个  网线__米", font3, Brushes.Black, 10f, 700f);
            graphics.DrawString("收费情况：□未收费   □基本5元,材料________\u3000\u3000满意调查：□非常满意    □满意    □一般    □不满意", font3, Brushes.Black, 10f, 730f);
            graphics.DrawString(string.Concat(new string[]
				{
					"维修员签字：________________  ",s,s," 客户签字：_________________"
				}), font3, Brushes.Black, 10f, 760f);
            graphics.DrawString(string.Concat(new string[]
				{
					
					"裁剪线",
                    s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线"
				}), font3, Brushes.Orange, 10f, 800f);
            graphics.DrawLine(Pens.Orange, 0, 810, 780, 810);
            graphics.DrawImage(this.PBLOGO.Image, 10, 830, 150, 65);
            //graphics.DrawString("CLOVER", font5, Brushes.Black, 7f, 840f);
            graphics.DrawString("四叶草网络技术公司\u3000BPO-公寓网络维护 专用票据\u3000\u3000\u3000④", font2, Brushes.Black, 165f, 830f);
            graphics.DrawLine(Pens.Black, 170, 860, 770, 860);
            graphics.DrawString(string.Concat(new string[]
				{
					"制单人：",
					loginUser,
					"      制单时间：",
					text,
					"                ",text2
				}), font3, Brushes.Black, 168f, 865f);
            graphics.DrawString("❹ 客户留存", font, Brushes.Yellow, 385f, 875f, stringFormat);
            graphics.DrawString("报修时间：" + str, font3, Brushes.Black, 10f, 890f);
            graphics.DrawString("客户信息：" + str2, font3, Brushes.Black, 10f, 910f);
            graphics.DrawString("故障信息：" + text4, font3, Brushes.Black, 10f, 930f);
            graphics.DrawString("实际故障：□面板或模块故障   □讯井或交换机故障    □墙线或接触故障   □用户自身原因   □无", font3, Brushes.Black, 10f, 960f);
            graphics.DrawString("处理方式：□维修   □不维修   □更换\u3000__________________________________\u3000\u3000\u3000\u3000维修结果：□成功    □失败", font3, Brushes.Black, 10f, 990f);
            graphics.DrawString("服务收费：□收费5元 （因用户所至的故障维修成功后收取），材料费____________", font3, Brushes.Black, 10f, 1020f);
            graphics.DrawString("\u3000\u3000\u3000\u3000\u3000□免费服务（非用户原因的故障，如讯井间或交换机故障）", font3, Brushes.Black, 10f, 1050f);
            graphics.DrawString(string.Concat(new string[]
				{
					"维修时间：________________  ",s,s," 维修员签字：_________________"
				}), font3, Brushes.Black, 10f, 1080f);
            graphics.DrawString("*请将此联保留3天，3天内出现非用户原因造成的故障凭此联免费维修，盖章生效，最终解释权归本公司所有", font4, Brushes.Black, 5f, 1115f);
            this.PB.Image = image;

        }

        public void initializeBMP2()
        {

            string loginUser = myuser.name;
            string text = zdtime;
            string text2 = "No：" + No;
            string str = bxtime;
            string s = ("").PadLeft(30, ' ');
            string str2 = string.Concat(new string[]
				{
					xuehao+"(学号)   ",
					name+"(姓名)   ",
                    tel+"(联系电话)   ",
                    lx+"(客户类型)"
                });
            string text4 = room + "     " + problem;
            Bitmap image = new Bitmap(780, 1140);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            Font font = new Font("微软雅黑", 64f, FontStyle.Bold);
            Font font2 = new Font("微软雅黑", 16f);
            Font font3 = new Font("微软雅黑", 11f);
            Font font4 = new Font("微软雅黑", 12f);
            Font font5 = new Font("华文彩云", 28f);
            graphics.DrawImage(this.PBLOGO.Image, 10, 0, 150, 65);
            // graphics.DrawString("CLOVER", font5, Brushes.Black, 7f, 10f);
            graphics.DrawString("四叶草网络技术公司\u3000BPO-公寓网络维护 专用票据\u3000\u3000\u3000①", font2, Brushes.Black, 165f, 0f);
            graphics.DrawLine(Pens.Black, 170, 30, 770, 30);
            graphics.DrawString(string.Concat(new string[]
				{
					"制单人：",
					loginUser,
					"      制单时间：",
					DateTime.Now.ToString(),
					"                ",text2
				}), font3, Brushes.Black, 168f, 35f);
            graphics.DrawString("❶ 存根联", font, Brushes.Yellow, 385f, 45f, stringFormat);
            graphics.DrawString("报修时间：" + str, font3, Brushes.Black, 10f, 60f);
            graphics.DrawString("客户信息：" + str2, font3, Brushes.Black, 10f, 80f);
            graphics.DrawString("故障信息：" + text4, font3, Brushes.Black, 10f, 100f);
            graphics.DrawString("派单时间：" + zdtime + "                接单人签字：___________________    授权人签字：___________________", font3, Brushes.Black, 10f, 155f);
            graphics.DrawString(string.Concat(new string[]
				{
					"裁剪线",
                    s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线"
				}), font3, Brushes.Orange, 10f, 195f);
            graphics.DrawLine(Pens.Orange, 0, 205, 780, 205);
            graphics.DrawImage(this.PBLOGO.Image, 10, 225, 150, 65);
            // graphics.DrawString("CLOVER", font5, Brushes.Black, 7f, 235f);
            graphics.DrawString("四叶草网络技术公司\u3000BPO-公寓网络维护 专用票据\u3000\u3000\u3000②", font2, Brushes.Black, 165f, 225f);
            graphics.DrawLine(Pens.Black, 170, 255, 770, 255);
            graphics.DrawString(string.Concat(new string[]
				{
					"制单人：",
					loginUser,
					"      制单时间：",
					DateTime.Now.ToString(),
					"                ",text2
				}), font3, Brushes.Black, 168f, 260f);
            graphics.DrawString("❷ 工程部留存", font, Brushes.Yellow, 385f, 270f, stringFormat);
            graphics.DrawString("报修时间：" + str, font3, Brushes.Black, 10f, 285f);
            graphics.DrawString("客户信息：" + str2, font3, Brushes.Black, 10f, 305f);
            graphics.DrawString("故障信息：" + text4, font3, Brushes.Black, 10f, 325f);
            graphics.DrawString("接单时间：" + zdtime + "        接单人签字：___________________", font3, Brushes.Black, 10f, 350f);
            graphics.DrawString("实际故障：□面板或模块故障   □讯井或交换机故障    □墙线或接触故障   □用户自身原因   □无", font3, Brushes.Black, 10f, 380f);
            graphics.DrawString("维修耗材：水晶头__个  模块__个  面板__个  网线__米\u3000\u3000\u3000\u3000□未收费    □基本5元   材料：________________", font3, Brushes.Black, 10f, 410f);
            graphics.DrawString(string.Concat(new string[]
				{
					"维修时间：________________  ",s,s," 维修员签字：_________________"
				}), font3, Brushes.Black, 10f, 440f);
            graphics.DrawString(string.Concat(new string[]
				{
					
					"裁剪线",
                    s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线"
				}), font3, Brushes.Orange, 10f, 480f);
            graphics.DrawLine(Pens.Orange, 0, 490, 780, 490);
            graphics.DrawImage(this.PBLOGO.Image, 10, 510, 150, 65);
            // graphics.DrawString("CLOVER", font5, Brushes.Black, 7f, 520f);
            graphics.DrawString("四叶草网络技术公司\u3000BPO-公寓网络维护 专用票据\u3000\u3000\u3000③", font2, Brushes.Black, 165f, 510f);
            graphics.DrawLine(Pens.Black, 170, 540, 770, 540);
            graphics.DrawString(string.Concat(new string[]
				{
					"制单人：",
					loginUser,
					"      制单时间：",
					DateTime.Now.ToString(),
					"                ",text2
				}), font3, Brushes.Black, 168f, 545f);
            graphics.DrawString("❸ 财务记账", font, Brushes.Yellow, 385f, 555f, stringFormat);
            graphics.DrawString("报修时间：" + str, font3, Brushes.Black, 10f, 570f);
            graphics.DrawString("客户信息：" + str2, font3, Brushes.Black, 10f, 590f);
            graphics.DrawString("故障信息：" + text4, font3, Brushes.Black, 10f, 610f);
            graphics.DrawString("实际故障：□面板或模块故障   □讯井或交换机故障    □墙线或接触故障   □用户自身原因   □无", font3, Brushes.Black, 10f, 635f);
            graphics.DrawString("处理方式：□维修   □不维修   □更换\u3000__________________________________\u3000\u3000\u3000\u3000维修结果：□成功    □失败", font3, Brushes.Black, 10f, 670f);
            graphics.DrawString("维修耗材：水晶头__个  模块__个  面板__个  网线__米", font3, Brushes.Black, 10f, 700f);
            graphics.DrawString("收费情况：□未收费   □基本5元,材料________\u3000\u3000满意调查：□非常满意    □满意    □一般    □不满意", font3, Brushes.Black, 10f, 730f);
            graphics.DrawString(string.Concat(new string[]
				{
					"维修员签字：________________  ",s,s," 客户签字：_________________"
				}), font3, Brushes.Black, 10f, 760f);
            graphics.DrawString(string.Concat(new string[]
				{
					
					"裁剪线",
                    s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线",
					s,
					"裁剪线"
				}), font3, Brushes.Orange, 10f, 800f);
            graphics.DrawLine(Pens.Orange, 0, 810, 780, 810);
            graphics.DrawImage(this.PBLOGO.Image, 10, 830, 150, 65);
            //graphics.DrawString("CLOVER", font5, Brushes.Black, 7f, 840f);
            graphics.DrawString("四叶草网络技术公司\u3000BPO-公寓网络维护 专用票据\u3000\u3000\u3000④", font2, Brushes.Black, 165f, 830f);
            graphics.DrawLine(Pens.Black, 170, 860, 770, 860);
            graphics.DrawString(string.Concat(new string[]
				{
					"制单人：",
					loginUser,
					"      制单时间：",
					DateTime.Now.ToString(),
					"                ",text2
				}), font3, Brushes.Black, 168f, 865f);
            graphics.DrawString("❹ 客户留存", font, Brushes.Yellow, 385f, 875f, stringFormat);
            graphics.DrawString("报修时间：" + str, font3, Brushes.Black, 10f, 890f);
            graphics.DrawString("客户信息：" + str2, font3, Brushes.Black, 10f, 910f);
            graphics.DrawString("故障信息：" + text4, font3, Brushes.Black, 10f, 930f);
            graphics.DrawString("实际故障：□面板或模块故障   □讯井或交换机故障    □墙线或接触故障   □用户自身原因   □无", font3, Brushes.Black, 10f, 960f);
            graphics.DrawString("处理方式：□维修   □无   □更换\u3000__________________________________\u3000\u3000\u3000\u3000维修结果：□成功    □失败", font3, Brushes.Black, 10f, 990f);
            graphics.DrawString("服务收费：□收费5元 （因用户所至的故障维修成功后收取），材料费____________", font3, Brushes.Black, 10f, 1020f);
            graphics.DrawString("\u3000\u3000\u3000\u3000\u3000□免费服务（非用户原因的故障，如讯井间或交换机故障）", font3, Brushes.Black, 10f, 1050f);
            graphics.DrawString(string.Concat(new string[]
				{
					"维修时间：________________  ",s,s," 维修员签字：_________________"
				}), font3, Brushes.Black, 10f, 1080f);
            graphics.DrawString("*请将此联保留3天，3天内出现非用户原因造成的故障凭此联免费维修，盖章生效，最终解释权归本公司所有", font4, Brushes.Black, 5f, 1115f);
            this.PB.Image = image;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (myuser.bm == "Teacher" || myuser.bm == "CEO" || myuser.bm == "CTO" || myuser.bm == "CTOA")
            {
                if (MessageBox.Show("确认接单并打印？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    string[] args = new string[9];
                    args[0] = "jd";
                    args[1] = myuser.ErrInfo;
                    args[2] = No;
                    args[3] = myuser.name;
                    args[4] = "";
                    args[5] = "";
                    args[6] = "";
                    args[7] = "";
                    object result = myuser.InvokeWebService(myuser.ServerPage, "GuanliRepair", args);
                    if ("接单成功" == result.ToString())
                    {
                        PrintDocument printDocument = new PrintDocument();
                        printDocument.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
                        printDocument.Print();
                        Bitmap bmp = new Bitmap(this.PB.Image, 780, 1140);
                        string fileCapturePath = "C:\\clover\\";
                        if (!Directory.Exists(fileCapturePath))
                            Directory.CreateDirectory(fileCapturePath);
                        bmp.Save("C:\\clover\\" + No + ".bmp");
                        myuser.shuaxin = "yes";
                    }
                    else
                    {
                        MessageBox.Show(result.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("你无权结单，NOT IS CEO CTO CTOA");
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(this.PB.Image, 0, 0, this.PB.Image.Width, checked(this.PB.Image.Height - 25));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "二次打印")
            {
                button2.Text = "首次打印";
                initializeBMP2();
            }
            else if (button2.Text == "首次打印")
            {
                button2.Text = "二次打印";
                initializeBMP();
            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }
    }
}
