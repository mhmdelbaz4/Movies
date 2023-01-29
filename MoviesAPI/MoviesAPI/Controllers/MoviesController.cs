using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesAPI.Controllers
{
    [Route("api/Movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _moviesRepository;
        private readonly IGenreRepository _genresRepository;

        private readonly long _maxAllowedPosterSize;
        private readonly List<string> _allowedPosterExtensions;

        public MoviesController(AppDBContext context, IMovieRepository moviesRepository, IGenreRepository genresRepository)
        {
            _maxAllowedPosterSize = 1048576;
            _allowedPosterExtensions = new() { ".jpg", ".png" };
            _moviesRepository = moviesRepository;
            _genresRepository = genresRepository;
        }

        [HttpGet]
        public async Task<IActionResult> getALlAsync()
        {
            IEnumerable<Movie> movies = await _moviesRepository.GetAllAsync();

            return Ok(movies);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Movie? movie =await _moviesRepository.GetByIdAsync(id);

            if (movie == null)
                return NotFound($"No movie found with id :{id}");

            return Ok(movie);
        }

        [HttpGet("GetByGenreIdAsync/{genreId:int}")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            List<Movie> movies = await _moviesRepository.GetAllByGenreId(genreId);

            if (movies.Count == 0)
                return NotFound("No Movies Found");

            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm]CreateMovieDto dto)
        {
            bool isValidGenreId = await _genresRepository.IsValidGenreIdAsync(dto.GenreId);
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

            await _moviesRepository.AddAsync(movie);

            return Ok(movie);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromForm]UpdateMovieDto dto ,int id)
        {
            Movie? UpdatedMovie = await _moviesRepository.GetByIdAsync(id);
            if (UpdatedMovie == null)
                return NotFound($"No movie found with ID : {id}");

            bool IsValidGenreId = await _genresRepository.IsValidGenreIdAsync(dto.GenreId);
            if (! IsValidGenreId)
                return BadRequest($"No genre found with ID : {dto.GenreId}");

            if (dto.Poster != null)
            {
                if (!IsValidPoster(dto.Poster, out string msg))
                    return BadRequest(msg);

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                UpdatedMovie.Poster = dataStream.ToArray();
            }

            UpdatedMovie.Title = dto.Title;
            UpdatedMovie.Rate = dto.Rate;
            UpdatedMovie.StoryLine = dto.StoryLine;
            UpdatedMovie.Year = dto.Year;
            UpdatedMovie.GenreId = dto.GenreId;

            _moviesRepository.Update(UpdatedMovie);

            return Ok(UpdatedMovie);
           
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            Movie? DeletedMovie = await _moviesRepository.GetByIdAsync(id);

            if (DeletedMovie == null)
                return NotFound($"No movie found with Id :{id}");

            _moviesRepository.Delete(DeletedMovie);

            return Ok(DeletedMovie);
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
