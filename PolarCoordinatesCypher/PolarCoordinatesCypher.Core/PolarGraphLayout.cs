namespace PolarCoordinatesCypher.Core;

/// <summary>
///  A letter's polar point translated into Cartesian coordinates for drawing,
///  relative to a graph's center.
/// </summary>
/// <param name="Source">The original polar point (letter, star, theta, radius).</param>
/// <param name="X">The horizontal offset from the graph center, in the same units as the layout request.</param>
/// <param name="Y">The vertical offset from the graph center (positive = up), in the same units as the layout request.</param>
public readonly record struct PlottedPoint(PolarPoint Source, double X, double Y);

/// <summary>
///  Pure geometry helper that converts <see cref="PolarPoint"/> values (theta/radius)
///  into Cartesian offsets, shared by both the on-screen GDI+ preview and the PDF exporter
///  so the two renderers stay visually consistent.
/// </summary>
public static class PolarGraphLayout
{
    /// <summary>The maximum radius value used by the Real-Star Polaris Cipher key (0-99).</summary>
    public const double MaxDataRadius = 99;

    /// <summary>
    ///  Converts polar points into Cartesian offsets from the graph's center.
    /// </summary>
    /// <param name="points">The polar points to plot, in message order.</param>
    /// <param name="maxPlotRadius">
    ///  The radius, in drawing units (pixels or PDF points), that corresponds to
    ///  <see cref="MaxDataRadius"/>. This is typically half of the smaller graph dimension.
    /// </param>
    /// <returns>
    ///  The Cartesian offsets from the graph center. X is positive to the right, Y is
    ///  positive upward (0 degrees points right/east, angles increase counter-clockwise).
    /// </returns>
    public static IReadOnlyList<PlottedPoint> Layout(IReadOnlyList<PolarPoint> points, double maxPlotRadius)
    {
        ArgumentNullException.ThrowIfNull(points);

        List<PlottedPoint> plotted = new(points.Count);

        foreach (PolarPoint point in points)
        {
            double angleRadians = point.ThetaDegrees * Math.PI / 180.0;
            double scaledRadius = point.Radius / MaxDataRadius * maxPlotRadius;

            double x = scaledRadius * Math.Cos(angleRadians);
            double y = scaledRadius * Math.Sin(angleRadians);

            plotted.Add(new PlottedPoint(point, x, y));
        }

        return plotted;
    }
}
