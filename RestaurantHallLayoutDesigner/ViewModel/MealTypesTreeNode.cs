using FLBManager.ViewModel;
using MenuModel;
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

        public System.Windows.Visibility CheckBoxVisibility { get; set; } = Visibility.Collapsed;

        ServiceAreaPresentation ServiceAreaPresentation;
        List<AssignedMealTypeViewMode> MealTypes;
        public MealTypesTreeNode(List<IMealType> mealTypes, ServiceAreaPresentation parent) : base(parent)
        {
            ServiceAreaPresentation = parent;
            MealTypes = mealTypes.Select(x => new AssignedMealTypeViewMode(x, ServiceAreaPresentation, this)).ToList(); ;
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

        }

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

        public override List<MenuCommand> ContextMenuItems => new List<MenuCommand>();

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
