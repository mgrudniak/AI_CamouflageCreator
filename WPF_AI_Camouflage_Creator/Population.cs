using prowaretech.com;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace WPF_AI_Camouflage_Creator
{
	class Populacja
	{
		private List<Individual> individuals;
		private List<Individual> transitionalIndividuals;
		private Bitmap image;
		private List<Bitmap> evaluationImages; // list of images to evaluate individuals
		private Color imagesEvaluation;
		private Hyperparameters parameters;

		public void GeneratePopulation()
		{
			for (int i = 0; i < parameters.Individuals; i++)
			{
				Individual individual = new Individual(evaluationImages[0].Width, evaluationImages[0].Height, image, imagesEvaluation, parameters);
				individuals.Add(individual);
			}
		}

		public void Reproduct() // asexually
		{
			for (int i = 0; i < individuals.Count; i++)
			{
				for (int j = 0; j < individuals[i].IntegerFitness; j++)
				{
					if (transitionalIndividuals.Count < parameters.Individuals)
						transitionalIndividuals.Add(individuals[i]);
					else break;
				}
			}

			individuals.Sort((x, y) => y.FractionalFitness.CompareTo(x.FractionalFitness));

			int freeSpace = parameters.Individuals - transitionalIndividuals.Count;
			for (int i = 0; i < freeSpace; i++)
				transitionalIndividuals.Add(individuals[i]);

			Mutate();

			int middle = parameters.Individuals / 2;
			for (int i = 0; i < middle; i++)
				individuals[i] = transitionalIndividuals[i];
			for (int i = middle; i < parameters.Individuals; i++)
				individuals[i] = transitionalIndividuals[i - middle];

			Update();

			transitionalIndividuals.RemoveRange(0, transitionalIndividuals.Count);
		}

		public void Mutate()
		{
			int camouflageWidth = evaluationImages[0].Width;
			int camouflageHeight = evaluationImages[0].Height;
			int numberOfMutations = (int)Math.Round(parameters.MutationProbability * parameters.Individuals * camouflageWidth * camouflageHeight * 10);

			int seed = Guid.NewGuid().GetHashCode();
			RandomMersenne rm = new RandomMersenne((uint)seed);
			for (int i = 0; i < numberOfMutations; i++)
			{
				int indexOfIndividual = rm.IRandom(0, parameters.Individuals - 1);

				transitionalIndividuals[indexOfIndividual].Mutate();
			}
		}

		private Color EvaluateImages()
		{
			int redSum = 0, greenSum = 0, blueSum = 0;
			int redMean = 0, greenMean = 0, blueMean = 0;

			for (int i = 0; i < evaluationImages.Count; i++)
			{
				for (int x = 0; x < evaluationImages[i].Width; x++)
				{
					for (int y = 0; y < evaluationImages[i].Height; y++)
					{
						Color color = evaluationImages[i].GetPixel(x, y);

						redSum += color.R;
						greenSum += color.G;
						blueSum += color.B;
					}
				}
				redMean += redSum / (evaluationImages[i].Width * evaluationImages[i].Height);
				greenMean += greenSum / (evaluationImages[i].Width * evaluationImages[i].Height);
				blueMean += blueSum / (evaluationImages[i].Width * evaluationImages[i].Height);

				redSum = 0;
				greenSum = 0;
				blueSum = 0;
			}

			redMean /= evaluationImages.Count;
			greenMean /= evaluationImages.Count;
			blueMean /= evaluationImages.Count;

			return Color.FromArgb(redMean, greenMean, blueMean);
		}

		public double CalculateDifferece(Bitmap bmp)
		{
			int width = evaluationImages[0].Width;
			int height = evaluationImages[0].Height;

			double difference = 0;

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Color pixel = bmp.GetPixel(x, y);

					int redDifference = Math.Abs(pixel.R - imagesEvaluation.R);
					int greenDifference = Math.Abs(pixel.G - imagesEvaluation.G);
					int blueDifference = Math.Abs(pixel.B - imagesEvaluation.B);

					int differencesSum = redDifference + greenDifference + blueDifference;
					difference += differencesSum;
				}
			}
			difference /= width * height;

			return difference;
		}

		private void CalculateFitness()
		{
			double evaluationSum = 0;

			for (int i = 0; i < individuals.Count; i++)
			{
				double evaluation = 1 / CalculateDifferece(individuals[i].Camouflage);
				individuals[i].Evaluation = evaluation;

				evaluationSum += evaluation;
			}

			for (int i = 0; i < individuals.Count; i++)
			{
				double fitness = (individuals[i].Evaluation / evaluationSum) * parameters.Individuals;

				individuals[i].IntegerFitness = (int)Math.Floor(fitness);
				individuals[i].FractionalFitness = fitness - individuals[i].IntegerFitness;
				individuals[i].Fitness = fitness;
			}
		}
		private void Sort()
		{
			individuals.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));
		}

		public void Update()
		{
			CalculateFitness();
			Sort();
		}

		public Individual Best()
		{
			return individuals[0];
		}

		public Populacja(List<Bitmap> evaluationImages, Hyperparameters parameters, Bitmap image)
		{
			individuals = new List<Individual>();
			transitionalIndividuals = new List<Individual>();

			this.image = image;
			this.evaluationImages = evaluationImages;
			this.parameters = parameters;

			imagesEvaluation = EvaluateImages();

			GeneratePopulation();
		}
	}
}
