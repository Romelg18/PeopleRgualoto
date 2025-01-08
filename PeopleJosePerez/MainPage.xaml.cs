using PeopleRgualoto.ViewModels;

namespace PeopleRgualoto
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModelRgualoto();
        }
    }
}