using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeaponLib;

namespace Assignment_2c
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public Assignment2a.Weapon ResultWeapon { get; private set; }

        private bool _isEdit = false;
        private Assignment2a.Weapon _original = null;
        public EditWindow()
        {
            InitializeComponent();

            // Populate Type ComboBox from enum (requires TypeComboBox x:Name in XAML)
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(Assignment2a.Weapon.WeaponType));
        }
        public EditWindow(Assignment2a.Weapon weapon) : this()
        {
            if (weapon is null) return;
            _isEdit = true;
            _original = weapon;

            // Fill fields with existing values
            NameTextBox.Text = weapon.Name;
            TypeComboBox.SelectedItem = weapon.Type;
            ImageTextBox.Text = weapon.Image;
            RarityComboBox.SelectedIndex = Math.Clamp(weapon.Rarity - 1, 0, 4);
            BaseAttackTextBox.Text = weapon.BaseAttack.ToString();
            SecondaryTextBox.Text = weapon.SecondaryStat;
            PassiveTextBox.Text = weapon.Passive;

            UpdateImagePreview(weapon.Image);
        }

        private void SaveClick_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation and populate a Weapon instance
            string name = NameTextBox.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!(TypeComboBox.SelectedItem is Assignment2a.Weapon.WeaponType selType))
            {
                MessageBox.Show("Select a valid Type.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(BaseAttackTextBox.Text?.Trim(), out int baseAttack))
            {
                MessageBox.Show("Base Attack must be an integer.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int rarity = 1;
            if (RarityComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem cbi)
            {
                int.TryParse(cbi.Content?.ToString(), out rarity);
            }

            if (_isEdit && _original != null)
            {
                // Update original instance
                _original.Name = name;
                _original.Type = selType;
                _original.Image = ImageTextBox.Text?.Trim();
                _original.Rarity = rarity;
                _original.BaseAttack = baseAttack;
                _original.SecondaryStat = SecondaryTextBox.Text?.Trim();
                _original.Passive = PassiveTextBox.Text?.Trim();
                ResultWeapon = _original;
            }
            else
            {
                // Create new weapon
                ResultWeapon = new Assignment2a.Weapon
                {
                    Name = name,
                    Type = selType,
                    Image = ImageTextBox.Text?.Trim(),
                    Rarity = rarity,
                    BaseAttack = baseAttack,
                    SecondaryStat = SecondaryTextBox.Text?.Trim(),
                    Passive = PassiveTextBox.Text?.Trim()
                };
            }

            this.DialogResult = true;
            this.Close();
        }

        private void CancelClick_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Called by XAML when ImageTextBox text changes (user edits URL)
        private void ImageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateImagePreview(ImageTextBox.Text?.Trim());
        }

        // Safely load the image from a URL or file path and display it in DisplayImage
        private void UpdateImagePreview(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                DisplayImage.Source = null;
                return;
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                // Load immediately so file can be released and errors surface now
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                // Only freeze if allowed — some BitmapImage instances cannot be frozen
                if (bitmap.CanFreeze)
                {
                    bitmap.Freeze();
                }

                DisplayImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                // Log the exception so you can see why loading failed (Output window)
                Debug.WriteLine($"UpdateImagePreview failed loading '{url}': {ex}");
                DisplayImage.Source = null;
            }
        }
    }
}

