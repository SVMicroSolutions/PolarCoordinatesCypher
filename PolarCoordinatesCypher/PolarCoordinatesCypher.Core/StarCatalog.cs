using System.Collections.ObjectModel;

namespace PolarCoordinatesCypher.Core;

/// <summary>
///  Provides the full A-Z Real-Star Polaris Cipher key, mapping each letter of the
///  alphabet to a real star name and a fixed polar coordinate (theta, radius) used
///  to form the cipher's 5-digit numeric code.
/// </summary>
public static class StarCatalog
{
    /// <summary>
    ///  Gets the ordered A-Z catalog of star key entries.
    /// </summary>
    public static ReadOnlyCollection<StarKeyEntry> Entries { get; } = new ReadOnlyCollection<StarKeyEntry>(
    [
        new StarKeyEntry('A', "Acrux", 014, 22),
        new StarKeyEntry('B', "Bellatrix", 028, 47),
        new StarKeyEntry('C', "Capella", 042, 11),
        new StarKeyEntry('D', "Deneb", 056, 83),
        new StarKeyEntry('E', "Elnath", 070, 35),
        new StarKeyEntry('F', "Fomalhaut", 084, 91),
        new StarKeyEntry('G', "Gacrux", 098, 27),
        new StarKeyEntry('H', "Hamal", 112, 68),
        new StarKeyEntry('I', "Izar", 126, 19),
        new StarKeyEntry('J', "Jabbah", 140, 74),
        new StarKeyEntry('K', "Kochab", 154, 31),
        new StarKeyEntry('L', "Lesath", 168, 88),
        new StarKeyEntry('M', "Mintaka", 182, 24),
        new StarKeyEntry('N', "Nunki", 196, 55),
        new StarKeyEntry('O', "Orionis (Omicron Orionis)", 210, 13),
        new StarKeyEntry('P', "Polaris", 224, 95),
        new StarKeyEntry('Q', "Quaoar Star", 238, 42),
        new StarKeyEntry('R', "Rigel", 252, 17),
        new StarKeyEntry('S', "Sirius", 266, 99),
        new StarKeyEntry('T', "Thuban", 280, 36),
        new StarKeyEntry('U', "Unukalhai", 294, 72),
        new StarKeyEntry('V', "Vega", 308, 28),
        new StarKeyEntry('W', "Wezen", 322, 84),
        new StarKeyEntry('X', "Xi Aquilae", 336, 21),
        new StarKeyEntry('Y', "Yed Prior", 350, 58),
        new StarKeyEntry('Z', "Zubenelgenubi", 359, 44),
    ]);

    private static readonly Dictionary<char, StarKeyEntry> s_byLetter =
        Entries.ToDictionary(entry => entry.Letter);

    private static readonly Dictionary<string, StarKeyEntry> s_byCode =
        Entries.ToDictionary(entry => entry.Code);

    /// <summary>
    ///  Gets the star key entry for the given letter.
    /// </summary>
    /// <param name="letter">A letter, case-insensitive (A-Z).</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///  Thrown when <paramref name="letter"/> is not a letter A-Z.
    /// </exception>
    public static StarKeyEntry GetByLetter(char letter)
    {
        char upper = char.ToUpperInvariant(letter);
        if (!s_byLetter.TryGetValue(upper, out StarKeyEntry? entry))
        {
            throw new ArgumentOutOfRangeException(nameof(letter), letter, "Letter must be A-Z.");
        }

        return entry;
    }

    /// <summary>
    ///  Attempts to look up a star key entry by its 5-digit numeric code.
    /// </summary>
    public static bool TryGetByCode(string code, out StarKeyEntry? entry) =>
        s_byCode.TryGetValue(code, out entry);
}
