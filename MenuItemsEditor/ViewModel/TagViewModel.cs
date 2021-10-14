using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{a5029aea-1f16-402d-8b23-3e958c492af4}</MetaDataID>
    public class TagViewModel : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, INotifyPropertyChanged
    {

        public TagViewModel(string name, int index)
        {
            Name = name;
            orgName = name;
            Index = index;
            DeleteTagCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                TagDeleted?.Invoke(this);
            });
        }
        string orgName;
        public string Name { get; set; }

        public int Index;

        public WPFUIElementObjectBind.RelayCommand DeleteTagCommand { get; protected set; }

        /// <exclude>Excluded</exclude>
        bool _Edit;

        public bool Edit
        {
            get => _Edit;

            internal set
            {
                _Edit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
                if (value == false && Name != orgName)
                {
                    orgName = Name;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    NameChanged?.Invoke(this);
                }
            }
        }

        public event TagDeletedHandle TagDeleted;
        public event NameChangedHandle NameChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public delegate void TagDeletedHandle(TagViewModel tag);
    public delegate void NameChangedHandle(TagViewModel tag);
}
