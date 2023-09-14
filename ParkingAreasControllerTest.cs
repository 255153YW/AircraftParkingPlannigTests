using AircraftParkingPlanning.Controllers;
using AircraftParkingPlanning;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using AircraftParkingPlanning.Model;

namespace AircraftParkingPlannigTests
{
  public class ParkingAreasControllerTest
  {
    private ParkingAreasController getParkingAreasControllerInstance(Setup setup)
    {
      ILogger<ParkingAreasController> mockLogger = A.Fake<ILogger<ParkingAreasController>>();
      return new ParkingAreasController(mockLogger, setup);
    }
    [Fact]
    public void GetFlights()
    {
      Setup setup = new Setup();
      ParkingAreasController pController = getParkingAreasControllerInstance(setup);
      List<ParkingArea> result = pController.Get();
      List<ParkingArea> expectedResult = setup.ParkingAreas;
      Assert.Equal(result.Count, expectedResult.Count);
      for (int i = 0; i < result.Count; i++)
      {
        Assert.Equal(result.ElementAt(i).Name, expectedResult.ElementAt(i).Name);
        Assert.Equal(result.ElementAt(i).TotalSurfaceSqm, expectedResult.ElementAt(i).TotalSurfaceSqm);
      }
    }
  }
}
