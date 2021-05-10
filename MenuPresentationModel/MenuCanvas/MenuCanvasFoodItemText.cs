using System;
using System.Windows;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using UIBaseEx;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{bedb6326-e271-4829-9dc7-038c08d36beb}</MetaDataID>
    [BackwardCompatibilityID("{bedb6326-e271-4829-9dc7-038c08d36beb}")]
    [Persistent()]
    public class MenuCanvasFoodItemText : MarshalByRefObject, IMenuCanvasFoodItemText
    {


        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{932c27fc-58b5-49bd-a782-21a3cbb337a9}</MetaDataID>
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{ab3e6b02-678a-46a9-8752-b211850f9ed0}</MetaDataID>
        public void ResetSize()
        {
            BaseLine = Font.GetTextBaseLine(Description);
        }
        /// <MetaDataID>{079295d2-dfdd-47d8-a7d9-480a0396a243}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }
        /// <exclude>Excluded</exclude>
        double _BaseLine;
        /// <MetaDataID>{c6ed4521-4b72-460d-9e84-a82fb4023e52}</MetaDataID>
        [PersistentMember(nameof(_BaseLine))]
        [BackwardCompatibilityID("+8")]
        public double BaseLine
        {
            get
            {
                return _BaseLine;
            }
            set
            {
                if (_BaseLine != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BaseLine = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{334b3455-1e59-4f28-bc4f-11a1251cc6c9}</MetaDataID>
        protected MenuCanvasFoodItemText()
        {
        }
        /// <MetaDataID>{4a577a92-5c3b-4359-ab9e-77d4bebf910e}</MetaDataID>
        public MenuCanvasFoodItemText(string text, double xPos, double yPos, FontData font)
        {
            _Description = text;
            _XPos = xPos;
            _YPos = yPos;
            _Font = font;
            _BaseLine = Font.GetTextBaseLine(_Description);
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
                BaseLine = Font.GetTextBaseLine(_Description);
            }
        }
        /// <exclude>Excluded</exclude>
        FontData _Font;
        /// <MetaDataID>{90d2341a-10cd-4216-8348-50cf6b55da32}</MetaDataID>
        [PersistentMember("_Font")]
        [BackwardCompatibilityID("+2")]
        public FontData Font
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
        double _FontSpacingCorrection;
        /// <MetaDataID>{79e2f923-0f78-4c67-b48e-acc6ef44837b}</MetaDataID>
        [PersistentMember(nameof(_FontSpacingCorrection))]
        [BackwardCompatibilityID("+9")]
        public double FontSpacingCorrection
        {
            get => _FontSpacingCorrection;
            set
            {
                if (_FontSpacingCorrection != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FontSpacingCorrection = value;
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
                BaseLine = Font.GetTextBaseLine(_Description);

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuCanvasFoodItem> _FoodItem = new OOAdvantech.Member<IMenuCanvasFoodItem>();
        /// <MetaDataID>{73c1fad3-db0e-4ee1-8d1d-c13eeb792e66}</MetaDataID>
        [ImplementationMember(nameof(_FoodItem))]
        public IMenuCanvasFoodItem FoodItem
        {
            get
            {
                return _FoodItem.Value;
            }
        }

        /// <MetaDataID>{6e63cf4b-138d-4a7e-95e2-9ef150802a1e}</MetaDataID>
        public bool UnTranslated { get; internal set; }


        /// <MetaDataID>{60737161-4b2f-4791-a278-c7d3c1f4d49f}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
            BaseLine = Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{9bed5ed3-c6c2-4d50-8151-94662b3375dc}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }

        internal void AdjustDescriptionForWidth(double width)
        {
            string description = Description;
            while (description != null && description.Length > 0)
            {
                if (Font.MeasureText(description).Width > width)
                    description = description.Substring(0, description.Length - 1);
                else
                    break;
            }
            _Width = Font.MeasureText(description).Width;
            Description = description;
        }
    }
}