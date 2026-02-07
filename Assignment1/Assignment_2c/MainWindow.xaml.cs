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

        }

        private void LoadClicked_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Data Files (*.xml;*.json;*.csv)|*.xml;*.json;*.csv|All Files (*.*)|*.*";

            if (dialog.ShowDialog() == true)
            {
                mWeaponCollection.Load(dialog.FileName);
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

        }

        private void FilterNameText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}