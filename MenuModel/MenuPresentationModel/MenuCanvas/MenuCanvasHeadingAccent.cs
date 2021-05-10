using System;
using System.Windows;
using System.Windows.Media;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{f468a1b0-9b22-4196-bfeb-251e14611369}</MetaDataID>
    public class MenuCanvasHeadingAccent :MarshalByRefObject, IMenuCanvasHeadingAccent
    {
        public event ObjectChangeStateHandle ObjectChangeState;

        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{ff832c90-ae41-496d-b561-b0aa9f8694b0}</MetaDataID>
        public MenuCanvasHeadingAccent(IMenuCanvasHeading heading, IHeadingAccent accent)
        {
            _Heading = heading;
            _Accent = accent;
        }
        /// <exclude>Excluded</exclude>
        IHeadingAccent _Accent;
        /// <MetaDataID>{95c0b33a-9cac-4256-9f05-86e02c5ecab0}</MetaDataID>
        public IHeadingAccent Accent
        {
            get
            {
                return _Accent;
            }

            set
            {

                if (_Accent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Accent = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{bfed063c-d596-41c4-b109-283616377a64}</MetaDataID>
        public string Description
        {
            get
            {
                return "";
            }

            set
            {

            }
        }

        /// <exclude>Excluded</exclude>
        IMenuCanvasHeading _Heading;
        /// <MetaDataID>{85b61f6c-133b-4285-b81e-3dc6158c28c9}</MetaDataID>
        public IMenuCanvasHeading Heading
        {
            get
            {
                return _Heading;
            }

            set
            {

                if (_Heading != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Heading = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <MetaDataID>{e2c50ac9-0c0c-43f4-a63c-4a0d18a1c16a}</MetaDataID>
        public IMenuPageCanvas Page
        {
            get
            {
                return Heading.Page;
            }

            set
            {
                throw new NotSupportedException();

            }
        }


        /// <exclude>Excluded</exclude>
        double _Width;
        /// <MetaDataID>{80fafdb6-0ef7-45d7-a5ac-80719030e9b6}</MetaDataID>
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
        /// <MetaDataID>{da01c6f0-fe03-42b3-a72c-07597b9d3106}</MetaDataID>
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

        /// <MetaDataID>{b802bc50-5a7f-4d63-8f5a-2b05aff45b6b}</MetaDataID>
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
        double _FullRowWidth;
        /// <MetaDataID>{2a661bf9-77e8-43f0-8a38-588cc8e2f649}</MetaDataID>
        public double FullRowWidth
        {
            get
            {
                return _FullRowWidth;
            }

            set
            {
                _FullRowWidth = value;

                //if (_FullRowWidth != value)
                //{
                //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                //    {
                //        _FullRowWidth = value;
                //        stateTransition.Consistent = true;
                //    }
                //}

            }
        }

        /// <MetaDataID>{6587780d-638a-4ab1-9c7b-479b2e520e6e}</MetaDataID>
        public bool FullRowImage
        {
            get
            {
                if (_Accent != null)
                    return _Accent.FullRowImage;
                else
                    return false;
            }

            set
            {
                if (_Accent != null)
                    _Accent.FullRowImage = value;

            }
        }

        /// <MetaDataID>{ced0f2c7-22fb-4df3-a63c-a5e2408da5eb}</MetaDataID>
        public double Height
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{bdc1e806-f7f8-4e32-8ca9-143bc70acf9e}</MetaDataID>
        public FontData Font
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public double HeadingTextMedline
        {
            get
            {
                return Heading.Font.GetTextMedline(Heading.Description);
            }
        }
        public double HeadingTextHeight
        {
            get
            {
                return Heading.Font.MeasureText(Heading.Description).Height;
            }
        }

        /// <MetaDataID>{79c8928f-430f-4c40-b4a9-c549186e4191}</MetaDataID>
        internal double MarginTop;

        /// <MetaDataID>{1d2debec-c8e0-412f-a79a-f69b6ca77f33}</MetaDataID>
        internal double MarginLeft;

        /// <MetaDataID>{79b02d9c-ddd0-473f-acca-f2a4102e55e6}</MetaDataID>
        internal double MarginRight;

        /// <MetaDataID>{088f19ee-3b0a-4b2f-bc00-96810369c307}</MetaDataID>
        internal double MarginBottom;

        /// <MetaDataID>{404cb746-87d2-4a3a-9802-fefa5f8626f3}</MetaDataID>
        public Rect GetAccentImageRect(int accentImageIndex)
        {
            double left = 0;
            double top = 0;
            double width = 0;
            double height = 0;

            if (_Accent != null)
            {
                if (_Accent.DoubleImage)
                {
                    if (accentImageIndex == 0)
                    {
                        left = Heading.XPos - Accent.AccentImages[accentImageIndex].Width* Heading.Font.FontSize;
                        left -= Accent.MarginLeft * Heading.Font.FontSize;
                        width = Accent.AccentImages[accentImageIndex].Width * Heading.Font.FontSize;
                        height = Accent.AccentImages[accentImageIndex].Height * Heading.Font.FontSize;
                        top = Heading.YPos + (Heading.Height - Accent.AccentImages[accentImageIndex].Height * Heading.Font.FontSize) / 2;
                    }
                    if (accentImageIndex == 1)
                    {
                        left = Heading.XPos + Heading.Width;// Accent.AccentImages[accentImageIndex].Width * Heading.Font.FontSize;
                        left += Accent.MarginLeft * Heading.Font.FontSize;
                        width = Accent.AccentImages[accentImageIndex].Width * Heading.Font.FontSize;
                        height = Accent.AccentImages[accentImageIndex].Height * Heading.Font.FontSize;
                        top = Heading.YPos + (Heading.Height - Accent.AccentImages[accentImageIndex].Height * Heading.Font.FontSize) / 2;


                    }
                }
                else
                {
                    double fontTextMedline = HeadingTextMedline;
                    double fontMedline = HeadingTextHeight/2;


                    MarginLeft = _Accent.MarginLeft * Heading.Font.FontSize;// (textSize.Width * 1.5 - textSize.Width) / 2;
                    MarginRight = Accent.MarginRight * Heading.Font.FontSize;// (textSize.Width * 1.5 - textSize.Width) / 2;
                    MarginTop = Accent.MarginTop * Heading.Font.FontSize; //(textSize.Height * 1.2 - textSize.Height) / 2;
                    MarginBottom = Accent.MarginBottom * Heading.Font.FontSize;
                    MarginTop += fontMedline - fontTextMedline;
                    MarginBottom -= (fontMedline - fontTextMedline);
                    if (_Accent.FullRowImage)
                    {
                        if (Heading.Page != null && Heading.Page.Style != null)
                        {
                            var pageStyle = (Page.Style.Styles["page"] as MenuStyles.PageStyle);
                            left = pageStyle.Margin.MarginLeft;

                        }
                        else
                            left = 0;
                    }
                    else
                        left = Heading.XPos - MarginLeft;

                    if (_Accent.UnderlineImage)
                        top = Heading.YPos + Heading.Height;
                    else if (_Accent.OverlineImage)
                        top = Heading.YPos + MarginTop;
                    else
                        top = Heading.YPos - MarginTop;



                    if (_Accent.FullRowImage)
                        width = FullRowWidth;
                    else
                        width = Heading.Width + MarginLeft + MarginRight;

                    if (_Accent.UnderlineImage|| _Accent.OverlineImage)
                        height = _Accent.Height;
                    else
                        height = Heading.Height + MarginTop + MarginBottom;
                }

            }

            return new Rect(left, top, width, height);


        }

        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }
    }
}