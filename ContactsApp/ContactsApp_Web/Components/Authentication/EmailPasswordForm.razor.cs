using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using ContactsApp_Web.States;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json;

namespace ContactsApp_Web.Components.Authentication;

public partial class EmailPasswordForm
{
    [Inject] private AppState AppState { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorage { get; set; } = null!;
    
    /// If true, the form should register a user. Otherwise it should log them in
    [Parameter] public bool RegisterAction { get; set; }

    private readonly HttpClient _httpClient = new(); 

    private string? _email;
    private string? _password;

    private bool _isError = false;
    private bool _isSuccess = false;
    
    
    private async Task RegisterAsync()
    {
        if (_email == null || _password == null)
        {
            _isError = true;
            return;
        }

        _isError = false;
        _isSuccess = false;

        var registerModel = new RegisterRequest()
        {
            Email = _email,
            Password = _password
        };
        var response =
            await _httpClient.PostAsJsonAsync("http://host.docker.internal:8080/register", registerModel);
        CheckResponse(response); 
    }
    
    private async Task LoginAsync()
    {
        if (_email == null || _password == null)
        {
            _isError = true;
            return;
        }

        _isError = false;
        _isSuccess = false;

        var loginModel = new LoginRequest()
        {
            Email = _email,
            Password = _password
        };
        var response =
            await _httpClient.PostAsJsonAsync("http://host.docker.internal:8080/login", loginModel);
        CheckResponse(response);

        var responseModelJson = await response.Content.ReadAsStringAsync();
        var responseModel = JsonConvert.DeserializeObject<AccessTokenResponse>(responseModelJson);
        if (responseModel == null)
        {
            _isError = true;
            return;
        }
        await LocalStorage.SetItemAsync("accessTokenResponse", responseModel);
    }


    private void CheckResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            _isSuccess = true;
        else
            _isError = true;
    }
}