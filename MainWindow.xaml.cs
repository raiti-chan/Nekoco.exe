using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Nekoco {
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {

		private readonly Random random = new Random();

		private readonly Uri[] nekocoUris = {
			new Uri("Resources/Nekoco_000.png", UriKind.Relative),
			new Uri("Resources/Nekoco_001.png", UriKind.Relative),
		};

		private readonly Uri[] otherUris = {
			new Uri("Resources/Yuma_000.png", UriKind.Relative),
		};

		private readonly DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);

		public MainWindow() {
			this.InitializeComponent();

			this.timer.Interval = new TimeSpan(0, 0, 10);
			this.timer.Tick += this.Tick;
			this.timer.Start();

			this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
			this.MouseDoubleClick += (sender, e) => {
				this.NextPicture();
			};
		}

		private void NextPicture() {
			if (this.random.NextDouble() < 0.10) {
				this.Image.Source = new BitmapImage(this.otherUris[this.random.Next(this.otherUris.Length)]);
			} else {
				this.Image.Source = new BitmapImage(this.nekocoUris[this.random.Next(this.nekocoUris.Length)]);
			}
			this.Width = this.Image.Source.Width / 2;
			this.Height = this.Image.Source.Height / 2;
		}

		private void Tick(object sender, EventArgs e) {
			if (this.random.NextDouble() < 0.40) {
				this.NextPicture();
			}
		}
	}
}
