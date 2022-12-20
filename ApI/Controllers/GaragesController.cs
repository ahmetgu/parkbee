using ApI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GaragesController : ControllerBase
{
    private readonly ParkingDbContext _parkingDbContext;

    public GaragesController(ParkingDbContext parkingDbContext)
    {
        _parkingDbContext = parkingDbContext;
    }

    [HttpGet]
    public Task<List<Garage>> GetGarages(CancellationToken cancellationToken)
    {
        return _parkingDbContext.Garages.Include(x => x.Doors).ToListAsync(cancellationToken);
    }

    [HttpGet("{garageId}")]
    public async Task<ActionResult<Garage>> GetGarage(Guid garageId, CancellationToken cancellationToken)
    {
        var garage = await _parkingDbContext.Garages.Include(x => x.Doors)
            .FirstOrDefaultAsync(g => g.Id == garageId, cancellationToken);
        if (garage == null)
        {
            return new NotFoundResult();
        }

        return garage;
    }

    [HttpGet("{garageId}/AvailableSpots")]
    public async Task<ActionResult<int>> GetGarageAvailability(Guid garageId, CancellationToken cancellationToken)
    {
        var garage = await _parkingDbContext.Garages.Include(g => g.ActiveSessions)
            .FirstOrDefaultAsync(g => g.Id == garageId, cancellationToken);
        if (garage == null)
        {
            return new NotFoundResult();
        }

        return garage.AvailableSpots;
    }

}