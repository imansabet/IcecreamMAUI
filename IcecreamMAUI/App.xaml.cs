using IcecreamMAUI.Services;
using IcecreamMAUI.ViewModels;

namespace IcecreamMAUI
{
    public partial class App : Application
    {
        private readonly AuthService _authService;
        private readonly CartViewModel _cartViewModel;

        public App(AuthService authService , CartViewModel cartViewModel)
        {
            InitializeComponent();
            authService.Initialize();
            _authService = authService;
            _cartViewModel = cartViewModel;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_authService));
        }
        protected override async void OnStart()
        {
            await _cartViewModel.InitialzeCartAsync();
        }
    }
}