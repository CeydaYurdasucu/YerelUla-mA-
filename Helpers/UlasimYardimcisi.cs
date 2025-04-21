using System;
using System.Collections.Generic;
using System.Linq;
using UlasimHaritaUygulamasi.Models;

namespace UlasimHaritaUygulamasi.Helpers
{
    public static class UlasimYardimcisi
    {
        public static (Durak? yakinDurak, double mesafeKm) EnYakinDuragiBul(Konum nokta, List<Durak> duraklar)
        {
            Durak? enYakin = null;
            double minMesafe = double.MaxValue;

            foreach (var durak in duraklar)
            {
                var durakKonum = new Konum(durak.lat, durak.lon);
                double mesafe = nokta.MesafeyiHesapla(durakKonum);

                if (mesafe < minMesafe)
                {
                    minMesafe = mesafe;
                    enYakin = durak;
                }
            }

            return (enYakin, minMesafe);
        }

        public static RotaAdimi? UlasimAdimiOlustur(Konum nokta, Durak durak, double mesafeKm, DurakVerisi veri)
        {
            var baslangic = nokta;
            var hedef = new Konum(durak.lat, durak.lon);

            if (veri.walking.enabled && mesafeKm <= veri.walking.maxDistanceKm)
            {
                
                double hiz = veri.walking.averageSpeedKmh;
                int sureDakika = (int)Math.Round((mesafeKm / hiz) * 60);

                return new RotaAdimi
                {
                    Baslat = baslangic,
                    Bitir = hedef,
                    Mode = "walk",
                    Sure = sureDakika,
                    Ucret = 0
                };
            }
            else
            {
                
                double taksiHiz = 60;
                int sureDakika = (int)Math.Round((mesafeKm / taksiHiz) * 60);
                double ucret = veri.taxi!.openingFee + (mesafeKm * veri.taxi.costPerKm);

                return new RotaAdimi
                {
                    Baslat = baslangic,
                    Bitir = hedef,
                    Mode = "taxi",
                    Sure = sureDakika,
                    Ucret = ucret
                };
            }
        }
    }
}