using System;
using System.Collections.Generic;
using OOAdvantech.Json;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{66712fca-7577-4d6b-86fc-0bc928b79fe7}</MetaDataID>
    [BackwardCompatibilityID("{66712fca-7577-4d6b-86fc-0bc928b79fe7}")]
    [Persistent()]
    public class HeadingStyle : MarshalByRefObject, IHeadingStyle, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {



        /// <MetaDataID>{93678d50-aabc-480d-bc6f-6742d94a0599}</MetaDataID>
        public HeadingStyle()
        {

        }
        /// <MetaDataID>{248c54ec-b93a-4585-ab2f-84a54725d5bc}</MetaDataID>
        public bool IsDerivedStyle
        {
            get
            {
                if (OrgHeadingStyle != null)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{82ff572b-18e5-46b0-b347-80ce2b916536}</MetaDataID>
        HeadingStyle OrgHeadingStyle;
        /// <MetaDataID>{342701c5-70b3-401d-bc6e-170d471b005b}</MetaDataID>
        HeadingStyle(HeadingStyle orgHeadingStyle)
        {
            OrgHeadingStyle = orgHeadingStyle;
            _Name = OrgHeadingStyle.Name;

            if (OrgHeadingStyle != null && OrgHeadingStyle.Accent != null)
                _Accent = new Accent(OrgHeadingStyle.Accent);
            if (_Accent != null)
                _Accent.ObjectChangeState += Accent_ObjectChangeState;
        }


        /// <MetaDataID>{d3cbd549-ca03-4817-8f5f-5ae5e7153868}</MetaDataID>
        public void OnCommitObjectState()
        {
            if (_Accent != null && OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_Accent) == null)
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(_Accent);
        }

        /// <MetaDataID>{a870d3c3-b9a2-4a96-a3b4-8060a8f1f050}</MetaDataID>
        public void BeforeCommitObjectState()
        {
        }
        /// <MetaDataID>{47e975af-2426-47c2-b919-0ad9b7ce8d17}</MetaDataID>
        public void OnActivate()
        {

            if (_Accent != null)
                _Accent.ObjectChangeState += Accent_ObjectChangeState;
        }

        /// <MetaDataID>{fd988001-ba55-4a3b-86cb-63277ad62ddc}</MetaDataID>
        private void Accent_ObjectChangeState(object _object, string member)
        {
            //if (member == nameof(MenuPresentationModel.MenuStyles.IHeadingAccent.AccentColor))
            //{

            //}
            // ObjectChangeState?.Invoke(this, nameof(Accent));
        }

        /// <MetaDataID>{3cab83e2-4290-447c-89b0-00defd12f3a9}</MetaDataID>
        public void OnDeleting()
        {
        }
        /// <MetaDataID>{87babe64-9bc7-43e7-bda0-d3606010627a}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <MetaDataID>{ee60ce04-1b8a-4814-b723-2d6b8a190271}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <exclude>Excluded</exclude>
        IStyleSheet _StyleSheet;
        /// <MetaDataID>{a30d84c9-ec68-4113-b814-9caae4fe537a}</MetaDataID>
        [PersistentMember("_StyleSheet")]
        [BackwardCompatibilityID("+10")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [OOAdvantech.Json.JsonIgnore]
        public MenuPresentationModel.MenuStyles.IStyleSheet StyleSheet
        {
            get
            {
                return _StyleSheet;
            }
        }



        /// <MetaDataID>{40dc9593-6509-43cf-9141-f38e1b8d1b0a}</MetaDataID>
        public void ChangeOrgStyle(IStyleRule style)
        {
            if (OrgHeadingStyle != style)
            {
                OrgHeadingStyle = style as HeadingStyle;
                if (OrgHeadingStyle != null)
                {
                    UseDefaultValues();
                    if (_Accent != null)
                    {
                        if (OrgHeadingStyle.Accent != null)
                            _Accent.ChangeOrgStyle(OrgHeadingStyle.Accent);
                        else
                            _Accent = null;
                    }
                    else if (OrgHeadingStyle.Accent != null)
                    {
                        _Accent = new Accent(OrgHeadingStyle.Accent);

                        //if (_Accent != null && ObjectStorage.GetStorageOfObject(this) != null && ObjectStorage.GetStorageOfObject(_Accent) == null)
                        //    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(_Accent);
                    }
                }
                else
                    _Accent = null;
            }





        }


        /// <MetaDataID>{7302575e-6fc0-4293-9e68-145a7d25cc27}</MetaDataID>
        public void UseDefaultValues()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this,TransactionOption.Supported))
            {
                _OverlineImage = null;
                _UnderlineImage = null;
                _AfterSpacing = null;
                _Alignment = null;
                _BeforeSpacing = null;
                _Accent = null;
                stateTransition.Consistent = true;
            }


        }

        /// <MetaDataID>{f094aad1-b2dd-4768-8a78-5b6136fc898d}</MetaDataID>
        public virtual IStyleRule GetDerivedStyle()
        {
            return new HeadingStyle(this);
        }

        /// <exclude>Excluded</exclude>
        bool? _OverlineImage;
        /// <MetaDataID>{a6b3cf72-e08b-45c7-b76f-ce67e12a522d}</MetaDataID>
        [PersistentMember(nameof(_OverlineImage))]
        [BackwardCompatibilityID("+8")]
        public bool OverlineImage
        {
            get
            {

                if (OrgHeadingStyle != null && !_OverlineImage.HasValue)
                    return OrgHeadingStyle.OverlineImage;
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

        /// <MetaDataID>{add029b1-7413-409e-a528-445feb35384f}</MetaDataID>
        [PersistentMember(nameof(_UnderlineImage))]
        [BackwardCompatibilityID("+7")]
        public bool UnderlineImage
        {
            get
            {
                if (OrgHeadingStyle != null && !_UnderlineImage.HasValue)
                    return OrgHeadingStyle.UnderlineImage;
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
        OOAdvantech.MultilingualMember<FontData> _Font = new MultilingualMember<FontData>();


        /// <MetaDataID>{9e509821-e5cf-4939-bb4d-15d69d097b5b}</MetaDataID>
        [PersistentMember(nameof(_Font))]
        [BackwardCompatibilityID("+5")]
        public FontData Font
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgHeadingStyle != null && !_Font.HasValue)
                        return OrgHeadingStyle.Font;
                    if (!_Font.HasValue)
                        return default(FontData);
                    return _Font.Value;
                }
            }
            set
            {
                if (_Font != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Font.Value = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Font));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double? _AfterSpacing;
        /// <MetaDataID>{406b7757-6819-4ccc-8e93-5df3dba966f8}</MetaDataID>
        [PersistentMember(nameof(_AfterSpacing))]
        [BackwardCompatibilityID("+2")]
        public double AfterSpacing
        {
            get
            {
                if (OrgHeadingStyle != null && !_AfterSpacing.HasValue)
                    return OrgHeadingStyle.AfterSpacing;
                if (!_AfterSpacing.HasValue)
                    return default(double);
                return _AfterSpacing.Value;
            }

            set
            {
                if (_AfterSpacing != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AfterSpacing = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        Alignment? _Alignment;
        /// <MetaDataID>{635a9e30-130f-435e-ae73-a6f9e3858804}</MetaDataID>
        [PersistentMember(nameof(_Alignment))]
        [BackwardCompatibilityID("+3")]
        public Alignment Alignment
        {
            get
            {
                if (OrgHeadingStyle != null && !_Alignment.HasValue)
                    return OrgHeadingStyle.Alignment;
                if (!_Alignment.HasValue)
                    return default(Alignment);
                return _Alignment.Value;
            }

            set
            {
                if (_Alignment != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Alignment = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        double? _BeforeSpacing;
        /// <MetaDataID>{a85673b9-3406-44e4-b891-c88a5f4f9901}</MetaDataID>
        [PersistentMember(nameof(_BeforeSpacing))]
        [BackwardCompatibilityID("+4")]
        public double BeforeSpacing
        {
            get
            {
                if (OrgHeadingStyle != null && !_BeforeSpacing.HasValue)
                    return OrgHeadingStyle.BeforeSpacing;
                if (!_BeforeSpacing.HasValue)
                    return default(double);
                return _BeforeSpacing.Value;

            }

            set
            {
                if (_BeforeSpacing != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BeforeSpacing = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{6f9cbc66-0af9-4a19-a28f-140b1da416f6}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                if (OrgHeadingStyle != null && _Name == null)
                    return OrgHeadingStyle.Name;
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

        /// <MetaDataID>{6fd86d0a-ffb1-4d18-8d27-f2f139b9bd2b}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+6")]
        string AccentImagesJson;

        /// <exclude>Excluded</exclude>
        List<Resource> _AccentImages = null;
        /// <MetaDataID>{37d13ede-2e9a-49c6-81b2-a3bb5d8e90c5}</MetaDataID>
        public System.Collections.Generic.List<MenuPresentationModel.MenuStyles.Resource> AccentImages
        {
            get
            {
                if (OrgHeadingStyle != null && _AccentImages == null)
                    return OrgHeadingStyle.AccentImages;
                if (_AccentImages == null)
                    _AccentImages = new List<Resource>();



                return _AccentImages;
            }
        }

        /// <exclude>Excluded</exclude>
        IAccent _Accent;

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{48afd4ff-867c-4ad2-850e-27206eca22fc}</MetaDataID>
        [PersistentMember(nameof(_Accent))]
        [AssociationEndBehavior(PersistencyFlag.AllowTransient)]
        [BackwardCompatibilityID("+9")]
        public IAccent Accent
        {
            get
            {
                if (OrgHeadingStyle != null && _Accent == null)
                    return OrgHeadingStyle.Accent;
                return _Accent;
            }

            set
            {

                if (_Accent != value)
                {
                    if (_Accent != null)
                        _Accent.ObjectChangeState -= Accent_ObjectChangeState;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Accent = value;
                        stateTransition.Consistent = true;
                    }
                    if (_Accent != null)
                        _Accent.ObjectChangeState += Accent_ObjectChangeState;
                    ObjectChangeState?.Invoke(this, nameof(Accent));
                }

            }
        }

        ///// <MetaDataID>{e75426db-d67d-4d32-b8b1-eda0e4098dfb}</MetaDataID>
        //public string Color
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }

        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        /// <MetaDataID>{4855a0c7-e40d-40d3-89d4-85fa41f44d0b}</MetaDataID>
        public void AddAccentImage(Resource accentImage)
        {
            if (_AccentImages == null)
                _AccentImages = new List<Resource>();

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
        public void DeleteAccentImage(Resource accentImage)
        {
            if (_AccentImages == null)
                return;

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _AccentImages.Remove(accentImage);

                AccentImagesJson = JsonConvert.SerializeObject(_AccentImages, Formatting.None, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{d220a711-2fbe-4fb4-a82b-cb4a0e0be1e8}</MetaDataID>
        public void RestFont()
        {
            if (OrgHeadingStyle != null)
            {
                //_Font = (FontData?)null;

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    if (_Font.HasValue)
                    {

                    }
                    _Font.ClearValue(); 
                    stateTransition.Consistent = true;
                }

            }
        }
    }

}