namespace MoviesAPI.Repos
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDBContext _context;

        public MovieRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
             await _context.Movies.AddAsync(movie); 

             _context.SaveChanges(); 
            
            return movie;   
        }

        public Movie Delete(Movie movie)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            List<Movie> movies = await _context.Movies
                                               .OrderByDescending(m => m.Rate)
                                               .Include(m => m.Genre)
                                               .ToListAsync();

            return movies;
        }

        public async Task<List<Movie>> GetAllByGenreId(byte id)
        {
            List<Movie> movies =await _context.Movies.Where(m => m.GenreId == id)
                                                     .OrderByDescending(m => m.Rate)
                                                     .ToListAsync();

            return movies;
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            Movie? movie = await _context.Movies
                                         .SingleOrDefaultAsync(m => m.MovieId == id);

            return movie;
        }

        public Movie Update(Movie movie)
        {
            _context.Movies.Update(movie);

            return movie;
        }
    }
}
