using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Core.Model;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BrainstormSessions.Infrastructure
{
    public class EFStormSessionRepository : IBrainstormSessionRepository
    {
        private readonly AppDbContext _dbContext;

        private readonly ILogger _logger;

        public EFStormSessionRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<BrainstormSession> GetByIdAsync(int id)
        {
            _logger.Debug($"GetByIdAsync execution started. Id: {id}");

            var session = _dbContext.BrainstormSessions
                .Include(s => s.Ideas)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                _logger.Debug($"GetByIdAsync executed. Session with id = {id} is not found");
            }
            else
            {
                _logger.Debug($"GetByIdAsync executed. Session with id = {id} is found");
            }

            return session;
        }

        public Task<List<BrainstormSession>> ListAsync()
        {
            _logger.Debug($"ListAsync execution started");

            var sessions = _dbContext.BrainstormSessions
                .Include(s => s.Ideas)
                .OrderByDescending(s => s.DateCreated)
                .ToListAsync();

            _logger.Debug($"ListAsync executed");

            return sessions;
        }

        public Task AddAsync(BrainstormSession session)
        {
            _logger.Debug($"AddAsync execution started");

            _dbContext.BrainstormSessions.Add(session);
            var result = _dbContext.SaveChangesAsync();

            _logger.Debug($"AddAsync executed");

            return result;
        }

        public Task UpdateAsync(BrainstormSession session)
        {
            _logger.Debug($"AddAsync execution started");

            _dbContext.Entry(session).State = EntityState.Modified;
            var result = _dbContext.SaveChangesAsync();

            _logger.Debug($"AddAsync executed");

            return result;
        }
    }
}
