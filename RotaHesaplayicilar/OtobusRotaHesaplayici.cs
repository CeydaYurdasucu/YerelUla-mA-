using System;
using System.Collections.Generic;
using System.Linq;
using UlasimHaritaUygulamasi.Models;
using UlasimHaritaUygulamasi.Yolcu;
using YolcuBase = UlasimHaritaUygulamasi.Yolcu.Yolcu;

namespace UlasimHaritaUygulamasi.RotaHesaplayicilar
{
    public class OtobusRotaHesaplayici : RotaHesaplayiciBase
    {
        public override RotaSonucu RotaHesapla(Konum baslangic, Konum hedef, DurakVerisi veri, YolcuBase yolcu)
        {
            var duraklar = veri.duraklar!;
            var otobusDuraklar = duraklar.Where(d => d.type == "bus").ToList();

            
            var dijkstra = new Dijkstra(duraklar, new List<string> { "bus", "transfer" });

            var baslangicDuragi = otobusDuraklar
                .OrderBy(d => new Konum(d.lat, d.lon).MesafeyiHesapla(baslangic)).First();
            var hedefDuragi = otobusDuraklar
                .OrderBy(d => new Konum(d.lat, d.lon).MesafeyiHesapla(hedef)).First();

            var rota = dijkstra.EnKisaRota(baslangicDuragi.id!, hedefDuragi.id!);

            
            foreach (var adim in rota.Adimlar)
            {
                if (adim.UlasimTuru == "bus" || adim.UlasimTuru == "tramvay")
                    adim.Mode = "transit";
            }

            
            foreach (var adim in rota.Adimlar)
            {
                var bas = veri.duraklar.FirstOrDefault(d => d.id == adim.BaslangicDurakId);
                var bit = veri.duraklar.FirstOrDefault(d => d.id == adim.BitisDurakId);

                if (bas != null && bit != null)
                {
                    adim.Baslat = new Konum(bas.lat, bas.lon);
                    adim.Bitir = new Konum(bit.lat, bit.lon);
                }
            }

            if (rota.Adimlar.Count == 0)
            {
                rota.Baslik = "Sadece Otobüs";
                rota.Bilgi = "Otobüs bağlantısı bulunamadı.";
                return rota;
            }

            
            double mesafeIlk = baslangic.MesafeyiHesapla(new Konum(baslangicDuragi.lat, baslangicDuragi.lon));
            if (mesafeIlk > 2)
            {
                rota.Adimlar.Insert(0, new RotaAdimi
                {
                    BaslangicDurakId = "Başlangıç",
                    BitisDurakId = baslangicDuragi.id!,
                    Sure = (int)(mesafeIlk / veri.walking!.averageSpeedKmh * 60),
                    Ucret = mesafeIlk * veri.taxi!.costPerKm + veri.taxi.openingFee,
                    UlasimTuru = "taksi",
                    Mode = "taxi",
                    Baslat = baslangic,
                    Bitir = new Konum(baslangicDuragi.lat, baslangicDuragi.lon)
                });
            }
            else
            {
                rota.Adimlar.Insert(0, new RotaAdimi
                {
                    BaslangicDurakId = "Başlangıç",
                    BitisDurakId = baslangicDuragi.id!,
                    Sure = (int)(mesafeIlk / veri.walking!.averageSpeedKmh * 60),
                    Ucret = 0,
                    UlasimTuru = "yurume",
                    Mode = "walk",
                    Baslat = baslangic,
                    Bitir = new Konum(baslangicDuragi.lat, baslangicDuragi.lon)
                });
            }

            
            double mesafeSon = new Konum(hedefDuragi.lat, hedefDuragi.lon).MesafeyiHesapla(hedef);
            if (mesafeSon > 2)
            {
                rota.Adimlar.Add(new RotaAdimi
                {
                    BaslangicDurakId = hedefDuragi.id!,
                    BitisDurakId = "Hedef",
                    Sure = (int)(mesafeSon / veri.walking!.averageSpeedKmh * 60),
                    Ucret = mesafeSon * veri.taxi!.costPerKm + veri.taxi.openingFee,
                    UlasimTuru = "taksi",
                    Mode = "taxi",
                    Baslat = new Konum(hedefDuragi.lat, hedefDuragi.lon),
                    Bitir = hedef
                });
            }
            else
            {
                rota.Adimlar.Add(new RotaAdimi
                {
                    BaslangicDurakId = hedefDuragi.id!,
                    BitisDurakId = "Hedef",
                    Sure = (int)(mesafeSon / veri.walking!.averageSpeedKmh * 60),
                    Ucret = 0,
                    UlasimTuru = "yurume",
                    Mode = "walk",
                    Baslat = new Konum(hedefDuragi.lat, hedefDuragi.lon),
                    Bitir = hedef
                });
            }

            rota.ToplamSure = rota.Adimlar.Sum(x => x.Sure);
            rota.ToplamUcret = rota.Adimlar.Sum(x => yolcu.UcretHesapla(x.Ucret, x.UlasimTuru));
            rota.Baslik = "Sadece Otobüs";
            rota.Bilgi = RotaBilgisiOlustur(rota.Adimlar, yolcu, duraklar);

            return rota;
        }
    }
}
