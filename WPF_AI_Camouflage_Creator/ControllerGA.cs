using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;

namespace WPF_AI_Camouflage_Creator
{
	public class ControllerGA
	{
		private Map map;
		private MainWindow mw;
		private Bitmap originalImage;
		private Bitmap changedImage;
		private Hyperparameters parameters;

		public ControllerGA(MainWindow mw)
		{
			originalImage = Photo.ToBitmap(new BitmapImage(new Uri(@"pack://application:,,,/Images/woods.png")));
			changedImage = new Bitmap(originalImage);
			parameters = new Hyperparameters(50, 25, 0.01, 30);
			this.mw = mw;

			if (mw.map == null)
			{
				map = new Map(mw, 20, 800, 800);
				mw.map = map;
			}
			else map = mw.map;

			MenuGA menu = new MenuGA(this.mw, parameters);
			menu.AddElement("Start", start_Click, 0);
			menu.AddElement("Clear", clear_Click, 1);
			menu.AddElementWithSlider("Individuals", parameters.Individuals, 100, 10);
			menu.AddElementWithSlider("Generations", parameters.Generations, 250, 25);
			menu.AddElementWithSlider("MutationProbability", parameters.MutationProbability, 0.05, 0.002);
		}
		public void start_Click(object sender, RoutedEventArgs e)
		{
			GA ga = new GA(map, changedImage, parameters);

			changedImage = ga.MakeCamouflage();

			mw.root.Background = new ImageBrush(changedImage.ToBitmapImage());
		}
		public void clear_Click(object sender, RoutedEventArgs e)
		{
			for (int y = 0; y < map.Grid.Count; y++)
			{
				for (int x = 0; x < map.Grid.Count; x++)
				{
					map.Grid[x][y].Selected = false;
					map.Grid[x][y].Background = Brushes.Transparent;
					map.SelectedPoints.Clear();
				}
			}
			changedImage = originalImage;
			mw.root.Background = new ImageBrush(originalImage.ToBitmapImage());
		}


	}
}
