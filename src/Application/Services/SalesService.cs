using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class SalesService : ISalesService
    {
        private readonly IRepositoryBase<Sale> _saleRepository;
        private readonly IRepositoryBase<SaleLine> _saleLineRepository;
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IRepositoryBase<Client> _clientRepository;

        public SalesService(IRepositoryBase<Sale> saleRepository, IRepositoryBase<SaleLine> saleLineRepository, IRepositoryBase<Product> productRepository, IRepositoryBase<Client> clientRepository)
        {
            _saleRepository = saleRepository;
            _saleLineRepository = saleLineRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
        }

        public async Task<Sale> CreateSaleAsync(int clientId, List<SaleLine> saleLines, CancellationToken cancellationToken = default)
        {
            var client = await _clientRepository.GetByIdAsync(clientId, cancellationToken);
            if (client == null)
            {
                throw new Exception("Client not found");
            }

            // Crear la venta
            var sale = new Sale
            {
                SaleDate = DateTime.UtcNow,
                UserId = clientId,
                TotalAmount = saleLines.Sum(sl => sl.TotalPrice),
                SaleLines = saleLines
            };

            sale = await _saleRepository.AddAsync(sale, cancellationToken);
            await _saleRepository.SaveChangesAsync(cancellationToken);

            // Asociar y guardar las líneas de venta
            foreach (var saleLine in saleLines)
            {
                saleLine.SaleId = sale.Id;
                await _saleLineRepository.AddAsync(saleLine, cancellationToken);
            }
            await _saleLineRepository.SaveChangesAsync(cancellationToken);

            return sale;
        }

        public async Task<Sale> GetSaleByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _saleRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<Sale>> GetAllSalesAsync(CancellationToken cancellationToken = default)
        {
            return await _saleRepository.ListAsync(cancellationToken);
        }

        public async Task<List<SaleLine>> GetSaleLinesBySaleIdAsync(int saleId, CancellationToken cancellationToken = default)
        {
            var saleLines = await _saleLineRepository.ListAsync(cancellationToken);
            return saleLines.Where(sl => sl.SaleId == saleId).ToList();
        }

        public async Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _productRepository.ListAsync(cancellationToken);
        }

        public async Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _productRepository.UpdateAsync(product, cancellationToken);
            await _productRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
