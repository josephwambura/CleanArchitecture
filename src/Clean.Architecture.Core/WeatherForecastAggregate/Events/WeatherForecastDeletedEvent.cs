﻿using Clean.Architecture.SharedKernel;

namespace Clean.Architecture.Core.WeatherForecastAggregate.Events;

public class WeatherForecastDeletedEvent : DomainEventBase
{
  public Guid WeatherForecastId { get; set; }

  public WeatherForecastDeletedEvent(Guid projectId)
  {
    WeatherForecastId = projectId;
  }
}