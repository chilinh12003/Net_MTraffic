using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using MyUtility;
namespace MyAdmin.Admin_Control
{
    public partial class Admin_Gridview : System.Web.UI.UserControl
    {
        public int PageIndex
        {
            get;
            set;
        }

        private string[] _HeaderName;

        public string[] HeaderName
        {
            get { return _HeaderName; }
            set { _HeaderName = value; }
        }

        public string CreateHeader()
        {
            try
            {
                if (HeaderName == null || HeaderName.Length < 1)
                    return string.Empty;

                string strFormat = "<th {0}>{1}</th>";
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Thêm header về Số thứ tự
                mBuilder.Append("<th width='10'>STT</th>");
                foreach (string Item in HeaderName)
                {
                    mBuilder.Append(string.Format(strFormat, string.Empty, Item));
                }
                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
                return string.Empty;
            }
        }

        public string[] ColumnNames
        {
            get;
            set;
        }

        public string CreateRow()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                string strStart_1 = "<tr class='Table_Row_1'><td class='Table_ML_1'></td>";
                string strEnd_1 = "<td class='Table_MR_1'></td></tr>";
                string strStart_2 = "<tr class='Table_Row_2'><td class='Table_ML_2'></td>";
                string strEnd_2 = "<td class='Table_MR_2'></td></tr>";
                string strFormatRow = "<td>{0}</td>";
                int ItemInex = 0;

                DataTable mTable = Admin_Paging1.Data;

                foreach (DataRow mRow in mTable.Rows)
                {
                    if (ItemInex % 2 == 0)
                    {
                        mBuilder.Append(strStart_1);
                    }
                    else
                    {
                        mBuilder.Append(strStart_2);
                    }
                    //Thêm dòng STT
                    mBuilder.Append(string.Format(strFormatRow, (ItemInex + PageIndex).ToString()));

                    foreach (string Item in ColumnNames)
                    {
                        ///Nếu không chưa cột nào thì lấy tên culumn kông tồn tại làm dữ liệu
                        if(!mTable.Columns.Contains(Item))
                        {
                            mBuilder.Append(string.Format(strFormatRow, Item));
                            continue;
                        }
                        if (mTable.Columns[Item].DataType == typeof(DateTime))
                        {
                            mBuilder.Append(string.Format(strFormatRow, ((DateTime)mRow[Item]).ToString(MyUtility.MyConfig.LongDateFormat)));
                        }
                        else if ((mTable.Columns[Item].DataType == typeof(decimal)))
                        {
                            mBuilder.Append(string.Format(strFormatRow, ((decimal)mRow[Item]).ToString(MyUtility.MyConfig.DecimalFormat)));
                        }
                        else if ((mTable.Columns[Item].DataType == typeof(double)))
                        {
                            mBuilder.Append(string.Format(strFormatRow, ((double)mRow[Item]).ToString(MyUtility.MyConfig.DoubleFormat)));
                        }
                        else
                        {
                            mBuilder.Append(string.Format(strFormatRow, mRow[Item].ToString()));
                        }
                    }

                    if (ItemInex % 2 == 0)
                    {
                        mBuilder.Append(strEnd_1);
                    }
                    else
                    {
                        mBuilder.Append(strEnd_2);
                    }

                    ItemInex++;
                }
                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
                return string.Empty;
            }
        }

        public Admin_Paging PagingControl
        {
            get { return Admin_Paging1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}