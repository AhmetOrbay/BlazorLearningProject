using AutoMapper;
using BlazorTest.Shared.Dto;
using MealOrdering.Server.Data.Models;

namespace BlazorTest.Server.Extensions
{
    public static class ConfigureMappingExtension
    {
        public static IServiceCollection ConfigureMapping(this IServiceCollection services)
        {
            var MappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = MappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues= true;
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(x => x.SupplierName, y => y.MapFrom(z => z.Supplier.Name))
                .ForMember(x => x.CreateUserFullName, y => y.MapFrom(x => x.Users.FirstName + " " + x.Users.LastName));

            CreateMap<OrderDto, Order>();

            CreateMap<OrderItems,OrderItemsDto>()
                .ForMember(x => x.OrderName, y => y.MapFrom(z => z.Order.Name))
                .ForMember(x => x.CreateUserFullName, y => y.MapFrom(x => x.Users.FirstName + " " + x.Users.LastName));

            CreateMap<OrderItemsDto, OrderItems>();
        }
    }
}
