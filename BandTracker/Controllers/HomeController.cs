using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System.Collections.Generic;
using System;

namespace BandTracker.Controllers
{
  public class HomeController : Controller
  {
      [HttpGet("/")]
      public ActionResult Index()
      {
          return View();
      }

      [HttpGet("/bands")]
      public ActionResult BandList()
      {
          return View(Band.GetAll());
      }

      [HttpGet("/venues")]
      public ActionResult VenueList()
      {
          return View(Venue.GetAll());
      }

      [HttpGet("/band/form")]
      public ActionResult BandForm()
      {
          return View();
      }

      [HttpGet("/venue/form")]
      public ActionResult VenueForm()
      {
          return View();
      }

      [HttpGet("/band/{id}")]
      public ActionResult BandDetails(int id)
      {
          var model = new Dictionary<string,object>();
          var foundBand = Band.Find(id);

          model.Add("band", foundBand);
          model.Add("venues", Venue.GetAll());

          return View(model);
      }

      [HttpGet("/venue/{id}")]
      public ActionResult VenueDetails(int id)
      {
          var model = new Dictionary<string,object>();
          var foundVenue = Venue.Find(id);

          model.Add("venue", foundVenue);
          model.Add("bands", Band.GetAll());

          return View(model);
      }

      [HttpPost("/bands")]
      public ActionResult BandListAdd()
      {
          Band newBand = new Band(Request.Form["band-name"]);
          newBand.Save();

          return View("BandList", Band.GetAll());
      }

      [HttpPost("/venues")]
      public ActionResult VenueListAdd()
      {
          Venue newVenue = new Venue(Request.Form["venue-name"]);
          newVenue.Save();

          return View("VenueList", Venue.GetAll());
      }

      [HttpPost("band/{id}")]
      public ActionResult BandDetailAdd(int id)
      {
          var model = new Dictionary<string,object>();
          var foundBand = Band.Find(id);

          foundBand.AddVenue(Venue.Find(int.Parse(Request.Form["venue"])));

          model.Add("band", foundBand);
          model.Add("venues", Venue.GetAll());

          return View("BandDetails", model);
      }

      [HttpPost("venue/{id}")]
      public ActionResult VenueDetailAdd(int id)
      {
          var model = new Dictionary<string,object>();
          var foundVenue = Venue.Find(id);

          foundVenue.AddBand(Band.Find(int.Parse(Request.Form["band"])));

          model.Add("venue", foundVenue);
          model.Add("bands", Band.GetAll());

          return View("VenueDetails", model);
      }

      [HttpGet("bands/{id}/delete")]
      public ActionResult BandDelete(int id)
      {
          var foundBand = Band.Find(id);
          foundBand.Delete();

          return View("BandList", Band.GetAll());
      }

      [HttpGet("venues/{id}/delete")]
      public ActionResult VenueDelete(int id)
      {
          var foundVenue = Venue.Find(id);
          foundVenue.Delete();

          return View("VenueList", Venue.GetAll());
      }
  }
}
