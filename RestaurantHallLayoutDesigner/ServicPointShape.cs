using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace RestaurantHallLayoutModel
{
    /// <MetaDataID>{8ee2017e-a1df-4c13-b2d6-3d6856bb81c5}</MetaDataID>
    [BackwardCompatibilityID("{8ee2017e-a1df-4c13-b2d6-3d6856bb81c5}")]
    [Persistent()]
    public class ServicePointShape : Shape
    {
        public ServicePointShape()
        {

        }
        [OOAdvantech.Json.JsonConstructor]
        public ServicePointShape(double? height, double? width, double? left, double? top, int? zIndex, ShapesGroup group, string shapeImageUrl,string servicePointUri) :
            base(height, width, left, top, zIndex, group, shapeImageUrl)
        {
            _ServicePointUri = servicePointUri;
        }

        /// <exclude>Excluded</exclude>
        string _ServicePointUri;
        /// <MetaDataID>{5226504a-825a-415e-ae62-ccca3259292f}</MetaDataID>
        [PersistentMember(nameof(_ServicePointUri)), BackwardCompatibilityID("+1")]
        public string ServicePointUri
        {
            get
            {
                return _ServicePointUri;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ServicePointUri = value;
                    stateTransition.Consistent = true;
                }
            }
        }


    }
}