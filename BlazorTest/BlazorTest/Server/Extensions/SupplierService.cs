using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorTest.Server.Services.Infrastruce;
using BlazorTest.Shared.Dto;
using MealOrdering.Server.Data.Context;
using MealOrdering.Server.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTest.Server.Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly MealOrderinDbContext context;
        private readonly IMapper mapper;

        public SupplierService(MealOrderinDbContext Context, IMapper Mapper)
        {
            context = Context;
            mapper = Mapper;
        }


        public async Task<List<SupplierDto>> GetSuppliers()
        {
            var list = await context.Supplier//.Where(i => i.IsActive)
                      .ProjectTo<SupplierDto>(mapper.ConfigurationProvider)
                      .OrderBy(i => i.CreateTime)
                      .ToListAsync();

            return list;
        }

        #region Post

        public async Task<SupplierDto> CreateSupplier(SupplierDto Supplier)
        {
            var dbSupplier = mapper.Map<Supplier>(Supplier);
            await context.AddAsync(dbSupplier);
            await context.SaveChangesAsync();

            return mapper.Map<SupplierDto>(dbSupplier);
        }

        public async Task<SupplierDto> UpdateSupplier(SupplierDto Supplier)
        {
            var dbSupplier = await context.Supplier.FirstOrDefaultAsync(i => i.Id == Supplier.Id);
            if (dbSupplier == null)
                throw new Exception("Supplier not found");

            mapper.Map(Supplier, dbSupplier);
            await context.SaveChangesAsync();

            return mapper.Map<SupplierDto>(dbSupplier);
        }

        public async Task DeleteSupplier(Guid SupplierId)
        {
            var Supplier = await context.Supplier.FirstOrDefaultAsync(i => i.Id == SupplierId);
            if (Supplier == null)
                throw new Exception("Supplier not found");

            int orderCount = await context.Supplier.Include(i => i.Orders).Select(i => i.Orders.Count).FirstOrDefaultAsync();

            if (orderCount > 0)
                throw new Exception($"There are {orderCount} sub order for the order you are trying to delete");

            context.Supplier.Remove(Supplier);
            await context.SaveChangesAsync();
        }

        public async Task<SupplierDto> GetSupplierById(Guid Id)
        {
            return await context.Supplier.Where(i => i.Id == Id)
                      .ProjectTo<SupplierDto>(mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync();
        }


        #endregion
    }
}
