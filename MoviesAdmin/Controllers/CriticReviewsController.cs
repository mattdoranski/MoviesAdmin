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
            .FirstOrDefaultAsync(c => c.Id == id && (c.IsPublished || c.CreatedBy == _userId));
        if (criticReview == null)
        {
            return NotFound();
        }

        return View(criticReview);
    }

    // GET: CriticReviews/Create
    public async Task<IActionResult> Create(int movieId)
    {
        var exists = await _context.CriticReview
            .FirstOrDefaultAsync(c => c.MovieId == movieId && c.CreatedBy == _userId);

        if (exists != null)
        {
            if (!exists.IsPublished)
            {
                //Draft exists
                return RedirectToAction(nameof(Edit), new { id = exists.Id });
            }

            //Published exists
            TempData["Error"] = "You have already reviewed this movie.";
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        ViewBag.MovieId = movieId;

        return View();
    }

    // POST: CriticReviews/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,MovieId,Description,Rating,IsPublished,CreatedBy,CreatedDate")] CriticReview criticReview)
    {
        //Initailize
        criticReview.CreatedBy = _userId!;
        criticReview.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

        if (ModelState.IsValid)
        {
            _context.Add(criticReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.MovieId = criticReview.MovieId;
        return View(criticReview);
    }

    // GET: CriticReviews/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var criticReview = await _context.CriticReview
            .Include(c => c.Movie)
            .FirstOrDefaultAsync(c => c.Id == id && c.CreatedBy == _userId);
        if (criticReview == null)
        {
            return NotFound();
        }

        if (criticReview.IsPublished) return Forbid();

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

        //TODO: Check if EDIT works for published reviews

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
            .FirstOrDefaultAsync(c => c.Id == id && c.CreatedBy == _userId);
        if (criticReview == null)
        {
            return NotFound();
        }

        if (criticReview.IsPublished) return Forbid();

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
