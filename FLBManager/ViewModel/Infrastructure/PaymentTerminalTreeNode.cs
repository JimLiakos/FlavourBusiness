using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WPFUIElementObjectBind;
using FlavourBusinessManager.ServicesContextResources;
using MenuDesigner.ViewModel;
using OOAdvantech.Transactions;
using StyleableWindow;
using System.Windows;

namespace FLBManager.ViewModel.Infrastructure
{
    /// <MetaDataID>{1b14c788-cf85-407f-b4d8-83d962a9b424}</MetaDataID>
    internal class PaymentTerminalTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        private PaymentTerminalsTreeNode paymentTerminalsTreeNode;
        internal IPaymentTerminal PaymentTerminal;

        public RelayCommand RenameCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand EditCommand { get; }

        public PaymentTerminalTreeNode(PaymentTerminalsTreeNode paymentTerminalsTreeNode, IPaymentTerminal paymentTerminal) : base(paymentTerminalsTreeNode)
        {
            this.paymentTerminalsTreeNode=paymentTerminalsTreeNode;
            this.PaymentTerminal=paymentTerminal;
            _Name=paymentTerminal.Description;

            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });


          
        }

  

        private void Delete()
        {
            paymentTerminalsTreeNode.RemovePaymentTerminal(this);
        }

        public override void SelectionChange()
        {
        }

        public override List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return ContextMenuItems;
                else
                    foreach (var treeNode in Members)
                    {
                        var contextMenuItems = treeNode.SelectedItemContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }

                return null;
            }
        }

        /// <exclude>Excluded</exclude>
        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();

        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var member = _Members.ToList();
                //if (GraphicMenuTreeNode!=null)
                //    member.Add(GraphicMenuTreeNode);
                return member;
            }
        }

        private void EditMode()
        {
            if (_Edit == true)
            {
                _Edit = !_Edit;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
        }

        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/pos.png"));
            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        public override string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                this.PaymentTerminal.Description= value;
                _Name=value;
                Task.Run(() =>
                {
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                });
            }
        }


        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;

        /// <MetaDataID>{5bec9819-de6e-4418-9535-3016bf4f934c}</MetaDataID>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    MenuCommand menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;
                    _ContextMenuItems.Add(menuItem);

                    //_ContextMenuItems.Add(null);

                    //menuItem = new MenuCommand();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/Key16.png"));
                    //menuItem.Header = Properties.Resources.AssignPreparationDevicePrompt;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AssignCommand;

                    //_ContextMenuItems.Add(menuItem);


                    //_ContextMenuItems.Add(null);

                    //menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/ServiceArea.png"));
                    //menuItem.Header = Properties.Resources.AddServiceAreaHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddServiceAreaCommand;

                    //_ContextMenuItems.Add(menuItem);

                    //MenuComamnd menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerIDLine16.png"));
                    //menuItem.Header = Properties.Resources.AddCallerIDLine;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddCallerIDLineCommand;
                    //_ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);



                }
                //if (_ContextMenuItems == null)
                //{
                //    _ContextMenuItems = new List<MenuComamnd>();
                //}
                return _ContextMenuItems;
            }
        }



    }
}
