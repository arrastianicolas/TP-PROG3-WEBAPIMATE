using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Services
{
    public interface ISalesService
    {
        Task<Sale> CreateSaleAsync(int clientId, List<SaleLine> saleLines, CancellationToken cancellationToken = default);
        Task<Sale> GetSaleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Sale>> GetAllSalesAsync(CancellationToken cancellationToken = default);
        Task<List<SaleLine>> GetSaleLinesBySaleIdAsync(int saleId, CancellationToken cancellationToken = default);
        Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default);
        Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
    }
}
