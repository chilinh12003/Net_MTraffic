using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MyUtility;
using MyMTraffic;
using MyMTraffic.Permission;
namespace MyAdmin.MasterPages
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Cho phép hiển thị SearchBox hay không
        /// </summary>
        public bool ShowSearchBox = true;

        public bool ShowToolBox = true;
        public HtmlInputCheckBox chk_ShowMessage
        {
            get { return chk_ShowMessage; }
        }

        /// <summary>
        /// Tiêu đề của SearchBox;
        /// </summary>
        public string str_TitleSearchBox = "Thông tin tìm kiếm:";

        public string str_PageTitle = "Administrators";

        public ContentPlaceHolder ContentPH_Header
        {
            get
            {
                return cph_Header;
            }
            set
            {
                cph_Header = value;
            }
        }

        public ContentPlaceHolder ContentPH_Search
        {
            get
            {
                return cph_Search;
            }
            set
            {
                cph_Search = value;
            }
        }
        public ContentPlaceHolder ContentPH_ToolBox
        {
            get
            {
                return cph_ToolBox;
            }
            set
            {
                cph_ToolBox = value;
            }
        }
        public ContentPlaceHolder ContentPH_Tool
        {
            get
            {
                return cph_Tools;
            }
            set
            {
                cph_Tools = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hid_Tocken.Value = Member.GenToken();
            }
            else
            {
                if (!Member.CheckToken(hid_Tocken.Value))
                {
                    //Chuyển đến trang thông báo request không hợp lệ
                    Response.Redirect(@"~/Admin/Ad_Alert.aspx?id=3");
                    return;
                }
            }
            a_DangNhap.HRef = MyConfig.URLLogin + "?PrevURL=" + Server.UrlEncode(Request.Url.ToString());

            if (Member.IsLogined() && (Session["Menu_1"] == null || Session["Menu_1"].ToString().Length < 1))
            {
                MenuAdmin mMenuAdmin = new MenuAdmin();
                //lấy các trang của nhóm mà user này có quyền truy cập.

                DataTable mTable = mMenuAdmin.Select(8, Member.MemberGroupID().ToString());

                string strFormat_1 = "  <a href='javascript:void(0);' onmouseover='ShowSubMenu(this);' onmouseout='HideSubMenu(this.title);' title='Sub_{0}'> " +
                                            "<span>{1}</span> " +
                                        "</a>";

                string strFormat_2_Out = "  <div class='SubMenu_Item' id='Sub_{0}' onmouseover='UndoHideSubMenu();' onmouseout='HideSubMenu(this.id);'>";

                string strFormat_2 = "  <a href='{0}'>"+
	                                        "<span class='Text'>{1}</span>"+
                                        "</a>";


                System.Text.StringBuilder mBuilder_1 = new System.Text.StringBuilder();
                System.Text.StringBuilder mBuilder_2 = new System.Text.StringBuilder();
                if(mTable != null && mTable.Rows.Count > 0)
                {
                    //Lấy menu mức 1.
                    mTable.DefaultView.RowFilter = " MenuID_2 IS NULL ";
                    DataTable mTable_1 = mTable.DefaultView.ToTable().Copy();
                    
                    foreach (DataRow mRow in mTable_1.Rows)
                    {
                        mBuilder_1.Append(string.Format(strFormat_1, mRow["MenuID_1"].ToString(), mRow["MenuName"].ToString()));
                        mBuilder_2.Append(string.Format(strFormat_2_Out, mRow["MenuID_1"].ToString()));
                        mTable.DefaultView.RowFilter = " MenuID_1 = " + mRow["MenuID_1"].ToString() + " AND MenuID_2 IS NOT NULL AND MenuID_3 IS NULL ";

                        DataTable mTable_2 = mTable.DefaultView.ToTable();
                        foreach (DataRow mRow_1 in mTable_2.Rows)
                        {
                            mBuilder_2.Append(string.Format(strFormat_2, mRow_1["Link"].ToString(), mRow_1["MenuName"].ToString()));
                        }
                        mBuilder_2.Append("</div>");
                    }
                    Session["Menu_1"] = mBuilder_1.ToString();
                    Session["Menu_2"] = mBuilder_2.ToString();
                }
            }
           
        }

        protected void lbtn_Thoat_Click(object sender, EventArgs e)
        {
            #region Log member
            MemberLog mLog = new MemberLog();
            MemberLog.ActionType Action = MemberLog.ActionType.Logout;
            mLog.Insert("Member", string.Empty, string.Empty, Action, true, Member.LoginName());
            #endregion

            Member.LogOut();
            
            Response.Redirect(MyConfig.URLLogin, false);
        }
    }
}
