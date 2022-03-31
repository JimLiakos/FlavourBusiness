using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{a5029aea-1f16-402d-8b23-3e958c492af4}</MetaDataID>
    public class TagViewModel : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, INotifyPropertyChanged
    {
        public readonly ITag Tag;
        public TagViewModel(ITag tag)
        {
            Tag = tag;
            Name = tag.Name;


            DeleteTagCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                TagDeleted?.Invoke(this);
            });
        }

        public TagViewModel()
        {

        }

        public TagViewModel(ITag tag, IItemsPreparationInfo itemsPreparationInfo, string ownerUri) : this(tag)
        {
            this.ItemsPreparationInfo = itemsPreparationInfo;
            this.OwnerUri = ownerUri;
        }

        public string Name
        {
            get => Tag.Name;
            set
            {

                Tag.Name = value;
                if (ItemsPreparationInfo != null)
                {
                    ItemsPreparationInfo.UpdateTag(Tag);
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedName)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public bool UnTranslatedName
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
        /// <MetaDataID>{56097b50-8be4-4397-8a8b-8309c42108a0}</MetaDataID>
        public ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new Translator();
                return _Translator;
            }
        }

        public WPFUIElementObjectBind.RelayCommand DeleteTagCommand { get; protected set; }

        public readonly IItemsPreparationInfo  ItemsPreparationInfo;
        public readonly string OwnerUri;

        /// <exclude>Excluded</exclude>
        bool _Edit;


        public bool Edit
        {
            get => _Edit;

            internal set
            {
                _Edit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
                if (value == false)
                {
                    Tag.Name = Name;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));

                }
            }
        }

        public event TagDeletedHandle TagDeleted;

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public delegate void TagDeletedHandle(TagViewModel tag);
    public delegate void NameChangedHandle(TagViewModel tag);
}
