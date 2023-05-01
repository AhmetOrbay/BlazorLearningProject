using BlazorTest.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTest.Server.Services.Infrastruce
{
    public interface ISupplierService
    {
        public Task<List<SupplierDto>> GetSuppliers();

        public Task<SupplierDto> CreateSupplier(SupplierDto Order);

        public Task<SupplierDto> UpdateSupplier(SupplierDto Order);

        public Task DeleteSupplier(Guid SupplierId);

        public Task<SupplierDto> GetSupplierById(Guid Id);
    }
}
