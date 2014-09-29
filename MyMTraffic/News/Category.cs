using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyConnect.SQLServer;
using MyUtility;
using System.Data.SqlClient;
using System.ComponentModel;

namespace MyMTraffic.News
{
    public class Category
    {
        MyExecuteData mExec;
        MyGetData mGet;

        public Category()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public Category(string KeyConnect_InConfig)
        {
            mExec = new MyExecuteData(KeyConnect_InConfig);
            mGet = new MyGetData(KeyConnect_InConfig);
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
                DataSet mSet = mGet.GetDataSet("Sp_Category_Select", mPara, mValue);

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
        /// Lấy dữ liệu về thể loại
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Lấy dữ liệu mẫu</para>
        /// <para>Type = 1: Lấy thông tin chi tiết 1 record (CateID = @Para_1)</para>
        /// <para>Type = 2: Lấy các nhóm ở Thể loại 1</para>
        /// <para>Type = 3: Lấy các nhóm ở Thể loại 2 Theo CateID của Thể loại 1 (Para_1 = CateID_1</para>
        /// <para>Type = 4: Lấy các nhóm ở Thể loại 3 Theo CateID của Thể loại 2 (Para_1 = CateID_2)</para>
        /// <para>Type = 5: </para>
        /// <para>Type = 6: Lấy các con gần nhất của nhóm cha (Para_1 = CateID)</para>
        /// <para>Type = 11: Lấy tất cả các nhóm con của 1 nhóm cha(Para_1 = CateID) nhưng không lấy cateID đưa vào</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_Category_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy giá của CateID cấp cao nhất
        /// </summary>
        /// <param name="CateID">CateID truyền vào để lấy giá của CateID hoặc của chính nó</param>
        /// <param name="CateID_Return">CateID lấy được</param>
        /// <param name="Price">Giá của CateID lấy được</param>
        /// <param name="SendPrice">Giá gửi tặng của CateID lấy được</param>
        public void GetPriceCate(int CateID, ref int CateID_Return, ref double Price, ref double SendPrice)
        {
            DataTable mTable = Select(8, CateID.ToString());
            if (mTable != null && mTable.Rows.Count > 0)
            {
                CateID_Return = (int)mTable.Rows[0]["CateID"];
                Price = (double)mTable.Rows[0]["Price"];
                SendPrice = (double)mTable.Rows[0]["SendPrice"];
            }
            else
            {
                CateID_Return = 0;
                Price = 0;
                SendPrice = 0;
            }
        }

        /// <summary>
        /// Lấy dữ liệu Cate
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 7: Lấy các cate cùng cấp Para_1 = CateID, Para_2 = RowCount</para>
        /// <para>Type = 12: Lấy các nhóm con của 1 nhóm cha(Para_1 = CateID), Para_2=RowCount</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1, string Para_2)
        {
            try
            {
                string[] mPara = { "Type", "Para_1","Para_2" };
                string[] mValue = { Type.ToString(), Para_1, Para_2 };
                return mGet.GetDataTable("Sp_Category_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy CateID từ CateID_1....
        /// </summary>
        /// <param name="CateID_1"></param>
        /// <param name="CateID_2"></param>
        /// <param name="CateID_3"></param>
        /// <param name="CateID_4"></param>
        /// <returns></returns>
        public int Get_CateID(int? CateID_1, int? CateID_2, int? CateID_3, int? CateID_4)
        {
            try
            {
                DataTable mTable = Select(10, CateID_1.ToString(), CateID_2.ToString(), CateID_3.ToString(), CateID_4.ToString());
                if (mTable != null && mTable.Rows.Count > 0)
                {
                    return (int)mTable.Rows[0]["CateID"];
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tiền tố của thế loại.
        /// </summary>
        /// <param name="CateID_1"></param>
        /// <param name="CateID_2"></param>
        /// <param name="CateID_3"></param>
        /// <param name="CateID_4"></param>
        /// <returns></returns>
        public string Get_PrefixCode(int? CateID_1, int? CateID_2, int? CateID_3, int? CateID_4)
        {
            try
            {
                DataTable mTable = Select(10, CateID_1.ToString(), CateID_2.ToString(), CateID_3.ToString(), CateID_4.ToString());
                if (mTable != null && mTable.Rows.Count > 0)
                {
                    return mTable.Rows[0]["PrefixCode"].ToString();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">cach thuc lay
        /// <para>Type = 10: Lay thong tin chi tiet 1 record (CateID_1 = Para_1,CateID_2 = Para_2,CateID_3 = Para_3,CateID_4 = Para_4)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <param name="Para_3"></param>
        /// <param name="Para_4"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1, string Para_2, string Para_3, string Para_4)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2", "Para_3", "Para_4" };
                string[] mValue = { Type.ToString(), Para_1, Para_2, Para_3, Para_4 };
                return mGet.GetDataTable("Sp_Category_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu theo các cấp khác nhau, kèm theo tình trang IsActive
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <param name="Para_3"></param>
        /// <param name="Para_4"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1, string Para_2, string Para_3, string Para_4, bool? IsActive)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2", "Para_3", "Para_4", "IsActive" };
                string[] mValue = { Type.ToString(), Para_1, Para_2, Para_3, Para_4, (IsActive == null ? null : IsActive.ToString()) };
                return mGet.GetDataTable("Sp_Category_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        /// <summary>
        /// Kích hoạt thể loại, hoặc hủy kích hoạt thể loại
        /// </summary>
        /// <param name="Type">Type = 0:</param>
        /// <param name="IsActive">Trạng thái kích hoạt cần cập nhật</param>
        /// <param name="XMLContent">Danh sách các CateID cần Kích hoạt hoặc Hủy kích  hoạt</param>
        /// <returns></returns>    
        public bool Active(int Type, bool IsActive, string XMLContent)
        {
            try
            {

                string[] mpara = { "Type", "IsActive", "XMLContent" };
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Category_Active", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_Category_Update", mpara, mValue) > 0)
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

                if (mExec.ExecProcedure("Sp_Category_Insert", mpara, mValue) > 0)
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
                string[] mPara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Category_Delete", mPara, mValue) > 0)
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

        /// <summary>
        /// Lấy tổng số dòng
        /// </summary>
        /// <returns></returns>
        public int TotalRow(int? Type,string SearchContent, int? CateID_1, int? CateID_2,int? CateID_3, bool? IsActive)
        {
            try
            {
                string[] mPara = { "Type", "SearchContent", "CateID_1", "CateID_2","CateID_3", "IsActive","IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, CateID_1.ToString(), CateID_2.ToString(), CateID_3.ToString(), (IsActive == null ? null : IsActive.ToString()),true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_Category_Search", mPara, mValue);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu về thể loại cho chức năng phân trang
        /// </summary>
        /// <param name="BeginRow"></param>
        /// <param name="EndRow"></param>
        /// <param name="SearchContent"></param>
        /// <param name="Parent_CateID"></param>
        /// <param name="IsActive"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        public DataTable Search(int? Type,int BeginRow, int EndRow, string SearchContent, int? CateID_1, int? CateID_2, int? CateID_3, bool? IsActive, string OrderBy)
        {
            try
            {
                string[] mPara = { "Type", "BeginRow", "EndRow", "SearchContent", "CateID_1", "CateID_2","CateID_3", "IsActive", "OrderBy","IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, CateID_1.ToString(), CateID_2.ToString(), CateID_3.ToString(), (IsActive == null ? null : IsActive.ToString()), OrderBy,false.ToString() };

                return mGet.GetDataTable("Sp_Category_Search", mPara, mValue);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

       
    }
}
