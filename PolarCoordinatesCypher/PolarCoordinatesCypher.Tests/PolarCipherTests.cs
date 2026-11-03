using PolarCoordinatesCypher.Core;

namespace PolarCoordinatesCypher.Tests;

[TestClass]
public class PolarCipherTests
{
    [TestMethod]
    public void Encrypt_Numeric_MatchesWorkedExample_Stars()
    {
        string cipherText = PolarCipher.Encrypt("STARS", CipherMode.Numeric);

        Assert.AreEqual("26699-28036-01422-25217-26699", cipherText);
    }

    [TestMethod]
    public void Encrypt_Numeric_MatchesWorkedExample_Vega()
    {
        string cipherText = PolarCipher.Encrypt("VEGA", CipherMode.Numeric);

        Assert.AreEqual("30828-07035-09827-01422", cipherText);
    }

    [TestMethod]
    public void Encrypt_Constellation_MatchesWorkedExample_Star()
    {
        string cipherText = PolarCipher.Encrypt("STAR", CipherMode.Constellation);

        Assert.AreEqual("S26699-T28036-A01422-R25217", cipherText);
    }

    [TestMethod]
    public void Encrypt_IsCaseInsensitive()
    {
        string lower = PolarCipher.Encrypt("stars", CipherMode.Numeric);
        string upper = PolarCipher.Encrypt("STARS", CipherMode.Numeric);

        Assert.AreEqual(upper, lower);
    }

    [TestMethod]
    public void Encrypt_MultipleWords_SeparatesWithWordSeparator()
    {
        string cipherText = PolarCipher.Encrypt("WE ARE READY", CipherMode.Numeric);

        string[] words = cipherText.Split(" / ");
        Assert.HasCount(3, words);
        Assert.AreEqual("32284-07035", words[0]);
    }

    [TestMethod]
    public void Encrypt_IgnoresNonLetterCharacters()
    {
        string withPunctuation = PolarCipher.Encrypt("S.T,A!R", CipherMode.Numeric);
        string withoutPunctuation = PolarCipher.Encrypt("STAR", CipherMode.Numeric);

        Assert.AreEqual(withoutPunctuation, withPunctuation);
    }

    [TestMethod]
    public void Encrypt_EmptyOrWhitespace_ReturnsEmptyString()
    {
        Assert.AreEqual(string.Empty, PolarCipher.Encrypt(string.Empty));
        Assert.AreEqual(string.Empty, PolarCipher.Encrypt("   "));
    }

    [TestMethod]
    public void Decrypt_Numeric_RoundTripsWorkedExample()
    {
        string plainText = PolarCipher.Decrypt("26699-28036-01422-25217-26699", CipherMode.Numeric);

        Assert.AreEqual("STARS", plainText);
    }

    [TestMethod]
    public void Decrypt_Constellation_RoundTripsWorkedExample()
    {
        string plainText = PolarCipher.Decrypt("S26699-T28036-A01422-R25217", CipherMode.Constellation);

        Assert.AreEqual("STAR", plainText);
    }

    [TestMethod]
    public void Decrypt_MultipleWords_RestoresSpaces()
    {
        string cipherText = PolarCipher.Encrypt("WE ARE READY", CipherMode.Numeric);

        string plainText = PolarCipher.Decrypt(cipherText, CipherMode.Numeric);

        Assert.AreEqual("WE ARE READY", plainText);
    }

    [TestMethod]
    public void Decrypt_Numeric_ThrowsFormatExceptionForUnknownCode()
    {
        Assert.ThrowsExactly<FormatException>(() => PolarCipher.Decrypt("99999", CipherMode.Numeric));
    }

    [TestMethod]
    public void Decrypt_Constellation_ThrowsFormatExceptionWhenLetterDoesNotMatchCode()
    {
        // 'A' prefix but 'S' (Sirius) code -> mismatch
        Assert.ThrowsExactly<FormatException>(() => PolarCipher.Decrypt("A26699", CipherMode.Constellation));
    }

    [TestMethod]
    [DataRow(CipherMode.Numeric)]
    [DataRow(CipherMode.Constellation)]
    public void EncryptThenDecrypt_RoundTripsForAllLetters(CipherMode mode)
    {
        const string plainText = "THEQUICKBROWNFOXJUMPSOVERTHELAZYDOG";

        string cipherText = PolarCipher.Encrypt(plainText, mode);
        string decrypted = PolarCipher.Decrypt(cipherText, mode);

        Assert.AreEqual(plainText, decrypted);
    }

    [TestMethod]
    public void ToPolarPoints_MatchesWorkedExample_Code()
    {
        IReadOnlyList<PolarPoint> points = PolarCipher.ToPolarPoints("CODE");

        Assert.HasCount(4, points);

        Assert.AreEqual('C', points[0].Letter);
        Assert.AreEqual(42, points[0].ThetaDegrees);
        Assert.AreEqual(11, points[0].Radius);

        Assert.AreEqual('O', points[1].Letter);
        Assert.AreEqual(210, points[1].ThetaDegrees);
        Assert.AreEqual(13, points[1].Radius);

        Assert.AreEqual('D', points[2].Letter);
        Assert.AreEqual(56, points[2].ThetaDegrees);
        Assert.AreEqual(83, points[2].Radius);

        Assert.AreEqual('E', points[3].Letter);
        Assert.AreEqual(70, points[3].ThetaDegrees);
        Assert.AreEqual(35, points[3].Radius);
    }

    [TestMethod]
    public void ToPolarPoints_SkipsNonLetterCharacters()
    {
        IReadOnlyList<PolarPoint> points = PolarCipher.ToPolarPoints("S.T A-R");

        Assert.HasCount(4, points);
        CollectionAssert.AreEqual(new[] { 'S', 'T', 'A', 'R' }, points.Select(p => p.Letter).ToArray());
    }
}
