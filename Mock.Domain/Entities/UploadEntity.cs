    //----------Upload开始----------
    using System;
	using Mock.Code;
    namespace Mock.Domain.Entities 
    {
        /// <summary>
        /// 数据表实体类：UploadEntity  
        /// </summary>
        [Serializable()]
        public partial class UploadEntity
        {    
                         
            /// <summary>
            /// Int32:
            /// </summary>                       
            public Int32 Id {get;set;}   
                         
            /// <summary>
            /// DateTime:
            /// </summary>                       
            public DateTime AddTime {get;set;}   
                         
            /// <summary>
            /// String:
            /// </summary>                       
            public String UserName {get;set;}   
                         
            /// <summary>
            /// Int32:
            /// </summary>                       
            public Int32 SortCode {get;set;}   
               
        }    
     }

    //----------Upload结束----------

    