using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;

namespace Cox.DataAccess.House
{
  /// <summary>
  /// The address of a House
  /// </summary>
  public class Address
  {
    #region MEMBER VARIABLES
    /// <summary>
    /// Address location
    /// </summary>
    protected string _location = string.Empty;

    /// <summary>
    /// Address fraction
    /// </summary>
    protected string _fraction = string.Empty;

    /// <summary>
    /// Apartment
    /// </summary>
    protected string _apartment = string.Empty;

    /// <summary>
    /// Pre directional
    /// </summary>
    protected string _preDirectional = string.Empty;

    /// <summary>
    /// Street name
    /// </summary>
    protected string _street = string.Empty;

    /// <summary>
    /// Post directional
    /// </summary>
    protected string _postDirectional = string.Empty;

    /// <summary>
    /// City
    /// </summary>
    protected string _city = string.Empty;

    /// <summary>
    /// State
    /// </summary>
    protected string _state = string.Empty;

    /// <summary>
    /// Zip 5
    /// </summary>
    protected string _zip5 = string.Empty;

    /// <summary>
    /// Zip 4
    /// </summary>
    protected string _zip4 = string.Empty;

    #endregion

    #region CONSTRUCTOR(S)
    public Address(string location
                  ,string fraction
                  ,string apartment
                  ,string preDirectional
                  ,string street
                  ,string postDirectional
                  ,string city
                  ,string state
                  ,string zip5
                  ,string zip4)
    { 
      _location = location;
      _fraction = fraction;
      _apartment = apartment;
      _preDirectional = preDirectional;
      _street = street;
      _postDirectional = postDirectional;
      _city = city;
      _state = state;
      _zip5 = zip5;
      _zip4 = zip4;
    }

    #endregion

    #region PROPERTIES
    public string Location 
    {
      get{return _location;}
      set{_location = value;}
    }

    public string Fraction
    {
      get{return _fraction;}
      set{_fraction = value;}
    }

    public string Apartment
    {
      get{return _apartment;}
      set{_apartment = value;}
    }

    public string PreDirectional
    {
      get{return _preDirectional;}
      set{_preDirectional = value;}
    }

    public string Street
    {
      get{return _street;}
      set{_street = value;}
    }

    public string PostDirectional
    {
      get{return _postDirectional;}
      set{_postDirectional = value;}
    }

    public string City
    {
      get{return _city;}
      set{_city = value;}
    }

    public string State
    {
      get{return _state;}
      set{_state = value;}
    }

    public string Zip5 
    {
      get{return _zip5;}
      set{_zip5 = value;}
    }

    public string Zip4
    {
      get{return _zip4;}
      set {_zip4 = value; }
    }
    #endregion

  }
}
