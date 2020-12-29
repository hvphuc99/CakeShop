using CakeShop.DTO;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadCakes(0,1);
            loadCakeTypes();
        }
        void loadCakes(int index,int option)
        {
            using (var db = new cakeShopEntities())
            {
                string query ;
                if (option == 1)
                {
                    query = "Select id, name, photo, description,type_id, price,quantity from cakes where id > " + index;
                }
                else query = "Select Top 12 id, name, photo, description,type_id, price,quantity from cakes where id < " + index + " ORDER BY id DESC";
                List<cake> cakes = db.cakes.SqlQuery(query).Take(12).ToList();
                if (option != 1) cakes.Reverse();
                if (cakes.Count == 0)
                    return;
                List<CakeDto> cakeDtos = convertCulteralInfoPrice(cakes);
                ListCake.ItemsSource = cakeDtos;
                ListCake.Tag = new page(cakes[0].id,cakes[cakes.Count-1].id);              
            }
        }
        public void loadCakeTypes()
        {
            using (var db = new cakeShopEntities())
            {
                List<cakeType> cakeTypes = db.cakeTypes.SqlQuery("Select id, name from cakeTypes").ToList();
                cakeTypes.Add(new cakeType() { id = 0, name = "All" });
                cbxtypeCakes.ItemsSource = cakeTypes;

            }
        }
        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = (ListCake.Tag as page).FinalIndex;
            if (cbxtypeCakes.SelectedValue==null || (cbxtypeCakes.SelectedValue as cakeType).id == 0)
            {
                loadCakes(currentIndex, 1);
                return;
            }
            int cakeType = (cbxtypeCakes.SelectedValue as cakeType).id;
            loadCakesByType(cakeType, 1, currentIndex);    
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = (ListCake.Tag as page).FirstIndex;
            if (cbxtypeCakes.SelectedValue == null || (cbxtypeCakes.SelectedValue as cakeType).id == 0)
            {
                loadCakes(currentIndex, 2);
                return;
            }
            int cakeType = (cbxtypeCakes.SelectedValue as cakeType).id;
            loadCakesByType(cakeType, 2, currentIndex);
        }
        private void loadCakesByType(int cakeType,int option, int index)
        {
            using (var db = new cakeShopEntities())
            {
                string query;
                if (option == 1)
                {
                    query = "Select id, name, photo, description,type_id, price,quantity from cakes where id > " + index + " and type_id = "+cakeType;
                }
                else query = "Select Top 12 id, name, photo, description,type_id, price,quantity from cakes where id < " + index + " and type_id = " + cakeType + " ORDER BY id DESC";
                List<cake> cakes = db.cakes.SqlQuery(query).Take(12).ToList();
                if (option != 1) cakes.Reverse();
                if (cakes.Count == 0)
                    return;
                List<CakeDto> cakeDtos = convertCulteralInfoPrice(cakes);
                ListCake.ItemsSource = cakeDtos;
                ListCake.Tag = new page(cakes[0].id, cakes[cakes.Count - 1].id);
            }
        }
        private List<CakeDto> convertCulteralInfoPrice(List<cake> cakes)
        {
            CultureInfo culture = new CultureInfo("en-US");
            List<CakeDto> cakeDtos = new List<CakeDto>();
            
            foreach (cake cake in cakes)
            {              
                CakeDto cakeDto = new CakeDto(cake.id,cake.name,cake.photo,cake.description, cake.price?.ToString("c", culture),cake.type_id);
                cakeDtos.Add(cakeDto);
            }
            return cakeDtos;
        }
        private void CbxtypeCakes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int cakeType_id = (cbxtypeCakes.SelectedValue as cakeType).id;
            if(cakeType_id == 0)
            {
                loadCakes(0, 1);
                return;
            }
            loadCakesByType(cakeType_id, 1, 0);
        }

        private void Btnxemchitiet_Click(object sender, RoutedEventArgs e)
        {
            var btncake = (Button)sender;
            cake targetcake = null;
            using (var db = new cakeShopEntities())
            {
               //targetcake = db.cakes.Where(c => c.id == (int)btncake.Tag).FirstOrDefault();
               targetcake = db.cakes.SqlQuery("select * from cakes where id = @id",new SqlParameter("@id", (int)btncake.Tag)).FirstOrDefault();
            }
            if(targetcake!=null)
            {
                CakeDetail detail = new CakeDetail(targetcake);
                this.Hide();
                detail.ShowDialog();
                this.Show();
            }
        }
    }
}
