using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IcecreamMAUI.Pages;
using IcecreamMAUI.Services;
using IcecreamMAUI.Shared.Dtos;

namespace IcecreamMAUI.ViewModels;

public partial class AuthViewMode(IAuthApi authApi) : BaseViewModel
{
    private readonly IAuthApi _authApi;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignup))]
    private string? _name;

    [ObservableProperty  , NotifyPropertyChangedFor(nameof(CanSignin)) , NotifyPropertyChangedFor(nameof(CanSignup))]
    private string? _password;
    
    [ObservableProperty , NotifyPropertyChangedFor(nameof(CanSignin)) , NotifyPropertyChangedFor(nameof(CanSignin))]
    private string? _email;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignup))]
    private string? _address;
    
    public bool CanSignin =>
       !string.IsNullOrEmpty(Password)
      && !string.IsNullOrEmpty(Email);



    public bool CanSignup => CanSignin
        && !string.IsNullOrEmpty(Password)
        && !string.IsNullOrEmpty(Email);


    [RelayCommand]
    private async Task SignupAsync() 
    {
        IsBusy = true;
        try
        {
            var signupDto = new SignupRequestDto(Name, Email, Password, Address);
            // Make Api Call
            var result = await _authApi.SignupAsync(signupDto);
            if (result.IsSuccess)
            {
                await ShowAlertAsync(result.Data.Token);

                // Navigate toHome page 
                await GoToAsync($"//{nameof(HomePage)}", animate: true);
            }
            else
            {
                await ShowErrorsAlertAsync(result.ErrorMessage ?? "Unknown Error Occured . ");
            }
        }
        catch (Exception ex)
        {
            await ShowErrorsAlertAsync(ex.Message);

        }
        finally 
        {
            IsBusy = false;
        }
    }
    
    
    [RelayCommand]
    private async Task SigninAsync() 
    {
        IsBusy = true;
        try
        {
            var signinDto = new SigninRequestDto (Email, Password);
            // Make Api Call
            var result = await _authApi.SigninAsync(signinDto);
            if (result.IsSuccess)
            {
                await ShowAlertAsync(result.Data.User.Name);
                // Navigate toHome page 
                await GoToAsync($"//{nameof(HomePage)}", animate: true);
            }
            else
            {
                await ShowErrorsAlertAsync(result.ErrorMessage ?? "Unknown Error Occured . ");
            }
        }
        catch (Exception ex)
        {
            await ShowErrorsAlertAsync(ex.Message);

        }
        finally
        {
            IsBusy = false;
        }
    }

}
