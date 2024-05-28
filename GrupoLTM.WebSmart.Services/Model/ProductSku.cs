using Newtonsoft.Json;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class ProductSku
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("vendorId")]
        public long? VendorId { get; set; }

        [JsonProperty("originalProductId")]
        public string OriginalProductId { get; set; }

        [JsonProperty("ean")]
        public long? ean { get; set; }

        [JsonProperty("name")]
        public string Product { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("descriptions")]
        public string descriptions { get; set; }

        [JsonProperty("supplierImage")]
        public string supplierImage { get; set; }

        [JsonProperty("defaultSku")]
        public DefaultSku defaultSku { get; set; }

        public decimal ConversionRate { get; set; }       
         
        public decimal ValueReal { get; set; }

        [JsonProperty("vendor")]
        public Vendor vendor { get; set; }

        public long? CatalogoId { get; set; }


    }
    public partial class DefaultSku
    {

        [JsonProperty("productSkuId")]
        public string productSkuId { get; set; }
        [JsonProperty("skuStatusId")]
        public int? skuStatusId { get; set; }
        [JsonProperty("vendorId")]
        public long? vendorId { get; set; }
        [JsonProperty("defaultPrice")]
        public decimal? defaultPrice { get; set; }
        [JsonProperty("sellingPrice")]
        public decimal? sellingPrice { get; set; }

        [JsonProperty("ean")]
        public long? ean { get; set; }

        [JsonProperty("originalProductSkuId")]
        public string originalProductSkuId { get; set; }


        [JsonProperty("skuImages")]
        public SkuImages[] skuImages { get; set; }
    }
    public partial class Vendor
    {

        [JsonProperty("vendorId")]
        public string vendorId { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("imageUrl")]
        public string imageUrl { get; set; }
        [JsonProperty("supplierTypeId")]
        public int? supplierTypeId { get; set; }
    }
    public partial class SkuImages
    {

        [JsonProperty("skuImageId")]
        public string skuImageId { get; set; }

        [JsonProperty("productSkuId")]
        public string productSkuId { get; set; }

        [JsonProperty("smallImage")]
        public string smallImage { get; set; }

        [JsonProperty("mediumImage")]
        public string mediumImage { get; set; }

        [JsonProperty("largeImage")]
        public string largeImage { get; set; }

        [JsonProperty("order")]
        public int? order { get; set; }
    }
}
