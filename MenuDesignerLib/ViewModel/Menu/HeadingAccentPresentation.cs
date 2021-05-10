using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{ed099022-288c-42d6-a62d-d169467ef602}</MetaDataID>
    public class HeadingAccentPresentation:MarshalByRefObject, INotifyPropertyChanged
    {
        /// <MetaDataID>{6381ed8c-7001-42cb-9187-74c7e806a1b0}</MetaDataID>
        public readonly MenuPresentationModel.MenuStyles.IAccent Accent;
        /// <MetaDataID>{89dcbd1a-3d70-4b46-895a-7536dfdb84f7}</MetaDataID>
        public HeadingAccentPresentation(MenuPresentationModel.MenuStyles.IAccent accent)
        {
            Accent = accent;

        }
        /// <MetaDataID>{49d2728a-91ca-4a01-b675-90624486a004}</MetaDataID>
        public override string ToString()
        {
            return Accent.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
