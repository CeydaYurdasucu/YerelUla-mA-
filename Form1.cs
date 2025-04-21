#nullable enable

using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UlasimHaritaUygulamasi.Models;
using UlasimHaritaUygulamasi.Yolcu;
using UlasimHaritaUygulamasi.RotaHesaplayicilar;
using UlasimHaritaUygulamasi.Helpers; 

namespace UlasimHaritaUygulamasi
{
    public partial class Form1 : Form
    {
        private DurakVerisi? veriler;

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
            cmbYolcuTipi.SelectedIndex = 0;
            cmbOdemeYontemi.SelectedIndex = 0; 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webView.Source = new Uri(Path.Combine(Application.StartupPath, "map.html"));
            webView.NavigationCompleted += (_, _) =>
            {
                webView.ExecuteScriptAsync("loadEmptyMap();");
            };
        }

        private void btnRotaOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                if (!double.TryParse(txtBaslangic.Text.Split(',')[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double baslangicEnlem) ||
                    !double.TryParse(txtBaslangic.Text.Split(',')[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double baslangicBoylam) ||
                    !double.TryParse(txtHedef.Text.Split(',')[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double hedefEnlem) ||
                    !double.TryParse(txtHedef.Text.Split(',')[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double hedefBoylam))
                {
                    MessageBox.Show("Koordinatlar doğru formatta girilmelidir (örnek: 40.76,29.94)");
                    return;
                }

                if (veriler == null)
                {
                    string json = File.ReadAllText("veriseti.json");
                    veriler = JsonConvert.DeserializeObject<DurakVerisi>(json);
                }

                var baslangic = new Konum(baslangicEnlem, baslangicBoylam);
                var hedef = new Konum(hedefEnlem, hedefBoylam);
                string yolcuTipi = cmbYolcuTipi.SelectedItem?.ToString() ?? "Normal";
                string odemeYontemiStr = cmbOdemeYontemi.SelectedItem?.ToString() ?? "Nakit";

                
                Yolcu.Yolcu yolcu = yolcuTipi switch
                {
                    "Öğrenci" => new OgrenciYolcu(),
                    "Yaşlı" => new YasliYolcu(),
                    _ => new GenelYolcu()
                };

                
                IOdemeYontemi odemeYontemi = OdemeYontemiFactory.Olustur(odemeYontemiStr);

                var alternatifRotalar = new List<object>();
                List<IRotaHesaplayici> hesaplayicilar = new()
                {
                    new SadeceTaksiRotaHesaplayici(),
                    new OtobusRotaHesaplayici(),
                    new EnUcuzRotaHesaplayici()
                };

                foreach (var hesaplayici in hesaplayicilar)
                {
                    var sonuc = hesaplayici.RotaHesapla(baslangic, hedef, veriler!, yolcu);

                    var rotaCoords = new List<object>();
                    foreach (var adim in sonuc.Adimlar)
                    {
                        if (adim.Baslat != null && adim.Bitir != null)
                        {
                            rotaCoords.Add(new
                            {
                                lat1 = adim.Baslat.Enlem,
                                lon1 = adim.Baslat.Boylam,
                                lat2 = adim.Bitir.Enlem,
                                lon2 = adim.Bitir.Boylam,
                                mode = adim.Mode
                            });
                        }
                    }

                    string odemeMesaji = odemeYontemi.Ode(sonuc.ToplamUcret);

                    alternatifRotalar.Add(new
                    {
                        ad = sonuc.Baslik,
                        bilgi = sonuc.Bilgi + $"<br><br><b>🧾 Ödeme:</b> {odemeMesaji}",
                        rotaCoords
                    });
                }

                var duraklarJson = veriler!.duraklar.Select(d => new
                {
                    lat = d.lat,
                    lon = d.lon,
                    popup = d.name
                }).ToList();

                string js = $"initializeMap({baslangic.Enlem.ToString(CultureInfo.InvariantCulture)}, {baslangic.Boylam.ToString(CultureInfo.InvariantCulture)}, " +
                            $"{hedef.Enlem.ToString(CultureInfo.InvariantCulture)}, {hedef.Boylam.ToString(CultureInfo.InvariantCulture)}, " +
                            $"{JsonConvert.SerializeObject(duraklarJson)}, false, {JsonConvert.SerializeObject(alternatifRotalar)});";

                webView.ExecuteScriptAsync(js);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
