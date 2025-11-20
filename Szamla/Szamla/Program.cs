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

            
            Console.Write("Eladó neve: ");
            string kibocsatoNev = Console.ReadLine();
            
            Console.WriteLine("Kérem adja meg a számla típusát(pl.: eszközszámla, forrásszámla, költségszámla, bevételszámla, ráfordítás számla): ");
            string tipus = Console.ReadLine();
            
            Console.Write($"Számla sorszáma: SZ{DateTime.Today.Year}/");
            string szamlaSzam = Console.ReadLine();

            Console.Write("Eladó adószám: ");
            string kibocsatoAdoszam = Console.ReadLine();

            Console.Write("Vevő neve: ");
            string vevoNev = Console.ReadLine();
            StreamWriter fajl = new StreamWriter($"SZ{DateTime.Today.Year}{szamlaSzam}.txt");

            Console.Write("Vevő adószám: ");
            string vevoAdoszam = Console.ReadLine();

            Console.Write("Teljesítés dátuma (ÉÉÉÉ.HH.NN): ");
            string teljesitesDatum = Console.ReadLine();

            string kidatum = $"{DateTime.Today.Year}.{DateTime.Today.Month}.{DateTime.Today.Day}";

            Console.Write("Fizetési határidő (ÉÉÉÉ.HH.NN): ");
            string fizetesiHatarido = Console.ReadLine();

            List<Tetel> tetelek = new List<Tetel>();
            string tetel = "";
            while (true)
            {
                Console.Write("Tétel megnevezése(ha megadta az összes tételt nyomjon entert üres sornál): ");
                tetel = Console.ReadLine();
                if (tetel == "")
                {
                    break;
                }

                Console.Write("Darabszám: ");
                int darabszam = int.Parse(Console.ReadLine());

                Console.Write("Egységár (nettó): ");
                decimal nettoAr = decimal.Parse(Console.ReadLine());

                Console.Write("Pénznem(EUR, USD, GBP, HUF): ");
                string penznemek = "";
                while (true)
                {
                    penznemek = Console.ReadLine();
                    if (penznemek == "EUR" || penznemek == "USD" || penznemek == "GBP" || penznemek == "HUF")
                    {
                        break;
                    }
                    else
                    {
                        Console.Write("Hibás pénznem! Kérem adja meg újra(EUR, USD, GBP, HUF): ");
                    }
                }
                

                Console.Write("ÁFA kulcs (%): ");
                int afaKulcs = int.Parse(Console.ReadLine());

                Tetel tetel1 = new Tetel(tetel, darabszam, nettoAr, afaKulcs, penznemek);
                tetelek.Add(tetel1);


            }
            



            string szamlaSzoveg =
$@"--- SZÁMLA ---
Számla sorszáma: {szamlaSzam}
Kibocsátó: {kibocsatoNev}, Adószám: {kibocsatoAdoszam}
Vevő: {vevoNev}, Adószám: {vevoAdoszam}
Teljesítés dátuma: {teljesitesDatum}
Kibocsátás dátuma: {kidatum}
Fizetési határidő: {fizetesiHatarido}";
            decimal osszespenz = 0;
            decimal ossznetto = 0;
            foreach (var item in tetelek)
            {
                szamlaSzoveg += $@"
Tétel: {item.Nev}, Mennyiség: {item.Darabszam}, Nettó egységár: {item.Egysegar} {item.Penznem}
Nettó összeg: {item.NettoOsszeg} {item.Penznem}
ÁFA ({item.AfaKulcs}%): {item.AfaOsszeg} {item.Penznem}
Bruttó összeg: {item.BruttoOsszeg} {item.Penznem}";
                ossznetto += item.Forint1;
                osszespenz += item.Forint;
            }
            szamlaSzoveg += $@"
Nettó végösszeg: {ossznetto.ToString("N2")} HUF
Bruttó végösszeg: {osszespenz.ToString("N2")} HUF";

            Console.WriteLine(szamlaSzoveg);
            fajl.WriteLine(szamlaSzoveg);
            fajl.Close();
            Console.WriteLine($"\nA számla adatai sikeresen elmentve a SZ{DateTime.Today.Year}{szamlaSzam}.txt fájlba!");
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
                        Console.Write("Egy(1) vagy több(2) fájlt szeretne beolvasni(0 = visszalépés)?");
                        int valasz = int.Parse(Console.ReadLine());
                        if (valasz == 0)
                        {
                            continue;
                        }
                        else if (valasz == 1)
                        {
                            Console.Write("Kérem adja meg a fájl elérési útvonalát: ");
                            string Utvonal = Console.ReadLine();
                            if (File.Exists(Utvonal))
                            {
                                string[] lines = File.ReadAllLines(Utvonal);
                                foreach (string line in lines)
                                {
                                    Console.WriteLine(line);  //Kiírja a fájl tartalmát a konzolra
                                }
                            }
                            else
                            {
                                Console.WriteLine("A fájl nem létezik!");
                            }
                        } else if(valasz == 2)
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
                                        // Console.WriteLine(f);
                                        if (File.Exists(f) && f.Contains(".txt"))
                                        {
                                            string[] lines = File.ReadAllLines(f);
                                            foreach (string line in lines)
                                            {
                                                Console.WriteLine(line);
                                            }
                                        }
                                        Console.WriteLine("");
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
