namespace CaesarKryptering
{
    public  class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Velkommen til Cæsar krypteringsprogram");
            Console.Write("For kryptering tast 1 \n" +
                "for dekryptering tast 2 \n" +
                "for brute force tast 3 \n"+ 
                "for file upload Encryption tast 4 \n"+
                "for file upload decryption tast 5 \n"+
                "for file upload decryption with prediction tast 6 \n"+
                "for Viginere Encryption tast 7 \n"+
                "for Viginere Decryption tast 8 \n");
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
            else if (cki.Key == ConsoleKey.D4 || cki.Key == ConsoleKey.NumPad4)
            {
                Console.WriteLine("Indtast stien til din tekstfil (f.eks. C:\\path\\to\\file.txt): ");
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    string input = File.ReadAllText(filePath);

                    Console.WriteLine("Indtast nøgle: ");
                    string inputShift = Console.ReadLine();
                    int shift = int.Parse(inputShift);

                    string encryptedText = EncryptText(input, shift);

                    string directory = Path.GetDirectoryName(filePath);     
                    string newFileName = Path.GetFileNameWithoutExtension(filePath) + "_encrypted.txt"; 
                    string newFilePath = Path.Combine(directory, newFileName); 

                    File.WriteAllText(newFilePath, encryptedText);

                    Console.WriteLine($"Krypteret tekst er gemt i filen: {newFilePath}");
                }
                else
                {
                    Console.WriteLine("Fil ikke fundet. Sørg for at stien er korrekt.");
                }
            }
            else if (cki.Key == ConsoleKey.D5 || cki.Key == ConsoleKey.NumPad5)
            {
                Console.WriteLine("Indtast stien til din tekstfil (f.eks. C:\\path\\to\\file.txt): ");
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    string input = File.ReadAllText(filePath);

                    string directory = Path.GetDirectoryName(filePath); 
                    string baseFileName = Path.GetFileNameWithoutExtension(filePath); 

                    for (int shift = 1; shift <= 25; shift++)
                    {
                        string decryptedText = DecryptText(input, shift);

                        string newFileName = $"{baseFileName}_decrypted_{shift}.txt"; 
                        string newFilePath = Path.Combine(directory, newFileName); 

                        File.WriteAllText(newFilePath, decryptedText);

                        Console.WriteLine($"Dekrypteret tekst med {shift} skift er gemt i filen: {newFilePath}");
                    }
                }
                else
                {
                    Console.WriteLine("Fil ikke fundet. Sørg for at stien er korrekt.");
                }
            }
            else if (cki.Key == ConsoleKey.D6 || cki.Key == ConsoleKey.NumPad6)
            {
                Console.WriteLine("Indtast stien til din tekstfil (f.eks. C:\\path\\to\\file.txt): ");
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    string input = File.ReadAllText(filePath);

                    // Expected English letter frequency (simplified version)
                    var englishLetterFrequency = new Dictionary<char, double>
                    {
                        { 'e', 12.70 }, { 't', 9.06 }, { 'a', 8.17 }, { 'o', 7.51 }, { 'i', 6.97 },
                        { 'n', 6.75 }, { 's', 6.33 }, { 'h', 6.09 }, { 'r', 5.99 }, { 'd', 4.25 },
                        { 'l', 4.03 }, { 'u', 2.76 }, { 'c', 2.23 }, { 'm', 2.02 }, { 'f', 1.97 },
                        { 'y', 1.97 }, { 'p', 1.93 }, { 'b', 1.49 }, { 'g', 1.49 }, { 'v', 0.98 },
                        { 'k', 0.77 }, { 'x', 0.15 }, { 'q', 0.10 }, { 'j', 0.10 }, { 'z', 0.07 }
                    };

                    // Normalize the frequencies so they sum to 1
                    double totalFrequency = englishLetterFrequency.Values.Sum();
                    var normalizedEnglishFrequency = englishLetterFrequency.ToDictionary(
                        pair => pair.Key, pair => pair.Value / totalFrequency);

                    string bestDecryptedText = "";
                    int bestShift = 0;
                    double bestMatch = double.MinValue;

                    // Loop through all 25 shifts
                    for (int shift = 1; shift <= 25; shift++)
                    {
                        string decryptedText = DecryptText(input, shift);

                        var letterFrequency = CalculateLetterFrequency(decryptedText);

                        // Normalize the frequency distribution
                        double totalLetters = letterFrequency.Values.Sum();
                        var normalizedLetterFrequency = letterFrequency.ToDictionary(
                            pair => pair.Key, pair => pair.Value / totalLetters);

                        // Calculate the similarity score based on Euclidean distance
                        double similarity = CalculateFrequencySimilarity(normalizedLetterFrequency, normalizedEnglishFrequency);

                        // If this shift produces the most "English-like" text, update the best match
                        if (similarity > bestMatch)
                        {
                            bestMatch = similarity;
                            bestShift = shift;
                            bestDecryptedText = decryptedText;
                        }
                    }

                    Console.WriteLine($"Den bedste dekryptering blev fundet med {bestShift} skift.");
                    Console.WriteLine("Dekrypteret tekst:");
                    Console.WriteLine(bestDecryptedText);

                    string directory = Path.GetDirectoryName(filePath);
                    string baseFileName = Path.GetFileNameWithoutExtension(filePath);
                    string bestDecryptedFilePath = Path.Combine(directory, $"{baseFileName}_best_decrypted_whith_shift_{bestShift}.txt");

                    File.WriteAllText(bestDecryptedFilePath, bestDecryptedText);
                    Console.WriteLine($"Den bedste dekrypterede tekst er gemt i filen: {bestDecryptedFilePath}");
                }
                else
                {
                    Console.WriteLine("Fil ikke fundet. Sørg for at stien er korrekt.");
                }
            }
            else if (cki.Key == ConsoleKey.D7 || cki.Key == ConsoleKey.NumPad7)
            {
                Console.WriteLine("Indtast din besked der skal krypteres: ");
                string input = Console.ReadLine();
                Console.WriteLine("Indtast nøgle: ");
                string inputKey = Console.ReadLine();

                string extendedKey = ExtendKey(input, inputKey);

                Console.WriteLine("Krypteret tekst:");
                Console.WriteLine($"{VigenereTransform(input,extendedKey,true)}");
                

            }
            else if (cki.Key == ConsoleKey.D8 || cki.Key == ConsoleKey.NumPad8)
            {
                Console.WriteLine("Indtast din besked der skal dekrypteres: ");
                string input = Console.ReadLine();
                Console.WriteLine("Indtast nøgle: ");
                string inputKey = Console.ReadLine();

                string extendedKey = ExtendKey(input, inputKey);

                Console.WriteLine("Dekrypteret tekst:");
                Console.WriteLine($"{VigenereTransform(input, extendedKey, false)}");
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

        public static char EncryptChar(char input, int shift)
        {
            if (char.IsLetter(input) && "ÆØÅ".IndexOf(input) == -1) // Exclude non-letter characters
            {
                char offset = 'A'; // Handle shift using 'A' as base for uppercase letters
                return (char)(((char.ToUpper(input) - 'A' + shift) % 26) + offset); // Shift based on uppercase value
            }
            return input;
        }

        public static string DecryptText(string input, int shift)
        {
            string result = "";

            foreach (char c in input.ToUpper()) //lav alle bogstaver om til stor
            {
                if (char.IsLetter(c) && "ÆØÅ".IndexOf(c) == -1) //Frasort ikke bostaver og bogstaver som æ, ø, å
                {
                    char offset = 'A'; //sæt start til wrap around
                                       // Ensure that the shift wraps around the alphabet correctly
                    result += (char)(((c - offset - shift + 26) % 26) + offset);
                }
                else
                {
                    result += c; // behold ikke-bostaver som de er
                }
            }

            return result;
        }

        public static char DecryptChar(char input, int shift)
        {
            if (char.IsLetter(input) && "ÆØÅ".IndexOf(input) == -1) // Exclude non-letter characters
            {
                char offset = 'A'; // Handle shift using 'A' as base for uppercase letters
                return (char)(((char.ToUpper(input) - 'A' - shift + 26) % 26) + offset); // Decrypt based on uppercase value
            }
            return input;
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

        // Method to calculate the frequency of each letter in the text
        static Dictionary<char, double> CalculateLetterFrequency(string text)
        {
            var letterFrequency = new Dictionary<char, double>();
            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (!letterFrequency.ContainsKey(c))
                    {
                        letterFrequency[c] = 0;
                    }
                    letterFrequency[c]++;
                }
            }
            return letterFrequency;
        }

        // Method to calculate the similarity between two frequency distributions
        static double CalculateFrequencySimilarity(Dictionary<char, double> freq1, Dictionary<char, double> freq2)
        {
            double similarity = 0;
            foreach (var letter in freq1.Keys.Concat(freq2.Keys).Distinct())
            {
                double freq1Value = freq1.ContainsKey(letter) ? freq1[letter] : 0;
                double freq2Value = freq2.ContainsKey(letter) ? freq2[letter] : 0;
                similarity += Math.Pow(freq1Value - freq2Value, 2);
            }
            return 1 / (1 + similarity);  // A smaller distance means higher similarity
        }

        //Udvid Vigenere key to match supplied tekst
        static string ExtendKey(string text, string key)
        {
            // Convert the text to uppercase to ensure both text and key are in sync
            text = text.ToUpper();

            char[] result = new char[text.Length];
            int keyIndex = 0;
            int keyLength = key.Length;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ') // Preserve spaces in the text
                {
                    result[i] = ' ';
                }
                else
                {
                    result[i] = key[keyIndex % keyLength]; // Extend the key to match the text length
                    keyIndex++;
                }
            }

            return new string(result);
        }

        static string VigenereTransform(string text, string extendedKey, bool encrypt)
        {
            // Convert the input text to uppercase first to ensure case-insensitivity
            text = text.ToUpper();

            string result = "";

            for (int i = 0; i < text.Length; i++)
            {
                char tekstChar = text[i];
                char keyChar = extendedKey[i];

                if (char.IsLetter(tekstChar) && encrypt)
                {
                    int shift = char.ToUpper(keyChar) - 'A'; // Convert key char to uppercase and calculate shift
                    result += EncryptChar(tekstChar, shift);
                }
                else if (char.IsLetter(tekstChar) && !encrypt)
                {
                    int shift = char.ToUpper(keyChar) - 'A'; // Convert key char to uppercase and calculate shift
                    result += DecryptChar(tekstChar, shift);
                }
                else
                {
                    result += tekstChar; // Keep spaces in the text as they are
                }
            }

            return result;
        }
    }
}
