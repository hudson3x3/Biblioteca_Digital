using System;

namespace GrupoLTM.WebSmart.Services.Interface
{
    public interface IResponseSMS
    {
        string AppId { get; set; }
        Object Request { get; set; }
        bool Success { get; set; }
        bool HasError { get; set; }
        string ErrorDetail { get; set; }
        Object Result { get; set; }
        IResultListObject[] ResultList { get; set; }
    }

    public interface IResultListObject
    {
        string StatusCode { get; set; }
        string StatusDescription { get; set; }
        string DetailCode { get; set; }
        string DetailDescription { get; set; }
    }

}
