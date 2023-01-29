namespace MoviesAPI.Repos
{
    public class GenreRepository : IGenreRepository
    {
        private readonly AppDBContext _Context;

        public GenreRepository(AppDBContext context)
        {
            _Context = context;
        }

        public async Task<Genre> AddAsync(Genre genre)
        {
            await _Context.Genres.AddAsync(genre);

            _Context.SaveChanges();

            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _Context.Genres.Remove(genre);
            _Context.SaveChanges();

            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            List<Genre> genres =await _Context.Genres.ToListAsync();

            return genres;
        }

        public async Task<Genre> GetByIdAsync(byte id)
        {
            Genre? genre = await _Context.Genres.SingleOrDefaultAsync(g => g.GenreId == id);

            return genre;
        }

        public async Task<bool> IsValidGenreIdAsync(byte id)
        {
            bool isValid = await _Context.Genres.AnyAsync(g => g.GenreId == id);

            return isValid;
        }

        public Genre Update(Genre genre)
        {
            _Context.Genres.Update(genre);
            _Context.SaveChanges();

            return genre;
        }
    }
}
