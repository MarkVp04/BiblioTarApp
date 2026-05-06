using BiblioTarApp.DTOs;
using System.Windows.Forms;

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
    private DataGridView _librarianFoglalasokGrid = null!;
    private ComboBox _roleSelector = null!;
    private TabControl _mainTabs = null!;
    private TabPage _authTab = null!;
    private TabPage _userTab = null!;
    private TabPage _librarianTab = null!;
    private TabPage _adminTab = null!;
    private TextBox _loginEmailInput = null!;
    private TextBox _loginPasswordInput = null!;
    private Label _loginValidationLabel = null!;
    private TextBox _registerNameInput = null!;
    private TextBox _registerEmailInput = null!;
    private TextBox _registerPhoneInput = null!;
    private TextBox _registerPasswordInput = null!;
    private TextBox _registerPasswordAgainInput = null!;
    private ComboBox _registerRoleInput = null!;
    private Label _registerValidationLabel = null!;
    private TextBox _userSearchInput = null!;
    private DataGridView _userBooksGrid = null!;
    private DataGridView _userHistoryGrid = null!;
    private Label _userBooksInfoLabel = null!;
    private Label _userHistoryInfoLabel = null!;
    private readonly List<UserBookMockItem> _userBooksData = new();
    private readonly List<UserHistoryMockItem> _userHistoryData = new();
    private TextBox _librarianUserIdInput = null!;
    private TextBox _librarianBookIdInput = null!;
    private DateTimePicker _librarianDeadlinePicker = null!;
    private Label _librarianLoanFeedbackLabel = null!;
    private TextBox _librarianFineUserInput = null!;
    private TextBox _librarianFineAmountInput = null!;
    private TextBox _librarianFineNoteInput = null!;
    private Label _librarianFineFeedbackLabel = null!;
    private DataGridView _librarianLoansGrid = null!;
    private DataGridView _adminStockGrid = null!;
    private TextBox _adminCimInput = null!;
    private TextBox _adminSzerzoInput = null!;
    private TextBox _adminIsbnInput = null!;
    private TextBox _adminKategoriaInput = null!;
    private TextBox _adminKiadasevInput = null!;
    private CheckBox _adminKolcsonozhetoCheck = null!;
    private ComboBox _adminAllapotCombo = null!;
    private Label _adminDetailFeedbackLabel = null!;
    private Label _adminStockInfoLabel = null!;
    private bool _adminSuppressSelectionChanged;
    private bool _adminIsNewBookMode;
    private int _nextAdminBookId = 1;
    private TextBox _registerConfirmPasswordInput = null!;
    private readonly List<KonyvDto> _adminBooksData = new();
    private List<KonyvDto> _allBooksFromApi = new();
    private int _currentUserId;
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
        //SeedUserMockData();
        LoadUserBooksFromApiAsync();
        RefreshUserBooksGrid(string.Empty);
        RefreshUserHistoryGrid();
        RefreshAdminStockGrid();
        ConfigureKeyboardAccessibility();
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
        if (_mainTabs is null) return;

        _mainTabs.SuspendLayout();
        _mainTabs.TabPages.Clear();

        _mainTabs.TabPages.Add(_authTab);

        _mainTabs.TabPages.Add(_userTab);

        if (role == "Konyvtaros" || role == "Adminisztrator")
        {
            _mainTabs.TabPages.Add(_librarianTab);
        }

        if (role == "Adminisztrator")
        {
            _mainTabs.TabPages.Add(_adminTab);
        }

        if (role == "Adminisztrator") _mainTabs.SelectedTab = _adminTab;
        else if (role == "Konyvtaros") _mainTabs.SelectedTab = _librarianTab;
        else _mainTabs.SelectedTab = _userTab;

        _mainTabs.ResumeLayout();
    }

    private enum StatusTone
    {
        Neutral,
        Success,
        Warning,
        Error
    }

    private void ConfigureKeyboardAccessibility()
    {
        KeyPreview = true;
        KeyDown += FormKeyDownHandler;

        if (_roleSelector is not null)
        {
            _roleSelector.TabIndex = 0;
            _roleSelector.AccessibleName = "Aktiv szerepkor valaszto";
        }

        if (_loginEmailInput is not null)
        {
            _loginEmailInput.TabIndex = 1;
            _loginEmailInput.AccessibleName = "Bejelentkezes email";
        }

        if (_loginPasswordInput is not null)
        {
            _loginPasswordInput.TabIndex = 2;
            _loginPasswordInput.AccessibleName = "Bejelentkezes jelszo";
        }

        if (_registerNameInput is not null)
        {
            _registerNameInput.TabIndex = 3;
            _registerNameInput.AccessibleName = "Regisztracio nev";
            _registerEmailInput.TabIndex = 4;
            _registerPhoneInput.TabIndex = 5;
            _registerPasswordInput.TabIndex = 6;
            _registerPasswordAgainInput.TabIndex = 7;
            _registerRoleInput.TabIndex = 8;
        }

        if (_userSearchInput is not null)
        {
            _userSearchInput.TabIndex = 9;
            _userSearchInput.AccessibleName = "Felhasznalo konyvkereso";
        }

        if (_librarianUserIdInput is not null)
        {
            _librarianUserIdInput.TabIndex = 10;
            _librarianBookIdInput.TabIndex = 11;
            _librarianDeadlinePicker.TabIndex = 12;
        }

        if (_adminCimInput is not null)
        {
            _adminCimInput.TabIndex = 13;
            _adminSzerzoInput.TabIndex = 14;
            _adminIsbnInput.TabIndex = 15;
            _adminKategoriaInput.TabIndex = 16;
            _adminKiadasevInput.TabIndex = 17;
            _adminKolcsonozhetoCheck.TabIndex = 18;
            _adminAllapotCombo.TabIndex = 19;
        }
    }

    private void FormKeyDownHandler(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.F)
        {
            if (_mainTabs.SelectedTab == _userTab)
            {
                _userSearchInput.Focus();
                _userSearchInput.SelectAll();
                SetStatusBar("Fokusz a konyvkereso mezon.");
            }

            e.SuppressKeyPress = true;
            return;
        }

        if (e.KeyCode == Keys.F5)
        {
            RefreshActiveView();
            e.SuppressKeyPress = true;
            return;
        }

        if (e.KeyCode == Keys.Escape)
        {
            SetStatusBar("Keszen all. Valassz muveletet.");
            e.SuppressKeyPress = true;
        }
    }

    private async void RefreshActiveView()
    {
        if (_mainTabs.SelectedTab == _userTab)
        {
            await LoadUserBooksFromApiAsync();
            RefreshUserHistoryGrid();
            SetStatusBar("Felhasznalo nezet frissitve az API-rol (F5).", StatusTone.Success);
            return;
        }

        if (_mainTabs.SelectedTab == _librarianTab)
        {
            await RefreshLibrarianLoansGrid();
            await RefreshFoglalasokGrid();

            SetStatusBar("Konyvtaros nezet frissitve (F5).", StatusTone.Success);
            return;
        }

        if (_mainTabs.SelectedTab == _adminTab)
        {
            RefreshAdminStockGrid(GetSelectedAdminBook()?.Id);
            SetStatusBar("Admin nezet frissitve (F5).", StatusTone.Success);
            return;
        }

        SetStatusBar("Auth nezet aktiv. Nincs frissitendo lista.");
    }
    private void RefreshAdminStockGrid(int? selectIdAfter = null)
    {
        _adminSuppressSelectionChanged = true;
        try
        {
            _adminStockGrid.DataSource = null;
            _adminStockGrid.DataSource = _adminBooksData.OrderBy(b => b.Id).ToList();
            _adminStockInfoLabel.Text = $"Könyvek: {_adminBooksData.Count}";

            if (selectIdAfter is int id)
            {
                foreach (DataGridViewRow row in _adminStockGrid.Rows)
                {
                    if (row.DataBoundItem is KonyvDto book && book.Id == id)
                        {
                        row.Selected = true;
                        _adminStockGrid.CurrentCell = row.Cells.Count > 0 ? row.Cells[0] : null;
                        break;
                    }
                }
            }
        }
        finally
        {
            _adminSuppressSelectionChanged = false;
        }
    }

    private void SetStatusBar(string message, StatusTone tone = StatusTone.Neutral)
    {
        _statusLabel.Text = message;
        _statusLabel.ForeColor = tone switch
        {
            StatusTone.Success => Color.FromArgb(22, 163, 74),
            StatusTone.Warning => Color.FromArgb(180, 83, 9),
            StatusTone.Error => Color.FromArgb(185, 28, 28),
            _ => TextColor
        };
    }

    private static void NotifyInfo(string title, string message) =>
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

    private static void NotifyWarning(string title, string message) =>
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    private static bool ConfirmWarning(string title, string message) =>
        MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;

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

        _loginEmailInput = new TextBox { Dock = DockStyle.Top };
        _loginPasswordInput = new TextBox { Dock = DockStyle.Top, UseSystemPasswordChar = true };
        _loginValidationLabel = CreateValidationLabel();

        loginForm.Controls.Add(CreateRequiredLabel("Email"), 0, 0);
        loginForm.Controls.Add(_loginEmailInput, 1, 0);
        loginForm.Controls.Add(CreateRequiredLabel("Jelszo"), 0, 1);
        loginForm.Controls.Add(_loginPasswordInput, 1, 1);
        loginForm.Controls.Add(CreatePrimaryButton("Bejelentkezes", HandleLoginClick), 1, 2);
        loginForm.Controls.Add(_loginValidationLabel, 1, 3);
        loginGroup.Controls.Add(loginForm);

        var registerGroup = new GroupBox { Text = "Regisztracio", Dock = DockStyle.Fill };
        var registerForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(12) };
        registerForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        registerForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _registerNameInput = new TextBox();
        _registerEmailInput = new TextBox();
        _registerPhoneInput = new TextBox();
        _registerPasswordInput = new TextBox { UseSystemPasswordChar = true };
        _registerPasswordAgainInput = new TextBox { UseSystemPasswordChar = true };
        _registerRoleInput = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = new[] { "Felhasznalo", "Konyvtaros", "Adminisztrator" }
        };
        _registerValidationLabel = CreateValidationLabel();

        AddRow(registerForm, "Nev *", _registerNameInput);
        AddRow(registerForm, "Email *", _registerEmailInput);
        AddRow(registerForm, "Telefon *", _registerPhoneInput);
        AddRow(registerForm, "Jelszo *", _registerPasswordInput);
        AddRow(registerForm, "Jelszo ujra *", _registerPasswordAgainInput);
        AddRow(registerForm, "Szerepkor *", _registerRoleInput);
        AddRow(registerForm, "", CreatePrimaryButton("Regisztracio", HandleRegisterClick));
        AddRow(registerForm, "", _registerValidationLabel);

        registerGroup.Controls.Add(registerForm);

        layout.Controls.Add(loginGroup, 0, 0);
        layout.Controls.Add(registerGroup, 1, 0);
        return tab;
    }

    private async void HandleLoginClick(object? sender, EventArgs e)
    {
        var email = _loginEmailInput.Text.Trim();
        var password = _loginPasswordInput.Text.Trim();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            _loginValidationLabel.Text = "Toltsd ki az osszes kotelezo mezot.";
            return;
        }

        if (!LooksLikeEmail(email))
        {
            _loginValidationLabel.Text = "Adj meg ervenyes email cimet.";
            return;
        }


        _loginValidationLabel.Text = "Bejelentkezes folyamatban...";
        _loginValidationLabel.ForeColor = MutedTextColor;
        SetStatusBar("Auth UI: Bejelentkezes folyamatban...", StatusTone.Neutral);

        var loginResult = await ApiClient.LoginAsync(email, password);

        if (loginResult != null)
        {
            _currentUserId = loginResult.FelhasznaloId;
            _loginValidationLabel.Text = $"Sikeres bejelentkezés! Üdv, {loginResult.Nev}!";
            _loginValidationLabel.ForeColor = Color.FromArgb(22, 163, 74);
            SetStatusBar($"Sikeres bejelentkezés. Szerepkör: {loginResult.Szerepkor}", StatusTone.Success);

            string uiRoleName = loginResult.Szerepkor == "Regisztralt" ? "Felhasznalo" : loginResult.Szerepkor;
            _roleSelector.SelectedItem = uiRoleName;

            _roleSelector.Enabled = (loginResult.Szerepkor == "Adminisztrator");

            ApplyRoleTabs(loginResult.Szerepkor);

            await LoadUserBooksFromApiAsync();

            if (loginResult.Szerepkor == "Konyvtaros" || loginResult.Szerepkor == "Adminisztrator")
            {
                await RefreshFoglalasokGrid();
                await RefreshLibrarianLoansGrid();
            }
        }
    }

    private async void HandleRegisterClick(object? sender, EventArgs e)
    {
        var name = _registerNameInput.Text.Trim();
        var email = _registerEmailInput.Text.Trim();
        var password = _registerPasswordInput.Text;
        var confirmPassword = _registerPasswordAgainInput.Text;

        var phone = _registerPhoneInput.Text.Trim();

        int szerepkorValue = _registerRoleInput.SelectedItem?.ToString() switch
        {
            "Konyvtaros" => 2,
            "Adminisztrator" => 4,
            _ => 1
        };


        SetStatusBar("Regisztráció folyamatban...", StatusTone.Neutral);
        _registerValidationLabel.Text = "Küldés...";
        _registerValidationLabel.ForeColor = MutedTextColor;

        bool success = await ApiClient.RegisterAsync(name, email, password, phone, szerepkorValue);

        if (success)
        {
            _registerValidationLabel.Text = "Sikeres regisztráció!";
            _registerValidationLabel.ForeColor = Color.FromArgb(22, 163, 74);
            SetStatusBar("Regisztráció sikeres.", StatusTone.Success);

            _registerNameInput.Clear();
            _registerEmailInput.Clear();
            _registerPhoneInput.Clear();
            _registerPasswordInput.Clear();
            _registerPasswordAgainInput.Clear();

            if (_registerRoleInput.Items.Count > 0)
            {
                _registerRoleInput.SelectedIndex = 0;
            }
        }
        else
        {
            _registerValidationLabel.Text = "Hiba a regisztráció során!";
            _registerValidationLabel.ForeColor = Color.Red;
            SetStatusBar("Regisztrációs hiba az API-n.", StatusTone.Error);
        }
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
        var booksPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(8) };
        booksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        booksPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        booksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        booksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        _userSearchInput = new TextBox { PlaceholderText = "Kereses cim/szerzo/kategoria alapjan..." };
        _userBooksGrid = CreateSampleGrid(new[] { "Cim", "Szerzo", "Kategoria", "Kiadas eve", "Elerheto" });
        _userBooksInfoLabel = new Label { AutoSize = true };

        var booksActionRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        booksActionRow.Controls.Add(CreatePrimaryButton("Kereses", HandleUserSearchClick));
        booksActionRow.Controls.Add(CreatePrimaryButton("Szuro torlese", HandleUserClearSearchClick));
        booksActionRow.Controls.Add(CreatePrimaryButton("Reszletek", HandleUserDetailsClick));
        booksActionRow.Controls.Add(CreatePrimaryButton("Elojegyzes", HandleUserReserveClick));

        booksPanel.Controls.Add(_userSearchInput, 0, 0);
        booksPanel.Controls.Add(_userBooksGrid, 0, 1);
        booksPanel.Controls.Add(booksActionRow, 0, 2);
        booksPanel.Controls.Add(_userBooksInfoLabel, 0, 3);
        booksGroup.Controls.Add(booksPanel);

        var profileGroup = new GroupBox { Text = "Sajat adatok", Dock = DockStyle.Fill };
        var profileForm = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(10) };
        profileForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        profileForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddRow(profileForm, "Nev", new TextBox());
        AddRow(profileForm, "Email", new TextBox());
        AddRow(profileForm, "Telefon", new TextBox());
        AddRow(profileForm, "Lakcim", new TextBox());
        AddRow(profileForm, "", CreatePrimaryButton("Adatok mentese", (s, ev) => {
            NotifyInfo("Profil", "A profiladatok mentese megtortent (mock).");
            SetStatusBar("Felhasznalo adatai frissitve.", StatusTone.Success);
        }));
        profileGroup.Controls.Add(profileForm);


        var historyGroup = new GroupBox { Text = "Kolcsonzesi elozmenyek", Dock = DockStyle.Fill };
        var historyPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(8) };
        historyPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        historyPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        historyPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _userHistoryGrid = CreateSampleGrid(new[] { "Konyv", "Kolcsonzes datuma", "Hatarido", "Statusz", "Hosszabbitasok" });
        _userHistoryInfoLabel = new Label { AutoSize = true };
        var historyActionRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        historyActionRow.Controls.Add(CreatePrimaryButton("Frissites", HandleUserHistoryRefreshClick));
        historyActionRow.Controls.Add(CreatePrimaryButton("Hosszabbitas (max 2x)", HandleUserHistoryExtendClick));
        historyPanel.Controls.Add(_userHistoryGrid, 0, 0);
        historyPanel.Controls.Add(historyActionRow, 0, 1);
        historyPanel.Controls.Add(_userHistoryInfoLabel, 0, 2);
        historyGroup.Controls.Add(historyPanel);

        layout.Controls.Add(booksGroup, 0, 0);
        layout.Controls.Add(profileGroup, 0, 1);
        layout.Controls.Add(historyGroup, 0, 2);
        return tab;
    }

    private void HandleUserSearchClick(object? sender, EventArgs e)
    {
        RefreshUserBooksGrid(_userSearchInput.Text);
        SetStatusBar("Felhasznalo nezet: kereses lefutott.", StatusTone.Success);
    }

    private void HandleUserClearSearchClick(object? sender, EventArgs e)
    {
        _userSearchInput.Clear();
        RefreshUserBooksGrid(string.Empty);
        SetStatusBar("Felhasznalo nezet: szuro torolve.");
    }

    private void HandleUserDetailsClick(object? sender, EventArgs e)
    {
        if (_userBooksGrid.CurrentRow?.DataBoundItem is not KonyvDto selected)
        {
            NotifyInfo("Reszletek", "Valassz egy konyvet a listabol!");
            return;
        }

        var details = $"Cim: {selected.Cim}\nSzerzo: {selected.Szerzo}\nKategoria: {selected.Kategoria}\nAllapot: {selected.Allapot}";
        NotifyInfo("Konyv reszletek", details);
    }

    private async void HandleUserReserveClick(object? sender, EventArgs e)
    {
        if (_userBooksGrid.CurrentRow?.DataBoundItem is not KonyvDto selected)
        {
            NotifyInfo("Elojegyzes", "Valassz egy konyvet!");
            return;
        }

        if (!selected.Kolcsonozheto) 
        {
            NotifyWarning("Hiba", "Ez a konyv jelenleg nem kolcsonozheto.");
            return;
        }

        bool siker = await ApiClient.CreateFoglalasAsync(_currentUserId, selected.Id, DateTime.Now.AddDays(14));

        if (siker)
        {
            NotifyInfo("Siker", $"A(z) {selected.Cim} sikeresen elojegyezve!");
            await LoadUserBooksFromApiAsync(); 
        }
        else
        {
            NotifyWarning("Hiba", "A foglalás nem sikerült (talán már foglalt).");
        }
    }

    private void RefreshUserBooksGrid(string? query)
    {
        var search = query?.Trim() ?? string.Empty;
        var filtered = string.IsNullOrWhiteSpace(search)
            ? _allBooksFromApi
            : _allBooksFromApi.Where(x =>
                x.Cim.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.Szerzo.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.Kategoria.Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToList();

        _userBooksGrid.DataSource = null;
        _userBooksGrid.DataSource = filtered;
        _userBooksInfoLabel.Text = $"Talalatok: {filtered.Count}";
    }

    private void RefreshUserHistoryGrid()
    {
        _userHistoryGrid.DataSource = _userHistoryData.ToList();
        _userHistoryInfoLabel.Text = _userHistoryData.Count == 0
            ? "Nincs meg kolcsonzesi elozmeny."
            : $"Kolcsonzesi tetelszam: {_userHistoryData.Count}";
    }

    public async Task LoadUserBooksFromApiAsync()
    {
        var konyvek = await ApiClient.GetKonyvekAsync();
        if (konyvek != null)
        {
            _allBooksFromApi = konyvek;
            RefreshUserBooksGrid(string.Empty);
        }
    }

    private void HandleUserHistoryRefreshClick(object? sender, EventArgs e)
    {
        RefreshUserHistoryGrid();
        SetStatusBar("Felhasznalo nezet: kolcsonzesi lista frissitve (mock).", StatusTone.Success);
    }

    private void HandleUserHistoryExtendClick(object? sender, EventArgs e)
    {
        if (_userHistoryGrid.CurrentRow?.DataBoundItem is not UserHistoryMockItem selected)
        {
            NotifyInfo("Hosszabbitas", "Valassz kolcsonzest a hosszabbitashoz.");
            return;
        }

        if (!string.Equals(selected.Statusz, "Aktiv", StringComparison.OrdinalIgnoreCase))
        {
            NotifyWarning("Hosszabbitas", "Csak aktiv kolcsonzes hosszabbithato.");
            return;
        }

        if (selected.Hosszabbitasok >= 2)
        {
            NotifyWarning("Hosszabbitas", "A kolcsonzes mar elerte a max 2 hosszabbitast.");
            return;
        }

        NotifyInfo("Hosszabbitas", "A hosszabbitas kerese rogzitve (mock). Backend bekotes kesobb.");
        SetStatusBar("Felhasznalo nezet: hosszabbitas kerese elokeszitve (mock).", StatusTone.Success);
    }

    private TabPage BuildLibrarianTab()
    {
        var tab = new TabPage("Konyvtaros");
        var layout = CreateTwoColumnLayout();
        tab.Controls.Add(layout);

        var loanGroup = new GroupBox { Text = "Kolcsonzes kezeles", Dock = DockStyle.Fill };
        var loanOuter = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(12)
        };
        loanOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize)); 
        loanOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize)); 
        loanOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        loanOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        loanOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var loanForm = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
        loanForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        loanForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _librarianUserIdInput = new TextBox();
        _librarianBookIdInput = new TextBox();
        _librarianDeadlinePicker = new DateTimePicker { Format = DateTimePickerFormat.Short, MinDate = DateTime.Today };
        AddRow(loanForm, "Felhasznalo ID *", _librarianUserIdInput);
        AddRow(loanForm, "Foglalas ID *", _librarianBookIdInput);
        AddRow(loanForm, "Hatarido *", _librarianDeadlinePicker);

        var loanButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        loanButtons.Controls.Add(CreatePrimaryButton("Kolcsonzes rogzitese", HandleLibrarianLoanCreateClick));
        loanButtons.Controls.Add(CreatePrimaryButton("Visszavetel", HandleLibrarianReturnClick));
        loanButtons.Controls.Add(CreatePrimaryButton("Hosszabbitas engedelyezese", HandleLibrarianExtendApproveClick));
        loanButtons.Controls.Add(CreatePrimaryButton("Hosszabbitas elutasitasa", HandleLibrarianExtendDenyClick));

        _librarianFoglalasokGrid = CreateMockDataGrid();
        _librarianFoglalasokGrid.MinimumSize = new Size(100, 150);
        _librarianFoglalasokGrid.Height = 200;
        _librarianFoglalasokGrid.SelectionChanged += HandleFoglalasSelectionChanged;

        loanOuter.Controls.Add(new Label { Text = "Aktuális foglalások (Válassz ki egyet):", AutoSize = true, Font = new Font(BaseFont, FontStyle.Bold) }, 0, 3);
        loanOuter.Controls.Add(_librarianFoglalasokGrid, 0, 4);

        _librarianLoanFeedbackLabel = new Label
        {
            AutoSize = true,
            Text = "Valassz sort a tablazatban (jobb oldal) vagy rogzits uj kolcsonzest.",
            ForeColor = MutedTextColor
        };

        loanOuter.Controls.Add(loanForm, 0, 0);
        loanOuter.Controls.Add(loanButtons, 0, 1);
        loanOuter.Controls.Add(_librarianLoanFeedbackLabel, 0, 2);
        loanGroup.Controls.Add(loanOuter);

        var fineGroup = new GroupBox { Text = "Birsag es kolcsonzesek", Dock = DockStyle.Fill };
        var fineOuter = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(12)
        };
        fineOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fineOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fineOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fineOuter.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var fineForm = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
        fineForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        fineForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        _librarianFineUserInput = new TextBox();
        _librarianFineAmountInput = new TextBox();
        _librarianFineNoteInput = new TextBox();
        AddRow(fineForm, "Felhasznalo ID *", _librarianFineUserInput);
        AddRow(fineForm, "Osszeg (Ft) *", _librarianFineAmountInput);
        AddRow(fineForm, "Megjegyzes", _librarianFineNoteInput);

        var fineButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        fineButtons.Controls.Add(CreatePrimaryButton("Birsag kiszabasa", HandleLibrarianFineCreateClick));
        fineButtons.Controls.Add(CreatePrimaryButton("Birsag torlese (mock)", HandleLibrarianFineDeleteClick));

        _librarianFineFeedbackLabel = new Label
        {
            AutoSize = true,
            Text = "A birsag rogzitese a keseshez kotodik (mock).",
            ForeColor = MutedTextColor
        };

        _librarianLoansGrid = CreateMockDataGrid();

        fineOuter.Controls.Add(fineForm, 0, 0);
        fineOuter.Controls.Add(fineButtons, 0, 1);
        fineOuter.Controls.Add(_librarianFineFeedbackLabel, 0, 2);
        fineOuter.Controls.Add(_librarianLoansGrid, 0, 3);
        fineGroup.Controls.Add(fineOuter);

        layout.Controls.Add(loanGroup, 0, 0);
        layout.Controls.Add(fineGroup, 1, 0);
        return tab;
    }

    private void HandleFoglalasSelectionChanged(object? sender, EventArgs e)
    {
        if (_librarianFoglalasokGrid.CurrentRow?.DataBoundItem is ApiClient.FoglalasGetDto selected)
        {
            _librarianUserIdInput.Text = selected.FelhasznaloId.ToString();
            _librarianBookIdInput.Text = selected.Id.ToString();
            _librarianDeadlinePicker.Value = selected.Hatarido;

            SetLibrarianLoanFeedback($"Kiválasztva: {selected.KonyvCim}");
        }
    }

    private async Task RefreshLibrarianLoansGrid()
    {
        SetStatusBar("Kölcsönzések betöltése...", StatusTone.Neutral);
        var kolcsonzesek = await ApiClient.GetAllKolcsonzesAsync();
        _librarianLoansGrid.DataSource = null;
        if (kolcsonzesek != null)
        {
            _librarianLoansGrid.DataSource = kolcsonzesek;
            SetStatusBar("Kölcsönzések frissítve.", StatusTone.Success);
        }
    }
    private async Task RefreshFoglalasokGrid()
    {
        if (string.IsNullOrEmpty(ApiClient.Token))
        {
            return;
        }

        var foglalasok = await ApiClient.GetAllFoglalasAsync();
        _librarianFoglalasokGrid.DataSource = null;

        if (foglalasok != null)
        {
            _librarianFoglalasokGrid.DataSource = foglalasok.ToList();
        }
    }

    private ApiClient.KolcsonzesGetDto? GetSelectedLibrarianLoanActual()
    {
        return _librarianLoansGrid.CurrentRow?.DataBoundItem as ApiClient.KolcsonzesGetDto;
    }

    private void SetLibrarianLoanFeedback(string text, bool isError = false)
    {
        _librarianLoanFeedbackLabel.Text = text;
        _librarianLoanFeedbackLabel.ForeColor = isError ? Color.FromArgb(185, 28, 28) : MutedTextColor;
    }

    private void SetLibrarianFineFeedback(string text, bool isError = false)
    {
        _librarianFineFeedbackLabel.Text = text;
        _librarianFineFeedbackLabel.ForeColor = isError ? Color.FromArgb(185, 28, 28) : MutedTextColor;
    }

    private async void HandleLibrarianLoanCreateClick(object? sender, EventArgs e)
    {
        if (!int.TryParse(_librarianBookIdInput.Text.Trim(), out var foglalasId))
        {
            SetLibrarianLoanFeedback("Érvénytelen Foglalás ID (szám kell)!", true);
            return;
        }

        SetLibrarianLoanFeedback("Kölcsönzés rögzítése folyamatban...");
        bool siker = await ApiClient.CreateKolcsonzesAsync(foglalasId);

        if (siker)
        {
            SetLibrarianLoanFeedback("Sikeres kölcsönzés!");
            await RefreshLibrarianLoansGrid();
        }
        else
        {
            SetLibrarianLoanFeedback("Hiba! Csak aktív foglalásból lehet kölcsönözni.", true);
        }
    }

    private async void HandleLibrarianReturnClick(object? sender, EventArgs e)
    {
        if (_librarianLoansGrid.CurrentRow?.DataBoundItem is not ApiClient.KolcsonzesGetDto selected)
        {
            NotifyInfo("Visszavétel", "Válassz ki egy kölcsönzést a táblázatból!");
            return;
        }

        SetLibrarianLoanFeedback("Visszavétel rögzítése...");
        if (await ApiClient.UpdateKolcsonzesStatuszAsync(selected.Id, 1))
        {
            SetLibrarianLoanFeedback("Könyv sikeresen visszavéve!");
            await RefreshLibrarianLoansGrid();
        }
    }

    private async void HandleLibrarianExtendApproveClick(object? sender, EventArgs e)
    {
        var selected = GetSelectedLibrarianLoanActual();
        if (selected == null) return;
        var ujDatum = selected.Hatarido.AddDays(7);
        if (await ApiClient.ExtendKolcsonzesAsync(selected.Id, ujDatum))
        {
            SetLibrarianLoanFeedback("Hosszabbítva!");
            await RefreshLibrarianLoansGrid();
        }
    }

    private void HandleLibrarianExtendDenyClick(object? sender, EventArgs e)
    {
        if (GetSelectedLibrarianLoanActual() == null)
        {
            NotifyInfo("Hosszabbítás", "Válassz egy kölcsönzést az elutasításhoz.");
            return;
        }

        NotifyInfo("Hosszabbítás", "A hosszabbítási kérelem elutasítva.");
        SetLibrarianLoanFeedback("Hosszabbítás elutasítva.");
        SetStatusBar("Könyvtáros nézet: hosszabbítás elutasítva.");
    }

    private async void HandleLibrarianFineCreateClick(object? sender, EventArgs e)
    {
        if (int.TryParse(_librarianFineUserInput.Text, out var uId) &&
            int.TryParse(_librarianFineAmountInput.Text, out var fId))
        {
            if (await ApiClient.CreateBuntetesAsync(uId, fId))
                SetLibrarianFineFeedback("Bírság kiszabva!");
        }
    }

    private void HandleLibrarianFineDeleteClick(object? sender, EventArgs e)
    {
        NotifyInfo("Bírság", "A törlés nem támogatott az API-n.");
    }

    private TabPage BuildAdminTab()
    {
        var tab = new TabPage("Adminisztrator");
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(8) };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 56));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 44));
        tab.Controls.Add(layout);

        var stockGroup = new GroupBox { Text = "Konyvallomany kezeles", Dock = DockStyle.Fill };
        var stockPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(8) };
        stockPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stockPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        stockPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        stockPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var stockToolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        stockToolbar.Controls.Add(CreatePrimaryButton("Uj konyv", HandleAdminNewBookClick));
        stockToolbar.Controls.Add(CreatePrimaryButton("Modositas", HandleAdminReloadSelectionClick));
        stockToolbar.Controls.Add(CreatePrimaryButton("Torles", HandleAdminDeleteClick));
        stockToolbar.Controls.Add(CreatePrimaryButton("Frissites", HandleAdminRefreshClick));

        _adminStockGrid = CreateMockDataGrid();
        _adminStockGrid.SelectionChanged += AdminStockGrid_SelectionChanged;

        var stateToolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        stateToolbar.Controls.Add(CreatePrimaryButton("Allapot: Jo", (_, _) => HandleAdminSetStateClick("Jo")));
        stateToolbar.Controls.Add(CreatePrimaryButton("Allapot: Serult", (_, _) => HandleAdminSetStateClick("Serult")));
        stateToolbar.Controls.Add(CreatePrimaryButton("Allapot: Elveszett", (_, _) => HandleAdminSetStateClick("Elveszett")));

        _adminStockInfoLabel = new Label { AutoSize = true, Text = "Konyvek: 0" };

        stockPanel.Controls.Add(stockToolbar, 0, 0);
        stockPanel.Controls.Add(_adminStockGrid, 0, 1);
        stockPanel.Controls.Add(stateToolbar, 0, 2);
        stockPanel.Controls.Add(_adminStockInfoLabel, 0, 3);
        stockGroup.Controls.Add(stockPanel);

        var detailsGroup = new GroupBox { Text = "Konyv adatok", Dock = DockStyle.Fill };
        var detailsOuter = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(12) };
        detailsOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        detailsOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        detailsOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var detailsForm = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
        detailsForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        detailsForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        _adminCimInput = new TextBox();
        _adminSzerzoInput = new TextBox();
        _adminIsbnInput = new TextBox();
        _adminKategoriaInput = new TextBox();
        _adminKiadasevInput = new TextBox();
        _adminKolcsonozhetoCheck = new CheckBox { Text = "", AutoSize = true };
        _adminAllapotCombo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = new[] { "Jo", "Serult", "Elveszett" }
        };

        AddRow(detailsForm, "Cim *", _adminCimInput);
        AddRow(detailsForm, "Szerzo *", _adminSzerzoInput);
        AddRow(detailsForm, "ISBN", _adminIsbnInput);
        AddRow(detailsForm, "Kategoria", _adminKategoriaInput);
        AddRow(detailsForm, "Kiadas eve *", _adminKiadasevInput);
        AddRow(detailsForm, "Kölcsönözhető", _adminKolcsonozhetoCheck);
        AddRow(detailsForm, "Allapot", _adminAllapotCombo);

        var detailButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        detailButtons.Controls.Add(CreatePrimaryButton("Ment", HandleAdminSaveClick));
        detailButtons.Controls.Add(CreatePrimaryButton("Reset", HandleAdminResetClick));

        _adminDetailFeedbackLabel = new Label
        {
            AutoSize = true,
            Text = "Valassz sort a tablazatbol, vagy kattints az Uj konyv gombra.",
            ForeColor = MutedTextColor
        };

        detailsOuter.Controls.Add(detailsForm, 0, 0);
        detailsOuter.Controls.Add(detailButtons, 0, 1);
        detailsOuter.Controls.Add(_adminDetailFeedbackLabel, 0, 2);
        detailsGroup.Controls.Add(detailsOuter);

        layout.Controls.Add(stockGroup, 0, 0);
        layout.Controls.Add(detailsGroup, 0, 1);
        return tab;
    }

    private void AdminStockGrid_SelectionChanged(object? sender, EventArgs e)
    {
        if (_adminSuppressSelectionChanged) return;

        if (_adminStockGrid.CurrentRow?.DataBoundItem is KonyvDto book)
        {
            _adminIsNewBookMode = false;
            LoadAdminDetailFromBook(book);
            SetAdminDetailFeedback("Kivalasztva: szerkesztheted az alabbi mezoket.");
        }
    }

    private void LoadAdminDetailFromBook(KonyvDto book)
    {
        _adminCimInput.Text = book.Cim;
        _adminSzerzoInput.Text = book.Szerzo;
        _adminIsbnInput.Text = book.Isbn ?? string.Empty;
        _adminKategoriaInput.Text = book.Kategoria;
        _adminKiadasevInput.Text = book.Kiadasev.ToString();
        _adminKolcsonozhetoCheck.Checked = book.Kolcsonozheto;
        _adminAllapotCombo.SelectedItem = book.Allapot;
    }
    private KonyvDto? GetSelectedAdminBook()
    {
        if (_adminStockGrid.CurrentRow?.DataBoundItem is KonyvDto book)
        {
            return book;
        }
        return null;
    }

    private void ClearAdminDetailForm()
    {
        _adminCimInput.Clear();
        _adminSzerzoInput.Clear();
        _adminIsbnInput.Clear();
        _adminKategoriaInput.Clear();
        _adminKiadasevInput.Clear();
        _adminKolcsonozhetoCheck.Checked = true;
        _adminAllapotCombo.SelectedIndex = 0;
    }

    private void SetAdminDetailFeedback(string text, bool isError = false)
    {
        _adminDetailFeedbackLabel.Text = text;
        _adminDetailFeedbackLabel.ForeColor = isError ? Color.FromArgb(185, 28, 28) : MutedTextColor;
    }

    private void HandleAdminNewBookClick(object? sender, EventArgs e)
    {
        _adminSuppressSelectionChanged = true;
        try
        {
            _adminStockGrid.ClearSelection();
        }
        finally
        {
            _adminSuppressSelectionChanged = false;
        }

        _adminIsNewBookMode = true;
        ClearAdminDetailForm();
        SetAdminDetailFeedback("Uj konyv mod: toltsd ki a kotelezo mezoket, majd Ment.");
        SetStatusBar("Admin nezet: uj konyv mod (mock).");
    }

    private void HandleAdminReloadSelectionClick(object? sender, EventArgs e)
    {
        var selected = GetSelectedAdminBook();
        if (selected is null)
        {
            NotifyInfo("Modositas", "Valassz egy konyvet a tablazatbol.");
            return;
        }

        _adminIsNewBookMode = false;
        LoadAdminDetailFromBook(selected);
        SetAdminDetailFeedback("Adatok ujratoltve a kivalasztott sorbol.");
        SetStatusBar("Admin nezet: szerkesztes (mock).");
    }

    private async void HandleAdminDeleteClick(object? sender, EventArgs e)
    {
        var selected = GetSelectedAdminBook();
        if (selected is null)
        {
            NotifyInfo("Törlés", "Válassz egy könyvet a törléshez.");
            return;
        }

        if (!ConfirmWarning("Törlés", $"Biztosan törlöd a rendszerből?\n\n{selected.Cim}"))
        {
            return;
        }

        SetStatusBar("Könyv törlése folyamatban...", StatusTone.Neutral);

        bool success = await ApiClient.DeleteKonyvAsync(selected.Id);

        if (success)
        {
            _adminIsNewBookMode = false;
            ClearAdminDetailForm();
            HandleAdminRefreshClick(null, null);
            SetAdminDetailFeedback("Könyv sikeresen törölve a backendből.");
            SetStatusBar("Admin nézet: sikeres törlés.", StatusTone.Success);
        }
        else
        {
            SetAdminDetailFeedback("Hiba történt a törlés során!", true);
            SetStatusBar("Admin nézet: API törlési hiba.", StatusTone.Error);
        }
    }

    private async void HandleAdminRefreshClick(object? sender, EventArgs e)
    {
        SetStatusBar("Konyvek lekerdezese folyamatban...", StatusTone.Neutral);

        var konyvek = await ApiClient.GetKonyvekAsync();

        if (konyvek != null)
        {
            _adminBooksData.Clear();
            _adminBooksData.AddRange(konyvek);

            var id = GetSelectedAdminBook()?.Id;
            RefreshAdminStockGrid(id);

            SetAdminDetailFeedback("Lista frissitve az adatbazisbol.");
            SetStatusBar("Admin nezet: sikeres adatbazis frissites.", StatusTone.Success);
        }
        else
        {
            SetAdminDetailFeedback("Hiba a konyvek lekerdezese soran!", true);
            SetStatusBar("Admin nezet: API hiba.", StatusTone.Error);
        }
    }

    private void HandleAdminSetStateClick(string allapot)
    {
        var selected = GetSelectedAdminBook();
        if (selected is null)
        {
            NotifyInfo("Allapot", "Valassz egy konyvet az allapot modositasahoz.");
            return;
        }

        var index = _adminBooksData.FindIndex(b => b.Id == selected.Id);
        if (index >= 0)
        {
            _adminBooksData[index] = selected with { Allapot = allapot };
            RefreshAdminStockGrid(selected.Id);
            SetAdminDetailFeedback($"Allapot beallitva: {allapot} (helyi frissites).");
            SetStatusBar("Admin nezet: allapot modositva.", StatusTone.Success);
        }
    }

    private async void HandleAdminSaveClick(object? sender, EventArgs e)
    {
        var cim = _adminCimInput.Text.Trim();
        var szerzo = _adminSzerzoInput.Text.Trim();

        var kategoria = string.IsNullOrWhiteSpace(_adminKategoriaInput.Text) ? "Egyéb" : _adminKategoriaInput.Text.Trim();
        var isbn = string.IsNullOrWhiteSpace(_adminIsbnInput.Text) ? null : _adminIsbnInput.Text.Trim();
        var allapot = _adminAllapotCombo.SelectedItem?.ToString() ?? "Jo";
        var kolcsonozheto = _adminKolcsonozhetoCheck.Checked;

        if (string.IsNullOrWhiteSpace(cim) || string.IsNullOrWhiteSpace(szerzo))
        {
            SetAdminDetailFeedback("A cím és a szerző megadása kötelező.", true);
            return;
        }

        if (!int.TryParse(_adminKiadasevInput.Text.Trim(), out var kiadasev))
        {
            SetAdminDetailFeedback("A kiadás éve egész szám legyen.", true);
            return;
        }


        SetStatusBar("Mentés folyamatban...", StatusTone.Neutral);

        try
        {
            bool success;
            if (_adminIsNewBookMode)
            {
                var createDto = new KonyvCreateDto
                {
                    Cim = cim,
                    Szerzo = szerzo,
                    Isbn = isbn,
                    Kategoria = kategoria,
                    Kiadasev = kiadasev,
                    Kolcsonozheto = kolcsonozheto,
                    Allapot = allapot
                };
                success = await ApiClient.CreateKonyvAsync(createDto);
            }
            else
            {
                var selected = GetSelectedAdminBook();
                if (selected is null)
                {
                    SetAdminDetailFeedback("Nincs kiválasztva könyv! Új könyv felvételéhez előbb kattints az 'Új könyv' gombra.", true);
                    SetStatusBar("Mentés megszakítva.", StatusTone.Warning);
                    return;
                }

                var updateDto = new KonyvUpdateDto
                {
                    Id = selected.Id,
                    Cim = cim,
                    Szerzo = szerzo,
                    Isbn = isbn,
                    Kategoria = kategoria,
                    Kiadasev = kiadasev,
                    Allapot = allapot,
                    Kolcsonozheto = kolcsonozheto,
                    Statusz = selected.Statusz,
                    PublikalasIdeje = DateTime.Now,
                    Ertelekes = true
                };
                success = await ApiClient.UpdateKonyvAsync(selected.Id, updateDto);
            }

            if (success)
            {
                SetAdminDetailFeedback(_adminIsNewBookMode ? "Új könyv sikeresen mentve." : "Könyv módosítva.");

                if (_adminIsNewBookMode)
                {
                    ClearAdminDetailForm();
                }

                _adminIsNewBookMode = false;
                HandleAdminRefreshClick(null, null);

                var currentBook = GetSelectedAdminBook();
                if (currentBook != null)
                {
                    LoadAdminDetailFromBook(currentBook);
                }
            }
            else
            {
                SetAdminDetailFeedback("A szerver hibát jelzett vissza (ellenőrizze a jogosultságokat és a bejelentkezést).", true);
                SetStatusBar("Admin nézet: API mentési hiba.", StatusTone.Error);
            }
        }
        catch (Exception ex)
        {
            SetAdminDetailFeedback($"Hálózati hiba: {ex.Message}", true);
            SetStatusBar("Hiba a mentés során!", StatusTone.Error);
        }
    }

    private void HandleAdminResetClick(object? sender, EventArgs e)
    {
        if (_adminIsNewBookMode)
        {
            ClearAdminDetailForm();
            SetAdminDetailFeedback("Urlap torolve (uj konyv mod).");
            return;
        }

        var selected = GetSelectedAdminBook();
        if (selected is not null)
        {
            LoadAdminDetailFromBook(selected);
            SetAdminDetailFeedback("Visszaallitva a kivalasztott sor alapjan.");
        }
        else
        {
            ClearAdminDetailForm();
            SetAdminDetailFeedback("Nincs kivalasztott sor — urlap torolve.");
        }
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
        button.Click += (_, _) => NotifyInfo("GUI vaz", "Ez a funkcio meg nincs implementalva.");

        return button;
    }

    private static Button CreatePrimaryButton(string text, EventHandler onClick)
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
        button.Click += onClick;
        return button;
    }

    private static Label CreateRequiredLabel(string text)
    {
        return new Label { Text = $"{text} *", AutoSize = true };
    }

    private static Label CreateValidationLabel()
    {
        return new Label
        {
            AutoSize = true,
            Text = "Kotelezo mezok: *",
            ForeColor = Color.FromArgb(185, 28, 28),
            Margin = new Padding(0, 4, 0, 0)
        };
    }

    private static bool LooksLikeEmail(string email)
    {
        return email.Contains('@') && email.Contains('.');
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
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoGenerateColumns = true
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

        return grid;
    }

    private static DataGridView CreateMockDataGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoGenerateColumns = true
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
        return grid;
    }

    private static void AddRow(TableLayoutPanel panel, string labelText, Control control)
    {
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var rowIndex = panel.RowCount++;
        panel.Controls.Add(new Label { Text = labelText, AutoSize = true, Margin = new Padding(3, 10, 10, 8) }, 0, rowIndex);

        // Pipa (CheckBox) esetén ne húzzuk szét teljes szélességre
        if (control is CheckBox)
        {
            control.Dock = DockStyle.Left;
        }
        else
        {
            control.Dock = DockStyle.Top;
        }

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

internal sealed record UserBookMockItem(string Cim, string Szerzo, string Kategoria, int Kiadasev, bool Elerheto);

internal sealed record UserHistoryMockItem(string Konyv, string KolcsonzesDatuma, string Hatarido, string Statusz, int Hosszabbitasok);


internal sealed record AdminBookMockItem(
    int Id,
    string Cim,
    string Szerzo,
    string? Isbn,
    string Kategoria,
    int Kiadasev,
    bool Kolcsonozheto,
    string Allapot);
