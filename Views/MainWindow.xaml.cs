using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tetris
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = vm = new MainWindowVM(MainGrid);
			//((MainWindowVM)this.DataContext).Reset();
			//(this.DataContext as MainWindowVM)?.Reset();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Left:
					vm.RotateLeft();
					vm.Refresh();
					break;
				case Key.Right:
					vm.RotateRight();
					vm.Refresh();
					break;
				case Key.R:
					vm.Reset();
					break;
				case Key.P:
					vm.Pause.Execute(null);
					break;
				case Key.S:
					vm.Cycle();
					break;
				case Key.D:
					vm.MoveRight();
					vm.Refresh();
					break;
				case Key.A:
					vm.MoveLeft();
					vm.Refresh();
					break;
				default:
					break;
			}
		}

		private readonly MainWindowVM vm;
	}
}
