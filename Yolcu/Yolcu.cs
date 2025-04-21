namespace UlasimHaritaUygulamasi.Yolcu
{
    public abstract class Yolcu
    {
        public abstract double IndirimOrani { get; }

        public virtual double UcretHesapla(double normalUcret, string ulasimTuru)
        {
            if (ulasimTuru.Equals("taksi", StringComparison.OrdinalIgnoreCase))
                return normalUcret;

            return normalUcret * (1 - IndirimOrani);
        }
    }
}
