using PolarCoordinatesCypher.Core;

namespace PolarCoordinatesCypher
{
    public partial class MainWindow : Form
    {
        private string? _backgroundImagePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private CipherMode SelectedMode => _rbConstellation.Checked ? CipherMode.Constellation : CipherMode.Numeric;

        private void BtnEncrypt_Click(object? sender, EventArgs e)
        {
            string plainText = _txtPlainText.Text;

            if (string.IsNullOrWhiteSpace(plainText))
            {
                MessageBox.Show(this, "Enter a message to encrypt.", "Real-Star Polaris Cipher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string cipherText = PolarCipher.Encrypt(plainText, SelectedMode);
                _txtCiphertext.Text = cipherText;
                _polarGraphControl.Points = PolarCipher.ToPolarPoints(plainText);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(this, ex.Message, "Real-Star Polaris Cipher",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDecrypt_Click(object? sender, EventArgs e)
        {
            string cipherText = _txtCiphertext.Text;

            if (string.IsNullOrWhiteSpace(cipherText))
            {
                MessageBox.Show(this, "Enter or generate ciphertext to decrypt.", "Real-Star Polaris Cipher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _txtDecrypted.Text = PolarCipher.Decrypt(cipherText, SelectedMode);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(this, ex.Message, "Real-Star Polaris Cipher",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChkConnectPoints_CheckedChanged(object? sender, EventArgs e)
        {
            _polarGraphControl.ConnectPoints = _chkConnectPoints.Checked;
        }

        private void ChkShowGrid_CheckedChanged(object? sender, EventArgs e)
        {
            _polarGraphControl.ShowGrid = _chkShowGrid.Checked;
        }

        private void ChkPolarisTheme_CheckedChanged(object? sender, EventArgs e)
        {
            _polarGraphControl.PolarisChartTheme = _chkPolarisTheme.Checked;
        }

        private void BtnLoadBackground_Click(object? sender, EventArgs e)
        {
            using OpenFileDialog openDialog = new()
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files (*.*)|*.*",
                Title = "Load Background Image",
            };

            if (openDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                using Image loadedImage = Image.FromFile(openDialog.FileName);
                _polarGraphControl.BackgroundImage?.Dispose();
                _polarGraphControl.BackgroundImage = new Bitmap(loadedImage);
                _polarGraphControl.BackgroundImageLayout = ImageLayout.Zoom;
                _backgroundImagePath = openDialog.FileName;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or OutOfMemoryException)
            {
                MessageBox.Show(this, $"Could not load the image: {ex.Message}", "Real-Star Polaris Cipher",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClearBackground_Click(object? sender, EventArgs e)
        {
            _polarGraphControl.BackgroundImage?.Dispose();
            _polarGraphControl.BackgroundImage = null;
            _backgroundImagePath = null;
        }

        private void BtnExportPdf_Click(object? sender, EventArgs e)
        {
            string plainText = _txtPlainText.Text;

            if (string.IsNullOrWhiteSpace(plainText))
            {
                MessageBox.Show(this, "Enter and encrypt a message before exporting.", "Real-Star Polaris Cipher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog saveDialog = new()
            {
                Filter = "PDF document (*.pdf)|*.pdf",
                FileName = "PolarisCipherStarMap.pdf",
                DefaultExt = "pdf",
            };

            if (saveDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                PolarGraphPdfExporter.Export(
                    plainText,
                    saveDialog.FileName,
                    SelectedMode,
                    _chkConnectPoints.Checked,
                    _backgroundImagePath,
                    _chkShowGrid.Checked,
                    _chkPolarisTheme.Checked);
                MessageBox.Show(this, "Star map PDF exported successfully.", "Real-Star Polaris Cipher",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(this, $"Could not save the PDF: {ex.Message}", "Real-Star Polaris Cipher",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
