using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace repairasmx
{
    /// <summary>
    /// Default 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Default : System.Web.Services.WebService
    {
        public static string consqlserver = "server=127.0.0.1;database=NSUBXDB;User ID=bx;Password=clover2014bx;Pooling=true;Max Pool Size=40000;Min Pool Size=0";
        public static string ErrInfo = null;

        //获取errinfo
        public void LookErrinfo()
        {
             string sql = "select * from BXDB_Control where 变量='Errinfo'";

            //定义SQL Server连接对象 打开数据库
            SqlConnection con = new SqlConnection(consqlserver);

            con.Open();

            //定义查询命令:表示对数据库执行一个SQL语句或存储过程
            SqlCommand com = new SqlCommand(sql, con);

            //执行查询:提供一种读取数据库行的方式
            SqlDataReader sread = com.ExecuteReader();

            try
            {
                //如果存在用户名和密码正确数据执行进入系统操作
                if (sread.Read())
                {
                    ErrInfo = sread.GetString(2).Trim();
                    con.Close();                    //关闭连接
                }
            }
            catch(Exception)
            {
                con.Close();                    //关闭连接    
            }
        }

        [WebMethod]//测试
        public string Test()
        {
            return "Hello World";
        }

        [WebMethod]//登录
        public string Login(string errinfo, string UserName, string PassWord, string PCName, string IP, string MAC,string Osver,string ClientVersion)
        {
            LookErrinfo();
            string Name, Dept, result = "False,null,null,客户端校验失败，需要升级,null";
            if (errinfo == ErrInfo)
            {
                string sql = "select * from BXDB_Control where 变量='ClientVer'";

                //定义SQL Server连接对象 打开数据库
                SqlConnection con = new SqlConnection(consqlserver);

                con.Open();

                //定义查询命令:表示对数据库执行一个SQL语句或存储过程
                SqlCommand com = new SqlCommand(sql, con);

                //执行查询:提供一种读取数据库行的方式
                SqlDataReader sread = com.ExecuteReader();

                try
                {
                    //如果存在用户名和密码正确数据执行进入系统操作
                    if (sread.Read())
                    {
                        if (sread.GetString(2).Trim() == ClientVersion)
                        {
                            con.Close();                    //关闭连接
                          
                            //定义SQL查询语句:用户名 密码
                            string sql1 = "select * from BXDB_Admin where 用户名='" + UserName + "' and 密码='" + PassWord + "'";

                            //定义SQL Server连接对象 打开数据库
                            SqlConnection con1 = new SqlConnection(consqlserver);

                            con1.Open();

                            //定义查询命令:表示对数据库执行一个SQL语句或存储过程
                            SqlCommand com1 = new SqlCommand(sql1, con1);

                            //执行查询:提供一种读取数据库行的方式
                            SqlDataReader sread1 = com1.ExecuteReader();

                            try
                            {
                                //如果存在用户名和密码正确数据执行进入系统操作
                                if (sread1.Read())
                                {
                                    string IPV4 = this.Context.Request.UserHostAddress;
                                    Name = sread1.GetString(2).Trim();
                                    Dept = sread1.GetString(4).Trim();    
                                    con1.Close();
                                    string tianjia = "INSERT INTO BXDB_Loginlog (用户名,登录时间,IP地址,计算机名,MAC,OS版本,ClientVer) VALUES('" + UserName+"','"+DateTime.Now.ToString()+ "','" + IPV4+ "','" + PCName + "','" + MAC+ "','" + Osver + "','" + ClientVersion+"')";
                                    SqlConnection myCon = new SqlConnection(consqlserver);
                                    //打开数据库
                                    myCon.Open();
                                    //检索数据是否添加成功
                                    SqlCommand MyCom = new SqlCommand();
                                    MyCom.Connection = myCon;
                                    MyCom.CommandType = CommandType.Text;
                                    try
                                    {
                                        MyCom.CommandText = tianjia;
                                        int j = MyCom.ExecuteNonQuery();
                                        myCon.Close();
                                        string del = "delete from  BXDB_Keep where 用户名='" + UserName + "'";
                                        SqlConnection myCon2 = new SqlConnection(consqlserver);
                                        myCon2.Open();
                                        SqlCommand MyCom2 = new SqlCommand();
                                        MyCom2.Connection = myCon2;
                                        MyCom2.CommandType = CommandType.Text;
                                        //检索是否删除成功
                                        MyCom2.CommandText = del;
                                        MyCom2.ExecuteNonQuery();
                                        myCon2.Close();
                                        string KeepMd5 = MyModule.myMD5(DateTime.Now.ToString());
                                        string tianjia1 = "INSERT INTO  BXDB_Keep (用户名,IP地址,KeepMd5,时间) VALUES('" + UserName + "','" + IPV4 + "','" + KeepMd5+ "','" + DateTime.Now.ToString() + "')";
                                        SqlConnection myCon1 = new SqlConnection(consqlserver);
                                        //打开数据库
                                        myCon1.Open();
                                        //检索数据是否添加成功
                                        SqlCommand MyCom1 = new SqlCommand();
                                        MyCom1.Connection = myCon1;
                                        MyCom1.CommandType = CommandType.Text;
                                        MyCom1.CommandText = tianjia1;
                                        int i= MyCom1.ExecuteNonQuery();
                                        myCon1.Close();    
                                        if (j >= 1&&i>=1)
                                        {
                                            result = "True," + Name + "," + Dept + ",null," + KeepMd5 + "";
                                        }
                                        else
                                        {
                                            result = "False,null,null,服务器开小差了,null";
                                        }
                                        myCon.Close();
                                    }
                                    catch
                                    {
                                        result = "False,null,null,服务器开小差了,null";
                                        myCon.Close();
                                    }

                                }
                                else
                                {
                                    result = "False,null,null,用户名或密码错误,null";
                                    con1.Close(); 

                                }
                            }
                            catch (Exception)
                            {
                                result = "False,null,null,服务器开小差了,null";
                            }
                        }
                        else
                        {
                            result = "False,null,null,客户端校验失败，需要升级,null";
                        }
                    }
                    else
                    {
                        result = "False,null,null,客户端校验失败，需要升级,null";

                    }
                }
                catch (Exception)
                {
                }
            }
            return result;

        }

        [WebMethod]//读取登录日志
        public DataSet LoginLog(string errinfo, string UserName)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            string sql = "select TOP 1000 ID,用户名,登录时间,计算机名,IP地址, MAC,OS版本,ClientVer from BXDB_LoginLog where 用户名 ='" + UserName + "' order by 登录时间 desc";
            sqlconn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "LoginLog");
            }
            else
            {
                da.Fill(ds, null);
            }
            sqlconn.Close();
            return ds;

        }

        [WebMethod]//保持在线
        public string KeepSession(string keepmd5,string UserName)
        {
            string result = "False";
            string IPV4 = this.Context.Request.UserHostAddress;
            string sql = "select top 1 * from BXDB_Keep where 用户名='"+UserName+"' order by ID desc";

            //定义SQL Server连接对象 打开数据库
            SqlConnection con = new SqlConnection(consqlserver);
            con.Open();
            //定义查询命令:表示对数据库执行一个SQL语句或存储过程
            SqlCommand com = new SqlCommand(sql, con);
            //执行查询:提供一种读取数据库行的方式
            SqlDataReader sread = com.ExecuteReader();
            try
            {
                //如果存在用户名和密码正确数据执行进入系统操作
                if (sread.Read())
                {
                    string KeepMd5 = keepmd5;
                    string md5=sread.GetString(3).Trim();
                    if (MyModule.myMD5(md5) == keepmd5)
                    {
                        SqlConnection Mycon = new SqlConnection(consqlserver);
                        Mycon.Open();
                        SqlCommand Mycom = new SqlCommand();
                        Mycom.Connection = Mycon;
                        Mycom.CommandType = CommandType.Text;
                        string xg = "update BXDB_Keep set KeepMd5='" + keepmd5 + "',时间='" + DateTime.Now.ToString() + "' where KeepMd5='" + md5 + "'";
                        Mycom.CommandText = xg;
                        int i = Mycom.ExecuteNonQuery();
                        if (i > 0)
                        {
                            result = "True";
                        }
                        else
                            result = "False";
                    }
                    else
                        result = "False";
                }
   
            }catch(Exception)
            {
                result = "False";
            }
           return result;
        }

       [WebMethod]//读取报修数据
        public DataSet ReaderRepair(string number, string errinfo)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            //all
            string sql1 = "SELECT TOP 1000 ID,学号,姓名,班级,电话,寝室,用户类型 类型,报修故障,报修时间,接单时间,制单人,实际问题,维修方式,维修结果,结单时间,结单人 FROM  BXDB_Net ORDER BY 结单时间, 接单时间,ID";
            //完结
            string sql2 = "SELECT TOP 1000 ID,学号,姓名,班级,电话,寝室,用户类型 类型,报修故障,报修时间,接单时间,制单人,实际问题,维修方式,维修结果,结单时间,结单人 FROM  BXDB_Net where 结单时间 is not null ORDER BY 结单时间, 接单时间,ID";
            //未接单
            string sql3 = "SELECT TOP 1000 ID,学号,姓名,班级,电话,寝室,用户类型 类型,报修故障,报修时间,接单时间,制单人,实际问题,维修方式,维修结果,结单时间,结单人 FROM  BXDB_Net where 接单时间 is null ORDER BY 报修时间";
            //未结单
            string sql4 = "SELECT TOP 1000 ID,学号,姓名,班级,电话,寝室,用户类型 类型,报修故障,报修时间,接单时间,制单人,实际问题,维修方式,维修结果,结单时间,结单人 FROM  BXDB_Net where 结单时间 is null and 接单时间 is not null ORDER BY 接单时间";
            string sql = sql1;
            if (number == "1")
            {
                sql = sql1;
            }
            else if (number == "2")
            {
                sql = sql2;
            }
            else if (number == "3")
            {
                sql = sql3;
            }
            else if (number == "4")
            {
                sql = sql4;
            }
            sqlconn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "net");

            }
            else
            {
                da.Fill(ds, null);

            }
            return ds;

        }

        [WebMethod]//检索报修数据
        public DataSet SearchRepair(string xx, string tj, string errinfo)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            //all
            string sql = "SELECT TOP 1000 ID,学号,姓名,班级,电话,寝室,用户类型 类型,报修故障,报修时间,接单时间,制单人,实际问题,维修方式,维修结果,结单时间,结单人 FROM  BXDB_Net where " + xx + " like '%" + tj + "%' ORDER BY ID desc";
            //完结
            sqlconn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "net");

            }
            else
            {
                da.Fill(ds, null);

            }
            return ds;

        }

        [WebMethod]//获取报修web状态
        public string ZhuangTai(string errinfo)
        {
            LookErrinfo();
            string serverstatus = "未知状态";
            //定义SQL查询语句:用户名 密码
            if (errinfo == ErrInfo)
            {
                string sql = "select * from BXDB_Control where 变量='Bxweb'";

                //定义SQL Server连接对象 打开数据库
                SqlConnection con = new SqlConnection(consqlserver);
                con.Open();

                //定义查询命令:表示对数据库执行一个SQL语句或存储过程
                SqlCommand com = new SqlCommand(sql, con);

                //执行查询:提供一种读取数据库行的方式
                SqlDataReader sread = com.ExecuteReader();


                if (sread.Read())
                {
                    serverstatus = sread.GetString(2).Trim();

                }
                else
                {
                    serverstatus = "获取状态出错";
                    
                }
                con.Close(); 
            }
            return serverstatus;
        }

        [WebMethod]//管理报修客户端状态
        public string GuanliZt(string onoff, string errinfo)
        {
            LookErrinfo();
            string resulet = "无权操作";
            if (errinfo == ErrInfo)
            {
                SqlConnection Mycon = new SqlConnection(consqlserver);
                Mycon.Open();
                SqlCommand Mycom = new SqlCommand();
                Mycom.Connection = Mycon;
                Mycom.CommandType = CommandType.Text;
                string xg = "update BXDB_Control set 值='" + onoff + "' where 变量='Bxweb'";
                Mycom.CommandText = xg;
                int i = Mycom.ExecuteNonQuery();
                if (i > 0)
                {
                    if (onoff == "on")
                    {
                        resulet = "报修系统开启成功";
                        Mycon.Close(); 
                    }
                    else if (onoff == "off")
                    {
                        resulet = "报修系统关闭成功";
                    }
                }
                else
                {
                    resulet = "操作失败！";

                }
                Mycon.Close(); 
            }
            
            return resulet;
        }

        [WebMethod]//管理报修黑名单
        public string GuanliBlack(string errinfo, string XueHao, string Name, string Reason, string AddName,string caozuo)
        {
            LookErrinfo();
            string resulet = "无权操作";
            if (errinfo == ErrInfo)
            {
                if (caozuo == "add")
                {
                    string SQLStr1 = "select * from BXDB_Blacklist where 学号='" + XueHao + "'";
                    SqlConnection myCon1 = new SqlConnection(consqlserver);
                    //打开数据库
                    myCon1.Open();
                    SqlCommand MyCom1 = new SqlCommand();
                    MyCom1.Connection = myCon1;
                    MyCom1.CommandType = CommandType.Text;
                    MyCom1.CommandText = SQLStr1;
                    SqlDataReader dr = MyCom1.ExecuteReader();
                    if (dr.Read())
                    {
                        resulet = "此学号已在黑名单存在！";
                    }
                    else
                    {
                        //如果没有空项，则建立数据库连接
                        //向相应的数据库添加数据
                        string tianjia = "INSERT INTO BXDB_Blacklist (学号,姓名,原因, 添加时间,添加人) VALUES('" + XueHao+"','"+Name+"','"+Reason+ "','" + DateTime.Now.ToString() + "','" + AddName+"')";
                        SqlConnection myCon = new SqlConnection(consqlserver);
                        //打开数据库
                        myCon.Open();
                        //检索数据是否添加成功
                        SqlCommand MyCom = new SqlCommand();
                        MyCom.Connection = myCon;
                        MyCom.CommandType = CommandType.Text;
                        try
                        {
                            MyCom.CommandText = tianjia;

                            int j = MyCom.ExecuteNonQuery();

                            if (j >= 1)
                            {
                                resulet = "黑名单添加成功";
                            }
                        }
                        catch
                        {
                            resulet = "黑名单添加失败";
                        }
                        myCon.Close();
                    }
                }
                else if (caozuo == "del")
                {
                    string gonghao = "delete from BXDB_Blacklist where 学号='" + XueHao+ "'";
                    SqlConnection myCon = new SqlConnection(consqlserver);
                    myCon.Open();
                    SqlCommand MyCom = new SqlCommand();
                    MyCom.Connection = myCon;
                    MyCom.CommandType = CommandType.Text;

                    //检索是否删除成功
                    try
                    {
                        MyCom.CommandText = gonghao;

                        int j = MyCom.ExecuteNonQuery();

                        if (j >= 1)
                        {
                            resulet = "黑名单删除成功";
                        }
                    }
                    catch (Exception )
                    {
                        resulet = "黑名单删除失败";
                    }
                    myCon.Close();
                }
            }
                return resulet;
        }

        [WebMethod]//读取报修黑名单数据
        public DataSet ReaderBlack(string number, string errinfo,string xuehao)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            //all
            string sql1 = "select ID,学号,姓名,原因, 添加时间,添加人 from BXDB_Blacklist where 学号 is not null";
            //搜索学号
            string sql2 = "select ID,学号,姓名,原因, 添加时间,添加人 from BXDB_Blacklist where 学号 ='" + xuehao+"'";

            string sql = sql1;
            if (number == "1")
            {
                sql = sql1;
            }
            else if (number == "2")
            {
                sql = sql2;
            }
           
            sqlconn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "blacklist");

            }
            else
            {
                da.Fill(ds, null);

            }
            sqlconn.Close();
            return ds;

        }

        [WebMethod]//操作报修数据
        public string GuanliRepair(string caozuo, string errinfo, string ID, string name,string rproblem,string wxfangshi,string wxname,string wxresult,string wxfee)
        {
            LookErrinfo();
            string IPV4 = this.Context.Request.UserHostAddress;
            string[] sip = IPV4.Split(new char[] { '.' });
            string ip = sip[0] + "." + sip[1] + ".*.*";
            string resulet = "无权操作";
            string cz = null;
            if (errinfo == ErrInfo)
            {
                if ("10.132.*.*" == ip)
                {
                    SqlConnection Mycon = new SqlConnection(consqlserver);
                    Mycon.Open();
                    SqlCommand Mycom = new SqlCommand();
                    Mycom.Connection = Mycon;
                    Mycom.CommandType = CommandType.Text;
                    string xg = null;
                    if (caozuo == "judan")
                    {
                        xg = "update BXDB_Net set 接单时间='" + DateTime.Now.ToString() + "',制单时间='" + DateTime.Now.ToString() + "',制单人='" + name + "',实际问题='NONE',维修方式='不维修',维修人='NONE',维修结果='拒单',维修费='0',结单时间='" + DateTime.Now.ToString() + "',结单人='" + name + "' where ID='" + ID + "'";
                        cz = "拒单";
                    }
                    else if (caozuo == "jd")
                    {
                        xg = "update BXDB_Net set 接单时间='" + DateTime.Now.ToString() + "',制单时间='" + DateTime.Now.ToString() + "',制单人='" + name + "' where ID='" + ID + "'";
                        cz = "接单";
                    }
                    else if (caozuo == "jiedan")
                    {
                        xg = "update BXDB_Net set 实际问题='" + rproblem + "',维修方式='" + wxfangshi + "',维修人='" + wxname + "',维修结果='" + wxresult + "',维修费='" + wxfee + "',结单时间='" + DateTime.Now.ToString() + "',结单人='" + name + "' where ID='" + ID + "'";
                        cz = "结单";
                    }
                    Mycom.CommandText = xg;

                    //检索修改是否成功
                    int i = Mycom.ExecuteNonQuery();
                    if (i > 0)
                    {
                        resulet = cz + "成功";
                    }
                    else
                    {
                        resulet = cz + "失败";
                    }
                    Mycon.Close();
                }
                else
                {
                    resulet = cz + "失败,必须在办公室才能打印接单";
                }
            }
            
            return resulet;
        }

        [WebMethod]//修改密码
        public string ChangePassword(string errinfo, string UserName,  string OPassWord, string NPassWord)
        {
            LookErrinfo();
            string result = "修改密码失败！";
            if (errinfo == ErrInfo)
            {
                //定义SQL查询语句:用户名 密码
                string sql1 = "select * from BXDB_Admin where 用户名='" + UserName + "'";

                //定义SQL Server连接对象 打开数据库
                SqlConnection con1 = new SqlConnection(consqlserver);

                con1.Open();

                //定义查询命令:表示对数据库执行一个SQL语句或存储过程
                SqlCommand com1 = new SqlCommand(sql1, con1);

                //执行查询:提供一种读取数据库行的方式
                SqlDataReader sread1 = com1.ExecuteReader();

                try
                {
                    //如果存在用户名和密码正确数据执行进入系统操作
                    if (sread1.Read())
                    {
                        if (sread1.GetString(3).Trim() == OPassWord)
                        {
                            con1.Close();
                            SqlConnection Mycon = new SqlConnection(consqlserver);
                            Mycon.Open();
                            SqlCommand Mycom = new SqlCommand();
                            Mycom.Connection = Mycon;
                            Mycom.CommandType = CommandType.Text;
                            string xg = null;
                            xg = "update BXDB_Admin set 密码='" + NPassWord + "' ,上次登录='" + DateTime.Now.ToString() + "' where 用户名='" + UserName + "'";
                            Mycom.CommandText = xg;

                            //检索修改是否成功
                            int i = Mycom.ExecuteNonQuery();
                            if (i > 0)
                            {
                                result = "修改密码成功！";
                            }
                            else
                            {
                                result = "修改密码失败！";
                            }
                            Mycon.Close();
                        }
                        else
                        {
                            result = "原密码错误！";
                            con1.Close();
                        }
                    }
                }
                catch (Exception)
                {
                    result = "服务器开小差了！";
                }
               
            }
            else
            {
                result = "客户端版本不符,修改密码失败！";
            }

            return result;
        }

        [WebMethod]//读取用户数据
        public DataSet ReaderLogin(string errinfo, string Dept)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            string sql1 = "select ID,用户名,姓名,密码,职位,上次登录 from BXDB_Admin";
            string sql = sql1;
            sqlconn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (Dept== "Teacher"&& errinfo == ErrInfo)
            {  
                da.Fill(ds, "admin");
            }
            else
                da.Fill(ds, null);
            sqlconn.Close();
            return ds;
        }

        [WebMethod]//修改密码
        public string GLChangePassword(string errinfo, string Dept, string UserName, string PassWord)
        {
            LookErrinfo();
            string result = "修改密码失败！";
            if (Dept == "Teacher" && errinfo == ErrInfo)
            {
                SqlConnection Mycon = new SqlConnection(consqlserver);
                Mycon.Open();
                SqlCommand Mycom = new SqlCommand();
                Mycom.Connection = Mycon;
                Mycom.CommandType = CommandType.Text;
                string xg = null;
                xg = "update BXDB_Admin set 密码='" + PassWord + "' ,上次登录='" + DateTime.Now.ToString() + "' where 用户名='" + UserName + "'";
                Mycom.CommandText = xg;

                //检索修改是否成功
                int i = Mycom.ExecuteNonQuery();
                if (i > 0)
                {
                    result = "修改密码成功！";
                }
                else
                {
                    result = "修改密码失败！";
                }
                Mycon.Close();
            }
            else
            {
                result = "客户端版本不符,修改密码失败！";
            }

            return result;
        }

        [WebMethod]//修改资料
        public string GLUserInfo(string errinfo, string Dept, string UserName, string TrueName, string UserDept)
        {
            LookErrinfo();
            string result = "修改资料失败！";
            if (Dept == "Teacher" && errinfo == ErrInfo)
            {
                SqlConnection Mycon = new SqlConnection(consqlserver);
                Mycon.Open();
                SqlCommand Mycom = new SqlCommand();
                Mycom.Connection = Mycon;
                Mycom.CommandType = CommandType.Text;
                string xg = null;
                xg = "update BXDB_Admin set 姓名='" + TrueName + "' ,职位='" + UserDept + "' ,上次登录='" + DateTime.Now.ToString() + "' where 用户名='" + UserName + "'";
                Mycom.CommandText = xg;

                //检索修改是否成功
                int i = Mycom.ExecuteNonQuery();
                if (i > 0)
                {
                    result = "修改资料成功！";
                }
                else
                {
                    result = "修改资料失败！";
                }
                Mycon.Close();
            }
            else
            {
                result = "修改资料失败！";
            }

            return result;
        }

        [WebMethod]//新加用户资料
        public string AddUserInfo(string errinfo, string Dept, string UserName, string TrueName,string PassWord, string UserDept)
        {
            LookErrinfo();
            string result = "新用户添加失败！";
            if (Dept == "Teacher" && errinfo == ErrInfo)
            {
                string SQLStr1 = "select * from  BXDB_Admin where 用户名='" + UserName + "'";
                SqlConnection myCon1 = new SqlConnection(consqlserver);
                //打开数据库
                myCon1.Open();
                SqlCommand MyCom1 = new SqlCommand();
                MyCom1.Connection = myCon1;
                MyCom1.CommandType = CommandType.Text;
                MyCom1.CommandText = SQLStr1;
                SqlDataReader dr = MyCom1.ExecuteReader();
                if (dr.Read())
                {
                    result = "此用户名已存在！";
                }
                else
                {
                    //如果没有空项，则建立数据库连接
                    //向相应的数据库添加数据
                    string tianjia = "INSERT INTO BXDB_Admin (用户名,姓名,密码,职位,上次登录) VALUES('" + UserName + "','" + TrueName + "','" + PassWord + "','" + UserDept + "','" + DateTime.Now.ToString() + "')";
                    SqlConnection myCon = new SqlConnection(consqlserver);
                    //打开数据库
                    myCon.Open();
                    //检索数据是否添加成功
                    SqlCommand MyCom = new SqlCommand();
                    MyCom.Connection = myCon;
                    MyCom.CommandType = CommandType.Text;
                    try
                    {
                        MyCom.CommandText = tianjia;

                        int j = MyCom.ExecuteNonQuery();

                        if (j >= 1)
                        {
                            result = "新用户添加成功！";
                        }
                    }
                    catch
                    {
                        result = "新用户添加失败！";
                    }
                    myCon.Close();
                }
            }
            return result;
        }

        [WebMethod]//删除用户
        public string DelUser(string errinfo ,string Dept,string UserName)
        {
            LookErrinfo();
            string result = "用户删除失败！";
            if (Dept == "Teacher" && errinfo == ErrInfo)
            {
                string gonghao = "delete from BXDB_Admin where 用户名='" + UserName + "'";
                SqlConnection myCon = new SqlConnection(consqlserver);
                myCon.Open();
                SqlCommand MyCom = new SqlCommand();
                MyCom.Connection = myCon;
                MyCom.CommandType = CommandType.Text;

                //检索是否删除成功
                try
                {
                    MyCom.CommandText = gonghao;

                    int j = MyCom.ExecuteNonQuery();

                    if (j >= 1)
                    {
                        result = "用户删除成功！";
                    }
                }
                catch (Exception)
                {
                    result = "用户删除失败！";
                }
                myCon.Close();
            }
            return result;
        }


        [WebMethod]//管理报修黑名单
        public string GuanliVip(string errinfo, string XueHao, string Name, string AddName, string caozuo)
        {
            LookErrinfo();
            string resulet = "无权操作";
            if (errinfo == ErrInfo)
            {
                if (caozuo == "add")
                {
                    string SQLStr1 = "select * from BXDB_Vip where 学号='" + XueHao + "'";
                    SqlConnection myCon1 = new SqlConnection(consqlserver);
                    //打开数据库
                    myCon1.Open();
                    SqlCommand MyCom1 = new SqlCommand();
                    MyCom1.Connection = myCon1;
                    MyCom1.CommandType = CommandType.Text;
                    MyCom1.CommandText = SQLStr1;
                    SqlDataReader dr = MyCom1.ExecuteReader();
                    if (dr.Read())
                    {
                        resulet = "此学号已在VIP名单存在！";
                    }
                    else
                    {
                        //如果没有空项，则建立数据库连接
                        //向相应的数据库添加数据
                        string tianjia = "INSERT INTO BXDB_Vip (学号,姓名,添加时间,添加人) VALUES('" + XueHao + "','" + Name + "','" + DateTime.Now.ToString() + "','" + AddName + "')";
                        SqlConnection myCon = new SqlConnection(consqlserver);
                        //打开数据库
                        myCon.Open();
                        //检索数据是否添加成功
                        SqlCommand MyCom = new SqlCommand();
                        MyCom.Connection = myCon;
                        MyCom.CommandType = CommandType.Text;
                        try
                        {
                            MyCom.CommandText = tianjia;

                            int j = MyCom.ExecuteNonQuery();

                            if (j >= 1)
                            {
                                resulet = "VIP添加成功";
                            }
                        }
                        catch
                        {
                            resulet = "VIP添加失败";
                        }
                        myCon.Close();
                    }
                }
                else if (caozuo == "del")
                {
                    string gonghao = "delete from BXDB_Vip where 学号='" + XueHao + "'";
                    SqlConnection myCon = new SqlConnection(consqlserver);
                    myCon.Open();
                    SqlCommand MyCom = new SqlCommand();
                    MyCom.Connection = myCon;
                    MyCom.CommandType = CommandType.Text;

                    //检索是否删除成功
                    try
                    {
                        MyCom.CommandText = gonghao;

                        int j = MyCom.ExecuteNonQuery();

                        if (j >= 1)
                        {
                            resulet = "VIP删除成功";
                        }
                    }
                    catch (Exception)
                    {
                        resulet = "VIP删除失败";
                    }
                    myCon.Close();
                }
            }
            return resulet;
        }

        [WebMethod]//读取报修黑名单数据
        public DataSet ReaderVip(string number, string errinfo, string xuehao)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            //all
            string sql1 = "select ID,学号,姓名, 添加时间,添加人 from BXDB_Vip where 学号 is not null";
            //搜索学号
            string sql2 = "select ID,学号,姓名, 添加时间,添加人 from BXDB_Vip where 学号 ='" + xuehao + "'";

            string sql = sql1;
            if (number == "1")
            {
                sql = sql1;
            }
            else if (number == "2")
            {
                sql = sql2;
            }

            sqlconn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "vip");

            }
            else
            {
                da.Fill(ds, null);

            }
            sqlconn.Close();
            return ds;

        }

        [WebMethod]//读取报修黑名单数据
        public DataSet ReaderPc(string number, string errinfo, string xuehao)
        {
            LookErrinfo();
            SqlConnection sqlconn = new SqlConnection(consqlserver);
            //all
            string sql1 = "select ID,学号,姓名,班级,时间,电脑品牌,维护方式,是否收费,收费,评价,维护人 from BXDB_Pc where 学号 is not null";
            //搜索学号
            string sql2 = "select ID,学号,姓名,班级,时间,电脑品牌,维护方式,是否收费,收费,评价,维护人 from BXDB_Pc where 学号 ='" + xuehao + "'";

            string sql = sql1;
            if (number == "1")
            {
                sql = sql1;
            }
            else if (number == "2")
            {
                sql = sql2;
            }

            sqlconn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, sqlconn);
            DataSet ds = new DataSet();
            if (errinfo == ErrInfo)
            {
                da.Fill(ds, "pc");

            }
            else
            {
                da.Fill(ds, null);

            }
            sqlconn.Close();
            return ds;

        }
    }
}
