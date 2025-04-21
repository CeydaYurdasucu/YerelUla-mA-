namespace UlasimHaritaUygulamasi
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelYolcu;
        private System.Windows.Forms.Label labelOdeme;
        private System.Windows.Forms.TextBox txtBaslangic;
        private System.Windows.Forms.TextBox txtHedef;
        private System.Windows.Forms.Button btnRotaOlustur;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.ComboBox cmbYolcuTipi;
        private System.Windows.Forms.ComboBox cmbOdemeYontemi;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelYolcu = new System.Windows.Forms.Label();
            this.labelOdeme = new System.Windows.Forms.Label();
            this.txtBaslangic = new System.Windows.Forms.TextBox();
            this.txtHedef = new System.Windows.Forms.TextBox();
            this.btnRotaOlustur = new System.Windows.Forms.Button();
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.cmbYolcuTipi = new System.Windows.Forms.ComboBox();
            this.cmbOdemeYontemi = new System.Windows.Forms.ComboBox();

            this.SuspendLayout();

            
            this.BackColor = System.Drawing.Color.FromArgb(224, 247, 250);
            this.ClientSize = new System.Drawing.Size(1500, 750);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Name = "Form1";
            this.Text = "Ulaşım Rota Planlama Haritası";
            this.Load += new System.EventHandler(this.Form1_Load);

            
            this.label1.Text = "Başlangıç Koordinat (lat,lon)";
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);

            
            this.txtBaslangic.Location = new System.Drawing.Point(10, 30);
            this.txtBaslangic.Size = new System.Drawing.Size(300, 35);
            this.txtBaslangic.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtBaslangic.BackColor = System.Drawing.Color.White;
            this.txtBaslangic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            
            this.label2.Text = "Hedef Koordinat (lat,lon)";
            this.label2.Location = new System.Drawing.Point(10, 70);
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);

            
            this.txtHedef.Location = new System.Drawing.Point(10, 90);
            this.txtHedef.Size = new System.Drawing.Size(300, 35);
            this.txtHedef.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtHedef.BackColor = System.Drawing.Color.White;
            this.txtHedef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            
            this.labelYolcu.Text = "Yolcu Tipi:";
            this.labelYolcu.Location = new System.Drawing.Point(10, 130);
            this.labelYolcu.AutoSize = true;
            this.labelYolcu.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);

            
            this.cmbYolcuTipi.Location = new System.Drawing.Point(10, 150);
            this.cmbYolcuTipi.Size = new System.Drawing.Size(300, 35);
            this.cmbYolcuTipi.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbYolcuTipi.BackColor = System.Drawing.Color.White;
            this.cmbYolcuTipi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbYolcuTipi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYolcuTipi.Items.AddRange(new string[] { "Normal", "Öğrenci", "Yaşlı" });

            
            this.labelOdeme.Text = "Ödeme Yöntemi:";
            this.labelOdeme.Location = new System.Drawing.Point(10, 190);
            this.labelOdeme.AutoSize = true;
            this.labelOdeme.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);

            
            this.cmbOdemeYontemi.Location = new System.Drawing.Point(10, 210);
            this.cmbOdemeYontemi.Size = new System.Drawing.Size(300, 35);
            this.cmbOdemeYontemi.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbOdemeYontemi.BackColor = System.Drawing.Color.White;
            this.cmbOdemeYontemi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbOdemeYontemi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOdemeYontemi.Items.AddRange(new string[] { "Nakit", "Kredi Kartı", "KentKart" });

            
            this.btnRotaOlustur.Text = "Rota Oluştur";
            this.btnRotaOlustur.Location = new System.Drawing.Point(10, 260);
            this.btnRotaOlustur.Size = new System.Drawing.Size(300, 42);
            this.btnRotaOlustur.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRotaOlustur.BackColor = System.Drawing.Color.FromArgb(102, 187, 106);
            this.btnRotaOlustur.ForeColor = System.Drawing.Color.White;
            this.btnRotaOlustur.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRotaOlustur.FlatAppearance.BorderSize = 0;
            this.btnRotaOlustur.Click += new System.EventHandler(this.btnRotaOlustur_Click);

            
            this.webView.Location = new System.Drawing.Point(330, 10);
            this.webView.Size = new System.Drawing.Size(1140, 700);

            
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBaslangic);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHedef);
            this.Controls.Add(this.labelYolcu);
            this.Controls.Add(this.cmbYolcuTipi);
            this.Controls.Add(this.labelOdeme);
            this.Controls.Add(this.cmbOdemeYontemi);
            this.Controls.Add(this.btnRotaOlustur);
            this.Controls.Add(this.webView);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}