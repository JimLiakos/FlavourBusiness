using System;
using System.Windows;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{897d5242-0fd1-4d86-933e-9a365ba21f6e}</MetaDataID>
    [BackwardCompatibilityID("{897d5242-0fd1-4d86-933e-9a365ba21f6e}")]
    [Persistent()]
    public class MenuCanvasPriceLeader : MarshalByRefObject, IMenuCanvasPriceLeader
    {

        public event ObjectChangeStateHandle ObjectChangeState;

        public MenuCanvasPriceLeader()
        {

        }
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
        }

        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <MetaDataID>{37e7945f-6814-491f-a9b7-e55c14b9d0d9}</MetaDataID>
        public bool Visisble { get; set; }

        /// <exclude>Excluded</exclude>
        MenuStyles.FontData? _Font;
        /// <MetaDataID>{4a081a73-efc5-464b-9c1c-fbbf1ec8097b}</MetaDataID>
        public MenuStyles.FontData Font
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
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuCanvasFoodItem> _FoodItem=new OOAdvantech.Member<IMenuCanvasFoodItem>();
        /// <MetaDataID>{03ea644a-a1fa-4358-b32e-1ce4c594b4ef}</MetaDataID>
        [ImplementationMember(nameof(_FoodItem))]
        public IMenuCanvasFoodItem FoodItem
        {
            get
            {
                return _FoodItem.Value;
            }
        }


        /// <MetaDataID>{aeb5c72a-bf0e-4015-8405-a782d7ce2b47}</MetaDataID>
        public IMenuPageCanvas Page
        {
            get
            {
                return FoodItem.Page;
            }
        }
        /// <exclude>Excluded</exclude>
        double? _Height;
        /// <MetaDataID>{1083508a-6059-468e-b10b-b5512de313fc}</MetaDataID>
        public double Height
        {
            get
            {
                if (!_Height.HasValue)
                    return Font.MeasureText(Description).Height;

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
        public double Width
        {
            get
            {
                if (!_Width.HasValue)
                    return Font.MeasureText(Description).Width;
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
        /// <MetaDataID>{c732bc82-1c1d-4f91-9547-9942705b2645}</MetaDataID>
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