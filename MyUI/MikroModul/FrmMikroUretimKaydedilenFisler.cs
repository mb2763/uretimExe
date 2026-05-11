using My.Business.Manager;
using My.Core.Result;
using My.Entities.Mikro;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.MikroModul {
    public partial class FrmMikroUretimKaydedilenFisler : MyFrmKayit {

        private MikroKayitManager _mngMikroKayit;
        private SiparisManager _mngSip;
        public string BelgeNo { get; set; }
        public Guid? SipId { get; set; }
        List<MikroStokHareketAktarilanFisler> _list;

        public FrmMikroUretimKaydedilenFisler() {
            InitializeComponent();
        }

        private void FrmMikroUretimKaydedilenFisler_Load(object sender, EventArgs e) {
            _mngMikroKayit = new MikroKayitManager(Ortak.DbPro, Ortak.DbMikro);
            _mngSip = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            Bagla();
        }
         
        private void Bagla() {
            var rs = _mngMikroKayit.GetMikroAktarilanFisByBelgeNo(BelgeNo);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            myGrid1.DataSource = _list;
            myView1.SutunEditAc("Sec");
        }

        private IResult FisSil(string seri, string sira) {
            var rs = _mngMikroKayit.DeleteMikroAktarilanFisBySeriSira(seri, sira);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return new ErrorResult();
            }
            return new SuccessResult();

        }

        private void BtnKaydet_Click(object sender, EventArgs e) {

        }

        private void BtnSil_Click(object sender, EventArgs e) {
            if (MesajSor("Seçilen Kayıtları silmek istiyormusunuz.")) {
                bool fisKaldimi = false;
                foreach (var itm in _list) {
                    if (itm.Sec != true) {
                        fisKaldimi = true;
                        continue; 
                    }
                    var rs = FisSil(itm.EvrakSeri, itm.EvrakSira);
                    if (!rs.Success) {
                        break;
                    }
                }
                if (!fisKaldimi) {
                    var son = _mngSip.SiparisEntGuncelle(SipId, "", "", "", "",0,0);
                    if (!son.Success) {
                        MesajHata(son.Message);
                        return;
                    }
                }
                // fiş kalmamışsa entegrasyonu temizle .
                Bagla();
            }

        }

        private void BtnKapat_Click_1(object sender, EventArgs e) {
            this.Close();
        }
    }
}
