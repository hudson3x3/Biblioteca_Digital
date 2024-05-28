using GrupoLTM.WebSmart.Admin.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using static GrupoLTM.WebSmart.Domain.Enums.EnumDomain;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class ResgateOffLineModel
    {
        public ResgateOffLineModel()
        {

        }

        public int? ProjectId { get; set; }
        public string ProductSku { get; set; }
        public string ProductId { get; set; }
        public string Product { get; set; }
        public decimal Value { get; set; }
        public decimal ValueReal { get; set; }
        public string OriginalSku { get; set; }
        public decimal ConversionRate { get; set; }
        public HttpPostedFileBase ArquivoUploadBaseRA { get; set; }
        public int MktPlaceCatalogoId { get; set; }
        public SelectList ddlCatalogo { get; set; }
        public long CatalogId { get; set; }
        public string PageName { get; set; }
        public int IdAdmin { get; set; }
        public string OriginalProductSkuId { get; set; }
        public List<ResgateOffLine> listaResgateoffline { get; set; }
        public IEnumerable<ProducktSkuByCatalogo> producktsSkuByCatalogo { get; set; }
        public string jsonProducktSkuByCatalogo { get; set; }

    }
    public class ProducktSkuByCatalogo
    {
        [JsonProperty("MktPlaceCatalogoId")]
        public int MktPlaceCatalogoId { get; set; }

        [JsonProperty("ProductSku")]
        public string ProductSku { get; set; }

    }

    public class ResgateOffLine
    {
        public int? ProjectId { get; set; }
        public string ProductSku { get; set; }
        public string ProductId { get; set; }
        public string Product { get; set; }
        public decimal Value { get; set; }
        public decimal ValueReal { get; set; }
        public string OriginalSku { get; set; }
        public decimal ConversionRate { get; set; }
        public int MktPlaceCatalogoId { get; set; }
        public long CatalogId { get; set; }
        public string PageName { get; set; }
        public int IdAdmin { get; set; }
        public string OriginalProductSkuId { get; set; }
        public string UrlImage { get; set; }

    }
}