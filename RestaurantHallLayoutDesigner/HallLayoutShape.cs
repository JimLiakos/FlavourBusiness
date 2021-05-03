using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace RestaurantHallLayoutModel
{
    /// <MetaDataID>{744f2bff-88cd-448e-9a2b-624a2f6646b3}</MetaDataID>
    [BackwardCompatibilityID("{744f2bff-88cd-448e-9a2b-624a2f6646b3}")]
    [Persistent()]
    public class Shape
    {

        /// <exclude>Excluded</exclude>
        string _Label;
        /// <MetaDataID>{65267040-0f20-47a0-b730-95938a14fc96}</MetaDataID>
        [PersistentMember(nameof(_Label))]
        [BackwardCompatibilityID("+10")]
        public string Label
        {
            get => _Label;
            set
            {

                if (_Label != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Label = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesPointIdentity;

        /// <MetaDataID>{87d281f8-ff22-40cf-becf-dbf81cd39cab}</MetaDataID>
        [PersistentMember(nameof(_ServicesPointIdentity))]
        [BackwardCompatibilityID("+9")]
        public string ServicesPointIdentity
        {
            get => _ServicesPointIdentity;
            set
            {

                if (_ServicesPointIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesPointIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        ///// <MetaDataID>{4760bca6-72cc-4820-a38d-243683d92c29}</MetaDataID>
        //public string Label
        //{
        //    get
        //    {
        //        if (Group == null)
        //            return "LAsa";
        //        else
        //            return "";
        //    }
        //}


        /// <exclude>Excluded</exclude>
        double _RotateAngle;

        /// <MetaDataID>{fb3dd6cd-5a21-4cad-ac26-0de2e258a331}</MetaDataID>
        /// <summary>
        /// Gets or sets the rotation angle of the shape. 
        /// </summary>
        [PersistentMember(nameof(_RotateAngle))]
        [BackwardCompatibilityID("+8")]
        public double RotateAngle
        {
            get => _RotateAngle;
            set
            {
                if (_RotateAngle != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RotateAngle = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{01d91f7c-b63e-4365-93e8-1c9a45179d60}</MetaDataID>
        public Shape()
        {

        }

        /// <MetaDataID>{cbd1b738-b245-4443-9ffc-c4bb2fd476f8}</MetaDataID>
        [OOAdvantech.Json.JsonConstructor]
        public Shape(double? height, double? width, double? left, double? top, int? zIndex, ShapesGroup group, string shapeImageUrl)
        {
            if (height != null)
                _Height = height.Value;
            if (width != null)
                _Width = width.Value;
            if (left != null)
                _Left = left.Value;
            if (top != null)
                _Top = top.Value;
            if (zIndex != null)
                _ZIndex = zIndex.Value;

            _Group.Value = group;
            _ShapeImageUrl = shapeImageUrl;


        }
        /// <exclude>Excluded</exclude>
       OOAdvantech.Member< ShapesGroup> _Group=new OOAdvantech.Member<ShapesGroup>();
        [Association("GroupedShapes", Roles.RoleB, "babd469a-0c54-4be8-abaa-7fed51b0fdc7")]
        [RoleBMultiplicityRange(1, 1)]
        [PersistentMember(nameof(_Group))]
        public ShapesGroup Group
        {
            get => _Group;
            set
            {

                if (_Group.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Group .Value= value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{56426007-e753-4015-bf61-9003d79778a0}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+7")]
        public string Identity
        {
            get => _Identity;
            set
            {
                if (_Identity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Identity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _ShapeImageUrl;

        /// <MetaDataID>{bd4ce134-eec7-49ad-8022-7d9940ed284b}</MetaDataID>
        [PersistentMember(nameof(_ShapeImageUrl))]
        [BackwardCompatibilityID("+6")]
        public string ShapeImageUrl
        {
            get => _ShapeImageUrl;
            set
            {
                if (_ShapeImageUrl != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ShapeImageUrl = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        protected OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double _Height;


        /// <summary>
        /// Gets or sets the height of the shape. 
        /// </summary>
        /// <MetaDataID>{c8381647-cf1e-4b42-90d4-1bfb6d486f69}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+1")]
        public double Height
        {
            get => _Height;
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
        double _Width;

        /// <summary>
        /// Gets or sets the width of the shape. 
        /// </summary>
        /// <MetaDataID>{a6060060-fcda-4783-bc20-d644fea62195}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+2")]
        public double Width
        {
            get => _Width;
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
        int _ZIndex;
        /// <MetaDataID>{2c6ad3ea-15b2-4f8a-a6d8-a9122eb6f334}</MetaDataID>
        [PersistentMember(nameof(_ZIndex))]
        [BackwardCompatibilityID("+3")]
        public int ZIndex
        {
            get => _ZIndex;
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ZIndex = value;
                    stateTransition.Consistent = true;
                }
            }
        }


        /// <exclude>Excluded</exclude>
        double _Top;

        /// <summary>
        ///  Gets or sets the position of the shape's top edge, in relation to the Canvas.
        /// </summary>
        /// <MetaDataID>{92af05f0-10ae-4f0d-925a-780203e5ddf8}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [PersistentMember(nameof(_Top))]
        public double Top
        {
            get
            {
                if (_Top < 0)
                    _Top = 0;
                return _Top;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Top = value;
                    if (_Top < 0)
                        _Top = 0;
                    stateTransition.Consistent = true;
                }
            }
        }


        /// <exclude>Excluded</exclude>
        double _Left;


        /// <summary>
        ///  Gets or sets the position of the shape's left edge, in relation to the Canvas.
        /// </summary>
        /// <MetaDataID>{fafa4cc4-9cdb-49fb-911e-fe65b76c451d}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [PersistentMember(nameof(_Left))]
        public double Left
        {
            get
            {
                if (_Left < 0)
                    _Left = 0;
                return _Left;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Left = value;
                    if (_Left < 0)
                        _Left = 0;

                    stateTransition.Consistent = true;
                }
            }
        }




    }
}