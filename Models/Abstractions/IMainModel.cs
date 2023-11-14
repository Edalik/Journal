using System.Collections.ObjectModel;

namespace Journal.Models.Abstractions;

internal interface IMainModel
{
    ObservableCollection<ClownTask> ClownTasks { get; set; }
    ObservableCollection<ClownTask> FilteredClownTasks { get; set; }
    ClownTask SelectedTask { get; set; }
    string TaskDescription { get; set; }
    string ProfilePicture { get; set; }
    string HeaderText { get; set; }
    int TabSelected { get; set; }
    DateTime Date { get; set; }
    string Button1Text { get; set; }
    string Button2Text { get; set; }
    bool IsVisible { get; set; }
}