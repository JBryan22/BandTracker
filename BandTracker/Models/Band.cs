using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace BandTracker.Models
{
  public class Band
  {
      private int _id;
      private string _name;

      public Band(string name, int id = 0)
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

    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = (this.GetId()) == newBand.GetId();
        bool nameEquality = (this.GetName()) == newBand.GetName();

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
      cmd.CommandText = @"INSERT INTO bands (name) VALUE (@name);";

      MySqlParameter BandId = new MySqlParameter();
      BandId.ParameterName = "@name";
      BandId.Value = _name;
      cmd.Parameters.Add(BandId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Band> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands ORDER BY name;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Band> allBands = new List<Band>{};

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Band foundBand = new Band(name, id);
        allBands.Add(foundBand);
      }
      conn.Close();
      return allBands;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands; DELETE FROM bands_venues;";

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands WHERE id = @id; DELETE FROM bands_venues WHERE band_id = @id;";

      MySqlParameter BandId = new MySqlParameter();
      BandId.ParameterName = "@id";
      BandId.Value = _id;
      cmd.Parameters.Add(BandId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Band Find(int searchId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands WHERE id = @id;";

      MySqlParameter BandId = new MySqlParameter();
      BandId.ParameterName = "@id";
      BandId.Value = searchId;
      cmd.Parameters.Add(BandId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newBandId = 0;
      string BandName = "";

      while(rdr.Read())
      {
        newBandId = rdr.GetInt32(0);
        BandName = rdr.GetString(1);
      }
      var foundBand = new Band(BandName, newBandId);
      conn.Close();
      return foundBand;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE bands SET name = @name WHERE id = @id;";

      MySqlParameter nameParameter = new MySqlParameter();
      nameParameter.ParameterName = "@name";
      nameParameter.Value = newName;
      cmd.Parameters.Add(nameParameter);

      MySqlParameter BandId = new MySqlParameter();
      BandId.ParameterName = "@id";
      BandId.Value = _id;
      cmd.Parameters.Add(BandId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddVenue(Venue newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands_venues (band_id, venue_id) VALUES (@BandId, @VenueId);";

      MySqlParameter BandId = new MySqlParameter();
      BandId.ParameterName = "@BandId";
      BandId.Value = _id;
      cmd.Parameters.Add(BandId);

      MySqlParameter VenueId = new MySqlParameter();
      VenueId.ParameterName = "@VenueId";
      VenueId.Value = newVenue.GetId();
      cmd.Parameters.Add(VenueId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Venue> GetVenues()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT venues.*
      FROM bands
      JOIN bands_venues ON (bands.id = bands_venues.band_id)
      JOIN venues ON (venues.id = bands_venues.venue_id)
      WHERE bands.id = @BandId;";

      MySqlParameter BandId = new MySqlParameter();
      BandId.ParameterName = "@BandId";
      BandId.Value = _id;
      cmd.Parameters.Add(BandId);

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