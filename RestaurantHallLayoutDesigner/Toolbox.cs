﻿using System.Windows;
using System.Windows.Controls;

namespace FloorLayoutDesigner
{
    // Implements ItemsControl for ToolboxItems    
    /// <MetaDataID>{6cbf278b-5506-4657-99be-88e2d8d96475}</MetaDataID>
    public class Toolbox : ItemsControl
    {
        // Defines the ItemHeight and ItemWidth properties of
        // the WrapPanel used for this Toolbox
        public Size ItemSize
        {
            get { return itemSize; }
            set { itemSize = value; }
        }
        private Size itemSize = new Size(50, 50);

        // Creates or identifies the element that is used to display the given item.        
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ToolboxItem();
        }

        // Determines if the specified item is (or is eligible to be) its own container.        
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ToolboxItem);
        }
    }
}
