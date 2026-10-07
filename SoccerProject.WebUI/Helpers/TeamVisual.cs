namespace SoccerProject.WebUI.Helpers
{
    public static class TeamVisual
    {
        public record Crest(string Code, string ColorA, string ColorB, string Ink);

        private static readonly Dictionary<string, Crest> Map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Inter"]      = new("INT", "#0A4FA8", "#101418", "#fff"),
            ["Milan"]      = new("MIL", "#C8141E", "#111111", "#fff"),
            ["Juventus"]   = new("JUV", "#16181D", "#E8E8E8", "#fff"),
            ["Napoli"]     = new("NAP", "#1E9BD7", "#0C3C78", "#fff"),
            ["Roma"]       = new("ROM", "#8E1F2F", "#F0B323", "#fff"),
            ["Lazio"]      = new("LAZ", "#8FD3F4", "#FFFFFF", "#0B2A4A"),
            ["Atalanta"]   = new("ATA", "#1E6FB8", "#111111", "#fff"),
            ["Fiorentina"] = new("FIO", "#5B2A86", "#FFFFFF", "#fff"),
            ["Bologna"]    = new("BOL", "#1A2F5A", "#A21C26", "#fff"),
            ["Torino"]     = new("TOR", "#7E1B1B", "#E9D9B0", "#fff"),
            ["Como"]       = new("COM", "#0B3E91", "#FFFFFF", "#fff"),
            ["Parma"]      = new("PAR", "#FFD200", "#1D3E8A", "#10224A"),
            ["Udinese"]    = new("UDI", "#26282C", "#FFFFFF", "#fff"),
            ["Genoa"]      = new("GEN", "#A2172E", "#002B5C", "#fff"),
            ["Sassuolo"]   = new("SAS", "#00A651", "#111111", "#fff"),
            ["Cagliari"]   = new("CAG", "#A5162B", "#0E2A56", "#fff"),
            ["Lecce"]      = new("LEC", "#F6D000", "#D21F26", "#3A1206"),
            ["Monza"]      = new("MON", "#D2122E", "#FFFFFF", "#fff"),
            ["Frosinone"]  = new("FRO", "#1C4E9A", "#FFD200", "#fff"),
            ["Venezia"]    = new("VEN", "#141414", "#F06A1A", "#fff"),
        };

        private static readonly string[] Palette =
        {
            "#2E7BFF", "#1FC98A", "#F2B53B", "#F2475C", "#8E1F2F",
            "#5B2A86", "#0B3E91", "#00A651", "#A2172E", "#1E6FB8"
        };

        public static Crest Get(string? teamName)
        {
            if (string.IsNullOrWhiteSpace(teamName))
                return new Crest("---", "#1F3358", "#101418", "#fff");

            if (Map.TryGetValue(teamName.Trim(), out var crest))
                return crest;

            var name = teamName.Trim();
            var code = new string(name.ToUpper(new System.Globalization.CultureInfo("tr-TR"))
                                      .Where(char.IsLetter).Take(3).ToArray());
            if (code.Length == 0) code = "?";

            int hash = 0;
            foreach (var ch in name) hash = (hash * 31 + ch) & 0x7fffffff;
            var color = Palette[hash % Palette.Length];
            return new Crest(code, color, "#101418", "#fff");
        }

        /// <summary>team-crest için inline style üretir.</summary>
        public static string Style(string? teamName)
        {
            var c = Get(teamName);
            return $"--crest-a:{c.ColorA};--crest-b:{c.ColorB};--crest-ink:{c.Ink}";
        }

        public static string Code(string? teamName) => Get(teamName).Code;

        public static string ColorA(string? teamName) => Get(teamName).ColorA;

        public static string ColorB(string? teamName) => Get(teamName).ColorB;
    }
}
