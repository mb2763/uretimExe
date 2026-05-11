using System;
using System.IO;
using System.Management;

namespace MyUI
{
    public class OrtakLis
    {
        public static bool SistemLisansKontrolu()
        {
            string yol = Environment.CurrentDirectory + "\\lisans.lic";
            bool lisansli = false;
            if (File.Exists(yol))
            {
                var str = File.ReadAllText(yol);
                string kontrol = Kisa(SistemLisansKontrolKey());
                if (kontrol == str)
                {
                    lisansli = true;
                }
            }
            return lisansli;
        }
        public static string SistemLisansKeyKontrolu(string data)
        {
            return Kisa(data);
        }
        public static string SistemLisansKontrolKey()
        {
            string don = GetBaseBoard();
            return Kisa(don);
        }
        public static string Sifrele64(string yazi)
        {
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider MD5şifreleyici = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] baytlar = System.Text.ASCIIEncoding.Default.GetBytes(yazi);
                byte[] hash = MD5şifreleyici.ComputeHash(baytlar);
                double kapasite = (hash.Length * 2 + (hash.Length / (double)8));
                System.Text.StringBuilder sb = new System.Text.StringBuilder((int)kapasite);
                int I;
                var loopTo = hash.Length - 1;
                for (I = 0; I <= loopTo; I++)
                    sb.Append(BitConverter.ToString(hash, I, 1));
                return sb.ToString().TrimEnd(new char[] { ' ' });
            }
            catch //(Exception ex)
            {
                return "";
            }
        }
        private static string GetBaseBoard()
        {
            var baseboard = GetBaseCpuInfo().SerialNumber;
            return baseboard;
        }

        public static BaseBoard GetBaseCpuInfo()
        {
            ManagementObjectSearcher management = new ManagementObjectSearcher(
                " select * From win32_processor");
            ManagementObjectCollection collection = management.Get();
            BaseBoard mdl = new BaseBoard();



            //var str = sb.ToString();
            foreach (var itm in collection)
            {
                mdl.Name = itm["Name"].ToString();
                mdl.SerialNumber = itm["processorID"].ToString();
                if (!string.IsNullOrEmpty(mdl.SerialNumber))
                {
                    break;
                }
            }
            return mdl;
        }
        
        private static string Kisa(string data)
        {
            string X = Sifrele64(data);
            string C = "";
            int topla = 0;
            foreach (char i in X.ToString())
            {
                if (topla < 1)
                {
                    C = C + Convert.ToInt32(i) * 571;
                }
                topla = topla + (Convert.ToInt32(i) * 2395);
            }
            return C + topla.ToString();
        }

    }

    public class BaseBoard
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
    }
}
