using System;
using System.Linq;
using System.Windows;

namespace Bliss.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Model = new MainWindowViewModel();
		}

		public MainWindowViewModel Model
		{
			get
			{
				return DataContext as MainWindowViewModel;
			}
			set
			{
				DataContext = value;
			}
		}
	}
}