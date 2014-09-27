using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
namespace MyLoad_Wap.LoadStatic
{
    public class MyGAScript : MyLoadBase
    {
         public MyGAScript()
        {
            mTemplatePath = "~/Templates/Static/GA_Script.htm";
            
            Init();
        }
        
        protected override string BuildHTML()
        {
            try
            {
                return mLoadTempLate.LoadTemplate(mTemplatePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
