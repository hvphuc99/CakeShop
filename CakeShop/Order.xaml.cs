using CakeShop.DTO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        static List<CakeDto> cart = new List<CakeDto>();
        static List<CakeDto> cakeDtos = new List<CakeDto>();
        static List<CakeDto> purchase = new List<CakeDto>();
        static CultureInfo culture = new CultureInfo("en-US");
        public Order()
        {
            InitializeComponent();
            loadCakes();
        }
        private void loadCakes()
        {
            using (var db = new cakeShopEntities())
            {
                string query;
                query = "Select id, name, photo, description,type_id, price,quantity from cakes ";
                List<cake> cakes = db.cakes.SqlQuery(query).ToList();
                if (cakes.Count == 0)
                    return;
                cakeDtos = convertCulteralInfoPrice(cakes);
                //lvCakes.ItemsSource = cakeDtos;
                dgvCakes.ItemsSource = cakeDtos;
                initCart();
            }
        }
        private List<CakeDto> convertCulteralInfoPrice(List<cake> cakes)
        {

            List<CakeDto> Dtos = new List<CakeDto>();

            foreach (cake cake in cakes)
            {
                CakeDto cakeDto = new CakeDto(cake.id, cake.name, cake.photo, cake.description, cake.price.ToString("c", culture), cake.quantity, cake.type_id);
                Dtos.Add(cakeDto);
            }
            return Dtos;
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IntegerUpDown integerUpDown = (IntegerUpDown)sender;
            if (integerUpDown.Tag != null)
            {
                int index = (int)integerUpDown.Tag;
                updateQuantity(index, (int)integerUpDown.Value);
            }

        }
        private void updateQuantity(int id, int quantity)
        {
            foreach (CakeDto cakeDto in cart)
            {
                if (cakeDto.Id == id)
                {
                    cakeDto.Quantity = quantity;
                    break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            foreach (CakeDto cakeDto in cart)
            {
                if (cakeDto.Id == (int)button.Tag)
                {
                    if (addToCart((int)button.Tag))
                    {
                        dgvCart.ItemsSource = null;
                        dgvCart.ItemsSource = purchase;
                        tbxSubtotal.Text = subtotal()?.ToString("c", culture);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Not enough amount!");
                    }
                    break;
                }
            }
        }
        private int updateIfExist(int id)
        {
            foreach (CakeDto Dto in purchase)
            {
                if (Dto.Id == id)
                {
                    Dto.Quantity = Dto.Quantity + findQuantityById(id);
                    if (Dto.Quantity > findById(cakeDtos, id).Quantity)
                    {
                        return 0;
                    }
                    return 1;
                }

            }
            return 2;
        }
        private int findQuantityById(int id)
        {
            foreach (CakeDto cakeDto in cart)
            {
                if (cakeDto.Id == id)
                    return cakeDto.Quantity;
            }
            return 0;
        }
        private void initCart()
        {
            foreach (CakeDto cakeDto in cakeDtos)
            {
                CakeDto Dto = new CakeDto(cakeDto);
                Dto.Quantity = 1;
                cart.Add(Dto);
            }
        }
        private CakeDto findById(List<CakeDto> dtos, int id)
        {
            foreach (CakeDto dto in dtos)
            {
                if (dto.Id == id)
                    return dto;
            }
            return null;
        }
        private bool addToCart(int id)
        {
            if (findById(purchase, id) == null)
            {
                if (findById(cart, id).Quantity > findById(cakeDtos, id).Quantity)
                {
                    return false;
                }
                CakeDto dto = new CakeDto(findById(cart, id));
                if (dto.Quantity != 1)
                {
                    int? price = findCakeById(id).price;

                    dto.Price = (price * dto.Quantity)?.ToString("c", culture);
                }
                purchase.Add(dto);
            }
            else
            {
                int count = findById(purchase, id).Quantity + findById(cart, id).Quantity;
                if (count > findById(cakeDtos, id).Quantity)
                    return false;
                findById(purchase, id).Quantity = count;
                int? price = findCakeById(id).price;
                CultureInfo culture = new CultureInfo("en-US");
                string culteralPrice = (price * count)?.ToString("c", culture);
                findById(purchase, id).Price = culteralPrice;
            }
            return true;
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
        private int? subtotal()
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

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int id = (int)button.Tag;
            purchase.Remove(findById(purchase, id));
            dgvCart.ItemsSource = null;
            dgvCart.ItemsSource = purchase;
            tbxSubtotal.Text = subtotal()?.ToString("c", culture);
        }

        private void BtnCloseOrder_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            //screen info
            if (purchase.Count == 0) return;
           
            CultureInfo culture = new CultureInfo("en-US");
            string price = subtotal()?.ToString("c",culture); 
            Confirm confirm = new Confirm(purchase,price);
            this.Hide();
            confirm.ShowDialog();
            if (purchase.Count == 0)
                refeshCart();
            this.ShowDialog();
        }
        private void refeshCart()
        {
            dgvCart.ItemsSource = null;
            tbxSubtotal.Text = "";
        }

        private void BtnBought_Click(object sender, RoutedEventArgs e)
        {
            Bought bought = new Bought();
            this.Hide();
            bought.ShowDialog();
            this.ShowDialog();
        }
        //private void DgvCakes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    DataGrid gd = (DataGrid)sender;

        //    CakeDto a =  gd.Items.CurrentItem as CakeDto;



        //}


    }
}
