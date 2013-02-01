using System;
using System.Windows.Input;

namespace Bliss.Wpf
{
	public class DelegateCommand : ICommand
	{
		private readonly Action execute;
		private readonly Func<bool> canExecute;

		public event EventHandler CanExecuteChanged;

		public DelegateCommand(Action execute, Func<bool> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			if (canExecute == null)
			{
				throw new ArgumentNullException("canExecute");
			}

			this.execute = execute;
			this.canExecute = canExecute;
		}

		public void RaiseCanExecuteChanged()
		{
			EventHandler canExecuteChanged = CanExecuteChanged;
			if (canExecuteChanged != null)
			{
				canExecuteChanged(this, EventArgs.Empty);
			}
		}

		public void Execute(object parameter)
		{
			this.execute();
		}

		public bool CanExecute(object parameter)
		{
			return this.canExecute();
		}

		protected virtual void RaiseCanExecutedChanged()
		{
			EventHandler onCanExecuteChanged = CanExecuteChanged;
			if (onCanExecuteChanged != null)
			{
				onCanExecuteChanged(this, EventArgs.Empty);
			}
		}
	}
}