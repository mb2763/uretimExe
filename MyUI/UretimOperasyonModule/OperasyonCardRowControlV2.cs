using System;
using System.Drawing;
using System.Windows.Forms;
using My.Entities.Models;
using MyUI.MyControl;

namespace MyUI.UretimOperasyonModule
{
    public partial class OperasyonCardRowControlV2 : UserControl
    {
        public int Id = 0;
        public Action Action;
        public UretimTakipModelV2 Model { get; set; }



        public event EventHandler<OperasyonCardRowEventArgsV2> SecildiEvent;

        public OperasyonCardRowControlV2()
        {
            InitializeComponent();
        }

        private void OperasyonCardRowControl_Load(object sender, EventArgs e)
        {
            LblRecete.Text = Model.ReceteAdi;
            if (Model.Turu == "Uretimde")
            {
                LblMiktar.Text = (Model.IslemdekiMiktar - Model.UretimMiktari).ToString();
            }
            else
            {
                LblMiktar.Text = (Model.PlanlananMiktar - Model.IslemdekiMiktar).ToString();
            }

            LblSiparisKodu.Text = Model.SiparisKodu.ToString();
            LblCariUnvani.Text = Model.CariUnvani.ToString();

            OperasyonCardManagerV2.SecildiEvent += SecilenRow;
            if (Id == 0)
            {
                Id = OperasyonCardManagerV2.RowId++;
            }
        }
        protected virtual void OnSecildiEvent(OperasyonCardRowEventArgsV2 e)
        {
            EventHandler<OperasyonCardRowEventArgsV2> handler = SecildiEvent;
            if (handler != null)
            {
                handler(this, e);
            }

        }
        public void Secildi()
        {
            OperasyonCardRowEventArgsV2 args = new OperasyonCardRowEventArgsV2();
            args.Id = Id;
            args.Model = Model;
            OnSecildiEvent(args);

            OperasyonCardManagerV2.SecildiTetikle(this, args);

            this.BackColor = Color.Teal;
        }
        void SecilenRow(object sender, OperasyonCardRowEventArgsV2 e)
        {
            int eid = e.Id;
            if (eid != Id)
            {
                Secilmedi();
            }

        }

        public void Secilmedi()
        {
            this.BackColor = Color.Transparent;
        }

        private void LblRecete_Click(object sender, EventArgs e)
        {
            Secildi();
        }

        private void LblMiktar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Action != null)
            {
                Action.Invoke();
            }
        }
    }
}
