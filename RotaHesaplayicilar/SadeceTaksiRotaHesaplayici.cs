using System;
using System.Collections.Generic;
using UlasimHaritaUygulamasi.Models;

namespace UlasimHaritaUygulamasi.RotaHesaplayicilar
{
    public class SadeceTaksiRotaHesaplayici : RotaHesaplayiciBase
    {
        public override RotaSonucu RotaHesapla(Konum baslangic, Konum hedef, DurakVerisi veri, Yolcu.Yolcu yolcu)
        {
            var rota = new RotaSonucu();

            var adim = TaksiAdimi(baslangic, hedef, veri.taxi);
            rota.Adimlar.Add(adim);

            rota.ToplamSure = adim.Sure;
            rota.ToplamUcret = yolcu.UcretHesapla(adim.Ucret, "taksi");
            rota.Baslik = "Sadece Taksi";
            rota.Bilgi = RotaBilgisiOlustur(rota.Adimlar, yolcu, veri.duraklar!);

            return rota;
        }
    }
}
