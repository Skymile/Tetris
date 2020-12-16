using System;
using System.Windows.Input;

namespace WpfTetris
{
	public class Command : ICommand
	{
		public Command(Action action) => this.action = action;

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter) => action();

		private readonly Action action;
	}
}
