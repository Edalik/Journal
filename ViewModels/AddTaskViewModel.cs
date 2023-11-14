using CommunityToolkit.Maui.Views;
using Journal.Models;
using Journal.Models.Abstractions;
using Journal.Views;
using ReactiveUI;
using SuperSimpleTcp;
using System.Text.Json;
using System.Windows.Input;

namespace Journal.ViewModels;

internal class AddTaskViewModel
{
    public IAddTaskModel Model { get; }

    public ICommand AddTaskCommand { get; }

    public AddTaskViewModel()
    {
        Model = new AddTaskModel();

        Model.WindowSize = new Size(DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density * 0.8, DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density * 0.6);

        AddTaskCommand = ReactiveCommand.Create
        (
            () =>
            {
                if (!Server.isConnected)
                {
                    try
                    {
                        Server.server = new SimpleTcpClient(Server.ip + ":1111");
                        Server.server.Connect();
                    }
                    catch
                    {
                        Application.Current?.Dispatcher.Dispatch(() => Application.Current.MainPage.DisplayAlert("Ошибка!", "Не удалось подключиться к серверу!", "Ладно🤙"));
                        return;
                    }
                }

                ClownTask newTask = new ClownTask()
                {
                    first_date = DateTime.Now.ToString("d.M.yyyy H:mm:ss"),
                    teacher_id = Server.user.user_id,
                    name = Model.Categories[Model.SelectedIndex],
                    text = Model.Description.Replace("|", "/").Replace(";", ","),
                    image = Model.Images[Model.SelectedIndex],
                    status = "awaiting"
                };

                Server.server.Send($"add task|{JsonSerializer.Serialize(newTask)};");

                Model.ClownClose = "Clown";
            }
        );
    }
}