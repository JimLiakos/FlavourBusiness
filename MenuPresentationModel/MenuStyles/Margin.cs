using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{89123ac3-2377-433d-9d18-b6fa04a9445e}</MetaDataID>
    [BackwardCompatibilityID("{89123ac3-2377-433d-9d18-b6fa04a9445e}")]
    [Persistent()]
    public struct Margin
    {
        /// <MetaDataID>{e1800e22-e51e-47be-9ded-7067059497e9}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+1")]
        public double MarginTop;
        /// <MetaDataID>{cc6bcad3-6282-4793-a1b4-7bee079c3408}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+2")]
        public double MarginBottom;
        /// <MetaDataID>{fe6b0d3c-f297-4f2d-80a4-fb79974f5ee2}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+3")]
        public double MarginLeft;
        /// <MetaDataID>{fcfe591f-af54-4c05-9bdd-0f1c01eeea9d}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+4")]
        public double MarginRight;


        /// <MetaDataID>{c6c2f60f-7336-4a54-9c4e-a3ea49a6953c}</MetaDataID>
        public static bool operator ==(Margin left, Margin right)
        {
            if (left.MarginLeft == right.MarginLeft &&
                left.MarginTop == right.MarginTop &&
                left.MarginRight == right.MarginRight &&
                left.MarginBottom == right.MarginBottom)
                return true;
            else
                return false;
        }

        /// <MetaDataID>{2eebe3f8-0629-4c3b-a069-a55880781a4e}</MetaDataID>
        public static bool operator !=(Margin left, Margin right)
        {
            return !(left == right);
        }
    }
}