using CommunityToolkit.Mvvm.ComponentModel;
using IcecreamMAUI.Services;
using IcecreamMAUI.Shared.Dtos;

namespace IcecreamMAUI.ViewModels;

public partial class HomeViewModel(IIcecreamsApi icecreamsApi , AuthService authService) : BaseViewModel
{
    private readonly IIcecreamsApi _icecreamsApi = icecreamsApi;
    private readonly AuthService _authService = authService;
   
    
    
    
    [ObservableProperty]
    private IcecreameDto[] _icecreams = [];

    [ObservableProperty]
    private string _userName = string.Empty;

    // Avoid Repeated Request
    private bool _isInitialized;
    public async Task InitializeAsync() 
    {
        UserName =  _authService.User!.Name;


        if (_isInitialized)
            return;

        IsBusy = true;
        try
        {
            // api call to fetch icecreams
            _isInitialized = true;
            Icecreams = await _icecreamsApi.GetIcecreamsAync();

        }
        catch(Exception ex) 
        {
            _isInitialized = false;
            await ShowErrorsAlertAsync(ex.Message);
        }
        finally 
        {
           IsBusy = false;
        }
    }

}
