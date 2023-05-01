using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlazorTest.Server.Services.Infrastruce;
using BlazorTest.Shared.Dto;
using BlazorTest.Shared.FilterModels;
using MealOrdering.Server.Data.Context;
using MealOrdering.Server.Data.Models;
using MealOrdering.Server.Services.Infrastruce;
using Microsoft.EntityFrameworkCore;

namespace BlazorTest.Server.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly MealOrderinDbContext context;
        private readonly IMapper mapper;
        private readonly IValidationService validationService;

        public OrderService(MealOrderinDbContext Context, IMapper Mapper, IValidationService ValidationService)
        {
            context = Context;
            mapper = Mapper;
            validationService = ValidationService;
        }


        #region Order Methods


        #region Get

        public async Task<List<OrderDto>> GetOrdersByFilter(OrderListFilterModel Filter)
        {
            var query = context.Order.Include(i => i.Supplier).AsQueryable();

            if (Filter.CreatedUserId != Guid.Empty)
                query = query.Where(i => i.CreateUserId == Filter.CreatedUserId);

            if (Filter.CreateDateFirst.HasValue || (Filter.CreateDateLast > DateTime.MinValue))
                query = query.Where(i => i.CreateTime >= Filter.CreateDateFirst.Value.ToUniversalTime());

            if (Filter.CreateDateLast > DateTime.MinValue)
                query = query.Where(i => i.CreateTime <= Filter.CreateDateLast.ToUniversalTime());


            var list = await query
                      .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                      .OrderBy(i => i.CreateTime)
                      .ToListAsync();

            return list;
        }


        public async Task<List<OrderDto>> GetOrders(DateTime OrderDate)
        {
            var list = await context.Order.Include(i => i.Supplier)
                      .Where(i => i.CreateTime.Date == OrderDate.Date)
                      .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                      .OrderBy(i => i.CreateTime)
                      .ToListAsync();

            return list;
        }



        public async Task<OrderDto> GetOrderById(Guid Id)
        {
            return await context.Order.Where(i => i.Id == Id)
                      .ProjectTo<OrderDto>(mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync();
        }

        #endregion

        #region Post

        public async Task<OrderDto> CreateOrder(OrderDto Order)
        {
            Order.CreateTime = DateTime.UtcNow;
            Order.ExpireTime = Order.ExpireTime.ToUniversalTime();
            var dbOrder = mapper.Map<Order>(Order);
            await context.AddAsync(dbOrder);
            await context.SaveChangesAsync();

            return mapper.Map<OrderDto>(dbOrder);
        }

        public async Task<OrderDto> UpdateOrder(OrderDto Order)
        {
            var dbOrder = await context.Order.FirstOrDefaultAsync(i => i.Id == Order.Id);
            if (dbOrder == null)
                throw new Exception("Order not found");


            if (!validationService.HasPermission(dbOrder.CreateUserId))
                throw new Exception("You cannot change the order unless you created");

            mapper.Map(Order, dbOrder);
            await context.SaveChangesAsync();

            return mapper.Map<OrderDto>(dbOrder);
        }

        public async Task DeleteOrder(Guid OrderId)
        {
            var detailCount = await context.OrderItems.Where(i => i.OrderId == OrderId).CountAsync();


            if (detailCount > 0)
                throw new Exception($"There are {detailCount} sub items for the order you are trying to delete");

            var order = await context.Order.FirstOrDefaultAsync(i => i.Id == OrderId);
            if (order == null)
                throw new Exception("Order not found");


            if (!validationService.HasPermission(order.CreateUserId))
                throw new Exception("You cannot change the order unless you created");



            context.Order.Remove(order);

            await context.SaveChangesAsync();
        }

        #endregion

        #endregion


        #region OrderItem Methods

        #region Get

        public async Task<List<OrderItemsDto>> GetOrderItems(Guid OrderId)
        {
            return await context.OrderItems.Where(i => i.OrderId == OrderId)
                      .ProjectTo<OrderItemsDto>(mapper.ConfigurationProvider)
                      .OrderBy(i => i.CreateTime)
                      .ToListAsync();
        }

        public async Task<OrderItemsDto> GetOrderItemsById(Guid Id)
        {
            return await context.OrderItems.Include(i => i.Order).Where(i => i.Id == Id)
                      .ProjectTo<OrderItemsDto>(mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync();
        }

        #endregion

        #region Post


        public async Task<OrderItemsDto> CreateOrderItem(OrderItemsDto OrderItem)
        {
            var order = await context.Order
                .Where(i => i.Id == OrderItem.OrderId)
                .Select(i => i.ExpireTime)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("The main order not found");

            if (order <= DateTime.Now)
                throw new Exception("You cannot create sub order. It is expired !!!");


            var dbOrder = mapper.Map<OrderItems>(OrderItem);
            await context.AddAsync(dbOrder);
            await context.SaveChangesAsync();

            return mapper.Map<OrderItemsDto>(dbOrder);
        }

        public async Task<OrderItemsDto> UpdateOrderItem(OrderItemsDto OrderItem)
        {
            var dbOrder = await context.OrderItems.FirstOrDefaultAsync(i => i.Id == OrderItem.Id);
            if (dbOrder == null)
                throw new Exception("Order not found");

            mapper.Map(OrderItem, dbOrder);
            await context.SaveChangesAsync();

            return mapper.Map<OrderItemsDto>(dbOrder);
        }

        public async Task DeleteOrderItem(Guid OrderItemId)
        {
            var orderItem = await context.OrderItems.FirstOrDefaultAsync(i => i.Id == OrderItemId);
            if (orderItem == null)
                throw new Exception("Sub order not found");

            context.OrderItems.Remove(orderItem);

            await context.SaveChangesAsync();
        }

        #endregion

        #endregion
    }
}
