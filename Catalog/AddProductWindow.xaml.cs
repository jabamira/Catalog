using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace Catalog
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {


        // Property to hold the newly created Product
        public Product NewProduct { get; private set; }
        string imagePath = "C:\\Users\\artem\\OneDrive\\Документы\\ShareX\\Screenshots\\2024-10\\chrome_ZyQxAV0SfC.png";
        public AddProductWindow()
        {
            InitializeComponent();

            BitmapImage bitmapImage = new BitmapImage(new Uri("C:\\Users\\artem\\OneDrive\\Документы\\ShareX\\Screenshots\\2024-10\\chrome_ZyQxAV0SfC.png"));
            Picture_box.Source = bitmapImage;


        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            string productName = ProductNameTextBox.Text;
            string productDescription = ProductDescriptionTextBox.Text;
            string productManufacturer = ProductManufacturerTextBox.Text;
            string productCount = ProductCountTextBox.Text;


            // Validate input fields
            if (string.IsNullOrEmpty(productName) ||
                string.IsNullOrEmpty(productDescription) ||
                string.IsNullOrEmpty(productManufacturer) ||
                string.IsNullOrEmpty(productCount) ||
                string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate product count
            if (!int.TryParse(productCount, out int count))
            {
                MessageBox.Show("Product count must be a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create a new Product object using the input data
            Product newProduct = new Product(0, productName, productDescription, productManufacturer, count, null, imagePath);

            // Set the NewProduct property
            NewProduct = newProduct;

            // Close the window (DialogResult = true indicates success)
            DialogResult = true;
        }

        private void Chose_file_click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();


            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";


            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                Picture_box.Source = bitmapImage;
            }


        }

    }

}