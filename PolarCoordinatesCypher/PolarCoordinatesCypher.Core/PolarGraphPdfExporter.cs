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
    /// <param name="polarisChartTheme">
    ///  Whether to render the decorative "Order of the Mark" Polaris star-chart theme: a night-sky
    ///  backdrop, gold star markers, faint silver constellation lines, a Polaris marker at the
    ///  origin, and a compass rose around the edge.
    /// </param>
    public static void Export(
        string plainText,
        string filePath,
        CipherMode mode = CipherMode.Numeric,
        bool connectPoints = true,
        string? backgroundImagePath = null,
        bool showGrid = true,
        bool polarisChartTheme = false)
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

        double captionWidth = width - (margin * 2);

        double messageTop = margin + 10;
        double messageHeight = DrawWrappedString(
            gfx, $"Message: {plainText}", captionFont, XBrushes.Black, margin, messageTop, captionWidth);

        double ciphertextTop = messageTop + messageHeight + 4;
        double ciphertextHeight = DrawWrappedString(
            gfx, $"Ciphertext: {cipherText}", captionFont, XBrushes.Black, margin, ciphertextTop, captionWidth);

        double graphTop = ciphertextTop + ciphertextHeight + 14;
        double graphSize = Math.Min(width - (margin * 2), height - graphTop - margin);
        double centerX = width / 2;
        double centerY = graphTop + (graphSize / 2);
        double maxPlotRadius = (graphSize / 2) - 20;

        DrawStarMap(
            gfx, points, centerX, centerY, graphTop, graphSize, maxPlotRadius, labelFont,
            connectPoints, backgroundImagePath, showGrid, polarisChartTheme);

        gfx.DrawString(
            "Decode using the Real-Star Polaris Cipher key (theta, radius \u2192 letter).",
            captionFont,
            XBrushes.Gray,
            new XRect(0, height - margin, width, 20),
            XStringFormats.TopCenter);

        DrawDecoderPage(
            document, points, width, height, margin, labelFont,
            connectPoints, backgroundImagePath, showGrid, polarisChartTheme);

        document.Save(filePath);
    }

    /// <summary>
    ///  Adds a second page containing only the polar star-map plot (no plaintext, ciphertext,
    ///  title, or captions), suitable for handing to someone else to decode using the
    ///  Real-Star Polaris Cipher key.
    /// </summary>
    private static void DrawDecoderPage(
        PdfDocument document,
        IReadOnlyList<PolarPoint> points,
        double width,
        double height,
        double margin,
        XFont labelFont,
        bool connectPoints,
        string? backgroundImagePath,
        bool showGrid,
        bool polarisChartTheme)
    {
        PdfPage decoderPage = document.AddPage();
        decoderPage.Size = PdfSharp.PageSize.Letter;

        using XGraphics gfx = XGraphics.FromPdfPage(decoderPage);

        double graphTop = margin;
        double graphSize = Math.Min(width - (margin * 2), height - (margin * 2));
        double centerX = width / 2;
        double centerY = graphTop + (graphSize / 2);
        double maxPlotRadius = (graphSize / 2) - 20;

        DrawStarMap(
            gfx, points, centerX, centerY, graphTop, graphSize, maxPlotRadius, labelFont,
            connectPoints, backgroundImagePath, showGrid, polarisChartTheme);
    }

    private static void DrawStarMap(
        XGraphics gfx,
        IReadOnlyList<PolarPoint> points,
        double centerX,
        double centerY,
        double graphTop,
        double graphSize,
        double maxPlotRadius,
        XFont labelFont,
        bool connectPoints,
        string? backgroundImagePath,
        bool showGrid,
        bool polarisChartTheme)
    {
        if (!string.IsNullOrWhiteSpace(backgroundImagePath))
        {
            DrawBackgroundImage(gfx, backgroundImagePath, centerX - (graphSize / 2), graphTop, graphSize);
        }
        else if (polarisChartTheme)
        {
            DrawNightSky(gfx, centerX, centerY, maxPlotRadius);
        }

        if (polarisChartTheme)
        {
            DrawCompassRose(gfx, centerX, centerY, maxPlotRadius);
        }

        if (showGrid)
        {
            DrawAxes(gfx, centerX, centerY, maxPlotRadius, polarisChartTheme);
        }

        if (polarisChartTheme)
        {
            DrawPolarisMarker(gfx, centerX, centerY);
        }

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout(points, maxPlotRadius);

        if (connectPoints)
        {
            XColor lineColor = polarisChartTheme ? XColor.FromArgb(170, 205, 205, 215) : XColor.FromArgb(120, 120, 180);
            XPen linePen = new(lineColor, 1);
            for (int i = 0; i < plotted.Count - 1; i++)
            {
                PlottedPoint a = plotted[i];
                PlottedPoint b = plotted[i + 1];
                gfx.DrawLine(linePen, centerX + a.X, centerY - a.Y, centerX + b.X, centerY - b.Y);
            }
        }

        XSolidBrush starBrush = new(XColors.DarkGoldenrod);
        XPen starOutline = new(polarisChartTheme ? XColor.FromArgb(220, 255, 236, 179) : XColors.Black, 0.5);

        foreach (PlottedPoint p in plotted)
        {
            double px = centerX + p.X;
            double py = centerY - p.Y;
            double markerSize = 3 + (p.Source.Radius / PolarGraphLayout.MaxDataRadius * 6);

            if (polarisChartTheme)
            {
                DrawStarGlyph(gfx, starBrush, starOutline, px, py, markerSize * 1.4);
            }
            else
            {
                gfx.DrawEllipse(starOutline, starBrush, px - (markerSize / 2), py - (markerSize / 2), markerSize, markerSize);
            }

            gfx.DrawString(p.Source.Letter.ToString(), labelFont, XBrushes.Black, new XPoint(px + markerSize, py - markerSize));
        }
    }

    /// <summary>
    ///  Draws text wrapped across multiple lines so it does not run off the right edge
    ///  of the page, breaking on the existing "token / token" separators used by the
    ///  ciphertext when a single line would otherwise overflow <paramref name="maxWidth"/>.
    /// </summary>
    /// <returns>The total height, in points, consumed by the wrapped text.</returns>
    private static double DrawWrappedString(XGraphics gfx, string text, XFont font, XBrush brush, double left, double top, double maxWidth)
    {
        string[] tokens = text.Split(' ');
        List<string> lines = [];
        System.Text.StringBuilder currentLine = new();

        foreach (string token in tokens)
        {
            string candidate = currentLine.Length == 0 ? token : $"{currentLine} {token}";

            if (gfx.MeasureString(candidate, font).Width > maxWidth && currentLine.Length > 0)
            {
                lines.Add(currentLine.ToString());
                currentLine.Clear();
                currentLine.Append(token);
            }
            else
            {
                currentLine.Clear();
                currentLine.Append(candidate);
            }
        }

        if (currentLine.Length > 0)
        {
            lines.Add(currentLine.ToString());
        }

        double lineHeight = font.GetHeight() * 1.15;
        for (int i = 0; i < lines.Count; i++)
        {
            gfx.DrawString(lines[i], font, brush, new XPoint(left, top + (lineHeight * i)));
        }

        return lineHeight * lines.Count;
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

    private static void DrawAxes(XGraphics gfx, double centerX, double centerY, double maxPlotRadius, bool polarisChartTheme)
    {
        XColor axisColor = polarisChartTheme ? XColor.FromArgb(90, 190, 190, 205) : XColors.LightGray;
        XPen axisPen = new(axisColor, 1);
        gfx.DrawLine(axisPen, centerX - maxPlotRadius - 10, centerY, centerX + maxPlotRadius + 10, centerY);
        gfx.DrawLine(axisPen, centerX, centerY - maxPlotRadius - 10, centerX, centerY + maxPlotRadius + 10);

        for (double r = maxPlotRadius / 4; r <= maxPlotRadius + 0.01; r += maxPlotRadius / 4)
        {
            gfx.DrawEllipse(axisPen, centerX - r, centerY - r, r * 2, r * 2);
        }
    }

    private static void DrawNightSky(XGraphics gfx, double centerX, double centerY, double maxPlotRadius)
    {
        double skyRadius = maxPlotRadius + 20;
        const int bands = 24;

        for (int i = bands; i >= 1; i--)
        {
            double bandRadius = skyRadius * i / bands;
            byte shade = (byte)(2 + ((20 - 2) * (bands - i) / (double)bands));
            XSolidBrush bandBrush = new(XColor.FromArgb(255, shade, (byte)(shade + 4), (byte)(shade + 22)));
            gfx.DrawEllipse(bandBrush, centerX - bandRadius, centerY - bandRadius, bandRadius * 2, bandRadius * 2);
        }

        Random rng = new(12345);
        XSolidBrush dimStarBrush = new(XColor.FromArgb(150, 255, 255, 255));
        for (int i = 0; i < 80; i++)
        {
            double angle = rng.NextDouble() * Math.PI * 2;
            double radius = rng.NextDouble() * skyRadius;
            double sx = centerX + (radius * Math.Cos(angle));
            double sy = centerY + (radius * Math.Sin(angle));
            double size = (rng.NextDouble() * 1.2) + 0.4;
            gfx.DrawEllipse(dimStarBrush, sx - (size / 2), sy - (size / 2), size, size);
        }
    }

    private static void DrawCompassRose(XGraphics gfx, double centerX, double centerY, double maxPlotRadius)
    {
        double roseRadius = maxPlotRadius + 16;
        XPen tickPen = new(XColor.FromArgb(190, 205, 205, 215), 1);
        XFont compassFont = new("Arial", 9, XFontStyleEx.Bold);
        XSolidBrush compassBrush = new(XColor.FromArgb(220, 205, 205, 220));

        (string Label, double AngleDegrees)[] headings =
        [
            ("N", 90), ("E", 0), ("S", 270), ("W", 180),
        ];

        foreach ((string label, double angleDegrees) in headings)
        {
            double angleRadians = angleDegrees * Math.PI / 180.0;
            double tx = centerX + (roseRadius * Math.Cos(angleRadians));
            double ty = centerY - (roseRadius * Math.Sin(angleRadians));

            XSize textSize = gfx.MeasureString(label, compassFont);
            gfx.DrawString(label, compassFont, compassBrush, new XPoint(tx - (textSize.Width / 2), ty + (textSize.Height / 2)));
        }

        for (int tick = 0; tick < 36; tick++)
        {
            double angleRadians = tick * 10 * Math.PI / 180.0;
            double innerRadius = roseRadius - (tick % 9 == 0 ? 8 : 4);
            double x1 = centerX + (innerRadius * Math.Cos(angleRadians));
            double y1 = centerY - (innerRadius * Math.Sin(angleRadians));
            double x2 = centerX + (roseRadius * Math.Cos(angleRadians));
            double y2 = centerY - (roseRadius * Math.Sin(angleRadians));
            gfx.DrawLine(tickPen, x1, y1, x2, y2);
        }
    }

    private static void DrawPolarisMarker(XGraphics gfx, double centerX, double centerY)
    {
        XSolidBrush glowBrush = new(XColor.FromArgb(70, 255, 255, 255));
        gfx.DrawEllipse(glowBrush, centerX - 10, centerY - 10, 20, 20);

        XSolidBrush starBrush = new(XColors.White);
        XPen starOutline = new(XColor.FromArgb(220, 255, 255, 255), 0.5);
        DrawStarGlyph(gfx, starBrush, starOutline, centerX, centerY, 8);

        XFont labelFont = new("Arial", 8, XFontStyleEx.Italic);
        XSolidBrush labelBrush = new(XColor.FromArgb(230, 255, 255, 255));
        gfx.DrawString("Polaris", labelFont, labelBrush, new XPoint(centerX + 7, centerY + 5));
    }

    private static void DrawStarGlyph(XGraphics gfx, XBrush fillBrush, XPen outlinePen, double centerX, double centerY, double size)
    {
        double outerRadius = size;
        double innerRadius = size * 0.4;
        const int points = 4;

        XPoint[] vertices = new XPoint[points * 2];
        for (int i = 0; i < points * 2; i++)
        {
            double angle = (Math.PI / points * i) - (Math.PI / 2);
            double radius = i % 2 == 0 ? outerRadius : innerRadius;
            vertices[i] = new XPoint(
                centerX + (radius * Math.Cos(angle)),
                centerY + (radius * Math.Sin(angle)));
        }

        gfx.DrawPolygon(fillBrush, vertices, XFillMode.Winding);
        gfx.DrawPolygon(outlinePen, vertices);
    }
}
