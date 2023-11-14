using Journal.Models;
using Journal.Models.Abstractions;
using Journal.Views;
using ReactiveUI;
using SuperSimpleTcp;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace Journal.ViewModels
{
    class LoginViewModel
    {
        public ILoginModel Model { get; }
        public ICommand LoginCommand { get; }
        public LoginViewModel()
        {
            Model = new LoginModel();

            LoginCommand = ReactiveCommand.Create
            (
                () =>
                {
                    if (!Server.isConnected)
                    {
                        try
                        {
                            Server.server = new SimpleTcpClient(Model.Ip + ":1111");
                            Server.server.Events.DataReceived += LoginDataReceived;
                            Server.server.Events.Connected += LoginConnected;
                            Server.server.Events.Disconnected += LoginDisconnected;
                            Server.server.Connect();
                        }
                        catch
                        {
                            Application.Current?.Dispatcher.Dispatch(() => Application.Current.MainPage.DisplayAlert("Ошибка!", "Не удалось подключиться к серверу!", "Ладно🤙"));
                            return;
                        }
                    }
                    Server.user.login = Model.Login;
                    Server.user.password = Model.Password;
                    Server.server.Send($"login|{JsonSerializer.Serialize(Server.user)};");
                }
            );
        }

        private void LoginDisconnected(object sender, ConnectionEventArgs e)
        {
            Server.isConnected = false;
        }

        private void LoginConnected(object sender, ConnectionEventArgs e)
        {
            Server.isConnected = true;
        }

        public async void LoginDataReceived(object sender, DataReceivedEventArgs e)
        {
            await Task.Run(() =>
            {
                string[] message = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count).Split(";");
                foreach (string s in message)
                {
                    string[] response = s.Split("|");
                    switch (response[0])
                    {
                        case "ip":
                            Server.user.ip = response[1];
                            break;
                        case "login succesful":
                            Server.user = JsonSerializer.Deserialize<UserInfo>(response[1]);
                            Application.Current?.Dispatcher.Dispatch(() => Shell.Current.GoToAsync(nameof(MainView)));
                            break;
                        case "wrong password":
                            Application.Current?.Dispatcher.Dispatch(() => Application.Current.MainPage.DisplayAlert("Ошибка!", "Неверный пароль!", "Ладно🤙"));
                            break;
                        case "wrong login":
                            Application.Current?.Dispatcher.Dispatch(() => Application.Current.MainPage.DisplayAlert("Ошибка!", "Неверный логин!", "Ладно🤙"));
                            break;
                    }
                }
            });            
        }
    }
}