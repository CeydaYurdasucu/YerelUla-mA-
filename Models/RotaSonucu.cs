namespace UlasimHaritaUygulamasi.Models
{
    public class RotaAdimi
    {
        public string UlasimTuru { get; set; } = string.Empty;

        public string BaslangicDurakId { get; set; } = "";
        public string BitisDurakId { get; set; } = "";
        public int Sure { get; set; }
        public double Ucret { get; set; }

        
        public string Aciklama { get; set; } = "";
        public string Mode { get; set; } = ""; 
        public Konum? Baslat { get; set; }
        public Konum? Bitir { get; set; }
    }

    public class RotaSonucu
    {
        public List<string> Duraklar { get; set; } = new();
        public List<RotaAdimi> Adimlar { get; set; } = new();
        public int ToplamSure { get; set; }
        public double ToplamUcret { get; set; }

        public string Baslik { get; set; } = "";
        public string Bilgi { get; set; } = "";
    }
}
