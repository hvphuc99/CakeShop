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
using System.Configuration;
using System.Diagnostics;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
            var showSplash = bool.Parse(value);
            if (showSplash == false)
            {
                MainWindow mainWindow = new MainWindow();

                mainWindow.Show();
                this.Close();
            }
            InitializeComponent();
            //RecipeBUS.Instance.loadSplashWindow(SplashEllipse, tbxIntroduction);
            using(var db = new cakeShopEntities())
            {
                List<cake> cakes = db.cakes.ToList();
                Random random = new Random();
                int index = random.Next(cakes.Count);
                var folder = AppDomain.CurrentDomain.BaseDirectory;
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(folder + "/Assets/" + cakes[index].photo));
                SplashEllipse.Fill = imageBrush;
                if (cakes[index].description.Length > 280)
                {
                    cakes[index].description = cakes[index].description.Substring(0, 280);
                    cakes[index].description = cakes[index].description + " ...";
                }
                tbxIntroduction.Text = cakes[index].description;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(checkBox.IsChecked==true)
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
                config.Save(ConfigurationSaveMode.Minimal);
            }
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
            var showSplash = bool.Parse(value);
            if(showSplash==false)
            {
                MainWindow mainWindow = new MainWindow();

                mainWindow.Show();
                this.Close();
            }
        }

        
    }
}
