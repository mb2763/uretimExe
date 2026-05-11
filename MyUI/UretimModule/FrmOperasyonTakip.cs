using System;
using System.Collections.Generic;
using System.Linq;
using My.Business.Manager;
using My.Entities.Models;
using My.Kontrol.Formlar;
using MyUI.UretimOperasyonModule; 
namespace MyUI.UretimModule
{
    public partial class FrmOperasyonTakip : MyFrmSadeFull
    {
        private UretimTakipManagerV2 _mng;
        private List<UretimTakipModelV2> _list;
        public FrmOperasyonTakip()
        {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new UretimTakipManagerV2(Ortak.DbPro);
            Bagla();
            TimerYenile.Start();
        }
        private void Bagla()
        {
            var rs = _mng.GetTakipList();
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            var query = _list.Select((xx) => xx.OperasyonKodu); 
            var Grp1 = query.GroupBy(item => item).ToList();
            PnlOrta.Controls.Clear();
            string opradi = "";
            foreach (var itm in Grp1)
            {
                opradi = "";
                string key = itm.Key ;
                var nameRs = _list.Where(xx => xx.OperasyonKodu == key).FirstOrDefault();
                if (nameRs!=null)
                { 
                    opradi = nameRs.OperasyonAdi;
                } 
                PnlOrta.Controls.Add(new OperasyonCardControlV2()
                {
                    OperasyonKodu = key , 
                    OperasyonAdi = opradi, 
                    Hareketler = _list.Where(c => c.OperasyonKodu == key).ToList(),
                    Action = Bagla
                });
            }
        }
        private void BtnYenile_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTumOperasyonlar_Click(object sender, EventArgs e)
        {
            FrmOperasyonTakipDetaylar f = new FrmOperasyonTakipDetaylar {TumunuBagla = true};
            f.ShowDialog();
        }
        private void TimerYenile_Tick(object sender, EventArgs e)
        {
            BtnYenile.PerformClick();
        }

    }
}