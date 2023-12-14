namespace atelier3.Models.Repositories
{
    public class ProductsRepository : IProducRepository
    {


        private readonly AppDbContext context;
        public ProductsRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Product Add(Product Product)
        {
            context.Products.Add(Product);
            context.SaveChanges();
            return Product;
        }
        public Product Delete(int Id)
        {
            Product Product = context.Products.Find(Id);
            if (Product != null)
            {
                context.Products.Remove(Product);
                context.SaveChanges();
            }
            return Product;
        }
        public IEnumerable<Product> GetAllProduct()
        {
            return context.Products;
        }
        public Product GetProduct(int Id)
        {
            return context.Products.Find(Id);
        }

        public Product Update(Product ProductChanges)
        {
            Product P1 = context.Products.Find(ProductChanges.ProductID);
            if (P1 != null)
            {
                P1.ProductSize = ProductChanges.ProductSize;
                P1.Price = ProductChanges.Price;
                P1.Category = ProductChanges.Category;
               // P1.Photo = ProductChanges.Photo;

                context.SaveChanges();
            }
            return P1;
        }

        /*
         * Product P1 = context.Products.Find(ProductChanges.ProductID);
            if (P1 != null)
            {
                P1.ProductSize = ProductChanges.ProductSize;
                P1.Price = ProductChanges.Price;
                P1.Category = ProductChanges.Category;
                P1.Photo = ProductChanges.Photo;

                context.SaveChanges();
            }
         */
    }
}
