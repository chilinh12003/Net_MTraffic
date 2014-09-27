using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyEmailReport.Report;
using MyUtility;
namespace MyEmailReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //ThreadDataSync mDataSync = new ThreadDataSync();
                //mDataSync.Run();

                EmailReport mReport = new EmailReport();
                mReport.Run();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
            }
        }
    }
}
