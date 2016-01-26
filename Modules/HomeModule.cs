using System.Collections.Generic;
using System;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace ToDoList
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            //Index
            Get["/"] = _ => {
              List<Category> AllCategories = Category.GetAll();
              return View["index.cshtml", AllCategories];
            };

            //Get: Lists
            Get["/tasks"] = _ => {
              List<Task> AllTasks = Task.GetAll();
              return View["tasks.cshtml", AllTasks];
            };
            Get["/categories"] = _ => {
              List<Category> AllCategories = Category.GetAll();
              return View["categories.cshtml", AllCategories];
            };

            //Create new category
            Get["/categories/new"] = _ => {
              return View["categories_form.cshtml"];
            };
            Post["/categories/new"] = _ => {
              Category newCategory = new Category(Request.Form["category-name"]);
              newCategory.Save();
              return View["success.cshtml"];
            };

            //Create new task
            Get["/tasks/new"] = _ => {
              List<Category> AllCategories = Category.GetAll();
              return View["tasks_form.cshtml", AllCategories];
            };
            Post["/tasks/new"] = _ => {
              Task newTask = new Task(Request.Form["task-description"]);
              newTask.Save();
              Category taskCategory = new Category(Request.Form["category-id"]);
              Console.WriteLine("taskCategory: {0}", taskCategory.GetName());
              taskCategory.AddTask(newTask);
              return View["success.cshtml"];
            };

            //"Clear All" routes
            Post["/tasks/clear"] = _ => {
              Task.DeleteAll();
              return View["cleared.cshtml"];
            };
            Post["/categories/clear"] = _ => {
              Category.DeleteAll();
              return View["cleared.cshtml"];
            };

            //View specific category
            Get["categories/{id}"] = parameters => {
              Dictionary<string, object> model = new Dictionary<string, object>();
              var SelectedCategory = Category.Find(parameters.id);
              var CategoryTasks = SelectedCategory.GetTasks();
              model.Add("category", SelectedCategory);
              model.Add("tasks", CategoryTasks);
              return View["category.cshtml", model];
            };

            //View specific task
            Get["tasks/{id}"] = parameters => {
              Dictionary<string, object> model = new Dictionary<string, object>();
              Task SelectedTask = Task.Find(parameters.id);
              return View["task.cshtml", SelectedTask];
            };

            //Edit specific category
            Get["category/edit/{id}"] = parameters => {
              Category SelectedCategory = Category.Find(parameters.id);
              return View["category_edit.cshtml", SelectedCategory];
            };
            Post["category/edit/{id}"] = parameters => {
              Category SelectedCategory = Category.Find(parameters.id);
              SelectedCategory.Update(Request.Form["category-name"]);
              return View["success.cshtml"];
            };

            //Delete specific category
            Get["category/delete/{id}"] = parameters => {
              Category SelectedCategory = Category.Find(parameters.id);
              return View["category_delete.cshtml", SelectedCategory];
            };
            Post["category/delete/{id}"] = parameters => {
              Category SelectedCategory = Category.Find(parameters.id);
              SelectedCategory.Delete();
              return View["success.cshtml"];
            };
        }
    }
}
