using Microsoft.Win32;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeaponLib;
using Assignment2a;

namespace Assignment_2c
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WeaponCollection mWeaponCollection;
        private ICollectionView _weaponView;

        public MainWindow()
        {
            InitializeComponent();
            mWeaponCollection = new WeaponCollection();
            WeaponListBox.ItemsSource = mWeaponCollection;



            _weaponView = CollectionViewSource.GetDefaultView(WeaponListBox.ItemsSource);

            FilterTypeComboBox.Items.Add("All");

            foreach (Assignment2a.Weapon.WeaponType type in Enum.GetValues(typeof(Assignment2a.Weapon.WeaponType)))
            {
                FilterTypeComboBox.Items.Add(type);
            }

            FilterTypeComboBox.SelectedIndex = 0;

        }

        private void LoadClicked_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Data Files (*.xml;*.json;*.csv)|*.xml;*.json;*.csv|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                mWeaponCollection.Load(dialog.FileName);
                WeaponListBox.Items.Refresh();
            }
        }

        private void SaveClicked_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Data Files (*.xml;*.json;*.csv)|*.xml;*.json;*.csv|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                mWeaponCollection.Save(dialog.FileName);
            }
        }

        private void AddClicked_Click(object sender, RoutedEventArgs e)
        {
            EditWindow win = new EditWindow();
            if (win.ShowDialog() == true)
            {
                
            }
        }

        private void EditClicked_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RemoveClicked_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowOnlyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterTypeComboBox.SelectedItem == null)
                return;

            if (FilterTypeComboBox.SelectedItem is string)
            {
                // ALL
                WeaponListBox.ItemsSource = mWeaponCollection;
            }
            else
            {
                Assignment2a.Weapon.WeaponType selectedType =
                    (Assignment2a.Weapon.WeaponType)FilterTypeComboBox.SelectedItem;

                WeaponListBox.ItemsSource =
                    mWeaponCollection.GetAllWeaponsOfType(selectedType);
            }
            FilterNameText_TextChanged(null, null);

        }

        private void FilterNameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get current filter text
            string filter = FilterNameTextBox?.Text?.Trim();

            // Get the view for the current ItemsSource (works whether ItemsSource is the full collection or a typed subset)
            ICollectionView view = CollectionViewSource.GetDefaultView(WeaponListBox.ItemsSource);
            if (view == null)
                return;

            if (string.IsNullOrEmpty(filter))
            {
                // Remove filter
                view.Filter = null;
            }
            else
            {
                // Apply case-insensitive name contains filter
                view.Filter = item =>
                {
                    if (item is Assignment2a.Weapon w)
                    {
                        return !string.IsNullOrEmpty(w.Name) &&
                               w.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                    return false;
                };
            }

            view.Refresh();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not RadioButton rb) return;

            string column = rb.Content?.ToString();
            if (string.IsNullOrWhiteSpace(column)) return;

            // Ask the collection to sort by the selected column
            mWeaponCollection.SortBy(column);

            // Re-populate the ListBox according to current type filter
            if (FilterTypeComboBox.SelectedItem == null || FilterTypeComboBox.SelectedItem is string)
            {
                WeaponListBox.ItemsSource = mWeaponCollection;
            }
            else
            {
                var selectedType = (Assignment2a.Weapon.WeaponType)FilterTypeComboBox.SelectedItem;
                WeaponListBox.ItemsSource = mWeaponCollection.GetAllWeaponsOfType(selectedType);
            }

            WeaponListBox.Items.Refresh();
        }
    }
}