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

            stu = new Student();

            Binding binding = new Binding();
            binding.Source = stu;
            binding.Path = new PropertyPath("Name");

            BindingOperations.SetBinding(this.textBoxName, TextBox.TextProperty, binding);

            

            this.listBoxStudents.ItemsSource = stuList1;
            //this.listBoxStudents.DisplayMemberPath ="Name";

            Binding binding1 = new Binding("SelectedItem.Id") { Source = this.listBoxStudents };
            this.textBoxId.SetBinding(TextBox.TextProperty, binding1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            stu.Name += "name";
        }
        
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            this.listViewStudent.ItemsSource = stuList1;
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
}
