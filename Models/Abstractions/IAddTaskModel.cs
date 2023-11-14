using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace Journal.Models.Abstractions;

internal interface IAddTaskModel
{
    ObservableCollection<string> Categories { get; set; }
    ObservableCollection<string> Images { get; set; }
    int SelectedIndex {  get; set; }
    string Description { get; set; }
    Size WindowSize { get; set; }
    string ClownClose { get; set; }
}