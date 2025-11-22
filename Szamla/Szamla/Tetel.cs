using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szamla
{
    internal class Tetel
    {
        public string Nev { get; set; }
        public int Darabszam { get; set; }
        public decimal Egysegar { get; set; }
        public decimal AfaKulcs { get; set; } 
        public string Penznem { get; set; }
        public decimal Forint { get; set; }
        public decimal Forint1 { get; set; }
        public decimal NettoOsszeg => Darabszam * Egysegar;
        public decimal AfaOsszeg => NettoOsszeg * (AfaKulcs / 100);
        public decimal BruttoOsszeg => NettoOsszeg + AfaOsszeg;
        public Tetel(string Neve, int Darabszama, decimal Egysegara, decimal Afa, string Penzneme) {
            Nev = Neve;
            Darabszam = Darabszama;
            Egysegar = Egysegara;
            AfaKulcs = Afa;
            Penznem = Penzneme;
            if (Penzneme != "HUF")
            {
                if (Penzneme == "USD")
                {
                    Forint1 = NettoOsszeg * 337;
                    Forint = BruttoOsszeg * 337;

                }else if (Penzneme == "EUR")
                {
                    Forint1 = NettoOsszeg * 382;
                    Forint = BruttoOsszeg * 382;
                }
                else if (Penzneme == "GBP")
                {
                    Forint1 = NettoOsszeg * 445;
                    Forint = BruttoOsszeg * 445;
                }

            }
            else
            {
                Forint1 = NettoOsszeg;
                Forint = BruttoOsszeg;
            }
        }
    }
}
