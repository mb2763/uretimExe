using System.Windows.Forms;

namespace MyUI.Yardim
{
    /// <summary>
    /// Uygulama genelinde F1 tusunu yakalar ve aktif formun yardimini gosterir.
    /// Ayrica her aktif formu "?" yardim butonuyla bir kez sus(dekore)ler.
    /// Program.cs icinde Application.AddMessageFilter ile kaydedilir.
    /// </summary>
    public class HelpMessageFilter : IMessageFilter
    {
        private const int WM_KEYDOWN = 0x0100;

        public bool PreFilterMessage(ref Message m)
        {
            var f = HelpManager.EtkinForm();
            if (f != null)
            {
                HelpManager.Dekore(f);
            }

            if (m.Msg == WM_KEYDOWN && (int)m.WParam == (int)Keys.F1)
            {
                if (f != null)
                {
                    HelpManager.Goster(f);
                    return true; // F1'i tukettik
                }
            }
            return false;
        }
    }
}
