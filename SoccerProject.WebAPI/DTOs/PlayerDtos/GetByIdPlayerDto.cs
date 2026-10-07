using SoccerProject.WebAPI.DTOs.PlayerCareerDtos;

namespace SoccerProject.WebAPI.DTOs.PlayerDtos
{
    public class GetByIdPlayerDto
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; }
        public int ShirtNumber { get; set; }
        public string Position { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public string? PhotoUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }

        // Oynadığı kulüpler (kariyer geçmişi)
        public List<ResultPlayerCareerDto> Careers { get; set; } = new();

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
