using System.Collections.Generic;
using System.Linq;

namespace UlasimHaritaUygulamasi.Models
{
    public class DijkstraUcreteGore
    {
        private readonly Dictionary<string, List<Komsu>> komsular = new();
        private readonly List<Durak> duraklar;

        public DijkstraUcreteGore(List<Durak> duraklar, List<string> gecerliTurler)
        {
            this.duraklar = duraklar;

            foreach (var durak in duraklar)
            {
                if (!gecerliTurler.Contains(durak.type)) continue;

                foreach (var ns in durak.nextStops)
                {
                    if (!komsular.ContainsKey(durak.id))
                        komsular[durak.id] = new();

                    komsular[durak.id].Add(new Komsu
                    {
                        Id = ns.stopId,
                        BaslangicDurakId = durak.id,
                        BitisDurakId = ns.stopId,
                        Sure = ns.sure,
                        Ucret = ns.ucret,
                        UlasimTuru = durak.type
                    });
                }

                
                if (durak.transfer != null)
                {
                    if (!komsular.ContainsKey(durak.id))
                        komsular[durak.id] = new();

                    komsular[durak.id].Add(new Komsu
                    {
                        Id = durak.transfer.transferStopId,
                        BaslangicDurakId = durak.id,
                        BitisDurakId = durak.transfer.transferStopId,
                        Sure = durak.transfer.transferSure,
                        Ucret = durak.transfer.transferUcret,
                        UlasimTuru = "transfer"
                    });
                }
            }
        }

        public RotaSonucu EnUcuzRota(string baslangicId, string hedefId)
        {
            var mesafe = new Dictionary<string, double>();
            var onceki = new Dictionary<string, string?>();
            var kuyruk = new SortedSet<(double maliyet, string id)>();

            foreach (var d in duraklar)
            {
                mesafe[d.id] = double.MaxValue;
                onceki[d.id] = null;
            }

            mesafe[baslangicId] = 0;
            kuyruk.Add((0, baslangicId));

            while (kuyruk.Count > 0)
            {
                var (guncelMaliyet, guncelId) = kuyruk.Min;
                kuyruk.Remove(kuyruk.Min);

                if (!komsular.ContainsKey(guncelId))
                    continue;

                foreach (var komsu in komsular[guncelId])
                {
                    double yeniMaliyet = mesafe[guncelId] + komsu.Ucret;
                    if (yeniMaliyet < mesafe[komsu.Id])
                    {
                        kuyruk.Remove((mesafe[komsu.Id], komsu.Id));
                        mesafe[komsu.Id] = yeniMaliyet;
                        onceki[komsu.Id] = guncelId;
                        kuyruk.Add((yeniMaliyet, komsu.Id));
                    }
                }
            }


var adimlar = new List<RotaAdimi>();
string? current = hedefId;
while (current != null && current != baslangicId)
{
    string? oncekiId = onceki[current];
    if (oncekiId == null) break;

    var komsu = komsular[oncekiId].First(k => k.Id == current);

    var baslatDurak = duraklar.FirstOrDefault(d => d.id == komsu.BaslangicDurakId);
    var bitirDurak = duraklar.FirstOrDefault(d => d.id == komsu.BitisDurakId);

    adimlar.Insert(0, new RotaAdimi
    {
        BaslangicDurakId = komsu.BaslangicDurakId,
        BitisDurakId = komsu.BitisDurakId,
        Sure = komsu.Sure,
        Ucret = komsu.Ucret,
        UlasimTuru = komsu.UlasimTuru,
        Mode = komsu.UlasimTuru switch
        {
            "bus" or "tram" => "transit",
            "transfer" => "transfer",
            _ => "other"
        },
        Baslat = baslatDurak?.Konumu(),
        Bitir = bitirDurak?.Konumu()
    });

    current = oncekiId;
}


            return new RotaSonucu { Adimlar = adimlar };
        }
    }
}
