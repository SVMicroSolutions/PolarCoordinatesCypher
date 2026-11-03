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

        if (ShowGrid)
        {
            DrawAxes(g, centerX, centerY, maxPlotRadius);
        }

        if (_points.Count == 0)
        {
            return;
        }

        IReadOnlyList<PlottedPoint> plotted = PolarGraphLayout.Layout(_points, maxPlotRadius);

        if (ConnectPoints && plotted.Count > 1)
        {
            using Pen linePen = new(Color.FromArgb(160, 120, 120, 200), 1.25f);
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
        using Pen starOutline = new(Color.White, 0.75f);
        using Font labelFont = new(Font.FontFamily, 8f);
        using Brush labelBrush = new SolidBrush(Color.White);

        foreach (PlottedPoint p in plotted)
        {
            float px = centerX + (float)p.X;
            float py = centerY - (float)p.Y;
            float markerSize = 3f + (float)(p.Source.Radius / PolarGraphLayout.MaxDataRadius * 7);

            RectangleF rect = new(px - (markerSize / 2), py - (markerSize / 2), markerSize, markerSize);
            g.FillEllipse(starBrush, rect);
            g.DrawEllipse(starOutline, rect);
            g.DrawString(p.Source.Letter.ToString(), labelFont, labelBrush, px + markerSize, py - markerSize);
        }
    }

    private static void DrawAxes(Graphics g, float centerX, float centerY, float maxPlotRadius)
    {
        using Pen axisPen = new(Color.FromArgb(90, 255, 255, 255), 1f);

        g.DrawLine(axisPen, centerX - maxPlotRadius - 10, centerY, centerX + maxPlotRadius + 10, centerY);
        g.DrawLine(axisPen, centerX, centerY - maxPlotRadius - 10, centerX, centerY + maxPlotRadius + 10);

        for (float r = maxPlotRadius / 4; r <= maxPlotRadius + 0.01f; r += maxPlotRadius / 4)
        {
            g.DrawEllipse(axisPen, centerX - r, centerY - r, r * 2, r * 2);
        }
    }
}
