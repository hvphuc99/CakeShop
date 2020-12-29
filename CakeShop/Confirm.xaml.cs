using CakeShop.DTO;
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
    /// Interaction logic for Confirm.xaml
    /// </summary>
    public partial class Confirm : Window
    {

        public Confirm(List<CakeDto> cakeDtos,string subtotal)
        {
            InitializeComponent();
            dgvOrders.ItemsSource = cakeDtos;
            tbxSubtotal.Text = subtotal;
            btnConfirmOk.Tag = cakeDtos;
        }

        private void BtnCloseConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnConfirmOk_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            List<CakeDto> cakeDtos = button.Tag as List<CakeDto>;
            addOrderDB(cakeDtos);
            cakeDtos.Clear();
            this.Close();
        }
        private void addOrderDB(List<CakeDto> cakeDtos)
        {
            //add order
            DateTime? dateTime = FutureDatePicker.SelectedDate;
            int? price = subtotal(cakeDtos);
            order order = new order() { custumer = tbxNameCus.Text, phone = tbxPhone.Text, address=tbxAddress.Text,payDate=dateTime,totalPrice=price};
            using (var db = new cakeShopEntities())
            {
                db.orders.Add(order);
                db.SaveChanges();
            }
            //add order details
            foreach(CakeDto cakeDto in cakeDtos)
            {
                int? Price = findCakeById(cakeDto.Id).price * cakeDto.Quantity;
                orderDetail orderDetail = new orderDetail() {amount = cakeDto.Quantity,totalPrice = price,cake_id=cakeDto.Id,order_id = order.id };
                using (var db = new cakeShopEntities())
                {
                    db.orderDetails.Add(orderDetail);
                    db.SaveChanges();
                }
            }
        }
        private int? subtotal(List<CakeDto> purchase)
        {
            int? subtotal = 0;
            if (purchase.Count != 0)
            {
                foreach (CakeDto cakeDto in purchase)
                {
                    subtotal = subtotal + (findCakeById(cakeDto.Id).price * cakeDto.Quantity);
                }
            }
            return subtotal;
        }
        private cake findCakeById(int id)
        {
            cake cake = null;
            using (var db = new cakeShopEntities())
            {
                cake = db.cakes.Where(c => c.id == id).FirstOrDefault();
            }
            return cake;
        }
    }
}
