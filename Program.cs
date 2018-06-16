using System;

namespace BioInf
{
    class Program
    {
        static string[,] tableKimura = null;
        static double a, b, t, average, branchTime;
        static string[] tabLetters = null;
        static double[] tabTime = null;
        static double[] tabTimeStatic = null;
        static Random random;
        static string[,] tab = null;
        static int root = 1;

        static void Main(string[] args)
        {
            random = new Random();

            string seq = "ATGTTCTTGCAT";
            tab = new string[1000,13];

            double d = Math.Exp(1);

            for (int i = 0; i < seq.Length; i++)
            {
                tab[1,i] = seq[i].ToString();
            }
            Console.WriteLine("alfa");
            a = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("beta");
            b = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Czas ewolucji");
            t = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Srednia rozkładu wykładniczego");
            average = Convert.ToDouble(Console.ReadLine());

            average = 1.0/average;

            //  A C G T
            //A   b a b
            //C b   b a
            //G a b   b
            //T b a b
            //double[,] ;

            tableKimura = new string[,] { 
                { "s", "b", "a", "b" }, 
                { "b", "s", "b", "a" }, 
                { "a", "b", "s", "b" }, 
                { "b", "a", "b", "s" } };

            tabLetters = new string[]{ "A", "C", "G", "T"};
            tabTime = new double[1000];
            tabTimeStatic = new double[18] { 23.0, 56.0, 3.0, 24.0, 10.0, 15.0,
            23.0, 59.0, 9.0, 24.0, 19.0, 15.0,
            6.0, 50.0,13.0, 39.0, 11.0, 19.0,};

            Console.WriteLine();
            Console.WriteLine("1: losowy czas wezłów");
            Console.WriteLine("2: podany czas wezłów");
             
            switch (Console.ReadLine())
            {
                case "1":
                    FillSeqTable(true, 500);
                    WriteSeq(500);

                    break;
                case "2":
                    FillSeqTable(false, 9);
                    WriteSeq(14);

                    break;
                        
            }
                    
            Console.ReadKey();                        
        }

        //wypełnienie tablicy z sekwencjami
        public static void FillSeqTable(bool isRandom, int index)
        {
            for (int j = 2; j < index; j++)
            {
                if (tab[root, 0] == null)
                    continue;

                branchTime = isRandom ? RandTime() : tabTimeStatic[2 * root];
                tabTime[2 * root] = branchTime;
                if (ChectIfItIsEndOfBranch(2 * root, tabTime[2 * root]))
                {
                    //dla każdej literki w sekwencji
                    for (int i = 0; i < 12; i++)
                    {
                        tab[2 * root, i] = OutputLetter(tab[root, i]);
                    }
                }
                branchTime = isRandom ? RandTime() : tabTimeStatic[2 * root + 1];
                tabTime[2 * root + 1] = branchTime;
                if (ChectIfItIsEndOfBranch(2 * root + 1, tabTime[2 * root + 1]))
                {
                    //dla każdej literki w sekwencji
                    for (int i = 0; i < 12; i++)
                    {
                        tab[2 * root + 1, i] = OutputLetter(tab[root, i]);
                    }
                }
                root++;
            }
        }

        //wpisanie w konsoli sekwencji po ewolucji
        public static void WriteSeq(int index)
        {
            Console.WriteLine("Sekwencja DNA");
            double x = 0.0;
            int k = 0;
            bool isNull = false;

            for (int i = 1; i < index; i++)
            {                
                if (Convert.ToInt32(Math.Pow(2.0, x)).Equals(i))
                {
                    Console.WriteLine();
                    x++;
                    k = i;

                    if (tab[k, 0] == null)
                        isNull = true;
                    
                    do
                    {
                        if (isNull == true && tab[k, 0] == null)
                            break;

                        k++;

                    } while (!Convert.ToInt32(Math.Pow(2.0, x)).Equals(k));

                    if (isNull == true)
                        return;

                }
                int u =0;
                if (i!=0) {
                    if (i % 2 == 0)
                        u = i / 2;
                    else
                        u = (i - 1) / 2;
                }
                Console.Write("wezel: "+i+" rodzicem jest: "+u +" (" + tabTime[i] + ") ");
                for (int j = 0; j < 13; j++)
                {
                    Console.Write(tab[i, j]);
                }
                Console.WriteLine();

            }
        }

        //wylosowanie czasu
        public static double RandTime()
        {
            double branchTime = 0.0;

            branchTime = random.Next(0, 250);

            //branchTime = (-1.0 / average) * Math.Log(Math.Abs(1.0 - branchTime));
            branchTime = 1.0 - Math.Exp(-1.0 * average * branchTime);

            return branchTime;
        }

        //sprawdzanie czy możliwa jest dalsza ewolucja
        public static bool ChectIfItIsEndOfBranch(int i, double lastTime)
        {
            double sum = 0.0;
            double k=i;

            //sumowanie ścieżki
            do
            {
                sum += tabTime[Convert.ToInt32(k)];
                k = k % 2 == 0 ? k / 2 : k - 1.0 / 2;

            } while(k!=1);

            if (sum + lastTime > t)
            {
                return false;
            }

            return true;
        }

        //zwrocenie litery w sekwencji po ewolucji 
        public static string OutputLetter(string letter)
        {
            double[] helpTab = new double[4];
            //0-A
            //1-C
            //2-G
            //3-T
            int helpOriginalLetter = 0;
            string returnValue = "";

            //obliczenie prawdopodobieństw
            double Pa = Transitions(branchTime, a, b);
            double Pb = Transversions(branchTime, a, b);
            double Ps = TheSameLetters(branchTime, a, b);
                        
            switch (letter)
            {
                case "A":
                    helpOriginalLetter = 0;
                    break;
                case "C":
                    helpOriginalLetter = 1;
                    break;
                case "G":
                    helpOriginalLetter = 2;
                    break;
                case "T":
                    helpOriginalLetter = 3;
                    break;
            }


            //wpisanie do tablicy prawdopodobieństw zgodnych z modelem kimura
            for (int i = 0; i < 4; i++)
            {
                if(tableKimura[i, helpOriginalLetter] == "a")
                {
                    helpTab[i] = Pa;
                }
                else if (tableKimura[i, helpOriginalLetter] == "b")
                {
                    helpTab[i] = Pb;
                }
                else
                {
                    helpTab[i] = Ps;
                }
            }

            //wylosowanie zmiennej i dopasowanie jej do odpowiedniego miejsca w wykresie
            double randValue = random.NextDouble();

            if (randValue <= helpTab[0])
            {
                returnValue = "A";
            }
            else if (randValue <= helpTab[0]+ helpTab[1])
            {
                returnValue = "C";
            }
            else if (randValue <= helpTab[0] + helpTab[1]+helpTab[2])
            {
                returnValue = "G";
            }
            else
            {
                returnValue = "T";
            }

            return returnValue;
        }

        //b
        //obliczenie prawdopodobieństwa b
        public static double Transitions(double t, double a, double b)
        {
            double p = 1.0 / 4.0 + 1.0 / 4.0 * Math.Exp(-4.0 * b * t) - 1.0 / 2.0 * Math.Exp(-2.0 * (a + b) * t);

            return p;
        }

        //a
        //obliczenie prawdopodobieństwa a
        public static double Transversions(double t, double a, double b)
        {
            double p = 1.0 / 4.0 - 1.0 / 4.0 * Math.Exp(-4.0 * b * t);

            return p;
        }

        //1-a-2*b
        //obliczenie prawdopodobieństwa zostania przy tej samej literze
        public static double TheSameLetters(double t, double a, double b)
        {
            double p = 1.0 / 4.0 + 1.0 / 4.0 * Math.Exp(-4.0 * b * t) + 1.0 / 2.0 * Math.Exp(-2.0 * (a + b) * t);

            return p;
        }
    }
}
