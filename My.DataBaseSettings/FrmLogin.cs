using System;
using System.Windows.Forms;

namespace My.DatabaseSettings
{
    public partial class FrmLogin : Form
    {
        public bool GirisYapildi = false;
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void Kontrol()
        { // GSYA_2
            DateTime trh = DateTime.Now;
            string kont = trh.Day.ToString().PadLeft(2, '0') + trh.Hour.ToString().PadLeft(2, '0') +
                (trh.Year - 2000).ToString().PadLeft(2, '0') + trh.Month.ToString().PadLeft(2, '0');

            if (kont == textBox1.Text.Trim())
            {
                GirisYapildi = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Şifre Geçersiz");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kontrol();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                Kontrol();
            }
        }
    }
}
