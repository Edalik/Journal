using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using Journal.Models;
using Journal.Models.Abstractions;
using Journal.Views;
using Plugin.LocalNotification;
using ReactiveUI;
using SuperSimpleTcp;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace Journal.ViewModels;

class MainViewModel
{
    public ICommand TasksCommand { get; }
    public ICommand InProgressCommand { get; }
    public ICommand FinishedCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand Button1Command { get; }
    public ICommand Button2Command { get; }
    public ICommand SelectionChangedCommand { get; }
    public IMainModel Model { get; }
    public MainViewModel()
    {
        Model = new MainModel();

        Model.ProfilePicture = Server.user.image;

        Server.server.Events.DataReceived += DataReceived;
        Server.server.Events.Connected += Connected;
        Server.server.Events.Disconnected += Disconnected;
        Model.HeaderText = Server.user.surname + " " + Server.user.name[0] + ". " + Server.user.fathername[0] + ".";
        if (Server.user.role.Equals("worker"))
        {
            Model.IsVisible = false;
            Model.Button1Text = "Взять задачу";
        }

        Server.server.Send($"get tasks|{JsonSerializer.Serialize(Server.user)};");

        TasksCommand = ReactiveCommand.Create
        (
            () =>
            {
                Model.TabSelected = 0;

                if (Server.user.role.Equals("teacher"))
                {
                    Model.IsVisible = true;
                    Model.Button1Text = "Добавить";
                    Model.Button2Text = "Удалить";
                }
                else
                {
                    Model.Button1Text = "Взять задачу";
                }

                Model.FilteredClownTasks = Model.ClownTasks?.Where(a => a.status.Equals("awaiting")).ToObservableCollection();
            }
        );

        InProgressCommand = ReactiveCommand.Create
        (
            () =>
            {
                Model.TabSelected = 1;
                Model.IsVisible = false;

                if (Server.user.role.Equals("teacher"))
                {
                    Model.Button1Text = "Пнуть техника";
                }
                else
                {
                    Model.Button1Text = "Задача выполнена";
                }

                if (Server.user.role.Equals("worker"))
                    Model.FilteredClownTasks = Model.ClownTasks?.Where(a => a.status.Equals("in_progress") && Server.user.user_id == a.worker_id).ToObservableCollection();
                else
                    Model.FilteredClownTasks = Model.ClownTasks?.Where(a => a.status.Equals("in_progress")).ToObservableCollection();
            }
        );

        FinishedCommand = ReactiveCommand.Create
        (
            () =>
            {
                Model.TabSelected = 2;
                Model.IsVisible = false;
                Model.Button1Text = "Удалить";

                Model.FilteredClownTasks = Model.ClownTasks?.Where(a => a.status.Equals("finished")).ToObservableCollection();
            }
        );

        Button1Command = ReactiveCommand.Create
        (
            () =>
            {
                if (!Server.isConnected)
                {
                    try
                    {
                        Server.server = new SimpleTcpClient(Server.ip + ":1111");
                        Server.server.Events.DataReceived += DataReceived;
                        Server.server.Events.Connected += Connected;
                        Server.server.Events.Disconnected += Disconnected;
                        Server.server.Connect();
                    }
                    catch
                    {
                        Application.Current?.Dispatcher.Dispatch(() => Application.Current.MainPage.DisplayAlert("Ошибка!", "Не удалось подключиться к серверу!", "Ладно🤙"));
                        return;
                    }
                }

                switch (Model.Button1Text)
                {
                    case "Добавить":
                        Application.Current.MainPage.ShowPopupAsync(new AddTaskView());
                        break;
                    case "Взять задачу":
                        if (Model.SelectedTask != null)
                        {
                            ClownTask takeTask = Model.SelectedTask;
                            takeTask.worker_id = Server.user.user_id;
                            Server.server.Send($"take task|{JsonSerializer.Serialize(takeTask)};");
                        }
                        break;
                    case "Пнуть техника":
                        if (Model.SelectedTask != null)
                            Server.server.Send($"kick|{JsonSerializer.Serialize(Model.SelectedTask)};");
                        break;
                    case "Удалить":
                        if (Model.SelectedTask != null)
                            Server.server.Send($"del task|{JsonSerializer.Serialize(Model.SelectedTask)};");
                        break;
                    case "Задача выполнена":
                        if (Model.SelectedTask != null)
                        {
                            ClownTask finishTask = Model.SelectedTask;
                            finishTask.second_date = DateTime.Now.ToString("d.M.yyyy H:mm:ss");
                            Server.server.Send($"release task|{JsonSerializer.Serialize(finishTask)};");
                        }
                        break;
                }
            }
        );

        Button2Command = ReactiveCommand.Create
        (
            () =>
            {

                if (!Server.isConnected)
                {
                    try
                    {
                        Server.server = new SimpleTcpClient(Server.ip + ":1111");
                        Server.server.Events.DataReceived += DataReceived;
                        Server.server.Events.Connected += Connected;
                        Server.server.Events.Disconnected += Disconnected;
                        Server.server.Connect();
                    }
                    catch
                    {
                        Application.Current?.Dispatcher.Dispatch(() => Application.Current.MainPage.DisplayAlert("Ошибка!", "Не удалось подключиться к серверу!", "Ладно🤙"));
                        return;
                    }
                }

                if (Model.SelectedTask != null)
                    Server.server.Send($"del task|{JsonSerializer.Serialize(Model.SelectedTask)};");
            }
        );

        LogoutCommand = ReactiveCommand.Create
        (
            () =>
            {
                try
                {
                    Server.server.Send($"logout|{JsonSerializer.Serialize(Server.user)};");
                    Server.server.Disconnect();
                }
                catch
                {

                }

                Shell.Current.GoToAsync("..");
            }
        );
    }

    private void Disconnected(object sender, ConnectionEventArgs e)
    {
        Server.isConnected = false;
    }

    private void Connected(object sender, ConnectionEventArgs e)
    {
        Server.isConnected = true;
    }

    public async void DataReceived(object sender, DataReceivedEventArgs e)
    {
        Server.messageBuffer += Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
        if (Server.messageBuffer.EndsWith(";") || Server.messageBuffer.EndsWith("|"))
        {
            if (!Server.messageBuffer.StartsWith("update tasks") && !Server.messageBuffer.StartsWith("kick"))
            {
                Server.messageBuffer = "";
                Server.server.Send($"get tasks|{JsonSerializer.Serialize(Server.user)};");
                return;
            }
            string[] message = Server.messageBuffer.Split(";");
            Server.messageBuffer = "";
            await Task.Run(() =>
            {
                foreach (string s in message)
                {
                    string[] response = s.Split("|");
                    switch (response[0])
                    {
                        case "update tasks":

                            try
                            {
                                Model.ClownTasks = JsonSerializer.Deserialize<ObservableCollection<ClownTask>>(response[1]);
                                switch (Model.TabSelected)
                                {
                                    case 0:
                                        Model.FilteredClownTasks = Model.ClownTasks?.Where(a => a.status.Equals("awaiting")).ToObservableCollection();
                                        break;
                                    case 1:
                                        if (Server.user.role.Equals("worker"))
                                            Model.FilteredClownTasks = Model.ClownTasks?.Where(a => a.status.Equals("in_progress") && Server.user.user_id == a.worker_id).ToObservableCollection();
                                        else
                                            Model.FilteredClownTasks = Model.ClownTasks?.Where(a => a.status.Equals("in_progress")).ToObservableCollection();
                                        break;
                                    case 2:
                                        Model.FilteredClownTasks = Model.ClownTasks?.Where(a => a.status.Equals("finished")).ToObservableCollection();
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                Application.Current?.Dispatcher.Dispatch(() => Application.Current.MainPage.DisplayAlert("Ошибка!", $"Не удалось получить список задач, выполнится повторный запрос", "Ладно🤙"));
                                Server.server.Send($"get tasks|{JsonSerializer.Serialize(Server.user)};");
                            }

                            break;

                        case "kick":

                            var request = new NotificationRequest()
                            {
                                Title = "Пора поработать!🤙",
                                Image =
                                {
                                    ResourceName = "kick"
                                }
                            };

                            LocalNotificationCenter.Current.Show(request);

                            break;
                    }
                }
            });
        }
    }
}
