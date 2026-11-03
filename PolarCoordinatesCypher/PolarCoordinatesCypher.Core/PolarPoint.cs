namespace PolarCoordinatesCypher.Core;

/// <summary>
///  Represents a single plotted letter on the polar star-map graph.
/// </summary>
/// <param name="Letter">The plaintext letter this point represents.</param>
/// <param name="StarName">The real star name assigned to the letter.</param>
/// <param name="ThetaDegrees">The angular position in degrees (0-359).</param>
/// <param name="Radius">The radial distance from the graph's center (0-99).</param>
public sealed record PolarPoint(char Letter, string StarName, double ThetaDegrees, double Radius);
