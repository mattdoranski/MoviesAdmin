using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;

[Authorize(Roles = "admin")]
public class DirectorsController : Controller
{
    private readonly MoviesAdminContext _context;

    public DirectorsController(MoviesAdminContext context)
    {
        _context = context;
    }

    // GET: Directors
    public async Task<IActionResult> Index()
    {
        return View(await _context.Director.ToListAsync());
    }

    // GET: Directors/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Director.FirstOrDefaultAsync(d => d.Id == id);
        if (director == null)
        {
            return NotFound();
        }

        return View(director);
    }

    // GET: Directors/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Directors/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,ImageUrl")] Director director)
    {
        if (ModelState.IsValid)
        {
            _context.Add(director);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(director);
    }

    // GET: Directors/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Director.FindAsync(id);
        if (director == null)
        {
            return NotFound();
        }

        return View(director);
    }

    // POST: Directors/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImageUrl")] Director director)
    {
        if (id != director.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(director);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorExists(director.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(director);
    }

    // GET: Directors/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var director = await _context.Director.FirstOrDefaultAsync(d => d.Id == id);
        if (director == null)
        {
            return NotFound();
        }

        return View(director);
    }

    // POST: Directors/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var director = await _context.Director.FindAsync(id);
        if (director != null)
        {
            _context.Director.Remove(director);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool DirectorExists(int id)
    {
        return _context.Director.Any(d => d.Id == id);
    }
}
