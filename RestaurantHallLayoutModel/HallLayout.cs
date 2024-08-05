using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using UIBaseEx;

namespace RestaurantHallLayoutModel
{
    /// <MetaDataID>{7b510ebb-3506-47e0-90ad-bece1601c6c4}</MetaDataID>
    [BackwardCompatibilityID("{7b510ebb-3506-47e0-90ad-bece1601c6c4}")]
    [Persistent()]
    public class HallLayout: IHallLayout
    {
        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;

        /// <MetaDataID>{416afd27-7e83-4ae7-9d4a-465a8a19148a}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+9")]
        public string ServicesContextIdentity
        {
            get => _ServicesContextIdentity;
            set
            {

                if (_ServicesContextIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        public string FontsLink { get; set; }

        /// <exclude>Excluded</exclude>
        double _LabelBkCornerRadius =4;
        /// <MetaDataID>{b55121a6-5668-49eb-a31a-1fe4228d6e28}</MetaDataID>
        /// <summary>
        /// Gets or sets the  background border corner radius of the hall.
        /// </summary>
        [PersistentMember(nameof(_LabelBkCornerRadius))]
        [BackwardCompatibilityID("+8")]
        public double LabelBkCornerRadius
        {
            get => _LabelBkCornerRadius;
            set
            {
                if (_LabelBkCornerRadius != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LabelBkCornerRadius = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude> 
        string _LabelBkColor= "#00FFFFFF";

        /// <MetaDataID>{6a19c0bf-3ebc-430b-be75-b6eecfefa4c8}</MetaDataID>
        [PersistentMember(nameof(_LabelBkColor))]
        [BackwardCompatibilityID("+7")]
        public string LabelBkColor
        {
            get => _LabelBkColor;
            set
            {
                if (_LabelBkColor != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LabelBkColor = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _LabelBkOpacity=1;
        /// <MetaDataID>{8729cd42-aa80-44c3-8016-34d7a8a6951f}</MetaDataID>
        /// <summary>
        /// Gets or sets the  background label opacity of the hall.
        /// </summary>
        [PersistentMember(nameof(_LabelBkOpacity))]
        [BackwardCompatibilityID("+6")]
        public double LabelBkOpacity
        {
            get => _LabelBkOpacity;
            set
            {
                if (_LabelBkOpacity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LabelBkOpacity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Margin _Margin = new Margin() { MarginTop = 0, MarginLeft = 7, MarginBottom = 4,  MarginRight = 7};


        /// <MetaDataID>{213963ac-4ba2-4e8f-9467-f0160e29083e}</MetaDataID>
        [PersistentMember(nameof(_Margin))]
        [BackwardCompatibilityID("+5")]
        public Margin Margin
        {
            get => _Margin;
            set
            {

                if (_Margin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Margin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        FontData? _Font;

        /// <MetaDataID>{4a85b4c9-5932-4ada-b118-2abe99c04592}</MetaDataID>
        [PersistentMember(nameof(_Font))]
        [BackwardCompatibilityID("+4")]
        public FontData Font
        {
            get
            {
                if (!_Font.HasValue)
                     _Font = new FontData() { AllCaps = true, BlurRadius = 0, FontFamilyName = "Airbag Free", FontSize = 22, Shadow = true, ShadowColor = "#FFD3D3D3", ShadowXOffset = 2, ShadowYOffset = 2 };
                
                return _Font.Value;
            }
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
        }



        /// <exclude>Excluded</exclude>
        double _Height;
        /// <MetaDataID>{5a43cab0-5038-4afb-9085-76446b0a4984}</MetaDataID>
        /// <summary>
        /// Gets or sets the height  of the hall.
        /// </summary>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+3")]
        public double Height
        {
            get => _Height;
            set
            {

                if ((int)_Height != (int)value)
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

        /// <MetaDataID>{e15557cc-ff30-4b98-a986-707760360c6f}</MetaDataID>
        /// <summary>
        /// Gets or sets the width of the hall. 
        /// </summary>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+2")]
        public double Width
        {
            get => _Width;
            set
            {

                if ((int)_Width != (int)value)
                {
                 
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Width = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <MetaDataID>{b4831d65-18e7-4494-851a-72bdb131a633}</MetaDataID>
        public HallLayout()
        {

        }
        /// <MetaDataID>{f26d364b-f41f-4238-8982-ca9c86c8c275}</MetaDataID>
        [OOAdvantech.Json.JsonConstructor]
        public HallLayout(string name, System.Collections.Generic.List<Shape> shapes)
        {
            _Name = name;
            _Shapes.AddRange(shapes);
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Shape> _Shapes = new OOAdvantech.Collections.Generic.Set<Shape>();


        [RoleAMultiplicityRange(0)]
        [Association("HallLayoutShape", Roles.RoleA, "8fb70efa-27ff-46cd-b6cb-082761d76962")]
        [RoleBMultiplicityRange(1, 1)]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember(nameof(_Shapes))]
        public System.Collections.Generic.List<Shape> Shapes => _Shapes.AsReadOnly().ToList();




        /// <MetaDataID>{438bf478-6d7f-43bf-9549-955cddf01992}</MetaDataID>
        public void AddShape(Shape shape)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (ObjectStorage.GetStorageOfObject(shape)==null)
                {

                }
                _Shapes.Add(shape);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{877a193a-b2d5-433a-9aec-c3806511df4a}</MetaDataID>
        public void RemoveShape(Shape shape)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Shapes.Remove(shape);
                stateTransition.Consistent = true;
            }

        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _Name;


        public IServiceArea _ServiceArea;
        /// <MetaDataID>{698bfaf8-4758-4ce7-a03d-8626d886fad0}</MetaDataID>
        public IServiceArea ServiceArea
        {
            get
            {
                return _ServiceArea;
            }
            set
            {
                _ServiceArea = value;
                if (value != null)
                    ServicesContextIdentity = value.ServicesContextIdentity;
            }
        }

        /// <MetaDataID>{12c715d6-2d08-4397-9689-6c7bba6e8d65}</MetaDataID>
        [PersistentMember(nameof(_Name)), BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                
                return _Name;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Name = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{fdb74280-e734-4b31-8352-3a54e0c159b3}</MetaDataID>
        public void SetShapesImagesRoot(string shapesRoot)
        {
            foreach (var shape in GetAllShapes())
                shape.ShapeImageUrl = shape.ShapeImageUrl.Replace("http://127.0.0.1:10000/devstoreaccount1/halllayoutsresources/Shapes/", shapesRoot);
        }
        /// <MetaDataID>{73089240-f45f-4794-b49f-4252d1b6c0a0}</MetaDataID>
        public List<Shape> GetAllShapes()
        {
            List<Shape> shapes = new List<Shape>();
            foreach (var shape in Shapes)
            {
                if ((shape is ShapesGroup))
                {
                    shapes.AddRange((shape as ShapesGroup).GetAllShapes());
                }
                else
                {
                    shapes.Add(shape);
                }
            }
            return shapes;
        }

        

        /// <exclude>Excluded</exclude>
        string _HallLayoutUri;
        public string HallLayoutUri
        {
            get
            {
                if(string.IsNullOrWhiteSpace(_HallLayoutUri))
                    _HallLayoutUri= ObjectStorage.GetStorageOfObject(this)?.GetPersistentObjectUri(this);

                return _HallLayoutUri;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(HallLayoutUri))
                    _HallLayoutUri = value;
            }
        }
    }
}