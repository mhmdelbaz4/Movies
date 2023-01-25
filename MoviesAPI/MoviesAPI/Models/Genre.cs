namespace MoviesAPI.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte GenreId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
