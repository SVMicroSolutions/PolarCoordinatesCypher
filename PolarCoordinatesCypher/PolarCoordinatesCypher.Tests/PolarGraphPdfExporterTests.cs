using System.Runtime.Versioning;
using PdfSharp.Pdf.IO;
using PolarCoordinatesCypher.Core;

namespace PolarCoordinatesCypher.Tests;

[TestClass]
public class PolarGraphPdfExporterTests
{
    [TestMethod]
    public void Export_CreatesNonEmptyPdfFile()
    {
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

        try
        {
            PolarGraphPdfExporter.Export("STARS", filePath, CipherMode.Numeric);

            Assert.IsTrue(File.Exists(filePath));
            FileInfo info = new(filePath);
            Assert.IsGreaterThan(0, info.Length);

            byte[] header = File.ReadAllBytes(filePath)[..5];
            Assert.AreEqual("%PDF-", System.Text.Encoding.ASCII.GetString(header));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [TestMethod]
    public void Export_CreatesTwoPages_WithSecondPageBeingDecoderStarMap()
    {
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

        try
        {
            PolarGraphPdfExporter.Export("STARS", filePath, CipherMode.Numeric);

            using PdfSharp.Pdf.PdfDocument document = PdfReader.Open(filePath, PdfDocumentOpenMode.ReadOnly);

            Assert.HasCount(2, document.Pages);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [TestMethod]
    public void Export_WithConnectPointsDisabled_StillCreatesFile()
    {
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

        try
        {
            PolarGraphPdfExporter.Export("VEGA", filePath, CipherMode.Constellation, connectPoints: false);

            Assert.IsTrue(File.Exists(filePath));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [TestMethod]
    public void Export_WithPolarisChartTheme_StillCreatesNonEmptyPdfFile()
    {
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

        try
        {
            PolarGraphPdfExporter.Export("POLARIS", filePath, CipherMode.Numeric, polarisChartTheme: true);

            Assert.IsTrue(File.Exists(filePath));
            FileInfo info = new(filePath);
            Assert.IsGreaterThan(0, info.Length);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [TestMethod]
    public void Export_WithShowGridDisabled_StillCreatesFile()
    {
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

        try
        {
            PolarGraphPdfExporter.Export("CODE", filePath, CipherMode.Numeric, showGrid: false);

            Assert.IsTrue(File.Exists(filePath));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [TestMethod]
    [SupportedOSPlatform("windows")]
    public void Export_WithBackgroundImage_StillCreatesFile()
    {
        string imagePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.bmp");
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");

        try
        {
            using (System.Drawing.Bitmap bitmap = new(50, 30))
            {
                bitmap.Save(imagePath, System.Drawing.Imaging.ImageFormat.Bmp);
            }

            PolarGraphPdfExporter.Export("STAR", filePath, CipherMode.Numeric, backgroundImagePath: imagePath);

            Assert.IsTrue(File.Exists(filePath));
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}
