using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WaitControl
{

    /// <MetaDataID>{50417024-dbe1-486b-9742-5943e274e7ea}</MetaDataID>
    public static class WPFHelper
    {
        private static bool? _isInDesignMode;
        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend
        /// or Visual Studio).
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
            _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    _isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
#endif
                }
                return _isInDesignMode.Value;
            }
        }

        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

    }

    /// <MetaDataID>{f564a3bf-ec4e-4983-867f-b559bf732d5e}</MetaDataID>
    public class ActionCommand : ICommand
    {
        private readonly Action _executeAction = null;
        private readonly Func<bool> _canExecuteFunc = null;

        public ActionCommand(Action executeAction, Func<bool> canExecFunc)
        {
            _executeAction = executeAction;
            _canExecuteFunc = canExecFunc;
        }

        public void Execute(object parameter)
        {
            _executeAction();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
