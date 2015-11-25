using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace BX
{
    public partial class Login : Form
    {
        public Login()
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (gh.Text == "")
            {
                MessageBox.Show("请输入用户名");
            }
            else if (mm.Text == "")
            {
                MessageBox.Show("请输入密码");
            }
            else
            {
                string PW =myuser.myMD5(mm.Text);
                ShowIP();
                try
                {
                    string[] args = new string[8];
                    args[0] = myuser.ErrInfo;
                    args[1] = gh.Text.Trim();
                    args[2] = PW;
                    args[3] = Dns.GetHostName();
                    args[4] = myuser.ipv4;
                    args[5] = myuser.mac;
                    args[6]=Environment.OSVersion.ToString();
                    args[7] = myuser.ClientVersion;
                   // MessageBox.Show(args[0]+args[1]+args[2]+args[3]+args[4]+args[5]+args[6]+args[7]);
                    object result = myuser.InvokeWebService(myuser.ServerPage, "Login", args);
                    string str = result.ToString();
                    string[] s = str.Split(new char[] { ',' });
                    if (s[0] == "False")
                    {
                        mm.Clear();
                        if (s[3] == "客户端校验失败，需要升级")
                        {
                            string id = Process.GetCurrentProcess().Id.ToString();
                            string path = System.IO.Path.GetTempPath().ToString() + "NSUBXUPDT.exe";
                            string sourceFileName = Process.GetCurrentProcess().MainModule.FileName;
                            string lstext = System.IO.Path.GetTempPath().ToString() + "报修.exe";

                            if (MessageBox.Show(s[3] + "点击确定进行升级！", "升级", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                try
                                {
                                    if (DownloadFile("http://bx.aaa.nsu.edu.cn/Download/NSUBXUPDT.exe", path) == "yes" && DownloadFile("http://bx.aaa.nsu.edu.cn/Download/报修.exe", lstext)=="yes")
                                    {

                                        System.Diagnostics.Process.Start(path, id + " " + lstext + " " + sourceFileName);
                                        // this.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("升级程序失败,可能需要手动到群里下载！");
                                    }
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("升级程序失败,可能需要手动到群里下载！");
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show(s[3]);
                        }

                    }
                    else if (s[0] == "True")
                    {
                        myuser.name = s[1];
                        myuser.bm = s[2];
                        myuser.KeepMd5 = s[4];
                        myuser.username = gh.Text.Trim();
                        if (checkBox1.Checked)
                        { try
                            {
                                RegistryKey location = Registry.LocalMachine;
                                RegistryKey soft = location.OpenSubKey("SOFTWARE", true);//可写 
                                RegistryKey myPass = soft.CreateSubKey("FTLiang");
                                myPass.SetValue("UserName", gh.Text);
                            }
                            catch(Exception)
                            {
                            }
                        }
                        
                        this.Close();
                      //  MessageBox.Show("ok");
                        this.DialogResult = DialogResult.OK;

                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("连接服务器超时.......");
                }
            }
        }
        public void ShowIP()
        {
            try
            {
                string HostName = Dns.GetHostName();
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        if ("10" == IpEntry.AddressList[i].ToString().Substring(0, 2) && myuser.ipv4 == null)
                        {
                            myuser.ipv4 = IpEntry.AddressList[i].ToString();
                        }
                        else if ("10" == IpEntry.AddressList[i].ToString().Substring(0, 2) && myuser.ipv4 != null)
                        {
                            myuser.ipv4 = myuser.ipv4 + "," + IpEntry.AddressList[i].ToString();
                        }
                    }
                }
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            checkBox1.Checked = true;
            ManagementClass mc;
            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["IPEnabled"].ToString() == "True")
                {
                    myuser.mac = mo["MacAddress"].ToString();
                }
            }
            try
            {
                RegistryKey location = Registry.LocalMachine;
                RegistryKey soft = location.OpenSubKey("SOFTWARE", false);//可写 
                RegistryKey myPass = soft.OpenSubKey("FTLiang", false);
                gh.Text = myPass.GetValue("UserName").ToString();
                //mm.Text = myPass.GetValue("PassWord").ToString();
                if (gh.Text != "")
                {
                    ActiveControl = mm;
                }
            }
            catch
            {
                //todo something
            }
        }

        public string DownloadFile(string URL, string filename)
        {
            float percent = 0;
            string ok = "no";
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
               
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                   
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                  //  System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                    ok = "yes";
                }
                so.Close();
                st.Close();

            }
            catch (System.Exception)
            {
                throw;
            }
            return ok;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            LinearGradientBrush myBrush = new LinearGradientBrush(this.ClientRectangle, Color.MediumTurquoise, Color.LightYellow, LinearGradientMode.Vertical);
            g.FillRectangle(myBrush, this.ClientRectangle);
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            gh.Clear();
            mm.Clear();
            gh.Focus();
        }
    }
}
