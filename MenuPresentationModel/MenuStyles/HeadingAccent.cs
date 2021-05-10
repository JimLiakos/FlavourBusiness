using System;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using OOAdvantech;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{7f8f9237-334f-4291-a910-99049f48d672}</MetaDataID>
    [BackwardCompatibilityID("{7f8f9237-334f-4291-a910-99049f48d672}")]
    [Persistent()]
    public class Accent : MarshalByRefObject,  IAccent, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {
        /// <exclude>Excluded</exclude>
        double _Width;
        /// <MetaDataID>{15c2cbce-b0c5-41a5-9104-e7612f7b4e61}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+22")]
        public double Width
        {
            get => _Width;
            set
            {
                if (_Width != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Width = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool _OrgSize;
        /// <MetaDataID>{45a4806b-dfb6-4081-8459-d25567aa2690}</MetaDataID>
        [PersistentMember(nameof(_OrgSize))]
        [BackwardCompatibilityID("+21")]
        public bool OrgSize
        {
            get => _OrgSize;
            set
            {

                if (_OrgSize != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OrgSize = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double _MinHeight;
        /// <MetaDataID>{cc2f3f9b-de75-4315-be8a-35028fe27ec7}</MetaDataID>
        [PersistentMember(nameof(_MinHeight))]
        [BackwardCompatibilityID("+19")]
        public double MinHeight
        {
            get => _MinHeight;
            set
            {
                if (_MinHeight != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MinHeight = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{1d3a529c-cd0d-49f8-8c8d-b0fcee18b29a}</MetaDataID>
        double _MinWidth;
        /// <MetaDataID>{d7f68e86-f203-4704-8a3c-efc49822a5ed}</MetaDataID>
        [PersistentMember(nameof(_MinWidth))]
        [BackwardCompatibilityID("+20")]
        public double MinWidth
        {
            get => _MinWidth;
            set
            {
                if (_MinWidth != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MinWidth = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Unit _MarginUnit = Unit.em;

        /// <MetaDataID>{71ca556c-dab4-4a7a-85e5-27ea3e2ce89c}</MetaDataID>
        [PersistentMember(nameof(_MarginUnit))]
        [BackwardCompatibilityID("+18")]
        public Unit MarginUnit
        {
            get => _MarginUnit;
            set
            {
                if (_MarginUnit != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MarginUnit = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _MultipleItemsAccent;

        /// <MetaDataID>{713f9a7c-a803-4a37-aeea-3077b7882d41}</MetaDataID>
        [PersistentMember(nameof(_MultipleItemsAccent))]
        [BackwardCompatibilityID("+17")]
        public bool MultipleItemsAccent
        {
            get => _MultipleItemsAccent;
            set
            {

                if (_MultipleItemsAccent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MultipleItemsAccent = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{f3149d9e-a2f6-47c1-ad3c-525483dd2610}</MetaDataID>
        public static string ResourcesRootPath;


        public event ObjectChangeStateHandle ObjectChangeState;

        /// <exclude>Excluded</exclude>
        string _AccentColor;
        
        /// <MetaDataID>{5e9f5cca-be6f-4a28-a45a-77e898321357}</MetaDataID>
        [PersistentMember(nameof(_AccentColor))]
        [BackwardCompatibilityID("+16")]
        public string AccentColor
        {
            get
            {
                if (OrgHeadingAccent != null && _AccentColor==null)
                    return OrgHeadingAccent.AccentColor;
                return _AccentColor;
            }
            set
            {
                if(value!=null&& value.Length==9)
                    value = "#" + value.Substring(3);


                if (_AccentColor != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AccentColor = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(AccentColor));
                }
            }
        }

        /// <MetaDataID>{96cd8419-8170-4c36-af64-4964072dee13}</MetaDataID>
        internal static Accent Clone(Accent value)
        {
            Accent headingAccent = new Accent();

            headingAccent.AccentColor = value.AccentColor;
            foreach (var image in value.AccentImages)
                headingAccent.AddAccentImage(new MenuImage(image.Image, image.Width, image.Height));
            headingAccent.DoubleImage = value.DoubleImage;
            headingAccent.FullRowImage = value.FullRowImage;
            headingAccent.Height = value.Height;
            headingAccent.Width = value.Width;
            headingAccent.MarginBottom = value.MarginBottom;
            headingAccent.MarginLeft = value.MarginLeft;
            headingAccent.MarginRight = value.MarginRight;
            headingAccent.MarginTop = value.MarginTop;
            headingAccent.MarginUnit = value.MarginUnit;
            headingAccent.MinHeight = value.MinHeight;
            headingAccent.MinWidth = value.MinWidth;
            headingAccent.MultipleItemsAccent = value.MultipleItemsAccent;
            headingAccent.OrgSize = value.OrgSize;

            headingAccent.Name = value.Name;
            headingAccent.SelectionAccentImageUri = value.SelectionAccentImageUri;
            headingAccent.TextBackgroundImage = value.TextBackgroundImage;
            headingAccent.UnderlineImage = value.UnderlineImage;
            headingAccent.OverlineImage = value.OverlineImage;
            headingAccent.OrgSize = value.OrgSize;
            


            return headingAccent;

        }

        internal static void Copy( Accent target, Accent source)
        {
            Accent headingAccent = target;
            headingAccent.AccentColor = source.AccentColor;
            foreach (var image in source.AccentImages)
                headingAccent.AddAccentImage(new MenuImage(image.Image, image.Width, image.Height));
            headingAccent.DoubleImage = source.DoubleImage;
            headingAccent.FullRowImage = source.FullRowImage;
            headingAccent.Height = source.Height;
            headingAccent.MarginBottom = source.MarginBottom;
            headingAccent.MarginLeft = source.MarginLeft;
            headingAccent.MarginRight = source.MarginRight;
            headingAccent.MarginTop = source.MarginTop;
            headingAccent.Name = source.Name;
            headingAccent.SelectionAccentImageUri = source.SelectionAccentImageUri;
            headingAccent.TextBackgroundImage = source.TextBackgroundImage;
            headingAccent.UnderlineImage = source.UnderlineImage;
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{ae8f0451-b74b-436b-a794-96392bb8ee12}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+12")]
        private string AccentImagesJson;


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IImage> _AccentImages = new OOAdvantech.Collections.Generic.Set<IImage>();
        /// <MetaDataID>{37d13ede-2e9a-49c6-81b2-a3bb5d8e90c5}</MetaDataID>
        [PersistentMember(nameof(_AccentImages))]
        [BackwardCompatibilityID("+14")]
        public System.Collections.Generic.IList<IImage> AccentImages
        {
            get
            {
                if (OrgHeadingAccent != null && (  _AccentImages.Count!= OrgHeadingAccent.AccentImages.Count))
                    return OrgHeadingAccent.AccentImages;

                return _AccentImages.AsReadOnly().ToList();
            }
        }

        /// <MetaDataID>{4855a0c7-e40d-40d3-89d4-85fa41f44d0b}</MetaDataID>
        public void AddAccentImage(IImage accentImage)
        {
            if (_AccentImages == null)
                _AccentImages = new OOAdvantech.Collections.Generic.Set<MenuStyles.IImage>();
            if (!_AccentImages.Contains(accentImage))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {

                    _AccentImages.Add(accentImage);
                    AccentImagesJson = JsonConvert.SerializeObject(_AccentImages, Formatting.None, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{bab5728d-4539-4685-b209-1d6754281be9}</MetaDataID>
        public void DeleteAccentImage(IImage accentImage)
        {
           
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _AccentImages.Remove(accentImage);

                AccentImagesJson = JsonConvert.SerializeObject(_AccentImages, Formatting.None, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                stateTransition.Consistent = true;
            }

            //if (OrgHeadingAccent != null && _AccentImages.Count == 0)
            //    _AccentImages = null;


        }

        /// <MetaDataID>{d9d4c26f-7a0e-4bce-bd39-bcbbda31bec0}</MetaDataID>
        public void OnCommitObjectState()
        {

        }

        /// <MetaDataID>{045e3c58-7b86-49eb-9654-f2382eeb3823}</MetaDataID>
        public void BeforeCommitObjectState()
        {
            foreach(var image in _AccentImages)
            {
                if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(image) == null)
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(image);
            }
        }
        /// <MetaDataID>{e6a8bc2e-4ed9-4fe0-8d9f-ec5e6515fbf2}</MetaDataID>
        public void OnActivate()
        {
            //if (string.IsNullOrWhiteSpace(AccentImagesJson))
            //    _AccentImages = new System.Collections.Generic.List<Resource>();
            //else
            //    _AccentImages = JsonConvert.DeserializeObject<System.Collections.Generic.List<Resource>>(AccentImagesJson);
        }

        /// <MetaDataID>{189660ba-84f1-43e5-93ce-d565c03bb961}</MetaDataID>
        public void OnDeleting()
        {

        }

        /// <MetaDataID>{72965d3e-d6d2-4be5-8f49-dbba8e140318}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{9d59d389-40b7-4272-8d27-f5cb3d21b223}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{27c0b027-3cb4-4de5-a1cb-c962ba56decf}</MetaDataID>
        public void ChangeOrgStyle(IAccent orgHeadingAccent)
        {
            OrgHeadingAccent = orgHeadingAccent;
        }

        /// <MetaDataID>{e8d71e12-617d-4bac-b295-d23ca783f449}</MetaDataID>
        public bool IsTheSameWith(IAccent accent)
        {
            if (accent == null)
                return false;
            var accentImages = accent.AccentImages;
            if (AccentImages.Count != accent.AccentImages.Count)
                return false;

            for (int i = 0; i != _AccentImages.Count; i++)
            {
                if (AccentImages[i].Uri != accentImages[i].Uri)
                    return false;
            }
            if (AccentColor != accent.AccentColor)
                return false;
            return true;
        }

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{bfed14ac-f7d1-4265-afdf-53ba232c429f}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+11")]
        public string Name
        {
            get
            {
                if (OrgHeadingAccent != null && _Name == null)
                    return OrgHeadingAccent.Name;
                return _Name;
            }

            set
            {

                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        double? _MarginRight;

        /// <MetaDataID>{502cc2a1-9091-4b47-a640-598e60fbe353}</MetaDataID>
        [PersistentMember(nameof(_MarginRight))]
        [BackwardCompatibilityID("+9")]
        public double MarginRight
        {
            get
            {
                if (OrgHeadingAccent != null && !_MarginRight.HasValue)
                    return OrgHeadingAccent.MarginRight;
                if (!_MarginRight.HasValue)
                    return default(double);
                return _MarginRight.Value;
            }

            set
            {

                if (_MarginRight != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MarginRight = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        double? _MarginBottom;
        /// <MetaDataID>{6c1e3486-bbe1-4f00-bc5a-de89c0dbbb87}</MetaDataID>
        [PersistentMember(nameof(_MarginBottom))]
        [BackwardCompatibilityID("+8")]
        public double MarginBottom
        {
            get
            {
                if (OrgHeadingAccent != null && !_MarginBottom.HasValue)
                    return OrgHeadingAccent.MarginBottom;
                if (!_MarginBottom.HasValue)
                    return default(double);
                return _MarginBottom.Value;
            }

            set
            {

                if (_MarginBottom != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MarginBottom = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double? _MarginTop;
        /// <MetaDataID>{979cb7d5-f25a-4e4e-9fe2-5a628f2c126f}</MetaDataID>
        [PersistentMember(nameof(_MarginTop))]
        [BackwardCompatibilityID("+7")]
        public double MarginTop
        {
            get
            {
                if (OrgHeadingAccent != null && !_MarginTop.HasValue)
                    return OrgHeadingAccent.MarginTop;
                if (!_MarginTop.HasValue)
                    return default(double);
                return _MarginTop.Value;

            }

            set
            {

                if (_MarginTop != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MarginTop = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double? _MarginLeft;
        /// <MetaDataID>{ec51254a-5b4f-48b6-b192-cf5307a75345}</MetaDataID>
        [PersistentMember(nameof(_MarginLeft))]
        [BackwardCompatibilityID("+6")]
        public double MarginLeft
        {
            get
            {
                if (OrgHeadingAccent != null && !_MarginLeft.HasValue)
                    return OrgHeadingAccent.MarginLeft;
                if (!_MarginLeft.HasValue)
                    return default(double);
                return _MarginLeft.Value;
            }
            set
            {

                if (_MarginLeft != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MarginLeft = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool? _DoubleImage;
        /// <MetaDataID>{2ce5e375-7bf1-4dc3-aa0c-ef220746a08a}</MetaDataID>
        [PersistentMember(nameof(_DoubleImage))]
        [BackwardCompatibilityID("+5")]
        public bool DoubleImage
        {
            get
            {
                if (OrgHeadingAccent != null && !_DoubleImage.HasValue)
                    return OrgHeadingAccent.DoubleImage;
                if (!_DoubleImage.HasValue)
                    return default(bool);
                return _DoubleImage.Value;
            }

            set
            {

                if (_DoubleImage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DoubleImage = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool? _FullRowImage;

        /// <MetaDataID>{b9276ce7-43a3-4b61-94a8-282ff6a79d38}</MetaDataID>
        [PersistentMember(nameof(_FullRowImage))]
        [BackwardCompatibilityID("+4")]
        public bool FullRowImage
        {
            get
            {
                if (OrgHeadingAccent != null && !_FullRowImage.HasValue)
                    return OrgHeadingAccent.FullRowImage;
                if (!_FullRowImage.HasValue)
                    return default(bool);
                return _FullRowImage.Value;
            }

            set
            {

                if (_FullRowImage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FullRowImage = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool? _TextBackgroundImage;

        /// <MetaDataID>{ebb37566-466e-4ef1-a6f6-c957275408e2}</MetaDataID>
        [PersistentMember(nameof(_TextBackgroundImage))]
        [BackwardCompatibilityID("+3")]
        public bool TextBackgroundImage
        {
            get
            {
                if (OrgHeadingAccent != null && !_TextBackgroundImage.HasValue)
                    return OrgHeadingAccent.TextBackgroundImage;
                if (!_TextBackgroundImage.HasValue)
                    return default(bool);
                return _TextBackgroundImage.Value;
              
            }

            set
            {

                if (_TextBackgroundImage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TextBackgroundImage = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _OverlineImage;
        /// <MetaDataID>{551ed363-5390-4120-bedc-770e8e3ad2b9}</MetaDataID>
        [PersistentMember(nameof(_OverlineImage))]
        [BackwardCompatibilityID("+2")]
        public bool OverlineImage
        {
            get
            {

                if (OrgHeadingAccent != null && !_OverlineImage.HasValue)
                    return OrgHeadingAccent.OverlineImage;
                if (!_OverlineImage.HasValue)
                    return default(bool);
                return _OverlineImage.Value;
            }

            set
            {
                if (_OverlineImage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OverlineImage = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _UnderlineImage;
        /// <MetaDataID>{c2f0dba2-3daa-4750-a03e-665e253931a7}</MetaDataID>
        [PersistentMember(nameof(_UnderlineImage))]
        [BackwardCompatibilityID("+1")]
        public bool UnderlineImage
        {
            get
            {
                if (OrgHeadingAccent != null && !_UnderlineImage.HasValue)
                    return OrgHeadingAccent.UnderlineImage;
                if (!_UnderlineImage.HasValue)
                    return default(bool);
                return _UnderlineImage.Value;
            }

            set
            {
                if (_UnderlineImage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UnderlineImage = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double? _Height;
        /// <MetaDataID>{4c6b18dd-48db-4feb-abbb-1553ccb35a88}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+13")]
        public double Height
        {
            get
            {
                if (OrgHeadingAccent != null && !_Height.HasValue)
                    return OrgHeadingAccent.Height;
                if (!_UnderlineImage.HasValue)
                    return default(double);
                return _Height.Value;
            }

            set
            {

                if (_Height != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Height = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{19c3696a-d012-4778-ab08-f9bed2b2005f}</MetaDataID>
        string _SelectionAccentImageUri;
        /// <MetaDataID>{c348f1ee-71ba-4b8d-925b-c0d89edfc2fd}</MetaDataID>
        private IAccent OrgHeadingAccent;

        /// <MetaDataID>{ab725dd4-03f9-4de5-a753-11a63cda23fc}</MetaDataID>
        public Accent(IAccent accent)
        {
            OrgHeadingAccent = accent;
            // _AccentImages = null;
        }
        /// <MetaDataID>{be8e0c04-09bb-41c8-bb87-18595586ffb7}</MetaDataID>
        public Accent()
        {

        }

        /// <MetaDataID>{070d1a7e-78b2-421c-9ab8-fe2fbfd8b8d8}</MetaDataID>
        [PersistentMember(nameof(_SelectionAccentImageUri))]
        [BackwardCompatibilityID("+15")]
        public string SelectionAccentImageUri
        {
            get
            {
                return _SelectionAccentImageUri;
            }

            set
            {
                if (_SelectionAccentImageUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SelectionAccentImageUri = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}