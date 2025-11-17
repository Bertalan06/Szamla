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

            Console.WriteLine("Kérem adja meg kinek számlázza ki: ");
            string nev = Console.ReadLine();
            nev = nev.Replace(" ", "_");
            StreamWriter fajl = new StreamWriter($"{nev}-szamlaja.txt");
            Console.WriteLine("Kérem adja meg a számla típusát(pl.: eszközszámla, forrásszámla, költségszámla, bevételszámla, ráfordítás számla): ");
            string tipus = Console.ReadLine();
            DateTime kidatum =  DateTime.Today;


        }
        static void Main(string[] args)
        {
           
            while (true)
            {
                Console.WriteLine("Új számla létrehozásaó(1) vagy számlák beolvasása(2) vagy kilépés(0)");
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
