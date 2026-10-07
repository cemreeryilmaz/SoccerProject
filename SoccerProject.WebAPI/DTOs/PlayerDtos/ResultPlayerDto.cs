namespace SoccerProject.WebAPI.DTOs.PlayerDtos
{
    public class ResultPlayerDto
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; }
        public int ShirtNumber { get; set; }
        public string Position { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }   // Listede takım adını göstermek için

        public string? PhotoUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }

        // Yaş, doğum tarihinden otomatik hesaplanır.
        public int? Age
        {
            get
            {
                if (BirthDate == null) return null;
                var today = DateTime.Today;
                int age = today.Year - BirthDate.Value.Year;
                if (BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
