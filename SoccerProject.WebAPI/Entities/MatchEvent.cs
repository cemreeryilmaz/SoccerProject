using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.Entities
{
    public class MatchEvent
    {
        public int MatchEventId { get; set; }
        public int FixtureId { get; set; }
        public Fixture Fixture { get; set; }
        public int TeamId { get; set; }
        public string PlayerName { get; set; }

        // Olayın ait olduğu gerçek futbolcu (opsiyonel FK). Maç detayında profile link için kullanılır.
        public int? PlayerId { get; set; }
        public Player Player { get; set; }

        public MatchEventType MatchEventType { get; set; }
        public string Description { get; set; }
        public int Minute { get; set; }
    }
}
