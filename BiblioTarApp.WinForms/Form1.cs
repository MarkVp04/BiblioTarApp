namespace BiblioTarApp.WinForms;

public partial class Form1 : Form
{
    private static readonly Color AppBackground = Color.FromArgb(245, 247, 250);
    private static readonly Color SurfaceBackground = Color.White;
    private static readonly Color PrimaryColor = Color.FromArgb(37, 99, 235);
    private static readonly Color TextColor = Color.FromArgb(31, 41, 55);
    private static readonly Color MutedTextColor = Color.FromArgb(75, 85, 99);
    private static readonly Font BaseFont = new("Segoe UI", 10F, FontStyle.Regular);
    private static readonly Font HeaderFont = new("Segoe UI", 13F, FontStyle.Bold);

    private Label _statusLabel = null!;
    private ComboBox _roleSelector = null!;
    private TabControl _mainTabs = null!;
    private TabPage _authTab = null!;
    private TabPage _userTab = null!;
    private TabPage _librarianTab = null!;
    private TabPage _adminTab = null!;

    public Form1()
    {
        InitializeComponent();
        BuildUiStructure();
    }

    private void BuildUiStructure()
    {
        var root = CreateRootLayout();
        Controls.Add(root);

        root.Controls.Add(CreateHeaderSection(), 0, 0);

        _authTab = BuildAuthTab();
        _userTab = BuildUserTab();
        _librarianTab = BuildLibrarianTab();
        _adminTab = BuildAdminTab();

        _mainTabs = new TabControl { Dock = DockStyle.Fill };
        root.Controls.Add(_mainTabs, 0, 1);

        _statusLabel = new Label
        {
            AutoSize = true,
            Text = "UI vaz kesz. Funkciok implementalasa kesobb.",
            Margin = new Padding(3, 8, 3, 8)
        };
        root.Controls.Add(_statusLabel, 0, 2);

        ApplyTheme(root);
        ApplyRoleTabs("Felhasznalo");
    }

    private static TableLayoutPanel CreateRootLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(12)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        return root;
    }

    private Control CreateHeaderSection()
    {
        var headerPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            Margin = new Padding(0, 0, 0, 8)
        };
        headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        headerPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        headerPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var titlePanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 1
        };
        titlePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        titlePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        titlePanel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = HeaderFont,
            Text = "BiblioTarApp - Teljes GUI vaz"
        }, 0, 0);

        titlePanel.Controls.Add(new Label
        {
            AutoSize = true,
            Text = "Szerepkoronkent elokeszitett feluletek, backend funkcionalitas nelkul."
        }, 0, 1);

        var rolePanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(0),
            Margin = new Padding(12, 0, 0, 0)
        };
        rolePanel.Controls.Add(new Label { AutoSize = true, Text = "Aktiv szerepkor" });
        _roleSelector = new ComboBox
        {
            Width = 180,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _roleSelector.Items.AddRange(new object[] { "Felhasznalo", "Konyvtaros", "Adminisztrator" });
        _roleSelector.SelectedIndex = 0;
        _roleSelector.SelectedIndexChanged += (_, _) => ApplyRoleTabs(_roleSelector.SelectedItem?.ToString());
        rolePanel.Controls.Add(_roleSelector);

        headerPanel.Controls.Add(titlePanel, 0, 0);
        headerPanel.SetRowSpan(titlePanel, 2);
        headerPanel.Controls.Add(rolePanel, 1, 0);
        return headerPanel;
    }

    private void ApplyRoleTabs(string? role)
    {
        if (_mainTabs is null)
        {
            return;
        }

        var selectedRole = string.IsNullOrWhiteSpace(role) ? "Felhasznalo" : role;

        _mainTabs.SuspendLayout();
        _mainTabs.TabPages.Clear();
        _mainTabs.TabPages.Add(_authTab);

        switch (selectedRole)
        {
            case "Konyvtaros":
                _mainTabs.TabPages.Add(_librarianTab);
                _mainTabs.SelectedTab = _librarianTab;
                _statusLabel.Text = "Aktiv nezet: Konyvtaros";
                break;
            case "Adminisztrator":
                _mainTabs.TabPages.Add(_adminTab);
                _mainTabs.SelectedTab = _adminTab;
                _statusLabel.Text = "Aktiv nezet: Adminisztrator";
                break;
            default:
                _mainTabs.TabPages.Add(_userTab);
                _mainTabs.SelectedTab = _userTab;
                _statusLabel.Text = "Aktiv nezet: Felhasznalo";
                break;
        }

        _mainTabs.ResumeLayout();
    }

    private TabPage BuildAuthTab()
    {
        var tab = new TabPage("Bejelentkezes / Regisztracio");
        var layout = CreateTwoColumnLayout();
        tab.Controls.Add(layout);

        var loginGroup = new GroupBox { Text = "Bejelentkezes", Dock = DockStyle.Fill };
        var loginForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(12) };
        loginForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        loginForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        loginForm.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        loginForm.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        loginForm.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        loginForm.Controls.Add(new Label { Text = "Email", AutoSize = true }, 0, 0);
        loginForm.Controls.Add(new TextBox { Dock = DockStyle.Top }, 1, 0);
        loginForm.Controls.Add(new Label { Text = "Jelszo", AutoSize = true }, 0, 1);
        loginForm.Controls.Add(new TextBox { Dock = DockStyle.Top, UseSystemPasswordChar = true }, 1, 1);
        loginForm.Controls.Add(CreateActionButton("Bejelentkezes"), 1, 2);
        loginGroup.Controls.Add(loginForm);

        var registerGroup = new GroupBox { Text = "Regisztracio", Dock = DockStyle.Fill };
        var registerForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(12) };
        registerForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        registerForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        AddRow(registerForm, "Nev", new TextBox());
        AddRow(registerForm, "Email", new TextBox());
        AddRow(registerForm, "Telefon", new TextBox());
        AddRow(registerForm, "Jelszo", new TextBox { UseSystemPasswordChar = true });
        AddRow(registerForm, "Jelszo ujra", new TextBox { UseSystemPasswordChar = true });
        AddRow(registerForm, "Szerepkor", new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = new[] { "Felhasznalo", "Konyvtaros", "Adminisztrator" }
        });
        AddRow(registerForm, "", CreateActionButton("Regisztracio"));

        registerGroup.Controls.Add(registerForm);

        layout.Controls.Add(loginGroup, 0, 0);
        layout.Controls.Add(registerGroup, 1, 0);
        return tab;
    }

    private TabPage BuildUserTab()
    {
        var tab = new TabPage("Felhasznalo");
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(8) };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
        tab.Controls.Add(layout);

        var booksGroup = new GroupBox { Text = "Konyvkereses es elojegyzes", Dock = DockStyle.Fill };
        var booksPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(8) };
        booksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        booksPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        booksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        booksPanel.Controls.Add(new TextBox { PlaceholderText = "Kereses cim/szerzo/kategoria alapjan..." }, 0, 0);
        booksPanel.Controls.Add(CreateSampleGrid(new[] { "Cim", "Szerzo", "Kiadas eve", "Elerheto" }), 0, 1);
        booksPanel.Controls.Add(CreateButtonsRow("Kereses", "Reszletek", "Elojegyzes"), 0, 2);
        booksGroup.Controls.Add(booksPanel);

        var profileGroup = new GroupBox { Text = "Sajat adatok", Dock = DockStyle.Fill };
        var profileForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(10) };
        profileForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        profileForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddRow(profileForm, "Nev", new TextBox());
        AddRow(profileForm, "Email", new TextBox());
        AddRow(profileForm, "Telefon", new TextBox());
        AddRow(profileForm, "Lakcim", new TextBox());
        AddRow(profileForm, "", CreateActionButton("Adatok mentese"));
        profileGroup.Controls.Add(profileForm);

        var historyGroup = new GroupBox { Text = "Kolcsonzesi elozmenyek", Dock = DockStyle.Fill };
        var historyPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(8) };
        historyPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        historyPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        historyPanel.Controls.Add(CreateSampleGrid(new[] { "Konyv", "Kolcsonzes datuma", "Hatarido", "Statusz" }), 0, 0);
        historyPanel.Controls.Add(CreateButtonsRow("Frissites", "Hosszabbitas (max 2x)"), 0, 1);
        historyGroup.Controls.Add(historyPanel);

        layout.Controls.Add(booksGroup, 0, 0);
        layout.Controls.Add(profileGroup, 0, 1);
        layout.Controls.Add(historyGroup, 0, 2);
        return tab;
    }

    private TabPage BuildLibrarianTab()
    {
        var tab = new TabPage("Konyvtaros");
        var layout = CreateTwoColumnLayout();
        tab.Controls.Add(layout);

        var loanGroup = new GroupBox { Text = "Kolcsonzes kezeles", Dock = DockStyle.Fill };
        var loanForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(12) };
        loanForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        loanForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddRow(loanForm, "Felhasznalo azonosito", new TextBox());
        AddRow(loanForm, "Konyv azonosito", new TextBox());
        AddRow(loanForm, "Hatarido", new DateTimePicker());
        AddRow(loanForm, "", CreateButtonsRow("Kolcsonzes rogzitese", "Visszavetel"));
        AddRow(loanForm, "", CreateButtonsRow("Hosszabbitas engedelyezese", "Hosszabbitas elutasitasa"));
        loanGroup.Controls.Add(loanForm);

        var fineGroup = new GroupBox { Text = "Birsag kezeles", Dock = DockStyle.Fill };
        var fineForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(12) };
        fineForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        fineForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddRow(fineForm, "Felhasznalo azonosito", new TextBox());
        AddRow(fineForm, "Osszeg", new TextBox());
        AddRow(fineForm, "Megjegyzes", new TextBox());
        AddRow(fineForm, "", CreateButtonsRow("Birsag kiszabasa", "Birsag torlese"));
        fineForm.Controls.Add(new Label { Text = "Kolcsonzesek", AutoSize = true }, 0, 4);
        fineForm.Controls.Add(CreateSampleGrid(new[] { "Felhasznalo", "Konyv", "Hatarido", "Keses napok" }), 1, 4);
        fineGroup.Controls.Add(fineForm);

        layout.Controls.Add(loanGroup, 0, 0);
        layout.Controls.Add(fineGroup, 1, 0);
        return tab;
    }

    private TabPage BuildAdminTab()
    {
        var tab = new TabPage("Adminisztrator");
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(8) };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 56));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 44));
        tab.Controls.Add(layout);

        var stockGroup = new GroupBox { Text = "Konyvallomany kezeles", Dock = DockStyle.Fill };
        var stockPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(8) };
        stockPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stockPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        stockPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stockPanel.Controls.Add(CreateButtonsRow("Uj konyv", "Modositas", "Torles", "Frissites"), 0, 0);
        stockPanel.Controls.Add(CreateSampleGrid(new[] { "Id", "Cim", "Szerzo", "Kategoria", "Allapot", "Kolcsonozheto" }), 0, 1);
        stockPanel.Controls.Add(CreateButtonsRow("Konyv allapot: Jo", "Konyv allapot: Serult", "Konyv allapot: Elveszett"), 0, 2);
        stockGroup.Controls.Add(stockPanel);

        var detailsGroup = new GroupBox { Text = "Konyv adatok", Dock = DockStyle.Fill };
        var detailsForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(12) };
        detailsForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        detailsForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddRow(detailsForm, "Cim", new TextBox());
        AddRow(detailsForm, "Szerzo", new TextBox());
        AddRow(detailsForm, "ISBN", new TextBox());
        AddRow(detailsForm, "Kategoria", new TextBox());
        AddRow(detailsForm, "Kiadas eve", new TextBox());
        AddRow(detailsForm, "Kolcsonozheto", new CheckBox { Text = "Igen", AutoSize = true });
        AddRow(detailsForm, "Allapot", new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = new[] { "Jo", "Serult", "Elveszett" }
        });
        AddRow(detailsForm, "", CreateButtonsRow("Ment", "Reset"));
        detailsGroup.Controls.Add(detailsForm);

        layout.Controls.Add(stockGroup, 0, 0);
        layout.Controls.Add(detailsGroup, 0, 1);
        return tab;
    }

    private static TableLayoutPanel CreateTwoColumnLayout()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(8)
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        return layout;
    }

    private static Control CreateButtonsRow(params string[] captions)
    {
        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            WrapContents = true,
            Margin = new Padding(0, 6, 0, 0),
            Padding = new Padding(0),
            FlowDirection = FlowDirection.LeftToRight
        };

        foreach (var caption in captions)
        {
            flow.Controls.Add(CreateActionButton(caption));
        }

        return flow;
    }

    private static Button CreateActionButton(string text)
    {
        var button = new Button
        {
            AutoSize = true,
            Text = text,
            Padding = new Padding(10, 5, 10, 5),
            Margin = new Padding(0, 0, 8, 8),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = PrimaryColor
        };

        button.FlatAppearance.BorderSize = 0;
        button.Click += (_, _) =>
            MessageBox.Show("Ez a funkcio meg nincs implementalva.", "GUI vaz", MessageBoxButtons.OK, MessageBoxIcon.Information);

        return button;
    }

    private static DataGridView CreateSampleGrid(string[] columns)
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        grid.BackgroundColor = SurfaceBackground;
        grid.BorderStyle = BorderStyle.None;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 246, 255);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font(BaseFont, FontStyle.Bold);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
        grid.DefaultCellStyle.SelectionForeColor = Color.Black;
        grid.RowHeadersVisible = false;

        foreach (var column in columns)
        {
            grid.Columns.Add(column, column);
        }

        grid.Rows.Add(columns.Select(_ => "-").ToArray());
        return grid;
    }

    private static void AddRow(TableLayoutPanel panel, string labelText, Control control)
    {
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var rowIndex = panel.RowCount++;
        panel.Controls.Add(new Label { Text = labelText, AutoSize = true, Margin = new Padding(3, 10, 10, 8) }, 0, rowIndex);
        control.Dock = DockStyle.Top;
        control.Margin = new Padding(3, 6, 3, 6);
        panel.Controls.Add(control, 1, rowIndex);
    }

    private static void ApplyTheme(Control root)
    {
        root.Font = BaseFont;
        root.BackColor = AppBackground;
        root.ForeColor = TextColor;
        ApplyThemeRecursive(root);
    }

    private static void ApplyThemeRecursive(Control parent)
    {
        foreach (Control control in parent.Controls)
        {
            control.Font = BaseFont;
            control.ForeColor = TextColor;

            switch (control)
            {
                case GroupBox:
                    control.BackColor = SurfaceBackground;
                    control.Padding = new Padding(10);
                    control.Margin = new Padding(6);
                    break;
                case TableLayoutPanel:
                    control.BackColor = AppBackground;
                    break;
                case TabControl tabControl:
                    tabControl.Padding = new Point(16, 8);
                    tabControl.BackColor = AppBackground;
                    break;
                case TabPage tabPage:
                    tabPage.BackColor = AppBackground;
                    break;
                case Label label:
                    label.BackColor = Color.Transparent;
                    if (label == parent.Controls[0] && parent is TableLayoutPanel)
                    {
                        label.ForeColor = TextColor;
                    }

                    if (label.Text.Contains("backend funkcionalitas", StringComparison.OrdinalIgnoreCase))
                    {
                        label.ForeColor = MutedTextColor;
                    }

                    break;
                case TextBox textBox:
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case ComboBox comboBox:
                    comboBox.FlatStyle = FlatStyle.Flat;
                    break;
                case DateTimePicker picker:
                    picker.CalendarTitleBackColor = Color.FromArgb(219, 234, 254);
                    break;
            }

            ApplyThemeRecursive(control);
        }
    }
}
