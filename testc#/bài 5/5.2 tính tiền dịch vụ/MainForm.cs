namespace App;

public class Service
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Service(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} - {Price:N0} đ";
    }
}

public class MainForm : Form
{
    private ComboBox cboCategory;
    private ListBox lstAvailableServices, lstSelectedServices;
    private Button btnSelect, btnRemove, btnClearAll, btnApplyCoupon;
    private TextBox txtCoupon, txtTotal, txtDiscount, txtPayable;
    private decimal discountPercent = 0;

    private readonly Dictionary<string, List<Service>> catalog = new()
    {
        ["Khám bệnh"] = new List<Service>
        {
            new Service("Khám nội tổng quát", 150000),
            new Service("Khám tai mũi họng", 120000),
            new Service("Khám da liễu", 130000),
            new Service("Khám tim mạch", 250000)
        },
        ["Xét nghiệm"] = new List<Service>
        {
            new Service("Xét nghiệm máu tổng quát", 180000),
            new Service("Xét nghiệm đường huyết", 60000),
            new Service("Xét nghiệm mỡ máu", 120000),
            new Service("Xét nghiệm nước tiểu", 70000)
        },
        ["Chụp X-Quang"] = new List<Service>
        {
            new Service("X-Quang phổi", 200000),
            new Service("X-Quang cột sống", 280000),
            new Service("X-Quang xương khớp", 220000)
        },
        ["Vắc-xin"] = new List<Service>
        {
            new Service("Vắc-xin cúm mùa", 350000),
            new Service("Vắc-xin viêm gan B", 300000),
            new Service("Vắc-xin uốn ván", 180000),
            new Service("Vắc-xin HPV", 1800000)
        }
    };

    private readonly Dictionary<string, decimal> coupons = new(StringComparer.OrdinalIgnoreCase)
    {
        ["GIAM5"] = 5,
        ["GIAM10"] = 10,
        ["GIAM20"] = 20
    };

    public MainForm()
    {
        Text = "Bảng tính tiền dịch vụ";
        ClientSize = new Size(820, 560);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10F);

        Controls.Add(new Label { Text = "Loại dịch vụ:", Location = new Point(20, 22), AutoSize = true });
        cboCategory = new ComboBox { Location = new Point(130, 18), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
        cboCategory.Items.AddRange(catalog.Keys.ToArray());
        cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;

        Controls.Add(new Label { Text = "Dịch vụ có sẵn", Location = new Point(20, 60), AutoSize = true });
        Controls.Add(new Label { Text = "Dịch vụ đã chọn", Location = new Point(450, 60), AutoSize = true });

        lstAvailableServices = new ListBox
        {
            Location = new Point(20, 88),
            Size = new Size(340, 260),
            SelectionMode = SelectionMode.MultiExtended
        };
        lstSelectedServices = new ListBox
        {
            Location = new Point(450, 88),
            Size = new Size(350, 260),
            SelectionMode = SelectionMode.MultiExtended
        };
        lstAvailableServices.DoubleClick += (s, e) => MoveSelected();

        btnSelect = new Button { Text = ">", Location = new Point(375, 120), Size = new Size(60, 36) };
        btnRemove = new Button { Text = "<", Location = new Point(375, 170), Size = new Size(60, 36) };
        btnClearAll = new Button { Text = "<<", Location = new Point(375, 220), Size = new Size(60, 36) };
        btnSelect.Click += (s, e) => MoveSelected();
        btnRemove.Click += BtnRemove_Click;
        btnClearAll.Click += BtnClearAll_Click;

        var grpPayment = new GroupBox { Text = "Thanh toán", Location = new Point(20, 365), Size = new Size(780, 175) };
        AddLabel(grpPayment, "Tổng tiền chưa giảm:", 15, 35);
        AddLabel(grpPayment, "Mã giảm giá:", 15, 75);
        AddLabel(grpPayment, "Tỷ lệ chiết khấu (%):", 15, 115);
        AddLabel(grpPayment, "Thành tiền thanh toán:", 400, 35);

        txtTotal = new TextBox { Location = new Point(190, 32), Width = 180, ReadOnly = true, TextAlign = HorizontalAlignment.Right, Text = "0 đ" };
        txtCoupon = new TextBox { Location = new Point(190, 72), Width = 110 };
        btnApplyCoupon = new Button { Text = "Áp dụng", Location = new Point(310, 70), Size = new Size(80, 30) };
        txtDiscount = new TextBox { Location = new Point(190, 112), Width = 180, ReadOnly = true, TextAlign = HorizontalAlignment.Right, Text = "0" };
        txtPayable = new TextBox
        {
            Location = new Point(400, 72),
            Width = 360,
            Height = 40,
            ReadOnly = true,
            TextAlign = HorizontalAlignment.Right,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.Firebrick,
            Text = "0 đ"
        };
        btnApplyCoupon.Click += BtnApplyCoupon_Click;
        grpPayment.Controls.AddRange(new Control[] { txtTotal, txtCoupon, btnApplyCoupon, txtDiscount, txtPayable });

        Controls.AddRange(new Control[] { cboCategory, lstAvailableServices, lstSelectedServices, btnSelect, btnRemove, btnClearAll, grpPayment });

        cboCategory.SelectedIndex = 0;
    }

    private static void AddLabel(Control parent, string text, int x, int y)
    {
        parent.Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true });
    }

    private void CboCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstAvailableServices.Items.Clear();
        if (cboCategory.SelectedItem is string key)
        {
            foreach (var service in catalog[key])
                lstAvailableServices.Items.Add(service);
        }
    }

    private void MoveSelected()
    {
        foreach (var item in lstAvailableServices.SelectedItems.Cast<Service>().ToList())
        {
            bool exists = lstSelectedServices.Items.Cast<Service>().Any(s => s.Name == item.Name);
            if (!exists)
                lstSelectedServices.Items.Add(item);
        }
        Recalculate();
    }

    private void BtnRemove_Click(object sender, EventArgs e)
    {
        foreach (var item in lstSelectedServices.SelectedItems.Cast<Service>().ToList())
            lstSelectedServices.Items.Remove(item);
        Recalculate();
    }

    private void BtnClearAll_Click(object sender, EventArgs e)
    {
        lstSelectedServices.Items.Clear();
        Recalculate();
    }

    private void BtnApplyCoupon_Click(object sender, EventArgs e)
    {
        string code = txtCoupon.Text.Trim();
        if (coupons.TryGetValue(code, out decimal percent))
        {
            discountPercent = percent;
            MessageBox.Show($"Áp dụng mã thành công, giảm {percent}%.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            discountPercent = 0;
            MessageBox.Show("Mã giảm giá không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        Recalculate();
    }

    private void Recalculate()
    {
        decimal total = lstSelectedServices.Items.Cast<Service>().Sum(s => s.Price);
        decimal payable = total - total * discountPercent / 100m;
        txtTotal.Text = $"{total:N0} đ";
        txtDiscount.Text = discountPercent.ToString("0.##");
        txtPayable.Text = $"{payable:N0} đ";
    }
}
