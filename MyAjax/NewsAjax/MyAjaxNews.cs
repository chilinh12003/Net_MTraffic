using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyAjax;
using MyUtility;
using System.Data;
using MyMTraffic.News;
using MyBase.MyLoad;
namespace MyAjax.NewsAjax
{
    public class MyAjaxNews : MyAjaxBase
    {

        public void SearchStreet()
        {
            Street mStreet = new Street();
            int PositionID = 0;
            string StreetName = "";
            string StreetName_EN = "";
            try
            {
                GetParemeter<int>(ref PositionID, "PositionID");
                GetParemeter<string>(ref StreetName, "StreetName");

                StreetName_EN = MyText.RemoveSignVietnameseString(StreetName);

                TemplatePath_Repat = "~/Templates/Street_Repeat.htm";

                DataTable mTable = new DataTable();
                if (PositionID == Position.HaNoi_ID)
                {
                    mTable = mStreet.GetStreetToCache_HN();
                }
                else if (PositionID == Position.HoChiMinh_ID)
                {
                    mTable = mStreet.GetStreetToCache_HCM();
                }

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, "KHONG CO DU LIEU", MyAjaxMessage.CommonSuccess.Success));
                }

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                mTable.DefaultView.RowFilter = "StreetName LIKE '" + StreetName + "%' OR StreetName_EN LIKE '" + StreetName_EN + "%' ";
                if (mTable.DefaultView.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, "KHONG CO DU LIEU", MyAjaxMessage.CommonSuccess.Success));
                }

                foreach (DataRowView mView in mTable.DefaultView)
                {
                    mBuilder.Append(mLoadTemplate.LoadTemplateByArray(TemplatePath_Repat, new string[] { mView["StreetID"].ToString(), mView["StreetName"].ToString() }));
                }
                mTable.DefaultView.RowFilter = string.Empty;

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, mBuilder.ToString(), MyAjaxMessage.CommonSuccess.Success));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyAjaxMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        public void GetStreetID()
        {
            Street mStreet = new Street();
            int PositionID = 0;
            string StreetName = "";
            string StreetName_EN = "";
            try
            {
                GetParemeter<int>(ref PositionID, "PositionID");
                GetParemeter<string>(ref StreetName, "StreetName");

                StreetName_EN = MyText.RemoveSignVietnameseString(StreetName);

                TemplatePath_Repat = "~/Templates/Street_Repeat.htm";

                DataTable mTable = new DataTable();
                if (PositionID == Position.HaNoi_ID)
                {
                    mTable = mStreet.GetStreetToCache_HN();
                }
                else if (PositionID == Position.HoChiMinh_ID)
                {
                    mTable = mStreet.GetStreetToCache_HCM();
                }

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.UnSuccess, "0", MyAjaxMessage.CommonSuccess.NoData));
                }

                mTable.DefaultView.RowFilter = "StreetName = '" + StreetName + "' OR StreetName_EN = '" + StreetName_EN + "' ";

                if (mTable.DefaultView.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.UnSuccess, "0", MyAjaxMessage.CommonSuccess.NoData));
                }

                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, mTable.DefaultView[0]["StreetID"].ToString(), MyAjaxMessage.CommonSuccess.Success));

                mTable.DefaultView.RowFilter = string.Empty;

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Ajax_Error", ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyAjaxMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }
    }
}
