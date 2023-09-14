using AircraftParkingPlanning;
using AircraftParkingPlanning.Controllers;
using AircraftParkingPlanning.Model;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AircraftParkingPlannigTests
{
  public class FlightsControllerTest
  {
    private readonly Setup setupControlVar = new Setup();
    private FlightsController getFLightsControllerInstance(Setup setup)
    {
      ILogger<FlightsController> mockLogger = A.Fake<ILogger<FlightsController>>();
      return new FlightsController(mockLogger, setup);
    }
    [Fact]
    public void GetFlights()
    {
      Setup setup = new Setup();
      FlightsController fController= getFLightsControllerInstance(setup);
      var result = fController.Get();
      var expectedResult = setup.Flights;
      Assert.Equal(result.Count, expectedResult.Count);
      for (int i = 0; i < result.Count; i++)
      {
        Assert.Equal(result.ElementAt(i).Id, expectedResult.ElementAt(i).Id);
        Assert.Equal(result.ElementAt(i).Aircraft?.RegistrationCode, expectedResult.ElementAt(i).Aircraft?.RegistrationCode);
        Assert.Equal(result.ElementAt(i).ParkingSpot?.Name, expectedResult.ElementAt(i).ParkingSpot?.Name);
      }
    }

    [Fact]
    public void AddNewFlightsWithNewAircraft()
    {
      Setup setup = new Setup();
      FlightsController fController = getFLightsControllerInstance(setup);
      Flight newFlightNewAircraft = new Flight(Guid.NewGuid().ToString())
      {
        Aircraft = new Aircraft { RegistrationCode = "TEST", FootprintSqm = 350, AircraftType = "Piper 350" },
        StartDateTime = DateTime.Parse("19 Jan 2023 12:00"),
        EndDateTime = DateTime.Parse("20 Jan 2023 08:00"),
        ParkingSpot = setupControlVar.ParkingAreas.ElementAt(0).ParkingSpots.ElementAt(0),
      };
      ActionResult actionResult = fController.Post(newFlightNewAircraft);
      Assert.IsType<OkResult>(actionResult);
      Assert.Equal(setup.Flights.Count, setupControlVar.Flights.Count+1);
      Assert.Equal(setup.AircraftList.Count, setupControlVar.AircraftList.Count + 1);
    }

    [Fact]
    public void AddNewFlightsWithExistingAircraft()
    {
      Setup setup = new Setup();
      FlightsController fController = getFLightsControllerInstance(setup);
      Flight newFlightNewAircraft = new Flight(Guid.NewGuid().ToString())
      {
        Aircraft = setupControlVar.AircraftList.ElementAt(0),
        StartDateTime = DateTime.Parse("19 Jan 2023 12:00"),
        EndDateTime = DateTime.Parse("20 Jan 2023 08:00"),
        ParkingSpot = setupControlVar.ParkingAreas.ElementAt(0).ParkingSpots.ElementAt(0),
      };
      ActionResult actionResult = fController.Post(newFlightNewAircraft);
      Assert.IsType<OkResult>(actionResult);
      Assert.Equal(setup.Flights.Count, setupControlVar.Flights.Count + 1);
      Assert.Equal(setup.AircraftList.Count, setupControlVar.AircraftList.Count);
    }

    [Fact]
    public void updateFlightsWithNewAircraft()
    {
      Setup setup = new Setup();
      FlightsController fController = getFLightsControllerInstance(setup);
      Flight incomingFlightNewAircraft = new Flight(setup.Flights.ElementAt(0).Id)
      {
        Aircraft = new Aircraft { RegistrationCode = "TEST", FootprintSqm = 350, AircraftType = "Piper 350" },
        StartDateTime = DateTime.Parse("19 Jan 2023 12:00"),
        EndDateTime = DateTime.Parse("20 Jan 2023 08:00"),
        ParkingSpot = setupControlVar.ParkingAreas.ElementAt(0).ParkingSpots.ElementAt(0),
      };
      ActionResult actionResult = fController.Post(incomingFlightNewAircraft);
      Assert.IsType<OkResult>(actionResult);
      Assert.Equal(setup.Flights.Count, setupControlVar.Flights.Count);
      Assert.Equal(setup.AircraftList.Count, setupControlVar.AircraftList.Count + 1);
    }

    [Fact]
    public void updateFlightsWithExistingAircraft()
    {
      Setup setup = new Setup();
      FlightsController fController = getFLightsControllerInstance(setup);
      Flight newFlightNewAircraft = new Flight(setup.Flights.ElementAt(0).Id)
      {
        Aircraft = setupControlVar.AircraftList.ElementAt(0),
        StartDateTime = DateTime.Parse("19 Jan 2023 12:00"),
        EndDateTime = DateTime.Parse("20 Jan 2023 08:00"),
        ParkingSpot = setupControlVar.ParkingAreas.ElementAt(0).ParkingSpots.ElementAt(0),
      };
      ActionResult actionResult = fController.Post(newFlightNewAircraft);
      Assert.IsType<OkResult>(actionResult);
      Assert.Equal(setup.Flights.Count, setupControlVar.Flights.Count);
      Assert.Equal(setup.AircraftList.Count, setupControlVar.AircraftList.Count);
    }

    [Fact]
    public void deleteFlightWithGivenId()
    {
      Setup setup = new Setup();
      FlightsController fController = getFLightsControllerInstance(setup);
      ActionResult actionResult = fController.Delete(setup.Flights.ElementAt(0).Id);
      Assert.IsType<OkResult>(actionResult);
      Assert.Equal(setup.Flights.Count, setupControlVar.Flights.Count-1);
    }
  }
}