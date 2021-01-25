namespace WPF_AI_Camouflage_Creator
{
	class Hyperparameters
	{
		public int Individuals { get; set; }
		public int Generations { get; set; }
		public double MutationProbability { get; set; }
		public int Mutmax { get; set; }

		public Hyperparameters()
		{
			Individuals = 50;
			Generations = 25;
			MutationProbability = 0.01;
			Mutmax = 30;
		}
		public Hyperparameters(int individuals, int generations, double mutationProbability, int mutmax)
		{
			Individuals = individuals;
			Generations = generations;
			MutationProbability = mutationProbability;
			Mutmax = mutmax;
		}
	}
}
