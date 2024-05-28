using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models.MktPlace
{
        public class ProductsShowCase
        {
            public ProductsShowCase()
            {

            }
        public ProductsShowCase(ProductSearchResult productSearchResult)
        {
            if (productSearchResult.Products != null && productSearchResult.Products.Count > 0)
                Products = productSearchResult.Products.Select(x => new ProductResultModel(x)).ToList();

            if (productSearchResult.Categories != null && productSearchResult.Categories.Count > 0)
                Categories = productSearchResult.Categories.Select(x => new CategoryResultModel(x)).ToList();

            if (productSearchResult.Brands != null && productSearchResult.Brands.Count > 0)
                Brands = productSearchResult.Brands.Select(x => new BrandResultModel(x)).ToList();

            if (productSearchResult.Vendors != null && productSearchResult.Vendors.Count > 0)
                Vendors = productSearchResult.Vendors.Select(x => new VendorResultModel(x)).ToList();

            PageIndex = productSearchResult.PageIndex;
            LastPage = productSearchResult.LastPage;
            LowerPrice = productSearchResult.LowerPrice;
            HigherPrice = productSearchResult.HigherPrice;
        }

        public List<ProductResultModel> Products { get; set; }
            public List<CategoryResultModel> Categories { get; set; }
            public List<BrandResultModel> Brands { get; set; }
            public List<VendorResultModel> Vendors { get; set; }

            public int PageIndex { get; set; }
            public int LastPage { get; set; }
            public int ProductsPerPage { get; set; }
            public decimal LowerPrice { get; set; }
            public decimal HigherPrice { get; set; }

            public decimal LowerPriceLimit { get; set; }
            public decimal HigherPriceLimit { get; set; }

            //public void ConvertToPoints(CatalogConfiguration catalog)
            //{
            //    ConvertToPoints(catalog, true);
            //}

            //public void ConvertToPoints(CatalogConfiguration catalog, bool convertProductPrice)
            //{
            //    if (convertProductPrice && Products != null)
            //    {
            //        foreach (var productModel in Products.Where(w => !w.HasPriceUpdated))
            //        {
            //            productModel.DefaultPrice = catalog.ConvertToPoints(productModel.DefaultPrice, productModel.VendorId);
            //            productModel.SellingPrice = catalog.ConvertToPoints(productModel.SellingPrice, productModel.VendorId);
            //        }
            //    }

            //    LowerPrice = catalog.ConvertToPoints(LowerPrice, default(long));
            //    HigherPrice = catalog.ConvertToPoints(HigherPrice, default(long));
            //}
        }

        public class CategoryResultModel
        {
            public CategoryResultModel()
            {

            }
            public CategoryResultModel(CategoryResult categoryResult)
            {
                CategoryId = categoryResult.CategoryId;
                Name = categoryResult.Name;
                Quantity = categoryResult.Quantity;
                Subcategories = categoryResult.SubCategories.Select(x => new SubcategoryResultModel(x)).ToList();
                ImagePath = categoryResult.ImagePath;
            }

            public string CategoryId { get; set; }
            public string Name { get; set; }

            public string UrlName
            {
                get { return StringExtensions.BuildsFriendlyUrl(Name); }
            }

            public int Quantity { get; set; }
            public List<SubcategoryResultModel> Subcategories { get; set; }
            public string ImagePath { get; set; }
        }

        public class SubcategoryResultModel
        {
            public SubcategoryResultModel()
            {
            }

            public SubcategoryResultModel(SubCategoryResult subCategoryResult)
            {
                SubcategoryId = subCategoryResult.SubCategoryId;
                Name = subCategoryResult.Name;
                Quantity = subCategoryResult.Quantity;
            }

            public string SubcategoryId { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
        }

        public class BrandResultModel
        {
            public BrandResultModel()
            {

            }
            public BrandResultModel(BrandResult brandResult)
            {
                BrandId = brandResult.BrandId;
                Name = brandResult.Name;
            }

            public string BrandId { get; set; }
            public string Name { get; set; }
        }

        public class VendorResultModel
        {
            public VendorResultModel()
            {

            }
            public VendorResultModel(VendorResult vendorResult)
            {
                VendorId = vendorResult.VendorId;
                Name = vendorResult.Name;
                ImageUrl = vendorResult.ImageUrl;
                UrlProductDetail = vendorResult.UrlProductDetail;
                SupplierTypeId = vendorResult.SupplierTypeId;
            }

            public string VendorId { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public string UrlProductDetail { get; set; }
            public int SupplierTypeId { get; set; }

            public string UrlName
            {
                get
                {
                    var url = this.Name.ToLower().Replace(".com", "").Replace(".br", "");
                    return StringExtensions.BuildsFriendlyUrl(url);
                }
            }
        }

        public class ProductResultModel
        {
            public ProductResultModel()
            {
            }

            public ProductResultModel(ProductResult productResult)
            {
                ProductSkuId = productResult.ProductSkuId;
                VendorId = productResult.VendorId;
                CategoryName = productResult.CategoryName;
                SubcategoryName = productResult.SubcategoryName;
                BrandName = productResult.BrandName;
                CategoryId = productResult.CategoryId;
                SubcategoryId = productResult.SubcategoryId;
                BrandId = productResult.BrandId;
                ProductName = productResult.ProductName;
                Image = productResult.Image;
                VendorName = productResult.VendorName;
                OriginalProductId = productResult.OriginalProductId;
                DefaultPrice = productResult.DefaultPrice;
                SellingPrice = productResult.SellingPrice;
                FromCostPrice = productResult.FromCostPrice;
                CostPrice = productResult.CostPrice;
                DiscountPercent = productResult.DiscountPercent;
                Vendor = productResult.Vendor;
                OrdenationSubCategory = productResult.OrdenationSubCategory;
                HasPriceUpdated = productResult.HasPriceUpdated;
                IsAvailable = productResult.IsAvailable;
                OrdenationMoreSales = productResult.OrdenationMoreSales;
            }

            public string ProductSkuId { get; set; }
            public long VendorId { get; set; }
            public string CategoryName { get; set; }
            public string SubcategoryName { get; set; }
            public string BrandName { get; set; }
            public string CategoryId { get; set; }
            public string SubcategoryId { get; set; }
            public string BrandId { get; set; }
            public string ProductName { get; set; }
            public string Image { get; set; }
            public string VendorName { get; set; }
            public string OriginalProductId { get; set; }
            public string OrdenationMoreSales { get; set; }
            public decimal DefaultPrice { get; set; }
            public decimal SellingPrice { get; set; }
            public decimal FromCostPrice { get; set; }
            public decimal CostPrice { get; set; }
            public string DiscountPercent { get; set; }
            public VendorResult Vendor { get; set; }
            public string OrdenationSubCategory { get; set; }
            public bool HasPriceUpdated { get; set; }
            public bool IsAvailable { get; set; }
        }
    }