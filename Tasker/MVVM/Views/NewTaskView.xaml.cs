using Tasker.MVVM.Models;
using Tasker.MVVM.ViewModels;

namespace Tasker.MVVM.Views;

public partial class NewTaskView : ContentPage
{
    public NewTaskView()
    {
        InitializeComponent();
    }

    private void AddTaskClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as NewTaskViewModel;

        if (string.IsNullOrWhiteSpace(vm.Task))
        {
            DisplayAlert("Error", "El nombre de la tarea no puede estar vacío", "OK");
            return;
        }

        if (vm.Task.Length > 30)
        {
            DisplayAlert("Error", "El nombre de la tarea no puede tener más de 30 caracteres", "OK");
            return;
        }

        var selectedCategory = vm.Categories.Where(x => x.IsSelected == true).FirstOrDefault();

        if (selectedCategory != null)
        {
            var task = new MyTask
            {
                TaskName = vm.Task,
                CategoryId = selectedCategory.Id
            };
            vm.Tasks.Add(task);
            vm.Task = string.Empty;
            foreach (var category in vm.Categories)
            {
                category.IsSelected = false;
            }
            DisplayAlert("Agregado", "Tarea añadida correctamente", "OK");
            Navigation.PopAsync();
        }
        else
        {
            DisplayAlert("Error", "Por favor, selecciona una categoría", "OK");
        }
    }

    private async void AddCategoryClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as NewTaskViewModel;

        string category = await DisplayPromptAsync("Nueva categoría", "Introduce el nombre de la nueva categoría", keyboard: Keyboard.Text);

        var r = new Random();

        if (!string.IsNullOrEmpty(category))
        {
            vm.Categories.Add(new Category
            {
                Id = vm.Categories.Any() ? vm.Categories.Max(x => x.Id) + 1 : 1,
                CategoryName = category,
                Color = Color.FromRgb(r.Next(255), r.Next(255), r.Next(255)).ToHex()
            });
        }
    }
}