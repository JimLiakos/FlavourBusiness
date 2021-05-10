using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel
{
    /// <MetaDataID>{a7553c39-0fe6-4802-bd6b-564b0e912294}</MetaDataID>
    public class StyleSheet : IStyleSheet
    {
        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{b76a4c1b-510e-448c-b689-99d6e3aec8e8}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
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

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IStyle> _Styles = new OOAdvantech.Collections.Generic.Set<IStyle>();
        /// <MetaDataID>{5f2a1d95-1061-4928-b208-b776d0b2d035}</MetaDataID>
        [PersistentMember(nameof(_Styles))]
        [BackwardCompatibilityID("+2")]
        public System.Collections.Generic.IList<MenuPresentationModel.IStyle> Styles
        {
           
            get
            {
                return _Styles.AsReadOnly();
            }
        }

        /// <MetaDataID>{b2513d92-2170-41d3-bc06-9a12683a12e7}</MetaDataID>
        public void AddStyle(IStyle style)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Styles.Add(style); 
                stateTransition.Consistent = true;
            }


        }

        /// <MetaDataID>{b4e71fd3-dd62-4bbd-894e-ea958d5d9350}</MetaDataID>
        public void RemoveStyle(IStyle style)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Styles.Remove(style); 
                stateTransition.Consistent = true;
            }

        }
    }
}