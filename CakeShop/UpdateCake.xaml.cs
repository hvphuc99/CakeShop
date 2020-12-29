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
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for AddCake.xaml
    /// </summary>
    public partial class UpdateCake : Window
    {
        private string photo = null;
        private cake currentCakeDetail;
        public UpdateCake(cake cakeDetail)
        {
            InitializeComponent();

            currentCakeDetail = cakeDetail;
            loadCakeDetail();
        }

        public void loadCakeDetail()
        {
            CakeName.Text = currentCakeDetail.name;
            CakeDecription.Text = currentCakeDetail.description;
            CakePrice.Text = currentCakeDetail.price.ToString();

            photo = currentCakeDetail.photo;
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var photoPath = $"{folder}/Assets/{photo}";
            var bitmap = new BitmapImage(new Uri(photoPath));

            uploadPhotoStackPanel.Children.Remove(textUploadPhoto);

            imageUploadPhoto.Source = bitmap;
            imageUploadPhoto.Source = bitmap;
            imageUploadPhoto.Width = 450;
            imageUploadPhoto.Height = 200;

            fetchListCakeType(currentCakeDetail.type_id);
            ComboBoxCakeType.DisplayMemberPath = "name";
        }

        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public void fetchListCakeType(int typeID)
        {
            var db = new cakeShopEntities();
            var listCakeTypes = db.cakeTypes.ToList();
            foreach (var cakeType in listCakeTypes)
            {
                ComboBoxCakeType.Items.Add(cakeType);
                if (cakeType.id == typeID)
                {
                    ComboBoxCakeType.SelectedItem = cakeType;
                }
            }
        }

        private void CakePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openImageDialog = new OpenFileDialog();
            openImageDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openImageDialog.ShowDialog() == true)
            {
                photo = openImageDialog.FileName;
                var bitmap = new BitmapImage(new Uri(photo));

                uploadPhotoStackPanel.Children.Remove(textUploadPhoto);

                imageUploadPhoto.Source = bitmap;
                imageUploadPhoto.Width = 450;
                imageUploadPhoto.Height = 200;
            }
        }

        private void UpdateCakeButton_Click(object sender, RoutedEventArgs e)
        {
            if (CakeName.Text != "" && CakePrice.Text != "" && photo != "" && ComboBoxCakeType.SelectedItem != null)
            {
                var db = new cakeShopEntities();
                var cake = db.cakes.Find(currentCakeDetail.id);
                cake.name = CakeName.Text;
                cake.description = CakeDecription.Text;
                cake.price = Int32.Parse(CakePrice.Text);
                var typeID = (ComboBoxCakeType.SelectedItem as dynamic).id;
                cake.cakeType = db.cakeTypes.Find(typeID);
                cake.type_id = typeID;

                currentCakeDetail.name = CakeName.Text;
                currentCakeDetail.description = CakeDecription.Text;
                currentCakeDetail.price = Int32.Parse(CakePrice.Text);
                currentCakeDetail.cakeType = db.cakeTypes.Find(typeID);
                currentCakeDetail.type_id = typeID;

                string[] listString = photo.Split('\\');
                if (listString[0] != photo)
                {
                    string imageName = listString.Last();

                    var folder = AppDomain.CurrentDomain.BaseDirectory;
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var newImageName = unixTimestamp.ToString() + "_" + imageName;
                    var path = System.IO.Path.Combine(folder, "Assets", newImageName);
                    File.Copy(photo, path, true);

                    cake.photo = newImageName;

                    currentCakeDetail.photo = newImageName;
                }

                db.SaveChanges();
                MessageBox.Show("Update cake successful !", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Name, Price, Type and Photo fields are mandatory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void CakePrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void CancelUpdateCakeButton_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }
    }
}
