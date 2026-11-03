using PolarCoordinatesCypher.Core;

namespace PolarCoordinatesCypher.Tests;

[TestClass]
public class PolarGraphLayoutTests
{
    private const double Tolerance = 0.0001;

    [TestMethod]
    public void Layout_ZeroDegrees_PlacesPointDirectlyOnPositiveXAxis()
    {
        PolarPoint point = new('A', "TestStar", ThetaDegrees: 0, Radius: 99);

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout([point], maxPlotRadius: 100);

        Assert.HasCount(1, plotted);
        Assert.AreEqual(100, plotted[0].X, Tolerance);
        Assert.AreEqual(0, plotted[0].Y, Tolerance);
    }

    [TestMethod]
    public void Layout_NinetyDegrees_PlacesPointDirectlyAboveCenter()
    {
        PolarPoint point = new('A', "TestStar", ThetaDegrees: 90, Radius: 99);

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout([point], maxPlotRadius: 100);

        Assert.AreEqual(0, plotted[0].X, Tolerance);
        Assert.AreEqual(100, plotted[0].Y, Tolerance);
    }

    [TestMethod]
    public void Layout_ZeroRadius_PlacesPointAtCenter()
    {
        PolarPoint point = new('A', "TestStar", ThetaDegrees: 45, Radius: 0);

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout([point], maxPlotRadius: 100);

        Assert.AreEqual(0, plotted[0].X, Tolerance);
        Assert.AreEqual(0, plotted[0].Y, Tolerance);
    }

    [TestMethod]
    public void Layout_ScalesRadiusProportionally()
    {
        PolarPoint halfRadius = new('A', "TestStar", ThetaDegrees: 0, Radius: PolarGraphLayout.MaxDataRadius / 2);

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout([halfRadius], maxPlotRadius: 100);

        Assert.AreEqual(50, plotted[0].X, Tolerance);
    }

    [TestMethod]
    public void Layout_PreservesInputOrderAndCount()
    {
        IReadOnlyList<PolarPoint> points = PolarCipher.ToPolarPoints("STAR");

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout(points, maxPlotRadius: 100);

        Assert.HasCount(points.Count, plotted);
        for (int i = 0; i < points.Count; i++)
        {
            Assert.AreEqual(points[i], plotted[i].Source);
        }
    }

    [TestMethod]
    public void Layout_EmptyList_ReturnsEmptyResult()
    {
        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout([], maxPlotRadius: 100);

        Assert.IsEmpty(plotted);
    }
}
