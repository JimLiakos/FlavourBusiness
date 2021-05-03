using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;
using System.Windows;
namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{bd6d5217-7fb2-441d-bb53-187a6f3febfd}</MetaDataID>
    public class PreparationOptionViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        public virtual FontWeight FontWeight
        {
            get
            {
                return FontWeights.Normal;
            }
        }

        public virtual bool IsHidden
        {
            get => false;
        }
        /// <MetaDataID>{eac8a25f-5bc6-4816-b8aa-7e46d763d194}</MetaDataID>
        public MenuItemViewModel MenuItemViewModel
        {
            get
            {
                if (PreparationOptionsListView is MenuItemTypeViewModel)
                    return (PreparationOptionsListView as MenuItemTypeViewModel).HostingMenuItemViewModel;

                if (PreparationOptionsListView is PreparationOptionViewModel)
                    return (PreparationOptionsListView as PreparationOptionViewModel).MenuItemViewModel;

                if (PreparationOptionsListView is MenuItemViewModel)
                    return PreparationOptionsListView as MenuItemViewModel;

                return null;
            }
        }
        public PreparationOptionViewModel()
        {

        }

        /// <MetaDataID>{857b7ef9-29ba-41d6-8bd6-67ce614266d3}</MetaDataID>
        internal protected IPreparationOptionsListView PreparationOptionsListView;

        /// <MetaDataID>{8df5bf9c-7ad3-479f-a2c8-755f6f4ad090}</MetaDataID>
        public readonly MenuModel.IPreparationOption PreparationOption;

        /// <MetaDataID>{c93bdbd9-53fd-4877-8f72-10b283e9621a}</MetaDataID>
        public PreparationOptionViewModel(MenuModel.IPreparationOption preparationOption, IPreparationOptionsListView preparationOptionsListView, bool isEditable)
        {
            _IsEditable = isEditable;
            //_IsEditable = true;
            PreparationOption = preparationOption;
            PreparationOptionsListView = preparationOptionsListView;

            MinimizeCommand = new RelayCommand((object sender) =>
             {
                 Minimize();
             });
            MaximizeCommand = new RelayCommand((object sender) =>
            {
                Maximaze();
            });

            TranslateOptionNameCommand = new RelayCommand((object sender) =>
            {
                TranslateOptionName();
            });

        }

        private void TranslateOptionName()
        {
            Name= BingAPILibrary.Translator.TranslateString(Name, OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
            //OOAdvantech.CultureContext
        }

        event PropertyChangedEventHandler _PropertyChanged;
        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _PropertyChanged += value;
            }
            remove
            {
                _PropertyChanged -= value;
            }
        }


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


        /// <exclude>Excluded</exclude>
        protected PreparationOptionViewType _ViewType = PreparationOptionViewType.Minimize;
        /// <MetaDataID>{94d6628a-7227-49f3-beab-e511195ff399}</MetaDataID>
        public PreparationOptionViewType ViewType
        {
            get
            {
                return _ViewType;
            }
            internal set
            {
                if (_ViewType != value)
                {
                    _ViewType = value;
                    _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewType)));
                }
            }
        }

        /// <MetaDataID>{fa9e4121-3b49-45a9-a083-5bcd43af55cc}</MetaDataID>
        public virtual void Maximaze()
        {
        }
        /// <MetaDataID>{74935706-18fc-4d82-a9c5-2b827cc8e281}</MetaDataID>
        public virtual void Minimize()
        {

            _ViewType = PreparationOptionViewType.Minimize;
            PreparationOptionsListView?.Minimized(this);
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewType)));

        }

        /// <MetaDataID>{ccad23f7-7b7e-49b5-a4e8-63d83acd9341}</MetaDataID>
        public virtual string Name
        {
            get
            {
                return PreparationOption.Name;
            }
            set
            {
                PreparationOption.Name = value;
                var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                {
                    using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                    {
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    }
                }));
            }
        }

        public bool UnTranslated
        {
            get
            {
                string name = Name;
                using(OOAdvantech.CultureContext cultureContext=new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo,false))
                {
                    return name != Name;
                }
            }
        }


        /// <MetaDataID>{1e2c8d33-4a94-4d17-a9e7-dc34ee1a8885}</MetaDataID>
        bool _IsEditable;
        /// <MetaDataID>{f4defe1b-3606-492f-8e0b-20fe2e501349}</MetaDataID>
        public bool IsEditable
        {
            get
            {
                return _IsEditable;
            }
        }

        /// <MetaDataID>{367b3e83-f09b-4d59-b667-8e21073d7b6f}</MetaDataID>
        public virtual Visibility HideOptionIsVisible
        {
            get
            {
                return Visibility.Collapsed;
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
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }


        /// <MetaDataID>{e6e7f6d2-32e6-42cc-a29a-e07161ad4336}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand MinimizeCommand { get; protected set; }
        /// <MetaDataID>{3cec9d5f-cdc5-4878-9c32-19132f8f66bd}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand MaximizeCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand TranslateOptionNameCommand  { get; protected set; }



    }


    /// <MetaDataID>{1d6f75f2-c7ff-4161-b758-63bee4bee1fa}</MetaDataID>
    public interface IPreparationOptionsListView
    {
        /// <MetaDataID>{637be57f-7525-4890-ae34-28ca1b381048}</MetaDataID>
        void Maximazed(PreparationOptionViewModel preparationOptionViewModel);

        /// <MetaDataID>{db21498b-f6db-4b64-b2e7-8f66f2101455}</MetaDataID>
        void PreparationOptionChanged(PreparationOptionViewModel preparationOptionViewModel);
        /// <MetaDataID>{5a84bef9-944c-44e5-a903-e49be37bb509}</MetaDataID>
        void Minimized(PreparationOptionViewModel preparationOptionViewModel);

        /// <MetaDataID>{6a411435-861a-48b2-a348-9e12f95605a1}</MetaDataID>
        Visibility OptionsListViewButtonsVisible { get; }

    }
}
