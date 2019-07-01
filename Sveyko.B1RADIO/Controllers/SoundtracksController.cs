using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sveyko.B1RADIO.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using Sveyko.B1RADIO.Utils;

namespace Sveyko.B1RADIO.Controllers
{
    public class SoundtracksController : Controller
    {
        private readonly B1RADIOContext _context;

        private string TmpFileDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/soundtracks/temp");
        private string WorkFileDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/soundtracks");

        public SoundtracksController(B1RADIOContext context)
        {
            _context = context;
        }

        // GET: Soundtracks
        public async Task<IActionResult> Index(string sortOrder, int searchTypeID, string searchString, string currentFilter, int? pageIndex)
        {
            var b1RADIOContext = _context.Soundtrack.Include(s => s.Genre).Include(s => s.Singer);

            ViewData["CurrentSortOrder"] = String.IsNullOrEmpty(sortOrder) ? "genre_asc" : sortOrder;
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            IQueryable<Soundtrack> soundtracks = from s in b1RADIOContext
                                                select s;

            if ((searchTypeID > 0) && (!String.IsNullOrEmpty(searchString)))
            {
                switch (searchTypeID)
                {
                    case 1:
                        soundtracks = soundtracks.Where(s => s.Genre.Name.Contains(searchString));
                        break;
                    case 2:
                        soundtracks = soundtracks.Where(s => s.Singer.Name.Contains(searchString));
                        break;
                    case 3:
                        soundtracks = soundtracks.Where(s => s.Title.Contains(searchString));
                        break;
                }
            }

            switch (sortOrder)
            {
                case "genre_desc":
                    soundtracks = soundtracks.OrderByDescending(s => s.Genre.Name);
                    break;
                case "singer_asc":
                    soundtracks = soundtracks.OrderBy(s => s.Singer.Name);
                    break;
                case "singer_desc":
                    soundtracks = soundtracks.OrderByDescending(s => s.Singer.Name);
                    break;
                case "title_asc":
                    soundtracks = soundtracks.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    soundtracks = soundtracks.OrderByDescending(s => s.Title);
                    break;
                default:
                    soundtracks = soundtracks.OrderBy(s => s.Genre.Name);
                    break;
            }

            int pageSize = 8;
            var soundtrackList = await PaginatedList<Soundtrack>.CreateAsync(soundtracks.AsNoTracking(), pageIndex ?? 1, pageSize);
            //var soundtrackList = await soundtracks.AsNoTracking().ToListAsync();
            return View(soundtrackList);
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
            ViewData["GenreId"] = new SelectList(_context.Genre.OrderBy(e => e.Name), "Id", "Name");
            ViewData["SingerId"] = new SelectList(_context.Singer.OrderBy(e => e.Name), "Id", "Name");
            return View();
        }

        // POST: Soundtracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, GenreId, SingerId, Title, ServerFilename, ClientFilename")] Soundtrack soundtrack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soundtrack);
                System.IO.File.Move(Path.Combine(TmpFileDir, soundtrack.ServerFilename), Path.Combine(WorkFileDir, soundtrack.ServerFilename));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genre.OrderBy(e => e.Name), "Id", "Name", soundtrack.GenreId);
            ViewData["SingerId"] = new SelectList(_context.Singer.OrderBy(e => e.Name), "Id", "Name", soundtrack.SingerId);
            
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
            ViewData["GenreId"] = new SelectList(_context.Genre.OrderBy(e => e.Name), "Id", "Name", soundtrack.GenreId);
            ViewData["SingerId"] = new SelectList(_context.Singer.OrderBy(e => e.Name), "Id", "Name", soundtrack.SingerId);
            return View(soundtrack);
        }

        // POST: Soundtracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, GenreId, SingerId, Title, ServerFilename, ClientFilename")] Soundtrack soundtrack)
        {
            if (id != soundtrack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Soundtrack existingSoundtrack = _context.Soundtrack.FirstOrDefault(e => e.Id == soundtrack.Id);
                try
                {
                    _context.Entry<Soundtrack>(existingSoundtrack).State = EntityState.Detached;
                    _context.Update(soundtrack);
                    if (System.IO.File.Exists(Path.Combine(TmpFileDir, soundtrack.ServerFilename)))
                    {
                        if (existingSoundtrack != null)
                            System.IO.File.Delete(Path.Combine(WorkFileDir, existingSoundtrack.ServerFilename));
                        System.IO.File.Move(Path.Combine(TmpFileDir, soundtrack.ServerFilename), Path.Combine(WorkFileDir, soundtrack.ServerFilename));
                    }
                    
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
            ViewData["GenreId"] = new SelectList(_context.Genre.OrderBy(e => e.Name), "Id", "Name", soundtrack.GenreId);
            ViewData["SingerId"] = new SelectList(_context.Singer.OrderBy(e => e.Name), "Id", "Name", soundtrack.SingerId);
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

            var fileName = Path.Combine(WorkFileDir, soundtrack.ServerFilename);
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

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

        public IActionResult AddSinger()
        {
            var model = new Singer();

            return PartialView("_AddSingerPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSinger(Singer model)
        {
            Singer model_out = model;
            if (ModelState.IsValid)
            {
                Singer existingSinger = _context.Singer.FirstOrDefault(e => e.Name == model_out.Name);
                if (existingSinger == null)
                {
                    _context.Singer.Add(model_out);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    model_out = existingSinger;
                }
            }
           // return PartialView("_AddSingerPartial", model_out);
            return Ok(model_out);
        }

        [HttpGet]
        public JsonResult GetSingerList()
        {
            return Json(new SelectList(_context.Singer.OrderBy(e => e.Name), "Id", "Name"));
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            Directory.CreateDirectory(TmpFileDir);
            string serverFileName = string.Format(@"{0}" + Path.GetExtension(file.FileName), Guid.NewGuid());
            var path = Path.Combine(TmpFileDir, serverFileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(serverFileName);
        }

        public static void PlaySound(string file)
        {
            Process.Start(@"powershell", $@"-c (New-Object Media.SoundPlayer '{file}').PlaySync();");
        }
    }
}
