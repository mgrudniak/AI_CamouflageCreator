using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Drawing.Color;

namespace WPF_AI_Camouflage_Creator
{
	class GA
	{
		private Map map;
		private Bitmap image;
		private Hyperparameters parameters;

		public void DeselectPoints()
		{
			map.SelectedPoints.Clear();

			for (int y = 0; y < map.GridSize; y++)
			{
				for (int x = 0; x < map.GridSize; x++)
				{
					map.Grid[x][y].Selected = false;
					map.Grid[x][y].Background = Brushes.Transparent;
				}
			}
		}

		public Bitmap MakeCamouflage()
		{
			if (map.SelectedPoints.Count > 0)
			{
				List<Bitmap> images = CutEvaluationImages();

				Populacja individuals = new Populacja(images, parameters, image);

				individuals.Update();
				for (int i = 0; i < parameters.Generations - 1; i++)
				{
					individuals.Reproduct();

					individuals.Update();
				}

				PasteImages(individuals.Best());

				DeselectPoints();
			}
			else MessageBox.Show("Select at least one point!");

			return image;
		}

		public List<Bitmap> CutEvaluationImages()
		{
			List<Bitmap> images = new List<Bitmap>();

			for (int i = 0; i < map.SelectedPoints.Count; i++)
			{
				int x = (map.SelectedPoints[i].X) - (map.TileWidth / 2);
				int y = (map.SelectedPoints[i].Y) - (map.TileHeight / 2);
				int width = map.TileWidth;
				int height = map.TileHeight;

				Rectangle rectangle = new Rectangle(x, y, width, height);

				Bitmap bmp = image.Clone(rectangle, image.PixelFormat);

				images.Add(bmp);
			}

			return images;
		}
		public void PasteImages(Individual best)
		{
			for (int i = 0; i < map.SelectedPoints.Count; i++)
			{
				int x = (map.SelectedPoints[i].X) - (map.TileWidth / 2);
				int y = (map.SelectedPoints[i].Y) - (map.TileHeight / 2);
				int width = map.TileWidth;
				int height = map.TileHeight;

				for (int j = 0; j < height; j++)
				{
					for (int k = 0; k < width; k++)
					{
						Color color = best.Camouflage.GetPixel(k, j);
						image.SetPixel(k + x, j + y, color);
					}
				}
			}
		}

		public GA(Map map, Bitmap image, Hyperparameters parameters)
		{
			this.map = map;
			this.image = image;
			this.parameters = parameters;
		}
	}
}
