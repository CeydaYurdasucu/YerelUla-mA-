namespace UlasimHaritaUygulamasi.Models
{
    public class DurakVerisi
    {
        public string? city { get; set; }
        public Taxi? taxi { get; set; }
        public required WalkingAyari walking { get; set; }
        public List<Durak>? duraklar { get; set; }
    }

    public class Taxi
    {
        public double openingFee { get; set; }
        public double costPerKm { get; set; }
    }

    public class WalkingAyari
    {
        public bool enabled { get; set; }
        public double maxDistanceKm { get; set; }
        public double averageSpeedKmh { get; set; }
    }

    public class Durak
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";
        public string type { get; set; } = "";
        public double lat { get; set; }
        public double lon { get; set; }
        public bool sonDurak { get; set; }
        public List<NextStop> nextStops { get; set; } = new();
        public Transfer? transfer { get; set; }

        public Konum Konumu()
        {
            return new Konum(lat, lon);
        }
    }

    public class NextStop
    {
        public string stopId { get; set; } = "";
        public double mesafe { get; set; }
        public int sure { get; set; }
        public double ucret { get; set; }
    }

    public class Transfer
    {
        public string transferStopId { get; set; } = "";
        public int transferSure { get; set; }
        public double transferUcret { get; set; }
    }
}
