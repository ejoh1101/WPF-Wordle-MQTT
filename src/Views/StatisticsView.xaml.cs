namespace WPFordle.Views;
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

/// <summary>
/// Interaction logic for StatisticsView.xaml
/// </summary>
public partial class StatisticsView : UserControl
{

    public static StatisticsView Current;

    public StatisticsView()
    {
        InitializeComponent();
        Current = this;
    }

    public void SetStatusText(string text)
    {
        _ = Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, () => this.status.Text = text);
    }
}
