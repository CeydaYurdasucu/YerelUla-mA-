using System.Collections.Generic;
using System.Linq;
using System.Text;
using UlasimHaritaUygulamasi.Models;
using YolcuBase = UlasimHaritaUygulamasi.Yolcu.Yolcu;
using UlasimHaritaUygulamasi.Yolcu;


namespace UlasimHaritaUygulamasi.RotaHesaplayicilar
{
    public abstract class RotaHesaplayiciBase : IRotaHesaplayici
    {
        public abstract RotaSonucu RotaHesapla(Konum baslangic, Konum hedef, DurakVerisi veri, YolcuBase yolcu);

protected string RotaBilgisiOlustur(List<RotaAdimi> adimlar, YolcuBase yolcu, List<Durak> tumDuraklar)
{
    var bilgi = new StringBuilder();
    bilgi.AppendLine("<b>🧭 Rota Detayları:</b><br/>");

    int index = 1;
    foreach (var adim in adimlar)
    {
        string baslangicAd;
        string bitisAd;

        if (adim.Mode == "walk" && adim.Baslat != null && adim.Bitir != null)
        {
            
            baslangicAd = tumDuraklar.FirstOrDefault(d => d.Konumu().MesafeyiHesapla(adim.Baslat) < 0.05)?.name ?? "Başlangıç";
            bitisAd = tumDuraklar.FirstOrDefault(d => d.Konumu().MesafeyiHesapla(adim.Bitir) < 0.05)?.name ?? "Hedef";
        }
        else
        {
            
            baslangicAd = tumDuraklar.FirstOrDefault(d => d.id == adim.BaslangicDurakId)?.name ?? adim.BaslangicDurakId;
            bitisAd = tumDuraklar.FirstOrDefault(d => d.id == adim.BitisDurakId)?.name ?? adim.BitisDurakId;
        }

        string tur = adim.UlasimTuru switch
        {
            "yurume" => "🚶 Yürüme",
            "taksi" => "🚕 Taksi",
            "transfer" => "🔄 Transfer",
            "tramvay" => "🚋 Tramvay",
            _ => "🚌 Otobüs"
        };

        bilgi.AppendLine($"<b>{index}.</b> {baslangicAd} → {bitisAd} ({tur})<br/>");
        bilgi.AppendLine($"⏱ Süre: {adim.Sure} dk<br/>");
        bilgi.AppendLine($"💰 Ücret: {adim.Ucret:0.00} TL → <b>{yolcu.UcretHesapla(adim.Ucret, adim.UlasimTuru):0.00} TL</b><br/><br/>");
        index++;
    }

    bilgi.AppendLine("<hr>");
    bilgi.AppendLine($"<b>Toplam Süre:</b> {adimlar.Sum(a => a.Sure)} dk<br>");
    bilgi.AppendLine($"<b>Toplam Ücret:</b> {adimlar.Sum(a => yolcu.UcretHesapla(a.Ucret,a.UlasimTuru)).ToString("0.00")} TL");

    return bilgi.ToString();
}


        protected RotaAdimi YurutmeAdimi(Konum baslangic, Konum hedef, double yurumeHizi)
        {
            double mesafe = baslangic.MesafeyiHesapla(hedef);
            int sure = (int)(mesafe / yurumeHizi * 60);
            return new RotaAdimi
            {
                BaslangicDurakId = "Yürüme",
                BitisDurakId = "Yürüme",
                Sure = sure,
                Ucret = 0,
                UlasimTuru = "yurume",
                Mode = "walk",
                Baslat = baslangic,
                Bitir = hedef
            };
        }

        protected RotaAdimi TaksiAdimi(Konum baslangic, Konum hedef, Taxi taxiAyari)
        {
            double mesafe = baslangic.MesafeyiHesapla(hedef);
            int sure = (int)(mesafe / 50.0 * 60); 
            double ucret = taxiAyari.openingFee + mesafe * taxiAyari.costPerKm;

            return new RotaAdimi
            {
                BaslangicDurakId = "Taksi",
                BitisDurakId = "Taksi",
                Sure = sure,
                Ucret = ucret,
                UlasimTuru = "taksi",
                Mode = "taxi",
                Baslat = baslangic,
                Bitir = hedef
            };
        }
    }
}
