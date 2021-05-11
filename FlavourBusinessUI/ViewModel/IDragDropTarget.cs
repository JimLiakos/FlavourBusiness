using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FLBManager.ViewModel
{
    /// <MetaDataID>{419f783e-f660-4fe5-831d-d5c07667fca2}</MetaDataID>
    public interface IDragDropTarget
    {
        /// <MetaDataID>{3a9c7278-11ab-4c80-a064-b9985297fa73}</MetaDataID>
        void DragEnter(object sender, DragEventArgs e);

        /// <MetaDataID>{74f60b3c-6346-42f1-9f63-47d3016c18a6}</MetaDataID>
        void DragLeave(object sender, DragEventArgs e);

        /// <MetaDataID>{2e536b6d-3f44-4c4e-b706-4020a715ff29}</MetaDataID>
        void DragOver(object sender, DragEventArgs e);

        /// <MetaDataID>{66b60780-81a8-4a4f-a880-c99146d5d54c}</MetaDataID>
        void Drop(object sender, DragEventArgs e);
    }
}

