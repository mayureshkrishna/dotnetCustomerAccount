using System;
using System.Reflection;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Cox.Validation;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Cox.BusinessLogic
{
	/// <summary>
	/// This class handles conversions to and from Icoms Dates
	/// represented as a string and .Net DateTime values
	/// </summary>
	public abstract class BaseIcomsDate
	{
		#region constants
		/// <summary>
		/// Value returned from Icoms to indicate that
		/// there is no date and that the bill is due
		/// upon receipt.
		/// </summary>
		protected const int __specialDate=9999999;
		#endregion constants

		#region member variables
		/// <summary>
		/// Contains the internal date value.
		/// </summary>
		protected DateTime _date=DateTime.Now;
		/// <summary>
		/// Sometimes Icoms returns bad data. If this occurs
		/// with an unrecognized Date value, then we will set
		/// this flag to true.
		/// </summary>
		protected bool _validDate = true;
		/// <summary>
		/// Icoms Date uses a date of 9999999 as a flag 
		/// to indicate that a statement is due upon receipt.
		/// </summary>
		protected bool _specialDate = false;
		#endregion member variables

		#region ctors
		/// <summary>
		/// Hide the default constructor
		/// </summary>
		protected BaseIcomsDate(){}
		/// <summary>
		/// Constructs an icomsDate based on a datetime value.
		/// </summary>
		/// <param name="date"></param>
		protected BaseIcomsDate( DateTime date )
		{
			_date=date;
			_validDate=true;
			_specialDate=false;
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Returns the underlying date value.
		/// </summary>
		public DateTime Date
		{
			get{return _date;}
		}
		/// <summary>
		/// Returns the underlying date value.
		/// </summary>
		public bool SpecialDate
		{
			get{return _specialDate;}
		}
		/// <summary>
		/// Returns true if the underlying date
		/// was actually valid; false otherwise.
		/// </summary>
		public bool ValidDate
		{
			get { return _validDate; }
		}
		#endregion properties

		#region public methods
		/// <summary>
		/// Returns the underlying date value as an icomsDate formatted string.
		/// </summary>
		public bool SetTime( int timeVal )
		{
			try
			{
				string stringVal = timeVal.ToString().PadLeft(4,'0');
				// use simple regex to test its validity.
				Regex regEx = new Regex(
					@"(?<hour>[0-9]{2})(?<min>[0-9]{2})",RegexOptions.None);
				if( regEx.IsMatch( stringVal ) )
				{
					Match match = regEx.Match(stringVal);
					if( match != null )
					{
						// ok. at this point, we have a match and we know that 
						// hour and minute are each some two digit numeric value.
						// now let's make sure that they are in the correct range.
						int hour=int.Parse( match.Groups[ "hour" ].Value );
						int minute=int.Parse( match.Groups[ "min" ].Value );
						// check range
						if( hour >= 24 || minute >= 60 ) return false;
						// clear out current time.
						_date=_date.Subtract( _date.TimeOfDay );
						// now set the hours and minute portion based on parameter
						_date=_date.AddHours(hour);
						_date=_date.AddMinutes(minute);
						return true;
					}
				}
			}
			catch{/*we don't care*/}
			// if we get here, then its a false.
			return false;
		}
		#endregion public methods
	}

	/// <summary>
	/// This class handles conversions to and from Icoms Dates
	/// represented as a string and .Net DateTime values
	/// </summary>
	public class IcomsDate : BaseIcomsDate
	{
		#region ctors
		/// <summary>
		/// Hide the default constructor
		/// </summary>
		protected IcomsDate() : base(){}

		/// <summary>
		/// Takes in an ICOMS date and converts to dates
		/// </summary>
		/// <param name="icomsDate"></param>
		public IcomsDate( int icomsDate ) : this(icomsDate.ToString()){}
		/// <summary>
		/// Takes in an ICOMS date and converts to dates
		/// </summary>
		/// <param name="icomsDate"></param>
		public IcomsDate( string icomsDate )
		{
			if( icomsDate == __specialDate.ToString() )
			{
				_date = DateTime.Now;
				_validDate = true;
				_specialDate = true;
			}
			else
			{
				_date=DateTime.MinValue;
				_specialDate = false;
				_validDate = false;
				// ok, we have something we care about, so use 
				// simple regex to test its validity.
				Regex regEx = new Regex(@"^\d{8}$",RegexOptions.None);
				if( regEx.IsMatch( icomsDate ) )
				{
					try
					{
						_date=DateTime.Parse( string.Format( "{0}/{1}/{2}",
							icomsDate.Substring(4,2), icomsDate.Substring(6,2),
							icomsDate.Substring( 0,4) ) );
						_validDate=true;
					}
					catch{/*do nothing*/}
				}
			}
		}
		/// <summary>
		/// Constructs an icomsDate based on a datetime value.
		/// </summary>
		/// <param name="date"></param>
		public IcomsDate( DateTime date ) : base( date ){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Returns the underlying date value as an icomsDate formatted string.
		/// </summary>
		public override string ToString()
		{
			return _date.ToString("yyyyMMdd");
		}
		#endregion public methods

		#region operators
		/// <summary>
		/// Converts to a date.
		/// </summary>
		/// <param name="icomsDate"></param>
		/// <returns></returns>
		public static implicit operator DateTime( IcomsDate icomsDate )
		{
			return icomsDate.Date;
		}
		/// <summary>
		/// Converts a date to an IcomsDate.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static implicit operator IcomsDate( DateTime date )
		{
			return new IcomsDate( date );
		}
		/// <summary>
		/// Converts an Icoms1900 Date to an integer.
		/// </summary>
		/// <param name="icomsDate"></param>
		/// <returns></returns>
		public static implicit operator int( IcomsDate icomsDate )
		{
			return Convert.ToInt32(icomsDate.ToString());
		}
		#endregion operators
	}
	/// <summary>
	/// ICOMS appears to have several different date formats. One of them
	/// is what we call the 1900 format. These dates come in the format of
	/// the year-1900 followed by the month and then the day.
	/// For example: 1031122 is 11/22/2003 and 990203 is 02/03/1999
	/// </summary>
	public class Icoms1900Date : BaseIcomsDate
	{
		#region ctors
		/// <summary>
		/// Hide the default constructor
		/// </summary>
		protected Icoms1900Date() : base(){}
		/// <summary>
		/// Takes in an ICOMS 1900 Date as an integer and converts it to a date.
		/// </summary>
		/// <param name="icomsDate"></param>
		public Icoms1900Date(int icomsDate):this(icomsDate.ToString()){}
        /// <summary>
        /// Takes in an ICOMS 1900 Date as a decimal and converts it to a date.
        /// </summary>
        /// <param name="icomsDate"></param>
        public Icoms1900Date(decimal icomsDate) : this(Convert.ToInt32(icomsDate)) { }
        /// <summary>
		/// Takes in an ICOMS 1900 Date as a string and converts it to a date.
		/// </summary>
		/// <param name="icomsDate"></param>
		public Icoms1900Date( string icomsDate )
		{
			string dateVal=icomsDate.Trim();
			if( dateVal == __specialDate.ToString() )
			{
				_date = DateTime.Now;
				_validDate = true;
				_specialDate = true;
			}
			else
			{
				_date=DateTime.MinValue;
				_validDate = false;
				_specialDate = false;

				// ok, we have something we care about, so use 
				// simple regex to test its validity.
				Regex regEx = new Regex(@"^\d{6,7}$",RegexOptions.None);
				if( regEx.IsMatch( dateVal ) )
				{
					try
					{
						// determine if the year is >= 2000
						int offset=dateVal.Length==7?1:0;
						// now use the offset to determine the year.
						int year=1900+int.Parse( dateVal.Substring( 0, 2 + offset ) );
						// now build the underlying date.
						_date=DateTime.Parse( string.Format( "{0}/{1}/{2}",
							dateVal.Substring(2+offset,2),
							dateVal.Substring(4+offset,2),
							year.ToString() ) );
					}
					catch{/*do nothing*/}
				}
			}
		}
		/// <summary>
		/// Constructs an icomsDate based on a datetime value.
		/// </summary>
		/// <param name="date"></param>
		public Icoms1900Date( DateTime date ) : base(date){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Returns the underlying date value as an icomsDate formatted string.
		/// </summary>
		public override string ToString()
		{
			return string.Format("{0}{1}{2}",
				(_date.Year-1900).ToString(),
				_date.Month.ToString().PadLeft(2,'0'),
				_date.Day.ToString().PadLeft(2,'0') );
		}
		#endregion public methods

		#region operators
		/// <summary>
		/// Converts to a date.
		/// </summary>
		/// <param name="icoms1900Date"></param>
		/// <returns></returns>
		public static implicit operator DateTime( Icoms1900Date icoms1900Date )
		{
			return icoms1900Date.Date;
		}
		/// <summary>
		/// Converts a date to an IcomsDate.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static implicit operator Icoms1900Date( DateTime date )
		{
			return new Icoms1900Date( date );
		}
		/// <summary>
		/// Converts an Icoms1900 Date to an integer.
		/// </summary>
		/// <param name="icoms1900Date"></param>
		/// <returns></returns>
		public static implicit operator int( Icoms1900Date icoms1900Date )
		{
			return Convert.ToInt32(icoms1900Date.ToString());
		}
		#endregion operators
	}
	/// <summary>
	/// Handles converting ICOMS Military Time Format to a normal user-friendly format.
	/// </summary>
	public class IcomsMilitaryTime
	{
		#region constants
		/// <summary>
		/// Member variable containing the error message when time passed to constructor was in an invalid format.
		/// </summary>
		protected const string __invalidTime = "Time format given was in an invalid format. It must be in the format 'HHMM'";
		/// <summary>
		/// The default format to use for the ToTime() method.
		/// </summary>
		protected const string __defaultTimeFormat = "h:mm tt";
		#endregion constants

		#region member variables
		/// <summary>
		/// Member variable containing the parsed hours.
		/// </summary>
		protected int _hours = 0;
		/// <summary>
		/// Member variable containing the parsed minutes.
		/// </summary>
		protected int _minutes = 0;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="time"></param>
		public IcomsMilitaryTime( string time )
		{
			if( time == null ) throw new ArgumentNullException( "time" );
			
			try
			{
				int totalTime = Convert.ToInt32( time );
				if( totalTime == 0 ) throw new ArgumentException( __invalidTime );
			}
			catch(Exception ex)
			{
				throw new ArgumentException( __invalidTime, ex );
			}
			// now convert.
			string paddedTime = time.PadLeft( 4, '0' );
			_hours = Convert.ToInt32( paddedTime.Substring( 0, 2 ) );
			_minutes = Convert.ToInt32( paddedTime.Substring( 2, 2 ) );
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Get property that returns back the given Time value as a TimeSpan.
		/// </summary>
		public TimeSpan TimeSpan
		{
			get
			{
				return new TimeSpan( _hours, _minutes, 0 );
			}
		}
		#endregion properties

		#region methods
		/// <summary>
		/// Override of ToString() method. This method returns back the given time (padded).
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _hours.ToString().PadLeft( 2, '0' ) + _minutes.ToString().PadLeft( 2, '0' );
		}
		/// <summary>
		/// This method returns back the time in the default ShortTimeFormat hh:mm tt.
		/// </summary>
		/// <returns></returns>
		public string ToTime()
		{
			return ToTime( __defaultTimeFormat );
		}
		/// <summary>
		/// Method to return back internal time in any format requested.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public string ToTime( string format )
		{
			try
			{
				DateTime now = DateTime.Now;
				DateTime date = new DateTime( now.Year, now.Month, now.Day, _hours, _minutes, 0 );
				string ret = date.ToString( format );
				return ret;
			}
			catch( Exception ex )
			{
				throw new ArgumentException( __invalidTime, ex );
			}
		}
		#endregion methods
	}
}
