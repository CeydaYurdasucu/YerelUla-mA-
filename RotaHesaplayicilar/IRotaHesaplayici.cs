using UlasimHaritaUygulamasi.Models;

namespace UlasimHaritaUygulamasi.RotaHesaplayicilar
{
    public interface IRotaHesaplayici
    {
        RotaSonucu RotaHesapla(Konum baslangic, Konum hedef, DurakVerisi veri, Yolcu.Yolcu yolcu);
    }
}