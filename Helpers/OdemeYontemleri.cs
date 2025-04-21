namespace UlasimHaritaUygulamasi.Helpers
{
    
    public interface IOdemeYontemi
    {
        string Ode(double miktar);
    }

    
    public class NakitOdeme : IOdemeYontemi
    {
        public string Ode(double miktar)
        {
            return $"💵 {miktar:0.00} TL nakit olarak ödendi.";
        }
    }

    
    public class KrediKartiOdeme : IOdemeYontemi
    {
        public string Ode(double miktar)
        {
            return $"💳 {miktar:0.00} TL kredi kartı ile ödendi.";
        }
    }

    
    public class KentKartOdeme : IOdemeYontemi
    {
        public string Ode(double miktar)
        {
            return $"🟦 {miktar:0.00} TL KentKart bakiyesinden düşüldü.";
        }
    }

    
    public static class OdemeYontemiFactory
    {
        public static IOdemeYontemi Olustur(string yontem)
        {
            return yontem switch
            {
                "Nakit" => new NakitOdeme(),
                "Kredi Kartı" => new KrediKartiOdeme(),
                "KentKart" => new KentKartOdeme(),
                _ => throw new ArgumentException("Geçersiz ödeme yöntemi seçildi.")
            };
        }
    }
}
