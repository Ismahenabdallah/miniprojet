namespace atelier3.Models.Repositories
{
    public interface IProducRepository
    {
        Product GetProduct(int Id);
        IEnumerable<Product> GetAllProduct();
        Product Add(Product Product);
        Product Update(Product ProductChanges);
        Product Delete(int Id);
    }
}
