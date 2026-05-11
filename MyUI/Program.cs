using System;
using System.Windows.Forms;

namespace MyUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // My.Kontrol.KeyKontrol.SetKey("6E41wNq6tSPznjyult5Kriij5157sLOkcWA8BSdodCk="); 
             My.Kontrol.KeyKontrol.SetKey("ldSvAgr7jI7R+E4Yg+gnbzpt+gI4WVn+coNELpxlLjs="); 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false); 
            Application.Run(new FrmAna() { Opacity=0.0});
        }        
    }
}
