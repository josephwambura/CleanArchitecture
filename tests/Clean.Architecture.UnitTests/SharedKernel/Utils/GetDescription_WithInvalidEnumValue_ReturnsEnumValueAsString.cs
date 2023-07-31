using Clean.Architecture.Application.DTO.ProjectAggregate;
using Clean.Architecture.SharedKernel.Utils;

using Xunit;

namespace Clean.Architecture.UnitTests.SharedKernel.Utils;

public class UtilsEnumsConstructor
{
  [Fact]
  public void GetDescription_WithInvalidEnumValue_ReturnsEnumValueAsString()
  {
    // Arrange
    RecordStatus value = (RecordStatus)100; // Invalid enum value

    // Act
    var actual = value.GetDescription();

    Assert.Equal("100", actual);
  }

  [Fact]
  public void GetDescription_WithEnumValue_ReturnsEnumValueAsString()
  {
    var projectDTOs = new List<ProjectDTO>();

    for (int i = 0; i < 1000; i++)
    {
      projectDTOs.Add(new ProjectDTO
      {
        CreatedBy = "_SYS_",
        CreatedDate = DateTime.UtcNow,
        Name = i.ToString(),
        Priority = PriorityStatus.Critical,
        Items = new List<ToDoItemDTO> { new ToDoItemDTO { CreatedDate = DateTime.UtcNow, CreatedBy = "_SYS_", Title = i.ToString(), Description = i.ToString() } }
      });
    }

    Assert.Contains(projectDTOs, p => !string.IsNullOrWhiteSpace(p.StatusDescription));
  }
}
