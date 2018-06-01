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
using System.ComponentModel;
using System.Globalization;

namespace Binding1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Student stu;
        List<Student1> stuList1 = new List<Student1>()
            {
                new Student1(){Id=0,Name="Tim",Age=29},
                new Student1(){Id=1,Name="Tom",Age=28},
                new Student1(){Id=2,Name="Bob",Age=27},
                new Student1(){Id=3,Name="Bib",Age=26},
                new Student1(){Id=4,Name="Tony",Age=25},
                new Student1(){Id=5,Name="Mike",Age=24}
            };
        public MainWindow()
        {
            InitializeComponent();
            SetBinding();
            //RelativeSource();
            stu = new Student();

            Binding binding = new Binding();
            binding.Source = stu;
            binding.Path = new PropertyPath("Name");

            BindingOperations.SetBinding(this.textBoxName, TextBox.TextProperty, binding);
            
            this.listBoxStudents.ItemsSource = stuList1;
            //this.listBoxStudents.DisplayMemberPath ="Name";

            Binding binding1 = new Binding("SelectedItem.Id") { Source = this.listBoxStudents };
            this.textBoxId.SetBinding(TextBox.TextProperty, binding1);

            Binding bindingSlider = new Binding("Value") { Source = this.slider1 };
            bindingSlider.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            RangeValidationRule rvr = new RangeValidationRule();
            rvr.ValidatesOnTargetUpdated = true;
            bindingSlider.ValidationRules.Add(rvr);
            bindingSlider.NotifyOnValidationError = true;
            this.textBox1.SetBinding(TextBox.TextProperty, bindingSlider);
            //Text="{Binding Path=Value,ElementName=slider1,UpdateSourceTrigger=PropertyChanged}"
            this.textBox1.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(this.ValidationError));
        }
        void ValidationError(object sender,RoutedEventArgs e)
        {
            if(Validation.GetErrors(this.textBox1).Count>0)
            {
                this.textBox1.ToolTip = Validation.GetErrors(this.textBox1)[0].ErrorContent.ToString();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            stu.Name += "name";
        }
        
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            this.listViewStudent.ItemsSource = stuList1;
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void SetBinding()
        {
            ObjectDataProvider odp = new ObjectDataProvider();
            odp.ObjectInstance = new Calculator();
            odp.MethodName = "Add";
            odp.MethodParameters.Add("1");
            odp.MethodParameters.Add("1");

            Binding bindingToArg1 = new Binding("MethodParameters[0]")
            {
                Source = odp,
                BindsDirectlyToSource = true,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            Binding bindingToArg2 = new Binding("MethodParameters[1]")
            {
                Source = odp,
                BindsDirectlyToSource = true,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            Binding bindingToResult = new Binding(".") { Source=odp };

            this.textBoxArg1.SetBinding(TextBox.TextProperty, bindingToArg1);
            this.textBoxArg2.SetBinding(TextBox.TextProperty, bindingToArg2);
            this.textBoxResult.SetBinding(TextBox.TextProperty, bindingToResult);
        }

        private void RelativeSource ()
        {
            RelativeSource rs = new RelativeSource(RelativeSourceMode.FindAncestor);
            rs.AncestorLevel = 1;
            rs.AncestorType = typeof(Grid);
            Binding binding = new Binding("Name") { RelativeSource = rs };
            this.textBox2.SetBinding(TextBox.TextProperty, binding);
        }
    }
    class Student:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string name;
        public string Name
        { get { return name; }
            set
            {
                name = value;
                if(this.PropertyChanged!=null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
    }
    public class Student1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class Calculator
    {
        public string Add(string arg1,string arg2)
        {
            double x = 0;double y = 0;double z = 0;
            if(double.TryParse(arg1,out x)&&double.TryParse(arg2,out y))
            {
                z = x + y;
                return z.ToString();
            }
            return "Input Error!";
        }
    }
    public class RangeValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(double.TryParse(value.ToString(), out var d))
            {
                if(d>=0 &&d<=100)
                {
                    return new ValidationResult(true, null);
                }
            }
            return new ValidationResult(false, "Validation Failed.");
        }
    }
}
