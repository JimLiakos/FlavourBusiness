using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{847dd153-16b8-4a1c-b1c7-73e9ad5bb64c}</MetaDataID>
    /// <summary>x-axis y-axis z-axis</summary>
    [BackwardCompatibilityID("{847dd153-16b8-4a1c-b1c7-73e9ad5bb64c}")]
    [Persistent()]
    public struct TransformOrigin
    {

        public TransformOrigin(string xAxis, string yAxis, string zAxis="")
        {
            _xAxis = xAxis;
            _yAxis = yAxis;
            _zAxis = zAxis;

        }
        /// <exclude>Excluded</exclude>
        string _zAxis;
        /// <MetaDataID>{55ea282d-ca42-457f-ae5c-cc0057066c01}</MetaDataID>
        [PersistentMember(nameof(_zAxis))]
        [BackwardCompatibilityID("+3")]
        public string zAxis
        {
            get
            {
                return _zAxis;
            }
            set
            {
                _zAxis = value;
            }
        }
        /// <MetaDataID>{7a4082e4-3d4f-4b48-ac0e-a8892d64a6f4}</MetaDataID>
        public static bool operator ==(TransformOrigin left, TransformOrigin right)
        {
            return left.xAxis == right.xAxis && left.yAxis == right.yAxis && left.zAxis == right.zAxis;

        }
        /// <MetaDataID>{097993f2-08fd-4201-9f35-125ae431f2b8}</MetaDataID>
        public static bool operator !=(TransformOrigin left, TransformOrigin right)
        {
            return !(left == right);
        }

        /// <exclude>Excluded</exclude>
        string _yAxis;
        /// <MetaDataID>{aedca5ee-2350-4810-9aa7-3fe58fa6dfba}</MetaDataID>
        [PersistentMember(nameof(_yAxis))]
        [BackwardCompatibilityID("+2")]
        public string yAxis
        {
            get
            {
                return _yAxis;
            }

            set
            {
                _yAxis = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _xAxis;

        /// <MetaDataID>{33c97347-44df-4eaf-af1f-41cd0fccc6be}</MetaDataID>
        [PersistentMember(nameof(_xAxis))]
        [BackwardCompatibilityID("+1")]
        public string xAxis
        {
            get
            {
                return _xAxis;
            }

            set
            {
                _xAxis = value;
            }
        }
    }
}