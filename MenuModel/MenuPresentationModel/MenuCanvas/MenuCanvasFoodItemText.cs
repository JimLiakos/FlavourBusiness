using System;
using System.Windows;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{bedb6326-e271-4829-9dc7-038c08d36beb}</MetaDataID>
    [BackwardCompatibilityID("{bedb6326-e271-4829-9dc7-038c08d36beb}")]
    [Persistent()]
    public class MenuCanvasFoodItemText  : MarshalByRefObject, IMenuCanvasFoodItemText
    {
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{334b3455-1e59-4f28-bc4f-11a1251cc6c9}</MetaDataID>
        protected MenuCanvasFoodItemText()
        {


        }
        /// <MetaDataID>{4a577a92-5c3b-4359-ab9e-77d4bebf910e}</MetaDataID>
        public MenuCanvasFoodItemText(string text, double xPos, double yPos, MenuStyles.FontData font)
        {
            _Description = text;
            _XPos = xPos;
            _YPos = yPos;
            _Font = font;
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{6f0c5d20-d1a7-430e-82fd-88097a16aad0}</MetaDataID>
        [PersistentMember("_Description")]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        FontData _Font;
        /// <MetaDataID>{90d2341a-10cd-4216-8348-50cf6b55da32}</MetaDataID>
        [PersistentMember("_Font")]
        [BackwardCompatibilityID("+2")]
        public MenuPresentationModel.MenuStyles.FontData Font
        {
            set
            {
                if (_Font != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Font = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
            get
            {
                return _Font;
            }
        }
        /// <exclude>Excluded</exclude>
        double _Height;
        /// <MetaDataID>{c47a3d8d-bef7-42da-81ae-b8a70e6a5d8b}</MetaDataID>
        [PersistentMember("_Height")]
        [BackwardCompatibilityID("+3")]
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
        OOAdvantech.Member<IMenuPageCanvas> _Page = new OOAdvantech.Member<IMenuPageCanvas>();
        /// <MetaDataID>{60899909-72c3-4745-ae19-ced0de750ea3}</MetaDataID>
                [BackwardCompatibilityID("+4")]
        public IMenuPageCanvas Page
        {

            get
            {
                return _Page.Value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Width;
        /// <MetaDataID>{b0e71aea-0b0d-4b25-858f-4c1e5e4a1354}</MetaDataID>
        [PersistentMember("_Width")]
        [BackwardCompatibilityID("+5")]
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

        /// <exclude>Excluded</exclude>
        double _XPos;
        /// <MetaDataID>{8ed8f09e-f82f-4e2b-99f2-a3444fe59440}</MetaDataID>
        [PersistentMember("_XPos")]
        [BackwardCompatibilityID("+6")]
        public double XPos
        {
            get
            {
                return _XPos;
            }

            set
            {

                if (_XPos != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _XPos = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        double _YPos;
        /// <MetaDataID>{3d0f27a6-eb1b-4dc5-b6e5-888101c3e144}</MetaDataID>
        [PersistentMember("_YPos")]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+7")]
        public double YPos
        {
            get
            {
                return _YPos;
            }

            set
            {

                if (_YPos != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _YPos = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
      OOAdvantech.Member<  IMenuCanvasFoodItem> _FoodItem=new OOAdvantech.Member<IMenuCanvasFoodItem>();
        /// <MetaDataID>{73c1fad3-db0e-4ee1-8d1d-c13eeb792e66}</MetaDataID>
        [ImplementationMember(nameof(_FoodItem))]
        public IMenuCanvasFoodItem FoodItem
        {
            get
            {
                return _FoodItem.Value;
            }
        }

        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
        }

        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }
    }
}