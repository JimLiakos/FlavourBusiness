using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{56fbb85e-52db-4014-86ba-984205077339}</MetaDataID>
    public interface IPriceHeading : IMenuCanvasItem
    {
        [Association("MultiPriceHeadings", Roles.RoleB, "c4ef3e80-84ab-4e20-8c00-983a35b19ac0")]
        [RoleBMultiplicityRange(1, 1)]
        IItemMultiPriceHeading PricesHeading { get; }

        /// <MetaDataID>{00b1e95a-6596-4a55-b948-a2ac8a4cf8b5}</MetaDataID>
        TransformOrigin TransformOrigin { get; set; }

        /// <MetaDataID>{9b2b00d1-78ec-4b22-9875-6c8017cd06a7}</MetaDataID>
        double Angle { get; set; }

        /// <MetaDataID>{9fd0673f-cd98-401b-a79b-1357430caa50}</MetaDataID>
        double PriceHeadinTextXPos { get; set; }

        [Association("PriceHeadingSource", Roles.RoleA, "fea6bbce-c264-4dc9-96a1-49527a1a4160")]
        [RoleAMultiplicityRange(1, 1)]
        MenuModel.ItemSelectorOption Source { set; get; }


        /// <MetaDataID>{3960d608-34ae-43fb-8f54-32ebcb5c5889}</MetaDataID>
        double PriceHeadingTextWitdh { get; set; }

        /// <MetaDataID>{7154c09a-7d63-4f89-85cb-1b6696ac713f}</MetaDataID>
        void UseStyleTransformOrigin();
        /// <MetaDataID>{ad9e9487-3e8d-4425-a52e-d177cab0b9bd}</MetaDataID>
        void ResetTransformOrigin();
        /// <MetaDataID>{7f0447e7-be72-424b-b902-dd9e71f2c881}</MetaDataID>
        void ResetAngle();
    }
}