namespace BiblioTarApp.WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        BuildGuiSkeleton();
    }

    private void BuildGuiSkeleton()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(12)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(root);

        var header = new Label
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            Font = new Font("Segoe UI", 13, FontStyle.Bold),
            Text = "BiblioTarApp - teljes GUI vaz (funkciok kesobb)"
        };
        root.Controls.Add(header, 0, 0);

        var tabs = new TabControl { Dock = DockStyle.Fill };
        root.Controls.Add(tabs, 0, 1);

        tabs.TabPages.Add(BuildAuthTab());
        tabs.TabPages.Add(BuildUserTab());
        tabs.TabPages.Add(BuildLibrarianTab());
        tabs.TabPages.Add(BuildAdminTab());
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
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
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
            Padding = new Padding(8, 4, 8, 4)
        };

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
        panel.Controls.Add(new Label { Text = labelText, AutoSize = true, Margin = new Padding(3, 8, 3, 8) }, 0, rowIndex);
        control.Dock = DockStyle.Top;
        panel.Controls.Add(control, 1, rowIndex);
    }
}
