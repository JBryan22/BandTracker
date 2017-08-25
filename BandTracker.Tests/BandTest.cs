using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker;
using System;
using BandTracker.Models;
using System.Collections.Generic;

namespace BandTracker.Tests
{
  [TestClass]
  public class BandTests : IDisposable
  {
     public BandTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=3306;database=band_tracker_test;";
    }

    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }

    [TestMethod]
    public void Equals_ReturnsTrueForTwoSameObjects_True()
    {
      Band newBand = new Band("Blink 182");
      Band newBand2 = new Band("Blink 182");

      bool result = newBand.Equals(newBand2);

      Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void Save_SavesNewBandToDatabase_List()
    {
      Band newBand = new Band("Blink 182");
      newBand.Save();

      List<Band> expected = new List<Band>(){newBand};
      var actual = Band.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeletesABand_List()
    {
      Band newBand = new Band("Blink 182");
      newBand.Save();
      Band newBand2 = new Band("Above and Beyond");
      newBand2.Save();

      newBand.Delete();

      List<Band> expected = new List<Band>() {newBand2};
      List<Band> actual = Band.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsBandById_Band()
    {
      Band newBand = new Band("Blink 182");
      newBand.Save();

      var expected = newBand;
      var result = Band.Find(newBand.GetId());

      Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Update_UpdatesBandName_Band()
    {
      Band newBand = new Band("Blink 182");
      newBand.Save();

      newBand.Update("blink-182");

      Band newBand2 = new Band("blink-182");

      var expected = newBand2.GetName();
      var actual = Band.Find(newBand.GetId()).GetName();

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddVenue_AddsNewVenueToDatabase_List()
    {
      Band newBand = new Band("Blink 182");
      newBand.Save();

      Venue newVenue = new Venue("Neumos");
      newVenue.Save();

      newBand.AddVenue(newVenue);

      var expected = new List<Venue>() {newVenue};
      var actual = newBand.GetVenues();

      CollectionAssert.AreEqual(expected, actual);
    }

  }
}