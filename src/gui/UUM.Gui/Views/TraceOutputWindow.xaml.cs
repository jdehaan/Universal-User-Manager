using Catel.Windows;

namespace UUM.Gui.Views
{
    /// <summary>
    /// Interaction logic for TraceOutputWindow.xaml
    /// </summary>
    public partial class TraceOutputWindow
    {
        public TraceOutputWindow()
            : base(DataWindowMode.Custom, setOwnerAndFocus: false)
        {
            InitializeComponent();
        }
    }
}
