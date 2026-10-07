namespace SoccerProject.WebAPI.Entities
{
    // Bir futbolcunun oynadığı kulüpler (kariyer geçmişi).
    public class PlayerCareer
    {
        public int PlayerCareerId { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public string ClubName { get; set; }   // Kulüp adı
        public int StartYear { get; set; }      // Başlangıç yılı
        public int? EndYear { get; set; }       // Bitiş yılı (null => halen oynuyor)
    }
}
