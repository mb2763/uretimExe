namespace MyUI.Yardim
{
    /// <summary>
    /// Bir form/sayfa icin yardim icerigi (help-uretim.json'dan yuklenir).
    /// </summary>
    public class HelpItem
    {
        public string Baslik { get; set; }
        public string Amac { get; set; }
        public string Onkosul { get; set; }
        public string Sonra { get; set; }
        public string[] Butonlar { get; set; }
        public string Istasyon { get; set; }
        public string Notlar { get; set; }
    }
}
