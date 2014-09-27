using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;

namespace MyLoad.LoadStatic
{
    public class LoadContent : MyLoadBase
    {
        
        public LoadContent()
        {
            mTemplatePath = "~/Templates/Static/Content.htm";            
            Init();
        }
        public delegate string InsertHTML();
       
        /// <summary>
        /// Hàm này sẽ được định nghĩa sau khi khởi tạo 1 đối tượng (để insert vào phần Left của Content)
        /// </summary>
        public event InsertHTML InsertHTML_Change;

        /// <summary>
        /// Hàm trả về chuỗi có chứa mã HTML
        /// </summary>
        /// <returns></returns>
        protected override string BuildHTML()
        {
            try
            {                
                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] {InsertHTML_Change() });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
