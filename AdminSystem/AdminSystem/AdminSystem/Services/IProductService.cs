using System.Collections.Generic;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> SearchProducts(string query);
        Product GetProductById(int productId);
        int CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeactivateProduct(int productId);
        void AddVariant(ProductVariant variant);
        void UpdateVariant(ProductVariant variant);
        void AddProductImage(ProductImage image);
        void DeleteProductImage(int imageId);
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Brand> GetAllBrands();
    }
}
