using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using OOAdvantech.PersistenceLayer;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{a921cf8c-1bcd-4972-8c52-84e034a32bdd}</MetaDataID>
    public class HeadingTypesAccentsViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        MenuPresentationModel.MenuStyles.IHeadingStyle TitleHeadingsStyle;
        MenuPresentationModel.MenuStyles.IHeadingStyle NormalHeadingsStyle;
        MenuPresentationModel.MenuStyles.IHeadingStyle SubHeadingsStyle;

        
        static ObjectStorage _HeadingAccentStorage;
        public static ObjectStorage HeadingAccentStorage
        {
            get
            {
                if (_HeadingAccentStorage == null)
                {
                    string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    appDataPath += "\\DontWaitWater";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string storageLocation = appDataPath + "\\HeadingAccents.xml";

                    try
                    {
                        _HeadingAccentStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("StyleSheets", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _HeadingAccentStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("StyleSheets",
                                                                    storageLocation,
                                                                    "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }
                        else
                            throw error;
                    }
                    catch (Exception error)
                    {
                    }
                }
                return _HeadingAccentStorage;
            }

            set
            {
                _HeadingAccentStorage = value;
            }
        }

        public HeadingTypesAccentsViewModel(MenuPresentationModel.MenuStyles.IHeadingStyle titleHeadingsStyle, MenuPresentationModel.MenuStyles.IHeadingStyle normalHeadingsStyle, MenuPresentationModel.MenuStyles.IHeadingStyle subHeadingsStyle)
        {
            TitleHeadingsStyle = titleHeadingsStyle;
            NormalHeadingsStyle = normalHeadingsStyle;
            SubHeadingsStyle = subHeadingsStyle;


            if (TitleHeadingsStyle.Accent != null &&
            !string.IsNullOrWhiteSpace(TitleHeadingsStyle.Accent.AccentColor) &&
            TitleHeadingsStyle.Accent.AccentColor != "none")
            {
                _TitleHeadingsAccentSelectedColor = (Color)ColorConverter.ConvertFromString(TitleHeadingsStyle.Accent.AccentColor);
                _TitleHeadingsAccentColorize = true;
            }
            else
                _TitleHeadingsAccentColorize = false;

            if (NormalHeadingsStyle.Accent != null &&
                !string.IsNullOrWhiteSpace(NormalHeadingsStyle.Accent.AccentColor) &&
               NormalHeadingsStyle.Accent.AccentColor != "none")
            {
                _NormalHeadingsAccentSelectedColor = (Color)ColorConverter.ConvertFromString(NormalHeadingsStyle.Accent.AccentColor);
                _NormalHeadinsAccentColorize = true;
            }
            else
                _NormalHeadinsAccentColorize = false;


            if (SubHeadingsStyle.Accent != null &&
            !string.IsNullOrWhiteSpace(SubHeadingsStyle.Accent.AccentColor) &&
            SubHeadingsStyle.Accent.AccentColor != "none")
            {
                _SubHeadingAccentSelectedColor = (Color)ColorConverter.ConvertFromString(SubHeadingsStyle.Accent.AccentColor);
                _SubHeadingsAccentColorize = true;
            }
            else
                _SubHeadingsAccentColorize = false;

            if (HeadingAccentStorage != null)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(HeadingAccentStorage);
                var HeadingAccents = (from accent in storage.GetObjectCollection<MenuPresentationModel.MenuStyles.Accent>() select new AccentViewModel(accent)).ToList();
                _AccentImages = HeadingAccents;//(from accent in HeadingAccents select new AccentViewModel(accent)).ToList();
                _AccentImages.Insert(0, new AccentViewModel(AccentViewModel.AccentViewModelType.None));
                _TitleHeadingsSelectedAccent = _AccentImages[0];
                _NormalHeadingSelectedAccent = _AccentImages[0];
                _SubHeadingsSelectedAccent = _AccentImages[0];

                if (TitleHeadingsStyle.Accent != null)
                    _TitleHeadingsSelectedAccent = (from accent in _AccentImages where accent.Accent != null && accent.Accent.Name == TitleHeadingsStyle.Accent.Name select accent).FirstOrDefault();

                if (NormalHeadingsStyle.Accent != null)
                    _NormalHeadingSelectedAccent = (from accent in _AccentImages where accent.Accent != null && accent.Accent.Name == NormalHeadingsStyle.Accent.Name select accent).FirstOrDefault();

                if (subHeadingsStyle.Accent != null)
                    _SubHeadingsSelectedAccent = (from accent in _AccentImages where accent.Accent != null && accent.Accent.Name == subHeadingsStyle.Accent.Name select accent).FirstOrDefault();
            }
        }

        /// <exclude>Excluded</exclude>
        AccentViewModel _TitleHeadingsSelectedAccent;
        public AccentViewModel TitleHeadingsSelectedAccent
        {
            get
            {

                return _TitleHeadingsSelectedAccent;
            }
            set
            {
                _TitleHeadingsSelectedAccent = value;
                if (_TitleHeadingsSelectedAccent != null)
                    TitleHeadingsStyle.Accent = _TitleHeadingsSelectedAccent.Accent;
            }
        }

        /// <exclude>Excluded</exclude>
        bool _NormalHeadinsAccentColorize;
        public bool NormalHeadingsAccentColorize
        {
            get
            {
                return _NormalHeadinsAccentColorize;
            }
            set
            {
                _NormalHeadinsAccentColorize = value;
                if (!_NormalHeadinsAccentColorize)
                {
                    _NormalHeadingsAccentSelectedColor = Colors.LightGray;
                    NormalHeadingsStyle.Accent.AccentColor = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NormalHeadingsAccentSelectedColor)));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _SubHeadingsAccentColorize;
        public bool SubHeadingsAccentColorize
        {
            get
            {
                return _SubHeadingsAccentColorize;
            }
            set
            {
                _SubHeadingsAccentColorize = value;

                if (!_SubHeadingsAccentColorize)
                {
                    _SubHeadingAccentSelectedColor = Colors.LightGray;
                    SubHeadingsStyle.Accent.AccentColor = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubHeadingAccentSelectedColor)));
                }
            }
        }
        /// <exclude>Excluded</exclude>
        bool _TitleHeadingsAccentColorize;
        public bool TitleHeadingsAccentColorize
        {
            get
            {
                return _TitleHeadingsAccentColorize;
            }
            set
            {
                _TitleHeadingsAccentColorize = value;

                if (!_TitleHeadingsAccentColorize)
                {
                    _TitleHeadingsAccentSelectedColor = Colors.LightGray;
                    TitleHeadingsStyle.Accent.AccentColor =null;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleHeadingsAccentSelectedColor)));

                }
            }
        }
        /// <exclude>Excluded</exclude>
        Color _TitleHeadingsAccentSelectedColor = Colors.LightGray;
        public Color TitleHeadingsAccentSelectedColor
        {
            get
            {
                return _TitleHeadingsAccentSelectedColor;
            }
            set
            {
                _TitleHeadingsAccentSelectedColor = value;
                TitleHeadingsStyle.Accent.AccentColor = new ColorConverter().ConvertToString(_TitleHeadingsAccentSelectedColor);

            }
        }

        /// <exclude>Excluded</exclude>
        AccentViewModel _SubHeadingsSelectedAccent;
        public AccentViewModel SubHeadingsSelectedAccent
        {
            get
            {

                return _SubHeadingsSelectedAccent;
            }
            set
            {
                _SubHeadingsSelectedAccent = value;
            }
        }

        /// <exclude>Excluded</exclude>
        Color _SubHeadingAccentSelectedColor = Colors.LightGray;
        public Color SubHeadingAccentSelectedColor
        {
            get
            {
                return _SubHeadingAccentSelectedColor;
            }
            set
            {
                _SubHeadingAccentSelectedColor = value;
            }
        }

        /// <exclude>Excluded</exclude>
        AccentViewModel _NormalHeadingSelectedAccent;
        public AccentViewModel NormalHeadingSelectedAccent
        {
            get
            {

                return _NormalHeadingSelectedAccent;
            }
            set
            {
                _NormalHeadingSelectedAccent = value;
                if (_NormalHeadingSelectedAccent != null)
                    NormalHeadingsStyle.Accent = _NormalHeadingSelectedAccent.Accent;
            }
        }

        /// <exclude>Excluded</exclude>
        Color _NormalHeadingsAccentSelectedColor = Colors.LightGray;
        public Color NormalHeadingsAccentSelectedColor
        {
            get
            {
                return _NormalHeadingsAccentSelectedColor;
            }
            set
            {
                _NormalHeadingsAccentSelectedColor = value;

                NormalHeadingsStyle.Accent.AccentColor = new ColorConverter().ConvertToString(_NormalHeadingsAccentSelectedColor);
            }
        }



        /// <exclude>Excluded</exclude>
        List<AccentViewModel> _AccentImages;
        public List<AccentViewModel> AccentImages
        {
            get
            {
                return _AccentImages;
            }
        }
    }
}
