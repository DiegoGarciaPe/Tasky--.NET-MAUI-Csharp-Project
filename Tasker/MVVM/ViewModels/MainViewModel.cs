using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tasker.MVVM.Models;

namespace Tasker.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        public ObservableCollection<Category>? Categories { get; set; }
        public ObservableCollection<MyTask>? Tasks { get; set; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand DeleteCategoryCommand { get; }


        public MainViewModel()
        {
            FillData();

            DeleteTaskCommand = new Command<MyTask>(DeleteTask);
            DeleteCategoryCommand = new Command<Category>(DeleteCategory);

            foreach (var t in Tasks)
            {
                ((INotifyPropertyChanged)t).PropertyChanged += Task_PropertyChanged;
            }
            Tasks.CollectionChanged += Task_CollectionChanged;
        }

        private void DeleteTask(MyTask task)
        {
            if (task != null)
            {
                Tasks.Remove(task);
                UpdateData();
            }
        }

        private void DeleteCategory(Category category)
        {
            if (category != null)
            {
                Categories.Remove(category);
                UpdateData();
            }
        }

        private void Task_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //No funciona, revisar
            UpdateData();
        }

        public void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MyTask.Completed))
            {
                UpdateData();
            }
        }

        private void FillData()
        {
            Categories = new ObservableCollection<Category>
                {
                    new Category() { Id = 1, CategoryName = "Personal", Color = "#FFB3BA" },
                    new Category() { Id = 2, CategoryName = "Trabajo", Color = "#B3E5FC" },
                    new Category() { Id = 3, CategoryName = "Compras", Color = "#FFECB3" },
                    new Category() { Id = 4, CategoryName = "Otros", Color = "#C8E6C9" }
                };
            Tasks = new ObservableCollection<MyTask>
                {
                    new MyTask() { TaskName = "Comprar comestibles", CategoryId = 1, Completed = false },
                    new MyTask() { TaskName = "Terminar informe del proyecto", CategoryId = 2, Completed = true },
                    new MyTask() { TaskName = "Llamar a mamá", CategoryId = 1, Completed = false },
                    new MyTask() { TaskName = "Preparar presentación", CategoryId = 2, Completed = false },
                    new MyTask() { TaskName = "Comprar zapatos nuevos", CategoryId = 3, Completed = false },
                    new MyTask() { TaskName = "Leer un libro", CategoryId = 4, Completed = true },
                    new MyTask() { TaskName = "Planear vacaciones", CategoryId = 1, Completed = false },
                    new MyTask() { TaskName = "Asistir a la reunión", CategoryId = 2, Completed = true },
                    new MyTask() { TaskName = "Comprar regalo de cumpleaños", CategoryId = 3, Completed = false },
                    new MyTask() { TaskName = "Limpiar la casa", CategoryId = 4, Completed = false },
                    new MyTask() { TaskName = "Organizar archivos", CategoryId = 2, Completed = true },
                    new MyTask() { TaskName = "Salir a caminar", CategoryId = 1, Completed = false },
                    new MyTask() { TaskName = "Hacer ejercicio", CategoryId = 1, Completed = false },
                    new MyTask() { TaskName = "Enviar correos electrónicos", CategoryId = 2, Completed = true },
                    new MyTask() { TaskName = "Visitar al doctor", CategoryId = 1, Completed = false },
                    new MyTask() { TaskName = "Revisar documentos", CategoryId = 2, Completed = false },
                    new MyTask() { TaskName = "Reservar boletos de avión", CategoryId = 3, Completed = true }
                };

            UpdateData();
        }

        public void UpdateData()
        {
            foreach (var c in Categories)
            {
                var tasks = from t in Tasks
                            where t.CategoryId == c.Id
                            select t;

                var completed = from t in tasks
                                where t.Completed == true
                                select t;

                var notCompleted = from t in tasks
                                   where t.Completed == false
                                   select t;

                c.PendingTasks = notCompleted.Count();
                c.Percentage = (float)completed.Count() / (float)tasks.Count();
            }
            foreach (var t in Tasks)
            {
                var catColor =
                     (from c in Categories
                      where c.Id == t.CategoryId
                      select c.Color).FirstOrDefault();
                t.taskColor = catColor;
            }
        }
    }
}