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
		private string current = "";
        private string mode = "Normal";
        private bool state = false;

        private int score = 0;
        private double gravity = 0.5;
        private double jumpForce = -8;

        public MainWindow()
		{
			InitializeComponent();
			LoadScreen_Menu();
		}
	}
}