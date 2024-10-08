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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public Product ProductToEdit { get; set; }
        string imagePath;
        public EditProductWindow(Product product)
        {
            InitializeComponent();
            ProductToEdit = product;
            // Set the initial values of the text boxes and image
            ProductNameTextBox.Text = product.Name;
            ProductDescriptionTextBox.Text = product.Description;
            ProductManufacturerTextBox.Text = product.Manufacturer;
            ProductCountTextBox.Text = product.Count.ToString();
            imagePath = product.PathImage;
            // Load the image into the Picture_box
            if (!string.IsNullOrEmpty(imagePath))
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
                Picture_box.Source = bitmapImage;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get data from the input controls in your AddProductWindow 
            string productName = ProductNameTextBox.Text;
            string productDescription = ProductDescriptionTextBox.Text;
            string productManufacturer = ProductManufacturerTextBox.Text;
            string productCount = ProductCountTextBox.Text;

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



            // Update the ProductToEdit object with the new values
            ProductToEdit.Name = productName;
            ProductToEdit.Description = productDescription;
            ProductToEdit.Manufacturer = productManufacturer;
            ProductToEdit.Count = int.Parse(productCount);
            ProductToEdit.PathImage = imagePath;

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

