using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WPFUIElementObjectBind;

namespace MenuItemsEditor
{
    /// <MetaDataID>{0a000290-c139-4f55-8c87-590b0979ce92}</MetaDataID>
    public interface IMenusTreeNode
    {

        IMenusTreeNode Parent { set; get; }

        string Name
        {
            get;
            set;
        }

        bool IsEditable
        {
            get;
        }


        bool IsNodeExpanded
        {
            get; set;
        }

        System.Windows.Media.ImageSource TreeImage
        {
            get;
        }

        List<IMenusTreeNode> Members
        {
            get;
        }

        bool Edit
        {
            get;
            set;
            
        }

        bool IsSelected
        {
            get;set;
        }
        bool HasContextMenu
        {
            get;
        }


        List<MenuCommand> ContextMenuItems
        {
            get;
        }

        List<MenuCommand> SelectedItemContextMenuItems
        {
            get;
        }


    }
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
