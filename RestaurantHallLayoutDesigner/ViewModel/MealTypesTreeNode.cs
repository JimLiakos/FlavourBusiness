using FLBManager.ViewModel;
using MenuItemsEditor.ViewModel;
using MenuModel;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace FloorLayoutDesigner.ViewModel
{
    /// <MetaDataID>{a7d4820a-b0ad-4e69-8887-a92bf937f61e}</MetaDataID>
    public class MealTypesTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        public System.Windows.Visibility CheckBoxVisibility { get; set; } = Visibility.Collapsed;

        ServiceAreaPresentation ServiceAreaPresentation;
        List<AssignedMealTypeViewMode> MealTypes;
        public MealTypesTreeNode(ServiceAreaPresentation parent) : base(parent)
        {
            ServiceAreaPresentation = parent;
            MealTypes = ServiceAreaPresentation.MealTypesViewModel.MealTypes.Select(x => new AssignedMealTypeViewMode(x.MealType, ServiceAreaPresentation, this)).ToList(); ;
            Name = "MealTypes";

            _Members = MealTypes.OfType<FBResourceTreeNode>().ToList();
            var _default = _Members.OfType<AssignedMealTypeViewMode>().Where(x => x.Assigned && x.Default).FirstOrDefault();
            if (_default != null)
            {
                _Members.Remove(_default);
                _Members.Insert(0, _default);
                _Members = _Members.ToList();
            }
            IsNodeExpanded = true;
            EditCommand = new RelayCommand((object sender) =>
            {


                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                MenuItemsEditor.Views.MealTypesWindow window = new MenuItemsEditor.Views.MealTypesWindow();
                window.Owner = win;


               // using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    window.GetObjectContext().SetContextInstance(this.ServiceAreaPresentation.MealTypesViewModel);
                    window.ShowDialog(); 
                 //   stateTransition.Consistent = true;
                }


                foreach (var mealType in ServiceAreaPresentation.MealTypesViewModel.MealTypes)
                {
                    if (MealTypes.Where(x => x.MealType == mealType.MealType).FirstOrDefault() == null)
                        MealTypes.Add(new AssignedMealTypeViewMode(mealType.MealType, ServiceAreaPresentation, this));
                }
                foreach (var assignedMealType in MealTypes.ToList())
                {
                    if (ServiceAreaPresentation.MealTypesViewModel.MealTypes.Where(x => x.MealType == assignedMealType.MealType).FirstOrDefault() == null)
                    {
                        if (assignedMealType.Assigned)
                            assignedMealType.Assigned = false;
                        MealTypes.Remove(assignedMealType);
                    }
                }
                ServiceAreaPresentation.RefreshMealTypes();
            });

        }



        public RelayCommand EditCommand { get; protected set; }

        public override string Name { get; set; }

        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/serve16.png"));
            }
        }

        List<FBResourceTreeNode> _Members;
        public override List<FBResourceTreeNode> Members
        {
            get => _Members;
        }

        /// <MetaDataID>{1f33bb95-2a33-4301-a821-22345003de4e}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{bc156757-a5f1-4ba9-b825-84e1bc92c46e}</MetaDataID>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };




                    MenuCommand menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/MealCoursesClock16.png")); ;
                    menuItem.Header = Properties.Resources.EditLabelMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;
                    _ContextMenuItems.Add(menuItem);



                }

                return _ContextMenuItems;
            }
        }


        public override bool HasContextMenu => true;

        public override List<MenuCommand> SelectedItemContextMenuItems => new List<MenuCommand>();

        public override void SelectionChange()
        {

        }

        internal void RefreshMealTypes()
        {

            _Members = MealTypes.OfType<FBResourceTreeNode>().ToList();
            foreach (var assignedMealTypeViewMode in MealTypes)
                assignedMealTypeViewMode.RefreshMealType();

            var _default = _Members.OfType<AssignedMealTypeViewMode>().Where(x => x.Default).FirstOrDefault();
            if (_default != null)
            {
                _Members.Remove(_default);
                _Members.Insert(0, _default);
                _Members = _Members.ToList();
            }


            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }
    }
}
