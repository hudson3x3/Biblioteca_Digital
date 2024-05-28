using GrupoLTM.WebSmart.Services.Interface;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class SMSStatusResult : IResponseSMSStatus
    {
        public SMSStatusResult()
        {
            getSmsStatusResp = new SMSStatusResultListObject { };
        }

        public string AppId { get; set; }
        public object Request { get; set; }
        public bool Success { get; set; }
        public bool HasError { get; set; }
        public string ErrorDetail { get; set; }
        public object Result { get; set; }
        
        public IResultListObjectStatus getSmsStatusResp { get; set; }
    }

    public class SMSStatusResultListObject : IResultListObjectStatus
    {
        public string id { get; set; }
        public string received { get; set; }
        public int? shortcode { get; set; }
        public string mobileOperatorName { get; set; }
        public string statusCode { get; set; }
        public string statusDescription { get; set; }
        public string detailCode { get; set; }
        public string detailDescription { get; set; }
       
    }

}
