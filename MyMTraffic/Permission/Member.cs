using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;

namespace MyMTraffic.Permission
{
    /// <summary>
    /// Thành viên quản trị
    /// </summary>
    public class Member
    {
        MyExecuteData mExec = new MyExecuteData();
        MyGetData mGet = new MyGetData();

        /// <summary>
        /// Mỗi lần đăng nhập thành công thì 1 key được tạo ra, và sẽ bị xóa đi khi logout
        /// </summary>
        /// <returns></returns>
        public static string MemberKeyLogined
        {
            get
            {
                try
                {
                    if (MyCurrent.CurrentPage.Session["MemberKeyLogined"] != null)
                        return MyCurrent.CurrentPage.Session["MemberKeyLogined"].ToString();
                    else
                        return string.Empty;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            set
            {
                try
                {
                    MyCurrent.CurrentPage.Session["MemberKeyLogined"] = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Token được sinh ra cho mỗi là truy cập vào trang.
        /// </summary>
        public static string Token
        {
            get
            {
                try
                {
                    if (MyCurrent.CurrentPage.Session["Token"] != null)
                        return MyCurrent.CurrentPage.Session["Token"].ToString();
                    else
                        return string.Empty;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            set
            {
                try
                {
                    MyCurrent.CurrentPage.Session["Token"] = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Gen token để kiểm tra cho mỗi lần POST từ User
        /// </summary>
        /// <returns></returns>
        public static string GenToken()
        {
            try
            {
                if (IsLogined())
                {
                    Token = MyUtility.MySecurity.AES.Encrypt(MemberID() + LoginName() + DateTime.Now.ToString("yyyyMMddHHmmssfff") + MemberKeyLogined, MySetting.AdminSetting.SpecialKey);
                    return Token;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Kiểm tra token có chính xác hay không
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static bool CheckToken(string Token_Client)
        {
            try
            {
                if (!IsLogined())
                    return true;

                if (string.IsNullOrEmpty(Token_Client))
                    return false;


                return Token.Equals(Token_Client);

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Tạo 1 Dataset mẫu
        /// </summary>
        /// <returns></returns>
        public DataSet CreateDataSet()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                DataSet mSet = mGet.GetDataSet("Sp_Member_Select", mPara, mValue);

                if (mSet != null && mSet.Tables.Count >= 1)
                {
                    mSet.DataSetName = "Parent";
                    mSet.Tables[0].TableName = "Child";
                }
                return mSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cho biết thành viên đã đăng nhập hay chưa
        /// </summary>
        /// <returns></returns>
        static public bool IsLogined()
        {
            try
            {
                string MemberID = MyCurrent.CurrentPage.Session["MemberID"] == null ? "0" : MyCurrent.CurrentPage.Session["MemberID"].ToString();
                if (int.Parse(MemberID) > 0)
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy mã nhóm thành viên của thành viên đang đăng nhập
        /// </summary>
        /// <returns></returns>
        public static int MemberGroupID()
        {
            try
            {
                string MemberGroupID = MyCurrent.CurrentPage.Session["MemberGroupID"] == null ? "0" : MyCurrent.CurrentPage.Session["MemberGroupID"].ToString();
                return int.Parse(MemberGroupID);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy mã MemberGroup của nhóm tài khoản là Amind
        /// </summary>
        /// <returns></returns>
        public static int MemberGroupID_Admin()
        {
            try
            {
                return int.Parse(MyConfig.GetKeyInConfigFile("MemberGroupID_Admin"));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Mã thành viên khi đã đăng nhập
        /// </summary>
        /// <returns></returns>
        public static int MemberID()
        {
            try
            {
                string MemberID = MyCurrent.CurrentPage.Session["MemberID"] == null ? "0" : MyCurrent.CurrentPage.Session["MemberID"].ToString();
                return int.Parse(MemberID);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Lấy LoginName của thành viên đã đăng nhập
        /// </summary>
        /// <returns></returns>
        public static string LoginName()
        {
            try
            {
                return MyCurrent.CurrentPage.Session["LoginName"] == null ? string.Empty : MyCurrent.CurrentPage.Session["LoginName"].ToString();

            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Lấy Password của thành viên theo MemberID
        /// </summary>
        /// <param name="MemberID">Mã thành viên cần lấy</param>
        /// <returns></returns>
        public string Password(int MemberID)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { "3", MemberID.ToString() };
                return (mGet.GetExecuteScalar("Sp_Member_Select", mPara, mValue).ToString());

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }



        public static bool Login(string LoginName, string Password, bool IsEncrypt, bool IsSaveSession, bool IsSaveCookie, ref string AlertMessage)
        {
            try
            {
                MyCurrent.CurrentPage.Session["Menu_1"] = null;
                MyCurrent.CurrentPage.Session["Menu_2"] = null;
                MyGetData mGetData = new MyGetData();
                if (!IsEncrypt)
                {
                    //Mã hóa mật khẩu
                    Password = MySecurity.Encrypt_MD5(Password);
                }

                //truyền tham số vào Store
                string[] mPara = { "LoginName", "Password", };
                string[] mValue = { LoginName, Password, };
                //Thực thi Store
                DataTable mTable = mGetData.GetDataTable("Sp_Member_Login", mPara, mValue);

                if (mTable.Rows.Count > 0) //Tồn tại tài khoản
                {
                    if (!(bool)mTable.Rows[0]["IsActive"])
                    {
                        AlertMessage = "Tài khoàn này đang BỊ KHÓA, xin hãy liên hệ với Adminisatrator để được kích hoạt lại.";
                        return false;
                    }

                    AlertMessage = "Đăng nhập không thành công.";

                    MemberKeyLogined = Guid.NewGuid().ToString("N");

                    if (IsSaveSession)
                    {
                        //lưu xuống session
                        HttpContext.Current.Session["MemberGroupID"] = mTable.Rows[0]["MemberGroupID"].ToString();
                        HttpContext.Current.Session["MemberID"] = mTable.Rows[0]["MemberID"].ToString();
                        HttpContext.Current.Session["LoginName"] = LoginName;
                        HttpContext.Current.Session["PartnerID"] = mTable.Rows[0]["PartnerID"].ToString();
                        HttpContext.Current.Session["PartnerTypeID"] = mTable.Rows[0]["PartnerTypeID"].ToString();

                    }
                    if (IsSaveCookie)
                    {
                        //Lưu xuống Cookie với thời gian sống là 3 ngày
                        HttpCookie Cookie_Account;
                        if (HttpContext.Current.Request.Cookies["mb"] != null)
                            HttpContext.Current.Response.Cookies["mb"].Expires = DateTime.Now.AddDays(-1);

                        Cookie_Account = new HttpCookie("mb");

                        Cookie_Account.Expires = DateTime.Now.AddDays(3);
                        Cookie_Account.HttpOnly = true;
                        //EnCode AccountCode và Accountpassword trường khi lưu xuống Cookie
                        Cookie_Account.Values.Add("ln", MySecurity.EnCodeData(LoginName));
                        Cookie_Account.Values.Add("pw", MySecurity.EnCodeData(Password));
                        HttpContext.Current.Response.Cookies.Remove("mb");
                        HttpContext.Current.Response.Cookies.Add(Cookie_Account);
                    }
                    else
                    {
                        if (HttpContext.Current.Request.Cookies["mb"] != null)
                            HttpContext.Current.Response.Cookies["mb"].Expires = DateTime.Now.AddDays(-1);
                    }

                    return true;
                }

                //NẾU ĐĂNG NHẬP KHÔNG THÀNH CÔNG
                MemberKeyLogined = string.Empty;

                //Reset lại Session
                HttpContext.Current.Session["MemberID"] = string.Empty;
                HttpContext.Current.Session["LoginName"] = string.Empty;
                HttpContext.Current.Session["MemberGroupID"] = string.Empty;
                HttpContext.Current.Session["PartnerID"] = string.Empty;
                HttpContext.Current.Session["PartnerTypeID"] = string.Empty;

                //Nếu Cookie tồn tại thì xóa cookie đó đi
                if (HttpContext.Current.Response.Cookies["mb"] != null)
                    HttpContext.Current.Response.Cookies["mb"].Expires = DateTime.Now.AddDays(-1);
                AlertMessage = "Tài khoản này không tồn tại hoặc mật khẩu bị sai, xin hãy xem xét lại!";
                return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool LogOut()
        {
            try
            {
                MyCurrent.CurrentPage.Session["Menu_1"] = null;
                MyCurrent.CurrentPage.Session["Menu_2"] = null;
                //Kiểm tra sự tồn tại của tài khoản
                if (!Member.IsLogined())
                {
                    return false;
                }
                else
                {
                    MemberKeyLogined = string.Empty;

                    //Reset lại Session
                    HttpContext.Current.Session["MemberID"] = null;
                    HttpContext.Current.Session["LoginName"] = null;
                    HttpContext.Current.Session["MemberGroupID"] = null;

                    //Nếu Cookie tồn tại thì xóa cookie đó đi
                    if (HttpContext.Current.Response.Cookies["mb"] != null)
                        HttpContext.Current.Response.Cookies["mb"].Expires = DateTime.Now.AddDays(-1);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// lấy đối tac 
        /// </summary>
        /// <returns></returns>
        public static int PartnerID()
        {
            try
            {
                string PartnerID = MyCurrent.CurrentPage.Session["PartnerID"] == null ? "0" : MyCurrent.CurrentPage.Session["PartnerID"].ToString();
                return int.Parse(PartnerID);
            }
            catch
            {
                return 0;
            }
        }

        public static int PartnerTypeID()
        {
            try
            {
                string PartnerTypeID = MyCurrent.CurrentPage.Session["PartnerTypeID"] == null ? "0" : MyCurrent.CurrentPage.Session["PartnerTypeID"].ToString();
                return int.Parse(PartnerTypeID);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">
        /// <para>Type = 0: Lấy dữ liệu mẫu</para>
        /// <para>Type = 1: Lấy chi tiết 1 Record (Para_1 = MemberID)</para>
        /// <para>Type = 2: Lay theo nhom thanh vien</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mpara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_Member_Select", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">Type = 0</param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Update(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Member_Update", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert dữ liệu và trả về 1 table cần lấy sau khi insert
        /// </summary>
        /// <param name="Type">Type = 0 </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Insert(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };

                if (mExec.ExecProcedure("Sp_Member_Insert", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Kích hoạt thành viên, hoặc hủy kích hoạt thành viên
        /// </summary>
        /// <param name="Type">Type = 0:</param>
        /// <param name="IsActive">Trạng thái kích hoạt cần cập nhật</param>
        /// <param name="XMLContent">Danh sách các MemberID cần Kích hoạt hoặc Hủy kích  hoạt</param>
        /// <returns></returns>
        public bool Active(int Type, bool IsActive, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsActive", "XMLContent" };
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Member_Active", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public bool Delete(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Member_Delete", mpara, mValue) > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tổng số dòng
        /// </summary>
        /// <returns></returns>
        public int TotalRow(int? Type, int? MemberGroupID, string SearchContent, string OrderBy, bool? IsActive)
        {
            try
            {
                string[] mPara = { "Type", "MemberGroupID", "SearchContent", "OrderBy", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), MemberGroupID.ToString(), SearchContent, OrderBy, (IsActive == null ? null : IsActive.ToString()), true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_Member_Search", mPara, mValue);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu về phân trang
        /// </summary>
        /// <param name="Type">Type =1: Lấy dữ liệu về phân trang</param>
        /// <param name="MemberGroupID"></param>
        /// <param name="SearchContent"></param>
        /// <param name="OrderBy"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DataTable Search(int? Type, int BeginRow, int EndRow, int? MemberGroupID, string SearchContent, string OrderBy, bool? IsActive)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "MemberGroupID", "SearchContent", "OrderBy", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), MemberGroupID.ToString(), SearchContent, OrderBy, (IsActive == null ? null : IsActive.ToString()), false.ToString() };
                return mGet.GetDataTable("Sp_Member_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Thay đổi mật khẩu ngưởi dùng
        /// </summary>
        /// <param name="MemberID">MemberID của tài khoản</param>
        /// <param name="Password">Mật khẩu mới</param>
        /// <returns></returns>
        public bool ChangePassword(int MemberID, string Password)
        {
            try
            {
                string[] mpara = { "Type", "MemberID", "Password" };
                string[] mValue = { "0", MemberID.ToString(), Password.ToString() };
                if (mExec.ExecProcedure("Sp_Member_ChangePassword", mpara, mValue) > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
