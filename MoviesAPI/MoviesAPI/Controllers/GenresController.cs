using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Repos;

namespace MoviesAPI.Controllers
{
    [Route("api/Genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        [HttpGet]
        public async Task<IActionResult> AllGenres()
        {
            IEnumerable<Genre> genres = await _genreRepository.GetAllAsync();

            return Ok(genres);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GenreById(byte id)
        {
            Genre? genre = await _genreRepository.GetByIdAsync(id);

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

            await _genreRepository.AddAsync(genre);

            return Ok(genre);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGenre(byte id, GenreDto dto)
        {

            Genre? updatedGenre = await _genreRepository.GetByIdAsync(id);

            if (updatedGenre == null)
                return NotFound($"No Genre with Id :{id}");

            updatedGenre.Name = dto.Name;
            _genreRepository.Update(updatedGenre);

            return Ok(updatedGenre);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGenre(byte id)
        {
            Genre? deletedGenre = await _genreRepository.GetByIdAsync(id);

            if (deletedGenre == null)
                return NotFound($"No Genre with Id :{id}");

            _genreRepository.Delete(deletedGenre);

            return Ok(deletedGenre);
        }

    }
}
