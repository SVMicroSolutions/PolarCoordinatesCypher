namespace PolarCoordinatesCypher.Core;

/// <summary>
///  Selects the output format used by <see cref="PolarCipher"/>.
/// </summary>
public enum CipherMode
{
    /// <summary>Plain 5-digit numeric codes, e.g. "26699".</summary>
    Numeric,

    /// <summary>Codes prefixed with the plaintext letter, e.g. "S26699".</summary>
    Constellation,
}
