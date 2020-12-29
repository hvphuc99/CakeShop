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
using LiveCharts;
using LiveCharts.Wpf;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for Statistical.xaml
    /// </summary>
    public partial class Statistical : Window
    {
        public Statistical()
        {
            InitializeComponent();

            //Draw total price in months
            var db = new cakeShopEntities();
            var totalPricesMonthsDB = db.orders
                .GroupBy(record => new { Month = record.payDate.Value.Month })
                .Select(record => new { record.Key, total = record.Sum(item => item.totalPrice) })
                .ToList();

            int[] totalPriceMonths = new int[12];

            for (int i = 0; i < 12; i++)
            {
                int totalPrice = 0;
                foreach (var data in totalPricesMonthsDB)
                {
                    if (i + 1 == data.Key.Month)
                    {
                        totalPrice = data.total;
                    }
                }
                totalPriceMonths[i] = totalPrice;
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "",
                    Values = new ChartValues<int>(totalPriceMonths),
                }
            };

            Labels = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            MonthChartData.Series = SeriesCollection;
            MonthChartLabel.Labels = Labels;


            //Draw pie
            var listCakeTypes = db.cakeTypes.ToArray();
            //var dataPieChart = db.orderDetails
            //   .Join(
            //        db.orders,
            //        entryPoint1 => entryPoint1.order_id,
            //        entry => entry.id,
            //        (entryPoint1, entry) => new { entryPoint1, entry }
            //    )
            //   .Join(
            //        db.cakes,
            //        entryPoint2 => entryPoint2.entryPoint1.cake_id,
            //        entry => entry.id,
            //        (entryPoint2, entry) => new { entryPoint2, entry }
            //    )
            //   .Join(
            //        db.cakeTypes,
            //        entryPoint3 => entryPoint3.entry.type_id,
            //        entry => entry.id,
            //        (entryPoint3, entry) => new { entryPoint3, entry }
            //    )
            //   .GroupBy(record => new { Month = record.entryPoint3.entryPoint2.entry.payDate.Value.Month })
            //   .ToArray();

            //Todo: get list data
            //{
            //    month
            //    cake type
            //    total Price
            //}

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            PieSeries a = new PieSeries
            {
                Title = "test 1",
                Values = new ChartValues<double> { 1 },
                DataLabels = true,
                LabelPoint = PointLabel,
            };
            PieSeries b = new PieSeries
            {
                Title = "test 2",
                Values = new ChartValues<double> { 1 },
                DataLabels = true,
                LabelPoint = PointLabel,
            };
            PieChart.Series.RemoveAt(0);
            PieChart.Series.Add(a);
            PieChart.Series.Add(b);
        }

        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        private void BackHomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
