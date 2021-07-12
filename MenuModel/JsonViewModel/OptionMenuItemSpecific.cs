using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{2bfa4438-abcd-46b7-bb1e-1136704c7a07}</MetaDataID>
    public class OptionMenuItemSpecific : IOptionMenuItemSpecific
    {
        public bool Hide { get; set; }

        public ILevel InitialLevel
        {
            get
            {
                if (InitialLevelIndex == -1)
                    return null;
                return Option?.LevelType.Levels[InitialLevelIndex];
            }
            set
            {

            }
        }
        public int InitialLevelIndex { get; set; }


        public IMenuItem MenuItemOptionSpecific { get; set; }

        public IPreparationScaledOption Option { get; set; }
        public string Uri { get; internal set; }
    }
}
