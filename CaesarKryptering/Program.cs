namespace CaesarKryptering
{
    public  class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Velkommen til Cæsar krypteringsprogram");
            Console.Write("For kryptering tast 1 \n" +
                "for dekryptering tast 2 \n" +
                "for brute force tast 3 ");
            ConsoleKeyInfo cki = Console.ReadKey();
            Console.WriteLine("");
            Console.WriteLine("----------------------------------------------");

            if (cki.Key == ConsoleKey.D1 || cki.Key == ConsoleKey.NumPad1)
            {
                Console.WriteLine("Indtast din besked der skal krypteres: ");
                string input = Console.ReadLine();
                Console.WriteLine("Indtast nøgle: ");
                string inputShift = Console.ReadLine();
                int shift = int.Parse(inputShift);
                
                string result = EncryptText(input, shift);
                Console.WriteLine($"Krypteret text med {shift} skift: {result}");

            }
            else if (cki.Key == ConsoleKey.D2 || cki.Key == ConsoleKey.NumPad2)
            {
                Console.WriteLine("Indtast din besked der skal dekrypteres: ");
                string input = Console.ReadLine();
                Console.WriteLine("Indtast nøgle: ");
                string inputShift = Console.ReadLine();
                int shift = int.Parse(inputShift);

                string result = DecryptText(input, shift);
                Console.WriteLine($"Dekrypteret text med {shift} skift: {result}");

            }
            else if (cki.Key == ConsoleKey.D3 || cki.Key == ConsoleKey.NumPad3)
            {
                Console.WriteLine("Indtast din besked der skal dekrypteres: ");
                string input = Console.ReadLine();

                Console.WriteLine("De mulige svar er: ");

                int i = 1;
                foreach (var item in BruteForceDecryptText(input))
                {
                    Console.WriteLine($"Shift {i}: {item}");
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Ugyldig input!");
            }

        }

        public static string EncryptText(string input, int shift)
        {
            string result = "";

            foreach (char c in input.ToUpper()) //lav alle bogstaver om til stor
            {
                if (char.IsLetter(c) && "ÆØÅ".IndexOf(c) == -1) //Frasort ikke bostaver og bogstaver som æ, ø, å
                {
                    char offset = 'A';      //sæt start til wrap around
                    result += (char)(((c - offset + shift) % 26) + offset); 
                }
                else
                {
                    result += c; // behold ikke-bostaver som de er
                }
            }

            return result;

        }

        public static string DecryptText(string input, int shift)
        {
            string result = "";

            foreach (char c in input.ToUpper()) //lav alle bogstaver om til stor
            {
                if (char.IsLetter(c) && "ÆØÅ".IndexOf(c) == -1) //Frasort ikke bostaver og bogstaver som æ, ø, å
                {
                    char offset = 'A';      //sæt start til wrap around
                    result += (char)(((c - offset - shift) % 26) + offset);
                }
                else
                {
                    result += c; // behold ikke-bostaver som de er
                }
            }

            return result;

        }

        public static List<string> BruteForceDecryptText(string input)
        {
            List<string> results = new List<string>();

            for (int shift = 1; shift <= 25; shift++) // Gå igennem alle shifts fra 1 to 25, 26 vil være den samme original tekst igen
            {
                string result = "";

                foreach (char c in input.ToUpper()) //lav alle bogstaver om til stor
                {
                    if (char.IsLetter(c) && "ÆØÅ".IndexOf(c) == -1) //Frasort ikke bostaver og bogstaver som æ, ø, å
                    {
                        char offset = 'A';  //sæt start til wrap around
                        result += (char)(((c - offset - shift + 26) % 26) + offset); // Decrypt using the shift
                    }
                    else
                    {
                        result += c; // behold ikke-bostaver som de er
                    }
                }

                results.Add(result); 
            }

            return results;
        }
    }
}
