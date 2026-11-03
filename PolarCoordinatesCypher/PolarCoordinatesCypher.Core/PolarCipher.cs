using System.Text;

namespace PolarCoordinatesCypher.Core;

/// <summary>
///  Encrypts and decrypts messages using the Real-Star Polaris Cipher, and
///  projects plaintext onto polar coordinates for graphing as a "star map".
/// </summary>
public static class PolarCipher
{
    /// <summary>Separator placed between each letter's code within a word.</summary>
    public const string LetterSeparator = "-";

    /// <summary>Separator placed between words.</summary>
    public const string WordSeparator = " / ";

    /// <summary>
    ///  Encrypts <paramref name="plainText"/> into the Real-Star Polaris Cipher.
    ///  Non-letter characters (other than whitespace, which separates words) are ignored.
    /// </summary>
    /// <param name="plainText">The message to encrypt.</param>
    /// <param name="mode">The output format to use.</param>
    /// <returns>The ciphertext string.</returns>
    public static string Encrypt(string plainText, CipherMode mode = CipherMode.Numeric)
    {
        ArgumentNullException.ThrowIfNull(plainText);

        string[] words = plainText.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        List<string> encodedWords = [];

        foreach (string word in words)
        {
            List<string> letterCodes = [];

            foreach (char c in word)
            {
                if (!char.IsLetter(c))
                {
                    continue;
                }

                StarKeyEntry entry = StarCatalog.GetByLetter(c);
                letterCodes.Add(mode == CipherMode.Constellation ? entry.ConstellationCode : entry.Code);
            }

            if (letterCodes.Count > 0)
            {
                encodedWords.Add(string.Join(LetterSeparator, letterCodes));
            }
        }

        return string.Join(WordSeparator, encodedWords);
    }

    /// <summary>
    ///  Decrypts a Real-Star Polaris Cipher ciphertext produced by <see cref="Encrypt"/>.
    /// </summary>
    /// <param name="cipherText">The ciphertext to decrypt.</param>
    /// <param name="mode">The format the ciphertext was encoded with.</param>
    /// <returns>The recovered plaintext, in upper case with single spaces between words.</returns>
    /// <exception cref="FormatException">Thrown when a code token cannot be decoded.</exception>
    public static string Decrypt(string cipherText, CipherMode mode = CipherMode.Numeric)
    {
        ArgumentNullException.ThrowIfNull(cipherText);

        string[] words = cipherText.Split(
            [WordSeparator, "/"],
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        List<string> plainWords = [];

        foreach (string word in words)
        {
            string[] tokens = word.Split(
                LetterSeparator,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            StringBuilder plainWord = new();

            foreach (string token in tokens)
            {
                plainWord.Append(DecodeToken(token, mode));
            }

            if (plainWord.Length > 0)
            {
                plainWords.Add(plainWord.ToString());
            }
        }

        return string.Join(' ', plainWords);
    }

    private static char DecodeToken(string token, CipherMode mode)
    {
        if (mode == CipherMode.Constellation)
        {
            if (token.Length != 6 || !char.IsLetter(token[0]))
            {
                throw new FormatException($"Invalid constellation code token: '{token}'.");
            }

            char letter = char.ToUpperInvariant(token[0]);
            string code = token[1..];
            StarKeyEntry entry = StarCatalog.GetByLetter(letter);
            if (entry.Code != code)
            {
                throw new FormatException($"Constellation code token '{token}' does not match the star key for '{letter}'.");
            }

            return letter;
        }

        if (!StarCatalog.TryGetByCode(token, out StarKeyEntry? numericEntry) || numericEntry is null)
        {
            throw new FormatException($"Unknown cipher code: '{token}'.");
        }

        return numericEntry.Letter;
    }

    /// <summary>
    ///  Projects the letters of <paramref name="plainText"/> onto their polar coordinates,
    ///  in order, for use when drawing the message as a star map. Non-letter characters are skipped.
    /// </summary>
    public static IReadOnlyList<PolarPoint> ToPolarPoints(string plainText)
    {
        ArgumentNullException.ThrowIfNull(plainText);

        List<PolarPoint> points = [];

        foreach (char c in plainText)
        {
            if (!char.IsLetter(c))
            {
                continue;
            }

            StarKeyEntry entry = StarCatalog.GetByLetter(c);
            points.Add(new PolarPoint(char.ToUpperInvariant(c), entry.StarName, entry.ThetaDegrees, entry.Radius));
        }

        return points;
    }
}
