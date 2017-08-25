using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace BandTracker.Models
{
  public class Venue
  {
      private int _id;
      private string _name;

      public Venue(string name, int id = 0)
      {
          _name = name;
          _id = id;
      }

      public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherVenue)
    {
      if (!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
        Venue newVenue = (Venue) otherVenue;
        bool idEquality = (this.GetId()) == newVenue.GetId();
        bool nameEquality = (this.GetName()) == newVenue.GetName();

        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues (name) VALUE (@name);";

      MySqlParameter VenueId = new MySqlParameter();
      VenueId.ParameterName = "@name";
      VenueId.Value = _name;
      cmd.Parameters.Add(VenueId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Venue> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues ORDER BY name;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Venue> allVenues = new List<Venue>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Venue foundVenue = new Venue(name, id);
        allVenues.Add(foundVenue);
      }
      conn.Close();
      return allVenues;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues; DELETE FROM bands_venues;";

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues WHERE id = @id; DELETE FROM bands_venues WHERE venue_id = @id;";

      MySqlParameter VenueId = new MySqlParameter();
      VenueId.ParameterName = "@id";
      VenueId.Value = _id;
      cmd.Parameters.Add(VenueId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Venue Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues WHERE id = @id;";

      MySqlParameter VenueId = new MySqlParameter();
      VenueId.ParameterName = "@id";
      VenueId.Value = searchId;
      cmd.Parameters.Add(VenueId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newVenueId = 0;
      string VenueName = "";

      while(rdr.Read())
      {
        newVenueId = rdr.GetInt32(0);
        VenueName = rdr.GetString(1);
      }
      var foundVenue = new Venue(VenueName, newVenueId);
      conn.Close();
      return foundVenue;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE venues SET name = @name WHERE id = @id;";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = newName;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter VenueId = new MySqlParameter();
      VenueId.ParameterName = "@id";
      VenueId.Value = _id;
      cmd.Parameters.Add(VenueId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddBand(Band newBand)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands_venues (band_id, venue_id) VALUES (@bandId, @VenueId);";

      MySqlParameter VenueId = new MySqlParameter();
      VenueId.ParameterName = "@VenueId";
      VenueId.Value = _id;
      cmd.Parameters.Add(VenueId);

      MySqlParameter bandId = new MySqlParameter();
      bandId.ParameterName = "@bandId";
      bandId.Value = newBand.GetId();
      cmd.Parameters.Add(bandId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Venue> GetBands()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT bands.*
      FROM venues
      JOIN bands_venues ON (venues.id = bands_venues.venue_id)
      JOIN bands ON (bands.id = bands_venues.band_id)
      WHERE venues.id = @VenueId;";

      MySqlParameter VenueId = new MySqlParameter();
      VenueId.ParameterName = "@VenueId";
      VenueId.Value = _id;
      cmd.Parameters.Add(VenueId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Venue> allVenues = new List<Venue>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Venue newVenue = new Venue(name, id);
        allVenues.Add(newVenue);
      }
      conn.Close();
      return allVenues;
    }
  }
}