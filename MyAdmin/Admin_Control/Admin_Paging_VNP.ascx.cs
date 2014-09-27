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
    public partial class Admin_Paging_VNP : System.Web.UI.UserControl
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

                    PageSize = 5;
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

            lbtn_SlidePrev.Enabled = mPaging.EnableSlidePrev;
            lbtn_1.Enabled = mPaging.EnablePage_1;
            lbtn_2.Enabled = mPaging.EnablePage_2;
            lbtn_3.Enabled = mPaging.EnablePage_3;
            lbtn_SlideNext.Enabled = mPaging.EnableSlideNext;


            lbtn_1.Text = mPaging.Page_1.ToString();
            lbtn_2.Text = mPaging.Page_2.ToString();
            lbtn_3.Text = mPaging.Page_3.ToString();

            lbtn_1.CssClass = null;
            lbtn_2.CssClass = null;
            lbtn_3.CssClass = null;

            if (!lbtn_SlideNext.Enabled || !lbtn_SlideNext.Visible)
                lbtn_SlideNext.CssClass = "local-disable";

            if (!lbtn_SlidePrev.Enabled || !lbtn_SlidePrev.Visible)
                lbtn_SlidePrev.CssClass = "local-disable";

            if (lbtn_1.Text == mPaging.CurrentPageIndex.ToString())
                lbtn_1.CssClass = "local-active";
            if (lbtn_2.Text == mPaging.CurrentPageIndex.ToString())
                lbtn_2.CssClass = "local-active";
            if (lbtn_3.Text == mPaging.CurrentPageIndex.ToString())
                lbtn_3.CssClass = "local-active";
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
    }
}