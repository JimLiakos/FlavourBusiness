using System;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{fc2a4bdb-6cd3-4a42-a773-26dfd756dd76}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{fc2a4bdb-6cd3-4a42-a773-26dfd756dd76}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public class PageImage : IPageImage
    {
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

        public PageImage()
        {
        }
        /// <exclude>Excluded</exclude>
        Resource _PortraitImage;
       
        public PageImage(PageImage border)
        {
            _LandscapeHeight = border.LandscapeHeight;
            _LandscapeImage = border.LandscapeImage;
            _LandscapeWidth = border.LandscapeWidth;
            _PortraitHeight = border.PortraitHeight;
            _PortraitImage = border.PortraitImage;
            _PortraitWidth = border.PortraitWidth;
            _Name = border.Name;
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


    }
}