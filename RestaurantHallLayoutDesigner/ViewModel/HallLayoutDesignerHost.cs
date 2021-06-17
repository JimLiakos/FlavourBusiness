using FloorLayoutDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorLayoutDesigner
{
    /// <MetaDataID>{633c2624-ef93-4e1c-84f7-35f266848575}</MetaDataID>
    public abstract class HallLayoutDesignerHost
    {
        public static HallLayoutDesignerHost Current;

        public HallLayoutViewModel HallLayout { get; internal set; }

        public abstract void ShowHallLayout(ServiceAreaPresentation serviceAreaPresentation);

        public abstract MenuItemsEditor.RestaurantMenus RestaurantMenus { get; }
    }
}
