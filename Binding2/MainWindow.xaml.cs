using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Binding2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            List<Plane> planeList = new List<Plane>()
            {
                new Plane(){CateGory=Category.Bomber,Name="B-1",State=State.Unknown},
                new Plane(){CateGory=Category.Bomber,Name="B-2",State=State.Unknown},
                new Plane(){CateGory=Category.Fighter,Name="F-22",State=State.Unknown},
                new Plane(){CateGory=Category.Fighter,Name="SU-47",State=State.Unknown},
                new Plane(){CateGory=Category.Bomber,Name="B-52",State=State.Unknown},
                new Plane(){CateGory=Category.Fighter,Name="J-10",State=State.Unknown}
            };
            this.listBoxPlane.ItemsSource = planeList;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Plane p in listBoxPlane.Items)
            {
                sb.AppendLine(string.Format("Category={0},Name={1},State={2}", p.CateGory, p.Name, p.State));
            }
            File.WriteAllText(@"D:\temp\plane.txt", sb.ToString());
        }
    }
    public enum Category
    {
        Bomber,Fighter
    }
    public enum State
    {
        Available,Locked,Unknown
    }
    public class Plane
    {
        public Category CateGory { get; set; }
        public string Name { get; set; }
        public State State { get; set; }
    } 
    public class CategoryToSourceConverter:IValueConverter
    {
        public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
        {
            Category c = (Category)value;
            switch(c)
            {
                case Category.Bomber:
                    //return new BitmapImage(new Uri(@"\Icons\Bomber.png",UriKind.Absolute));
                    return @"\Icons\Bomber.png";
                case Category.Fighter:
                    //return new BitmapImage(new Uri(@"\Icons\Fighter.png",UriKind.Relative));
                    return @"\Icons\Fighter.png";
                default:
                    return null;
            }
        }
        public object ConvertBack(object value,Type targetType,object parameter,CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StateToNullableBoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            State s = (State)value;
            switch (s)
            {
                case State.Available:
                    return true;
                case State.Locked:
                    return false;
                case State.Unknown:
                default:
                    return null;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? nb = (bool?)value;
            switch (nb)
            {
                case true:
                    return State.Available;
                case false:
                    return State.Locked;
                case null:
                default:
                    return State.Unknown;
            }
        }
    }
}
