namespace TechMart
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            mnuExportCsv = new System.Windows.Forms.ToolStripMenuItem();
            mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            lblTongSP = new System.Windows.Forms.ToolStripStatusLabel();
            tableMain = new System.Windows.Forms.TableLayoutPanel();
            grpNhapLieu = new System.Windows.Forms.GroupBox();
            tableInput = new System.Windows.Forms.TableLayoutPanel();
            lblMa = new System.Windows.Forms.Label();
            txtProductId = new System.Windows.Forms.TextBox();
            lblTen = new System.Windows.Forms.Label();
            txtProductName = new System.Windows.Forms.TextBox();
            lblGia = new System.Windows.Forms.Label();
            txtUnitPrice = new System.Windows.Forms.TextBox();
            lblSoLuong = new System.Windows.Forms.Label();
            txtQuantity = new System.Windows.Forms.TextBox();
            lblDanhMuc = new System.Windows.Forms.Label();
            cboCategory = new System.Windows.Forms.ComboBox();
            btnChooseImage = new System.Windows.Forms.Button();
            picAvatar = new System.Windows.Forms.PictureBox();
            flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            btnThem = new System.Windows.Forms.Button();
            btnCapNhat = new System.Windows.Forms.Button();
            btnXoa = new System.Windows.Forms.Button();
            btnLamMoi = new System.Windows.Forms.Button();
            btnExport = new System.Windows.Forms.Button();
            tableRight = new System.Windows.Forms.TableLayoutPanel();
            pnlSearch = new System.Windows.Forms.Panel();
            txtSearch = new System.Windows.Forms.TextBox();
            lblSearch = new System.Windows.Forms.Label();
            dgvProducts = new System.Windows.Forms.DataGridView();
            errorProvider = new System.Windows.Forms.ErrorProvider(components);
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            tableMain.SuspendLayout();
            grpNhapLieu.SuspendLayout();
            tableInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            flowButtons.SuspendLayout();
            tableRight.SuspendLayout();
            pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFile });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(1100, 24);
            menuStrip1.TabIndex = 0;
            // 
            // mnuFile
            // 
            mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuExportCsv, mnuExit });
            mnuFile.Name = "mnuFile";
            mnuFile.Size = new System.Drawing.Size(37, 20);
            mnuFile.Text = "File";
            // 
            // mnuExportCsv
            // 
            mnuExportCsv.Name = "mnuExportCsv";
            mnuExportCsv.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E;
            mnuExportCsv.Size = new System.Drawing.Size(172, 22);
            mnuExportCsv.Text = "Export CSV";
            mnuExportCsv.Click += mnuExportCsv_Click;
            // 
            // mnuExit
            // 
            mnuExit.Name = "mnuExit";
            mnuExit.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X;
            mnuExit.Size = new System.Drawing.Size(172, 22);
            mnuExit.Text = "Exit";
            mnuExit.Click += mnuExit_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { lblTongSP });
            statusStrip1.Location = new System.Drawing.Point(0, 628);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(1100, 22);
            statusStrip1.TabIndex = 2;
            // 
            // lblTongSP
            // 
            lblTongSP.Name = "lblTongSP";
            lblTongSP.Size = new System.Drawing.Size(116, 17);
            lblTongSP.Text = "Tổng số sản phẩm: 0";
            // 
            // tableMain
            // 
            tableMain.ColumnCount = 2;
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            tableMain.Controls.Add(grpNhapLieu, 0, 0);
            tableMain.Controls.Add(tableRight, 1, 0);
            tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableMain.Location = new System.Drawing.Point(0, 24);
            tableMain.Name = "tableMain";
            tableMain.Padding = new System.Windows.Forms.Padding(5);
            tableMain.RowCount = 1;
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableMain.Size = new System.Drawing.Size(1100, 604);
            tableMain.TabIndex = 1;
            // 
            // grpNhapLieu
            // 
            grpNhapLieu.Controls.Add(tableInput);
            grpNhapLieu.Dock = System.Windows.Forms.DockStyle.Fill;
            grpNhapLieu.Location = new System.Drawing.Point(8, 8);
            grpNhapLieu.Name = "grpNhapLieu";
            grpNhapLieu.Size = new System.Drawing.Size(375, 588);
            grpNhapLieu.TabIndex = 0;
            grpNhapLieu.TabStop = false;
            grpNhapLieu.Text = "Thông tin sản phẩm";
            // 
            // tableInput
            // 
            tableInput.ColumnCount = 2;
            tableInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            tableInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68F));
            tableInput.Controls.Add(lblMa, 0, 0);
            tableInput.Controls.Add(txtProductId, 1, 0);
            tableInput.Controls.Add(lblTen, 0, 1);
            tableInput.Controls.Add(txtProductName, 1, 1);
            tableInput.Controls.Add(lblGia, 0, 2);
            tableInput.Controls.Add(txtUnitPrice, 1, 2);
            tableInput.Controls.Add(lblSoLuong, 0, 3);
            tableInput.Controls.Add(txtQuantity, 1, 3);
            tableInput.Controls.Add(lblDanhMuc, 0, 4);
            tableInput.Controls.Add(cboCategory, 1, 4);
            tableInput.Controls.Add(btnChooseImage, 0, 5);
            tableInput.Controls.Add(picAvatar, 1, 5);
            tableInput.Controls.Add(flowButtons, 0, 6);
            tableInput.Dock = System.Windows.Forms.DockStyle.Fill;
            tableInput.Location = new System.Drawing.Point(3, 19);
            tableInput.Name = "tableInput";
            tableInput.RowCount = 7;
            tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            tableInput.Size = new System.Drawing.Size(369, 566);
            tableInput.TabIndex = 0;
            // 
            // lblMa
            // 
            lblMa.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblMa.AutoSize = true;
            lblMa.Location = new System.Drawing.Point(3, 10);
            lblMa.Name = "lblMa";
            lblMa.Size = new System.Drawing.Size(43, 15);
            lblMa.TabIndex = 0;
            lblMa.Text = "Mã SP:";
            // 
            // txtProductId
            // 
            txtProductId.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtProductId.Location = new System.Drawing.Point(121, 6);
            txtProductId.Name = "txtProductId";
            txtProductId.Size = new System.Drawing.Size(245, 23);
            txtProductId.TabIndex = 0;
            // 
            // lblTen
            // 
            lblTen.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblTen.AutoSize = true;
            lblTen.Location = new System.Drawing.Point(3, 45);
            lblTen.Name = "lblTen";
            lblTen.Size = new System.Drawing.Size(44, 15);
            lblTen.TabIndex = 1;
            lblTen.Text = "Tên SP:";
            // 
            // txtProductName
            // 
            txtProductName.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtProductName.Location = new System.Drawing.Point(121, 41);
            txtProductName.Name = "txtProductName";
            txtProductName.Size = new System.Drawing.Size(245, 23);
            txtProductName.TabIndex = 1;
            // 
            // lblGia
            // 
            lblGia.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblGia.AutoSize = true;
            lblGia.Location = new System.Drawing.Point(3, 80);
            lblGia.Name = "lblGia";
            lblGia.Size = new System.Drawing.Size(51, 15);
            lblGia.TabIndex = 2;
            lblGia.Text = "Đơn giá:";
            // 
            // txtUnitPrice
            // 
            txtUnitPrice.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtUnitPrice.Location = new System.Drawing.Point(121, 76);
            txtUnitPrice.Name = "txtUnitPrice";
            txtUnitPrice.Size = new System.Drawing.Size(245, 23);
            txtUnitPrice.TabIndex = 2;
            // 
            // lblSoLuong
            // 
            lblSoLuong.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblSoLuong.AutoSize = true;
            lblSoLuong.Location = new System.Drawing.Point(3, 115);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new System.Drawing.Size(57, 15);
            lblSoLuong.TabIndex = 3;
            lblSoLuong.Text = "Số lượng:";
            // 
            // txtQuantity
            // 
            txtQuantity.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtQuantity.Location = new System.Drawing.Point(121, 111);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new System.Drawing.Size(245, 23);
            txtQuantity.TabIndex = 3;
            // 
            // lblDanhMuc
            // 
            lblDanhMuc.Anchor = System.Windows.Forms.AnchorStyles.Left;
            lblDanhMuc.AutoSize = true;
            lblDanhMuc.Location = new System.Drawing.Point(3, 150);
            lblDanhMuc.Name = "lblDanhMuc";
            lblDanhMuc.Size = new System.Drawing.Size(65, 15);
            lblDanhMuc.TabIndex = 4;
            lblDanhMuc.Text = "Danh mục:";
            // 
            // cboCategory
            // 
            cboCategory.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboCategory.FormattingEnabled = true;
            cboCategory.Location = new System.Drawing.Point(121, 146);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new System.Drawing.Size(245, 23);
            cboCategory.TabIndex = 4;
            // 
            // btnChooseImage
            // 
            btnChooseImage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            btnChooseImage.Location = new System.Drawing.Point(19, 178);
            btnChooseImage.Name = "btnChooseImage";
            btnChooseImage.Size = new System.Drawing.Size(80, 30);
            btnChooseImage.TabIndex = 5;
            btnChooseImage.Text = "Chọn ảnh";
            btnChooseImage.UseVisualStyleBackColor = true;
            btnChooseImage.Click += btnChooseImage_Click;
            // 
            // picAvatar
            // 
            picAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picAvatar.Dock = System.Windows.Forms.DockStyle.Fill;
            picAvatar.Location = new System.Drawing.Point(121, 178);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new System.Drawing.Size(245, 295);
            picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            picAvatar.TabIndex = 6;
            picAvatar.TabStop = false;
            // 
            // flowButtons
            // 
            tableInput.SetColumnSpan(flowButtons, 2);
            flowButtons.Controls.Add(btnThem);
            flowButtons.Controls.Add(btnCapNhat);
            flowButtons.Controls.Add(btnXoa);
            flowButtons.Controls.Add(btnLamMoi);
            flowButtons.Controls.Add(btnExport);
            flowButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            flowButtons.Location = new System.Drawing.Point(3, 479);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new System.Drawing.Size(363, 84);
            flowButtons.TabIndex = 6;
            // 
            // btnThem
            // 
            btnThem.Location = new System.Drawing.Point(3, 3);
            btnThem.Name = "btnThem";
            btnThem.Size = new System.Drawing.Size(75, 30);
            btnThem.TabIndex = 0;
            btnThem.Text = "Thêm mới";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnCapNhat
            // 
            btnCapNhat.Location = new System.Drawing.Point(84, 3);
            btnCapNhat.Name = "btnCapNhat";
            btnCapNhat.Size = new System.Drawing.Size(75, 30);
            btnCapNhat.TabIndex = 1;
            btnCapNhat.Text = "Cập nhật";
            btnCapNhat.UseVisualStyleBackColor = true;
            btnCapNhat.Click += btnCapNhat_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new System.Drawing.Point(165, 3);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new System.Drawing.Size(75, 30);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new System.Drawing.Point(246, 3);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new System.Drawing.Size(75, 30);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new System.Drawing.Point(3, 39);
            btnExport.Name = "btnExport";
            btnExport.Size = new System.Drawing.Size(75, 30);
            btnExport.TabIndex = 4;
            btnExport.Text = "Xuất CSV";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // tableRight
            // 
            tableRight.ColumnCount = 1;
            tableRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableRight.Controls.Add(pnlSearch, 0, 0);
            tableRight.Controls.Add(dgvProducts, 0, 1);
            tableRight.Dock = System.Windows.Forms.DockStyle.Fill;
            tableRight.Location = new System.Drawing.Point(389, 8);
            tableRight.Name = "tableRight";
            tableRight.RowCount = 2;
            tableRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableRight.Size = new System.Drawing.Size(703, 588);
            tableRight.TabIndex = 1;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlSearch.Location = new System.Drawing.Point(3, 3);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            pnlSearch.Size = new System.Drawing.Size(697, 29);
            pnlSearch.TabIndex = 0;
            // 
            // txtSearch
            // 
            txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            txtSearch.Location = new System.Drawing.Point(85, 6);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(612, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Dock = System.Windows.Forms.DockStyle.Left;
            lblSearch.Location = new System.Drawing.Point(0, 6);
            lblSearch.Name = "lblSearch";
            lblSearch.Padding = new System.Windows.Forms.Padding(0, 3, 8, 0);
            lblSearch.Size = new System.Drawing.Size(85, 18);
            lblSearch.TabIndex = 1;
            lblSearch.Text = "Tìm theo tên:";
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.BackgroundColor = System.Drawing.SystemColors.Window;
            dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvProducts.Location = new System.Drawing.Point(3, 38);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new System.Drawing.Size(697, 547);
            dgvProducts.TabIndex = 1;
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            errorProvider.ContainerControl = this;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1100, 650);
            Controls.Add(tableMain);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new System.Drawing.Size(850, 520);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "TechMart Product Manager";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tableMain.ResumeLayout(false);
            grpNhapLieu.ResumeLayout(false);
            tableInput.ResumeLayout(false);
            tableInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            flowButtons.ResumeLayout(false);
            tableRight.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuExportCsv;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblTongSP;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.GroupBox grpNhapLieu;
        private System.Windows.Forms.TableLayoutPanel tableInput;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.TextBox txtProductId;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.Label lblSoLuong;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label lblDanhMuc;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.FlowLayoutPanel flowButtons;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TableLayoutPanel tableRight;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDanhMuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
