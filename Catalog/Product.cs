using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Windows.Media;

namespace Catalog
{
    public class Product
    {
        public int ID { get; set; }
        public int Number_product {get;set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public string PathImage { get; set; }
        public int Count { get; set; }

        public string Manufacturer { get; set; }

        [NotMapped]
        public SolidColorBrush Color { get; set; }
        public float Opacity
        {
            get
            {
                return Count > 0 ?  1.0f : 0.70f;
            }
        }

        public string Description_name
        {
            get
            {
                return "Description " + Name;
            }
        }

        public Product(int Number_product, string name, string description, string manufacturer, int count, string pathImage = "C:\\Users\\artem\\OneDrive\\Документы\\ShareX\\Screenshots\\2024-09\\Telegram_uxnN8sKEy4.png")
        {
            this.Number_product = Number_product;
            Count = count;
            Name = name;
            Description = description;
            PathImage = pathImage;
            Manufacturer = manufacturer;

        }
        public Product( int Number_product, string name, string description, string manufacturer, int count, SolidColorBrush color = null, string pathImage = "C:\\Users\\artem\\OneDrive\\Документы\\ShareX\\Screenshots\\2024-09\\Telegram_uxnN8sKEy4.png")
        {
            this.Number_product = Number_product; 
            Count = count;
            if (count > 0)
            {
                Color = color ?? new SolidColorBrush(System.Windows.Media.Color.FromRgb(32, 33, 36));
            }
            else 
            {
                Color = color ?? new SolidColorBrush(System.Windows.Media.Color.FromRgb(46, 50, 59));
            }
           
            Name = name;
            Description = description;
            PathImage = pathImage;
            Manufacturer = manufacturer;
                                                        
        }
        
    }

}
