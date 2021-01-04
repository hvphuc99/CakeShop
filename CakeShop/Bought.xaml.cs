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

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for Bought.xaml
    /// </summary>
    public partial class Bought : Window
    {
        public Bought()
        {
            InitializeComponent();
            loadOrders();
        }
        void loadOrders()
        {
            using (var db = new cakeShopEntities())
            {
                List<order> orders = db.orders.ToList();
                dgvOrders.ItemsSource = orders;
            }
        }

        private void DataGridRow_Selected(object sender, RoutedEventArgs e)
        {
            DataGridRow dataGridRow = (DataGridRow)sender;
            int id = (int)dataGridRow.Tag;
            List<orderDetail>  orderDetails = null;
            using (var db = new cakeShopEntities())
            {
                orderDetails = db.orderDetails.Include("cake").Where(o => o.order_id == id).ToList();
            }
            if(orderDetails != null)
            {
                dgvOrderDetail.ItemsSource = null;
                dgvOrderDetail.ItemsSource = orderDetails;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
