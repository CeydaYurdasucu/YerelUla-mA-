using System.Collections.Generic;
using System.Linq;
using UlasimHaritaUygulamasi.Models;
using YolcuBase = UlasimHaritaUygulamasi.Yolcu.Yolcu;

namespace UlasimHaritaUygulamasi.RotaHesaplayicilar
{
    public class EnUcuzRotaHesaplayici : RotaHesaplayiciBase
    {
        public override RotaSonucu RotaHesapla(Konum baslangic, Konum hedef, DurakVerisi veri, YolcuBase yolcu)
        {
            var duraklar = veri.duraklar!;
            var gecerliTurler = new List<string> { "bus", "tram", "transfer" };

            var dijkstra = new DijkstraUcreteGore(duraklar, gecerliTurler);

            var enYakinDurak = duraklar
                .Where(d => gecerliTurler.Contains(d.type))
                .OrderBy(d => new Konum(d.lat, d.lon).MesafeyiHesapla(baslangic))
                .First();

            var hedefeYakinDurak = duraklar
                .Where(d => gecerliTurler.Contains(d.type))
                .OrderBy(d => new Konum(d.lat, d.lon).MesafeyiHesapla(hedef))
                .First();

            var rota = dijkstra.EnUcuzRota(enYakinDurak.id, hedefeYakinDurak.id);

            
            double mesafeIlk = baslangic.MesafeyiHesapla(new Konum(enYakinDurak.lat, enYakinDurak.lon));
            if (mesafeIlk > 2)
            {
                rota.Adimlar.Insert(0, TaksiAdimi(baslangic, new Konum(enYakinDurak.lat, enYakinDurak.lon), veri.taxi!));
            }
            else
            {
                rota.Adimlar.Insert(0, YurutmeAdimi(baslangic, new Konum(enYakinDurak.lat, enYakinDurak.lon), veri.walking!.averageSpeedKmh));
            }

            
            double mesafeSon = hedefeYakinDurak != null ? new Konum(hedefeYakinDurak.lat, hedefeYakinDurak.lon).MesafeyiHesapla(hedef) : 0;
            if (mesafeSon > 2)
            {
                rota.Adimlar.Add(TaksiAdimi(new Konum(hedefeYakinDurak.lat, hedefeYakinDurak.lon), hedef, veri.taxi!));
            }
            else
            {
                rota.Adimlar.Add(YurutmeAdimi(new Konum(hedefeYakinDurak.lat, hedefeYakinDurak.lon), hedef, veri.walking!.averageSpeedKmh));
            }

foreach (var adim in rota.Adimlar)
{
    
    var baslangicDurak = veri.duraklar?.FirstOrDefault(d => d.id == adim.BaslangicDurakId);
    var bitisDurak = veri.duraklar?.FirstOrDefault(d => d.id == adim.BitisDurakId);

    
    adim.Baslat ??= baslangicDurak?.Konumu();
    adim.Bitir ??= bitisDurak?.Konumu();

    
    adim.Mode = adim.UlasimTuru switch
    {
        "bus" or "tram" => "transit",
        "yurume" => "walk",
        "taksi" => "taxi",
        "transfer" => "transfer",
        _ => "other"
    };
}





            rota.ToplamSure = rota.Adimlar.Sum(a => a.Sure);
            rota.ToplamUcret = rota.Adimlar.Sum(a => yolcu.UcretHesapla(a.Ucret, a.UlasimTuru));
            rota.Baslik = "💸 En Ucuz Rota";
            rota.Bilgi = RotaBilgisiOlustur(rota.Adimlar, yolcu, duraklar);

            return rota;
        }
    }
}
