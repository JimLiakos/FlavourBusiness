using System;
using System.Windows;
using MenuModel;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using UIBaseEx;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{99e2a108-5f66-42ea-8f50-18122d051d5d}</MetaDataID>
    [BackwardCompatibilityID("{99e2a108-5f66-42ea-8f50-18122d051d5d}")]
    [Persistent()]
    public class MenuCanvasTextPriceHeading : MarshalByRefObject, IPriceHeading, IMenuCanvasItem
    {


        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{67bac3b4-f230-4508-9857-ab11edc363e5}</MetaDataID>
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }

        /// <MetaDataID>{09a88b29-e18f-40bb-a8a1-abfd5bb55b59}</MetaDataID>
        public void ResetSize()
        {
            BaseLine = Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{3d891853-69f9-4d35-b24d-942120bf1511}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }
        /// <exclude>Excluded</exclude>
        double _BaseLine;
        /// <MetaDataID>{e47f21f5-abfc-4df5-9f4b-5f7e66284ee7}</MetaDataID>
        [PersistentMember(nameof(_BaseLine))]
        [BackwardCompatibilityID("+11")]
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


        /// <exclude>Excluded</exclude>
        TransformOrigin? _TransformOrigin;
        /// <MetaDataID>{e2872073-7e8c-4b61-b660-0cff874f39f1}</MetaDataID>
        [PersistentMember(nameof(_TransformOrigin))]
        [BackwardCompatibilityID("+9")]
        public TransformOrigin TransformOrigin
        {
            get
            {
               
                if (_TransformOrigin.HasValue)
                    return _TransformOrigin.Value;
                else if (Style != null)
                    return Style.PriceHeadingTransformOrigin;
                else
                    return default(TransformOrigin);
            }
            set
            {
                if (_TransformOrigin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TransformOrigin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{6c87fdc8-5eda-44b2-8993-e692f7444174}</MetaDataID>
        public void UseStyleTransformOrigin()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _TransformOrigin = null;
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{ed82895e-407e-4aba-9d02-0f69f39486f6}</MetaDataID>
        public double PriceHeadingTextWitdh { get; set; }

        /// <exclude>Excluded</exclude>
        double? _Angle;

        /// <MetaDataID>{e0708e1f-623f-4d9f-82c6-68b48442fd72}</MetaDataID>
        [PersistentMember(nameof(_Angle))]
        [BackwardCompatibilityID("+8")]
        public double Angle
        {
            get
            {
                if (_Angle.HasValue)
                    return _Angle.Value;
                else if (Style != null)
                {
                    if (Style.PriceHeadingAngle != 0)
                        return Style.PriceHeadingAngle;
                    else
                        return 310;//55;//
                }
                else
                    return default(double);
            }
            set
            {
                if (_Angle != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Angle = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <MetaDataID>{19889833-b505-4380-b384-4396a87de549}</MetaDataID>
        public MenuCanvasTextPriceHeading()
        { 

        }
        /// <exclude>Excluded</exclude>
        double _Height;

        /// <MetaDataID>{f1faa6c8-c90c-4743-a601-c5a0a2d06bc2}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+2")]
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

         

        /// <MetaDataID>{7313bd17-522d-42a3-9d6a-8fbd1ea35d04}</MetaDataID>
        public MenuPresentationModel.MenuCanvas.IMenuPageCanvas Page
        {
            get
            {

                if (_PricesHeading != null)
                    return _PricesHeading.Page;
                else
                    return null;
            }
        }

        /// <MetaDataID>{b7b6234c-351b-4ca1-a913-06a25974c22e}</MetaDataID>
        public MenuStyles.IPriceStyle Style
        {
            get
            {
                MenuStyles.IStyleSheet styleSheet = null;
                if (Page != null)
                    styleSheet = Page.Style;
               else if (RestaurantMenu.ConntextStyleSheet != null)
                    styleSheet = RestaurantMenu.ConntextStyleSheet;
                if (styleSheet != null)
                    return styleSheet.Styles["price-options"] as MenuStyles.IPriceStyle;
                else
                    return null;
            }
        }



        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{d0694f26-700a-4efc-8059-a88d4a724e54}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                if (_Description == null && _Source != null)
                    _Description = _Source.Name;

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
        FontData? _Font;
        /// <MetaDataID>{6de240ae-4f7f-4e66-96d6-6ddce371f7b0}</MetaDataID>
        public FontData Font
        {
            get
            {
                if (Style.Layout == MenuStyles.PriceLayout.WithDescription || Style.Layout == MenuStyles.PriceLayout.FollowDescription)
                    return (Page.Style.Styles["menu-item"] as MenuStyles.IMenuItemStyle).DescriptionFont;
                else if (Style != null)
                    return Style.Font;
                else
                    return default(FontData);

            }
            set
            {
                _Font = value;
            }
        }

    

        /// <exclude>Excluded</exclude>
        ItemSelectorOption _Source;
        /// <MetaDataID>{d1570788-91cc-466c-8aee-32fe3f6beae0}</MetaDataID>
        public ItemSelectorOption Source
        {
            get
            {
                return _Source;
            }

            set
            {
                _Source = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Width;
        /// <MetaDataID>{e5a0c03a-e8a2-45a6-b3c5-3c50526b18c3}</MetaDataID>
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


        

        /// <exclude>Excluded</exclude>
        double _XPos;

        /// <MetaDataID>{09ba3000-b03f-4c37-b381-2f13f044f73c}</MetaDataID>
        [PersistentMember(nameof(_XPos))]
        [BackwardCompatibilityID("+4")]
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

        /// <MetaDataID>{2ee9cfe2-350d-438f-8d08-508e15ccab6e}</MetaDataID>
        [PersistentMember(nameof(_YPos))]
        [BackwardCompatibilityID("+5")]
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
        IItemMultiPriceHeading _PricesHeading;
        /// <MetaDataID>{cbe3937c-86a4-455c-903a-e8cf2aafe16b}</MetaDataID>
        [ImplementationMember(nameof(_PricesHeading))]
        public IItemMultiPriceHeading PricesHeading
        {
            get
            {
                return _PricesHeading;
            }
        }

        /// <exclude>Excluded</exclude>
        double _PriceHeadinTextXPos;
        /// <MetaDataID>{50357ef4-3aa8-4dec-a8ca-ca13fbe02c26}</MetaDataID>
        [PersistentMember(nameof(_PriceHeadinTextXPos))]
        [BackwardCompatibilityID("+10")]
        public double PriceHeadinTextXPos
        {
            get
            {
                return _PriceHeadinTextXPos;
            }
            set
            {

                if (_PriceHeadinTextXPos != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceHeadinTextXPos = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{e2375319-9896-4a3f-8175-8e9ed5d17fcd}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem menuCanvasItemLineText)
        {
            _YPos = menuCanvasItemLineText.YPos + menuCanvasItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
            BaseLine = Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{8192842d-b023-46ea-b901-682ea0448913}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{720a72bd-cef7-4317-b366-1f6bf08c68ed}</MetaDataID>
        public void ResetTransformOrigin()
        {
            _TransformOrigin = null;
        }

        /// <MetaDataID>{d50bbf6f-bfc2-488e-88f9-9b7fd5e8f213}</MetaDataID>
        public void ResetAngle()
        {
            _Angle = null;
        }
    }
}