using AutoMapper;
using Core.Models;

namespace Services.Dtos.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapAccount();
            MapAnimal();
            MapAnimalType();
            MapAnimalVisitedLocation();
            MapArea();
            MapAreaPoint();
            MapLocation();
        }

        void MapAccount()
        {
            CreateMap<AccountRegistrationDto, Account>();
            CreateMap<AccountCreateDto, Account>()
                .ForMember(a => a.Role, options => options.Ignore());
            CreateMap<AccountUpdateDto, Account>()
                .ForMember(a => a.Role, options => options.Ignore());
            CreateMap<Account, AccountDto>()
                .ForMember(
                    ad => ad.Role,
                    options => options
                        .MapFrom(a => a.Role.Role));
        }

        void MapAnimal()
        {
            CreateMap<AnimalCreateDto, Animal>()
                .ForMember(a => a.Gender, options => options.Ignore())
                .ForMember(a => a.Types, options => options.Ignore());
            CreateMap<AnimalUpdateDto, Animal>()
                .ForMember(a => a.Gender, options => options.Ignore())
                .ForMember(a => a.LifeStatus, options => options.Ignore());
            CreateMap<Animal, AnimalDto>()
                .ForMember(
                    ad => ad.VisitedLocations,
                    options => options
                        .MapFrom(a => a.VisitedLocations.Select(a => a.Id)))
                .ForMember(
                    ad => ad.Types,
                    options => options
                        .MapFrom(a => a.Types.Select(a => a.Id)))
                .ForMember(
                    ad => ad.Gender,
                    options => options
                        .MapFrom(a => a.Gender.Gender))
                .ForMember(
                    ad => ad.LifeStatus,
                    options => options
                        .MapFrom(a => a.LifeStatus.LifeStatus));
        }

        void MapAnimalType()
        {
            CreateMap<AnimalTypeCreateDto, AnimalType>();
            CreateMap<AnimalTypeUpdateDto, AnimalType>();
            CreateMap<AnimalType, AnimalTypeDto>();
        }

        void MapAnimalVisitedLocation()
        {
            CreateMap<AnimalVisitedLocationUpdateDto, AnimalVisitedLocation>();
            CreateMap<AnimalVisitedLocation, AnimalVisitedLocationDto>();
        }

        void MapArea()
        {
            CreateMap<AreaCreateDto, Area>();
            CreateMap<AreaUpdateDto, Area>();
            CreateMap<Area, AreaDto>();
        }

        void MapAreaPoint()
        {
            CreateMap<AreaPointCreateDto, AreaPoint>();
            CreateMap<AreaPointUpdateDto, AreaPoint>();
            CreateMap<AreaPoint, AreaPointDto>();
        }

        void MapLocation()
        {
            CreateMap<LocationCreateDto, Location>();
            CreateMap<LocationUpdateDto, Location>();
            CreateMap<Location, LocationDto>();
        }
    }
}
