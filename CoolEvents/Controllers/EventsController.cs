using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoolEvents.Data;
using CoolEvents.ViewModels.Events;
using CoolEvents.ExtensionMethods;

namespace CoolEvents.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddTicket(int eventId)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null)
            {
                return View("NoPermission");
            }

            //check if ticket for current event exists
            Ticket ticket = _context.Tickets.Where(t => t.EventId == eventId).FirstOrDefault();
            if (ticket == null)
            {
                //ticket does not exist, create one
                ticket = new Ticket();
                ticket.EventId = eventId;
                ticket.Event = _context.Events.Where(e => e.Id == eventId).First();
                _context.Tickets.Add(ticket);
            }

            _context.UserTickets.Add(new UserTickets() { UserId = loggedUser.Id, Ticket = ticket });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Events
        public async Task<IActionResult> Index(string eventFilter)
        {
            // Can be accessed by logged users
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null)
            {
                return View("NoPermission");
            }

            if (string.IsNullOrEmpty(eventFilter))
                return _context.Events != null ?
                              View(await _context.Events.ToListAsync()) :
                              Problem("Entity set 'ApplicationDbContext.Events'  is null.");
            else
                return _context.Events != null ?
                    View(await _context.Events.Where(e => e.Name.ToLower().StartsWith(eventFilter)).ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.Events' is null");
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            // only logged in users that are admins can access!
            if (loggedUser != null && loggedUser.RoleId == 1)
            {
                if (id == null || _context.Events == null)
                {
                    return NotFound();
                }

                var @event = await _context.Events
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (@event == null)
                {
                    return NotFound();
                }

                return View(@event);
            }
            return View("NoPermission");
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            // only logged in users that are admins can access!
            if (loggedUser != null && loggedUser.RoleId == 1)
                return View();
            else
                return View("NoPermission");
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] EventCreateVM model, IFormFile? imageFile)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            // only logged in users that are admins can access!
            if (loggedUser != null && loggedUser.RoleId == 1)
            {
                model.ImageUrl = "unset";

                if (ModelState.IsValid)
                {
                    //Retrieves the image from the http request and saves it in the root folder
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        //concatinates the date and time of the upload in order to avoid duplicated image names
                        var fileNameWithExtension = Path.GetFileName(imageFile.FileName);
                        var fileName = fileNameWithExtension.Split('.')[0].Trim() + DateTime.Now.ToString("yyyMMddHHmmssff");
                        var extension = fileNameWithExtension.Substring(fileNameWithExtension.LastIndexOf('.'));
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", String.Concat(fileName, '.', extension));

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        model.ImageUrl = "/image/" + String.Concat(fileName, '.', extension);
                    }

                    Event evnt = new Event();
                    evnt.Name = model.Name;
                    evnt.Description = model.Description;
                    evnt.Date = model.Date;
                    evnt.imageUrl = model.ImageUrl;

                    _context.Add(evnt);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model);
            }
            else
                return View("NoPermission");
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            // only logged in users that are admins can access!
            if (loggedUser != null && loggedUser.RoleId == 1)
            {
                if (id == null || _context.Events == null)
                {
                    return NotFound();
                }

                var evnt = await _context.Events.FindAsync(id);

                // map
                var model = new EventEditVM();
                model.Id = evnt.Id;
                model.Name = evnt.Name;
                model.Description = evnt.Description;
                model.Date = evnt.Date;
                model.ImageUrl = evnt.imageUrl;

                if (evnt == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            return View("NoPermission");
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] EventEditVM model, IFormFile? imageFile)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            // only logged in users that are admins can access!
            if (loggedUser != null && loggedUser.RoleId == 1)
            {
                if (id != model.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        //mapping
                        var evnt = _context.Events.Where(e => e.Id.Equals(id)).First();
                        evnt.Name = model.Name;
                        evnt.Description = model.Description;
                        evnt.Date = model.Date;

                        // Checks if the image of the game has been changed
                        if (imageFile != null)
                        {
                            // Deletes the old image from the root folder
                            string oldImageUrl = model.ImageUrl;

                            if (!string.IsNullOrEmpty(oldImageUrl))
                            {
                                string url = oldImageUrl.Split('/', StringSplitOptions.RemoveEmptyEntries)[1].Trim();

                                var imageFullPath = string.Concat(Directory.GetCurrentDirectory(), "\\wwwroot\\", "image\\", url);
                                if (System.IO.File.Exists(imageFullPath))
                                {
                                    System.IO.File.Delete(imageFullPath);
                                }
                            }

                            var fileNameWithExtension = Path.GetFileName(imageFile.FileName);
                            var fileName = fileNameWithExtension.Split('.')[0].Trim() + DateTime.Now.ToString("yyyMMddHHmmssff");
                            var extension = fileNameWithExtension.Split('.')[1].Trim();
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", String.Concat(fileName, '.', extension));

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }

                            evnt.imageUrl = "/image/" + String.Concat(fileName, '.', extension);
                            model.ImageUrl = evnt.imageUrl;
                        }
                        else
                        {
                            // evnt.imageUrl = model.ImageUrl;
                            model.ImageUrl = evnt.imageUrl;
                        }

                        _context.Update(evnt);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EventExists(model.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            return View("NoPermission");
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            // only logged in users that are admins can access!
            if (loggedUser != null && loggedUser.RoleId == 1)
            {
                if (id == null || _context.Events == null)
                {
                    return NotFound();
                }

                var @event = await _context.Events
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (@event == null)
                {
                    return NotFound();
                }
                return View(@event);
            }

            return View("NoPermission");

        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            // only logged in users that are admins can access!
            if (loggedUser != null && loggedUser.RoleId == 1)
            {
                if (_context.Events == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Events'  is null.");
                }
                var @event = await _context.Events.FindAsync(id);
                if (@event != null)
                {
                    _context.Events.Remove(@event);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("NoPermission");
        }

        private bool EventExists(int id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
