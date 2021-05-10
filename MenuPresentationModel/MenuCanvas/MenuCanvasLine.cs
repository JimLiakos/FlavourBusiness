using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{9e19e598-74a7-4eca-bf72-81815b02a1ca}</MetaDataID>
    [BackwardCompatibilityID("{9e19e598-74a7-4eca-bf72-81815b02a1ca}")]
    [Persistent()]
    public class MenuCanvasLine :MarshalByRefObject,IMenuCanvasLine
    {

        /// <exclude>Excluded</exclude>
        MenuStyles.LineType _LineType;
        /// <MetaDataID>{d8a6f008-5e52-41d1-8ffb-a9747420480a}</MetaDataID>
        [PersistentMember(nameof(_LineType))]
        [BackwardCompatibilityID("+8")]
        public MenuStyles.LineType LineType
        {
            get
            {
                return _LineType;
            }
            set
            {
                if (_LineType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LineType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{6637af55-05a6-4e0b-8778-64bc0833346e}</MetaDataID>
        protected MenuCanvasLine()
        {

        }
        /// <MetaDataID>{8927012c-de29-4622-a99e-b8d91cd1f486}</MetaDataID>
        public MenuCanvasLine(double x1, double y1, double x2, double y2, string stroke = "#000000", double strokeThickness = 1, MenuStyles.LineType lineType=MenuStyles.LineType.Single)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            _Stroke = stroke;
            if (_Stroke == null)
                _Stroke = "#000000";
            _StrokeThickness = strokeThickness;
            if (_StrokeThickness == 0)
                _StrokeThickness = 2;

            _LineType = lineType;
        }

        /// <exclude>Excluded</exclude>
        IMenuPageCanvas _Page;
        /// <MetaDataID>{77d282cb-894e-4547-85d2-424a6bec9a2d}</MetaDataID>
        [PersistentMember(nameof(_Page))]
        [BackwardCompatibilityID("+7")]
        public IMenuPageCanvas Page
        {
            get
            {

                return _Page;
            }
        }

        /// <exclude>Excluded</exclude>
        double _StrokeThickness;

        /// <MetaDataID>{23b5764f-2e1d-4ce7-a48b-3a00926d88de}</MetaDataID>
        [PersistentMember(nameof(_StrokeThickness))]
        [BackwardCompatibilityID("+6")]
        public double StrokeThickness
        {

            get
            {
                return _StrokeThickness;
            }
            set
            {
                if (_StrokeThickness != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StrokeThickness = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        string _Stroke;

        /// <MetaDataID>{88ff4177-b917-4671-a499-3a3582078620}</MetaDataID>
        [PersistentMember(nameof(_Stroke))]
        [BackwardCompatibilityID("+5")]
        public string Stroke
        {
            get
            {
                if (_Stroke == null)
                    return "#000000";
                return _Stroke;
            }
            set
            {
                if (_Stroke != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Stroke = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        double _Y2;

        /// <MetaDataID>{e10c1f1a-1c6a-48d2-bafa-841b421ad08f}</MetaDataID>
        [PersistentMember(nameof(_Y2))]
        [BackwardCompatibilityID("+4")]
        public double Y2
        {
            get
            {
                return _Y2;
            }

            set
            {
                if (_Y2 != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Y2 = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        double _X2;

        /// <MetaDataID>{069f3874-4e7c-4c59-b0c4-06b38ff5bdff}</MetaDataID>
        [PersistentMember(nameof(_X2))]
        [BackwardCompatibilityID("+3")]
        public double X2
        {
            get
            {
                return _X2;
            }

            set
            {
                if (_X2 != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _X2 = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _Y1;
        /// <MetaDataID>{a17b9a8c-32ee-4fb5-9e80-163d28645346}</MetaDataID>
        [PersistentMember(nameof(_Y1))]
        [BackwardCompatibilityID("+2")]
        public double Y1
        {
            get
            {
                return _Y1;
            }
            set
            {
                if (_Y1 != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Y1 = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double _X1;
        /// <MetaDataID>{b3f7327d-1d64-49c2-b646-75ab79e0c1bd}</MetaDataID>
        [PersistentMember(nameof(_X1))]
        [BackwardCompatibilityID("+1")]
        public double X1
        {
            get
            {
                return _X1;
            }
            set
            {
                if (_X1 != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _X1 = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}