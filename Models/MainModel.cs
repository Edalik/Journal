using Journal.Models.Abstractions;
using System.Collections.ObjectModel;

namespace Journal.Models;

class MainModel : BindableBase, IMainModel
{
    private ObservableCollection<ClownTask> _clownTasks = new ObservableCollection<ClownTask>();
    public ObservableCollection<ClownTask> ClownTasks
    {
        get => _clownTasks;
        set => SetProperty(ref _clownTasks, value);
    }

    private ObservableCollection<ClownTask> _filteredClownTasks;
    public ObservableCollection<ClownTask> FilteredClownTasks
    {
        get => _filteredClownTasks;
        set => SetProperty(ref _filteredClownTasks, value);
    }

    private ClownTask _selectedTask;
    public ClownTask SelectedTask
    {
        get => _selectedTask;
        set => SetProperty(ref _selectedTask, value);
    }

    private string _taskDescription;
    public string TaskDescription
    {
        get => _taskDescription;
        set => SetProperty(ref _taskDescription, value);
    }

    private string _profilePicture = "";
    public string ProfilePicture
    {
        get => _profilePicture;
        set => SetProperty(ref _profilePicture, value);
    }

    private string _headerText = "";
    public string HeaderText
    {
        get => _headerText;
        set => SetProperty(ref _headerText, value);
    }

    private int _tabSelected = 0;
    public int TabSelected
    {
        get => _tabSelected;
        set => SetProperty(ref _tabSelected, value);
    }

    private DateTime _date = DateTime.Now;
    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private string _button1Text = "Добавить";
    public string Button1Text
    {
        get => _button1Text;
        set => SetProperty(ref _button1Text, value);
    }

    private string _button2Text = "Удалить";
    public string Button2Text
    {
        get => _button2Text;
        set => SetProperty(ref _button2Text, value);
    }

    private bool _isVisible = true;
    public bool IsVisible
    {
        get => _isVisible;
        set => SetProperty(ref _isVisible, value);
    }
}