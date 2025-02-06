using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Runtime.CompilerServices;

namespace UnitTestCasesar
{
    [TestClass]
    public sealed class Test1
    {
        private const string Expected1 = "BCDE";
        private const string Expected2 = "ABCD";
        private const string Expected3 = "FISK ER GODT FOR DIG!";

        [TestMethod]
        public void TestEncryptText()
        {
            // Arrange
            string input = "ABCD";
            int shift = 1;

            // Act
            string result = CaesarKryptering.Program.EncryptText(input, shift);

            // Assert
            Assert.AreEqual(Expected1, result, "Encryption failed");
        }

        [TestMethod]
        public void TestDecryptText()
        {
            // Arrange
            string input = "BCDE";
            int shift = 1;

            // Act
            string result = CaesarKryptering.Program.DecryptText(input, shift);

            // Assert
            Assert.AreEqual(Expected2, result, "Decryption failed");
        }

        [TestMethod]
        public void TestEncryptText2()
        {
            // Arrange
            string input = "ABCD";
            int shift = 1;

            // Act
            string result = CaesarKryptering.Program.EncryptText(input, shift);

            // Assert
            Assert.AreNotEqual(Expected2, result, "Decryption failed");
        }

        [TestMethod]
        public void TestDecryptText2()
        {
            // Arrange
            string input = "ABCD";
            int shift = 1;

            // Act
            string result = CaesarKryptering.Program.DecryptText(input, shift);

            // Assert
            Assert.AreNotEqual(Expected1, result, "Decryption failed");
        }

        [TestMethod]
        public void BruteForce()
        {
            // Arrange
            string input = "MPZR LY NVKA MVY KPN!";

            // Act
            List<string> result = CaesarKryptering.Program.BruteForceDecryptText(input);

            // Assert
            Assert.AreEqual(Expected3, result[6], "Decryption failed");
        }

        private const char Expected4 = 'D';
                [TestMethod]
        public void TestEncryptChar1()
        {
            // Arrange
            char input = 'A';
            int shift = 3;

            // Act
            char result = CaesarKryptering.Program.EncryptChar(input, shift);

            // Assert
            Assert.AreEqual(Expected4, result, "Encryption failed");
        }

        private const char Expected5 = 'A';
        [TestMethod]
        public void TestDecryptChar1()
        {
            // Arrange
            char input = 'D';
            int shift = 3;

            // Act
            char result = CaesarKryptering.Program.DecryptChar(input, shift);

            // Assert
            Assert.AreEqual(Expected5, result, "Decryption failed");
        }

        private const string Expected6 = "FISKF ISKFI";
        [TestMethod]
        public void TestExtendKey1()
        {
            // Arrange
            string input = "SKOLE SKOLE";
            string key = "FISK";

            // Act
            string result = CaesarKryptering.Program.ExtendKey(input, key);

            // Assert
            Assert.AreEqual(Expected6, result, "Extension failed");
        }

        private const string Expected7 = "MMBR";
        [TestMethod]
        public void TestVigenereEncrypt1()
        {
            // Arrange
            string input = "Fisk";
            string key = "hej";

            string extendedKey = CaesarKryptering.Program.ExtendKey(input, key);

            // Act
            string result = CaesarKryptering.Program.VigenereTransform(input, extendedKey, true);

            // Assert
            Assert.AreEqual(Expected7, result, "Encryption failed");
        }

        private const string Expected8 = "FISK";
        [TestMethod]
        public void TestVigenereDecrypt1()
        {
            // Arrange
            string input = "MMBR";
            string key = "hej";

            string extendedKey = CaesarKryptering.Program.ExtendKey(input, key);

            // Act
            string result = CaesarKryptering.Program.VigenereTransform(input, extendedKey, false);

            // Assert
            Assert.AreEqual(Expected8, result, "Decryption failed");
        }
    }
}
