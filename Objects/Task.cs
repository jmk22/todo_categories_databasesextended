using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace ToDoList
{
  public class Task
  {
    private int id;
    private string description;

    public Task(string Description, int Id = 0)
    {
      id = Id;
      description = Description;
    }

    public override bool Equals(System.Object otherTask)
    {
        if (!(otherTask is Task))
        {
          return false;
        }
        else {
          Task newTask = (Task) otherTask;
          bool idEquality = this.GetId() == newTask.GetId();
          bool descriptionEquality = this.GetDescription() == newTask.GetDescription();
          return (idEquality && descriptionEquality);
        }
    }


    public int GetId()
    {
      return id;
    }
    public string GetDescription()
    {
      return description;
    }
    public void SetDescription(string newDescription)
    {
      description = newDescription;
    }

    public static List<Task> GetAll()
    {
      List<Task> AllTasks = new List<Task>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        Task newTask = new Task(taskDescription, taskId);
        AllTasks.Add(newTask);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllTasks;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO tasks (description) OUTPUT INSERTED.id VALUES (@TaskDescription)", conn);

      SqlParameter descriptionParam = new SqlParameter();
      descriptionParam.ParameterName = "@TaskDescription";
      descriptionParam.Value = this.GetDescription();

      cmd.Parameters.Add(descriptionParam);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM tasks;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Task Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM tasks WHERE id = @TaskId", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = id.ToString();
      cmd.Parameters.Add(taskIdParameter);
      rdr = cmd.ExecuteReader();

      int foundTaskId = 0;
      string foundTaskDescription = null;

      while(rdr.Read())
      {
        foundTaskId = rdr.GetInt32(0);
        foundTaskDescription = rdr.GetString(1);
      }
      Task foundTask = new Task(foundTaskDescription, foundTaskId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundTask;
    }

    public void Update(string newDescription)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE tasks SET description = @NewDescription OUTPUT INSERTED.description WHERE id = @TaskId;", conn);

      SqlParameter newDescriptionParameter = new SqlParameter();
      newDescriptionParameter.ParameterName = "@NewDescription";
      newDescriptionParameter.Value = newDescription;
      cmd.Parameters.Add(newDescriptionParameter);

      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = this.GetId();
      cmd.Parameters.Add(taskIdParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.description = rdr.GetString(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM tasks WHERE id = @TaskId; DELETE FROM categories_tasks WHERE task_id = @TaskId;", conn);
      SqlParameter taskIdParameter = new SqlParameter();
      taskIdParameter.ParameterName = "@TaskId";
      taskIdParameter.Value = this.GetId();

      cmd.Parameters.Add(taskIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
