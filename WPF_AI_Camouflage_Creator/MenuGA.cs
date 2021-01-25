using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace WPF_AI_Camouflage_Creator
{
	class MenuGA
	{
		private MainWindow mw;
		private MenuItem options;
		public Hyperparameters Parameters { get; set; }

		public void AddElement(string title, RoutedEventHandler click, int postition)
		{
			MenuItem element = new MenuItem
			{
				Header = title,
				Height = 30
			};
			element.Click += click;

			mw.menu.Items.Insert(postition, element);
		}

		public void AddElementWithSlider(string title, double defaultValue, double maxValue, double scale)
		{
			MenuItem element = new MenuItem
			{
				Header = title
			};
			DockPanel panel = new DockPanel();

			Slider slider = new Slider
			{
				Width = 100,
				Maximum = maxValue,
				TickPlacement = TickPlacement.BottomRight,
				TickFrequency = scale,
				IsSnapToTickEnabled = true
			};

			Binding bindingSlider = new Binding(title)
			{
				Source = Parameters,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			slider.SetBinding(Slider.ValueProperty, bindingSlider);

			panel.Children.Add(slider);

			TextBox textBox = new TextBox
			{
				Width = 35,
				Text = defaultValue.ToString(),
				TextAlignment = TextAlignment.Center
			};

			Binding bindingTextBox = new Binding(title)
			{
				Source = Parameters,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			};
			textBox.SetBinding(TextBox.TextProperty, bindingTextBox);

			panel.Children.Add(textBox);

			element.Items.Add(panel);
			options.Items.Add(element);
		}

		public MenuGA(MainWindow mw, Hyperparameters parameters)
		{
			this.mw = mw;
			this.Parameters = parameters;

			options = new MenuItem
			{
				Header = "Options",
				Height = 30
			};

			this.mw.menu.Items.Add(options);
		}
	}
}
