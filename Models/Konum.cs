namespace UlasimHaritaUygulamasi.Models
{
    public class Konum
    {
        public double Enlem { get; set; }
        public double Boylam { get; set; }

        public Konum(double enlem, double boylam)
        {
            Enlem = enlem;
            Boylam = boylam;
        }

        public double MesafeyiHesapla(Konum hedefKonum)
        {
            const double R = 6371;
            double enlemFarki = (hedefKonum.Enlem - Enlem) * Math.PI / 180;
            double boylamFarki = (hedefKonum.Boylam - Boylam) * Math.PI / 180;

            double a = Math.Sin(enlemFarki / 2) * Math.Sin(enlemFarki / 2) +
                       Math.Cos(Enlem * Math.PI / 180) * Math.Cos(hedefKonum.Enlem * Math.PI / 180) *
                       Math.Sin(boylamFarki / 2) * Math.Sin(boylamFarki / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
