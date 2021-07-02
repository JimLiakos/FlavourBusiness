using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLBManager.ViewModel
{
    /// <MetaDataID>{c42cd5e8-46a0-4bd7-9cea-2af75b43b176}</MetaDataID>
    public abstract class FBResourceTreeNode : MarshalByRefObject, INotifyPropertyChanged
    {

        /// <MetaDataID>{db5cf529-d7e1-4ac7-a73c-f2a063b71ee2}</MetaDataID>
        public Dictionary<string, List<FBResourceTreeNode>> FBResourceTreeNodesDictionary = new Dictionary<string, List<FBResourceTreeNode>>();

        /// <MetaDataID>{cbaca0ae-61da-4dac-b2b0-6b6361243943}</MetaDataID>
        public FBResourceTreeNode(FBResourceTreeNode parent)
        {
            _Parent = parent;
        }



        /// <MetaDataID>{338fc1c4-6c57-46ce-8045-f87fc32fc60b}</MetaDataID>
        protected void RunPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _PropertyChanged?.Invoke(sender, e);
        }
        /// <MetaDataID>{fe7a7368-320c-43c5-a270-1ef440b87d6f}</MetaDataID>
        public abstract void SelectionChange();
        /// <MetaDataID>{22480628-7def-43af-b306-cbeb3a831305}</MetaDataID>
        public virtual FBResourceTreeNode HeaderNode
        {
            get
            {
                if (Parent != null)
                    return Parent.HeaderNode;
                else
                    return this;
            }

        }

        /// <exclude>Excluded</exclude> 
        protected FBResourceTreeNode _Parent;
        /// <MetaDataID>{4e8d01a8-e486-4402-bea7-187805f9f520}</MetaDataID>
        public virtual FBResourceTreeNode Parent
        {
            set
            {
                _Parent = value;
            }
            get
            {
                return _Parent;
            }
        }

        /// <MetaDataID>{e6e8605e-21fd-4d87-a39a-ed5ad34a6fc6}</MetaDataID>
        public abstract string Name
        {
            get;
            set;
        }

        /// <MetaDataID>{13faa6b2-7048-4035-9d0e-9791cc53c652}</MetaDataID>
        public virtual bool IsEditable
        {
            get
            {
                return false;
            }
        }


        /// <exclude>Excluded</exclude>
        protected bool _IsNodeExpanded;
        /// <MetaDataID>{e7d8600d-a61a-4e57-98ed-d76f4f3ce4bc}</MetaDataID>
        public virtual bool IsNodeExpanded
        {
            get
            {
                return _IsNodeExpanded;
            }
            set
            {
                _IsNodeExpanded = value;
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
            }
        }

        /// <MetaDataID>{c1e2b6a3-25e5-4d65-8c60-40ede5691271}</MetaDataID>
        public abstract System.Windows.Media.ImageSource TreeImage
        {
            get;
        }

        /// <MetaDataID>{43ad83d6-d3cd-431c-b7c2-ced872b11389}</MetaDataID>
        public abstract List<FBResourceTreeNode> Members
        {
            get;
        }


        /// <exclude>Excluded</exclude>
        protected bool _Edit;
        /// <MetaDataID>{90b67577-84d4-47af-b849-b9ff670ab35c}</MetaDataID>
        public virtual bool Edit
        {
            get
            {
                return _Edit;
            }
            set
            {
                _Edit = value;
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }

        /// <exclude>Excluded</exclude>
        protected bool _IsSelected;

        protected event PropertyChangedEventHandler _PropertyChanged;
        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _PropertyChanged += value;
            }
            remove
            {
                _PropertyChanged -= value;
            }
        }

        /// <MetaDataID>{7c90fb35-2432-4969-bebe-335e5b28cd17}</MetaDataID>
        public virtual bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    HeaderNode.SelectionChange();
                    _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }
        /// <MetaDataID>{26172323-f8ab-42f9-98ba-3cbe6af5ecb8}</MetaDataID>
        public virtual bool HasContextMenu
        {
            get
            {
                return false;
            }
        }


        /// <MetaDataID>{0c5958ea-2f11-4a93-9952-65f4eed4de36}</MetaDataID>
        public abstract List<WPFUIElementObjectBind.MenuCommand> ContextMenuItems
        {
            get;
        }

        /// <MetaDataID>{1a65e203-0eb3-4341-af15-c0e01ea7e7d7}</MetaDataID>
        public abstract List<WPFUIElementObjectBind.MenuCommand> SelectedItemContextMenuItems
        {
            get;
        }


    }




}
