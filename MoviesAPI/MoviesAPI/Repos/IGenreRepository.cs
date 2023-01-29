namespace MoviesAPI.Repos
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllAsync();

        Task<Genre> GetByIdAsync(byte id);

        Task<Genre> AddAsync(Genre genre);

        Genre Update(Genre genre);

        Genre Delete(Genre genre);

        Task<bool> IsValidGenreIdAsync(byte id);

    }
}
