using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyConnect.SQLServer;
using MyUtility;
using System.Data.SqlClient;
namespace MyMTraffic.Permission
{
    public class MemberLog
    {
        public enum ActionType
        {
            Insert = 1,
            Update = 2,
            Delete = 3,
            Active = 4,
            InActive = 5,
            Login = 6,
            Logout = 7,
            Publish = 8,
            UnPublish = 9
        }

        MyExecuteData mExec = new MyExecuteData();
        MyGetData mGet = new MyGetData();

        /// <summary>
        /// Lấy dữ liệu trong table memberLog
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Lẫy dữ liệu mẫu</para>
        /// <para>Type = 1: Lấy thông tin chi tiết 1 record (Para_1 = MemberLogID)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mpara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_MemberLog_Select", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tạo dữ liệu mẫu
        /// </summary>
        /// <returns></returns>
        public DataSet CreateDataSet()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                DataSet mSet = mGet.GetDataSet("Sp_MemberLog_Select", mPara, mValue);
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
        /// Insert dữ liệu và table MemberLog
        /// </summary>
        /// <param name="Type">Cách thức insert
        /// <para>Type = 0: Insert bình thường</para>
        /// </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Insert(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_MemberLog_Insert", mpara, mValue) > 0)
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
        /// Insert vào table MemberLog và trả lại maxID vừa mới insert vào
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 1: Inser và trả về maxid của record mới insert vào</para>
        /// </param>
        /// <param name="XMLContent">Dữ liệu dạng XML cần insert</param>
        /// <returns></returns>
        public DataTable Insert_Return(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };

                return mExec.ExecProcedure_Return("Sp_MemberLog_Insert", mpara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Nếu lưu log thành công thì phải cập nhất thành công
        /// </summary>
        /// <param name="MemberLogID"></param>
        /// <returns></returns>
        public bool Update_Success(int MemberLogID)
        {
            try
            {
                string[] mpara = { "Type", "MemberLogID" };
                string[] mValue = { "0", MemberLogID.ToString() };
                if (mExec.ExecProcedure("Sp_MemberLog_Update", mpara, mValue) > 0)
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
        public bool Insert(string TableName, string OldData, string NewData, ActionType Action, bool IsSuccess, string SearchContent, int MemberID, string LoginName)
        {
            try
            {
                //Tạo dữ liệu mẫu;
                DataSet mSet = CreateDataSet();
                if (mSet == null || mSet.Tables.Count != 1)
                {
                    return false;
                }
                string RequestIP = string.Empty;
                RequestIP = MyUtility.MyCurrent.GetRequestIP;

                //Hiệu chỉnh các column dạng DateTime thành dạng string
                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                //Tạo dòng dữ liêu mẫu
                DataRow mRow = mSet.Tables[0].NewRow();
                mRow["MemberID"] = MemberID;
                mRow["MemberName"] = LoginName;
                mRow["TableName"] = TableName;
                mRow["ActionID"] = (int)Action;
                mRow["ActionName"] = Action.ToString();
                mRow["OldData"] = OldData;
                mRow["NewData"] = NewData;

                mRow["IsSuccess"] = IsSuccess;
                mRow["RequestIP"] = RequestIP;
                mRow["CreateDate"] = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);
                mRow["SearchContent"] = SearchContent;

                mSet.Tables[0].Rows.Add(mRow);
                mSet.AcceptChanges();

                //thực hiện insert dữ liệu
                return Insert(0, mSet.GetXml());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public bool Insert(string TableName, string OldData, string NewData, ActionType Action, bool IsSuccess, string SearchContent)
        {
            try
            {
                //Tạo dữ liệu mẫu;
                DataSet mSet = CreateDataSet();
                if (mSet == null || mSet.Tables.Count != 1)
                {
                    return false;
                }
                string RequestIP = string.Empty;
                RequestIP = MyUtility.MyCurrent.GetRequestIP;
                
                //Hiệu chỉnh các column dạng DateTime thành dạng string
                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                //Tạo dòng dữ liêu mẫu
                DataRow mRow = mSet.Tables[0].NewRow();
                mRow["MemberID"] = Member.MemberID();
                mRow["MemberName"] = Member.LoginName();
                mRow["TableName"] = TableName;
                mRow["ActionID"] = (int)Action;
                mRow["ActionName"] = Action.ToString();
                mRow["OldData"] = OldData;
                mRow["NewData"] = NewData;
              
                mRow["IsSuccess"] = IsSuccess;
                mRow["RequestIP"] = RequestIP;
                mRow["CreateDate"] = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);
                mRow["SearchContent"] = SearchContent;

                mSet.Tables[0].Rows.Add(mRow);
                mSet.AcceptChanges();

                //thực hiện insert dữ liệu
                return Insert(0, mSet.GetXml());
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Lấy tổng số dòng
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0:</para>
        /// <para>Type = 1: Tìm kiếm theo MemberID</para>
        /// <para>Type = 2: Tìm kiếm theo Thên thành viên</para>
        /// <para>Type = 3: Tìm kiếm theo TableName</para>
        /// <para>Type = 4: Tìm kiếm theo ActionName</para>
        /// <para>Type = 5: Tìm kiếm theo IDValue</para>
        /// </param>
        /// <param name="SearchContent"></param>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public int TotalRow(int Type, string SearchContent, bool? IsSuccess)
        {
            try
            {
                string[] mpara = { "Type", "SearchContent", "IsSuccess", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, (IsSuccess == null ? null : IsSuccess.ToString()), true.ToString() };
                return (int)mGet.GetExecuteScalar("Sp_MemberLog_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tìm kiếm dữ liệu trên table log MemberLog
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0:</para>
        /// <para>Type = 1: Tìm kiếm theo MemberID</para>
        /// <para>Type = 2: Tìm kiếm theo Thên thành viên</para>
        /// <para>Type = 3: Tìm kiếm theo TableName</para>
        /// <para>Type = 4: Tìm kiếm theo ActionName</para>
        /// <para>Type = 5: Tìm kiếm theo IDValue</para>
        /// </param>
        /// <param name="BeginRow">Dòng bắt đầu</param>
        /// <param name="EndRow">Dòng kết thúc</param>
        /// <param name="SearchContent">Nội dung cần tìm</param>
        /// <param name="IsSuccess">Action thành công hay chưa</param>
        /// <param name="OrderBy">Sắp xếp theo trường nào</param>
        /// <returns></returns>
        public DataTable Search(int Type, int BeginRow, int EndRow, string SearchContent, bool? IsSuccess, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "IsSuccess", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, (IsSuccess == null ? null : IsSuccess.ToString()), OrderBy, false.ToString() };
                return mGet.GetDataTable("Sp_MemberLog_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Delete(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_MemberLog_Delete", mpara, mValue) > 0)
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
