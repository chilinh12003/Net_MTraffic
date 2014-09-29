using System;
using System.Collections.Generic;
using System.Text;
using MyConnect.SQLServer;
using System.Data;
using System.Data.SqlClient;
namespace MyMTraffic.Permission
{
    
    /// <summary>
    /// 
    /// </summary>
    public class MenuAdmin
    {
        
        MyExecuteData mExec = new MyExecuteData();
        MyGetData mGet = new MyGetData();

        /// <summary>
        /// Lấy dữ liệu về thể loại
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Lấy nhóm theo thể loại 1</para>
        /// <para>Type = 1: Lấy thông tin chi tiết 1 record (MenuID = @Para_1)</para>
        /// <para>Type = 2: Lấy các nhóm ở Thể loại 2 Theo MenuID của Thể loại 1 (Para_1 = MenuID_1</para>
        /// <para>Type = 3: Lấy các nhóm ở Thể loại 3 Theo MenuID của Thể loại 2 (Para_1 = MenuID_2)</para>
        /// <para>Type = 4: </para>
        /// <para>Type = 5: Lấy các nhóm con của 1 nhóm cha(Para_1 = MenuID)</para>
        /// <para>Type = 7: Lấy các nhóm ở Thể loại 4 Theo MenuID của Thể loại 3 (Para_1 = MenuID_3)</para>
        /// <para>Type = 8: Lấy menu theo MemberGroupID (@Para_1 = MemberGroupID)</para>
        /// <para>Type = 9: Lấy các Menu chưa phân quyền của 1 nhóm thành viên (para_1 = MemberGroupID)</para>
        /// <para>Type = 11: Lấy tất cả các trang con có mức nhỏ nhất</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_MenuAdmin_Select", mPara, mValue);
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
        /// <para>Type = 9: Lấy các Dataset (table các nút con, table các nút cha) Menu chưa phân quyền của 1 nhóm thành viên (para_1 = MemberGroupID)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataSet Select_DataSet(int? Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataSet("Sp_MenuAdmin_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu Cate
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 6: Lấy các cate cùng cấp Para_1 = MenuID, Para_2 = RowCount</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1, string Para_2)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2" };
                string[] mValue = { Type.ToString(), Para_1, Para_2 };
                return mGet.GetDataTable("Sp_MenuAdmin_Select", mPara, mValue);
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
        /// <para>Type = 10: Lay thong tin chi tiet 1 record (MenuID_1 = Para_1,MenuID_2 = Para_2,MenuID_3 = Para_3,MenuID_4 = Para_4)</para>
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
                return mGet.GetDataTable("Sp_MenuAdmin_Select", mPara, mValue);
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
        /// <param name="XMLContent">Danh sách các MenuID cần Kích hoạt hoặc Hủy kích  hoạt</param>
        /// <returns></returns>    
        public bool Active(int Type, bool IsActive, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsActive", "XMLContent"};
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent};
                if (mExec.ExecProcedure("Sp_MenuAdmin_Active", mpara, mValue) > 0)
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

      
        public bool Insert(int Type, int? MenuID_1, int? MenuID_2, int? MenuID_3, int? MenuID_4, string MenuName, string ImagePath, int Priority, bool IsActive, string PageCode, string Link)
        {
            try
            {
                string[] mPara = { "Type", "MenuID_1", "MenuID_2", "MenuID_3", "MenuID_4", "MenuName", "ImagePath", "Priority", "IsActive", "PageCode", "Link" };
                string[] mValue = { Type.ToString(), MenuID_1.ToString(), MenuID_2.ToString(), MenuID_3.ToString(), MenuID_4.ToString(), MenuName, ImagePath, Priority.ToString(), IsActive.ToString(), PageCode, Link };
                if (mExec.ExecProcedure("Sp_MenuAdmin_Insert", mPara, mValue) > 0)
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

        public bool Update(int Type, int MenuID, int? MenuID_1, int? MenuID_2, int? MenuID_3, int? MenuID_4, string MenuName, string ImagePath, int Priority, bool IsActive, string PageCode, string Link)
        {
            try
            {
                string[] mPara = { "Type", "MenuID", "MenuID_1", "MenuID_2", "MenuID_3", "MenuID_4", "MenuName", "ImagePath", "Priority", "IsActive", "PageCode", "Link" };
                string[] mValue = { Type.ToString(), MenuID.ToString(), MenuID_1.ToString(), MenuID_2.ToString(), MenuID_3.ToString(), MenuID_4.ToString(), MenuName, ImagePath, Priority.ToString(), IsActive.ToString(), PageCode,Link };
                if (mExec.ExecProcedure("Sp_MenuAdmin_Update", mPara, mValue) > 0)
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

        public bool Delete(int Type, string XMLContent)
        {
            try
            {
                string[] mPara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_MenuAdmin_Delete", mPara, mValue) > 0)
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
        
        public int TotalRow(string SearchContent, int? MenuID_1, int? MenuID_2, int? MenuID_3, bool? IsActive)
        {
            try
            {
                string[] mPara = { "Type", "SearchContent", "MenuID_1", "MenuID_2", "MenuID_3", "IsActive" };
                string[] mValue = { "0", SearchContent, MenuID_1.ToString(), MenuID_2.ToString(), MenuID_3.ToString(), (IsActive == null ? null : IsActive.ToString()) };

                return (int)mGet.GetExecuteScalar("Sp_MenuAdmin_Search", mPara, mValue);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int BeginRow, int EndRow, string SearchContent, int? MenuID_1, int? MenuID_2, int? MenuID_3, bool? IsActive, string OrderBy)
        {
            try
            {
                string[] mPara = { "Type", "BeginRow", "EndRow", "SearchContent", "MenuID_1", "MenuID_2", "MenuID_3", "IsActive", "OrderBy"};
                string[] mValue = { "1", BeginRow.ToString(), EndRow.ToString(), SearchContent, MenuID_1.ToString(), MenuID_2.ToString(), MenuID_3.ToString(), (IsActive == null ? null : IsActive.ToString()), OrderBy };

                return mGet.GetDataTable("Sp_MenuAdmin_Search", mPara, mValue);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }



    }
}
