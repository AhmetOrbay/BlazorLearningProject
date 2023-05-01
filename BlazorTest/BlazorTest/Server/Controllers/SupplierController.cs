using BlazorTest.Server.Extensions;
using BlazorTest.Server.Services.Infrastruce;
using BlazorTest.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTest.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {

        private readonly ISupplierService supplierService;

        public SupplierController(ISupplierService SupplierService)
        {
            supplierService = SupplierService;
        }



        [HttpGet("SupplierById/{Id}")]
        public async Task<ServiceResponse<SupplierDto>> GetSupplierById(Guid Id)
        {
            return new ServiceResponse<SupplierDto>()
            {
                Data = await supplierService.GetSupplierById(Id)
            };
        }


        [HttpGet("Suppliers")]
        public async Task<ServiceResponse<List<SupplierDto>>> GetSuppliers()
        {
            return new ServiceResponse<List<SupplierDto>>()
            {
                Data = await supplierService.GetSuppliers()
            };
        }


        [HttpPost("CreateSupplier")]
        public async Task<ServiceResponse<SupplierDto>> CreateSupplier(SupplierDto Supplier)
        {
            return new ServiceResponse<SupplierDto>()
            {
                Data = await supplierService.CreateSupplier(Supplier)
            };
        }


        [HttpPost("UpdateSupplier")]
        public async Task<ServiceResponse<SupplierDto>> UpdateSupplier(SupplierDto Supplier)
        {
            return new ServiceResponse<SupplierDto>()
            {
                Data = await supplierService.UpdateSupplier(Supplier)
            };
        }


        [HttpPost("DeleteSupplier")]
        public async Task<BaseResponse> DeleteSupplier([FromBody] Guid SupplierId)
        {
            await supplierService.DeleteSupplier(SupplierId);
            return new BaseResponse();
        }
    }
}
