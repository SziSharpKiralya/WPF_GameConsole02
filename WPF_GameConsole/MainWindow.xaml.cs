using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_GameConsole
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Random random = new Random();
		private List<Pipe> pipes = new();
		private string current = "";
		private bool state = false;
		

		public MainWindow()
		{
			InitializeComponent();
			LoadScreen_Menu();
		}
	}
}