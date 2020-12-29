using System;
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
        private cake currentCakeDetail;
        public CakeDetail(cake cakeDetail)
        {
            InitializeComponent();
            loadDetail(cakeDetail);
            currentCakeDetail = cakeDetail;
        }
        private void loadDetail(cake cakeDetail)
        {
            
            CultureInfo culture = new CultureInfo("en-US");
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var photoPath = $"{folder}/Assets/{cakeDetail.photo}";
            var bitmap = new BitmapImage(new Uri(photoPath));
            cakeImage.ImageSource = bitmap;
            cakeName.Text = cakeDetail.name;
            using (var db = new cakeShopEntities())
            {
                cakeType.Text = db.cakeTypes.SqlQuery("select * from cakeTypes where id = @id",new SqlParameter("@id",cakeDetail.type_id)).FirstOrDefault().name;
            }           
            cakePrice.Text = cakeDetail.price.ToString("c", culture);
            cakeDescription.Text = cakeDetail.description;

        }
        private void CloseDetail_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateCakeButton_Click(object sender, RoutedEventArgs e)
        {
            var updateCake = new UpdateCake(currentCakeDetail);
            this.Hide();
            updateCake.ShowDialog();
            loadDetail(currentCakeDetail);  
            this.ShowDialog();
        }
    }
}
