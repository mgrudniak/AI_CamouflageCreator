using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_AI_Camouflage_Creator
{
	public class Map
	{
		public int GridSize { get; }
		public int TileHeight { get; }
		public int TileWidth { get; }
		public List<List<GridPoint>> Grid { get; set; }
		public List<GridPoint> SelectedPoints { get; set; }

		private MainWindow mw;

		public Map(MainWindow mw, int gridSize, int height, int width)
		{
			this.mw = mw;

			GridSize = gridSize;
			TileHeight = height / gridSize;
			TileWidth = width / gridSize;

			Grid = new List<List<GridPoint>>();
			SelectedPoints = new List<GridPoint>();
			
			this.mw.Height = height + 68;
			this.mw.Width = width + 15;
			this.mw.ResizeMode = ResizeMode.NoResize;
			this.mw.root.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Images/woods.png")));

			for (int i = 0; i < gridSize; i++)
			{
				List<GridPoint> subList = new List<GridPoint>();
				for (int j = 0; j < gridSize; j++)
				{
					GridPoint button = new GridPoint
					{
						X = TileWidth * j + TileWidth / 2,
						Y = TileHeight * i + TileHeight / 2,
						Height = TileHeight,
						Width = TileWidth,
						Background = Brushes.Transparent,
						BorderThickness = new Thickness(0, 0, 0, 0)
					};

					button.MouseEnter += button_MouseEnter;
					button.MouseLeave += button_MouseLeave;

					button.Click += button_Click;

					Canvas.SetTop(button, i * TileHeight);
					Canvas.SetLeft(button, j * TileWidth);

					this.mw.root.Children.Add(button);
					subList.Add(button);
				}
				Grid.Add(subList);
			}
		}

		void button_MouseEnter(object sender, RoutedEventArgs e)
		{
			GridPoint ctrl = (GridPoint)sender;
			ctrl.Opacity = 0.5;
			ctrl.BorderThickness = new Thickness(2, 2, 2, 2);
			ctrl.BorderBrush = Brushes.SteelBlue;
		}
		void button_MouseLeave(object sender, RoutedEventArgs e)
		{
			GridPoint ctrl = (GridPoint)sender;
			ctrl.Opacity = 1.0;
			ctrl.BorderThickness = new Thickness(0, 0, 0, 0);
		}
		void button_Click(object sender, RoutedEventArgs e)
		{
			GridPoint ctrl = (GridPoint)sender;
			if (ctrl.Background == Brushes.Transparent)
			{
				ctrl.Background = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
				ctrl.Selected = true;
				SelectedPoints.Add(ctrl);
			}
			else
			{
				ctrl.Background = Brushes.Transparent;
				ctrl.Selected = false;
				SelectedPoints.Remove(ctrl);
			}
		}
	}
}
