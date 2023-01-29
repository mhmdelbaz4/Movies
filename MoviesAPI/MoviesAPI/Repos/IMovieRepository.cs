namespace MoviesAPI.Repos
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();

        Task<Movie> GetByIdAsync(int id);

        Task<Movie> AddAsync(Movie movie);

        Movie Update(Movie movie);

        Movie Delete(Movie movie);

        Task<List<Movie>> GetAllByGenreId(byte id);
    }
}
