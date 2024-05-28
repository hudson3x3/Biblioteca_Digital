using System.Collections.Generic;
using System.Linq;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.DTO;

namespace GrupoLTM.WebSmart.Services
{
    public class ProductResult
    {
        public string ProductSkuId { get; set; }
        public string ProductDescription { get; set; }
        public string ProductName { get; set; }
    }

    public class ClusterProductService : BaseService<ClusterProduct>
    {
        public List<ClusterProductModel> ProcurarProdutos(string nome)
        {
            List<ClusterProductModel> clusterProductModelList = new List<ClusterProductModel>();

            if (string.IsNullOrWhiteSpace(nome))
            {
                return clusterProductModelList;
            }

            List<ProductResult> productResultList = new List<ProductResult>();

            int codeAvon;

            if (int.TryParse(nome, out codeAvon) && codeAvon != 0)
            {
                productResultList = GenericSupplierService.SearhClusteredProducts(codeAvon.ToString(), null);
            }
            else
            {
                productResultList = GenericSupplierService.SearhClusteredProducts(null, nome);
            }

            if (productResultList != null && productResultList.Count > 0)
            {
                clusterProductModelList = productResultList.Select(x => new ClusterProductModel() { ProductSku = x.ProductSkuId, ProductDescription = x.ProductDescription, ProductName = x.ProductName }).ToList();
            }

            return clusterProductModelList;
        }
    }
}