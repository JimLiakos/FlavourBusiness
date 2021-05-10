using System;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{fc2a4bdb-6cd3-4a42-a773-26dfd756dd76}</MetaDataID>
    [BackwardCompatibilityID("{fc2a4bdb-6cd3-4a42-a773-26dfd756dd76}")]
    [Persistent()]
    public class PageImage : IPageImage, ITransactionNotification
    {
        /// <exclude>Excluded</exclude>
        double _Opacity=1;
        /// <MetaDataID>{63fa99b7-6fa6-4796-92cc-110b63791c02}</MetaDataID>
        [PersistentMember(nameof(_Opacity))]
        [BackwardCompatibilityID("+14")]
        public double Opacity
        {
            get
            {
                return _Opacity;
            }

            set
            {

                if (_Opacity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Opacity = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Opacity));
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool _Mirror;
        /// <MetaDataID>{05a7fdbf-d0fc-474f-a281-3bd5d040d369}</MetaDataID>
        [PersistentMember(nameof(_Mirror))]
        [BackwardCompatibilityID("+12")]
        public bool Mirror
        {
            get
            {
                return _Mirror; 
            }
            set
            {

                if (_Mirror != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Mirror = value;
                        stateTransition.Consistent = true;
                    }

                    ObjectChangeState?.Invoke(this, nameof(Mirror));
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool _Flip;

        /// <MetaDataID>{809a64e4-7b7a-45ed-922f-32f6c3081821}</MetaDataID>
        [PersistentMember(nameof(_Flip))]
        [BackwardCompatibilityID("+13")]
        public bool Flip
        {
            get
            {
                return _Flip;
            }

            set
            {

                if (_Flip != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Flip = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Flip));
                }

            }
        }



      
      

        public event ObjectChangeStateHandle ObjectChangeState;
        /// <exclude>Excluded</exclude>
        string _Color;
        /// <MetaDataID>{6fb532f9-f3cf-4eac-880f-b19ca6380324}</MetaDataID>
        [PersistentMember(nameof(_Color))]
        [BackwardCompatibilityID("+10")]
        public string Color
        {
            get
            {
                return _Color;
            }

            set
            {
                if (_Color != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Color = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Color));
                }
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{6bba3bd1-b8df-4d4b-9e84-dd062be618ff}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Name))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+9")]
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

        /// <MetaDataID>{f5e79701-c55c-4013-b378-811b212ffcaa}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+8")]
        public string LandscapeUri
        {
            get
            {
                if (_LandscapeImage != null)
                    return _LandscapeImage.Uri;
                return default(string);
            }
        }

        /// <MetaDataID>{804de58e-afa4-4888-91f0-afda116765ab}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+4")]
        public string PortraitUri
        {
            get
            {
                if (_PortraitImage != null)
                    return _PortraitImage.Uri;
                return default(string);

            }
        }


        ///// <exclude>Excluded</exclude>
        //string _Name;


        //[PersistentMember("_Name")]
        //[BackwardCompatibilityID("+1")]
        //public string Name
        //{
        //    get
        //    {
        //        return _Name;
        //    }
        //    set
        //    {

        //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //        {
        //            _Name = value;
        //            stateTransition.Consistent = true;
        //        }

        //    }
        //}



        /// <exclude>Excluded</exclude>
        double _LandscapeHeight;

        /// <MetaDataID>{42fee83b-17ee-479c-8af5-9765432852f4}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_LandscapeHeight))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+7")]
        public double LandscapeHeight
        {
            get
            {
                return _LandscapeHeight;
            }
            set
            {

                if (_LandscapeHeight != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LandscapeHeight = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        double _LandscapeWidth;

        /// <MetaDataID>{d20acd12-ded2-4754-8e56-7c5f38fb6d55}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_LandscapeWidth))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+6")]
        public double LandscapeWidth
        {
            get
            {
                return _LandscapeWidth;
            }

            set
            {

                if (_LandscapeWidth != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LandscapeWidth = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        Resource _LandscapeImage;

        /// <MetaDataID>{63d3f87b-e253-4e65-871f-62a77ecec9c8}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_LandscapeImage))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+5")]
        public MenuPresentationModel.MenuStyles.Resource LandscapeImage
        {
            get
            {
                return _LandscapeImage;
            }

            set
            {

                if (_LandscapeImage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LandscapeImage = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        /// <exclude>Excluded</exclude>
        double _PortraitHeight;
        /// <MetaDataID>{fd243e15-38b3-4a85-ad83-f6ea85e0e50d}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_PortraitHeight))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+3")]
        public double PortraitHeight
        {
            get
            {
                return _PortraitHeight;
            }

            set
            {

                if (_PortraitHeight != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PortraitHeight = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _PortraitWidth;
        /// <MetaDataID>{ff83071c-149a-4b4f-9b0a-39ade91bb9f8}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_PortraitWidth))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public double PortraitWidth
        {
            get
            {
                return _PortraitWidth;
            }

            set
            {

                if (_PortraitWidth != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PortraitWidth = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{3f18219c-e08e-4a4b-ab92-716e69bc9d53}</MetaDataID>
        public PageImage()
        {
        }
        /// <exclude>Excluded</exclude>
        Resource _PortraitImage;

        /// <MetaDataID>{53730378-2c14-4755-a54d-002e0feb28de}</MetaDataID>
        public PageImage(PageImage pageImage)
        {
            _LandscapeHeight = pageImage.LandscapeHeight;
            _LandscapeImage = pageImage.LandscapeImage;
            _LandscapeWidth = pageImage.LandscapeWidth;
            _PortraitHeight = pageImage.PortraitHeight;
            _PortraitImage = pageImage.PortraitImage;
            _PortraitWidth = pageImage.PortraitWidth;
            _Name = pageImage.Name;
            _Flip = pageImage.Flip;
            _Mirror = pageImage.Mirror;
        }

        /// <MetaDataID>{69b20774-eb94-42fe-8dce-a9a5ebf164cb}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_PortraitImage))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        public MenuPresentationModel.MenuStyles.Resource PortraitImage
        {
            get
            {
                return _PortraitImage;
            }

            set
            {

                if (_PortraitImage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PortraitImage = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{735db423-3f63-415a-98cb-2399ca94ac29}</MetaDataID>
        public bool IsVectorImage
        {
            get
            {
                if (System.IO.File.Exists(PortraitUri))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(PortraitUri);

                    return fileInfo.Extension.ToLower() == ".svg"; ;
                }
                return false;
            }
        }

        /// <MetaDataID>{deba015d-23a4-4776-a172-2329068221bc}</MetaDataID>
        public void OnTransactionCompletted(Transaction transaction)
        {
            ObjectChangeState?.Invoke(this, null);
        }
    }
}