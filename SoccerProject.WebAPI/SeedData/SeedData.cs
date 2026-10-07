using Microsoft.EntityFrameworkCore;
using SoccerProject.WebAPI.Context;
using SoccerProject.WebAPI.Entities;
using SoccerProject.WebAPI.Enums;

namespace SoccerProject.WebAPI.SeedData
{
    public static class DbInitializer
    {
        public static void Initialize(SoccerDbContext context)
        {
            context.Database.Migrate();

            if (context.Teams.Any())
                return; 

            var teamSeed = new (string Name, string Stadium, string Code)[]
            {
                ("Atalanta",   "Gewiss Stadium",                "ATA"),
                ("Bologna",    "Stadio Renato Dall'Ara",        "BOL"),
                ("Cagliari",   "Unipol Domus",                  "CAG"),
                ("Como",       "Stadio Giuseppe Sinigaglia",    "COM"),
                ("Fiorentina", "Stadio Artemio Franchi",        "FIO"),
                ("Frosinone",  "Stadio Benito Stirpe",          "FRO"),
                ("Genoa",      "Stadio Luigi Ferraris",         "GEN"),
                ("Inter",      "San Siro",                      "INT"),
                ("Juventus",   "Allianz Stadium",               "JUV"),
                ("Lazio",      "Stadio Olimpico",               "LAZ"),
                ("Lecce",      "Stadio Via del Mare",           "LEC"),
                ("Milan",      "San Siro",                      "MIL"),
                ("Monza",      "U-Power Stadium",               "MON"),
                ("Napoli",     "Stadio Diego Armando Maradona", "NAP"),
                ("Parma",      "Stadio Ennio Tardini",          "PAR"),
                ("Roma",       "Stadio Olimpico",               "ROM"),
                ("Sassuolo",   "Mapei Stadium",                 "SAS"),
                ("Torino",     "Stadio Olimpico Grande Torino", "TOR"),
                ("Udinese",    "Bluenergy Stadium",             "UDI"),
                ("Venezia",    "Stadio Pier Luigi Penzo",       "VEN"),
            };

            var teams = teamSeed
                .Select(t => new Team
                {
                    TeamName = t.Name,
                    LogoUrl = $"/images/teams/{t.Code.ToLower()}.png"
                })
                .ToList();

            context.Teams.AddRange(teams);
            context.SaveChanges();

            var map = context.Teams.ToDictionary(x => x.TeamName, x => x);
            var stadiumOf = teamSeed.ToDictionary(x => x.Name, x => x.Stadium);

            var fixtures = new List<Fixture>();

            Fixture Add(int week, DateTime date, string home, string away,
                        int? hs, int? aws, MatchStatus status)
            {
                bool played = status == MatchStatus.Finished || status == MatchStatus.InProgress;
                int homeScore = played ? (hs ?? 0) : 0;
                int awayScore = played ? (aws ?? 0) : 0;

                var f = new Fixture
                {
                    HomeTeamId = map[home].TeamId,
                    AwayTeamId = map[away].TeamId,
                    HomeTeamScore = homeScore,
                    AwayTeamScore = awayScore,
                    HomeTeamHalfScore = played ? homeScore / 2 : 0,
                    AwayTeamHalfScore = played ? awayScore / 2 : 0,
                    Stadium = stadiumOf[home],
                    MatchDate = date,
                    WeekNumber = week,
                    MatchStatus = status
                };
                fixtures.Add(f);
                return f;
            }

            Add(11, new DateTime(2026, 10, 31, 18, 0, 0), "Roma", "Inter", 1, 2, MatchStatus.Finished);
            Add(11, new DateTime(2026, 10, 31, 15, 0, 0), "Lazio", "Torino", 1, 1, MatchStatus.Finished);
            Add(11, new DateTime(2026, 10, 31, 20, 45, 0), "Napoli", "Atalanta", 2, 0, MatchStatus.Finished);
            Add(11, new DateTime(2026, 11, 1, 12, 30, 0), "Fiorentina", "Juventus", 0, 2, MatchStatus.Finished);
            Add(11, new DateTime(2026, 11, 1, 15, 0, 0), "Bologna", "Milan", 1, 3, MatchStatus.Finished);
            Add(11, new DateTime(2026, 11, 1, 15, 0, 0), "Genoa", "Como", 0, 1, MatchStatus.Finished);
            Add(11, new DateTime(2026, 11, 1, 18, 0, 0), "Parma", "Udinese", 2, 2, MatchStatus.Finished);
            Add(11, new DateTime(2026, 11, 1, 20, 45, 0), "Cagliari", "Lecce", 1, 0, MatchStatus.Finished);
            Add(11, new DateTime(2026, 11, 2, 18, 30, 0), "Monza", "Frosinone", 1, 1, MatchStatus.Finished);
            Add(11, new DateTime(2026, 11, 2, 20, 45, 0), "Venezia", "Sassuolo", 1, 2, MatchStatus.Finished);

            // ---------- 3) 12. Hafta (biten + canlı + başlamayan) ----------
            Add(12, new DateTime(2026, 11, 7, 15, 0, 0), "Atalanta", "Lazio", 3, 1, MatchStatus.Finished);
            Add(12, new DateTime(2026, 11, 7, 18, 0, 0), "Juventus", "Napoli", 1, 1, MatchStatus.Finished);
            // Öne çıkan maç: referans "Detaylı Maç Sonucu" ekranıyla birebir (18 Kasım 2026, 22:45, San Siro)
            var featured = Add(12, new DateTime(2026, 11, 18, 22, 45, 0), "Milan", "Roma", 2, 1, MatchStatus.Finished);
            featured.HomeTeamHalfScore = 2; // Tema: ilk yarı 2-0
            featured.AwayTeamHalfScore = 0;
            Add(12, new DateTime(2026, 11, 8, 12, 30, 0), "Inter", "Fiorentina", 2, 1, MatchStatus.Finished);
            Add(12, new DateTime(2026, 11, 8, 15, 0, 0), "Torino", "Bologna", 1, 0, MatchStatus.InProgress);
            Add(12, new DateTime(2026, 11, 8, 15, 0, 0), "Como", "Parma", 2, 0, MatchStatus.InProgress);
            Add(12, new DateTime(2026, 11, 8, 20, 45, 0), "Udinese", "Genoa", null, null, MatchStatus.NotStarted);
            Add(12, new DateTime(2026, 11, 9, 18, 30, 0), "Sassuolo", "Cagliari", null, null, MatchStatus.NotStarted);
            Add(12, new DateTime(2026, 11, 9, 20, 45, 0), "Lecce", "Monza", null, null, MatchStatus.NotStarted);
            Add(12, new DateTime(2026, 11, 9, 20, 45, 0), "Frosinone", "Venezia", null, null, MatchStatus.NotStarted);

            // ---------- 4) 13. Hafta (tamamı başlamadı) ----------
            Add(13, new DateTime(2026, 11, 14, 20, 45, 0), "Inter", "Juventus", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 14, 18, 0, 0), "Napoli", "Milan", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 14, 15, 0, 0), "Roma", "Como", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 15, 12, 30, 0), "Lazio", "Bologna", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 15, 15, 0, 0), "Fiorentina", "Atalanta", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 15, 15, 0, 0), "Genoa", "Torino", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 15, 18, 0, 0), "Cagliari", "Udinese", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 15, 20, 45, 0), "Parma", "Lecce", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 16, 18, 30, 0), "Venezia", "Monza", null, null, MatchStatus.NotStarted);
            Add(13, new DateTime(2026, 11, 16, 20, 45, 0), "Sassuolo", "Frosinone", null, null, MatchStatus.NotStarted);

            context.Fixtures.AddRange(fixtures);
            context.SaveChanges();

            // ---------- 5) Öne çıkan maç: Milan 2-1 Roma detayları ----------
            int milanId = map["Milan"].TeamId;
            int romaId = map["Roma"].TeamId;

            var events = new List<MatchEvent>
            {
                new() { FixtureId = featured.FixtureId, TeamId = milanId, PlayerName = "Rafael Leão",       MatchEventType = MatchEventType.Goal,           Description = "Asist: Theo Hernández",      Minute = 14 },
                new() { FixtureId = featured.FixtureId, TeamId = romaId,  PlayerName = "Paulo Dybala",       MatchEventType = MatchEventType.YellowCard,     Description = "Faul",                       Minute = 27 },
                new() { FixtureId = featured.FixtureId, TeamId = milanId, PlayerName = "Olivier Giroud",     MatchEventType = MatchEventType.Goal,           Description = "Kafa, asist: Rafael Leão",   Minute = 41 },
                new() { FixtureId = featured.FixtureId, TeamId = romaId,  PlayerName = "Bryan Cristante",    MatchEventType = MatchEventType.YellowCard,     Description = "Taktik faul",                Minute = 54 },
                new() { FixtureId = featured.FixtureId, TeamId = romaId,  PlayerName = "Lorenzo Pellegrini", MatchEventType = MatchEventType.SubstitutionIn, Description = "Çıkan: Stephan El Shaarawy", Minute = 58 },
                new() { FixtureId = featured.FixtureId, TeamId = milanId, PlayerName = "Christian Pulisic",  MatchEventType = MatchEventType.SubstitutionIn, Description = "Çıkan: Samuel Chukwueze",    Minute = 64 },
                new() { FixtureId = featured.FixtureId, TeamId = milanId, PlayerName = "Fikayo Tomori",      MatchEventType = MatchEventType.YellowCard,     Description = "Faul",                       Minute = 67 },
                new() { FixtureId = featured.FixtureId, TeamId = romaId,  PlayerName = "Tammy Abraham",      MatchEventType = MatchEventType.Goal,           Description = "Asist: Lorenzo Pellegrini",  Minute = 72 },
                new() { FixtureId = featured.FixtureId, TeamId = romaId,  PlayerName = "Gianluca Mancini",   MatchEventType = MatchEventType.YellowCard,     Description = "İtiraz",                     Minute = 74 },
                new() { FixtureId = featured.FixtureId, TeamId = milanId, PlayerName = "Yunus Musah",        MatchEventType = MatchEventType.SubstitutionIn, Description = "Çıkan: Ruben Loftus-Cheek",  Minute = 78 },
                new() { FixtureId = featured.FixtureId, TeamId = romaId,  PlayerName = "Andrea Belotti",     MatchEventType = MatchEventType.SubstitutionIn, Description = "Çıkan: Tammy Abraham",       Minute = 79 },
                new() { FixtureId = featured.FixtureId, TeamId = milanId, PlayerName = "Theo Hernández",     MatchEventType = MatchEventType.YellowCard,     Description = "Oyunu geciktirme",           Minute = 83 },
                new() { FixtureId = featured.FixtureId, TeamId = milanId, PlayerName = "Noah Okafor",        MatchEventType = MatchEventType.SubstitutionIn, Description = "Çıkan: Rafael Leão",         Minute = 88 },
                new() { FixtureId = featured.FixtureId, TeamId = romaId,  PlayerName = "Diego Llorente",     MatchEventType = MatchEventType.RedCard,        Description = "Gol şansını engelleme",      Minute = 90 },
            };
            context.MatchEvents.AddRange(events);

            var stat = new MatchStatic
            {
                FixtureId = featured.FixtureId,
                HomePossession = 54,
                AwayPossession = 46,
                HomeShoots = 15,
                AwayShoots = 10,
                HomeShootsOnTarget = 7,
                AwayShootsOnTarget = 4,
                HomePasses = 512,
                AwayPasses = 438,
                HomePassAccuracy = 87,
                AwayPassAccuracy = 82,
                HomeCorners = 6,
                AwayCorners = 4,
                HomeFouls = 11,
                AwayFouls = 13,
                HomeOffsides = 2,
                AwayOffsides = 1,
                HomeYellowCards = 2,
                AwayYellowCards = 3,
                HomeRedCards = 0,
                AwayRedCards = 1
            };
            context.MatchStatics.Add(stat);

            context.SaveChanges();
        }
    }
}
