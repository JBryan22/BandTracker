using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker;
using System;
using BandTracker.Models;
using System.Collections.Generic;

namespace BandTracker.Tests
{
  [TestClass]
  public class VenueTests : IDisposable
  {
     public VenueTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=band_tracker_test;";
    }

    public void Dispose()
    {
      Venue.DeleteAll();
      Band.DeleteAll();
    }

    [TestMethod]
    public void Equals_ReturnsTrueForTwoSameObjects_True()
    {
      Venue newVenue = new Venue("The Showbox");
      Venue newVenue2 = new Venue("The Showbox");

      bool result = newVenue.Equals(newVenue2);

      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void Save_SavesNewVenueToDatabase_List()
    {
      Venue newVenue = new Venue("The Showbox");
      newVenue.Save();

      List<Venue> expected = new List<Venue>(){newVenue};
      var actual = Venue.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeletesAVenue_List()
    {
      Venue newVenue = new Venue("The Showbox");
      newVenue.Save();
      Venue newVenue2 = new Venue("The Triple Door");
      newVenue2.Save();

      newVenue.Delete();

      List<Venue> expected = new List<Venue>() {newVenue2};
      List<Venue> actual = Venue.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsVenueById_Venue()
    {
      Venue newVenue = new Venue("The Showbox");
      newVenue.Save();

      var expected = newVenue;
      var result = Venue.Find(newVenue.GetId());

      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Update_UpdatesVenueName_Venue()
    {
      Venue newVenue = new Venue("The Showbox");
      newVenue.Save();

      newVenue.Update("The Showbox SODO");

      Venue newVenue2 = new Venue("The Showbox SODO");

      var expected = newVenue2.GetName();
      var actual = Venue.Find(newVenue.GetId()).GetName();

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddBand_AddsNewBandToDatabase_List()
    {
      Venue newVenue = new Venue("The Showbox");
      newVenue.Save();

      Band newBand = new Band("The Spinners");
      newBand.Save();

      newVenue.AddBand(newBand);

      var expected = newBand.GetName();
      var actual = newVenue.GetBands();

      Assert.AreEqual(expected, actual[0].GetName());
    }

  }
}