﻿using Clean.Architecture.SharedKernel;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Core.AdministrationModule.EnumerationAggregate;

public class Enumeration : EntityBase, IAggregateRoot
{
  public string Key { get; set; } = default!;

  public int Value { get; set; }

  public string Description { get; set; } = default!;

  public override string ToString()
  {
    return $"Enumeration: {Description}";
  }
}