using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Bliss.Core;
using Bliss.Core.Grayscale;
using Bliss.Core.Hsl;
using Bliss.Core.Rgb;
using Bliss.Wpf.Adapters;
using Microsoft.Win32;

namespace Bliss.Wpf
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private readonly ObservableCollection<ImageAdapter> images;
		private readonly ObservableCollection<HistogramViewModel> histograms;
		private ICommand openImageCommand;
		private DelegateCommand convertToGrayscaleCommand;
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
					ShowHistograms();
					ConvertToGrayscaleCommand.RaiseCanExecuteChanged();
				}
			}
		}
  
		public ICommand OpenImageCommand
		{
			get
			{
				if (this.openImageCommand == null)
				{
					this.openImageCommand = new DelegateCommand(OpenImage, () => true);
				}
				return this.openImageCommand;
			}
		}

		public DelegateCommand ConvertToGrayscaleCommand
		{
			get
			{
				if (this.convertToGrayscaleCommand == null)
				{
					this.convertToGrayscaleCommand = new DelegateCommand(ConvertToGrayscaleImage, CanConvertToGrayscale);
				}
				return this.convertToGrayscaleCommand;
			}
		}
  
		private bool CanConvertToGrayscale()
		{
			return !(CurrentImage.Image is GrayscaleImage);
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
		private void OpenImage()
		{
			OpenFileDialog dialog = new OpenFileDialog();
			
			Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
			bool? result = dialog.ShowDialog();
			if (result.HasValue && result.Value)
			{
				string path = dialog.FileName;
				Images.Add(ImageAdapterFactory.Create(path));
				if (Images.Count == 1)
				{
					CurrentImage = Images.First();
				}
				OnPropertyChanged("Images");
			}
		}

		private void ShowHistograms()
		{
			this.histograms.Clear();
			foreach (var histogram in CurrentImage.Image.GetHistograms())
			{
				this.histograms.Add(new HistogramViewModel(this, histogram));
			}
		}

		private void ConvertToGrayscaleImage()
		{
			ImageAdapter grayscaleImageAdapter = null;
			if (currentImage.Image is RgbImage)
			{
				grayscaleImageAdapter = ImageAdapterFactory.CreateAdapter(((RgbImage)currentImage.Image).ToGrayscaleImage());
			}
			else if (currentImage.Image is HslImage)
			{
				RgbImage rgbImage = ((HslImage)currentImage.Image).ToRgbImage();
				grayscaleImageAdapter = ImageAdapterFactory.CreateAdapter(rgbImage.ToGrayscaleImage());
			}
			if (grayscaleImageAdapter != null)
			{
				CurrentImage = grayscaleImageAdapter;
			}
		}
	}
}