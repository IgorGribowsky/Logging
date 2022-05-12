using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Core.Model;
using BrainstormSessions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BrainstormSessions.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBrainstormSessionRepository _sessionRepository;

        private readonly ILogger _logger;

        public HomeController(IBrainstormSessionRepository sessionRepository, ILogger logger)
        {
            _sessionRepository = sessionRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Information($"Session list loading started");
            var sessionList = await _sessionRepository.ListAsync();
            _logger.Information($"Session list loading finished");

            var model = sessionList.Select(session => new StormSessionViewModel()
            {
                Id = session.Id,
                DateCreated = session.DateCreated,
                Name = session.Name,
                IdeaCount = session.Ideas.Count
            });

            return View(model);
        }

        public class NewSessionModel
        {
            [Required]
            public string SessionName { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Index(NewSessionModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning($"Session {model.SessionName} is invalid. BadRequest returned");
                return BadRequest(ModelState);
            }
            else
            {
                _logger.Information($"Session {model.SessionName} creation started");
                await _sessionRepository.AddAsync(new BrainstormSession()
                {
                    DateCreated = DateTimeOffset.Now,
                    Name = model.SessionName
                });
                _logger.Information($"Session {model.SessionName} created");
            }

            return RedirectToAction(actionName: nameof(Index));
        }
    }
}
