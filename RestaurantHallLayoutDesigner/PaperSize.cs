using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository;

namespace RestaurantHallLayoutModel
{
    /// <MetaDataID>{e479de6e-ebee-4d13-a096-b01f7e9ec41c}</MetaDataID>
    [BackwardCompatibilityID("{e479de6e-ebee-4d13-a096-b01f7e9ec41c}")]
    [Persistent()]
    public struct PaperSize
    {

        public readonly string Unit;
        public PaperSize(PaperType paperType, string description, double width, double height, string unit)
        {
            PaperType = paperType;
            _Description = description;
            _Width = width;
            _Height = height;
            Unit = unit;


        }



        public readonly PaperType PaperType;

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{282e3459-f5d5-4dc0-bafd-14def09c13ee}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+3")]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description != value)
                    _Description = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Height;

        /// <MetaDataID>{62c02947-89f6-4996-8114-ff7f9d4b3942}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+2")]
        public double Height
        {
            get
            {
                return _Height;
            }

            set
            {

                if (_Height != value)
                {

                    _Height = value;

                }

            }
        }

        /// <exclude>Excluded</exclude>
        double _Width;

        /// <MetaDataID>{5de393c6-a20d-48dd-8692-13c42ee7a668}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+1")]
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

                    _Width = value;

                }
            }
        }
    }

    /// <MetaDataID>{be25d174-7b0e-4028-ab55-8d40d7124fd2}</MetaDataID>
    public enum PaperType
    {
        Unspecified, Letter, Legal, Tabloid, Statement, A3, A4, A5, B4, B5, HDTV, Custom
    }
}
