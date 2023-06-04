using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.Extensions.Logging;
using Services.Check;
using Services.Dtos;

namespace Services.Management
{
    public class AreaManagementService : IAreaManagementService
    {
        readonly ILogger<AreaManagementService> _logger;
        readonly ChippedAnimalsDbContext _context;
        readonly IAreaIntersectionValidationService _areaIntersectionValidationService;
        readonly IAreaPointsCoincidenceValidationService _areaPointsCoincidenceValidationService;
        readonly IInsideAreaCheckService _locationInsideAreaCheckService;
        readonly IMapper _mapper;

        public AreaManagementService(
            ILogger<AreaManagementService> logger, 
            ChippedAnimalsDbContext context,
            IAreaIntersectionValidationService areaSelfIntersectionValidationService,
            IAreaPointsCoincidenceValidationService areaPointsCoincidenceValidationService,
            IInsideAreaCheckService locationInsideAreaCheckService,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _areaIntersectionValidationService = areaSelfIntersectionValidationService;
            _areaPointsCoincidenceValidationService = areaPointsCoincidenceValidationService;
            _locationInsideAreaCheckService = locationInsideAreaCheckService;
            _mapper = mapper;
        }

        public async Task<AreaDto> GetByIdAsync(long? areaId)
        {
            Area? fetchedArea = await _context.Areas.FetchByIdNoTrackingAsync(areaId);
            _logger.LogInformation("Fetched from database {@model}", fetchedArea);
            if (fetchedArea == null)
            {
                throw new AreaNotFoundException(areaId);
            }
            return _mapper.Map<AreaDto>(fetchedArea);
        }

        public async Task<AreaDto> CreateAsync(AreaCreateDto createDto)
        {
            if (await DoesAreaNameExists(createDto.Name))
            {
                throw new AreaNameExistsException(createDto.Name);
            }
            Area newArea = _mapper.Map<Area>(createDto);
            if (DoAreaPointsHaveDuplicates(newArea.AreaPoints))
            {
                throw new AreaPointSetHasDuplicatesException();
            }
            IList<Area> allAreas = await _context.Areas.FetchAllNoTrackingOrderedPointsAsync();
            _areaPointsCoincidenceValidationService.Validate(newArea, allAreas);
            _areaIntersectionValidationService.Validate(newArea, allAreas);
            if (_locationInsideAreaCheckService.Check(newArea, allAreas)
                || allAreas.Any(a => _locationInsideAreaCheckService.Check(a, newArea)))
            {
                throw new AreaBordersAreInsideOtherAreaException(newArea.Name);
            }
            await _context.Areas.AddAsync(newArea);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added to database: {@model}", newArea);
            return _mapper.Map<AreaDto>(newArea);
        }

        public async Task<AreaDto> UpdateAsync(long? areaId, AreaUpdateDto updateDto)
        {
            if (await DoesAreaNameExists(updateDto.Name))
            {
                throw new AreaNameExistsException(updateDto.Name);
            }
            Area? fetchedArea = await _context.Areas.FetchByIdAsync(areaId);
            _logger.LogInformation("Fetched from database {@model}", fetchedArea);
            if (fetchedArea == null)
            {
                throw new AreaNotFoundException(areaId);
            }
            IList<Area> allAreas = await _context.Areas.FetchAllNoTrackingOrderedPointsAsync();
            allAreas.Remove(fetchedArea);
            _areaPointsCoincidenceValidationService.Validate(fetchedArea, allAreas);
            if (_locationInsideAreaCheckService.Check(fetchedArea, allAreas)
                || allAreas.Any(a => _locationInsideAreaCheckService.Check(a, fetchedArea)))
            {
                throw new AreaBordersAreInsideOtherAreaException(fetchedArea.Name);
            }
            _areaIntersectionValidationService.Validate(fetchedArea, allAreas);
            _mapper.Map(updateDto, fetchedArea);
            await _context.SaveChangesAsync();
            return _mapper.Map<AreaDto>(fetchedArea);
        }

        public async Task DeleteAsync(long? areaId)
        {
            Area? fetchedArea = await _context.Areas.FetchByIdAsync(areaId);
            _logger.LogInformation("Fetched from database {@model}", fetchedArea);
            if (fetchedArea == null)
            {
                throw new AreaNotFoundException(areaId);
            }
            _context.Areas.Remove(fetchedArea);
            await _context.SaveChangesAsync();
        }

        async Task<bool> DoesAreaNameExists(string name)
        {
            return await _context.Areas.DoesNameExist(name);
        }

        bool DoAreaPointsHaveDuplicates(ICollection<AreaPoint> areaPoints)
        {
            return areaPoints.Count != areaPoints.Distinct().Count();
        }
    }
}
