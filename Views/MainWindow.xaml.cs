using System.Windows;
using FF.WPF.ViewModels;

namespace FF.WPF.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        
    }
}