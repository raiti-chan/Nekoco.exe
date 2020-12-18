using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Nekoco {
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {

		/// <summary>
		/// ランダムインスタンス
		/// </summary>
		private readonly Random random = new Random();

		/// <summary>
		/// 画像URIリスト
		/// </summary>
		private readonly Uri[] nekocoUris = {
			new Uri("Resources/Nekoco_000.png", UriKind.Relative),
			new Uri("Resources/Nekoco_001.png", UriKind.Relative),
		};

		/// <summary>
		/// レア画像URIリスト
		/// </summary>
		private readonly Uri[] otherUris = {
			new Uri("Resources/Yuma_000.png", UriKind.Relative),
		};

		/// <summary>
		/// 更新タイマー
		/// </summary>
		private readonly DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);

		/// <summary>
		/// 初期化
		/// </summary>
		public MainWindow() {
			this.InitializeComponent();

			this.timer.Interval = new TimeSpan(0, 0, 10);
			this.timer.Tick += this.Tick;
			this.timer.Start();

			this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
			this.MouseDoubleClick += this.DoubleClick;
		}

		/// <summary>
		/// 更新処理
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">e</param>
		private void Tick(object sender, EventArgs e) {
			if (this.random.NextDouble() < 0.40) {
				this.NextPicture();
			}
		}

		/// <summary>
		/// ウィンドウダブルクリック処理
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">e</param>
		private void DoubleClick(object sender, MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left) {
				this.NextPicture();
			}
		}

		/// <summary>
		/// Exitクリック
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">e</param>
		private void ExitClick(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}


		/// <summary>
		/// 次の画像へ
		/// </summary>
		private void NextPicture() {
			if (this.random.NextDouble() < 0.10) {
				this.Image.Source = new BitmapImage(this.otherUris[this.random.Next(this.otherUris.Length)]);
			} else {
				this.Image.Source = new BitmapImage(this.nekocoUris[this.random.Next(this.nekocoUris.Length)]);
			}
			this.Width = this.Image.Source.Width / 2;
			this.Height = this.Image.Source.Height / 2;
		}

	}
}
