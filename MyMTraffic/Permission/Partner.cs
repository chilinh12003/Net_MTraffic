using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace MyMTraffic.Permission
{
    /// <summary>
    /// Đối tác
    /// </summary>
    public class Partner
    {
        MyExecuteData mExec ;
        MyGetData mGet;

        public Partner(string sConnectionString)
        {
            mExec = new MyExecuteData(sConnectionString);
            mGet = new MyGetData(sConnectionString);
        }

        public Partner()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        /// <summary>
        /// Thể loại đối tác
        /// </summary>
        public enum PartnerType
        {
            /// <summary>
            /// Không xác định
            /// </summary>
            /// 
            [DescriptionAttribute("Không xác định")]
            Nothing = 1,
            /// <summary>
            /// Đối tác là nhà cung cấp dữ liệu
            /// </summary>
            /// 
            [DescriptionAttribute("Đối tác cung cấp")]
            Provider = 2,
            /// <summary>
            /// Đối tác lấy dữ liệu của ICom
            /// </summary>
            /// 
            [DescriptionAttribute("Đối tác lấy dữ liệu")]
            Receiver = 3,
            [DescriptionAttribute("Thêm tiền")]
            ReCharge = 4, 
        }

        /// <summary>
        /// Tạo một chuỗi xác thực cho partner để dành cho phần đăng nhập hoặc gọi WS sau này
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PasswordNotEncrypt"></param>
        /// <returns></returns>
        public string CreateValidString(string UserName, string PasswordNotEncrypt)
        {
            try
            {
                return MySecurity.Encrypt_MD5(UserName + PasswordNotEncrypt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">
        /// <para>Type = 0: đăng nhập với Full tham số truyền vào</para>
        /// <para>Type = 1: Đăng nhập với UserName và Password</para>
        /// <para>Type = 2: Đăng nhập với UserName và ValidString</para>
        /// </param>
        /// <param name="PartnerUserName"></param>
        /// <param name="PartnerPassword"></param>
        /// <param name="IsEncryptPassword"></param>
        /// <param name="ValidString"></param>
        /// <param name="mPartnerType"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool Login(int? Type, string PartnerUserName, string PartnerPassword,bool IsEncryptPassword, string ValidString,PartnerType mPartnerType, ref string Message)
        {
            try
            {
                MyGetData mGetData = new MyGetData();
                if (!IsEncryptPassword)
                {
                    //Mã hóa mật khẩu
                    PartnerPassword = MySecurity.Encrypt_MD5(PartnerPassword);

                }
                string[] mPara = { "Type", "PartnerUserName", "PartnerPassword", "ValidString" };
                string[] mValue = { Type.ToString(), PartnerUserName, PartnerPassword, ValidString };

                DataTable mTable = mGetData.GetDataTable("Sp_Partner_Login", mPara, mValue);
                if (mTable == null || mTable.Rows.Count < 1)
                    return false;

                if (mTable.Rows[0]["PartnerTypeID"] == DBNull.Value && mPartnerType == PartnerType.Nothing)
                {
                    return true;
                }
                if ((PartnerType)(int)(byte)mTable.Rows[0]["PartnerTypeID"] != mPartnerType)
                {
                    Message = "Nhóm đối tác truyền vào là không phù hợp";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Login(int? Type, string PartnerUserName, string ValidString, PartnerType mPartnerType, ref string Message)
        {
            try
            {
                return Login(2, PartnerUserName, string.Empty, true, ValidString, mPartnerType, ref Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Login(string PartnerUserName, string PartnerPassword, bool IsEncryptPassword, PartnerType mPartnerType, ref string Message)
        {
            try
            {
                return Login(1, PartnerUserName, PartnerPassword, IsEncryptPassword, string.Empty, mPartnerType, ref Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu Partner
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Lấy dữ liệu mẫu</para>
        /// <para>Type = 1: Lấy thông tin chi tiết 1 record (Para_1 = PartnerID)</para>
        /// <para>Type = 2: Lấy danh sách đối tác đã được kích hoạt (dành cho combobox)</para>
        /// <para>Type = 3: Lấy các đối tác theo PartnerType (Para_1)</para>
        ///</param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mpara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_Partner_Select", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">
        /// <para>Type = 4: Lấy tất cả Partner đang active</para>
        /// </param>
        /// <returns></returns>
        public DataTable Select(int Type)
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { Type.ToString() };

                DataTable mTable = new DataTable();

                mTable = mGet.GetDataTable("Sp_Partner_Select", mPara, mValue);

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        /// <summary>
        /// Kích hoạt đối tác, hoặc hủy kích hoạt đối tác
        /// </summary>
        /// <param name="IsActive">Trạng thái kích hoạt cần cập nhật</param>
        /// <param name="XMLContent">Danh sách các PartnerID cần Kích hoạt hoặc Hủy kích  hoạt</param>
        /// <returns></returns>
        public bool Active(int Type, bool IsActive, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsActive", "XMLContent" };
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Partner_Active", mpara, mValue) > 0)
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
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="PartnerID"></param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Delete(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Partner_Delete", mpara, mValue) > 0)
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
       /// Lấy tổng số dòng cho phân trang
       /// </summary>
       /// <param name="Type"></param>
       /// <param name="SearchContent"></param>
       /// <param name="OrderBy"></param>
       /// <param name="IsActive"></param>
       /// <returns></returns>
        public int TotalRow(int? Type, string SearchContent, string OrderBy, bool? IsActive )
        {
            try
            {
                string[] mPara = { "Type", "SearchContent", "OrderBy", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, OrderBy, (IsActive == null ? null : IsActive.ToString()),true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_Partner_Search", mPara, mValue);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Lẫy dữ liệu phân trang
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="BeginRow"></param>
        /// <param name="EndRow"></param>
        /// <param name="SearchContent"></param>
        /// <param name="OrderBy"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DataTable Search(int? Type,int BeginRow, int EndRow, string SearchContent, string OrderBy, bool? IsActive)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "OrderBy", "IsActive","IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, OrderBy, (IsActive == null ? null : IsActive.ToString()) ,false.ToString()};
                return mGet.GetDataTable("Sp_Partner_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
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
                DataSet mSet = mGet.GetDataSet("Sp_Partner_Select", mPara, mValue);

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
                if (mExec.ExecProcedure("Sp_Partner_Update", mpara, mValue) > 0)
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

                if (mExec.ExecProcedure("Sp_Partner_Insert", mpara, mValue) > 0)
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

    }
}


