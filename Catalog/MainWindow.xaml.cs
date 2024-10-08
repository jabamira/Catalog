using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
using System.Xml.Schema;

namespace Catalog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<string> Manufacturers { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Sorts { get; set; } = new ObservableCollection<string>();

        public int temp_ind = -1;
        int _currentPage = 0;
        int _pageSize = 8;
        private int _totalProducts;
        private string _currentSort = "None";
        private string _searchTerm = "";
        private string _selectedManufacturer = "None";

        public MainWindow()
        {
            InitializeComponent();


            Products = new ObservableCollection<Product>();
            _totalProducts = GetTotalProductsFromDatabase();




            Sorts.Add("None");
            Sorts.Add("By name");
            Sorts.Add("By Manufacturer");
            LoadPage(0);
            UpdateManufacturers();

            listView.DataContext = this;

            Sort_box.ItemsSource = Sorts;
            Sort_box.SelectedIndex = 0;

            Manufacturer_box.ItemsSource = Manufacturers;
            Manufacturer_box.SelectedIndex = 0;
            Sort_box.SelectionChanged += Sort_box_SelectionChanged;
            Manufacturer_box.SelectionChanged += Manufacturer_box_SelectionChanged;
            Search_box.TextChanged += Search_box_TextChanged;


        }



        private void LoadPage(int pageNumber)
        {

            ApplyFilteringAndSorting(pageNumber);

        }
        private void Update_totalProducts()
        {
            _totalProducts = GetTotalProductsFromDatabase();
            all_product_label.Text = _totalProducts.ToString() + " items in stock";
        }
        private void DeleteProduct(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                Product selectedProduct = (Product)listView.SelectedItem;

                using (var context = new MyDbContext())
                {


                    var productToDelete = context.Products_.Find(selectedProduct.Number_product);
                    if (productToDelete != null)
                    {
                        Debug.WriteLine(productToDelete.Name);
                        context.Products_.Remove(productToDelete);
                        context.SaveChanges();
                    }
                }

                Products.Remove(selectedProduct);
                UpdateManufacturers();
            }
        }

        private void UpdateManufacturers()
        {
            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
                var uniqueManufacturers = context.Products_.Select(p => p.Manufacturer).Distinct().ToList();


                Manufacturers.Clear();
                Manufacturers.Add("None");
                foreach (var manufacturer in uniqueManufacturers)
                {
                    if (!Manufacturers.Contains(manufacturer))
                    {
                        Manufacturers.Add(manufacturer);

                    }


                }
                Manufacturer_box.SelectedIndex = 0;



            }
            Update_totalProducts();



        }




        // ref
        private void exit_btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = listView.SelectedItem;
            if (selectedItem != null)
            {
                if (temp_ind != -1 && temp_ind < Products.Count)
                {
                    if (Products[temp_ind].Count > 0)
                    {
                        Products[temp_ind].Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(32, 33, 36));
                    }
                    else
                    {
                        Products[temp_ind].Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(46, 50, 59));
                    }
                }

                int index = listView.Items.IndexOf(selectedItem);


                if (index >= 0 && index < Products.Count)
                {
                    temp_ind = index;

                    Products[index].Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(18, 19, 20));

                    Debug.WriteLine($"Index: {index}");
                    Debug.WriteLine(Products[index].Color);

                    listView.Items.Refresh();
                }
            }
        }


        private void LoadPreviousPage(object sender, RoutedEventArgs e)
        {
            temp_ind = -1;
            if (_currentPage > 0)
            {
                LoadPage(_currentPage - 1);

            }

        }

        private void Sort_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentSort = (string)Sort_box.SelectedItem; // Store the current sort order
            LoadPage(0); // Reload the current page with the new sort
        }
        private void Manufacturer_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedManufacturer = (string)Manufacturer_box.SelectedItem;

            ApplyFilteringAndSorting();
        }
        private void Search_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            _searchTerm = Search_box.Text;
            ApplyFilteringAndSorting();
        }
        private void ApplyFilteringAndSorting(int pageNumber = 0) // Add default parameter for LoadPage
        {
            Products.Clear();

            using (var context = new MyDbContext())
            {
                IQueryable<Product> query = context.Products_;

                // Apply Manufacturer filter
                if (_selectedManufacturer != "None")
                {
                    query = query.Where(p => p.Manufacturer == _selectedManufacturer);
                }

                // Apply search term
                if (!string.IsNullOrEmpty(_searchTerm))
                {
                    query = query.Where(p => p.Name.ToLower().Contains(_searchTerm.ToLower()));
                }

                // Apply sorting based on the current sort order
                if (_currentSort == "By name")
                {
                    query = query.OrderBy(p => p.Name);
                }
                else if (_currentSort == "By Manufacturer")
                {
                    query = query.OrderBy(p => p.Manufacturer);
                }

                // Apply pagination
                var pageProducts = query.Skip(pageNumber * _pageSize).Take(_pageSize).ToList();

                foreach (var product in pageProducts)
                {
                    Products.Add(new Product(product.ID, product.Name, product.Description, product.Manufacturer, product.Count, null, product.PathImage));
                }
            }

            _currentPage = pageNumber;
            string Page_text = (pageNumber + 1).ToString();
            Page_label.Text = $"Page: {Page_text}";
        }
        private void LoadNextPage(object sender, RoutedEventArgs e)
        {
            temp_ind = -1;

            if ((_currentPage + 1) * _pageSize < _totalProducts)
            {
                LoadPage(_currentPage + 1);
            }
        }
        private int GetTotalProductsFromDatabase()
        {
            using (var context = new MyDbContext())
            {
                context.Database.EnsureCreated();
                return context.Products_.Count();
            }
        }

        private void AddNewProduct(object sender, RoutedEventArgs e)
        {

            AddProductWindow addProductWindow = new AddProductWindow();


            addProductWindow.Owner = this;

            addProductWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;





            if (addProductWindow.ShowDialog() == true)
            {

                Product newProduct = addProductWindow.NewProduct;


                using (var context = new MyDbContext())
                {
                    context.Products_.Add(newProduct);
                    context.SaveChanges();
                }


                Products.Add(newProduct);

                UpdateManufacturers();
                LoadPage(_currentPage);
            }
        }
        private void EditProduct(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                Product selectedProduct = (Product)listView.SelectedItem;

                // Create the EditProductWindow and pass the selected product
                EditProductWindow editWindow = new EditProductWindow(selectedProduct);
                editWindow.Owner = this;
                editWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                // Show the EditProductWindow as a dialog
                if (editWindow.ShowDialog() == true)
                {
                    // Update the database
                    using (var context = new MyDbContext())
                    {
                        context.Database.EnsureCreated();
                        // Find the product in the database using its ID
                        var productToUpdate = context.Products_.Find(selectedProduct.Number_product);

                        if (productToUpdate != null)
                        {
                           
                            productToUpdate.Name = editWindow.ProductToEdit.Name;
                            productToUpdate.Description = editWindow.ProductToEdit.Description;
                            productToUpdate.Manufacturer = editWindow.ProductToEdit.Manufacturer;
                            productToUpdate.Count = editWindow.ProductToEdit.Count;
                            productToUpdate.PathImage = editWindow.ProductToEdit.PathImage;

                            // Save the changes to the database
                            context.SaveChanges();

                            // Update the ObservableCollection in MainWindow
                            int index = Products.IndexOf(selectedProduct);
                            if (index != -1)
                            {
                                // Replace the existing product with the updated one
                                Products[index] = editWindow.ProductToEdit;
                            }
                        }
                        else
                        {
                            // Handle the case where the product wasn't found (e.g., show an error message)
                            MessageBox.Show("Product not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    // Optionally, reload the current page to reflect the changes
                    // LoadPage(_currentPage); 
                }
            }
        }


    }
}