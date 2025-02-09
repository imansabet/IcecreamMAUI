﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace IcecreamMAUI.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    protected async Task GoToAsync(string url , bool animate = false) => 
        await Shell.Current.GoToAsync(url, animate);
    protected async Task ShowErrorsAlertAsync(string errorMessage ) =>
        await Shell.Current.DisplayAlert("Error", errorMessage, "Ok");

    protected async Task ShowAlertAsync(string message) =>
     await Shell.Current.DisplayAlert("Alert", message, "Ok");

}
