using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Bliss.Wpf.Adapters;
using Microsoft.Win32;

namespace Bliss.Wpf
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private readonly ObservableCollection<ImageAdapter> images;
		private readonly ObservableCollection<HistogramViewModel> histograms;
		private ICommand openPictureCommand;
		private ICommand showHistogramsCommand;
		private ICommand convertToGrayscaleCommand;
		private ICommand removeImageCommand;

		private ImageAdapter currentImage;

		public MainWindowViewModel()
		{
			this.images = new ObservableCollection<ImageAdapter>();
			this.histograms = new ObservableCollection<HistogramViewModel>();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<ImageAdapter> Images
		{
			get
			{
				return this.images;
			}
		}

		public ObservableCollection<HistogramViewModel> Histograms
		{
			get
			{
				return this.histograms;
			}
		}

		public ImageAdapter CurrentImage
		{
			get
			{
				return this.currentImage;
			}
			set
			{
				if (this.currentImage != value)
				{
					this.currentImage = value;
					OnPropertyChanged("CurrentImage");
				}
			}
		}
  
		public ICommand OpenPictureCommand
		{
			get
			{
				if (this.openPictureCommand == null)
				{
					this.openPictureCommand = new DelegateCommand(OpenPicture, () => true);
				}
				return this.openPictureCommand;
			}
		}

		public ICommand ShowHistogramsCommand
		{
			get
			{
				if (this.showHistogramsCommand == null)
				{
					this.showHistogramsCommand = new DelegateCommand(ShowHistograms, () => true);
				}
				return this.showHistogramsCommand;
			}
		}

		public ICommand ConvertToGrayscaleCommand
		{
			get
			{
				if (this.convertToGrayscaleCommand == null)
				{
					this.convertToGrayscaleCommand = new DelegateCommand(ConvertToGrayscaleImage, () => true);
				}
				return this.convertToGrayscaleCommand;
			}
		}

		public ICommand RemoveImageCommand
		{
			get
			{
				if (this.removeImageCommand == null)
				{
					this.removeImageCommand = new DelegateCommand(() => Images.Remove(CurrentImage), () => true);
				}
				return this.removeImageCommand;
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler onPropertyChanged = PropertyChanged;
			PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
			if (onPropertyChanged != null)
			{
				onPropertyChanged(this, e);
			}
		}

		// TODO: refactor this, showing a FileChooserDialog from a view model is plain wrong!!!
		private void OpenPicture()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			Environment.CurrentDirectory = @"C:\Users\svetlozar\Desktop\patriots";
			dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
			bool? result = dialog.ShowDialog();
			if (result.HasValue && result.Value)
			{
				string path = dialog.FileName;
				images.Add(ImageAdapterFactory.Create(path));
			}
		}

		private void ShowHistograms()
		{
			this.histograms.Clear();
			foreach (var histogram in CurrentImage.Image.GetHistograms())
			{
				this.histograms.Add(new HistogramViewModel(histogram));
			}
		}

		private void ConvertToGrayscaleImage()
		{
			GrayscaleImageAdapter grayscaleImage = ImageAdapterFactory.CreateGrayscaleImageAdapter(CurrentImage);
			if (grayscaleImage != null)
			{
				CurrentImage = grayscaleImage;
			}
		}
	}
}