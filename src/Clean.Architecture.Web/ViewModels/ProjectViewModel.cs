﻿using System.Collections.Generic;

namespace Clean.Architecture.Web.ViewModels;

public class ProjectViewModel
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public List<ToDoItemViewModel> Items = new();
}
