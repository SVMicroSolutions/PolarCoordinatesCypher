namespace PolarCoordinatesCypher
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _polarGraphControl.BackgroundImage?.Dispose();

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _tlpMain = new TableLayoutPanel();
            _tlpInput = new TableLayoutPanel();
            _lblPlainText = new Label();
            _txtPlainText = new TextBox();
            _flpModeAndEncrypt = new FlowLayoutPanel();
            _grpMode = new GroupBox();
            _flpModeOptions = new FlowLayoutPanel();
            _rbNumeric = new RadioButton();
            _rbConstellation = new RadioButton();
            _btnEncrypt = new Button();
            _lblCiphertext = new Label();
            _txtCiphertext = new TextBox();
            _flpDecrypt = new FlowLayoutPanel();
            _btnDecrypt = new Button();
            _lblDecrypted = new Label();
            _txtDecrypted = new TextBox();
            _tlpGraph = new TableLayoutPanel();
            _polarGraphControl = new PolarGraphControl();
            _flpExport = new FlowLayoutPanel();
            _chkConnectPoints = new CheckBox();
            _chkShowGrid = new CheckBox();
            _chkPolarisTheme = new CheckBox();
            _btnClearBackground = new Button();
            _btnLoadBackground = new Button();
            _btnExportPdf = new Button();

            components = new System.ComponentModel.Container();
            _tlpMain.SuspendLayout();
            _tlpInput.SuspendLayout();
            _flpModeAndEncrypt.SuspendLayout();
            _grpMode.SuspendLayout();
            _flpModeOptions.SuspendLayout();
            _flpDecrypt.SuspendLayout();
            _tlpGraph.SuspendLayout();
            _flpExport.SuspendLayout();
            SuspendLayout();

            // _tlpMain
            _tlpMain.ColumnCount = 1;
            _tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpMain.RowCount = 2;
            _tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpMain.Dock = DockStyle.Fill;
            _tlpMain.Controls.Add(_tlpInput, 0, 0);
            _tlpMain.Controls.Add(_tlpGraph, 0, 1);
            _tlpMain.Name = "_tlpMain";
            _tlpMain.Padding = new Padding(8);

            // _tlpInput
            _tlpInput.ColumnCount = 2;
            _tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _tlpInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpInput.RowCount = 5;
            _tlpInput.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tlpInput.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tlpInput.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tlpInput.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tlpInput.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tlpInput.Dock = DockStyle.Top;
            _tlpInput.AutoSize = true;
            _tlpInput.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _tlpInput.Controls.Add(_lblPlainText, 0, 0);
            _tlpInput.Controls.Add(_txtPlainText, 1, 0);
            _tlpInput.Controls.Add(_flpModeAndEncrypt, 1, 1);
            _tlpInput.Controls.Add(_lblCiphertext, 0, 2);
            _tlpInput.Controls.Add(_txtCiphertext, 1, 2);
            _tlpInput.Controls.Add(_flpDecrypt, 1, 3);
            _tlpInput.Controls.Add(_lblDecrypted, 0, 4);
            _tlpInput.Controls.Add(_txtDecrypted, 1, 4);
            _tlpInput.Name = "_tlpInput";

            // _lblPlainText
            _lblPlainText.AutoSize = true;
            _lblPlainText.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _lblPlainText.Margin = new Padding(3, 8, 3, 3);
            _lblPlainText.Name = "_lblPlainText";
            _lblPlainText.Text = "Plain text:";
            _lblPlainText.AccessibleName = "Plain text label";

            // _txtPlainText
            _txtPlainText.Dock = DockStyle.Fill;
            _txtPlainText.Margin = new Padding(3);
            _txtPlainText.Name = "_txtPlainText";
            _txtPlainText.AccessibleName = "Plain text input";
            _txtPlainText.AccessibleDescription = "Enter the message to encrypt using the Real-Star Polaris Cipher.";

            // _flpModeAndEncrypt
            _flpModeAndEncrypt.AutoSize = true;
            _flpModeAndEncrypt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpModeAndEncrypt.FlowDirection = FlowDirection.LeftToRight;
            _flpModeAndEncrypt.Margin = new Padding(3);
            _flpModeAndEncrypt.Controls.Add(_grpMode);
            _flpModeAndEncrypt.Controls.Add(_btnEncrypt);
            _flpModeAndEncrypt.Name = "_flpModeAndEncrypt";

            // _grpMode
            _grpMode.AutoSize = true;
            _grpMode.AutoSizeMode = AutoSizeMode.GrowOnly;
            _grpMode.Controls.Add(_flpModeOptions);
            _grpMode.Margin = new Padding(3, 3, 12, 3);
            _grpMode.Name = "_grpMode";
            _grpMode.Text = "Cipher mode";
            _grpMode.AccessibleName = "Cipher mode group";

            // _flpModeOptions
            _flpModeOptions.AutoSize = true;
            _flpModeOptions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpModeOptions.FlowDirection = FlowDirection.TopDown;
            _flpModeOptions.Dock = DockStyle.Fill;
            _flpModeOptions.Controls.Add(_rbNumeric);
            _flpModeOptions.Controls.Add(_rbConstellation);
            _flpModeOptions.Name = "_flpModeOptions";

            // _rbNumeric
            _rbNumeric.AutoSize = true;
            _rbNumeric.Checked = true;
            _rbNumeric.Name = "_rbNumeric";
            _rbNumeric.TabStop = true;
            _rbNumeric.Text = "Numeric (26699-28036-...)";
            _rbNumeric.AccessibleName = "Numeric cipher mode";

            // _rbConstellation
            _rbConstellation.AutoSize = true;
            _rbConstellation.Name = "_rbConstellation";
            _rbConstellation.Text = "Constellation (S26699-T28036-...)";
            _rbConstellation.AccessibleName = "Constellation cipher mode";

            // _btnEncrypt
            _btnEncrypt.AutoSize = true;
            _btnEncrypt.Margin = new Padding(3, 3, 3, 3);
            _btnEncrypt.Name = "_btnEncrypt";
            _btnEncrypt.Text = "Encrypt \u2192";
            _btnEncrypt.AccessibleName = "Encrypt button";
            _btnEncrypt.AccessibleDescription = "Encrypts the plain text message and plots it on the star map.";
            _btnEncrypt.Click += BtnEncrypt_Click;

            // _lblCiphertext
            _lblCiphertext.AutoSize = true;
            _lblCiphertext.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _lblCiphertext.Margin = new Padding(3, 8, 3, 3);
            _lblCiphertext.Name = "_lblCiphertext";
            _lblCiphertext.Text = "Ciphertext:";
            _lblCiphertext.AccessibleName = "Ciphertext label";

            // _txtCiphertext
            _txtCiphertext.Dock = DockStyle.Fill;
            _txtCiphertext.Margin = new Padding(3);
            _txtCiphertext.Multiline = true;
            _txtCiphertext.ReadOnly = true;
            _txtCiphertext.Size = new Size(200, 48);
            _txtCiphertext.Name = "_txtCiphertext";
            _txtCiphertext.AccessibleName = "Ciphertext output";
            _txtCiphertext.AccessibleDescription = "The encrypted ciphertext, or paste ciphertext here to decrypt.";

            // _flpDecrypt
            _flpDecrypt.AutoSize = true;
            _flpDecrypt.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpDecrypt.FlowDirection = FlowDirection.LeftToRight;
            _flpDecrypt.Margin = new Padding(3);
            _flpDecrypt.Controls.Add(_btnDecrypt);
            _flpDecrypt.Name = "_flpDecrypt";

            // _btnDecrypt
            _btnDecrypt.AutoSize = true;
            _btnDecrypt.Margin = new Padding(3);
            _btnDecrypt.Name = "_btnDecrypt";
            _btnDecrypt.Text = "\u2190 Decrypt";
            _btnDecrypt.AccessibleName = "Decrypt button";
            _btnDecrypt.AccessibleDescription = "Decrypts the ciphertext back into plain text.";
            _btnDecrypt.Click += BtnDecrypt_Click;

            // _lblDecrypted
            _lblDecrypted.AutoSize = true;
            _lblDecrypted.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            _lblDecrypted.Margin = new Padding(3, 8, 3, 3);
            _lblDecrypted.Name = "_lblDecrypted";
            _lblDecrypted.Text = "Decrypted:";
            _lblDecrypted.AccessibleName = "Decrypted text label";

            // _txtDecrypted
            _txtDecrypted.Dock = DockStyle.Fill;
            _txtDecrypted.Margin = new Padding(3);
            _txtDecrypted.ReadOnly = true;
            _txtDecrypted.Name = "_txtDecrypted";
            _txtDecrypted.AccessibleName = "Decrypted text output";

            // _tlpGraph
            _tlpGraph.ColumnCount = 1;
            _tlpGraph.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tlpGraph.RowCount = 2;
            _tlpGraph.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _tlpGraph.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _tlpGraph.Dock = DockStyle.Fill;
            _tlpGraph.Controls.Add(_polarGraphControl, 0, 0);
            _tlpGraph.Controls.Add(_flpExport, 0, 1);
            _tlpGraph.Name = "_tlpGraph";
            _tlpGraph.Margin = new Padding(0, 8, 0, 0);

            // _polarGraphControl
            _polarGraphControl.Dock = DockStyle.Fill;
            _polarGraphControl.Margin = new Padding(3);
            _polarGraphControl.MinimumSize = new Size(300, 300);
            _polarGraphControl.Name = "_polarGraphControl";
            _polarGraphControl.AccessibleName = "Polar star map";
            _polarGraphControl.AccessibleDescription = "Displays the encrypted message plotted as stars on a polar graph.";

            // _flpExport
            _flpExport.AutoSize = true;
            _flpExport.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _flpExport.FlowDirection = FlowDirection.RightToLeft;
            _flpExport.Dock = DockStyle.Fill;
            _flpExport.Controls.Add(_btnExportPdf);
            _flpExport.Controls.Add(_chkConnectPoints);
            _flpExport.Controls.Add(_chkShowGrid);
            _flpExport.Controls.Add(_chkPolarisTheme);
            _flpExport.Controls.Add(_btnClearBackground);
            _flpExport.Controls.Add(_btnLoadBackground);
            _flpExport.Name = "_flpExport";

            // _chkConnectPoints
            _chkConnectPoints.AutoSize = true;
            _chkConnectPoints.Checked = true;
            _chkConnectPoints.CheckState = CheckState.Checked;
            _chkConnectPoints.Margin = new Padding(3, 10, 12, 3);
            _chkConnectPoints.Name = "_chkConnectPoints";
            _chkConnectPoints.Text = "Connect stars (constellation lines)";
            _chkConnectPoints.AccessibleName = "Connect stars checkbox";
            _chkConnectPoints.CheckedChanged += ChkConnectPoints_CheckedChanged;

            // _chkShowGrid
            _chkShowGrid.AutoSize = true;
            _chkShowGrid.Checked = true;
            _chkShowGrid.CheckState = CheckState.Checked;
            _chkShowGrid.Margin = new Padding(3, 10, 12, 3);
            _chkShowGrid.Name = "_chkShowGrid";
            _chkShowGrid.Text = "Show grid";
            _chkShowGrid.AccessibleName = "Show grid checkbox";
            _chkShowGrid.AccessibleDescription = "Shows or hides the polar axes and concentric grid circles.";
            _chkShowGrid.CheckedChanged += ChkShowGrid_CheckedChanged;

            // _chkPolarisTheme
            _chkPolarisTheme.AutoSize = true;
            _chkPolarisTheme.Margin = new Padding(3, 10, 12, 3);
            _chkPolarisTheme.Name = "_chkPolarisTheme";
            _chkPolarisTheme.Text = "Polaris star-chart theme";
            _chkPolarisTheme.AccessibleName = "Polaris star-chart theme checkbox";
            _chkPolarisTheme.AccessibleDescription = "Applies a night-sky backdrop, gold stars, silver constellation lines, a Polaris marker, and a compass rose.";
            _chkPolarisTheme.CheckedChanged += ChkPolarisTheme_CheckedChanged;

            // _btnClearBackground
            _btnClearBackground.AutoSize = true;
            _btnClearBackground.Margin = new Padding(3);
            _btnClearBackground.Name = "_btnClearBackground";
            _btnClearBackground.Text = "Clear Background";
            _btnClearBackground.AccessibleName = "Clear background image button";
            _btnClearBackground.AccessibleDescription = "Removes the background image from the star map.";
            _btnClearBackground.Click += BtnClearBackground_Click;

            // _btnLoadBackground
            _btnLoadBackground.AutoSize = true;
            _btnLoadBackground.Margin = new Padding(3);
            _btnLoadBackground.Name = "_btnLoadBackground";
            _btnLoadBackground.Text = "Load Background Image\u2026";
            _btnLoadBackground.AccessibleName = "Load background image button";
            _btnLoadBackground.AccessibleDescription = "Loads an image to display behind the plotted star map.";
            _btnLoadBackground.Click += BtnLoadBackground_Click;

            // _btnExportPdf
            _btnExportPdf.AutoSize = true;
            _btnExportPdf.Margin = new Padding(3);
            _btnExportPdf.Name = "_btnExportPdf";
            _btnExportPdf.Text = "Export Star Map to PDF\u2026";
            _btnExportPdf.AccessibleName = "Export to PDF button";
            _btnExportPdf.AccessibleDescription = "Saves the star map as a Letter-size (8.5 by 11 inch) PDF for printing.";
            _btnExportPdf.Click += BtnExportPdf_Click;

            // MainWindow
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(900, 700);
            MinimumSize = new Size(760, 560);
            Controls.Add(_tlpMain);
            Name = "MainWindow";
            Text = "Real-Star Polaris Cipher";

            _flpExport.ResumeLayout(false);
            _flpExport.PerformLayout();
            _tlpGraph.ResumeLayout(false);
            _flpDecrypt.ResumeLayout(false);
            _flpDecrypt.PerformLayout();
            _flpModeOptions.ResumeLayout(false);
            _flpModeOptions.PerformLayout();
            _grpMode.ResumeLayout(false);
            _grpMode.PerformLayout();
            _flpModeAndEncrypt.ResumeLayout(false);
            _flpModeAndEncrypt.PerformLayout();
            _tlpInput.ResumeLayout(false);
            _tlpInput.PerformLayout();
            _tlpMain.ResumeLayout(false);
            _tlpMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel _tlpMain;
        private TableLayoutPanel _tlpInput;
        private Label _lblPlainText;
        private TextBox _txtPlainText;
        private FlowLayoutPanel _flpModeAndEncrypt;
        private GroupBox _grpMode;
        private FlowLayoutPanel _flpModeOptions;
        private RadioButton _rbNumeric;
        private RadioButton _rbConstellation;
        private Button _btnEncrypt;
        private Label _lblCiphertext;
        private TextBox _txtCiphertext;
        private FlowLayoutPanel _flpDecrypt;
        private Button _btnDecrypt;
        private Label _lblDecrypted;
        private TextBox _txtDecrypted;
        private TableLayoutPanel _tlpGraph;
        private PolarGraphControl _polarGraphControl;
        private FlowLayoutPanel _flpExport;
        private CheckBox _chkConnectPoints;
        private CheckBox _chkShowGrid;
        private CheckBox _chkPolarisTheme;
        private Button _btnClearBackground;
        private Button _btnLoadBackground;
        private Button _btnExportPdf;
    }
}
