using System;
using System.Drawing;
using prowaretech.com;

namespace WPF_AI_Camouflage_Creator
{
	class Individual
	{
		public Bitmap Camouflage { get; }
		public double Evaluation { get; set; }
		public double Fitness { get; set; } // suma fitnessow R, G, B
		public int IntegerFitness { get; set; }
		public double FractionalFitness { get; set; }

		private Color imagesEvaluation;
		private Hyperparameters parameters;

		public void Mutate()
		{
			int seed = Guid.NewGuid().GetHashCode();
			RandomMersenne rm = new RandomMersenne((uint)seed);

			int x = rm.IRandom(0, Camouflage.Width - 1);
			int y = rm.IRandom(0, Camouflage.Height - 1);

			int min = Math.Max(0, imagesEvaluation.R - parameters.Mutmax);
			int max = Math.Min(255, imagesEvaluation.R + parameters.Mutmax);
			int r = rm.IRandom(min, max);

			min = Math.Max(0, imagesEvaluation.G - parameters.Mutmax);
			max = Math.Min(255, imagesEvaluation.G + parameters.Mutmax);
			int g = rm.IRandom(min, max);

			min = Math.Max(0, imagesEvaluation.B - parameters.Mutmax);
			max = Math.Min(255, imagesEvaluation.B + parameters.Mutmax);
			int b = rm.IRandom(min, max);

			Camouflage.SetPixel(x, y, Color.FromArgb(r, g, b));
		}

		public Individual(int width, int height, Bitmap image, Color imagesEvaluation, Hyperparameters parameters)
		{
			Fitness = 0;
			IntegerFitness = 0;
			FractionalFitness = 0;

			this.imagesEvaluation = imagesEvaluation;
			this.parameters = parameters;

			Camouflage = new Bitmap(width, height);

			int seed = Guid.NewGuid().GetHashCode();
			RandomMersenne rm = new RandomMersenne((uint)seed);

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					int r = rm.IRandom(0, 255);
					int g = rm.IRandom(0, 255);
					int b = rm.IRandom(0, 255);

					Camouflage.SetPixel(x, y, Color.FromArgb(r, g, b));
				}
			}
		}
	}
}