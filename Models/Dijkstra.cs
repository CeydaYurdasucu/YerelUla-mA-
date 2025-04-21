using System;
using System.Collections.Generic;
using System.Linq;

namespace UlasimHaritaUygulamasi.Models
{
    public class Dijkstra
    {
        private readonly Dictionary<string, Durak> _duraklar;
        private readonly Dictionary<string, List<Komsu>> _komsuluk;

        public Dijkstra(List<Durak> durakListesi, List<string> gecerliUlasimTurleri)
        {
            _duraklar = durakListesi.ToDictionary(d => d.id!);
            _komsuluk = new Dictionary<string, List<Komsu>>();

            foreach (var durak in durakListesi)
            {
                if (durak.id == null) continue;

                if (!_komsuluk.ContainsKey(durak.id))
                    _komsuluk[durak.id] = new List<Komsu>();

                if (gecerliUlasimTurleri.Contains(durak.type))
                {
                    if (durak.nextStops != null)
                    {
                        foreach (var next in durak.nextStops)
                        {
                            if (!string.IsNullOrEmpty(next.stopId))
                            {
                                _komsuluk[durak.id].Add(new Komsu
                                {
                                    HedefId = next.stopId!,
                                    Sure = next.sure,
                                    Ucret = next.ucret,
                                    UlasimTuru = durak.type!
                                });
                            }
                        }
                    }

                    if (gecerliUlasimTurleri.Contains("transfer") && durak.transfer != null)
                    {
                        if (!string.IsNullOrEmpty(durak.transfer.transferStopId))
                        {
                            _komsuluk[durak.id].Add(new Komsu
                            {
                                HedefId = durak.transfer.transferStopId!,
                                Sure = durak.transfer.transferSure,
                                Ucret = durak.transfer.transferUcret,
                                UlasimTuru = "transfer"
                            });
                        }
                    }
                }
            }
        }

        public Dijkstra(List<Durak> durakListesi)
            : this(durakListesi, new List<string> { "bus", "tram", "transfer" }) { }

        public RotaSonucu EnKisaRota(string baslangicId, string hedefId)
        {
            var mesafe = new Dictionary<string, int>();
            var onceki = new Dictionary<string, string?>();
            var ucret = new Dictionary<string, double>();
            var kuyruk = new SortedSet<(int, string)>(Comparer<(int, string)>.Create((a, b) =>
                a.Item1 == b.Item1 ? a.Item2.CompareTo(b.Item2) : a.Item1.CompareTo(b.Item1)));

            foreach (var durakId in _duraklar.Keys)
            {
                mesafe[durakId] = int.MaxValue;
                ucret[durakId] = double.MaxValue;
                onceki[durakId] = null;
            }

            mesafe[baslangicId] = 0;
            ucret[baslangicId] = 0;
            kuyruk.Add((0, baslangicId));

            while (kuyruk.Any())
            {
                var (sure, current) = kuyruk.Min;
                kuyruk.Remove(kuyruk.Min);

                if (current == hedefId)
                    break;

                if (!_komsuluk.ContainsKey(current)) continue;

                foreach (var komsu in _komsuluk[current])
                {
                    int yeniSure = sure + komsu.Sure;
                    double yeniUcret = ucret[current] + komsu.Ucret;

                    if (yeniSure < mesafe[komsu.HedefId])
                    {
                        kuyruk.Remove((mesafe[komsu.HedefId], komsu.HedefId));
                        mesafe[komsu.HedefId] = yeniSure;
                        ucret[komsu.HedefId] = yeniUcret;
                        onceki[komsu.HedefId] = current;
                        kuyruk.Add((yeniSure, komsu.HedefId));
                    }
                }
            }

            var yol = new List<string>();
            string? node = hedefId;
            while (node != null)
            {
                yol.Insert(0, node);
                node = onceki[node];
            }

            var adimlar = new List<RotaAdimi>();
            for (int i = 0; i < yol.Count - 1; i++)
            {
                var kaynak = yol[i];
                var hedef = yol[i + 1];
                var komsu = _komsuluk.ContainsKey(kaynak)
                    ? _komsuluk[kaynak].FirstOrDefault(k => k.HedefId == hedef)
                    : null;

                if (komsu != null)
                {
                    string mode = komsu.UlasimTuru switch
                    {
                        "bus" or "tram" => "transit",
                        "transfer" => "transfer",
                        _ => komsu.UlasimTuru
                    };

                    adimlar.Add(new RotaAdimi
                    {
                        BaslangicDurakId = kaynak,
                        BitisDurakId = hedef,
                        Sure = komsu.Sure,
                        Ucret = komsu.Ucret,
                        UlasimTuru = komsu.UlasimTuru,
                        Mode = mode,
                        Baslat = _duraklar.TryGetValue(kaynak, out var d1) ? d1.Konumu() : null,
                        Bitir = _duraklar.TryGetValue(hedef, out var d2) ? d2.Konumu() : null
                    });


                }
            }

            return new RotaSonucu
            {
                Duraklar = yol,
                Adimlar = adimlar,
                ToplamSure = adimlar.Sum(a => a.Sure),
                ToplamUcret = adimlar.Sum(a => a.Ucret)
            };
        }
    }
    public class Komsu
    {
        public string Id { get; set; } = "";
        public string BaslangicDurakId { get; set; } = "";
        public string BitisDurakId { get; set; } = "";
        public int Sure { get; set; }
        public double Ucret { get; set; }
        public string UlasimTuru { get; set; } = "";

        public string HedefId
        {
            get => BitisDurakId;
            set => BitisDurakId = value;
        }

        public string KaynakId
        {
            get => BaslangicDurakId;
            set => BaslangicDurakId = value;
        }
    }


}