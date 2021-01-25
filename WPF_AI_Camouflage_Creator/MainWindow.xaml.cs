using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF_AI_Camouflage_Creator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Map map;
		private ControllerGA ga;
		public MainWindow()
		{
			InitializeComponent();

			Icon = BitmapFrame.Create(new Uri(@"pack://application:,,,/Images/chameleon.ico"));

			ga = new ControllerGA(this);
		}
	}
}
