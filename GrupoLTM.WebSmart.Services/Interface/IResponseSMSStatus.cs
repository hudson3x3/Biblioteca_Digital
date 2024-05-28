using System;

namespace GrupoLTM.WebSmart.Services.Interface
{
    public interface IResponseSMSStatus
    {
        string AppId { get; set; }
        Object Request { get; set; }
        bool Success { get; set; }
        bool HasError { get; set; }
        string ErrorDetail { get; set; }
        Object Result { get; set; }
        IResultListObjectStatus getSmsStatusResp { get; set; }
    }

    public interface IResultListObjectStatus
    {
        string id { get; set; }
        string received { get; set; }
        int? shortcode { get; set; }
        string mobileOperatorName { get; set; }
        string statusCode { get; set; }
        string statusDescription { get; set; }
        string detailCode { get; set; }
        string detailDescription { get; set; }
    }

}
