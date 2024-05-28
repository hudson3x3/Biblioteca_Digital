using GrupoLTM.WebSmart.Services.Interface;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class SMSResult : IResponseSMS
    {
        public SMSResult()
        {
            ResultList = new SMSResultListObject[] { };
        }

        public string AppId { get; set; }
        public object Request { get; set; }
        public bool Success { get; set; }
        public bool HasError { get; set; }
        public string ErrorDetail { get; set; }
        public object Result { get; set; }
        public IResultListObject[] ResultList { get; set; }
    }

    public class SMSResultListObject : IResultListObject
    {
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string DetailCode { get; set; }
        public string DetailDescription { get; set; }
    }
}
