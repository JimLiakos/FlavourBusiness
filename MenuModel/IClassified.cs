using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{740b8ca3-8272-485b-88f5-390442d1828d}</MetaDataID>
    public interface IClassified
    {
        [Association("ClassifiedItem", Roles.RoleB, "08c2a04a-ed64-4ffb-b460-ba544fbeda1b")]
        [RoleBMultiplicityRange(1, 1)]
        IClass Class { get; set; }
    }
}