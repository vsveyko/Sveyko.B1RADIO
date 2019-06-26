using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sveyko.B1RADIO.Models;
using System.Diagnostics;

namespace Sveyko.B1RADIO.Controllers
{
    public class SoundtracksController : Controller
    {
        private readonly B1RADIOContext _context;

        public SoundtracksController(B1RADIOContext context)
        {
            _context = context;
        }

        // GET: Soundtracks
        public async Task<IActionResult> Index()
        {
            var b1RADIOContext = _context.Soundtrack.Include(s => s.Genre).Include(s => s.Singer);
            return View(await b1RADIOContext.ToListAsync());
        }

        // GET: Soundtracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soundtrack = await _context.Soundtrack
                .Include(s => s.Genre)
                .Include(s => s.Singer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soundtrack == null)
            {
                return NotFound();
            }

            return View(soundtrack);
        }

        // GET: Soundtracks/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name");
            ViewData["SingerId"] = new SelectList(_context.Singer, "Id", "Name");
            return View();
        }

        // POST: Soundtracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GenreId,SingerId,Title,Filepath")] Soundtrack soundtrack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soundtrack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", soundtrack.GenreId);
            ViewData["SingerId"] = new SelectList(_context.Singer, "Id", "Name", soundtrack.SingerId);
            return View(soundtrack);
        }

        // GET: Soundtracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soundtrack = await _context.Soundtrack.FindAsync(id);
            if (soundtrack == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", soundtrack.GenreId);
            ViewData["SingerId"] = new SelectList(_context.Singer, "Id", "Name", soundtrack.SingerId);
            return View(soundtrack);
        }

        // POST: Soundtracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenreId,SingerId,Title,Filepath")] Soundtrack soundtrack)
        {
            if (id != soundtrack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soundtrack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoundtrackExists(soundtrack.Id))
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
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "Name", soundtrack.GenreId);
            ViewData["SingerId"] = new SelectList(_context.Singer, "Id", "Name", soundtrack.SingerId);
            return View(soundtrack);
        }

        // GET: Soundtracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soundtrack = await _context.Soundtrack
                .Include(s => s.Genre)
                .Include(s => s.Singer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soundtrack == null)
            {
                return NotFound();
            }

            return View(soundtrack);
        }

        // POST: Soundtracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soundtrack = await _context.Soundtrack.FindAsync(id);
            _context.Soundtrack.Remove(soundtrack);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoundtrackExists(int id)
        {
            return _context.Soundtrack.Any(e => e.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
