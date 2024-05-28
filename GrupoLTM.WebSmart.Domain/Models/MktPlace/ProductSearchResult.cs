using System.Collections.Generic;

namespace GrupoLTM.WebSmart.Domain.Models.MktPlace
{
    public class ProductSearchResult
    {
        public ProductSearchResult()
        {
            this.Products = new List<ProductResult>(new ProductResult[0]);
            this.Brands = new List<BrandResult>(new BrandResult[0]);
            this.Categories = new List<CategoryResult>(new CategoryResult[0]);
            this.Vendors = new List<VendorResult>(new VendorResult[0]);
        }

        public ProductSearchResult(int pageIndex, int lastPage, decimal lowerPrice, decimal higherPrice)
        {
            this.Products = new List<ProductResult>();
            this.Brands = new List<BrandResult>();
            this.Categories = new List<CategoryResult>();
            this.Vendors = new List<VendorResult>();

            this.PageIndex = pageIndex;
            this.LastPage = lastPage;
            this.LowerPrice = lowerPrice;
            this.HigherPrice = higherPrice;
        }

        public List<ProductResult> Products { get; set; }
        public List<BrandResult> Brands { get; set; }
        public List<CategoryResult> Categories { get; set; }
        public List<VendorResult> Vendors { get; set; }
        public int PageIndex { get; private set; }
        public int LastPage { get; private set; }
        public decimal LowerPrice { get; private set; }
        public decimal HigherPrice { get; private set; }

        public static readonly ProductSearchResult Error = new ProductSearchResult();
    }

    public class ProductResult
    {
        public ProductResult()
        {

        }

        public ProductResult(long vendorId, string vendorName, string productSkuId, string productName, string categoryId, string categoryName, string subCategoryId, string subCategoryName, string brandId, string brandName)
        {
            this.VendorId = vendorId;
            this.VendorName = vendorName;
            this.ProductSkuId = productSkuId;
            this.ProductName = productName;
            this.CategoryId = categoryId;
            this.CategoryName = categoryName;
            this.SubcategoryId = subCategoryId;
            this.SubcategoryName = subCategoryName;
            this.BrandId = brandId;
            this.BrandName = brandName;
        }

        public string ProductSkuId { get; private set; }
        public string ProductName { get; private set; }
        public long VendorId { get; private set; }
        public string VendorName { get; private set; }
        public string CategoryId { get; private set; }
        public string CategoryName { get; private set; }
        public string SubcategoryId { get; private set; }
        public string SubcategoryName { get; private set; }
        public string BrandId { get; private set; }
        public string BrandName { get; private set; }
        public string Image { get; set; }
        public string OriginalProductId { get; set; }
        public string OrdenationMoreSales { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal FromCostPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string DiscountPercent { get; set; }
        public string Ean { get; set; }
        public bool IsAvailable { get; set; }
        public string OrdenationSubCategory { get; set; }
        public VendorResult Vendor { get; set; }

        public bool HasPriceUpdated { get; set; }
        public bool InvalidVisibility { get; set; }
    }

    public class VendorResult
    {

        public VendorResult()
        {

        }

        public VendorResult(string vendorId, string name)
        {
            this.VendorId = vendorId;
            this.Name = name;
        }

        public VendorResult(string vendorId, string name, string imageUrl, string urlProductDetail)
        {
            this.VendorId = vendorId;
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.UrlProductDetail = (string.IsNullOrEmpty(urlProductDetail) ? "{0}/produto/{1}/{2}" : urlProductDetail);
        }

        public VendorResult(string vendorId, string name, string imageUrl, string urlProductDetail, int supplierTypeId)
        {
            this.VendorId = vendorId;
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.UrlProductDetail = (string.IsNullOrEmpty(urlProductDetail) ? "{0}/produto/{1}/{2}" : urlProductDetail);
            this.SupplierTypeId = supplierTypeId;
        }

        public string VendorId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string UrlProductDetail { get; set; }
        public int SupplierTypeId { get; set; }

    }

    public class BrandResult
    {

        public BrandResult(string brandId, string name)
        {
            this.BrandId = brandId;
            this.Name = name;
        }

        public string BrandId { get; private set; }
        public string Name { get; private set; }
    }

    public class CategoryResult
    {
        public CategoryResult() { }

        public CategoryResult(string categoryId, string name, int quantity, IList<SubCategoryResult> subCategories, string imagePath)
        {
            this.CategoryId = categoryId;
            this.Name = name;
            this.Quantity = quantity;
            this.SubCategories = subCategories;
            this.ImagePath = imagePath;
        }

        public CategoryResult(string categoryId, string name, int quantity, IList<SubCategoryResult> subCategories)
        {
            this.CategoryId = categoryId;
            this.Name = name;
            this.Quantity = quantity;
            this.SubCategories = subCategories;
        }

        public CategoryResult(string categoryId, string name, int quantity)
        {
            this.CategoryId = categoryId;
            this.Name = name;
            this.Quantity = quantity;
            this.SubCategories = new List<SubCategoryResult>();
        }

        public string CategoryId { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public IList<SubCategoryResult> SubCategories { get; private set; }
        public string ImagePath { get; private set; }

        public void SetImagePath(string path)
        {
            this.ImagePath = path;
        }
    }

    public class SubCategoryResult
    {
        public SubCategoryResult(string categoryId, string name, int quantity)
        {
            this.SubCategoryId = categoryId;
            this.Name = name;
            this.Quantity = quantity;
        }

        public string SubCategoryId { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
    }
}
