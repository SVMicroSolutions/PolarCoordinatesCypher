namespace PolarCoordinatesCypher.Core;

/// <summary>
///  Represents a single letter-to-star mapping in the Real-Star Polaris Cipher key.
/// </summary>
/// <param name="Letter">The uppercase plaintext letter (A-Z).</param>
/// <param name="StarName">The real star name assigned to this letter.</param>
/// <param name="ThetaDegrees">The cipher's angular position in degrees (000-359).</param>
/// <param name="Radius">The cipher's radial distance (00-99).</param>
public sealed record StarKeyEntry(char Letter, string StarName, int ThetaDegrees, int Radius)
{
    /// <summary>
    ///  Gets the 5-digit numeric code for this entry, formed from the zero-padded
    ///  theta (3 digits) followed by the zero-padded radius (2 digits).
    /// </summary>
    public string Code => $"{ThetaDegrees:000}{Radius:00}";

    /// <summary>
    ///  Gets the "Advanced Constellation" code, prefixed with the letter itself,
    ///  e.g. "S26699".
    /// </summary>
    public string ConstellationCode => $"{Letter}{Code}";
}
