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

        public EFStormSessionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<BrainstormSession> GetByIdAsync(int id)
        {
            Log.Debug($"GetByIdAsync execution started. Id: {id}");

            var session = _dbContext.BrainstormSessions
                .Include(s => s.Ideas)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                Log.Debug($"GetByIdAsync executed. Session with id = {id} is not found");
            }
            else
            {
                Log.Debug($"GetByIdAsync executed. Session with id = {id} is found");
            }

            return session;
        }

        public Task<List<BrainstormSession>> ListAsync()
        {
            Log.Debug($"ListAsync execution started");

            var sessions = _dbContext.BrainstormSessions
                .Include(s => s.Ideas)
                .OrderByDescending(s => s.DateCreated)
                .ToListAsync();

            Log.Debug($"ListAsync executed");

            return sessions;
        }

        public Task AddAsync(BrainstormSession session)
        {
            Log.Debug($"AddAsync execution started");

            _dbContext.BrainstormSessions.Add(session);
            var result = _dbContext.SaveChangesAsync();

            Log.Debug($"AddAsync executed");

            return result;
        }

        public Task UpdateAsync(BrainstormSession session)
        {
            Log.Debug($"AddAsync execution started");

            _dbContext.Entry(session).State = EntityState.Modified;
            var result = _dbContext.SaveChangesAsync();

            Log.Debug($"AddAsync executed");

            return result;
        }
    }
}
