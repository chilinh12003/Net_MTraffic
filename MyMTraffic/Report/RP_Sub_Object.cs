using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;
using System.ComponentModel;
using System.Reflection;

namespace MyMTraffic.Report
{
    public class RP_Sub_Object
    {
        public enum PropertyType
        {
            SubTotal = 1,
            SubNew = 2,
            SubSMS = 3,
            SubWAP = 4,
            SubOther = 5,
            UnsubTotal = 6,
            UnsubNew = 7,
            UnsubSelf = 8,
            UnsubExtend = 9,
            UnsubOther = 10,
            RenewTotal = 11,
            RenewSuccess = 12,
            RenewFail = 13,
            RenewRate = 14,
            SaleReg = 15,
            SaleRenew = 16,
            RateSaleDay = 17,
        }

        public DateTime ReportDay = DateTime.MinValue; 
        public int ServiceID = 0;
        public int PartnerID = 0;
        public double SubTotal = 0;
        public double SubNew = 0;
        public double SubSMS = 0;
        public double SubWAP = 0;
        public double SubOther = 0;
        public double UnsubTotal = 0;
        public double UnsubNew = 0;
        public double UnsubSelf = 0;
        public double UnsubExtend = 0;
        public double UnsubOther = 0;
        public double RenewTotal = 0;
        public double RenewSuccess = 0;
        public double RenewFail = 0;
        public double RenewRate = 0;
        public double SaleReg = 0;
        public double SaleRenew = 0;
        public double RateSaleDay = 0;
        public string Note = string.Empty;

        /// <summary>
        /// Xóa hết dữ liệu thống kê
        /// </summary>
        public void Clear()
        {
            ReportDay = DateTime.MinValue;
            ServiceID = 0;
            PartnerID = 0;
            SubTotal = 0;
            SubNew = 0;
            SubSMS = 0;
            SubWAP = 0;
            SubOther = 0;
            UnsubTotal = 0;
            UnsubNew = 0;
            UnsubSelf = 0;
            UnsubExtend = 0;
            UnsubOther = 0;
            RenewTotal = 0;
            RenewSuccess = 0;
            RenewFail = 0;
            RenewRate = 0;
            SaleReg = 0;
            SaleRenew = 0;
            RateSaleDay = 0;
            Note = string.Empty;
        }

        public static FieldInfo[] GetAListField()
        {
            try
            {
                FieldInfo[] Arr_FieldInfo;
                Arr_FieldInfo = typeof(RP_Sub_Object).GetFields();

                return Arr_FieldInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static FieldInfo GetField(PropertyType mProType)
        {
            try
            {
                FieldInfo[] Arr_FieldInfo = GetAListField();
                foreach (FieldInfo mField in Arr_FieldInfo)
                {
                    if (mField.Name.Equals(mProType.ToString(), StringComparison.CurrentCultureIgnoreCase))
                        return mField;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static RP_Sub_Object Convert(DataRow mRow)
        {
            try
            {
                RP_Sub_Object mObj = new RP_Sub_Object();

                mObj.ReportDay = mRow["ReportDay"] != DBNull.Value ? (DateTime)mRow["ReportDay"] : DateTime.Now;
                mObj.ServiceID = mRow["ServiceID"] != DBNull.Value ? (int)mRow["ServiceID"] : 0;
                mObj.PartnerID = mRow["PartnerID"] != DBNull.Value ? (int)mRow["PartnerID"] : 0;
                mObj.SubTotal = mRow["SubTotal"] != DBNull.Value ? (double)mRow["SubTotal"] : 0;
                mObj.SubNew = mRow["SubNew"] != DBNull.Value ? (double)mRow["SubNew"] : 0;
                mObj.SubSMS = mRow["SubSMS"] != DBNull.Value ? (double)mRow["SubSMS"] : 0;
                mObj.SubWAP = mRow["SubWAP"] != DBNull.Value ? (double)mRow["SubWAP"] : 0;
                mObj.SubOther = mRow["SubOther"] != DBNull.Value ? (double)mRow["SubOther"] : 0;
                mObj.UnsubTotal = mRow["UnsubTotal"] != DBNull.Value ? (double)mRow["UnsubTotal"] : 0;
                mObj.UnsubNew = mRow["UnsubNew"] != DBNull.Value ? (double)mRow["UnsubNew"] : 0;
                mObj.UnsubSelf = mRow["UnsubSelf"] != DBNull.Value ? (double)mRow["UnsubSelf"] : 0;
                mObj.UnsubExtend = mRow["UnsubExtend"] != DBNull.Value ? (double)mRow["UnsubExtend"] : 0;
                mObj.UnsubOther = mRow["UnsubOther"] != DBNull.Value ? (double)mRow["UnsubOther"] : 0;
                mObj.RenewTotal = mRow["RenewTotal"] != DBNull.Value ? (double)mRow["RenewTotal"] : 0;
                mObj.RenewSuccess = mRow["RenewSuccess"] != DBNull.Value ? (double)mRow["RenewSuccess"] : 0;
                mObj.RenewFail = mRow["RenewFail"] != DBNull.Value ? (double)mRow["RenewFail"] : 0;
                mObj.RenewRate = mRow["RenewRate"] != DBNull.Value ? (double)mRow["RenewRate"] : 0;
                mObj.SaleReg = mRow["SaleReg"] != DBNull.Value ? (double)mRow["SaleReg"] : 0;
                mObj.SaleRenew = mRow["SaleRenew"] != DBNull.Value ? (double)mRow["SaleRenew"] : 0;
                mObj.SaleRenew = mRow["RateSaleDay"] != DBNull.Value ? (double)mRow["RateSaleDay"] : 0;
                mObj.Note = mRow["Note"] != DBNull.Value ? mRow["Note"].ToString() : string.Empty;

                return mObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<RP_Sub_Object> Convert(DataTable mTable)
        {
            try
            {
                List<RP_Sub_Object> mList = new List<RP_Sub_Object>();

                foreach (DataRow mRow in mTable.Rows)
                {
                    mList.Add(Convert(mRow));
                }
                return mList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        public void AddNewRow(ref DataTable mTable)
        {
            try
            {               
                DataRow mRow = mTable.NewRow();
                               
                mRow["ReportDay"] = this.ReportDay;
                mRow["ServiceID"] = this.ServiceID;
                mRow["PartnerID"] = this.PartnerID;
                mRow["SubTotal"] = this.SubTotal;
                mRow["SubNew"] = this.SubNew;
                mRow["SubSMS"] = this.SubSMS;
                mRow["SubWAP"] = this.SubWAP;
                mRow["SubOther"] = this.SubOther;
                mRow["UnsubTotal"] = this.UnsubTotal;
                mRow["UnsubNew"] = this.UnsubNew;
                mRow["UnsubSelf"] = this.UnsubSelf;
                mRow["UnsubExtend"] = this.UnsubExtend;                
                mRow["UnsubOther"] = this.UnsubOther;              
                mRow["RenewTotal"] = this.RenewTotal;
                mRow["RenewSuccess"] = this.RenewSuccess;
                mRow["RenewFail"] = this.RenewFail;
                mRow["RenewRate"] = this.RenewRate;
                mRow["SaleReg"] = this.SaleReg;
                mRow["SaleRenew"] = this.SaleRenew;
                mRow["RateSaleDay"] = this.RateSaleDay;
                mRow["Note"] = this.Note;

                mTable.Rows.Add(mRow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsNull
        {
            get
            {
                if (ReportDay == DateTime.MinValue || ServiceID == 0)
                    return true;
                else
                    return false;
            }
        }

        public RP_Sub_Object()
        {
            
        }       
     
    }
}
