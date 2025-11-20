using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Szamla
{
    internal class Program
    {
        static void Ujszamla()
        {

            
            Console.Write("Kibocsátó neve: ");
            string kibocsatoNev = Console.ReadLine();
            
            
            Console.WriteLine("Kérem adja meg a számla típusát(pl.: eszközszámla, forrásszámla, költségszámla, bevételszámla, ráfordítás számla): ");
            string tipus = Console.ReadLine();
            
            Console.Write("Számla sorszáma: ");
            string szamlaSzam = Console.ReadLine();

            Console.Write("Kibocsátó adószám: ");
            string kibocsatoAdoszam = Console.ReadLine();

            Console.Write("Vevő neve: ");
            string vevoNev = Console.ReadLine();
            string vevoNev1 = vevoNev.Replace(" ", "_");
            StreamWriter fajl = new StreamWriter($"{vevoNev1}-szamlaja.txt");
            Console.Write("Teljesítés dátuma (ÉÉÉÉ.HH.NN): ");
            string teljesitesDatum = Console.ReadLine();

            string kidatum = $"{DateTime.Today.Year}.{DateTime.Today.Month}.{DateTime.Today.Day}";

            Console.Write("Fizetési határidő (ÉÉÉÉ.HH.NN): ");
            string fizetesiHatarido = Console.ReadLine();

            Console.Write("Tétel megnevezése: ");
            string tetel = Console.ReadLine();

            Console.Write("Mennyiség: ");
            int mennyiseg = int.Parse(Console.ReadLine());

            Console.Write("Egységár (nettó): ");
            decimal nettoAr = decimal.Parse(Console.ReadLine());

            Console.Write("ÁFA kulcs (%): ");
            int afaKulcs = int.Parse(Console.ReadLine());

            decimal nettoOsszeg = mennyiseg * nettoAr;
            decimal afaOsszeg = nettoOsszeg * afaKulcs / 100;
            decimal bruttoOsszeg = nettoOsszeg + afaOsszeg;

            string szamlaSzoveg =
$@"--- SZÁMLA ---
Számla sorszáma: {szamlaSzam}
Kibocsátó: {kibocsatoNev}, Adószám: {kibocsatoAdoszam}
Vevő: {vevoNev}
Teljesítés dátuma: {teljesitesDatum}
Kibocsátás dátuma: {kidatum}
Fizetési határidő: {fizetesiHatarido}
Tétel: {tetel}, Mennyiség: {mennyiseg}, Nettó egységár: {nettoAr} Ft
Nettó összeg: {nettoOsszeg} Ft
ÁFA ({afaKulcs}%): {afaOsszeg} Ft
Bruttó összeg: {bruttoOsszeg} Ft";

            Console.WriteLine(szamlaSzoveg);
            fajl.WriteLine(szamlaSzoveg);
            fajl.Close();
            Console.WriteLine($"\nA számla adatai sikeresen elmentve a {vevoNev1}-szamlaja.txt fájlba!");
        }
        static void Main(string[] args)
        {
           
            while (true)
            {
                Console.WriteLine("Új számla létrehozása(1) vagy számlák beolvasása(2) vagy kilépés(0)");
                try
                {
                    int feladat = int.Parse(Console.ReadLine());
                    if (feladat == 1)
                    {
                        Ujszamla();
                    }
                    else if (feladat == 2)
                    {
                        while (true)
                        {
                            Console.Write("Kérem adja meg a számlák elérési útvonalát(0 = visszalépés): ");
                            string Utvonal = Console.ReadLine();
                            if (Directory.Exists(Utvonal))
                            {
                                string[] files = Directory.GetFiles(Utvonal);
                                foreach (string f in files)
                                {
                                    Console.WriteLine(f);
                                }
                                break;
                            }
                            else
                            {
                                if (Utvonal == "0")
                                {
                                    break;
                                }
                                else { Console.WriteLine("A mappa nem létezik!"); }
                                    
                            }
                        }
                        
                    }
                    else if (feladat == 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Kérem a 1-est vagy 2-est vagy 0-át írjon be!");
                    }
                    
                }
                catch (Exception)
                {
                    
                        Console.WriteLine("Kérem a 1-est vagy 2-est vagy 0-át írjon be!");
                    
                    
                }
                
               
               
            }
            
            
        }
    }
}
