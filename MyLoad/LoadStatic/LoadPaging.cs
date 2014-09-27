using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using MyUtility;
namespace MyLoad.LoadStatic
{
    public class LoadPaging : MyLoadBase
    {
        public LoadPaging()
        {
            mTemplatePath = "~/Templates/Static/Paging.htm";
            Init();
        }
        
        public bool IsTheme = false;

        private int _PageSize = 0;

        /// <summary>
        /// Số lượng record trên 1 trang
        /// </summary>
        public int PageSize
        {
            get
            {
                try
                {
                    if (_PageSize < 1)
                    {
                        if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("PageSize")))
                        {
                            return _PageSize;
                        }
                        else
                        {
                            return int.Parse(MyConfig.GetKeyInConfigFile("PageSize"));
                        }
                    }
                    else
                    {
                        return _PageSize;
                    }
                }
                catch
                {
                    return 5;
                }
            }
            set { _PageSize = value; }
        }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPageIndex = 1;

        private int _TotalRow = 0;
        /// <summary>
        /// Tổng số dòng
        /// </summary>
        public int TotalRow
        {
            get { return _TotalRow; }
            set
            {
                _TotalRow = value;
                TotalPage = (int)(TotalRow / PageSize);
                if (TotalRow % PageSize > 0)
                {
                    TotalPage++;
                }
                if (CurrentPageIndex > TotalPage)
                    CurrentPageIndex = TotalPage;
                if (CurrentPageIndex < 1)
                    CurrentPageIndex = 1;
            }
        }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPage = 0;

        /// <summary>
        /// Số lượng page trên 1 trang
        /// </summary>
        public static int MaxPage
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("MaxPage")))
                    {
                        return 5;
                    }
                    else
                    {
                        return int.Parse(MyConfig.GetKeyInConfigFile("MaxPage"));
                    }
                }
                catch
                {
                    return 5;
                }
            }
        }

        /// <summary>
        /// Link của trang hiện tại.
        /// </summary>
        public string PageLink = string.Empty;

        public int BeginPage
        {
            get
            {
                if (TotalPage <= MaxPage)
                    return 1;

                if (CurrentPageIndex == 1)
                {
                    return 1;
                }
                if (CurrentPageIndex == TotalPage)
                {
                    return TotalPage + 1 - MaxPage;
                }

                if (CurrentPageIndex + MaxPage / 2 >= TotalPage)
                {
                    return TotalPage + 1 - MaxPage;
                }
                if (CurrentPageIndex - MaxPage / 2 <= 1)
                {
                    return 1;
                }
                return CurrentPageIndex - MaxPage / 2;
            }
        }

        public int BeginRow
        {
            get { return (CurrentPageIndex - 1) * PageSize + 1; }
        }
        public int EndRow
        {
            get { return (CurrentPageIndex - 1) * PageSize + PageSize; }
        }
        protected override string BuildHTML()
        {
            try
            {
                if (PageLink.IndexOf("?") >= 0)
                    PageLink += "&i=";
                else
                    PageLink += "?i=";

                StringBuilder mBuilder = new StringBuilder(string.Empty);
                
                if (TotalPage > MaxPage)
                {
                    mBuilder.Append("<li><a href=\"" + PageLink + "1" + "\">[Đầu]</a></li>");
                    for (int i = 0; i < MaxPage; i++)
                    {
                        if (BeginPage + i == CurrentPageIndex)
                        {
                            mBuilder.Append("<li class=\"current\"><a href=\"" + PageLink + (BeginPage + i).ToString() + "\">[" + (BeginPage + i).ToString() + "]</a></li>");
                        }
                        else
                        {
                            mBuilder.Append("<li><a href=\"" + PageLink + (BeginPage + i).ToString() + "\">[" + (BeginPage + i).ToString() + "]</a></li>");
                        }
                    }
                    mBuilder.Append("<li><a href=\"" + PageLink + TotalPage.ToString() + "\">[Cuối]</a></li>");
                }
                else
                {
                    for (int i = 0; i < TotalPage; i++)
                    {
                        if (BeginPage + i == CurrentPageIndex)
                        {
                            mBuilder.Append("<li class=\"current\"><a href=\"" + PageLink + (BeginPage + i).ToString() + "\">[" + (BeginPage + i).ToString() + "]</a></li>");
                        }
                        else
                        {
                            mBuilder.Append("<li><a href=\"" + PageLink + (BeginPage + i).ToString() + "\">[" + (BeginPage + i).ToString() + "]</a></li>");
                        }
                    }
                }

                return mLoadTempLate.LoadTemplateByString(mTemplatePath, mBuilder.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
