using Journal.Models.Abstractions;
using System.Collections.ObjectModel;

namespace Journal.Models;

internal class AddTaskModel : BindableBase, IAddTaskModel
{
    private ObservableCollection<string> _categories = new ObservableCollection<string>() { "Пофиксить кампутер", "Открыть аудиторию", "Сбегать за пивком", "Другое" };
    public ObservableCollection<string> Categories
    {
        get => _categories;
        set => SetProperty(ref _categories, value);
    }

    private ObservableCollection<string> _images = new ObservableCollection<string>() { "pc.png", "keys.png", "run.png", "other.png" };
    public ObservableCollection<string> Images
    {
        get => _images;
        set => SetProperty(ref _images, value);
    }

    private int _selectedIndex = 0;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }

    private string _description = "";
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private Size _windowSize;
    public Size WindowSize
    {
        get => _windowSize;
        set => SetProperty(ref _windowSize, value);
    }

    private string _clownClose;
    public string ClownClose
    {
        get => _clownClose;
        set => SetProperty(ref _clownClose, value);
    }
}