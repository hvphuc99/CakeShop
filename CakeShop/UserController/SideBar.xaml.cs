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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CakeShop.UserController
{
    /// <summary>
    /// Interaction logic for SideBar.xaml
    /// </summary>
    /// 
    public partial class SideBar : UserControl
    {
        public delegate void AddCakeScreenDelegate();
        public event AddCakeScreenDelegate AddCakeScreen;
        public delegate void AddOrderScreenDelegate();
        public event AddCakeScreenDelegate AddOrderScreen;
        public SideBar()
        {
            InitializeComponent();
        }
        private void btnAddCake_Click(object sender, RoutedEventArgs e)
        {
            AddCakeScreen();
        }

        private void BtnOrderScreen_Click(object sender, RoutedEventArgs e)
        {
            AddOrderScreen();
        }
    }
   
    
}
