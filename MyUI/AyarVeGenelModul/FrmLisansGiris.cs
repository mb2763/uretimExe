using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyUI.AyarVeGenel
{
    public partial class FrmLisansGiris : Form
    {
        public FrmLisansGiris()
        {
            InitializeComponent();
        }
        private void FrmLisans_Load(object sender, EventArgs e)
        {
            SistemLisansKontrolu();
        }

        public void SistemLisansKontrolu()
        {
            textBox1.Text = OrtakLis.SistemLisansKontrolKey();


            string yol = Environment.CurrentDirectory + "\\lisans.lic";


            if (File.Exists(yol))
            {
                var str = File.ReadAllText(yol);
                textBox2.Text = str;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kontrol = OrtakLis.SistemLisansKeyKontrolu(textBox1.Text);

            string karsilik = textBox2.Text;

            if (kontrol == karsilik)
            {
                string yol = Environment.CurrentDirectory + "\\lisans.lic";
                File.WriteAllText(yol, karsilik);

                Application.Restart();
            }
            else
            {
                MessageBox.Show("Lisans Kodu Geçersiz");
            }
        }
    }
}
