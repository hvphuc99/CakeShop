using CakeShop.DTO;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            drawColumnChart();
            drawPieChart(0);
        }

        public void drawColumnChart()
        {
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
        }

        public void drawPieChart(int month)
        {
            //Draw pie
            var db = new cakeShopEntities();
            var listCakeTypes = db.cakeTypes.ToArray();

            var months = new List<List<Revenue>>();
            months = revenues();

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            if (PieChart != null)
            {
                PieChart.Series.Clear();

                for (int i = 0; i < listCakeTypes.Length; i++)
                {
                    PieSeries pie = new PieSeries
                    {
                        Title = months[month][i].TypeName,
                        Values = new ChartValues<int> { months[month][i].TotalPrice ?? default(int) },
                        DataLabels = true,
                        LabelPoint = PointLabel,
                    };
                    PieChart.Series.Add(pie);
                }
            }
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
        private List<List<Revenue>> revenues()
        {
            
            using (var db = new cakeShopEntities())
            {
                var listRevenueDB = (from ol in db.orderDetails
                                   join or in db.orders
                                    on ol.order_id equals or.id
                                   join c in db.cakes
                                    on ol.cake_id equals c.id
                                   join ct in db.cakeTypes
                                    on c.type_id equals ct.id
                                   group new { ct.id, ct.name, ol.totalPrice } by new { or.payDate.Value.Month, ct.id, ct.name } into g
                                   select new
                                   {
                                       typeId = g.Key.id,
                                       typeName = g.Key.name,
                                       month = g.Key.Month,
                                       totalPrice = g.Sum(x => x.totalPrice),
                                   }
                                    ).ToArray();

                var listRevenue = new List<Revenue>();
                for (int i = 0; i < listRevenueDB.Length; i++)
                {
                    Revenue revenue = new Revenue(listRevenueDB[i].typeId, listRevenueDB[i].typeName, listRevenueDB[i].month, listRevenueDB[i].totalPrice);
                    listRevenue.Add(revenue);
                }

                var listTypes = db.cakeTypes.ToArray();
                var months = new List<List<Revenue>>();
                for (int i = 1; i <= 12; i++)
                {
                    var arr = new List<Revenue>();
                    for (int j = 0; j < listRevenue.Count; j++)
                    {
                        if (listRevenue[j].Month == i)
                        {
                            arr.Add(listRevenue[j]);
                        }
                    }

                    for (int j = 0; j < listTypes.Length; j++)
                    {
                        var isExist = false;
                        foreach(Revenue revenue in arr)
                        {
                            if (listTypes[j].id == revenue.TypeId)
                            {
                                isExist = true;
                            }
                        }

                        if (!isExist)
                        {
                            Revenue revenue = new Revenue(listTypes[j].id, listTypes[j].name, i, 0);
                            arr.Add(revenue);
                        }
                    }

                    months.Add(arr);
                }
    
                if (months != null)
                    return months;
            }
            return null;
        }

        private void cbxMonth_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedMonthString = ((ComboBoxItem)cbxMonth.SelectedItem).Tag.ToString();
            int selectedMonth = Int32.Parse(selectedMonthString);
            drawPieChart(selectedMonth);
        }
    }
}
