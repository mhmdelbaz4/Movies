namespace MoviesAPI.Dtos
{
    public class MovieBaseDto
    {
        [MaxLength(200)]
        public string Title { get; set; }

        [Range(1950, 2023)]
        public int Year { get; set; }

        [Range(0, 10)]
        public double Rate { get; set; }

        [MaxLength(2500)]
        public string StoryLine { get; set; }

        public byte GenreId { get; set; }
    }
}
