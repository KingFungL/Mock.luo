    //----------UploadEntry开始----------
    using System;
	using Mock.Code;
    namespace Mock.Domain.Entities 
    {
        /// <summary>
        /// 数据表实体类：UploadEntryEntity  
        /// </summary>
        [Serializable()]
        public partial class UploadEntryEntity
        {    
                         
            /// <summary>
            /// Int32:
            /// </summary>                       
            public Int32 Id {get;set;}   
                         
            /// <summary>
            /// Int32:
            /// </summary>                       
            public Int32 FId {get;set;}   
                         
            /// <summary>
            /// DateTime:
            /// </summary>                       
            public DateTime AddTime {get;set;}   
                         
            /// <summary>
            /// String:
            /// </summary>                       
            public String Url {get;set;}   
                         
            /// <summary>
            /// String:
            /// </summary>                       
            public String FileName {get;set;}   
                         
            /// <summary>
            /// String:
            /// </summary>                       
            public String FileSize {get;set;}   
               
        }    
     }

    //----------UploadEntry结束----------

    