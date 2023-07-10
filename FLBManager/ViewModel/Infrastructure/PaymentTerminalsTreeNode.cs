using FinanceFacade;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Infrastructure
{
    /// <MetaDataID>{ab94e964-b0c3-4507-bd39-69539e1db3ec}</MetaDataID>
    internal class PaymentTerminalsTreeNode :  FBResourceTreeNode, INotifyPropertyChanged
    {
        /// <MetaDataID>{1652ece1-2088-4a7f-b5c4-1c644594b20a}</MetaDataID>
        public InfrastructureTreeNode ServiceContextInfrastructure { get; }
        /// <MetaDataID>{05a0b69e-56c3-4434-8b80-0eedb748f7c2}</MetaDataID>
        public RelayCommand NewPaymentTerminalCommand { get; }

        /// <MetaDataID>{7970d312-2d14-4fe4-a88f-1f506fca2dbc}</MetaDataID>
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        Dictionary<IPaymentTerminal, PaymentTerminalTreeNode> PaymentTerminals = new Dictionary<IPaymentTerminal, PaymentTerminalTreeNode>();

        /// <MetaDataID>{15c2a0dc-4595-42f4-8d77-bd29f508987b}</MetaDataID>
        public PaymentTerminalsTreeNode(InfrastructureTreeNode parent) : base(parent)
        {
            ServiceContextInfrastructure = parent;

            NewPaymentTerminalCommand = new RelayCommand((object sender) =>
            {
                NewPaymentTerminal();
            });



            try
            {


                foreach (var paymentTerminal in ServiceContextInfrastructure.ServiceContextResources.PaymentTerminals)
                    PaymentTerminals.Add(paymentTerminal, new PaymentTerminalTreeNode(this, paymentTerminal));

            }
            catch (System.Exception error)
            {
            }
        }

        /// <MetaDataID>{6746b8bb-2698-4244-9c08-b33d61dd6325}</MetaDataID>
        private void NewPaymentTerminal()
        {
            var paymentTerminal = ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.NewPaymentTerminal();
            var paymentTerminalPresentation = new PaymentTerminalTreeNode(this, paymentTerminal);
            paymentTerminalPresentation.Edit = true;
            PaymentTerminals.Add(paymentTerminal, paymentTerminalPresentation);


            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            IsNodeExpanded = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
        }

        internal void RemovePaymentTerminal(PaymentTerminalTreeNode paymentTerminalTreeNode)
        {


            try
            {
                this.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.RemovePaymentTerminal(paymentTerminalTreeNode.PaymentTerminal);
                _Members.Remove(paymentTerminalTreeNode);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            }
            catch (Exception error)
            {


            }

        }


        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();
                    MenuCommand menuItem = new MenuCommand(); ;
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/pos-terminal16.png"));
                    menuItem.Header = Properties.Resources.NewTakeAwayStationPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewPaymentTerminalCommand;
                    _ContextMenuItems.Add(menuItem);
                }

                return _ContextMenuItems;
            }
        }

        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return "Payment Terminals";
            }

            set
            {

            }
        }


        /// <exclude>Excluded</exclude>
        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return _Members.ToList();
            }
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

        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/cards.png"));
            }
        }

        public override void SelectionChange()
        {

        }
    }
}
