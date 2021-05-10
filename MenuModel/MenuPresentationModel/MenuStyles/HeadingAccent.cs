using System;
using Newtonsoft.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{7f8f9237-334f-4291-a910-99049f48d672}</MetaDataID>
    [BackwardCompatibilityID("{7f8f9237-334f-4291-a910-99049f48d672}")]
    [Persistent()]
    public class HeadingAccent : MarshalByRefObject,  IHeadingAccent, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{ae8f0451-b74b-436b-a794-96392bb8ee12}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+12")]
        private string AccentImagesJson;


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IAccentImage> _AccentImages = new OOAdvantech.Collections.Generic.Set<IAccentImage>();
        /// <MetaDataID>{37d13ede-2e9a-49c6-81b2-a3bb5d8e90c5}</MetaDataID>
        [PersistentMember(nameof(_AccentImages))]
        [BackwardCompatibilityID("+14")]
        public System.Collections.Generic.List<IAccentImage> AccentImages
        {
            get
            {
                return _AccentImages.AsReadOnly().ToList();
            }
        }

        /// <MetaDataID>{4855a0c7-e40d-40d3-89d4-85fa41f44d0b}</MetaDataID>
        public void AddAccentImage(IAccentImage accentImage)
        {
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
        public void DeleteAccentImage(IAccentImage accentImage)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _AccentImages.Remove(accentImage);

                AccentImagesJson = JsonConvert.SerializeObject(_AccentImages, Formatting.None, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{d9d4c26f-7a0e-4bce-bd39-bcbbda31bec0}</MetaDataID>
        public void OnCommitObjectState()
        {

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

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{bfed14ac-f7d1-4265-afdf-53ba232c429f}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+11")]
        public string Name
        {
            get
            {
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
        double _MarginRight;

        /// <MetaDataID>{502cc2a1-9091-4b47-a640-598e60fbe353}</MetaDataID>
        [PersistentMember(nameof(_MarginRight))]
        [BackwardCompatibilityID("+9")]
        public double MarginRight
        {
            get
            {
                return _MarginRight;
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
        double _MarginBottom;
        /// <MetaDataID>{6c1e3486-bbe1-4f00-bc5a-de89c0dbbb87}</MetaDataID>
        [PersistentMember(nameof(_MarginBottom))]
        [BackwardCompatibilityID("+8")]
        public double MarginBottom
        {
            get
            {
                return _MarginBottom;
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
        double _MarginTop;
        /// <MetaDataID>{979cb7d5-f25a-4e4e-9fe2-5a628f2c126f}</MetaDataID>
        [PersistentMember(nameof(_MarginTop))]
        [BackwardCompatibilityID("+7")]
        public double MarginTop
        {
            get
            {
                return _MarginTop;
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
        double _MarginLeft;
        /// <MetaDataID>{ec51254a-5b4f-48b6-b192-cf5307a75345}</MetaDataID>
        [PersistentMember(nameof(_MarginLeft))]
        [BackwardCompatibilityID("+6")]
        public double MarginLeft
        {
            get
            {
                return _MarginLeft;
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
        bool _DoubleImage;
        /// <MetaDataID>{2ce5e375-7bf1-4dc3-aa0c-ef220746a08a}</MetaDataID>
        [PersistentMember(nameof(_DoubleImage))]
        [BackwardCompatibilityID("+5")]
        public bool DoubleImage
        {
            get
            {
                return _DoubleImage;
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
        bool _FullRowImage;

        /// <MetaDataID>{b9276ce7-43a3-4b61-94a8-282ff6a79d38}</MetaDataID>
        [PersistentMember(nameof(_FullRowImage))]
        [BackwardCompatibilityID("+4")]
        public bool FullRowImage
        {
            get
            {
                return _FullRowImage;
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
        bool _TextBackgroundImage;

        /// <MetaDataID>{ebb37566-466e-4ef1-a6f6-c957275408e2}</MetaDataID>
        [PersistentMember(nameof(_TextBackgroundImage))]
        [BackwardCompatibilityID("+3")]
        public bool TextBackgroundImage
        {
            get
            {
                return _TextBackgroundImage;
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
        bool _OverlineImage;
        /// <MetaDataID>{551ed363-5390-4120-bedc-770e8e3ad2b9}</MetaDataID>
        [PersistentMember(nameof(_OverlineImage))]
        [BackwardCompatibilityID("+2")]
        public bool OverlineImage
        {
            get
            {
               return _OverlineImage;
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
        bool _UnderlineImage;
        /// <MetaDataID>{c2f0dba2-3daa-4750-a03e-665e253931a7}</MetaDataID>
        [PersistentMember(nameof(_UnderlineImage))]
        [BackwardCompatibilityID("+1")]
        public bool UnderlineImage
        {
            get
            {
                return _UnderlineImage;
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
        double _Height;
        /// <MetaDataID>{4c6b18dd-48db-4feb-abbb-1553ccb35a88}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+13")]
        public double Height
        {
            get
            {
                return _Height;
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