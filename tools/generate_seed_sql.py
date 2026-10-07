# -*- coding: utf-8 -*-
"""
Serie A 2026/27 - 38 haftalik ornek veri seti ureteci.
Ciktisi: SoccerProject.WebAPI/SeedData/SeriA_38Hafta_SeedData.sql (MSSQL)

Kurallar:
- 20 takim, cift devreli lig usulu (double round-robin) => 38 hafta, her hafta 10 mac.
- 1-20. haftalar OYNANMIS (MatchStatus=2 Finished), skorlu + istatistikli + gol olayli.
- 21-38. haftalar OYNANMAMIS (MatchStatus=0 NotStarted), 0-0.
- Tarihler: her hafta sonu Cumartesi/Pazar. 20. haftanin son maci Pazar 27 Eylul 2026 21:45.
- Milan (ev) - Roma (deplasman) one cikan maci: 2-1 (IY 2-0) + referans olaylar/istatistik.
"""
import random
import unicodedata
from urllib.parse import quote
from datetime import datetime, timedelta

random.seed(20262027)

TEAMS = [
    "Atalanta", "Bologna", "Cagliari", "Como", "Fiorentina", "Frosinone",
    "Genoa", "Inter", "Juventus", "Lazio", "Lecce", "Milan", "Monza",
    "Napoli", "Parma", "Roma", "Sassuolo", "Torino", "Udinese", "Venezia",
]

STADIUM = {
    "Atalanta": "Gewiss Stadium",
    "Bologna": "Stadio Renato Dall'Ara",
    "Cagliari": "Unipol Domus",
    "Como": "Stadio Giuseppe Sinigaglia",
    "Fiorentina": "Stadio Artemio Franchi",
    "Frosinone": "Stadio Benito Stirpe",
    "Genoa": "Stadio Luigi Ferraris",
    "Inter": "San Siro",
    "Juventus": "Allianz Stadium",
    "Lazio": "Stadio Olimpico",
    "Lecce": "Stadio Via del Mare",
    "Milan": "San Siro",
    "Monza": "U-Power Stadium",
    "Napoli": "Stadio Diego Armando Maradona",
    "Parma": "Stadio Ennio Tardini",
    "Roma": "Stadio Olimpico",
    "Sassuolo": "Mapei Stadium",
    "Torino": "Stadio Olimpico Grande Torino",
    "Udinese": "Bluenergy Stadium",
    "Venezia": "Stadio Pier Luigi Penzo",
}

# Stadyum bilgileri: ad -> (sehir, kapasite). Bazi stadyumlar paylasimli (San Siro, Olimpico).
STADIUM_INFO = {
    "Gewiss Stadium": ("Bergamo", 21300),
    "Stadio Renato Dall'Ara": ("Bologna", 36462),
    "Unipol Domus": ("Cagliari", 16416),
    "Stadio Giuseppe Sinigaglia": ("Como", 13602),
    "Stadio Artemio Franchi": ("Firenze", 43147),
    "Stadio Benito Stirpe": ("Frosinone", 16227),
    "Stadio Luigi Ferraris": ("Genova", 36599),
    "San Siro": ("Milano", 75923),
    "Allianz Stadium": ("Torino", 41507),
    "Stadio Olimpico": ("Roma", 70634),
    "Stadio Via del Mare": ("Lecce", 31533),
    "U-Power Stadium": ("Monza", 16917),
    "Stadio Diego Armando Maradona": ("Napoli", 54726),
    "Stadio Ennio Tardini": ("Parma", 27906),
    "Mapei Stadium": ("Reggio Emilia", 21584),
    "Stadio Olimpico Grande Torino": ("Torino", 27958),
    "Bluenergy Stadium": ("Udine", 25144),
    "Stadio Pier Luigi Penzo": ("Venezia", 11150),
}

# Oyuncu profilleri icin yardimci havuzlar
POSITIONS = ["Kaleci", "Defans", "Orta Saha", "Forvet"]
NATIONALITIES = [
    "İtalya", "Fransa", "Arjantin", "Brezilya", "Portekiz", "İspanya",
    "Hollanda", "Belçika", "Almanya", "Nijerya", "Hırvatistan", "Danimarka",
    "İngiltere", "Gürcistan", "ABD", "Uruguay", "Kolombiya", "İsviçre",
]
# Onceki kulup havuzu (kariyer gecmisi icin)
PREV_CLUBS = [
    "Sporting CP", "Benfica", "FC Porto", "Ajax", "PSV Eindhoven", "Olympique Lyon",
    "AS Monaco", "Sevilla", "Villarreal", "Valencia", "Borussia Dortmund", "RB Leipzig",
    "Feyenoord", "Celtic", "Shakhtar Donetsk", "Dinamo Zagreb", "River Plate",
    "Boca Juniors", "Flamengo", "Santos", "Chelsea", "Everton", "Southampton",
    "Olympique Marseille", "OGC Nice", "Red Bull Salzburg", "Club Brugge", "Anderlecht",
]
# Takim bazli avatar arkaplan rengi (foto placeholder icin)
TEAM_BG = {
    "Atalanta": "1E6FB8", "Bologna": "A21C26", "Cagliari": "A5162B", "Como": "0B3E91",
    "Fiorentina": "5B2A86", "Frosinone": "1C4E9A", "Genoa": "A2172E", "Inter": "0A4FA8",
    "Juventus": "16181D", "Lazio": "5AA9E6", "Lecce": "D21F26", "Milan": "C8141E",
    "Monza": "D2122E", "Napoli": "1E9BD7", "Parma": "1D3E8A", "Roma": "8E1F2F",
    "Sassuolo": "00A651", "Torino": "7E1B1B", "Udinese": "26282C", "Venezia": "141414",
}

def norm_name(s):
    """Aksanlari kaldirip kucuk harfe cevirir (ayni oyuncuyu eslemek icin anahtar)."""
    s = unicodedata.normalize("NFKD", s)
    s = "".join(c for c in s if not unicodedata.combining(c))
    return s.lower().strip()

# ---------------- Takim kadrolari (tam Serie A kadrolari) ----------------
# Her oyuncu: (AdSoyad, Mevki, Uyruk). Her takim icin ~22 kisilik gercek kadro.
# Not: One cikan Milan-Roma maci oyunculari Milan/Roma kadrolarinda yer alir.
SQUADS = {
    "Atalanta": [
        ("Marco Carnesecchi", "Kaleci", "İtalya"), ("Juan Musso", "Kaleci", "Arjantin"),
        ("Rafael Tolói", "Defans", "İtalya"), ("Berat Djimsiti", "Defans", "Arnavutluk"),
        ("Isak Hien", "Defans", "İsveç"), ("Sead Kolašinac", "Defans", "Bosna Hersek"),
        ("Davide Zappacosta", "Defans", "İtalya"), ("Matteo Ruggeri", "Defans", "İtalya"),
        ("Raoul Bellanova", "Defans", "İtalya"), ("Odilon Kossounou", "Defans", "Fildişi"),
        ("Éderson", "Orta Saha", "Brezilya"), ("Marten de Roon", "Orta Saha", "Hollanda"),
        ("Mario Pašalić", "Orta Saha", "Hırvatistan"), ("Charles De Ketelaere", "Orta Saha", "Belçika"),
        ("Marco Brescianini", "Orta Saha", "İtalya"), ("Ademola Lookman", "Forvet", "Nijerya"),
        ("Mateo Retegui", "Forvet", "İtalya"), ("Gianluca Scamacca", "Forvet", "İtalya"),
        ("El Bilal Touré", "Forvet", "Mali"), ("Daniel Maldini", "Forvet", "İtalya"),
    ],
    "Bologna": [
        ("Łukasz Skorupski", "Kaleci", "Polonya"), ("Nicola Ravaglia", "Kaleci", "İtalya"),
        ("Sam Beukema", "Defans", "Hollanda"), ("Jhon Lucumí", "Defans", "Kolombiya"),
        ("Stefan Posch", "Defans", "Avusturya"), ("Charalampos Lykogiannis", "Defans", "Yunanistan"),
        ("Emil Holm", "Defans", "İsveç"), ("Juan Miranda", "Defans", "İspanya"),
        ("Lewis Ferguson", "Orta Saha", "İskoçya"), ("Remo Freuler", "Orta Saha", "İsviçre"),
        ("Nikola Moro", "Orta Saha", "Hırvatistan"), ("Giovanni Fabbian", "Orta Saha", "İtalya"),
        ("Tommaso Pobega", "Orta Saha", "İtalya"), ("Dan Ndoye", "Forvet", "İsviçre"),
        ("Riccardo Orsolini", "Forvet", "İtalya"), ("Santiago Castro", "Forvet", "Arjantin"),
        ("Jens Odgaard", "Forvet", "Danimarka"), ("Thijs Dallinga", "Forvet", "Hollanda"),
        ("Benjamin Dominguez", "Forvet", "Arjantin"),
    ],
    "Cagliari": [
        ("Simone Scuffet", "Kaleci", "İtalya"), ("Elia Caprile", "Kaleci", "İtalya"),
        ("Yerry Mina", "Defans", "Kolombiya"), ("Sebastian Walukiewicz", "Defans", "Polonya"),
        ("Adam Obert", "Defans", "Slovakya"), ("Tommaso Augello", "Defans", "İtalya"),
        ("Gabriele Zappa", "Defans", "İtalya"), ("Nadir Zortea", "Defans", "İtalya"),
        ("Nahitan Nández", "Orta Saha", "Uruguay"), ("Răzvan Marin", "Orta Saha", "Romanya"),
        ("Michel Adopo", "Orta Saha", "Fildişi"), ("Antoine Makoumbou", "Orta Saha", "Kongo"),
        ("Nicolas Viola", "Orta Saha", "İtalya"), ("Zito Luvumbo", "Forvet", "Angola"),
        ("Gianluca Lapadula", "Forvet", "Peru"), ("Roberto Piccoli", "Forvet", "İtalya"),
        ("Leonardo Pavoletti", "Forvet", "İtalya"), ("Mattia Felici", "Forvet", "İtalya"),
    ],
    "Como": [
        ("Pepe Reina", "Kaleci", "İspanya"), ("Emil Audero", "Kaleci", "İtalya"),
        ("Alberto Dossena", "Defans", "İtalya"), ("Marc-Oliver Kempf", "Defans", "Almanya"),
        ("Edoardo Goldaniga", "Defans", "İtalya"), ("Alberto Moreno", "Defans", "İspanya"),
        ("Ivan Smolčić", "Defans", "Hırvatistan"), ("Alex Valle", "Defans", "İspanya"),
        ("Sergi Roberto", "Orta Saha", "İspanya"), ("Máximo Perrone", "Orta Saha", "Arjantin"),
        ("Nico Paz", "Orta Saha", "Arjantin"), ("Lucas da Cunha", "Orta Saha", "Fransa"),
        ("Yannik Engelhardt", "Orta Saha", "Almanya"), ("Gabriel Strefezza", "Forvet", "Brezilya"),
        ("Patrick Cutrone", "Forvet", "İtalya"), ("Assane Diao", "Forvet", "Senegal"),
        ("Alieu Fadera", "Forvet", "Gambiya"), ("Simone Verdi", "Forvet", "İtalya"),
    ],
    "Fiorentina": [
        ("David de Gea", "Kaleci", "İspanya"), ("Pietro Terracciano", "Kaleci", "İtalya"),
        ("Luca Ranieri", "Defans", "İtalya"), ("Marin Pongračić", "Defans", "Hırvatistan"),
        ("Pietro Comuzzo", "Defans", "İtalya"), ("Robin Gosens", "Defans", "Almanya"),
        ("Dodô", "Defans", "Brezilya"), ("Cristiano Biraghi", "Defans", "İtalya"),
        ("Rolando Mandragora", "Orta Saha", "İtalya"), ("Danilo Cataldi", "Orta Saha", "İtalya"),
        ("Yacine Adli", "Orta Saha", "Fransa"), ("Edoardo Bove", "Orta Saha", "İtalya"),
        ("Andrea Colpani", "Orta Saha", "İtalya"), ("Moise Kean", "Forvet", "İtalya"),
        ("Albert Gudmundsson", "Forvet", "İzlanda"), ("Riccardo Sottil", "Forvet", "İtalya"),
        ("Jonathan Ikoné", "Forvet", "Fransa"), ("Christian Kouamé", "Forvet", "Fildişi"),
    ],
    "Frosinone": [
        ("Stefano Turati", "Kaleci", "İtalya"), ("Pol Lirola", "Defans", "İspanya"),
        ("Riccardo Marchizza", "Defans", "İtalya"), ("Simone Romagnoli", "Defans", "İtalya"),
        ("Jordi Mboula", "Defans", "İspanya"), ("Anthony Oyono", "Defans", "Gabon"),
        ("Enzo Barrenechea", "Orta Saha", "Arjantin"), ("Marco Brescianini", "Orta Saha", "İtalya"),
        ("Luca Mazzitelli", "Orta Saha", "İtalya"), ("Nadir Zortea", "Orta Saha", "İtalya"),
        ("Arijon Ibrahimović", "Orta Saha", "Almanya"), ("Matías Soulé", "Forvet", "Arjantin"),
        ("Walid Cheddira", "Forvet", "Fas"), ("Giuseppe Caso", "Forvet", "İtalya"),
        ("Marvin Čuni", "Forvet", "Arnavutluk"), ("Abdou Harroui", "Orta Saha", "Fas"),
        ("Kaio Jorge", "Forvet", "Brezilya"), ("Ilario Monterisi", "Defans", "İtalya"),
    ],
    "Genoa": [
        ("Nicola Leali", "Kaleci", "İtalya"), ("Pierluigi Gollini", "Kaleci", "İtalya"),
        ("Koni De Winter", "Defans", "Belçika"), ("Mattia Bani", "Defans", "İtalya"),
        ("Johan Vásquez", "Defans", "Meksika"), ("Aaron Martín", "Defans", "İspanya"),
        ("Stefano Sabelli", "Defans", "İtalya"), ("Brooke Norton-Cuffy", "Defans", "İngiltere"),
        ("Morten Frendrup", "Orta Saha", "Danimarka"), ("Milan Badelj", "Orta Saha", "Hırvatistan"),
        ("Ruslan Malinovskyi", "Orta Saha", "Ukrayna"), ("Patrizio Masini", "Orta Saha", "İtalya"),
        ("Fabio Miretti", "Orta Saha", "İtalya"), ("Mateo Retegui", "Forvet", "İtalya"),
        ("Andrea Pinamonti", "Forvet", "İtalya"), ("Vitinha", "Forvet", "Portekiz"),
        ("Caleb Ekuban", "Forvet", "Gana"), ("Junior Messias", "Forvet", "Brezilya"),
    ],
    "Inter": [
        ("Yann Sommer", "Kaleci", "İsviçre"), ("Josep Martínez", "Kaleci", "İspanya"),
        ("Benjamin Pavard", "Defans", "Fransa"), ("Alessandro Bastoni", "Defans", "İtalya"),
        ("Francesco Acerbi", "Defans", "İtalya"), ("Stefan de Vrij", "Defans", "Hollanda"),
        ("Yann Bisseck", "Defans", "Almanya"), ("Denzel Dumfries", "Defans", "Hollanda"),
        ("Federico Dimarco", "Defans", "İtalya"), ("Carlos Augusto", "Defans", "Brezilya"),
        ("Nicolò Barella", "Orta Saha", "İtalya"), ("Hakan Çalhanoğlu", "Orta Saha", "Türkiye"),
        ("Henrikh Mkhitaryan", "Orta Saha", "Ermenistan"), ("Davide Frattesi", "Orta Saha", "İtalya"),
        ("Piotr Zieliński", "Orta Saha", "Polonya"), ("Kristjan Asllani", "Orta Saha", "Arnavutluk"),
        ("Lautaro Martínez", "Forvet", "Arjantin"), ("Marcus Thuram", "Forvet", "Fransa"),
        ("Mehdi Taremi", "Forvet", "İran"), ("Marko Arnautović", "Forvet", "Avusturya"),
    ],
    "Juventus": [
        ("Michele Di Gregorio", "Kaleci", "İtalya"), ("Mattia Perin", "Kaleci", "İtalya"),
        ("Gleison Bremer", "Defans", "Brezilya"), ("Federico Gatti", "Defans", "İtalya"),
        ("Danilo", "Defans", "Brezilya"), ("Pierre Kalulu", "Defans", "Fransa"),
        ("Andrea Cambiaso", "Defans", "İtalya"), ("Juan Cabal", "Defans", "Kolombiya"),
        ("Manuel Locatelli", "Orta Saha", "İtalya"), ("Khéphren Thuram", "Orta Saha", "Fransa"),
        ("Douglas Luiz", "Orta Saha", "Brezilya"), ("Weston McKennie", "Orta Saha", "ABD"),
        ("Teun Koopmeiners", "Orta Saha", "Hollanda"), ("Nicolò Fagioli", "Orta Saha", "İtalya"),
        ("Kenan Yıldız", "Forvet", "Türkiye"), ("Dušan Vlahović", "Forvet", "Sırbistan"),
        ("Francisco Conceição", "Forvet", "Portekiz"), ("Timothy Weah", "Forvet", "ABD"),
        ("Samuel Mbangula", "Forvet", "Belçika"), ("Nico González", "Forvet", "Arjantin"),
    ],
    "Lazio": [
        ("Ivan Provedel", "Kaleci", "İtalya"), ("Christos Mandas", "Kaleci", "Yunanistan"),
        ("Alessio Romagnoli", "Defans", "İtalya"), ("Mario Gila", "Defans", "İspanya"),
        ("Nuno Tavares", "Defans", "Portekiz"), ("Adam Marušić", "Defans", "Karadağ"),
        ("Patric", "Defans", "İspanya"), ("Manuel Lazzari", "Defans", "İtalya"),
        ("Nicolò Rovella", "Orta Saha", "İtalya"), ("Matteo Guendouzi", "Orta Saha", "Fransa"),
        ("Danilo Cataldi", "Orta Saha", "İtalya"), ("Gaetano Castrovilli", "Orta Saha", "İtalya"),
        ("Luis Alberto", "Orta Saha", "İspanya"), ("Mattia Zaccagni", "Forvet", "İtalya"),
        ("Valentín Castellanos", "Forvet", "Arjantin"), ("Boulaye Dia", "Forvet", "Senegal"),
        ("Pedro", "Forvet", "İspanya"), ("Gustav Isaksen", "Forvet", "Danimarka"),
    ],
    "Lecce": [
        ("Wladimiro Falcone", "Kaleci", "İtalya"), ("Jasper Samooja", "Kaleci", "Finlandiya"),
        ("Federico Baschirotto", "Defans", "İtalya"), ("Marin Pongračić", "Defans", "Hırvatistan"),
        ("Antonino Gallo", "Defans", "İtalya"), ("Valentin Gendrey", "Defans", "Fransa"),
        ("Kialonda Gaspar", "Defans", "Angola"), ("Jean Harisson Marcelin", "Defans", "Fransa"),
        ("Ylber Ramadani", "Orta Saha", "Arnavutluk"), ("Hamza Rafia", "Orta Saha", "Tunus"),
        ("Balthazar Pierret", "Orta Saha", "Fransa"), ("Lassana Coulibaly", "Orta Saha", "Mali"),
        ("Nikola Krstović", "Forvet", "Karadağ"), ("Lameck Banda", "Forvet", "Zambiya"),
        ("Rémi Oudin", "Forvet", "Fransa"), ("Santiago Pierotti", "Forvet", "Arjantin"),
        ("Patrick Dorgu", "Forvet", "Danimarka"), ("Nikola Štulić", "Forvet", "Sırbistan"),
    ],
    "Milan": [
        ("Mike Maignan", "Kaleci", "Fransa"), ("Marco Sportiello", "Kaleci", "İtalya"),
        ("Fikayo Tomori", "Defans", "İngiltere"), ("Malick Thiaw", "Defans", "Almanya"),
        ("Matteo Gabbia", "Defans", "İtalya"), ("Theo Hernández", "Defans", "Fransa"),
        ("Emerson Royal", "Defans", "Brezilya"), ("Davide Calabria", "Defans", "İtalya"),
        ("Strahinja Pavlović", "Defans", "Sırbistan"), ("Youssouf Fofana", "Orta Saha", "Fransa"),
        ("Tijjani Reijnders", "Orta Saha", "Hollanda"), ("Ruben Loftus-Cheek", "Orta Saha", "İngiltere"),
        ("Yunus Musah", "Orta Saha", "ABD"), ("Ismaël Bennacer", "Orta Saha", "Cezayir"),
        ("Rafael Leão", "Forvet", "Portekiz"), ("Christian Pulisic", "Forvet", "ABD"),
        ("Olivier Giroud", "Forvet", "Fransa"), ("Álvaro Morata", "Forvet", "İspanya"),
        ("Noah Okafor", "Forvet", "İsviçre"), ("Samuel Chukwueze", "Forvet", "Nijerya"),
        ("Luka Jović", "Forvet", "Sırbistan"),
    ],
    "Monza": [
        ("Stefano Turati", "Kaleci", "İtalya"), ("Alessio Cragno", "Kaleci", "İtalya"),
        ("Pablo Marí", "Defans", "İspanya"), ("Armando Izzo", "Defans", "İtalya"),
        ("Danilo D'Ambrosio", "Defans", "İtalya"), ("Pedro Pereira", "Defans", "Portekiz"),
        ("Georgios Kyriakopoulos", "Defans", "Yunanistan"), ("Andrea Carboni", "Defans", "İtalya"),
        ("Matteo Pessina", "Orta Saha", "İtalya"), ("Roberto Gagliardini", "Orta Saha", "İtalya"),
        ("Stefano Sensi", "Orta Saha", "İtalya"), ("Omari Forson", "Orta Saha", "İngiltere"),
        ("Samuele Vignato", "Orta Saha", "İtalya"), ("Dany Mota", "Forvet", "Portekiz"),
        ("Milan Đurić", "Forvet", "Bosna Hersek"), ("Lorenzo Colombo", "Forvet", "İtalya"),
        ("Gianluca Caprari", "Forvet", "İtalya"), ("Daniel Maldini", "Forvet", "İtalya"),
    ],
    "Napoli": [
        ("Alex Meret", "Kaleci", "İtalya"), ("Elia Caprile", "Kaleci", "İtalya"),
        ("Amir Rrahmani", "Defans", "Kosova"), ("Alessandro Buongiorno", "Defans", "İtalya"),
        ("Juan Jesus", "Defans", "Brezilya"), ("Giovanni Di Lorenzo", "Defans", "İtalya"),
        ("Leonardo Spinazzola", "Defans", "İtalya"), ("Mathías Olivera", "Defans", "Uruguay"),
        ("Stanislav Lobotka", "Orta Saha", "Slovakya"), ("André-Frank Zambo Anguissa", "Orta Saha", "Kamerun"),
        ("Scott McTominay", "Orta Saha", "İskoçya"), ("Billy Gilmour", "Orta Saha", "İskoçya"),
        ("Khvicha Kvaratskhelia", "Forvet", "Gürcistan"), ("Matteo Politano", "Forvet", "İtalya"),
        ("Romelu Lukaku", "Forvet", "Belçika"), ("Giovanni Simeone", "Forvet", "Arjantin"),
        ("David Neres", "Forvet", "Brezilya"), ("Giacomo Raspadori", "Forvet", "İtalya"),
    ],
    "Parma": [
        ("Zion Suzuki", "Kaleci", "Japonya"), ("Edoardo Corvi", "Kaleci", "İtalya"),
        ("Giovanni Leoni", "Defans", "İtalya"), ("Botond Balogh", "Defans", "Macaristan"),
        ("Enrico Delprato", "Defans", "İtalya"), ("Woyo Coulibaly", "Defans", "Fransa"),
        ("Emanuele Valeri", "Defans", "İtalya"), ("Alessandro Circati", "Defans", "Avustralya"),
        ("Hernani", "Orta Saha", "Brezilya"), ("Simon Sohm", "Orta Saha", "İsviçre"),
        ("Adrián Bernabé", "Orta Saha", "İspanya"), ("Mandela Keita", "Orta Saha", "Belçika"),
        ("Dennis Man", "Forvet", "Romanya"), ("Ange-Yoan Bonny", "Forvet", "Fransa"),
        ("Valentin Mihăilă", "Forvet", "Romanya"), ("Matteo Cancellieri", "Forvet", "İtalya"),
        ("Pontus Almqvist", "Forvet", "İsveç"), ("Gabriel Charpentier", "Forvet", "Fransa"),
    ],
    "Roma": [
        ("Mile Svilar", "Kaleci", "Sırbistan"), ("Pietro Boer", "Kaleci", "İtalya"),
        ("Gianluca Mancini", "Defans", "İtalya"), ("Evan Ndicka", "Defans", "Fildişi"),
        ("Mats Hummels", "Defans", "Almanya"), ("Diego Llorente", "Defans", "İspanya"),
        ("Zeki Çelik", "Defans", "Türkiye"), ("Angeliño", "Defans", "İspanya"),
        ("Mario Hermoso", "Defans", "İspanya"), ("Bryan Cristante", "Orta Saha", "İtalya"),
        ("Leandro Paredes", "Orta Saha", "Arjantin"), ("Manu Koné", "Orta Saha", "Fransa"),
        ("Lorenzo Pellegrini", "Orta Saha", "İtalya"), ("Niccolò Pisilli", "Orta Saha", "İtalya"),
        ("Paulo Dybala", "Forvet", "Arjantin"), ("Artem Dovbyk", "Forvet", "Ukrayna"),
        ("Stephan El Shaarawy", "Forvet", "İtalya"), ("Tammy Abraham", "Forvet", "İngiltere"),
        ("Andrea Belotti", "Forvet", "İtalya"), ("Matías Soulé", "Forvet", "Arjantin"),
    ],
    "Sassuolo": [
        ("Andrea Consigli", "Kaleci", "İtalya"), ("Alessandro Russo", "Kaleci", "İtalya"),
        ("Martin Erlić", "Defans", "Hırvatistan"), ("Filippo Romagna", "Defans", "İtalya"),
        ("Josh Doig", "Defans", "İskoçya"), ("Jeremy Toljan", "Defans", "Almanya"),
        ("Tarik Muharemović", "Defans", "Bosna Hersek"), ("Marcus Pedersen", "Defans", "Norveç"),
        ("Kristian Thorstvedt", "Orta Saha", "Norveç"), ("Daniel Boloca", "Orta Saha", "Romanya"),
        ("Nemanja Matić", "Orta Saha", "Sırbistan"), ("Pedro Obiang", "Orta Saha", "Ekvator Ginesi"),
        ("Domenico Berardi", "Forvet", "İtalya"), ("Andrea Pinamonti", "Forvet", "İtalya"),
        ("Armand Laurienté", "Forvet", "Fransa"), ("Grégoire Defrel", "Forvet", "Fransa"),
        ("Samuele Mulattieri", "Forvet", "İtalya"), ("Luca Moro", "Forvet", "İtalya"),
    ],
    "Torino": [
        ("Vanja Milinković-Savić", "Kaleci", "Sırbistan"), ("Alberto Paleari", "Kaleci", "İtalya"),
        ("Alessandro Buongiorno", "Defans", "İtalya"), ("Saba Sazonov", "Defans", "Gürcistan"),
        ("Guillermo Maripán", "Defans", "Şili"), ("Mergim Vojvoda", "Defans", "Kosova"),
        ("Valentino Lazaro", "Defans", "Avusturya"), ("Adam Masina", "Defans", "Fas"),
        ("Samuele Ricci", "Orta Saha", "İtalya"), ("Ivan Ilić", "Orta Saha", "Sırbistan"),
        ("Karol Linetty", "Orta Saha", "Polonya"), ("Gvidas Gineitis", "Orta Saha", "Litvanya"),
        ("Nikola Vlašić", "Orta Saha", "Hırvatistan"), ("Duván Zapata", "Forvet", "Kolombiya"),
        ("Antonio Sanabria", "Forvet", "Paraguay"), ("Che Adams", "Forvet", "İskoçya"),
        ("Pietro Pellegri", "Forvet", "İtalya"), ("Yann Karamoh", "Forvet", "Fildişi"),
    ],
    "Udinese": [
        ("Maduka Okoye", "Kaleci", "Nijerya"), ("Daniele Padelli", "Kaleci", "İtalya"),
        ("Jaka Bijol", "Defans", "Slovenya"), ("Thomas Kristensen", "Defans", "Danimarka"),
        ("Christian Kabasele", "Defans", "Belçika"), ("Kingsley Ehizibue", "Defans", "Nijerya"),
        ("Jordan Zemura", "Defans", "Zimbabve"), ("Hassane Kamara", "Defans", "Fildişi"),
        ("Sandi Lovrić", "Orta Saha", "Slovenya"), ("Oier Zarraga", "Orta Saha", "İspanya"),
        ("Jurgen Ekkelenkamp", "Orta Saha", "Hollanda"), ("Martín Payero", "Orta Saha", "Arjantin"),
        ("Roberto Pereyra", "Orta Saha", "Arjantin"), ("Lorenzo Lucca", "Forvet", "İtalya"),
        ("Florian Thauvin", "Forvet", "Fransa"), ("Keinan Davis", "Forvet", "İngiltere"),
        ("Lautaro Giannetti", "Defans", "Arjantin"), ("Isaac Success", "Forvet", "Nijerya"),
    ],
    "Venezia": [
        ("Jesse Joronen", "Kaleci", "Finlandiya"), ("Filip Stanković", "Kaleci", "Sırbistan"),
        ("Giorgio Altare", "Defans", "İtalya"), ("Jay Idzes", "Defans", "Endonezya"),
        ("Ridgeciano Haps", "Defans", "Surinam"), ("Marin Sverko", "Defans", "Hırvatistan"),
        ("Joel Schingtienne", "Defans", "Belçika"), ("Michael Svoboda", "Defans", "Avusturya"),
        ("Hans Nicolussi Caviglia", "Orta Saha", "İtalya"), ("Issa Doumbia", "Orta Saha", "Fildişi"),
        ("Gaetano Oristanio", "Orta Saha", "İtalya"), ("Magnus Kofod Andersen", "Orta Saha", "Danimarka"),
        ("Christian Gytkjær", "Forvet", "Danimarka"), ("Joel Pohjanpalo", "Forvet", "Finlandiya"),
        ("Gianluca Busio", "Orta Saha", "ABD"), ("Daniel Fila", "Forvet", "Çekya"),
        ("John Yeboah", "Forvet", "Almanya"), ("Francesco Zampano", "Defans", "İtalya"),
    ],
}

# Gol atabilecek oyuncu havuzu: her takimin Forvet + Orta Saha oyunculari (agirlikli gol dagilimi icin)
SCORERS = {
    t: [p[0] for p in SQUADS[t] if p[1] in ("Forvet", "Orta Saha")]
    for t in TEAMS
}
# Her oyuncunun (takim, isim) -> (mevki, uyruk) hizli erisim sozlugu
SQUAD_INFO = {}
for t in TEAMS:
    for (nm, pos, nat) in SQUADS[t]:
        SQUAD_INFO.setdefault((t, norm_name(nm)), (nm, pos, nat))

# ---------------- Round-robin (circle method) ----------------
def build_schedule(teams):
    n = len(teams)
    arr = list(range(n))
    first_half = []
    for r in range(n - 1):
        pairs = []
        for i in range(n // 2):
            a, b = arr[i], arr[n - 1 - i]
            if r % 2 == 0:
                pairs.append((a, b))
            else:
                pairs.append((b, a))
        first_half.append(pairs)
        arr = [arr[0]] + [arr[-1]] + arr[1:-1]
    # Ikinci devre: ev-deplasman ters
    second_half = [[(b, a) for (a, b) in rnd] for rnd in first_half]
    return first_half + second_half  # 38 hafta

schedule = build_schedule(TEAMS)  # index 0..37 -> hafta 1..38

# ---------------- Tarih/slot ----------------
BASE_SUNDAY = datetime(2026, 9, 27, 0, 0, 0)  # 20. hafta Pazar
assert BASE_SUNDAY.weekday() == 6, "27 Eylul 2026 Pazar degil!"

SLOTS = [
    ("SAT", 15, 0), ("SAT", 18, 0), ("SAT", 20, 45),
    ("SUN", 12, 30), ("SUN", 15, 0), ("SUN", 15, 0),
    ("SUN", 18, 0), ("SUN", 20, 45), ("SUN", 20, 45), ("SUN", 21, 45),
]

def week_dates(week):  # week 1..38
    sunday = BASE_SUNDAY + timedelta(days=(week - 20) * 7)
    saturday = sunday - timedelta(days=1)
    return saturday, sunday

def match_datetime(week, slot_idx):
    sat, sun = week_dates(week)
    day, hh, mm = SLOTS[slot_idx]
    base = sat if day == "SAT" else sun
    return base.replace(hour=hh, minute=mm, second=0)

# ---------------- Skor uretimi ----------------
GOAL_DIST = [0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 4, 5]  # agirlikli

def gen_goals():
    return random.choice(GOAL_DIST)

def gen_minutes(count):
    mins = set()
    while len(mins) < count:
        mins.add(random.randint(1, 90))
    return sorted(mins)

# ---------------- Fixtures olustur ----------------
fixtures = []  # dict listesi
fid = 0
for w in range(1, 39):
    rnd = schedule[w - 1]
    for slot_idx, (hi, ai) in enumerate(rnd):
        fid += 1
        home = TEAMS[hi]
        away = TEAMS[ai]
        finished = w <= 20
        f = {
            "id": fid, "week": w, "home": home, "away": away,
            "stadium": STADIUM[home], "dt": match_datetime(w, slot_idx),
            "status": 2 if finished else 0,
            "hs": 0, "as": 0, "hhs": 0, "ahs": 0,
            "home_goal_min": [], "away_goal_min": [],
        }
        if finished:
            hg, ag = gen_goals(), gen_goals()
            hmin, amin = gen_minutes(hg), gen_minutes(ag)
            f["hs"], f["as"] = hg, ag
            f["hhs"] = sum(1 for m in hmin if m <= 45)
            f["ahs"] = sum(1 for m in amin if m <= 45)
            f["home_goal_min"], f["away_goal_min"] = hmin, amin
        fixtures.append(f)

# ---------------- One cikan: ilk devre Milan-Roma macini bul, Milan ev yap ----------------
featured = None
for f in fixtures:
    if f["week"] <= 20 and {f["home"], f["away"]} == {"Milan", "Roma"}:
        if featured is None or f["week"] < featured["week"]:
            featured = f
# Milan ev olacak sekilde yonlendir
featured["home"], featured["away"] = "Milan", "Roma"
featured["stadium"] = "San Siro"
featured["hs"], featured["as"] = 2, 1
featured["hhs"], featured["ahs"] = 2, 0
featured["home_goal_min"], featured["away_goal_min"] = [], []  # olaylar ozel eklenecek
FEATURED_ID = featured["id"]

# ---------------- MatchStatic (oynanan maclar) ----------------
def gen_static(f):
    if f["id"] == FEATURED_ID:
        return dict(pos=(54, 46), sh=(15, 10), sot=(7, 4), pas=(512, 438),
                    pacc=(87, 82), cor=(6, 4), foul=(11, 13), off=(2, 1),
                    yc=(2, 3), rc=(0, 1))
    hp = random.randint(41, 59)
    hsh, ash = random.randint(7, 20), random.randint(6, 18)
    return dict(
        pos=(hp, 100 - hp),
        sh=(hsh, ash),
        sot=(max(f["hs"], random.randint(2, max(2, hsh // 2))), max(f["as"], random.randint(1, max(1, ash // 2)))),
        pas=(random.randint(330, 600), random.randint(300, 560)),
        pacc=(random.randint(76, 90), random.randint(74, 89)),
        cor=(random.randint(2, 9), random.randint(1, 8)),
        foul=(random.randint(6, 16), random.randint(6, 16)),
        off=(random.randint(0, 4), random.randint(0, 4)),
        yc=(random.randint(0, 4), random.randint(0, 4)),
        rc=(1 if random.random() < 0.06 else 0, 1 if random.random() < 0.06 else 0),
    )

statics = {f["id"]: gen_static(f) for f in fixtures if f["status"] == 2}

# ---------------- Events ----------------
# Olay tipleri: 0=Goal, 2=YellowCard, 3=RedCard, 5=SubstitutionIn, 10=Corner
# Featured 14 olay: (takim, oyuncu, tip, aciklama, dk)
FEATURED_EVENTS = [
    ("Milan", "Rafael Leão", 0, "Asist: Theo Hernández", 14),
    ("Roma", "Paulo Dybala", 2, "Faul", 27),
    ("Milan", "Olivier Giroud", 0, "Kafa, asist: Rafael Leão", 41),
    ("Roma", "Bryan Cristante", 2, "Taktik faul", 54),
    ("Roma", "Lorenzo Pellegrini", 5, "Çıkan: Stephan El Shaarawy", 58),
    ("Milan", "Christian Pulisic", 5, "Çıkan: Samuel Chukwueze", 64),
    ("Milan", "Fikayo Tomori", 2, "Faul", 67),
    ("Roma", "Tammy Abraham", 0, "Asist: Lorenzo Pellegrini", 72),
    ("Roma", "Gianluca Mancini", 2, "İtiraz", 74),
    ("Milan", "Yunus Musah", 5, "Çıkan: Ruben Loftus-Cheek", 78),
    ("Roma", "Andrea Belotti", 5, "Çıkan: Tammy Abraham", 79),
    ("Milan", "Theo Hernández", 2, "Oyunu geciktirme", 83),
    ("Milan", "Noah Okafor", 5, "Çıkan: Rafael Leão", 88),
    ("Roma", "Diego Llorente", 3, "Gol şansını engelleme", 90),
]

CARD_REASONS = ["Faul", "Taktik faul", "İtiraz", "Oyunu geciktirme", "Sert müdahale", "Elle oynama"]
RED_REASONS = ["İkinci sarı kart", "Gol şansını engelleme", "Ciddi faullü hareket"]
ASSIST_CHANCE = 0.6

def other(pool, player):
    alts = [p for p in pool if p != player]
    return random.choice(alts) if alts else player

def build_events():
    evs = []  # (fixture_id, team_name, player, type, desc, minute)
    for f in fixtures:
        if f["status"] != 2:
            continue
        if f["id"] == FEATURED_ID:
            for (team, player, etype, desc, minute) in FEATURED_EVENTS:
                evs.append((f["id"], team, player, etype, desc, minute))
            continue
        s = statics[f["id"]]
        sides = [
            (f["home"], f["home_goal_min"], s["yc"][0], s["rc"][0], s["cor"][0]),
            (f["away"], f["away_goal_min"], s["yc"][1], s["rc"][1], s["cor"][1]),
        ]
        for team, goal_min, yc, rc, corners in sides:
            pool = SCORERS[team]                               # gol havuzu (forvet + orta saha)
            full_pool = [p[0] for p in SQUADS[team]]           # tum kadro (kart/degisiklik icin)
            # Forvetler orta sahalardan daha sik gol atar (gercekci krallik tablosu)
            weights = [3 if SQUAD_INFO[(team, norm_name(n))][1] == "Forvet" else 1 for n in pool]
            # Goller (bazen asistli)
            for m in goal_min:
                scorer = random.choices(pool, weights=weights, k=1)[0]
                desc = f"Asist: {other(pool, scorer)}" if random.random() < ASSIST_CHANCE else "Gol"
                evs.append((f["id"], team, scorer, 0, desc, m))
            # Sari kartlar (istatistikteki sayi kadar) - tum kadro
            for _ in range(yc):
                evs.append((f["id"], team, random.choice(full_pool), 2, random.choice(CARD_REASONS), random.randint(10, 90)))
            # Kirmizi kartlar - tum kadro
            for _ in range(rc):
                evs.append((f["id"], team, random.choice(full_pool), 3, random.choice(RED_REASONS), random.randint(35, 92)))
            # Oyuncu degisiklikleri (3) - tum kadro
            for mi in sorted(random.sample(range(46, 90), 3)):
                inp = random.choice(full_pool)
                evs.append((f["id"], team, inp, 5, f"Çıkan: {other(full_pool, inp)}", mi))
            # Kornerler (istatistikteki sayi kadar) - tum kadro
            for _ in range(corners):
                evs.append((f["id"], team, random.choice(full_pool), 10, "Köşe vuruşu", random.randint(3, 90)))
    return evs

events = build_events()

# ---------------- Stadyumlar (benzersiz, sabit Id) ----------------
stadium_names = []
for t in TEAMS:
    nm = STADIUM[t]
    if nm not in stadium_names:
        stadium_names.append(nm)
stadium_id_map = {nm: i + 1 for i, nm in enumerate(stadium_names)}

# Her fikstüre ev sahibinin stadyum Id'sini ata
for f in fixtures:
    f["stadium_id"] = stadium_id_map[f["stadium"]]

# ---------------- Futbolcular (tam Serie A kadrolarindan) ----------------
# Her (takim, oyuncu) ikilisi icin gercek bir Player kaydi uretilir; olaylara PlayerId baglanir.
players = []            # dict listesi
player_id_map = {}      # (team, normkey) -> player_id
pid = 0
# Pozisyona gore gercekci yas araligi
AGE_BY_POS = {"Kaleci": (24, 37), "Defans": (20, 35), "Orta Saha": (19, 34), "Forvet": (19, 34)}
for team in TEAMS:
    # Her takim icin benzersiz forma numarasi havuzu (deterministik karistirma)
    shirt_pool = list(range(1, 50))
    random.shuffle(shirt_pool)
    shirt_iter = iter(shirt_pool)
    seen = set()
    for (name, pos, nat) in SQUADS[team]:
        key = (team, norm_name(name))
        if key in seen:
            continue  # takim ici tekrarlari atla
        seen.add(key)
        pid += 1
        player_id_map[key] = pid
        lo, hi = AGE_BY_POS.get(pos, (20, 34))
        age = random.randint(lo, hi)
        birth = datetime(2026 - age, random.randint(1, 12), random.randint(1, 28))
        shirt = next(shirt_iter)
        bg = TEAM_BG.get(team, "0D3B82")
        photo = ("https://ui-avatars.com/api/?name=" + quote(name)
                 + f"&background={bg}&color=fff&size=256&bold=true")
        players.append({
            "id": pid, "name": name, "team": team, "shirt": shirt,
            "pos": pos, "photo": photo, "birth": birth, "nat": nat,
        })

# ---------------- Kariyer gecmisi (her oyuncu icin 2-3 kulup) ----------------
careers = []  # (player_id, club, start, end)
for p in players:
    cur_start = 2026 - random.randint(1, 5)
    careers.append((p["id"], p["team"], cur_start, None))  # halen oynuyor (EndYear=NULL)
    y = cur_start
    prev_pool = [c for c in PREV_CLUBS]
    for _ in range(random.randint(1, 2)):
        end = y
        start = max(2006, end - random.randint(1, 4))
        if start >= end:
            start = end - 1
        club = random.choice(prev_pool)
        prev_pool.remove(club)
        careers.append((p["id"], club, start, end))
        y = start

# ---------------- SQL yaz ----------------
def q(s):
    return "N'" + s.replace("'", "''") + "'"

def dts(dt):
    return "'" + dt.strftime("%Y-%m-%dT%H:%M:%S") + "'"

def var(team):
    return "@" + team

lines = []
A = lines.append
A("/* ============================================================")
A("   Serie A 2026/27 - 38 Haftalik Ornek Veri Seti (MSSQL)")
A("   Uretim: tools/generate_seed_sql.py  (deterministik)")
A("   - 1-20. haftalar: OYNANMIS (skor + istatistik + gol olaylari)")
A("   - 21-38. haftalar: OYNANMAMIS (fikstur cekilmis, 0-0)")
A("   - 20. haftanin son maci: Pazar 27.09.2026 21:45")
A("   NOT: Takimlar (20 adet) zaten ekli olmalidir; Id'ler isimden cozulur.")
A("   ============================================================ */")
A("SET NOCOUNT ON;")
A("SET XACT_ABORT ON;")
A("BEGIN TRANSACTION;")
A("")
A("-- 1) Mevcut mac verilerini temizle (Teams korunur). FK sirasina dikkat.")
A("DELETE FROM MatchEvents;")
A("DELETE FROM MatchStatics;")
A("DELETE FROM PlayerCareers;")
A("DELETE FROM Fixtures;")
A("DELETE FROM Players;")
A("DELETE FROM Stadiums;")
A("DBCC CHECKIDENT ('Fixtures', RESEED, 0);")
A("DBCC CHECKIDENT ('MatchEvents', RESEED, 0);")
A("DBCC CHECKIDENT ('MatchStatics', RESEED, 0);")
A("DBCC CHECKIDENT ('Players', RESEED, 0);")
A("DBCC CHECKIDENT ('PlayerCareers', RESEED, 0);")
A("DBCC CHECKIDENT ('Stadiums', RESEED, 0);")
A("")
A("-- 2) Takim Id'leri (isimden cozulur)")
for t in TEAMS:
    A(f"DECLARE {var(t)} INT = (SELECT TeamId FROM Teams WHERE TeamName = N'{t}');")
A("")
A("-- Takimlarin ekli oldugunu dogrula")
cond = " OR ".join(f"{var(t)} IS NULL" for t in TEAMS)
A(f"IF ({cond})")
A("BEGIN")
A("    RAISERROR('Takimlar bulunamadi. Once 20 Serie A takimini ekleyin, sonra bu script i calistirin.', 16, 1);")
A("    ROLLBACK TRANSACTION;")
A("    RETURN;")
A("END")
A("")
A("-- 2b) Stadyumlar (sabit StadiumId ile)")
A("SET IDENTITY_INSERT Stadiums ON;")
A("INSERT INTO Stadiums (StadiumId, Name, City, Capacity) VALUES")
srows2 = []
for nm in stadium_names:
    city, cap = STADIUM_INFO[nm]
    srows2.append(f"({stadium_id_map[nm]}, {q(nm)}, {q(city)}, {cap})")
A(",\n".join(srows2) + ";")
A("SET IDENTITY_INSERT Stadiums OFF;")
A("")

A("-- 2c) Futbolcular (sabit PlayerId ile, maç olaylarıyla eşleşir)")
A("SET IDENTITY_INSERT Players ON;")
A("INSERT INTO Players (PlayerId, FullName, ShirtNumber, Position, PhotoUrl, BirthDate, Nationality, TeamId) VALUES")
prows = []
for p in players:
    prows.append(
        f"({p['id']}, {q(p['name'])}, {p['shirt']}, {q(p['pos'])}, {q(p['photo'])}, "
        f"{dts(p['birth'])}, {q(p['nat'])}, {var(p['team'])})"
    )
A(",\n".join(prows) + ";")
A("SET IDENTITY_INSERT Players OFF;")
A("")

A("-- 2d) Oyuncu kariyer geçmişi")
A("INSERT INTO PlayerCareers (PlayerId, ClubName, StartYear, EndYear) VALUES")
crows = []
for (cpid, club, start, end) in careers:
    end_sql = "NULL" if end is None else str(end)
    crows.append(f"({cpid}, {q(club)}, {start}, {end_sql})")
A(",\n".join(crows) + ";")
A("")

A("-- 3) Fixtures (sabit FixtureId ile)")
A("SET IDENTITY_INSERT Fixtures ON;")
A("INSERT INTO Fixtures (FixtureId, HomeTeamId, AwayTeamId, HomeTeamHalfScore, AwayTeamHalfScore, HomeTeamScore, AwayTeamScore, Stadium, StadiumId, MatchDate, WeekNumber, MatchStatus) VALUES")
rows = []
for f in fixtures:
    rows.append(
        f"({f['id']}, {var(f['home'])}, {var(f['away'])}, {f['hhs']}, {f['ahs']}, {f['hs']}, {f['as']}, "
        f"{q(f['stadium'])}, {f['stadium_id']}, {dts(f['dt'])}, {f['week']}, {f['status']})"
    )
A(",\n".join(rows) + ";")
A("SET IDENTITY_INSERT Fixtures OFF;")
A("")

# MatchStatics
A("-- 4) MatchStatics (yalnizca oynanan maclar)")
A("INSERT INTO MatchStatics (FixtureId, HomePossession, AwayPossession, HomeShoots, AwayShoots, HomeShootsOnTarget, AwayShootsOnTarget, HomePasses, AwayPasses, HomePassAccuracy, AwayPassAccuracy, HomeCorners, AwayCorners, HomeFouls, AwayFouls, HomeOffsides, AwayOffsides, HomeYellowCards, AwayYellowCards, HomeRedCards, AwayRedCards) VALUES")
srows = []
for fidk in sorted(statics.keys()):
    s = statics[fidk]
    srows.append(
        f"({fidk}, {s['pos'][0]}, {s['pos'][1]}, {s['sh'][0]}, {s['sh'][1]}, {s['sot'][0]}, {s['sot'][1]}, "
        f"{s['pas'][0]}, {s['pas'][1]}, {s['pacc'][0]}, {s['pacc'][1]}, {s['cor'][0]}, {s['cor'][1]}, "
        f"{s['foul'][0]}, {s['foul'][1]}, {s['off'][0]}, {s['off'][1]}, {s['yc'][0]}, {s['yc'][1]}, {s['rc'][0]}, {s['rc'][1]})"
    )
A(",\n".join(srows) + ";")
A("")

# MatchEvents (chunked)
A("-- 5) MatchEvents (goller+asist, sari/kirmizi kart, degisiklik, korner - tum oynanan maclar)")
def team_var_for_event(team):
    return var(team)

CHUNK = 800
for i in range(0, len(events), CHUNK):
    chunk = events[i:i + CHUNK]
    A("INSERT INTO MatchEvents (FixtureId, TeamId, PlayerName, PlayerId, MatchEventType, Description, Minute) VALUES")
    erows = []
    for (fx, team, player, etype, desc, minute) in chunk:
        ev_pid = player_id_map[(team, norm_name(player))]
        erows.append(f"({fx}, {team_var_for_event(team)}, {q(player)}, {ev_pid}, {etype}, {q(desc)}, {minute})")
    A(",\n".join(erows) + ";")
    A("")

A("COMMIT TRANSACTION;")
A("PRINT '38 haftalik fikstur yuklendi: 380 mac (1-20 oynanmis, 21-38 oynanmamis).';")

sql = "\n".join(lines)

out_path = r"C:\Users\cemre\OneDrive\Desktop\SoccerProject\SoccerProject.WebAPI\SeedData\SeriA_38Hafta_SeedData.sql"
# UTF-8 BOM: SSMS Turkce karakterleri dogru okusun diye
with open(out_path, "w", encoding="utf-8-sig") as fp:
    fp.write(sql)

# Ozet
finished = [f for f in fixtures if f["status"] == 2]
print("Toplam mac:", len(fixtures))
print("Oynanan (1-20):", len(finished), "| Oynanmayan (21-38):", len(fixtures) - len(finished))
print("MatchStatics:", len(statics), "| MatchEvents:", len(events))
print("Stadiums:", len(stadium_names), "| Players:", len(players), "| PlayerCareers:", len(careers))
print("Featured id:", FEATURED_ID, "week:", featured["week"])
print("1. hafta:", week_dates(1)[0].date(), "/", week_dates(1)[1].date())
print("20. hafta:", week_dates(20)[0].date(), "/", week_dates(20)[1].date(), "son mac:", match_datetime(20, 9))
print("21. hafta:", week_dates(21)[0].date(), "/", week_dates(21)[1].date())
print("38. hafta:", week_dates(38)[0].date(), "/", week_dates(38)[1].date())
print("Yazildi:", out_path)
