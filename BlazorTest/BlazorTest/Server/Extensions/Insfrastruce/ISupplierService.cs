using BlazorTest.Shared.Dto;
using BlazorTest.Shared.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTest.Server.Services.Infrastruce
{
    public interface IOrderService
    {
        public Task<OrderDto> CreateOrder(OrderDto Order);

        public Task<OrderDto> UpdateOrder(OrderDto Order);

        public Task DeleteOrder(Guid OrderId);

        public Task<List<OrderDto>> GetOrders(DateTime OrderDate);

        public Task<List<OrderDto>> GetOrdersByFilter(OrderListFilterModel Filter);

        public Task<OrderDto> GetOrderById(Guid Id);



        public Task<OrderItemsDto> CreateOrderItem(OrderItemsDto OrderItem);

        public Task<OrderItemsDto> UpdateOrderItem(OrderItemsDto OrderItem);

        public Task<List<OrderItemsDto>> GetOrderItems(Guid OrderId);

        public Task<OrderItemsDto> GetOrderItemsById(Guid Id);

        public Task DeleteOrderItem(Guid OrderItemId);
    }
}
