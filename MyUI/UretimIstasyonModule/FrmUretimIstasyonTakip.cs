using System;
using System.Collections.Generic;
using System.Linq;
using My.Business.Manager;
using My.Entities.Models;
using My.Kontrol.Formlar;
using MyUI.MyControl;

namespace MyUI.UretimIstasyonModule
{
    public partial class FrmUretimIstasyonTakip : MyFrmSadeFull
    {
        
        private List<UretimIstasyonTakipModel> _list;
        private UretimIstasyonTakipManager _mng;
        public FrmUretimIstasyonTakip()
        {
            InitializeComponent();
            this.Load += Frm_Load;
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
            var query = _list.Select((fruit) => fruit.IstasyonKodu);
            var Grp1 = query.GroupBy(item => item).ToList();
            PnlOrta.Controls.Clear();
            foreach (var itm in Grp1)
            {
                string key = itm.Key.ToString();
                PnlOrta.Controls.Add(new IstasyonCardControl()
                {
                    Model = _list.Where(c => c.IstasyonKodu == key).FirstOrDefault(),
                });
            }
            if (PnlOrta.Controls.Count < 1) PnlOrta.Controls.Add(new IstasyonCardControl());
            if (PnlOrta.Controls.Count < 2) PnlOrta.Controls.Add(new IstasyonCardControl());
            if (PnlOrta.Controls.Count < 3) PnlOrta.Controls.Add(new IstasyonCardControl());
            if (PnlOrta.Controls.Count < 4) PnlOrta.Controls.Add(new IstasyonCardControl());
            if (PnlOrta.Controls.Count < 5) PnlOrta.Controls.Add(new IstasyonCardControl());
            if (PnlOrta.Controls.Count < 6) PnlOrta.Controls.Add(new IstasyonCardControl());
            if (PnlOrta.Controls.Count < 7) PnlOrta.Controls.Add(new IstasyonCardControl());
            if (PnlOrta.Controls.Count < 8) PnlOrta.Controls.Add(new IstasyonCardControl());
            if (PnlOrta.Controls.Count < 9) PnlOrta.Controls.Add(new IstasyonCardControl());
        }

        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new UretimIstasyonTakipManager(Ortak.DbPro);
            Bagla();
        }

        private void BtnYenile_Click(object sender, EventArgs e) {
            Bagla();
        }
    }
}
