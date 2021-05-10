namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{2e2ceb4e-4cb1-4909-9948-9929fdb22d21}</MetaDataID>
    public struct Rect
    {

        public Rect(double x, double y,double width,double height)
        {
            _X = x;
            _Y = y;
            _Width = width;
            _Height = height;
        }
        /// <exclude>Excluded</exclude> 
        double _X;
        /// <MetaDataID>{905fd0a8-7b5e-4ee2-a6e6-453c51370f61}</MetaDataID>
        public double X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Y;
        /// <MetaDataID>{90b6b5d5-c420-4abf-83cd-9fc47087e07d}</MetaDataID>
        public double Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Width;
        /// <MetaDataID>{96083f9f-39d3-4425-bc94-23e06768cf0b}</MetaDataID>
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }


        /// <exclude>Excluded</exclude>
        double _Height;
        /// <MetaDataID>{c828cff8-c460-40fd-b706-0cb56d0bb8a9}</MetaDataID>
        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }

    }
}