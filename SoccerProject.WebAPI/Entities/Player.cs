namespace SoccerProject.WebAPI.Entities
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; }      // Ad Soyad
        public int ShirtNumber { get; set; }       // Forma numarası
        public string Position { get; set; }       // Kaleci / Defans / Orta saha / Forvet

        // Profil bilgileri
        public string? PhotoUrl { get; set; }      // Fotoğraf (URL)
        public DateTime? BirthDate { get; set; }   // Doğum tarihi (yaş bundan hesaplanır)
        public string? Nationality { get; set; }   // Uyruk (opsiyonel)

        // Futbolcu bir takıma aittir (FK)
        public int TeamId { get; set; }
        public Team Team { get; set; }

        // Oynadığı kulüpler (kariyer geçmişi) - ayrı entity
        public IList<PlayerCareer> Careers { get; set; }
    }
}
