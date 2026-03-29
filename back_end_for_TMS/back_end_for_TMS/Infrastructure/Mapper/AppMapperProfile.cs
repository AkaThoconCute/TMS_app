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
  }
}
