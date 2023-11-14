using CommunityToolkit.Maui.Views;

namespace Journal.Views;

public partial class AddTaskView : Popup
{
	public AddTaskView()
	{
		InitializeComponent();
	}

    private void Close(object sender, TextChangedEventArgs e)
    {
        Close();
    }
}