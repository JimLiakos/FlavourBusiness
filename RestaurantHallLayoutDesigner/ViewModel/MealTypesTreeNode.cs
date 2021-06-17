using FLBManager.ViewModel;
using MenuModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace FloorLayoutDesigner.ViewModel
{
    /// <MetaDataID>{a7d4820a-b0ad-4e69-8887-a92bf937f61e}</MetaDataID>
    public class MealTypesTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {

        ServiceAreaPresentation ServiceAreaPresentation;
        List<IMealType> MealTypes;
        public MealTypesTreeNode(List<IMealType> mealTypes, ServiceAreaPresentation parent) : base(parent)
        {
            ServiceAreaPresentation = parent;
            MealTypes = mealTypes;
            Name = "MealTypes";

            _Members = mealTypes.Select(x => new AssignedMealTypeViewMode(x, ServiceAreaPresentation.ServiceArea, this)).OfType<FBResourceTreeNode>().ToList();

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
    }
}
