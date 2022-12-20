
using ApI.Data;
using ApI.Utils;
using Microsoft.EntityFrameworkCore;

namespace ApI.Managers
{
    public class SessionManager : ISessionManager
    {
        //TODO: Introduce sessionRepository and move db operations into repository
        private readonly ParkingDbContext _parkingDbContext;

        public SessionManager(ParkingDbContext parkingDbContext)
        {
            _parkingDbContext = parkingDbContext;
        }
        public async Task<SessionResult> StartSession(SessionInput sessionInfo, CancellationToken cancellationToken)
        {
            var user = await _parkingDbContext.Users.Include(u => u.ActiveSession)
                                                    .FirstOrDefaultAsync(u => u.Id == sessionInfo.UserId, cancellationToken);

            if (user == null)
            {
                return new SessionResult { Message = "User not found" , Status = StatusType.NotFound };
            }

            if (user.HasActiveSession)
            {
                return new SessionResult { Message = "User already has an active session", Status = StatusType.Conflict }; 
            }

            var garage = await _parkingDbContext.Garages.Include(x => x.Doors).Include(x => x.ActiveSessions)
                .FirstOrDefaultAsync(g => g.Id == sessionInfo.GarageId, cancellationToken);
            if (garage == null)
            {
                return new SessionResult { Message = "Garage not found", Status = StatusType.NotFound };
            }

            if (garage.AvailableSpots == 0)
            {
                return new SessionResult { Message = "No available parking spots", Status = StatusType.NotFound };
            }

            var door = garage.Doors.FirstOrDefault(d => d.DoorType == DoorType.Entry);
            if (door != null && !await door.CheckAvailability())
            {
                return new SessionResult { Message = "Failed to communicate with garage door", Status = StatusType.DependencyFail };
            }

            var session = new Session
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.Now,
                LicencePlate = sessionInfo.LicencePlate,
                UserId = user.Id,
                GarageId = sessionInfo.GarageId,
            };

            user.ActiveSession = session;
            garage.ActiveSessions.Add(session);
            await _parkingDbContext.AddAsync(session, cancellationToken);
            await _parkingDbContext.SaveChangesAsync(cancellationToken);

            return new SessionResult 
            { 
                Value = session.Id, 
                Status = StatusType.Success 
            };
        }

        public async Task<SessionResult> StopSession(Guid sessionId, CancellationToken cancellationToken)
        {
            var session = await _parkingDbContext.Sessions.FirstOrDefaultAsync(u => u.Id == sessionId, cancellationToken);
            if (session == null)
            {
                return new SessionResult { Message = "User not found", Status = StatusType.NotFound };
            }

            var garage = await _parkingDbContext.Garages.Include(x => x.Doors).Include(x => x.ActiveSessions)
                .FirstOrDefaultAsync(g => g.Id == session.GarageId, cancellationToken);
            if (garage == null)
            {
                return new SessionResult { Message = "Garage not found", Status = StatusType.NotFound };
            }

            var door = garage.Doors.FirstOrDefault(d => d.DoorType == DoorType.Exit);
            if (door == null)
            {
                return new SessionResult { Message = "Failed to communicate with garage door", Status = StatusType.DependencyFail };
            }

            var sessionHistory = new SessionHistory
            {
                Id = sessionId,
                StartTime = session.StartTime,
                EndTime = DateTime.Now,
                UserId = session.UserId,
                GarageId = session.GarageId,
                LicancePlate = session.LicencePlate
            };
            await _parkingDbContext.AddAsync(sessionHistory, cancellationToken);
            _parkingDbContext.Remove(session);
            await _parkingDbContext.SaveChangesAsync(cancellationToken);
            await door.Open();

            return new SessionResult { Status = StatusType.Success };   
        }
    }

    public interface ISessionManager
    {
        public Task<SessionResult> StartSession(SessionInput sessionInfo, CancellationToken cancellationToken);

        public Task<SessionResult> StopSession(Guid sessionId, CancellationToken cancellationToken);
    }
}
