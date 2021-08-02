using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using WPFUIElementObjectBind;

namespace MenuItemsEditor
{
    /// <MetaDataID>{0a000290-c139-4f55-8c87-590b0979ce92}</MetaDataID>
    public interface IMenusTreeNode
    {

        /// <MetaDataID>{0e0b24ab-14bc-4931-9243-a4150f4b6c5c}</MetaDataID>
        IMenusTreeNode Parent { set; get; }

        /// <MetaDataID>{f0291a2d-7418-4b27-89ae-c465f77273ed}</MetaDataID>
        string Name
        {
            get;
            set;
        }

        /// <MetaDataID>{86f26e0e-b4a0-4356-b51c-84cc5dd61498}</MetaDataID>
        bool IsEditable
        {
            get;
        }


        /// <MetaDataID>{70879150-6d69-4d7f-a3ef-34a680e8f81c}</MetaDataID>
        bool IsNodeExpanded
        {
            get; set;
        }

        /// <MetaDataID>{d9765d9e-d718-40a7-bc42-9bca31e65341}</MetaDataID>
        System.Windows.Media.ImageSource TreeImage
        {
            get;
        }

        /// <MetaDataID>{c74adff8-12b9-4340-8703-50081db7aa0b}</MetaDataID>
        List<IMenusTreeNode> Members
        {
            get;
        }

        /// <MetaDataID>{1d5d6578-1eca-45cb-8edf-bc1e099716b8}</MetaDataID>
        bool Edit
        {
            get;
            set;

        }

        /// <MetaDataID>{7678808c-cc89-4df3-a7b2-6872ef77d61d}</MetaDataID>
        bool IsSelected
        {
            get; set;
        }
        /// <MetaDataID>{4feba61c-ef76-4417-a2c6-236a9689ef16}</MetaDataID>
        bool HasContextMenu
        {
            get;
        }


        /// <MetaDataID>{6c7e1203-a053-4ec8-b914-e82ed50d7dab}</MetaDataID>
        List<MenuCommand> ContextMenuItems
        {
            get;
        }

        /// <MetaDataID>{573cbb11-5231-4b31-a732-2b6984e6084a}</MetaDataID>
        List<MenuCommand> SelectedItemContextMenuItems
        {
            get;
        }


    }


    ///// <MetaDataID>{419f783e-f660-4fe5-831d-d5c07667fca2}</MetaDataID>
    //public interface IDragDropTarget
    //{
    //    /// <MetaDataID>{3a9c7278-11ab-4c80-a064-b9985297fa73}</MetaDataID>
    //    void DragEnter(object sender, DragEventArgs e);

    //    /// <MetaDataID>{74f60b3c-6346-42f1-9f63-47d3016c18a6}</MetaDataID>
    //    void DragLeave(object sender, DragEventArgs e);

    //    /// <MetaDataID>{2e536b6d-3f44-4c4e-b706-4020a715ff29}</MetaDataID>
    //    void DragOver(object sender, DragEventArgs e);

    //    /// <MetaDataID>{66b60780-81a8-4a4f-a880-c99146d5d54c}</MetaDataID>
    //    void Drop(object sender, DragEventArgs e);
    //}


    ///// <MetaDataID>{8f8ace39-910a-4182-a3db-5469860fcccd}</MetaDataID>
    // class MenuComamnd : MarshalByRefObject, System.ComponentModel.INotifyPropertyChanged
    //{
    //    System.Windows.Input.ICommand _Command;
    //    public System.Windows.Input.ICommand Command
    //    {
    //        get
    //        {
    //            return _Command;
    //        }
    //        set
    //        {
    //            _Command = value;
    //        }
    //    }

    //    string _Header;
    //    public string Header
    //    {
    //        get
    //        {
    //            return _Header;
    //        }
    //        set
    //        {
    //            _Header = value;
    //        }
    //    }

    //    System.Windows.Controls.Image _Icon;
    //    public System.Windows.Controls.Image Icon
    //    {
    //        get
    //        {
    //            return _Icon;
    //        }
    //        set
    //        {
    //            _Icon = value;
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;





    //}


}
