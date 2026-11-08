
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesAdmin.Models;
using MoviesAdmin.Services;

[Authorize]
public class MoviesController : Controller
{
    private readonly MoviesAdminContext _context;
    private readonly AzureStorageService _blobService;

    public MoviesController(MoviesAdminContext context, AzureStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index()    
    {
        List<Movie> movies = await _context.Movie
            .Include(t => t.Genre)
            .Include(t => t.AudienceRating)
            .Include(t => t.Director)
            .ToListAsync();

        return View(movies);
    }

    // GET: MOVIES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .Include(m => m.Genre)
            .Include(m => m.AudienceRating)
            .Include(m => m.Director)
            .Include(m => m.CriticReview.Where(c => c.IsPublished))
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // GET: MOVIES/Create
    [Authorize(Roles = "admin")]
    public IActionResult Create()
    {
        var genres = _context.Genre.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");

        var ratings = _context.AudienceRating.ToList();
        ViewBag.AudienceRatingId = new SelectList(ratings, "Id", "Title");

        var directors = _context.Director.ToList();
        ViewBag.DirectorId = new SelectList(directors, "Id", "Name", "ImageUrl");

        return View();
    }

    // POST: MOVIES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([Bind("Id,Title,Synopsis,Runtime,ReleaseDate,DirectorId,GenreId,AudienceRatingId")] Movie movie, IFormFile? imageFile) // Collect inputs and file inputs
    {
        // Validate if image file is uploaded
        if (imageFile == null)
        {
            ModelState.AddModelError("ImageFile", "Image file upload is required.");
        }

        if (ModelState.IsValid)
        {
            //Setup for unique filename so files are not overwritten
            var extension = Path.GetExtension(imageFile!.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            // save file in azure storage
            string imageUrl = await _blobService.UploadFileAsync(imageFile, uniqueFileName);

            // save image url in record
            movie.ImageUrl = imageUrl;

            // save record in databsae
            _context.Add(movie);
            await _context.SaveChangesAsync();

            // redirect to index after creating movie
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Edit/5
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        var genres = _context.Genre.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");

        var ratings = _context.AudienceRating.ToList();
        ViewBag.AudienceRatingId = new SelectList(ratings, "Id", "Title");

        var directors = _context.Director.ToList();
        ViewBag.DirectorId = new SelectList(directors, "Id", "Name", "ImageUrl");

        return View(movie);
    }

    // POST: MOVIES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Synopsis,Runtime,ReleaseDate,DirectorId,GenreId,AudienceRatingId,ImageUrl")] Movie movie, IFormFile? imageFile)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // replace image when new file is uploaded
                if (imageFile != null && imageFile.Length > 0)
                {
                    var extension = Path.GetExtension(imageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    movie.ImageUrl = await _blobService.UploadFileAsync(imageFile, uniqueFileName);
                }

                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
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

        var genres = _context.Genre.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");

        var ratings = _context.AudienceRating.ToList();
        ViewBag.AudienceRatingId = new SelectList(ratings, "Id", "Title");

        var directors = _context.Director.ToList();
        ViewBag.DirectorId = new SelectList(directors, "Id", "Name", "ImageUrl");

        return View(movie);
    }

    // GET: MOVIES/Delete/5
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movie
            .Include(m => m.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        var genres = _context.Genre.ToList();
        ViewBag.GenreId = new SelectList(genres, "Id", "Title");

        var ratings = _context.AudienceRating.ToList();
        ViewBag.AudienceRatingId = new SelectList(ratings, "Id", "Title");

        var directors = _context.Director.ToList();
        ViewBag.DirectorId = new SelectList(directors, "Id", "Name", "ImageUrl");

        return View(movie);
    }

    // POST: MOVIES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}
