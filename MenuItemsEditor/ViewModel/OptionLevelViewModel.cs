using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{ab31f748-08aa-4374-96f8-a4ef9d24202f}</MetaDataID>
    public class OptionLevelViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public readonly MenuModel.IPreparationScaledOption PreparationOption;
        public readonly MenuModel.ILevel Level;
        PreparationScaledOptionViewModel PreparationScaledOptionViewModel;


        public OptionLevelViewModel(PreparationScaledOptionViewModel preparationScaledOptionViewModel, MenuModel.ILevel level)
        {
            PreparationOption = preparationScaledOptionViewModel.PreparationOption as MenuModel.IPreparationScaledOption;
            PreparationScaledOptionViewModel = preparationScaledOptionViewModel;
            Level = level;
        }

        public string Name
        {
            get
            {
                return Level.Name;
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

        public event PropertyChangedEventHandler PropertyChanged;


       MenuModel.ILevel PreparationOptionInitial
        {
            get
            {
                if(PreparationScaledOptionViewModel.MenuItemViewModel!=null)
                {
                    MenuModel.IMenuItem menuItem = null;
                    if (PreparationScaledOptionViewModel.MenuItemViewModel != null)
                        menuItem = PreparationScaledOptionViewModel.MenuItemViewModel.MenuItem;
                    if (menuItem != null)
                        return PreparationOption.GetInitialFor(menuItem);
                }
                return PreparationOption.Initial;
            }
        }
        public ImageSource Image
        {
            get
            {
                if(PreparationOptionInitial == Level)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/checkRed16.png"));
                else
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
            }
        }

        public ImageSource UncheckOptionImage
        {
            get
            {
                if (Level.UncheckOption)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/uncheck-square.png"));
                else
                {
                    if (Level.DeclaringType.ZeroLevelScaleType)
                    {
                        if (Level.DeclaringType.Levels.Count > 0 && Level.DeclaringType.Levels[0] == Level)
                            return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/uncheck-square.png"));
                        else
                            return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/check-square.png"));
                    }
                    else
                    {
                        if (Level.DeclaringType.Levels.Count > 0 && Level.DeclaringType.Levels[0].UncheckOption)
                            return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/check-square.png"));
                        else
                            return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    }
                }


                //if (Level.UncheckOption)
                //    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/uncheck-square.png"));
                //else
                //{
                //    if (PreparationOption.LevelType != null&& PreparationOption.LevelType.Levels.Count>0&& PreparationOption.LevelType.Levels[0].UncheckOption)
                //        return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/check-square.png"));

                //    return null;// new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                //}
            }
        }

        internal void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UncheckOptionImage)));
        }


        public decimal Price
        {
            get
            {
                if (!(PreparationOption is MenuModel.IPricedSubject))
                    return 0;


                if (PreparationOption is MenuModel.ItemSelectorOption && Level.UncheckOption)
                    return 0;

                if (PreparationOption is MenuModel.ItemSelectorOption && !Level.UncheckOption)
                    return PreparationScaledOptionViewModel.Price;


                if (PreparationOption.LevelType != null)
                {
                    int dif = PreparationOption.LevelType.Levels.IndexOf(Level) - PreparationOption.LevelType.Levels.IndexOf(PreparationOptionInitial);

                    return ((decimal)(Level.PriceFactor - PreparationOptionInitial.PriceFactor))* PreparationScaledOptionViewModel.Price;
                    //return dif* PreparationScaledOptionViewModel.Price;
                }
                return 0;
            }

        }
    }
}
