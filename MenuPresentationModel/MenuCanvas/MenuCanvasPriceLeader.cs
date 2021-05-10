using System;
using System.Windows;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using UIBaseEx;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{897d5242-0fd1-4d86-933e-9a365ba21f6e}</MetaDataID>
    [BackwardCompatibilityID("{897d5242-0fd1-4d86-933e-9a365ba21f6e}")]
    [Persistent()]
    public class MenuCanvasPriceLeader : MarshalByRefObject, IMenuCanvasPriceLeader
    {

        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{1fd011f2-6827-413e-8cc8-d19b2365de62}</MetaDataID>
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }
        /// <MetaDataID>{49b9f3c4-e241-4f3c-b7b4-e27dae826f12}</MetaDataID>
        public void ResetSize()
        {
            Size priceLeaderSize = Font.MeasureText(Description);
            Height = priceLeaderSize.Height;
            Width = priceLeaderSize.Width;
            BaseLine = Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{ca503450-7b17-44be-a75f-cec0cc5e617d}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }

        /// <exclude>Excluded</exclude>
        double _BaseLine;
        /// <MetaDataID>{e4609ca3-910c-486c-8cac-8187b4ee1d6b}</MetaDataID>
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



        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{36a4b6ba-fc2b-4ec6-8e77-f82198a0a0b7}</MetaDataID>
        public MenuCanvasPriceLeader()
        {

        }
        /// <MetaDataID>{cb64b65a-1a60-4363-8de3-5ad0b8a0ff09}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
            BaseLine = Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{21adee9e-db55-4959-bd57-ee7021934140}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <MetaDataID>{37e7945f-6814-491f-a9b7-e55c14b9d0d9}</MetaDataID>
        public bool Visisble { get; set; }

        /// <exclude>Excluded</exclude>
        FontData? _Font;
        /// <MetaDataID>{4a081a73-efc5-464b-9c1c-fbbf1ec8097b}</MetaDataID>
        [PersistentMember(nameof(_Font))]
        [BackwardCompatibilityID("+6")]
        public FontData Font
        {
            get
            {
                if (!_Font.HasValue)
                    return Style.Font;
                return _Font.Value;
            }
            set
            {
                _Font = value;
            }
        }

        /// <MetaDataID>{15ed3ace-cbc4-402d-ba71-b1bdd7ab72da}</MetaDataID>
        MenuStyles.IPriceStyle Style
        {
            get
            {
                return Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
            }
        }
        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{3a0a3589-3093-4511-b2e3-31e925a73e0f}</MetaDataID>
        [PersistentMember(nameof(_Description))]
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
                        _Width = (double?)null;
                        _Height = (double?)null;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasFoodItem> _FoodItem = new MultilingualMember<IMenuCanvasFoodItem>();
        /// <MetaDataID>{03ea644a-a1fa-4358-b32e-1ce4c594b4ef}</MetaDataID>

        [PersistentMember(nameof(_FoodItem))]
        [BackwardCompatibilityID("+2")]
        public MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItem FoodItem
        {
            get
            {
                return _FoodItem.Value;
            }
        }


        /// <MetaDataID>{aeb5c72a-bf0e-4015-8405-a782d7ce2b47}</MetaDataID>
        public MenuPresentationModel.MenuCanvas.IMenuPageCanvas Page
        {
            get
            {
                return FoodItem.Page;
            }
        }
        /// <exclude>Excluded</exclude>
        double? _Height;
        /// <MetaDataID>{1083508a-6059-468e-b10b-b5512de313fc}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+7")]
        public double Height
        {
            get
            {
                if (!_Height.HasValue)
                    _Height= Font.MeasureText(Description).Height;

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
        double? _Width;
        /// <MetaDataID>{1ad10594-69ef-4e82-a508-48e0e9c903c0}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+5")]
        public double Width
        {
            get
            {
                if (!_Width.HasValue)
                {
                    var size = Font.MeasureText(Description);
                    _Width = size.Width;
                    _Height = size.Height;
                }
                return _Width.Value;
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
        /// <MetaDataID>{05d3bdd1-b169-4cb2-b720-b62e427b721c}</MetaDataID>
        [PersistentMember(nameof(_XPos))]
        [BackwardCompatibilityID("+3")]
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
        double _YPos ;
        /// <MetaDataID>{c732bc82-1c1d-4f91-9547-9942705b2645}</MetaDataID>
        [PersistentMember(nameof(_YPos))]
        [BackwardCompatibilityID("+4")]
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
    }
}