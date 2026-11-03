using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PolarCoordinatesCypher.Core;

/// <summary>
///  Renders a Real-Star Polaris Cipher message as a polar "star map" PDF,
///  sized for standard 8.5x11in (US Letter) printer paper.
/// </summary>
public static class PolarGraphPdfExporter
{
    /// <summary>
    ///  Ensures PdfSharp has a valid (free, AGPL) license loaded so that
    ///  <see cref="PdfSharp.Pdf.PdfDocument"/> can be constructed without throwing.
    ///  See https://docs.pdfsharp.net/link/license-loading.html for details.
    /// </summary>
    static PolarGraphPdfExporter()
    {
        PdfSharp.PdfSharpLicense.SetAgplLicense();
        PdfSharp.Fonts.GlobalFontSettings.UseWindowsFontsUnderWindows = true;
    }

    /// <summary>
    ///  Encrypts <paramref name="plainText"/> and exports a one-page Letter-size PDF
    ///  containing the ciphertext and a polar star-map plot of the message.
    /// </summary>
    /// <param name="plainText">The plaintext message to plot.</param>
    /// <param name="filePath">The destination .pdf file path.</param>
    /// <param name="mode">The ciphertext display format.</param>
    /// <param name="connectPoints">Whether to draw connecting lines between successive letters ("constellation" lines).</param>
    /// <param name="backgroundImagePath">
    ///  Optional path to an image file (e.g. a star field photo) drawn behind the polar
    ///  graph, scaled and centered to fill the graph area, to disguise the plotted message.
    /// </param>
    /// <param name="showGrid">Whether to draw the polar grid (axes and concentric circles).</param>
    public static void Export(
        string plainText,
        string filePath,
        CipherMode mode = CipherMode.Numeric,
        bool connectPoints = true,
        string? backgroundImagePath = null,
        bool showGrid = true)
    {
        ArgumentNullException.ThrowIfNull(plainText);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        IReadOnlyList<PolarPoint> points = PolarCipher.ToPolarPoints(plainText);
        string cipherText = PolarCipher.Encrypt(plainText, mode);

        using PdfDocument document = new();
        document.Info.Title = "Real-Star Polaris Cipher";

        PdfPage page = document.AddPage();
        page.Size = PdfSharp.PageSize.Letter;

        using XGraphics gfx = XGraphics.FromPdfPage(page);

        double width = page.Width.Point;
        double height = page.Height.Point;
        const double margin = 54; // 0.75in

        XFont titleFont = new("Arial", 18, XFontStyleEx.Bold);
        XFont captionFont = new("Arial", 10);
        XFont labelFont = new("Arial", 9);

        gfx.DrawString(
            "Real-Star Polaris Cipher \u2014 Star Map",
            titleFont,
            XBrushes.Black,
            new XRect(0, margin / 2, width, 30),
            XStringFormats.TopCenter);

        gfx.DrawString($"Message: {plainText}", captionFont, XBrushes.Black, new XPoint(margin, margin + 10));
        gfx.DrawString($"Ciphertext: {cipherText}", captionFont, XBrushes.Black, new XPoint(margin, margin + 26));

        double graphTop = margin + 50;
        double graphSize = Math.Min(width - (margin * 2), height - graphTop - margin);
        double centerX = width / 2;
        double centerY = graphTop + (graphSize / 2);
        double maxPlotRadius = (graphSize / 2) - 20;

        if (!string.IsNullOrWhiteSpace(backgroundImagePath))
        {
            DrawBackgroundImage(gfx, backgroundImagePath, centerX - (graphSize / 2), graphTop, graphSize);
        }

        if (showGrid)
        {
            DrawAxes(gfx, centerX, centerY, maxPlotRadius);
        }

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout(points, maxPlotRadius);

        if (connectPoints)
        {
            XPen linePen = new(XColor.FromArgb(120, 120, 180), 1);
            for (int i = 0; i < plotted.Count - 1; i++)
            {
                PlottedPoint a = plotted[i];
                PlottedPoint b = plotted[i + 1];
                gfx.DrawLine(linePen, centerX + a.X, centerY - a.Y, centerX + b.X, centerY - b.Y);
            }
        }

        XSolidBrush starBrush = new(XColors.DarkGoldenrod);
        XPen starOutline = new(XColors.Black, 0.5);

        foreach (PlottedPoint p in plotted)
        {
            double px = centerX + p.X;
            double py = centerY - p.Y;
            double markerSize = 3 + (p.Source.Radius / PolarGraphLayout.MaxDataRadius * 6);

            gfx.DrawEllipse(starOutline, starBrush, px - (markerSize / 2), py - (markerSize / 2), markerSize, markerSize);
            gfx.DrawString(p.Source.Letter.ToString(), labelFont, XBrushes.Black, new XPoint(px + markerSize, py - markerSize));
        }

        gfx.DrawString(
            "Decode using the Real-Star Polaris Cipher key (theta, radius \u2192 letter).",
            captionFont,
            XBrushes.Gray,
            new XRect(0, height - margin, width, 20),
            XStringFormats.TopCenter);

        document.Save(filePath);
    }

    private static void DrawBackgroundImage(XGraphics gfx, string backgroundImagePath, double left, double top, double size)
    {
        using XImage image = XImage.FromFile(backgroundImagePath);

        double imageAspect = image.PixelWidth / (double)image.PixelHeight;
        double drawWidth = size;
        double drawHeight = size;

        if (imageAspect > 1)
        {
            drawHeight = size / imageAspect;
        }
        else
        {
            drawWidth = size * imageAspect;
        }

        double offsetX = left + ((size - drawWidth) / 2);
        double offsetY = top + ((size - drawHeight) / 2);

        gfx.DrawImage(image, offsetX, offsetY, drawWidth, drawHeight);
    }

    private static void DrawAxes(XGraphics gfx, double centerX, double centerY, double maxPlotRadius)
    {
        XPen axisPen = new(XColors.LightGray, 1);
        gfx.DrawLine(axisPen, centerX - maxPlotRadius - 10, centerY, centerX + maxPlotRadius + 10, centerY);
        gfx.DrawLine(axisPen, centerX, centerY - maxPlotRadius - 10, centerX, centerY + maxPlotRadius + 10);

        for (double r = maxPlotRadius / 4; r <= maxPlotRadius + 0.01; r += maxPlotRadius / 4)
        {
            gfx.DrawEllipse(axisPen, centerX - r, centerY - r, r * 2, r * 2);
        }
    }
}
