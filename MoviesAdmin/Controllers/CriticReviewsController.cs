using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin;
using MoviesAdmin.Models;

[Authorize]
public class CriticReviewsController : Controller
{
    private readonly MoviesAdminContext _context;

    // logged in user
    private string? _userId;

    public CriticReviewsController(MoviesAdminContext context)
    {
        _context = context;
    }

    // Called after constructor but before method
    // userId should no longer be null
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        _userId = User.GetUserId();
    }

    // GET: CriticReviews
    public async Task<IActionResult> Index()
    {
        return View(await _context.CriticReview
            .Where(c => c.CreatedBy == _userId)
            .Include(c => c.Movie)
            .ToListAsync());
    }

    // GET: CriticReviews/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticReview = await _context.CriticReview
            .Include(c => c.Movie)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (criticReview == null)
        {
            return NotFound();
        }

        return View(criticReview);
    }

    // GET: CriticReviews/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CriticReviews/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MovieId,Description,Rating,IsPublished,CreatedBy,CreatedDate")] CriticReview criticReview)
    {
        if (ModelState.IsValid)
        {
            _context.Add(criticReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(criticReview);
    }

    // GET: CriticReviews/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticReview = await _context.CriticReview.FindAsync(id);
        if (criticReview == null)
        {
            return NotFound();
        }

        return View(criticReview);
    }

    // POST: CriticReviews/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,Description,Rating,IsPublished,CreatedBy,CreatedDate")] CriticReview criticReview)
    {
        if (id != criticReview.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(criticReview);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriticReviewExists(criticReview.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(criticReview);
    }

    // GET: CriticReviews/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticReview = await _context.CriticReview
            .Include(c => c.Movie)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (criticReview == null)
        {
            return NotFound();
        }

        return View(criticReview);
    }

    // POST: CriticReviews/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var criticReview = await _context.CriticReview.FindAsync(id);
        if (criticReview != null)
        {
            _context.CriticReview.Remove(criticReview);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CriticReviewExists(int id)
    {
        return _context.CriticReview.Any(c => c.Id == id);
    }
}
