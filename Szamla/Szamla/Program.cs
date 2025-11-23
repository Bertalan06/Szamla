using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Szamla
{
    internal class Program
    {
        static void Ujszamla()
        {
            bool datumValidalas(string teljesites, string kelte, string hatarido)
            {
                const string format = "yyyy-MM-dd";


                if (!DateTime.TryParseExact(kelte, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime kelteDate))
                {
                    return false;
                }

                if (!DateTime.TryParseExact(hatarido, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime hataridoDate))
                {
                    return false;
                }

                if (teljesites != "Teljesítetlen számla")
                {
                    // Dátumok formátum + létezés ellenőrzése
                    if (!DateTime.TryParseExact(teljesites, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime teljesitesiDate))
                    {
                        return false;
                    }
                    // Teljesítési dátum nem lehet későbbi, mint a számla kelte
                    if (teljesitesiDate > kelteDate)
                    {
                        return false;
                    }

                }
                // Számla kelte nem lehet későbbi, mint a fizetési határidő
                if (kelteDate > hataridoDate)
                {
                    return false;
                }

                return true;
            }

            //számla sorszám 
            string szamlaSzam = "";
            while (true)
            {
                Console.Write($"Számla sorszáma: SZ{DateTime.Today.Year}/");
                szamlaSzam = Console.ReadLine();
                if (File.Exists($"SZ{DateTime.Today.Year}{szamlaSzam}.txt"))
                {
                    Console.WriteLine("Ez a számla sorszám már létezik! Kérem adjon meg egy újat!");
                }
                else
                {
                    break;
                }
            }
            StreamWriter fajl = new StreamWriter($"SZ{DateTime.Today.Year}{szamlaSzam}.txt"); //fájl létrehozása

            // számla típus bekerese
            Console.WriteLine("Kérem adja meg a számla típusát(pl.: eszközszámla, forrásszámla, költségszámla, bevételszámla, ráfordítás számla): ");
            string tipus = Console.ReadLine();

            //elado neve bekerese
            string kibocsatoNev = "";
            string nevRegex = @"^[A-ZÁÉÍÓÖŐÚÜŰ][a-záéíóöőúüű]+( [A-ZÁÉÍÓÖŐÚÜŰ][a-záéíóöőúüű]+)+$";
            while (true)
            {
                Console.Write("Eladó neve: ");
                kibocsatoNev = Console.ReadLine();
                if (Regex.IsMatch(kibocsatoNev, nevRegex))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Hibás név!");
                }
            }

            //elado adoszam bekerese
            Console.Write("Eladó adószám: ");
            string kibocsatoAdoszam = Console.ReadLine();

            //vevo neve bekerese
            string vevoNev = "";
            while (true)
            {
                Console.Write("Vevő neve: ");
                vevoNev = Console.ReadLine();
                if (Regex.IsMatch(vevoNev, nevRegex))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Hibás név!");
                }
            }

            //vevo adoszam bekerese
            Console.Write("Vevő adószám: ");
            string vevoAdoszam = Console.ReadLine();

            //dátumok bekerese és validálása
            string szamlaKelte = "";
            string teljesitesDatum = "";
            string fizetesiHatarido = "";
            while (true)
            {
                Console.Write("Keltezés dátuma (ÉÉÉÉ-HH-NN) (enter = mai nap): ");
                szamlaKelte = Console.ReadLine();
                if (szamlaKelte == "")
                {
                    szamlaKelte = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}";
                }

                Console.Write("Teljesítés dátuma (ÉÉÉÉ-HH-NN) (enter = mai nap; 0 = teljesítetlen számla): ");
                teljesitesDatum = Console.ReadLine();
                if (teljesitesDatum == "")
                {
                    teljesitesDatum = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}";
                }
                else if (teljesitesDatum == "0")
                {
                    teljesitesDatum = "Teljesítetlen számla";
                }

                Console.Write("Fizetési határidő (ÉÉÉÉ-HH-NN) (enter = mai nap): ");
                fizetesiHatarido = Console.ReadLine();
                if (fizetesiHatarido == "")
                {
                    fizetesiHatarido = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}";
                }
                if (datumValidalas(teljesitesDatum, szamlaKelte, fizetesiHatarido))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Hibás dátum(ok)!\nTeljesítési dátum nem lehet későbbi, mint a számla kelte\nSzámla kelte nem lehet későbbi, mint a fizetési határidő");
                    continue;
                }
            }

            //tételek bekérése
            List<Tetel> tetelek = new List<Tetel>();
            string tetel = "";
            while (true)
            {
                while (true)
                {
                    Console.Write("Tétel megnevezése(ha megadta az összes tételt nyomjon entert üres sornál): ");
                    tetel = Console.ReadLine();

                    if (tetel == "")
                    {
                        if (tetelek.Count() == 0)
                        {
                            Console.WriteLine("Legalább egy tételt meg kell adni!");
                            continue;
                        }
                        else { break; }


                    }
                    else
                    {
                        break;
                    }
                }
                if (tetel == "") { break; }
                int darabszam;
                while (true)
                {
                    Console.Write("Darabszám: ");
                    try
                    {
                        darabszam = int.Parse(Console.ReadLine());
                        if (darabszam > 0)
                        {
                            break;
                        }
                        Console.WriteLine("Nem lehet 0 vagy 0-nál kisebb a darabszám!");
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Kérjük számot írjon be!");
                    }


                }
                decimal nettoAr;
                while (true)
                {
                    Console.Write("Egységár (nettó): ");
                    try
                    {
                        nettoAr = decimal.Parse(Console.ReadLine());
                        if (nettoAr > 0)
                        {
                            break;
                        }
                        Console.WriteLine("Nem lehet 0 vagy 0-nál kisebb a nettó egységár!");
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Kérjük számot írjon be!");
                    }


                }




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
                int afaKulcs;
                while (true)
                {
                    Console.Write("ÁFA kulcs (%): ");
                    try
                    {
                        afaKulcs = int.Parse(Console.ReadLine());
                        if (afaKulcs >= 0)
                        {
                            break;
                        }
                        Console.WriteLine("Nem lehet 0-nál kisebb az áfakulcs!");
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Kérjük számot írjon be!");
                    }


                }


                Tetel tetel1 = new Tetel(tetel, darabszam, nettoAr, afaKulcs, penznemek);
                tetelek.Add(tetel1);




            }



            //számla szöveg összeállítása és fájlba, konzolra írása
            string szamlaSzoveg =
$@"--- SZ{DateTime.Today.Year}{szamlaSzam} ---
Számla sorszáma: {szamlaSzam}
Kibocsátó: {kibocsatoNev}, Adószám: {kibocsatoAdoszam}
Vevő: {vevoNev}, Adószám: {vevoAdoszam}

Teljesítés dátuma: {teljesitesDatum}
Kibocsátás dátuma: {szamlaKelte}
Fizetési határidő: {fizetesiHatarido}
";
            decimal osszespenz = 0;
            decimal ossznetto = 0;
            foreach (var item in tetelek)
            {
                szamlaSzoveg += $@"
Tétel:      {item.Nev}
            Mennyiség: {item.Darabszam}, Nettó egységár: {item.Egysegar} {item.Penznem}
            Nettó összeg: {item.NettoOsszeg} {item.Penznem}
            ÁFA ({item.AfaKulcs}%): {item.AfaOsszeg} {item.Penznem}
            Bruttó összeg: {item.BruttoOsszeg} {item.Penznem}
";
                ossznetto += item.Forint1;
                osszespenz += item.Forint;
            }
            szamlaSzoveg += $@"
Nettó végösszeg (HUF): {ossznetto.ToString("N0")} HUF
Bruttó végösszeg (HUF): {osszespenz.ToString("N0")} HUF";

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
                        bool ki = false;
                        while (true)
                        {
                            if (ki)
                            {
                                break;
                            }
                            Console.Write("Egy(1) vagy több(2) fájlt szeretne beolvasni(0 = visszalépés)? ");
                            int valasz = int.Parse(Console.ReadLine());
                            if (valasz == 0)
                            {
                                break;
                            }
                            else if (valasz == 1)
                            {
                                while (true)
                                {
                                    if (ki)
                                    {
                                        break;
                                    }
                                    Console.Write("Kérem adja meg a fájl elérési útvonalát (0 = visszalépés): ");
                                    string Utvonal = Console.ReadLine();
                                    if (File.Exists(Utvonal) && Utvonal.EndsWith(".txt"))
                                    {
                                        string[] lines = File.ReadAllLines(Utvonal);
                                        foreach (string line in lines)
                                        {
                                            Console.WriteLine(line);  //Kiírja a fájl tartalmát a konzolra
                                        }
                                        Console.WriteLine();
                                        ki = true;
                                        break;
                                    }
                                    else
                                    {
                                        if (Utvonal == "0")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("A fájl nem létezik vagy nem .txt típusú a fájl!");
                                            continue;
                                        }
                                    }
                                }
                            }
                            else if (valasz == 2)
                            {
                                while (true)
                                {
                                    Console.Write("Kérem adja meg a számlák elérési útvonalát(0 = visszalépés): ");
                                    string Utvonal = Console.ReadLine();



                                    if (Directory.Exists(Utvonal))
                                    {
                                        bool exitOptions = false;
                                        while (!exitOptions)
                                        {
                                            Console.Write("Fájlok tartalmának kiírása(1), Összegeik(2), Vevők nevei(3), Kibocsátok nevei(4), Teljesítetlen(5), Teljesített(6), Visszalépés(0): ");
                                            if (!int.TryParse(Console.ReadLine(), out int opcio))
                                            {
                                                Console.WriteLine("Érvénytelen választás!");
                                                continue;
                                            }

                                            switch (opcio)
                                            {
                                                case 0:
                                                    // lépj vissza a mappa választáshoz / felső menübe
                                                    ki = false;
                                                    exitOptions = true;
                                                    break;


                                                case 1:
                                                    {
                                                        string[] files = Directory.GetFiles(Utvonal, "*.txt");
                                                        foreach (string f in files)
                                                        {
                                                            if (File.Exists(f))
                                                            {
                                                                string[] lines = File.ReadAllLines(f);
                                                                foreach (string line in lines)
                                                                {
                                                                    Console.WriteLine(line);
                                                                }
                                                                Console.WriteLine();
                                                            }
                                                        }
                                                        ki = true;
                                                    }
                                                    break;

                                                case 2:
                                                    {
                                                        try
                                                        {
                                                            decimal osszeg = 0m;
                                                            string[] files1 = Directory.GetFiles(Utvonal, "*.txt");
                                                            foreach (string f in files1)
                                                            {
                                                                string[] lines = File.ReadAllLines(f);
                                                                foreach (string line in lines)
                                                                {
                                                                    if (line.StartsWith("Bruttó végösszeg"))
                                                                    {
                                                                        // kivonjuk a számot (tördelő szóközöket is kezelve)
                                                                        string amtRaw = Regex.Match(line, @"[\d\s\u00A0\.,]+").Value.Trim(); // \u00A0 a nem tördelő szóköz
                                                                        if (decimal.TryParse(amtRaw, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal amount))
                                                                        {
                                                                            osszeg += amount;
                                                                        }
                                                                        else if (decimal.TryParse(amtRaw, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out amount))
                                                                        {
                                                                            osszeg += amount;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            Console.WriteLine($"Összes bruttó végösszeg: {osszeg.ToString("N0")} HUF");
                                                            ki = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine("2-es nem jó: " + ex.Message);
                                                        }
                                                    }
                                                    break;
                                                case 3:
                                                    {
                                                        try
                                                        {
                                                            List<string> vevok = new List<string>();
                                                            string[] files2 = Directory.GetFiles(Utvonal, "*.txt");
                                                            foreach (string f in files2)
                                                            {
                                                                string[] lines = File.ReadAllLines(f);
                                                                foreach (string line in lines)
                                                                {
                                                                    if (line.StartsWith("Vevő: "))
                                                                    {
                                                                        // Vevő: Név, Adószám: ...
                                                                        var m = Regex.Match(line, @"^Vevő:\s*(.+?),");
                                                                        if (m.Success) { 
                                                                            vevok.Add(m.Groups[1].Value.Trim()); //Group 1 a ()-ben lévő rész
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            Console.WriteLine($"Vevők: {string.Join(", ", vevok)}.");
                                                            ki = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine("3-as nem jó: " + ex.Message);
                                                        }
                                                    }
                                                    break;
                                                case 4:
                                                    {
                                                        try
                                                        {
                                                            List<string> eladok = new List<string>();
                                                            string[] files3 = Directory.GetFiles(Utvonal, "*.txt");
                                                            foreach (string f in files3)
                                                            {
                                                                string[] lines = File.ReadAllLines(f);
                                                                foreach (string line in lines)
                                                                {
                                                                    if (line.StartsWith("Kibocsátó: "))
                                                                    {
                                                                        var m = Regex.Match(line, @"^Kibocsátó:\s*(.+?),");
                                                                        if (m.Success) { 
                                                                            eladok.Add(m.Groups[1].Value.Trim());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            Console.WriteLine($"Eladók: {string.Join(", ", eladok)}.");
                                                            ki = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine("4-es nem jó: " + ex.Message);
                                                        }
                                                    }
                                                    break;
                                                case 5:
                                                    {
                                                        try
                                                        {
                                                            int teljesitetlenDb = 0;
                                                            List<string> teljesitetlenek = new List<string>();
                                                            string[] files4 = Directory.GetFiles(Utvonal, "*.txt");
                                                            foreach (string f in files4)
                                                            {
                                                                string[] lines = File.ReadAllLines(f);
                                                                bool tel = lines.Any(l => l.Contains("Teljesítetlen számla"));
                                                                if (tel)
                                                                {
                                                                    teljesitetlenDb++;
                                                                    string header = lines.FirstOrDefault(l => l.StartsWith("---"));
                                                                    if (!string.IsNullOrEmpty(header))
                                                                    {
                                                                        var m = Regex.Match(header, @"---\s*(.+?)\s*---");
                                                                        if (m.Success) teljesitetlenek.Add(m.Groups[1].Value.Trim());
                                                                        else teljesitetlenek.Add(header.Substring(4).Trim());
                                                                    }
                                                                }
                                                            }
                                                            Console.WriteLine($"Teljesítetlen számlák: {string.Join(", ", teljesitetlenek)}.");
                                                            Console.WriteLine($"Összesen: {teljesitetlenDb} db.");
                                                            ki = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine("5-ös nem jó: " + ex.Message);
                                                        }
                                                    }
                                                    break;
                                                case 6:
                                                    {
                                                        try
                                                        {
                                                            int teljesitettDb = 0;
                                                            List<string> teljesitettek = new List<string>();
                                                            string[] files5 = Directory.GetFiles(Utvonal, "*.txt");
                                                            foreach (string f in files5)
                                                            {
                                                                string[] lines = File.ReadAllLines(f);
                                                                bool tel = lines.Any(l => l.Contains("Teljesítetlen számla"));
                                                                if (!tel)
                                                                {
                                                                    teljesitettDb++;
                                                                    string header = lines.FirstOrDefault(l => l.StartsWith("---"));
                                                                    if (!string.IsNullOrEmpty(header))
                                                                    {
                                                                        var m = Regex.Match(header, @"---\s*(.+?)\s*---");
                                                                        if (m.Success) teljesitettek.Add(m.Groups[1].Value.Trim());
                                                                        else teljesitettek.Add(header.Substring(4).Trim());
                                                                    }
                                                                }
                                                            }
                                                            Console.WriteLine($"Teljesített számlák: {string.Join(", ", teljesitettek)}.");
                                                            Console.WriteLine($"Összesen: {teljesitettDb} db.");
                                                            ki = true;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine("6-os nem jó: " + ex.Message);
                                                        }
                                                    }
                                                    break;

                                                default:
                                                    Console.WriteLine("Kérem az 1-től 6-ig terjedő számok egyikét adja meg!");
                                                    break;


                                            }


                                        }


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
