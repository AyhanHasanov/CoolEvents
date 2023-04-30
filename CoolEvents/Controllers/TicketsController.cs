using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoolEvents.Data;
using CoolEvents.ViewModels.Tickets;
using CoolEvents.ExtensionMethods;

namespace CoolEvents.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null)
            {
                return View("NoPermission");
            }

            switch (loggedUser.RoleId)
            {
                case 1:
                    //admin
                    var modelsAll = new List<TicketDetailsVM>();

                    foreach (var item in _context.UserTickets)
                    {
                        TicketDetailsVM model = new TicketDetailsVM();
                        model.UserId = item.UserId;
                        model.TicketId = item.TicketId;
                        int eventId = _context.Tickets.Where(t => t.Id.Equals(item.TicketId)).First().EventId;
                        Event evnt = _context.Events.Where(e => e.Id.Equals(eventId)).First();
                        model.EventName = evnt.Name;
                        model.EventDescription = evnt.Description;
                        model.EventDate = evnt.Date;
                        model.User = _context.Users.Where(u => u.Id.Equals(item.UserId)).First();
                        model.TicketCount = 1;
                        modelsAll.Add(model);
                    }

                    return View(modelsAll);

                case 2:
                default:
                    //regular user
                    var ticketsOfCurrentUser = _context.UserTickets.Where(ut => ut.UserId.Equals(loggedUser.Id)).Select(x => x.TicketId).ToList(); // 3 4 5 5

                    Dictionary<int, int> ticketCounts = new Dictionary<int, int>();

                    // key is ticketid, value is count of ticket taken
                    foreach (var ticketID in ticketsOfCurrentUser)
                    {
                        if (!ticketCounts.ContainsKey(ticketID))
                            ticketCounts.Add(ticketID, 1);
                        else
                            ticketCounts[ticketID] += 1;
                    }

                    List<TicketDetailsVM> models = new List<TicketDetailsVM>();
                    foreach (var pair in ticketCounts)
                    {
                        TicketDetailsVM model = new TicketDetailsVM();
                        int ticketID = pair.Key;
                        int count = pair.Value;

                        int eventIdOfTicket = _context.Tickets.Where(t => t.Id == ticketID).First().EventId;
                        Event evnt = _context.Events.Where(e => e.Id == eventIdOfTicket).First();
                        model.TicketId = ticketID;
                        model.EventName = evnt.Name;
                        model.EventDescription = evnt.Description;
                        model.EventDate = evnt.Date;
                        model.TicketCount = count;

                        models.Add(model);
                    }
                    return View(models);
            }
        }

        public async Task<IActionResult> RemoveTicket(int ticketId)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null)
            {
                return View("NoPermission");
            }

            int userId = loggedUser.Id;
            foreach (var userTicket in _context.UserTickets.Where(ut => ut.UserId.Equals(userId) && ut.TicketId.Equals(ticketId)))
            {
                _context.UserTickets.Remove(userTicket);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveTicketAsAdmin(int ticketId, int userId)
        {
            var userticket = _context.UserTickets.Where(ut => ut.UserId.Equals(userId) && ut.TicketId.Equals(ticketId)).First();

            _context.UserTickets.Remove(userticket);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RemoveAll()
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null)
            {
                return View("NoPermission");
            }

            int userId = loggedUser.Id;
            foreach (var userTicket in _context.UserTickets.Where(ut => ut.UserId.Equals(userId)))
            {
                _context.UserTickets.Remove(userTicket);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RemoveAllFromAllUsers()
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null || loggedUser.RoleId != 1)
            {
                return View("NoPermission");
            }

            foreach (var userticket in _context.UserTickets)
            {
                _context.Remove(userticket);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}