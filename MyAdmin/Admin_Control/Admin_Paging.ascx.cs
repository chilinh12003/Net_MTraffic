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
namespace MyAdmin.Admin_Control
{
    public partial class Admin_Paging : System.Web.UI.UserControl
    {
        Repeater _rpt_Data;
        public Repeater rpt_Data
        {
            get { return _rpt_Data; }
            set { _rpt_Data = value; }
        }

        int _PageSize = MyConfig.DefaultPageSize;
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        public bool ShowPageSize
        {
            get{return ddl_PageSize.Visible;}
            set { ddl_PageSize.Visible = value; }
        }
        string str_NameCookie = "Admin_PageSize";
        public string NameCookie
        {
            get { return str_NameCookie; }
            set { str_NameCookie = value; }
        }

        public MyPaging mPaging = new MyPaging();

        private DataTable _Data = new DataTable();
        public DataTable Data
        {
            get
            {
                //if (ViewState["_Data"] == null)
                //{
                    _Data = GetData_Callback_Change();
                //    ViewState["_Data"] = _Data.Copy();
                //}
                //else
                //{
                //    _Data = (DataTable)ViewState["_Data"];
                //}
                return _Data;
            }
        }

        /// <summary>
        /// tạo ra một hàng callback lấy dữ liệu
        /// </summary>
        /// <returns></returns>
        public delegate DataTable GetData_Callback();

        /// <summary>
        /// Khai báo 1 sự kiện để chạy hàm callback, hàm sẽ được định nghĩa khi thêm control này vào page
        /// </summary>
        public event GetData_Callback GetData_Callback_Change;

        /// <summary>
        /// tạo ra một hàng callback lấy totalpage
        /// </summary>
        /// <returns></returns>
        public delegate int GetTotalPage_Callback();

        /// <summary>
        /// Khai báo 1 sự kiện để chạy hàm callback, hàm sẽ được định nghĩa khi thêm control này vào page
        /// </summary>
        public event GetTotalPage_Callback GetTotalPage_Callback_Change;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ViewState[this.ID + "Paging"] == null)
                {
                    //Nếu Pagesize đã được fix thì không cần quan tâm tới việc save PageSize trong Cookie
                    if (Request.Cookies[NameCookie] != null && ShowPageSize)
                    {
                        int temp = PageSize;
                        int.TryParse(Request.Cookies[NameCookie].Value, out temp);

                        PageSize = temp > 0 ? temp : PageSize;

                        ddl_PageSize.SelectedIndex = ddl_PageSize.Items.IndexOf(ddl_PageSize.Items.FindByValue(PageSize.ToString()));
                    }

                    mPaging.PageSize = PageSize;
                    
                    ReGetTotalPage();

                    ViewState[this.ID+ "Paging"] = mPaging;

                    if (GetTotalPage_Callback_Change != null)
                        lbtn_First_Click(null, null);
                }
                else
                    mPaging = (MyPaging)ViewState[this.ID + "Paging"];
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }
        public void ResetLoadData()
        {
            ReGetTotalPage();
            BindDa();
        }
        public void ReGetTotalPage()
        {
            if (GetTotalPage_Callback_Change != null)
                mPaging.TotalRow = GetTotalPage_Callback_Change();

            mPaging.SetToTalPage();
            mPaging.CheckStatus();
            EnableDisableButton();
        }

        private void EnableDisableButton()
        {
            lbtn_First.Visible = mPaging.EnableFirst;
            lbtn_Prevous.Visible = mPaging.EnablePrevious;
            lbtn_SlidePrev.Visible = mPaging.EnableSlidePrev;
            lbtn_1.Visible = mPaging.EnablePage_1;
            lbtn_2.Visible = mPaging.EnablePage_2;
            lbtn_3.Visible = mPaging.EnablePage_3;
            lbtn_SlideNext.Visible = mPaging.EnableSlideNext;
            lbtn_Next.Visible = mPaging.EnableNext;
            lbtn_Last.Visible = mPaging.EnableLast;

            lbtn_1.Text = mPaging.Page_1.ToString();
            lbtn_2.Text = mPaging.Page_2.ToString();
            lbtn_3.Text = mPaging.Page_3.ToString();
            lbtn_1.CssClass = null;
            lbtn_2.CssClass = null;
            lbtn_3.CssClass = null;

            if (lbtn_1.Text == mPaging.CurrentPageIndex.ToString())
                lbtn_1.CssClass = "Active";
            if (lbtn_2.Text == mPaging.CurrentPageIndex.ToString())
                lbtn_2.CssClass = "Active";
            if (lbtn_3.Text == mPaging.CurrentPageIndex.ToString())
                lbtn_3.CssClass = "Active";
        }

        private void BindDa()
        {
            rpt_Data.DataSource = Data;
            rpt_Data.DataBind();
        }

        protected void lbtn_First_Click(object sender, EventArgs e)
        {
            try
            {

                mPaging.PagingSlide(MyPaging.PagingType.First, string.Empty);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }
        protected void lbtn_Prevous_Click(object sender, EventArgs e)
        {
            try
            {

                mPaging.PagingSlide(MyPaging.PagingType.Previous, string.Empty);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }
        protected void lbtn_SlidePrev_Click(object sender, EventArgs e)
        {
            try
            {

                mPaging.PagingSlide(MyPaging.PagingType.SlidePrev, string.Empty);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }
        protected void lbtn_1_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn_Buuton = (LinkButton)sender;
                mPaging.PagingSlide(MyPaging.PagingType.Slide, lbtn_Buuton.Text);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }
        protected void lbtn_2_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn_Buuton = (LinkButton)sender;
                mPaging.PagingSlide(MyPaging.PagingType.Slide, lbtn_Buuton.Text);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }
        protected void lbtn_3_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn_Buuton = (LinkButton)sender;
                mPaging.PagingSlide(MyPaging.PagingType.Slide, lbtn_Buuton.Text);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }
        protected void lbtn_SlideNext_Click(object sender, EventArgs e)
        {
            try
            {
                mPaging.PagingSlide(MyPaging.PagingType.SlideNext, string.Empty);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }
        protected void lbtn_Next_Click(object sender, EventArgs e)
        {
            try
            {
                mPaging.PagingSlide(MyPaging.PagingType.Next, string.Empty);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }
        protected void lbtn_Last_Click(object sender, EventArgs e)
        {
            try
            {
                mPaging.PagingSlide(MyPaging.PagingType.Last, string.Empty);
                EnableDisableButton();
                BindDa();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.PagingError, "Chilinh");
            }
        }

        protected void ddl_PageSize_SelectedIndexChanged(object sender, EventArgs e)
        {            
            mPaging.PageSize = int.Parse(ddl_PageSize.SelectedValue);
            ResetLoadData();

            HttpCookie Cookie_PageSize = new HttpCookie("Admin_PageSize");

            Cookie_PageSize.Expires = DateTime.Now.AddDays(3);

            //EnCode AccountCode và Accountpassword trường khi lưu xuống Cookie
            Cookie_PageSize.Value = mPaging.PageSize.ToString();

            HttpContext.Current.Response.Cookies.Remove("Admin_PageSize");
            HttpContext.Current.Response.Cookies.Add(Cookie_PageSize);

        }
    }
}