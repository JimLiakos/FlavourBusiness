using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OOAdvantech.Transactions;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{fb33dd12-0357-4e53-9198-9763146e273c}</MetaDataID>
    public class ScaleTypeViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

       public readonly MenuModel.IScaleType ScaleType;
        int NumOfLevelsUnderSelctionGroup;

        public ScaleTypeViewModel(MenuModel.IScaleType scaleType)
        {
            ScaleType = scaleType;
            if (scaleType.Levels[0].UncheckOption)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(scaleType));


                NumOfLevelsUnderSelctionGroup = (from preparasionOptionsGroup in storage.GetObjectCollection<MenuModel.IPreparationOptionsGroup>()
                                                 from preparationOption in preparasionOptionsGroup.GroupedOptions
                                                 from level in preparationOption.LevelType.Levels
                                                 where level.UncheckOption && preparationOption.LevelType == scaleType && preparasionOptionsGroup.SelectionType!=MenuModel.SelectionType.SimpleGroup
                                                 select level).Count();
            }

            AddScaleTypeLevelCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
              {
                  using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                  {
                      MenuModel.Level level = new MenuModel.Level(Properties.Resources.ScaleTypeLevelDefaultName);
                      OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ScaleType).CommitTransientObjectState(level);
                      ScaleType.AddLevel(level);
                      stateTransition.Consistent = true;
                  }
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Levels)));

              }, (object sender) => !(ScaleType is MenuModel.FixedScaleType));

            DeleteSelectedScaleTypeLevelCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
              {
                  ScaleType.RemoveLevel(SelectedLevel.Level);
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Levels)));

              }, (object sender) => SelectedLevel != null&&!(ScaleType is MenuModel.FixedScaleType)&& ScaleType.Levels.Count>2);

            RenameSelectedScaleTypeLevelCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedLevel.Edit = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }, (object sender) => SelectedLevel != null&&!(ScaleType is MenuModel.FixedScaleType));

            MoveUpSelectedScaleTypeLevelCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
              {
                  int pos = ScaleType.Levels.IndexOf(SelectedLevel.Level);
                  if (pos > 0)
                      ScaleType.MoveLevel(SelectedLevel.Level, pos - 1);

                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Levels)));

              }, (object sender) => CanBeSelectedLevelMoveUp);

            MoveDownSelectedScaleTypeLevelCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                int pos = ScaleType.Levels.IndexOf(SelectedLevel.Level);
                if (pos < ScaleType.Levels.Count-1)
                    ScaleType.MoveLevel(SelectedLevel.Level, pos + 1);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Levels)));

            }, (object sender) => CanBeSelectedLevelMoveDown);

            SetSelectedScaleTypeLevelUncheckCommand= new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedLevel.ToggleUncheckOtption();
            }, (object sender) => CanBeSelectedLevelUncheckOtption);
        }

        bool CanBeSelectedLevelUncheckOtption
        {
            get
            {
                if (SelectedLevel == null)
                    return false;
                if (this.ScaleType == null || ScaleType is MenuModel.FixedScaleType)
                    return false;
                if (NumOfLevelsUnderSelctionGroup > 0)
                    return false;
                return this.ScaleType.Levels.IndexOf(SelectedLevel.Level) == 0;
            }
        }

        bool CanBeSelectedLevelMoveDown
        {
            get
            {
                if (SelectedLevel == null)
                    return false;
                if (this.ScaleType == null || ScaleType is MenuModel.FixedScaleType)
                    return false;
                if (!SelectedLevel.Level.UncheckOption)
                    return true;
                else
                    return false;
                
            }
        }

        bool CanBeSelectedLevelMoveUp
        {
            get
            {
                if (SelectedLevel == null)
                    return false;
                if (this.ScaleType == null|| ScaleType is MenuModel.FixedScaleType)
                    return false;
                int pos = this.ScaleType.Levels.IndexOf(SelectedLevel.Level);
                if (pos == -1 || pos == 0 || (pos == 1 && this.ScaleType.Levels[0].UncheckOption))
                    return false;
                return true;
            }
        }


        /// <exclude>Excluded</exclude>
        LevelViewModel _SelectedLevel;
        public LevelViewModel SelectedLevel
        {
            get
            {
                return _SelectedLevel;
            }
            set
            {
                _SelectedLevel = value;
            }
        }


        public string Name
        {
            get
            {
                return ScaleType.Name;
            }
            set
            {
                ScaleType.Name = value;
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



        public string SetSelectedScaleTypeLevelUncheckTooltip
        {
            get
            {
                //!CanBeSelectedLevelUncheckOtption&&
                if (ScaleType is MenuModel.FixedScaleType)
                    return Properties.Resources.FixedScaleTypeEditMessage;
                if (NumOfLevelsUnderSelctionGroup > 0)
                    return Properties.Resources.PreparationOptionsGroupInvariantMessage;

                return Properties.Resources.ChangeUncheckOptionPropertyValuePrompt;
            }
        }



        public WPFUIElementObjectBind.RelayCommand AddScaleTypeLevelCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand DeleteSelectedScaleTypeLevelCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand RenameSelectedScaleTypeLevelCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand MoveUpSelectedScaleTypeLevelCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand MoveDownSelectedScaleTypeLevelCommand { get; protected set; }


        public WPFUIElementObjectBind.RelayCommand SetSelectedScaleTypeLevelUncheckCommand { get; protected set; }

        
        /// <exclude>Excluded</exclude>
        bool _Edit;
        /// <MetaDataID>{757665b9-5915-4dfe-b5d4-cf79ec303582}</MetaDataID>
        public bool Edit
        {
            get
            {
                return _Edit;
            }

            set
            {
                _Edit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }
        public List<LevelViewModel> Levels
        {
            get
            {
               return (from level in ScaleType.Levels select LevelsDitionary.GetViewModelFor(level, level,this)).ToList();
                //return ScaleType.Levels.ToList();
            }
        }


        ViewModelWrappers<MenuModel.ILevel, LevelViewModel> LevelsDitionary = new ViewModelWrappers<MenuModel.ILevel, LevelViewModel>();


    }
}
