using My.Business.Service.UretimStoklar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyUI.AyarVeGenelModul {
    public partial class FrmStokKodGuncelle : Form {
        private readonly IUretimStokFisService _srv = Ortak.DbPro.UretimStokFis;
        public FrmStokKodGuncelle() {
            InitializeComponent();
            }

        private void btnKaydet_Click(object sender, EventArgs e) {
            try {
                if (MessageBox.Show(txtEskiStokKod.Text + " Stok Kodu," + txtYeniStokkod.Text + " Stok Kodu ile Değiştirilecektir. Devam Edilsin mi?", "Uyarı!!!", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    var rs = _srv.StokKoduGuncelle(txtEskiStokKod.Text, txtYeniStokkod.Text);
                    if (rs.Success) {
                        MessageBox.Show("Başarıyla Güncellendi.");
                        }
                    }
                } catch (Exception ex) {

                MessageBox.Show("Hata:" + ex.Message);
                }

            }
        }
    }
