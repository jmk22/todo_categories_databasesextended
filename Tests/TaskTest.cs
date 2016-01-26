using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  public class TaskTest : IDisposable
  {
    public TaskTest()
    {
      DBConfiguration.connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_extended_test;Integrated Security=SSPI;";
    }

    // [Fact]
    // public void Test_EmptyAtFirst()
    // {
    //   //Arrange, Act
    //   int result = Task.GetAll().Count;
    //
    //   //Assert
    //   Assert.Equal(0, result);
    // }
    //
    // [Fact]
    // public void Test_EqualOverrideTrueForSameDescription()
    // {
    //   //Arrange, Act
    //   Task firstTask = new Task("Mow the lawn");
    //   Task secondTask = new Task("Mow the lawn");
    //
    //   //Assert
    //   Assert.Equal(firstTask, secondTask);
    // }
    //
    // [Fact]
    // public void Test_Save()
    // {
    //   //Arrange
    //   Task testTask = new Task("Mow the lawn");
    //   testTask.Save();
    //
    //   //Act
    //   List<Task> result = Task.GetAll();
    //   List<Task> testList = new List<Task>{testTask};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }
    //
    // [Fact]
    // public void Test_SaveAssignsIdToObject()
    // {
    //   //Arrange
    //   Task testTask = new Task("Mow the lawn");
    //   testTask.Save();
    //
    //   //Act
    //   Task savedTask = Task.GetAll()[0];
    //
    //   int result = savedTask.GetId();
    //   int testId = testTask.GetId();
    //
    //   //Assert
    //   Assert.Equal(testId, result);
    // }
    //
    // [Fact]
    // public void Test_FindFindsTaskInDatabase()
    // {
    //   //Arrange
    //   Task testTask = new Task("Mow the lawn");
    //   testTask.Save();
    //
    //   //Act
    //   Task foundTask = Task.Find(testTask.GetId());
    //
    //   //Assert
    //   Assert.Equal(testTask, foundTask);
    // }
    //
    // [Fact]
    // public void Test_Update_UpdatesTaskInDatabase()
    // {
    //   //Arrange
    //   string description = "Mow the lawn";
    //   Task testTask = new Task(description);
    //   testTask.Save();
    //   string newDescription = "Walk the dog";
    //
    //   //Act
    //   testTask.Update(newDescription);
    //   string result = testTask.GetDescription();
    //
    //   //Assert
    //   Assert.Equal(newDescription, result);
    // }

    // [Fact]
    // public void Test_Delete_DeletesTaskFromDatabase()
    // {
    //   //Arrange
    //   string description1 = "Mow the lawn";
    //   Task testTask1 = new Task(description1);
    //   testTask1.Save();
    //
    //   string description2 = "Walk the dog";
    //   Task testTask2 = new Task(description2);
    //   testTask2.Save();
    //
    //   //Act
    //   testTask1.Delete();
    //   List<Task> resultTasks = Task.GetAll();
    //   List<Task> testTaskList = new List<Task> {testTask2};
    //
    //   //Assert
    //   Assert.Equal(testTaskList, resultTasks);
    // }

    [Fact]
    public void Test_AddCategory_AddsCategoryToTask()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn");
      testTask.Save();

      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      //Act
      testTask.AddCategory(testCategory);

      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }

        [Fact]
        public void Test_GetCategories_ReturnsAllTaskCategories()
        {
          //Arrange
          Task testTask = new Task("Mow the lawn");
          testTask.Save();

          Category testCategory1 = new Category("Home stuff");
          testCategory1.Save();

          Category testCategory2 = new Category("Work stuff");
          testCategory2.Save();

          //Act
          testTask.AddCategory(testCategory1);
          List<Category> result = testTask.GetCategories();
          List<Category> testList = new List<Category> {testCategory1};

          //Assert
          Assert.Equal(testList, result);
        }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
      Category.DeleteAllJoin();
    }
  }
}
