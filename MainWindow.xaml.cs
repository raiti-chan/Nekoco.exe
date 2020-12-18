using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Nekoco {
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {

		/// <summary>
		/// スクリーンサイズ
		/// </summary>
		private Point screenSize;

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
		/// ウィンドウアニメーション用のストーリーボード
		/// </summary>
		private readonly Storyboard windowAnimStorybord = new Storyboard();

		private readonly DoubleAnimation windowAnimLeft = new DoubleAnimation();
		private readonly DoubleAnimation windowAnimTop = new DoubleAnimation();


		/// <summary>
		/// 初期化
		/// </summary>
		public MainWindow() {
			this.InitializeComponent();

			this.timer.Interval = new TimeSpan(0, 0, 10);
			this.timer.Tick += this.Tick;
			this.timer.Start();

			this.MouseLeftButtonDown += (sender, e) => { 
				this.DragMove();
				this.windowAnimStorybord.Stop();
			};

			this.MouseDoubleClick += this.DoubleClick;
			
			this.screenSize = Util.GetVirtualScreenSize();

			this.Left = random.Next((int)screenSize.X - (int)this.Width);
			this.Top = random.Next((int)screenSize.Y - (int)this.Height);

			#region Window Animation

			Storyboard.SetTarget(this.windowAnimLeft, this);
			Storyboard.SetTargetProperty(this.windowAnimLeft, new PropertyPath("Left"));
			this.windowAnimStorybord.Children.Add(this.windowAnimLeft);

			Storyboard.SetTarget(this.windowAnimTop, this);
			Storyboard.SetTargetProperty(this.windowAnimTop, new PropertyPath("Top"));
			this.windowAnimStorybord.Children.Add(this.windowAnimTop);

			this.windowAnimStorybord.Completed += (sender, e) => {
				this.windowAnimStorybord.Stop();
			};

			#endregion
		}

		/// <summary>
		/// 更新処理
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">e</param>
		private void Tick(object sender, EventArgs e) {
			if (this.random.Next(5) == 3) {
				// 次の画像へ
				this.NextPicture();
			}

			if (this.random.Next(10) == 5) {
				// 移動
				if (this.random.Next(3) == 1) {
					this.MoveToPointer();
				} else {
					this.RandomMove();
				}
			}

			if (this.random.Next(20) == 14) {
				// アクティブ化
				this.Activate();
			}

			if (this.random.Next(50) == 18) {
				// マウスカーソル移動
				Util.SetCursorPos(this.Left + this.Width / 2, this.Top + this.Height / 2);
			}

			if (this.random.Next(50) == 25) {
				// 増殖
				Process.Start("Nekoco.exe");
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
			if (this.random.Next(20) == 7) {
				this.Image.Source = new BitmapImage(this.otherUris[this.random.Next(this.otherUris.Length)]);
			} else {
				this.Image.Source = new BitmapImage(this.nekocoUris[this.random.Next(this.nekocoUris.Length)]);
			}
			this.Width = this.Image.Source.Width / 2;
			this.Height = this.Image.Source.Height / 2;
		}

		/// <summary>
		/// ランダムな位置に移動します。
		/// </summary>
		private void RandomMove() {
			this.windowAnimStorybord.Stop();
			this.windowAnimLeft.From = this.Left;
			this.windowAnimLeft.To = random.Next((int)(this.screenSize.X - this.Width));
			this.windowAnimLeft.Duration = TimeSpan.FromSeconds(random.Next(4) + 1);

			this.windowAnimTop.From = this.Top;
			this.windowAnimTop.To = random.Next((int)(this.screenSize.Y - this.Height));
			this.windowAnimTop.Duration = this.windowAnimLeft.Duration;
			this.windowAnimStorybord.Begin();
		}


		/// <summary>
		/// ランダムな位置に移動します。
		/// </summary>
		private void MoveToPointer() {
			Point p = Util.GetCursorPos();
			this.windowAnimStorybord.Stop();
			this.windowAnimLeft.From = this.Left;
			this.windowAnimLeft.To = p.X - (this.Width / 2);
			this.windowAnimLeft.Duration = TimeSpan.FromSeconds(random.Next(4) + 1);

			this.windowAnimTop.From = this.Top;
			this.windowAnimTop.To = p.Y - (this.Height / 2);
			this.windowAnimTop.Duration = this.windowAnimLeft.Duration;
			this.windowAnimStorybord.Begin();
		}

	}

	public static class Util {
		/// <summary>
		/// ディスプレイの論理サイズを求める
		/// </summary>
		/// <returns>論理サイズ</returns>
		public static Point GetVirtualScreenSize() {
			PresentationSource source = PresentationSource.FromVisual(Application.Current.MainWindow);
			double dpiWidthFactor = 1.0;
			double dpiHeightFactor = 1.0;
			if (source != null) {
				Matrix m = source.CompositionTarget.TransformToDevice;
				dpiWidthFactor = m.M11;
				dpiHeightFactor = m.M22;
			}
			return new Point() {
				X = SystemParameters.VirtualScreenWidth / dpiWidthFactor,
				Y = SystemParameters.VirtualScreenHeight / dpiHeightFactor,
			};
		}

		[DllImport("User32.dll")]
		private static extern bool GetCursorPos(out PointW32 lppoint);
		[StructLayout(LayoutKind.Sequential)]
		struct PointW32 {
			public int X { get; set; }
			public int Y { get; set; }
			public static implicit operator Point(PointW32 point) {
				return new Point(point.X, point.Y);
			}
		}

		public static Point GetCursorPos() {
			GetCursorPos(out PointW32 pt);
			PresentationSource source = PresentationSource.FromVisual(Application.Current.MainWindow);
			if (source != null) {
				Matrix m = source.CompositionTarget.TransformToDevice;
				pt.X /= (int)m.M11;
				pt.Y /= (int)m.M22;
			}
			return pt;
		}

		[DllImport("User32.dll")]
		private static extern bool SetCursorPos(int x, int y);

		public static void SetCursorPos(double x, double y) {
			PresentationSource source = PresentationSource.FromVisual(Application.Current.MainWindow);
			if (source != null) {
				Matrix m = source.CompositionTarget.TransformToDevice;
				x *= m.M11;
				y *= m.M22;
			}
			SetCursorPos((int)x, (int)y);
		}

	}
}
