using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{eac8372c-0412-4991-ad40-35d6c4bd0bbc}</MetaDataID>
    [BackwardCompatibilityID("{eac8372c-0412-4991-ad40-35d6c4bd0bbc}")]
    [Persistent()]
    public class AccentImage : IAccentImage
    {


        public AccentImage(Resource image,double width,double height)
        {
            _Image = image;
            _Width = width;
            _Height = height;
        }



        protected AccentImage()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double _Height;
        /// <MetaDataID>{939ba076-7a00-4b04-a231-4fa497f616d2}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+1")]
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
        Resource _Image;

        /// <MetaDataID>{50952c81-d2fa-458c-86d4-212ed44eeb00}</MetaDataID>
        [PersistentMember(nameof(_Image))]
        [BackwardCompatibilityID("+2")]
        public Resource Image
        {
            get
            {
                return _Image;
            }

            set
            {

                if (_Image != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Image = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double _Width;
        /// <MetaDataID>{803f2a93-21ce-42df-9bbf-66c93fa7bb58}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+3")]
        public double Width
        {
            get
            {
                return _Width;
            }

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
    }
}