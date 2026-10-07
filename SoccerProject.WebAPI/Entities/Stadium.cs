namespace SoccerProject.WebAPI.Entities
{
    public class Stadium
    {
        public int StadiumId { get; set; }
        public string Name { get; set; }        // Stadyum adı (ör. San Siro)
        public string City { get; set; }         // Şehir (ör. Milano)
        public int Capacity { get; set; }        // Kapasite
    }
}
