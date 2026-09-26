using AutoMapper;
using back_end_for_TMS.Business.Types;
using back_end_for_TMS.Models;

namespace back_end_for_TMS.Infrastructure.Mapper;

public class AppMapperProfile : Profile
{
  public AppMapperProfile()
  {
    // Truck mappings
    CreateMap<CreateTruckDto, Truck>();
    CreateMap<UpdateTruckDto, Truck>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    CreateMap<Truck, TruckDto>();

    // Driver mappings
    CreateMap<CreateDriverDto, Driver>();
    CreateMap<UpdateDriverDto, Driver>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    CreateMap<Driver, DriverDto>()
        .ForMember(d => d.IsLicenseExpiringSoon, opt => opt.MapFrom(src =>
            src.LicenseExpiry.HasValue && src.LicenseExpiry.Value <= DateTime.UtcNow.AddDays(30)));

    // Customer mappings
    CreateMap<CreateCustomerDto, Customer>();
    CreateMap<UpdateCustomerDto, Customer>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    CreateMap<Customer, CustomerDto>()
        .ForMember(d => d.CustomerTypeLabel, opt => opt.MapFrom(src => src.CustomerType == 1 ? "Individual" : "Business"))
        .ForMember(d => d.StatusLabel, opt => opt.MapFrom(src => src.Status == 1 ? "Active" : "Inactive"));

    // Order mappings
    CreateMap<CreateOrderDto, Order>();
    CreateMap<UpdateOrderDto, Order>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    CreateMap<Order, OrderDto>()
        .ForMember(d => d.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : string.Empty))
        .ForMember(d => d.TripCount, opt => opt.MapFrom(src => src.Trips != null ? src.Trips.Count : 0))
        .ForMember(d => d.StatusLabel, opt => opt.MapFrom((src, dest) => src.Status switch
        {
          1 => "Created",
          2 => "Assigned",
          3 => "InTransit",
          4 => "Delivered",
          5 => "Completed",
          6 => "Cancelled",
          _ => "Unknown"
        }));

    // Trip mappings
    CreateMap<CreateTripDto, Trip>();
    CreateMap<UpdateTripDto, Trip>()
        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    CreateMap<Trip, TripDto>()
        .ForMember(d => d.OrderNumber, opt => opt.MapFrom(src => src.Order != null ? src.Order.OrderNumber : string.Empty))
        .ForMember(d => d.TruckLicensePlate, opt => opt.MapFrom(src => src.Truck != null ? src.Truck.LicensePlate : string.Empty))
        .ForMember(d => d.DriverFullName, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.FullName : string.Empty))
        .ForMember(d => d.TotalCost, opt => opt.MapFrom(src => (src.FuelCost ?? 0) + (src.TollCost ?? 0) + (src.OtherCost ?? 0)))
        .ForMember(d => d.StatusLabel, opt => opt.MapFrom((src, dest) => src.Status switch
        {
          1 => "Planned",
          2 => "InTransit",
          3 => "Completed",
          4 => "Cancelled",
          _ => "Unknown"
        }));
  }
}
