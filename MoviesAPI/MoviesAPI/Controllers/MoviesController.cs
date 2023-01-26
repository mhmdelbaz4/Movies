using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesAPI.Controllers
{
    [Route("api/Movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly long _maxAllowedPosterSize;
        private readonly List<string> _allowedPosterExtensions;

        public MoviesController(AppDBContext context)
        {
            _context = context;
            _maxAllowedPosterSize = 1048576;
            _allowedPosterExtensions = new() { ".jpg", ".png" };
        }

        [HttpGet]
        public async Task<IActionResult> AllMovies()
        {
            List<Movie> movies = await _context.Movies.ToListAsync();

            if (movies.Count == 0)
                return NotFound("No Movies Found");

            return Ok(movies);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> MovieById(int id)
        {
            Movie? movie =await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No movie found with id :{id}");

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm]MovieDto dto)
        {
            bool isValidGenreId =await _context.Genres.AnyAsync(g => g.GenreId == dto.GenreId);
            if(! isValidGenreId)
                return BadRequest($"No genre found with ID : {dto.GenreId}");

            if(! IsValidPoster(dto.Poster ,out string msg))
                return BadRequest(msg);

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            Movie movie = new()
            {
                Title = dto.Title,
                Rate = dto.Rate,
                StoryLine = dto.StoryLine,
                Year = dto.Year,
                GenreId = dto.GenreId,
                Poster = dataStream.ToArray()
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            return Ok(movie);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMovie([FromForm]MovieDto dto ,int id)
        {
            Movie? UpdatedMovie = await _context.Movies.FindAsync(id);
            if (UpdatedMovie == null)
                return NotFound($"No movie found with ID : {id}");

            bool isValidGenreId = await _context.Genres.AnyAsync(g => g.GenreId == dto.GenreId);
            if (!isValidGenreId)
                return BadRequest($"No genre found with ID : {dto.GenreId}");

            if (!IsValidPoster(dto.Poster, out string msg))
                return BadRequest(msg);

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            UpdatedMovie.Title = dto.Title;
            UpdatedMovie.Rate = dto.Rate;
            UpdatedMovie.StoryLine = dto.StoryLine;
            UpdatedMovie.Year = dto.Year;
            UpdatedMovie.GenreId = dto.GenreId;
            UpdatedMovie.Poster = dataStream.ToArray();

            _context.SaveChanges();

            return Ok(UpdatedMovie);
           
        }

        public bool IsValidPoster(IFormFile poster ,out string msg)
        {
            msg = "valid";
            
            if(poster.Length > _maxAllowedPosterSize)
            {
                msg = "max allowed poster size is 1M";
                return false;
            }

            if (! _allowedPosterExtensions.Contains(Path.GetExtension(poster.FileName)))
            {
                msg = "only png and jpg is allowed poster extensions";
                return false;
            }

            return true;
        }
    }
}
