using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using IcecreamMAUI.Pages;
using Refit;

namespace IcecreamMAUI.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    protected async Task GoToAsync(string url , bool animate = false) => 
        await Shell.Current.GoToAsync(url, animate);
    
    protected async Task GoToAsync(string url , bool animate ,IDictionary<string,object> parameters) => 
        await Shell.Current.GoToAsync(url, animate , parameters);
    protected async Task ShowErrorsAlertAsync(string errorMessage ) =>
        await Shell.Current.DisplayAlert("Error", errorMessage, "Ok");

    protected async Task ShowAlertAsync(string message) => await ShowAlertAsync("Alert", message);

    protected async Task ShowAlertAsync(string title,string message) =>
     await Shell.Current.DisplayAlert(title, message, "Ok");

    protected async Task ShowToastAsync(string message) =>  await Toast.Make(message).Show();
    protected async Task<bool> ConfirmAsync(string title, string message) 
        => await Shell.Current.DisplayAlert(title, message, "Yes", "No");

    protected async Task HandleApiExceptionAsync(ApiException ex , Action? signout = null )
    {
        if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await ShowAlertAsync("Session Expired .");

            signout?.Invoke();

            await GoToAsync($"//{nameof(OnboardingPage)}");
            return;
        }
        await ShowErrorsAlertAsync(ex.Message);
    }



}
