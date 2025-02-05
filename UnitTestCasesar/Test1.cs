using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Runtime.CompilerServices;

namespace UnitTestCasesar
{
    [TestClass]
    public sealed class Test1
    {
        private const string Expected1 = "BCDE";
        private const string Expected2 = "ABCD";
        private const string Expected3 = "MPZR LY NVKA MVY KPN!";

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
            string input = "Fisk er godt for dig!";

            // Act
            List<string> result = CaesarKryptering.Program.BruteForceDecryptText(input);

            // Assert
            Assert.AreNotEqual(Expected1, result[6], "Decryption failed");
        }
    }
}
