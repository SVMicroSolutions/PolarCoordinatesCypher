using PolarCoordinatesCypher.Core;

namespace PolarCoordinatesCypher;

/// <summary>
///  A custom panel that draws the Real-Star Polaris Cipher message as a polar
///  "star map": concentric grid circles, plotted star markers for each letter,
///  and connecting lines forming a "constellation".
/// </summary>
public sealed class PolarGraphControl : Panel
{
    private IReadOnlyList<PolarPoint> _points = [];

    /// <summary>
    ///  Gets or sets the message to plot. Setting this property triggers a redraw.
    /// </summary>
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    [System.ComponentModel.Browsable(false)]
    public IReadOnlyList<PolarPoint> Points
    {
        get => _points;
        set
        {
            _points = value ?? [];
            Invalidate();
        }
    }

    /// <summary>
    ///  Gets or sets whether successive letters are connected with lines.
    /// </summary>
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    [System.ComponentModel.Browsable(false)]
    public bool ConnectPoints { get; set; } = true;

    /// <summary>
    ///  Gets or sets whether the polar grid (axes and concentric circles) is drawn.
    /// </summary>
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    [System.ComponentModel.Browsable(false)]
    public bool ShowGrid { get; set; } = true;

    /// <summary>
    ///  Gets or sets whether the decorative "Order of the Mark" Polaris star-chart theme is used:
    ///  a night-sky backdrop, gold star markers, faint silver constellation lines, a Polaris
    ///  marker at the origin, and a compass rose around the edge.
    /// </summary>
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    [System.ComponentModel.Browsable(false)]
    public bool PolarisChartTheme { get; set; }

    public PolarGraphControl()
    {
        DoubleBuffered = true;
        BackColor = Color.Black;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        float centerX = ClientSize.Width / 2f;
        float centerY = ClientSize.Height / 2f;
        float maxPlotRadius = (Math.Min(ClientSize.Width, ClientSize.Height) / 2f) - 20f;

        if (maxPlotRadius <= 0)
        {
            return;
        }

        bool paintNightSky = PolarisChartTheme && BackgroundImage is null;

        if (paintNightSky)
        {
            DrawNightSky(g, centerX, centerY, maxPlotRadius);
        }

        if (PolarisChartTheme)
        {
            DrawCompassRose(g, centerX, centerY, maxPlotRadius);
        }

        if (ShowGrid)
        {
            DrawAxes(g, centerX, centerY, maxPlotRadius, PolarisChartTheme);
        }

        if (PolarisChartTheme)
        {
            DrawPolarisMarker(g, centerX, centerY);
        }

        if (_points.Count == 0)
        {
            return;
        }

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout(_points, maxPlotRadius);

        if (ConnectPoints && plotted.Count > 1)
        {
            Color lineColor = PolarisChartTheme ? Color.FromArgb(170, 210, 210, 220) : Color.FromArgb(160, 120, 120, 200);
            using Pen linePen = new(lineColor, 1.25f);
            for (int i = 0; i < plotted.Count - 1; i++)
            {
                PlottedPoint a = plotted[i];
                PlottedPoint b = plotted[i + 1];
                g.DrawLine(
                    linePen,
                    centerX + (float)a.X, centerY - (float)a.Y,
                    centerX + (float)b.X, centerY - (float)b.Y);
            }
        }

        using Brush starBrush = new SolidBrush(Color.Gold);
        using Pen starOutline = new(PolarisChartTheme ? Color.FromArgb(220, 255, 236, 179) : Color.White, 0.75f);
        using Font labelFont = new(Font.FontFamily, 8f);
        using Brush labelBrush = new SolidBrush(PolarisChartTheme ? Color.FromArgb(240, 230, 210) : Color.White);

        foreach (PlottedPoint p in plotted)
        {
            float px = centerX + (float)p.X;
            float py = centerY - (float)p.Y;
            float markerSize = 3f + (float)(p.Source.Radius / PolarGraphLayout.MaxDataRadius * 7);

            if (PolarisChartTheme)
            {
                DrawStarGlyph(g, starBrush, starOutline, px, py, markerSize);
            }
            else
            {
                RectangleF rect = new(px - (markerSize / 2), py - (markerSize / 2), markerSize, markerSize);
                g.FillEllipse(starBrush, rect);
                g.DrawEllipse(starOutline, rect);
            }

            g.DrawString(p.Source.Letter.ToString(), labelFont, labelBrush, px + markerSize, py - markerSize);
        }
    }

    private static void DrawNightSky(Graphics g, float centerX, float centerY, float maxPlotRadius)
    {
        float skyRadius = maxPlotRadius + 20f;
        RectangleF bounds = new(centerX - skyRadius, centerY - skyRadius, skyRadius * 2, skyRadius * 2);

        using System.Drawing.Drawing2D.GraphicsPath path = new();
        path.AddEllipse(bounds);

        using System.Drawing.Drawing2D.PathGradientBrush skyBrush = new(path)
        {
            CenterPoint = new PointF(centerX, centerY),
            CenterColor = Color.FromArgb(255, 20, 24, 46),
            SurroundColors = [Color.FromArgb(255, 2, 4, 12)],
        };

        g.FillEllipse(skyBrush, bounds);

        // Scatter faint background stars for atmosphere (deterministic seed for a stable look).
        Random rng = new(12345);
        using Brush dimStarBrush = new SolidBrush(Color.FromArgb(140, 255, 255, 255));
        for (int i = 0; i < 80; i++)
        {
            double angle = rng.NextDouble() * Math.PI * 2;
            double radius = rng.NextDouble() * skyRadius;
            float sx = centerX + (float)(radius * Math.Cos(angle));
            float sy = centerY + (float)(radius * Math.Sin(angle));
            float size = (float)(rng.NextDouble() * 1.5) + 0.5f;
            g.FillEllipse(dimStarBrush, sx - (size / 2), sy - (size / 2), size, size);
        }
    }

    private static void DrawCompassRose(Graphics g, float centerX, float centerY, float maxPlotRadius)
    {
        float roseRadius = maxPlotRadius + 16f;
        using Pen tickPen = new(Color.FromArgb(180, 200, 200, 210), 1f);
        using Font compassFont = new("Segoe UI", 9f, FontStyle.Bold);
        using Brush compassBrush = new SolidBrush(Color.FromArgb(220, 200, 200, 215));

        (string Label, double AngleDegrees)[] headings =
        [
            ("N", 90), ("E", 0), ("S", 270), ("W", 180),
        ];

        foreach ((string label, double angleDegrees) in headings)
        {
            double angleRadians = angleDegrees * Math.PI / 180.0;
            float tx = centerX + (float)(roseRadius * Math.Cos(angleRadians));
            float ty = centerY - (float)(roseRadius * Math.Sin(angleRadians));

            SizeF textSize = g.MeasureString(label, compassFont);
            g.DrawString(label, compassFont, compassBrush, tx - (textSize.Width / 2), ty - (textSize.Height / 2));
        }

        for (int tick = 0; tick < 36; tick++)
        {
            double angleRadians = tick * 10 * Math.PI / 180.0;
            float innerRadius = roseRadius - (tick % 9 == 0 ? 8f : 4f);
            float x1 = centerX + (float)(innerRadius * Math.Cos(angleRadians));
            float y1 = centerY - (float)(innerRadius * Math.Sin(angleRadians));
            float x2 = centerX + (float)(roseRadius * Math.Cos(angleRadians));
            float y2 = centerY - (float)(roseRadius * Math.Sin(angleRadians));
            g.DrawLine(tickPen, x1, y1, x2, y2);
        }
    }

    private static void DrawPolarisMarker(Graphics g, float centerX, float centerY)
    {
        using Brush glowBrush = new SolidBrush(Color.FromArgb(70, 255, 255, 255));
        g.FillEllipse(glowBrush, centerX - 10, centerY - 10, 20, 20);

        using Brush starBrush = new SolidBrush(Color.White);
        using Pen starOutline = new(Color.FromArgb(220, 255, 255, 255), 0.75f);
        DrawStarGlyph(g, starBrush, starOutline, centerX, centerY, 8f);

        using Font labelFont = new("Segoe UI", 8f, FontStyle.Italic);
        using Brush labelBrush = new SolidBrush(Color.FromArgb(230, 255, 255, 255));
        g.DrawString("Polaris", labelFont, labelBrush, centerX + 7, centerY + 5);
    }

    private static void DrawStarGlyph(Graphics g, Brush fillBrush, Pen outlinePen, float centerX, float centerY, float size)
    {
        float outerRadius = size;
        float innerRadius = size * 0.4f;
        const int points = 4;

        PointF[] vertices = new PointF[points * 2];
        for (int i = 0; i < points * 2; i++)
        {
            double angle = (Math.PI / points * i) - (Math.PI / 2);
            float radius = i % 2 == 0 ? outerRadius : innerRadius;
            vertices[i] = new PointF(
                centerX + (float)(radius * Math.Cos(angle)),
                centerY + (float)(radius * Math.Sin(angle)));
        }

        g.FillPolygon(fillBrush, vertices);
        g.DrawPolygon(outlinePen, vertices);
    }

    private static void DrawAxes(Graphics g, float centerX, float centerY, float maxPlotRadius, bool polarisChartTheme)
    {
        Color axisColor = polarisChartTheme ? Color.FromArgb(70, 200, 200, 215) : Color.FromArgb(90, 255, 255, 255);
        using Pen axisPen = new(axisColor, 1f);

        g.DrawLine(axisPen, centerX - maxPlotRadius - 10, centerY, centerX + maxPlotRadius + 10, centerY);
        g.DrawLine(axisPen, centerX, centerY - maxPlotRadius - 10, centerX, centerY + maxPlotRadius + 10);

        for (float r = maxPlotRadius / 4; r <= maxPlotRadius + 0.01f; r += maxPlotRadius / 4)
        {
            g.DrawEllipse(axisPen, centerX - r, centerY - r, r * 2, r * 2);
        }
    }
}
