using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{9c8d985f-e566-417e-95f0-c14b1b1b389e}</MetaDataID>
    public class LevelViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

       readonly public MenuModel.ILevel Level;
        public readonly ScaleTypeViewModel ScaleTypeViewModel;

        public LevelViewModel(MenuModel.ILevel level, ScaleTypeViewModel scaleTypeViewModel)
        {
            Level = level;
            ScaleTypeViewModel = scaleTypeViewModel;
        }


        public string Name
        {
            get
            {
                return Level.Name;
            }
            set
            {
                Level.Name = value;
            }
        }

        public double PriceFactor
        {
            get
            {
                return Level.PriceFactor;
            }
            set
            {
                Level.PriceFactor = value;
            }
        }
        /// <exclude>Excluded</exclude>
        ITranslator _Translator;
        public ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new Translator();
                return _Translator;
            }
        }

        public bool UnTranslated
        {
            get
            {
                string name = Name;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return name != Name;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Edit;
        
        public bool Edit
        {
            get
            {
                return _Edit;
            }

            set
            {
                _Edit = value;
                if (Level.DeclaringType is MenuModel.FixedScaleType)
                    _Edit = false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }

        internal void ToggleUncheckOtption()
        {
            if (Level.DeclaringType.Levels.Count > 2)
                Level.DeclaringType.ZeroLevelScaleType = !Level.DeclaringType.ZeroLevelScaleType;
            else
                Level.UncheckOption = !Level.UncheckOption;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));

            foreach (var levelViewModel in ScaleTypeViewModel.Levels)
                levelViewModel.NewImage();

        }

        private void NewImage()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
        }

        public ImageSource Image
        {
            get
            {
                if (Level.UncheckOption)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/uncheck-square.png"));
                else
                {
                    if (ScaleTypeViewModel.ScaleType.ZeroLevelScaleType)
                    {
                        if (ScaleTypeViewModel.ScaleType.Levels.Count > 0 && ScaleTypeViewModel.ScaleType.Levels[0]==Level)
                            return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/uncheck-square.png"));
                        else
                            return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/check-square.png"));
                    }
                    else
                    {
                        if (ScaleTypeViewModel.ScaleType.Levels.Count > 0 && ScaleTypeViewModel.ScaleType.Levels[0].UncheckOption)
                            return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/check-square.png"));
                        else
                            return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    }
                }
             
            }
        }
    }
}
