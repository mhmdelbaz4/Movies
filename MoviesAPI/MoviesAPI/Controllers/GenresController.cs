using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace MoviesAPI.Controllers
{
    [Route("api/Genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly AppDBContext _context;

        public GenresController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AllGenres()
        {
            List<Genre> genres = await _context.Genres.ToListAsync();

            return Ok(genres);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GenreById(byte id)
        {
            Genre? genre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            if (genre == null)
                return NotFound($"No Genre with Id :{id}");

            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreDto dto)
        {
            Genre genre = new()
            {
                Name = dto.Name
            };

            _context.Genres.Add(genre);
            _context.SaveChanges();

            return Ok(genre);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGenre(int id, GenreDto dto)
        {
            
            Genre? updatedGenre =await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            if (updatedGenre == null)
                return NotFound($"No Genre with Id :{id}");

            updatedGenre.Name = dto.Name;
            _context.SaveChanges();

            return Ok(updatedGenre);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            Genre? deletedGenre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == id);

            if (deletedGenre == null)
                return NotFound($"No Genre with Id :{id}");

            _context.Genres.Remove(deletedGenre);
            _context.SaveChanges();

            return Ok(deletedGenre);
        }

    }
}
