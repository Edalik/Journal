using Journal.Models.Abstractions;

namespace Journal.Models;

class LoginModel : BindableBase, ILoginModel
{
    private string _ip;
    public string Ip
    {
        get => _ip;
        set => SetProperty(ref _ip, value);
    }

    private string _login;
    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }
}