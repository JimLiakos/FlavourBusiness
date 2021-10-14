﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
//using OOAdvantech.Json;
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech.Transactions;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{0ebde6ac-f023-4655-aabb-1aee55450ef7}</MetaDataID>
    public class MenuItemTypeViewModel : OOAdvantech.UserInterface.Runtime.PresentationObject<MenuModel.IMenuItemType>, INotifyPropertyChanged, IPreparationOptionsListView
    {

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


        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler IsCheckedChanged;

        /// <MetaDataID>{b1d6ef7c-3b59-413e-bf55-9e03ab32122d}</MetaDataID>
        bool _IsChecked;
        /// <MetaDataID>{49244ec8-58f5-467e-99c0-45b50b9b805e}</MetaDataID>
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    IsCheckedChanged?.Invoke(this, EventArgs.Empty);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
                }
            }
        }

        /// <MetaDataID>{f5a879b1-8ef7-4ea1-b6ea-3c787abdad5e}</MetaDataID>
        bool _IsSelected;
        /// <MetaDataID>{526c53aa-d3c8-4e3b-8723-78481cd9659a}</MetaDataID>
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
            }
        }

        /// <MetaDataID>{3d856f1c-5e79-434a-9eed-95ada9b7290e}</MetaDataID>
        internal MenuItemViewModel HostingMenuItemViewModel;

        /// <MetaDataID>{c5c7c7e0-808a-4eb7-a276-d0ba6a6d668f}</MetaDataID>
        public MenuItemTypeViewModel(MenuModel.IMenuItemType menuItemType, MenuItemViewModel menuItem, bool isEditable) : this(menuItemType, isEditable)
        {
            HostingMenuItemViewModel = menuItem;
        }

        /// <MetaDataID>{a62d36f1-1ad9-4f41-96b2-b85293d30bf3}</MetaDataID>
        public MenuItemTypeViewModel(MenuModel.IMenuItemType menuItemType, bool isEditable) : base(menuItemType)
        {


            _IsEditable = isEditable;

            AddOptionsGroupCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
              {
                  using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                  {
                      var preparationOption = new MenuModel.PreparationOptionsGroup(Properties.Resources.PreparationOptionsGroupDefaultName);
                      OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RealObject);
                      if (objectStorage != null)
                          objectStorage.CommitTransientObjectState(preparationOption);
                      RealObject.AddPreparationOption(preparationOption);
                      stateTransition.Consistent = true;
                  }
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Options)));
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionVisibility)));
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionsGroupVisibility)));
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddStepperOptionsGroupVisibility)));


              });

            AddMagnitudeOtionsGoupCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
             {
                 using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                 {
                     var preparationOption = new MenuModel.ItemSelectorOptionsGroup(Properties.Resources.MagnitudeOptionsGroupDefaultName);
                     OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RealObject);
                     if (objectStorage != null)
                         objectStorage.CommitTransientObjectState(preparationOption);
                     RealObject.AddPreparationOption(preparationOption);
                     stateTransition.Consistent = true;
                 }
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Options)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionVisibility)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionsGroupVisibility)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddStepperOptionsGroupVisibility)));


             });
            AddStepperOptionsGroupCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var preparationOption = new MenuModel.StepperOptionsGroup(Properties.Resources.StepOptionsGroupName);
                    OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RealObject);
                    if (objectStorage != null)
                        objectStorage.CommitTransientObjectState(preparationOption);
                    RealObject.AddPreparationOption(preparationOption);
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Options)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionsGroupVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddStepperOptionsGroupVisibility)));

            });

            AddOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var preparationOption = new MenuModel.PreparationScaledOption(Properties.Resources.PreparationScaledOptionDefaultName);
                    OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RealObject);
                    if (objectStorage != null)
                        objectStorage.CommitTransientObjectState(preparationOption);

                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                    var checkUncheckScaleType = (from scaleType in storage.GetObjectCollection<MenuModel.FixedScaleType>()
                                                 where scaleType.UniqueIdentifier == MenuModel.FixedScaleTypes.CheckUncheck
                                                 select scaleType).FirstOrDefault();
                    preparationOption.LevelType = checkUncheckScaleType;
                    preparationOption.Initial = checkUncheckScaleType.Levels[0];

                    RealObject.AddPreparationOption(preparationOption);
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Options)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionsGroupVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddStepperOptionsGroupVisibility)));

            });
            RenameSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedOption.Edit = true;
            }, (object sender) => SelectedOption != null && !MultipleSelection);

            EditSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
              {
                  if (SelectedOption.ViewType == PreparationOptionViewType.Minimize)
                  {
                      foreach (var option in Options)
                          option.Minimize();
                      SelectedOption.Maximaze();
                  }
                  else
                      SelectedOption.Minimize();

              }, (object sender) => SelectedOption != null && !MultipleSelection);

            NewTagCommand = new RelayCommand((object sender) =>
            {
                NewTag();
            });

            DeleteSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    RealObject.RemovePreparationOption(SelectedOption.PreparationOption);
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Options)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionsGroupVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddStepperOptionsGroupVisibility)));

            }, (object sender) => SelectedOption != null && !MultipleSelection);

            CopySelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {


                Dictionary<object, object> mappedObjects = new Dictionary<object, object>();

                if (SelectedOptions.Count > 0)
                {
                    List<MenuModel.IPreparationOption> options = SelectedOptions.Select(x => MenuModel.JsonViewModel.Option.GetOption(x.PreparationOption, mappedObjects)).ToList();


                    var jSetttings = new JsonSerializerSettings(JsonContractType.Serialize, JsonSerializationFormat.TypeScriptJsonSerialization, null);// { TypeNameHandling = ServerSession.Web ? TypeNameHandling.None : TypeNameHandling.All, Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, ChannelUri, InternalChannelUri, ServerSession,Web) };
                    var json = OOAdvantech.Json.JsonConvert.SerializeObject(options, jSetttings);

                    //var json = JsonConvert.SerializeObject(options, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.All });
                    Clipboard.SetData("OptionsJson", json);
                }


            }, (object sender) => SelectedOption != null);

            PasteOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                Dictionary<object, object> mappedObjects = new Dictionary<object, object>();
                var json = Clipboard.GetData("OptionsJson") as string;
                var jSetttings = new JsonSerializerSettings(JsonContractType.Deserialize, JsonSerializationFormat.TypeScriptJsonSerialization, null);
                var options = OOAdvantech.Json.JsonConvert.DeserializeObject<object[]>(json, jSetttings).OfType<MenuModel.IPreparationOption>().ToList();

                foreach (var option in options)
                {
                    if (option is MenuModel.IPreparationScaledOption)
                    {
                        MenuModel.PreparationScaledOption preparationScaledOption = new MenuModel.PreparationScaledOption(option as MenuModel.IPreparationScaledOption, RealObject);
                        RealObject.AddPreparationOption(preparationScaledOption);
                    }
                    if (option is MenuModel.IPreparationOptionsGroup)
                    {
                        MenuModel.PreparationOptionsGroup preparationScaledOption = new MenuModel.PreparationOptionsGroup(option as MenuModel.IPreparationOptionsGroup, RealObject);
                        RealObject.AddPreparationOption(preparationScaledOption);

                    }
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Options)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddOptionsGroupVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddStepperOptionsGroupVisibility)));


            }, (object sender) => Clipboard.ContainsData("OptionsJson"));

            MoveUpSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { MoveUpSelectedOption(); }, (object sender) => SelectedOption != null && !MultipleSelection);
            MoveDownSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { MoveDownSelectedOption(); }, (object sender) => SelectedOption != null && !MultipleSelection);

            SetSelectedOptionCheckUncheckCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { SetSelectedOptionCheckUncheck(); }, (object sender) => SelectedScaledOption != null && SelectedScaledOption.Quantitative && !SelectedScaledOption.IsHidden);
            SetSelectedOptionHideShowCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { SetSelectedOptionHideShow(); }, (object sender) => SelectedOption != null);
        }



        string _OptionShowImagePath = @"pack://application:,,,/MenuItemsEditor;Component/Image/view16.png";
        string _OptionHideImagePath = @"pack://application:,,,/MenuItemsEditor;Component/Image/restriction16.png";
        public string OptionShowImagePath
        {
            get
            {
                if (SelectedScaledOption != null && SelectedScaledOption.IsHidden)
                    return _OptionShowImagePath;
                return _OptionHideImagePath;
            }
        }



        private void SetSelectedOptionHideShow()
        {
            if (SelectedScaledOption != null)
            {
                SelectedScaledOption.TonggleShowHide();
                PreparationOptionChanged(SelectedOption);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionShowImagePath)));
            }
        }

        private void SetSelectedOptionCheckUncheck()
        {
            foreach (var selectedScaledOption in SelectedScaledOptions)
            {
                if (selectedScaledOption.PreparationScaledOption.Initial != null)
                {
                    selectedScaledOption.ToggleUncheckOtption();
                    PreparationOptionChanged(selectedScaledOption);
                }
            }
        }

        /// <MetaDataID>{c092d01e-c781-4564-a161-64bed86f9672}</MetaDataID>
        public void Maximazed(PreparationOptionViewModel preparationOptionViewModel)
        {

            foreach (var option in Options)
            {
                if (option != preparationOptionViewModel)
                    option.ViewType = PreparationOptionViewType.Minimize;
            }
            SelectedOption = Options[0];
            SelectedOption = preparationOptionViewModel;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionsListViewButtonsVisible)));
        }


        /// <MetaDataID>{fa6eaf62-2e32-4909-a2df-2e22160d8e9a}</MetaDataID>
        bool _IsEditable;
        /// <MetaDataID>{d2ece6ad-10b8-49b2-9595-34eccf714b6c}</MetaDataID>
        public bool IsEditable
        {
            get
            {
                return _IsEditable;
            }
        }

        /// <MetaDataID>{1c3dbcb7-3e51-4312-9679-641f7bc3b478}</MetaDataID>
        public string Name
        {
            get
            {
                return RealObject.Name;
            }
            set
            {
                RealObject.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
            }
        }

        List<TagViewModel> _Tags;

        public List<TagViewModel> Tags
        {
            get
            {
                if (_Tags == null)
                {
                    List<TagViewModel> tags = new List<TagViewModel>();
                    int index = 0;
                    if (RealObject.PreparationTags != null)
                    {
                        foreach (var tag in RealObject.PreparationTags.Split(';'))
                        {
                            var tagPresentation = new TagViewModel(tag, index++);
                            tagPresentation.TagDeleted += TagPresentation_TagDeleted;
                            tagPresentation.NameChanged += TagPresentation_NameChanged;
                            tags.Add(tagPresentation);
                        }
                    }
                    _Tags = tags;
                }

                return _Tags;
            }
        }

        private void TagPresentation_TagDeleted(TagViewModel tag)
        {
            _Tags.RemoveAt(tag.Index);
            tag.TagDeleted -= TagPresentation_TagDeleted;
            tag.NameChanged -= TagPresentation_NameChanged;
            int i = 0;
            RealObject.PreparationTags = null;
            foreach (var theTag in _Tags)
            {
                theTag.Index = _Tags.IndexOf(theTag);
                if (_Tags.IndexOf(theTag) == 0)
                    RealObject.PreparationTags = tag.Name;
                else
                    RealObject.PreparationTags = ";" + tag.Name;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
        }

        private void NewTag()
        {
            var tagPresentation = new TagViewModel("new tag", Tags.Count);
            tagPresentation.TagDeleted += TagPresentation_TagDeleted;
            tagPresentation.NameChanged += TagPresentation_NameChanged;
            if (Tags.Count == 0)
                RealObject.PreparationTags += tagPresentation.Name;
            else
                RealObject.PreparationTags += ";" + tagPresentation.Name;

            _Tags.Add(tagPresentation);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));

        }

        private void TagPresentation_NameChanged(TagViewModel tag)
        {
            int i = 0;
            RealObject.PreparationTags = null;
            foreach (var theTag in _Tags)
            {
                theTag.Index = _Tags.IndexOf(theTag);
                if (_Tags.IndexOf(theTag) == 0)
                    RealObject.PreparationTags = tag.Name;
                else
                    RealObject.PreparationTags = ";" + tag.Name;
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
        /// <MetaDataID>{cd9889c6-777c-4984-9a55-123a5e051232}</MetaDataID>
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



        /// <MetaDataID>{130ae6be-6db5-40f8-9685-ce5c89bd2b23}</MetaDataID>
        private bool CanRenameSelectedOption()
        {
            return SelectedOption != null;
        }

        /// <MetaDataID>{cd8796e1-21c2-45cf-9176-c7ef94ca54bf}</MetaDataID>
        public void PreparationOptionChanged(PreparationOptionViewModel preparationOptionViewModel)
        {

        }

        /// <MetaDataID>{61c7a799-a623-4a0f-ab5d-70c527fabb7a}</MetaDataID>
        ViewModelWrappers<MenuModel.IPreparationOptionsGroup, PreparationOptionsGroupViewModel> PreparationOptionsWrappers = new ViewModelWrappers<MenuModel.IPreparationOptionsGroup, PreparationOptionsGroupViewModel>();

        /// <MetaDataID>{3bb11edb-8e8e-4fc4-9787-517852c5f262}</MetaDataID>
        ViewModelWrappers<MenuModel.IPreparationScaledOption, PreparationScaledOptionViewModel> PreparationScaledOptionsWrappers = new ViewModelWrappers<MenuModel.IPreparationScaledOption, PreparationScaledOptionViewModel>();

        /// <MetaDataID>{bf143a0a-adfd-4864-9761-963c8dc6329e}</MetaDataID>
        public List<PreparationOptionViewModel> Options
        {
            get
            {
                return (from scaledOption in RealObject.Options.OfType<MenuModel.IPreparationScaledOption>()
                        select PreparationScaledOptionsWrappers.GetViewModelFor(scaledOption, scaledOption, this, _IsEditable)).OfType<PreparationOptionViewModel>().Union(
                    from optionsGroup in RealObject.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                    select PreparationOptionsWrappers.GetViewModelFor(optionsGroup, optionsGroup, this, _IsEditable)).ToList();
            }
        }


        /// <exclude>Excluded</exclude>
        PreparationOptionViewModel _SelectedOption;
        /// <MetaDataID>{2ce89a60-534b-493a-8a7b-0d216791d455}</MetaDataID>
        public PreparationOptionViewModel SelectedOption
        {
            get
            {
                return _SelectedOption;
            }
            set
            {
                _SelectedOption = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedOption)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionShowImagePath)));

            }
        }
        public bool MultipleSelection
        {
            get
            {
                return this.Options.Where(x => x.IsSelected).Count() > 1;
            }
        }
        public List<PreparationOptionViewModel> SelectedOptions
        {
            get
            {
                return this.Options.Where(x => x.IsSelected).ToList();
            }
        }

        public PreparationScaledOptionViewModel SelectedScaledOption
        {
            get
            {
                return SelectedOption as PreparationScaledOptionViewModel;
            }
        }

        public List<PreparationScaledOptionViewModel> SelectedScaledOptions
        {
            get
            {
                return SelectedOptions.OfType<PreparationScaledOptionViewModel>().ToList();

            }
        }


        public WPFUIElementObjectBind.RelayCommand NewTagCommand { get; protected set; }

        /// <MetaDataID>{80e44ffa-d230-4eb5-8d25-24d79a0a17f5}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand EditSelectedOptionCommand { get; protected set; }
        /// <MetaDataID>{2d356ff9-2775-452a-b698-52b73adfec03}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand DeleteSelectedOptionCommand { get; protected set; }



        public WPFUIElementObjectBind.RelayCommand CopySelectedOptionCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand PasteOptionCommand { get; protected set; }

        /// <MetaDataID>{6e39d5f4-ba91-4220-9f40-a2f476576d97}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand RenameSelectedOptionCommand { get; protected set; }

        /// <MetaDataID>{06271ae9-1630-4b5b-bb42-676d72c51cc3}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand AddOptionCommand { get; protected set; }

        /// <MetaDataID>{7feaa9e4-0c1b-4721-90f4-9960c67ff0d9}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand AddOptionsGroupCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand AddStepperOptionsGroupCommand { get; protected set; }

        /// <MetaDataID>{c5f65966-91e3-4726-9f19-ff90ffea397a}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand AddMagnitudeOtionsGoupCommand { get; protected set; }


        public Visibility AddStepperOptionsGroupVisibility
        {
            get
            {
                if (RealObject.Options.OfType<MenuModel.PreparationOptionsGroup>().Where(x => x.GetType() == typeof(MenuModel.PreparationOptionsGroup)).Count() > 0)
                    return Visibility.Collapsed;
                if (RealObject.Options.OfType<MenuModel.PreparationScaledOption>().Where(x => x.GetType() == typeof(MenuModel.PreparationScaledOption)).Count() > 0)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }
        }

        public Visibility AddOptionsGroupVisibility
        {
            get
            {
                if (RealObject.Options.OfType<MenuModel.StepperOptionsGroup>().Where(x => x.GetType() == typeof(MenuModel.StepperOptionsGroup)).Count() > 0)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }
        }
        public Visibility AddOptionVisibility
        {
            get
            {
                if (RealObject.Options.OfType<MenuModel.StepperOptionsGroup>().Where(x => x.GetType() == typeof(MenuModel.StepperOptionsGroup)).Count() > 0)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }
        }

        private void MoveDownSelectedOption()
        {
            if (SelectedOption is PreparationScaledOptionViewModel)
            {
                int index = Options.IndexOf(SelectedOption);
                if (index < Options.Count - 1)
                {
                    if (Options[index + 1] is PreparationScaledOptionViewModel)
                    {
                        int pos = RealObject.Options.IndexOf(Options[index + 1].PreparationOption);
                        RealObject.MovePreparationOption(SelectedOption.PreparationOption, pos);
                    }
                }
            }
            else if (SelectedOption is PreparationOptionsGroupViewModel)
            {
                int index = Options.IndexOf(SelectedOption);
                if (index < Options.Count - 1)
                {
                    if (Options[index + 1] is PreparationOptionsGroupViewModel)
                    {
                        int pos = RealObject.Options.IndexOf(Options[index + 1].PreparationOption);
                        RealObject.MovePreparationOption(SelectedOption.PreparationOption, pos);
                    }
                }
            }


            //if (SelectedOption is PreparationOptionsGroupViewModel)
            //{

            //}
            //    int pos = RealObject.Options.IndexOf(SelectedOption.PreparationOption);
            //if (pos < Options.Count - 1)
            //    RealObject.MovePreparationOption(SelectedOption.PreparationOption, pos + 1);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Options)));
        }

        /// <MetaDataID>{12635194-9306-42ea-a7a9-b68a7a135fe7}</MetaDataID>
        private void MoveUpSelectedOption()
        {

            if (SelectedOption is PreparationScaledOptionViewModel)
            {
                int index = Options.IndexOf(SelectedOption);
                if (index > 0)
                {
                    if (Options[index - 1] is PreparationScaledOptionViewModel)
                    {
                        int pos = RealObject.Options.IndexOf(Options[index - 1].PreparationOption);
                        RealObject.MovePreparationOption(SelectedOption.PreparationOption, pos);
                    }
                }
            }
            else if (SelectedOption is PreparationOptionsGroupViewModel)
            {
                int index = Options.IndexOf(SelectedOption);
                if (index > 0)
                {
                    if (Options[index - 1] is PreparationOptionsGroupViewModel)
                    {
                        int pos = RealObject.Options.IndexOf(Options[index - 1].PreparationOption);
                        RealObject.MovePreparationOption(SelectedOption.PreparationOption, pos);
                    }
                }
            }





            //int pos = RealObject.Options.IndexOf(SelectedOption.PreparationOption);
            //if (pos > 0)
            //    RealObject.MovePreparationOption(SelectedOption.PreparationOption, pos - 1);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Options)));

        }

        public void Minimized(PreparationOptionViewModel preparationOptionViewModel)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionsListViewButtonsVisible)));
        }

        public WPFUIElementObjectBind.RelayCommand MoveDownSelectedOptionCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand MoveUpSelectedOptionCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand SetSelectedOptionCheckUncheckCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand SetSelectedOptionHideShowCommand { get; protected set; }


        public Visibility OptionsListViewButtonsVisible
        {
            get
            {
                foreach (var option in Options)
                {

                    if (option.ViewType != PreparationOptionViewType.Minimize)
                        return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
    }


}



//Severity Code	Description	Project	File	Line	Suppression State
//Error		[A]Xceed.Wpf.Toolkit.WatermarkTextBox cannot be cast to [B]Xceed.Wpf.Toolkit.WatermarkTextBox.Type A originates from 
//    'Xceed.Wpf.Toolkit, Version=3.7.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4' in the context 'LoadNeither' at location 
//    'C:\Users\jimli\AppData\Local\Microsoft\VisualStudio\16.0_8c82261d\Designer\ShadowCache\re11xlhc.3wy\kvqugq3u.xuh\Xceed.Wpf.Toolkit.dll'. 
//    Type B originates from 
//    'Xceed.Wpf.Toolkit, Version=3.7.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4' in the context 'LoadNeither' at location 
//    'C:\Users\jimli\AppData\Local\Microsoft\VisualStudio\16.0_8c82261d\Designer\ShadowCache\2endohll.w01\pytmz01q.ave\Xceed.Wpf.Toolkit.dll'.	
//    MenuItemsEditor	F:\X - Drive\Source\OpenVersions\FlavourBusiness\MenuItemsEditor\Views\PreparationOptionView.xaml 885
