using System.Windows;

namespace Nekoco {
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
		}
	}
}
