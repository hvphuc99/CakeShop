﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for CakeDetail.xaml
    /// </summary>
    public partial class CakeDetail : Window
    {
        public CakeDetail(cake cakeDetail)
        {
            InitializeComponent();
            loadDetail(cakeDetail);
        }
        private void loadDetail(cake cakeDetail)
        {
            
            CultureInfo culture = new CultureInfo("en-US");
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri($"pack://application:,,,/CakeShop;component{cakeDetail.photo}");
            bitmapImage.EndInit();
            cakeImage.ImageSource = bitmapImage;
            cakeName.Text = cakeDetail.name;
            using (var db = new cakeShopEntities())
            {
                cakeType.Text = db.cakeTypes.SqlQuery("select * from cakeTypes where id = @id",new SqlParameter("@id",cakeDetail.type_id)).FirstOrDefault().name;
            }           
            cakePrice.Text = cakeDetail.price?.ToString("c", culture);
            cakeDescription.Text = cakeDetail.description;

        }
        private void CloseDetail_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}