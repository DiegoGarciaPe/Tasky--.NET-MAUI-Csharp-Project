using System.ComponentModel;
using Tasker.MVVM.Models;
using Tasker.MVVM.ViewModels;

namespace Tasker.MVVM.Views;

public partial class MainView : ContentPage
{
    private MainViewModel mainViewModel = new MainViewModel();
    public MainView()
    {
        InitializeComponent();
        BindingContext = mainViewModel; // Usa la misma instancia
    }

    private void checkBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        mainViewModel.UpdateData();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var taskView = new NewTaskView()
        {
            BindingContext = new NewTaskViewModel()
            {
                Tasks = mainViewModel.Tasks,
                Categories = mainViewModel.Categories
            }
        };

        // Suscribir al evento CollectionChanged para nuevas tareas
        mainViewModel.Tasks.CollectionChanged += (s, args) =>
        {
            if (args.NewItems != null)
            {
                foreach (MyTask newTask in args.NewItems)
                {
                    ((INotifyPropertyChanged)newTask).PropertyChanged += mainViewModel.Task_PropertyChanged;
                }
            }
        };

        Navigation.PushAsync(taskView);
    }
}