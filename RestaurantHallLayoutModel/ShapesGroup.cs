using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantHallLayoutModel
{
    /// <MetaDataID>{34e5c918-a60f-4116-8b2c-76b8699a6cdc}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{34e5c918-a60f-4116-8b2c-76b8699a6cdc}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public class ShapesGroup : Shape
    {


        /// <exclude>Excluded</exclude>
        double _ClientAreaWidth;
        /// <MetaDataID>{3cb3dc10-54ba-422d-ac43-6c40d285ff39}</MetaDataID>
        /// <summary>
        /// Gets or sets the height of the shape. 
        /// </summary>
        [PersistentMember(nameof(_ClientAreaWidth))]
        [BackwardCompatibilityID("+2")]
        public double ClientAreaWidth
        {
            get => _ClientAreaWidth;
            set
            {

                if (_ClientAreaWidth != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ClientAreaWidth = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        double _ClientAreaHeight;

        /// <MetaDataID>{2366cb08-1fbc-4ae7-847d-7e0f04679bab}</MetaDataID>
        /// <summary>
        /// Gets or sets the height of the shape. 
        /// </summary>
        [PersistentMember(nameof(_ClientAreaHeight))]
        [BackwardCompatibilityID("+1")]
        public double ClientAreaHeight
        {
            get => _ClientAreaHeight;
            set
            {
                if (_ClientAreaHeight != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ClientAreaHeight = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <MetaDataID>{268cb57b-e357-4767-acdb-fb3c26161d69}</MetaDataID>
        public ShapesGroup()
        {

        }
        /// <MetaDataID>{7034aebc-966a-41e9-a382-90d8489fa94e}</MetaDataID>
        [OOAdvantech.Json.JsonConstructor]
        public ShapesGroup(double? height, double? width, double? left, double? top, int? zIndex, ShapesGroup group, string shapeImageUrl, System.Collections.Generic.List<Shape> shapes) :
                    base(height, width, left, top, zIndex, group, shapeImageUrl)
        {
            _Shapes.AddRange(shapes);
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Shape> _Shapes = new OOAdvantech.Collections.Generic.Set<Shape>();

        /// <MetaDataID>{d208b294-86b0-4a58-bd10-98fbccd5669b}</MetaDataID>
        [Association("GroupedShapes", Roles.RoleA, "babd469a-0c54-4be8-abaa-7fed51b0fdc7")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange()]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember(nameof(_Shapes))]
        public System.Collections.Generic.List<Shape> Shapes => _Shapes.AsReadOnly().ToList();




        //[Association("GroupedShapes", Roles.RoleA, "babd469a-0c54-4be8-abaa-7fed51b0fdc7")]
        //[RoleAMultiplicityRange(1)]
        //[RoleBMultiplicityRange(1, 1)]
        //[PersistentMember(nameof(_Shapes))]
        //public System.Collections.Generic.List<Shape> Shapes => _Shapes.AsReadOnly().ToList();



        /// <MetaDataID>{e2d1a93d-2c20-4318-9b48-1d6ce2c282ca}</MetaDataID>
        public void AddGroupedShape(Shape shape)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (ObjectStorage.GetStorageOfObject(shape) == null)
                {

                }
                _Shapes.Add(shape);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{3841638c-499f-47f0-9c4c-e7f744089745}</MetaDataID>
        public void RemoveGroupedShape(Shape shape)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Shapes.Remove(shape);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{51d963e6-9eb5-4d00-9fd0-4330e79c18f6}</MetaDataID>
        internal List<Shape> GetAllShapes()
        {
            List<Shape> shapes = new List<Shape>();
            foreach (var shape in Shapes)
            {
                if ((shape is ShapesGroup))
                    shapes.AddRange((shape as ShapesGroup).GetAllShapes());
                else
                    shapes.Add(shape);
            }
            return shapes;
        }
    }
}