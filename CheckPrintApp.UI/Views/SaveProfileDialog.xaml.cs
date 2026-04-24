using System.Windows;
using System.Windows.Input;

namespace CheckPrintApp.UI.Views;

/// <summary>
/// Banka profili adı giriş dialog'u.
/// ShowDialog() true döndürdüğünde ProfileName property'si dolu olur.
/// </summary>
public partial class SaveProfileDialog : Window
{
    /// <summary>
    /// Kullanıcının girdiği profil adı (dialog onaylandıktan sonra geçerli)
    /// </summary>
    public string ProfileName { get; private set; } = string.Empty;

    /// <summary>
    /// Varolan profil adlarını üzerine yazma uyarısı için kullanılır
    /// </summary>
    public System.Collections.Generic.IEnumerable<string>? ExistingProfiles { get; set; }

    public SaveProfileDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => ProfileNameTextBox.Focus();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e) => TrySave();

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void ProfileNameTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) TrySave();
        if (e.Key == Key.Escape) { DialogResult = false; Close(); }
    }

    private void TrySave()
    {
        string name = ProfileNameTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            ShowError("Profil adı boş olamaz.");
            return;
        }

        // Varolan profil üzerine yazma uyarısı
        if (ExistingProfiles != null)
        {
            foreach (var existing in ExistingProfiles)
            {
                if (string.Equals(existing, name, System.StringComparison.OrdinalIgnoreCase))
                {
                    var result = MessageBox.Show(
                        $"'{name}' profili zaten mevcut. Üzerine yazılsın mı?",
                        "Profil Zaten Var",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result != MessageBoxResult.Yes) return;
                    break;
                }
            }
        }

        ProfileName  = name;
        DialogResult = true;
        Close();
    }

    private void ShowError(string message)
    {
        ErrorText.Text       = message;
        ErrorText.Visibility = System.Windows.Visibility.Visible;
        ProfileNameTextBox.Focus();
    }
}
