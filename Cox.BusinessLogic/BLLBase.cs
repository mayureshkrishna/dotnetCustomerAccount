using System;
using System.Reflection;
using Cox.ActivityLog;
using Cox.Validation;
using Cox.DataAccess;
using Cox.DataAccess.Exceptions;
using Cox.DataAccess.Enterprise;
using Cox.BusinessLogic.Exceptions;

namespace Cox.BusinessLogic
{
	/// <summary>
	/// Base class for Business Logic Layer
	/// </summary>
	public class BllBase
	{
		#region member variables
		/// <summary>
		/// Member variable containing the userName of person calling this method.
		/// </summary>
		protected string _userName=null;
		/// <summary>
		/// Member variable containing the userId associated with the passed in userName.
		/// </summary>
		protected int _userId=0;
		#endregion member variables

		#region ctor
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="userName"></param>
		public BllBase([RequiredItem()]string userName)
		{
			ConstructorValidator validator=new ConstructorValidator(
				MethodBase.GetCurrentMethod(),userName);
			validator.Validate();
			_userName = userName;
			try
			{
				// get userInformation
				DalUser dalUser=new DalUser();
				_userId=dalUser.GetUserId(this.UserName);
			}
			catch( Exception e )
			{
				// throw this if we cannot access the Dal.
				throw new Exceptions.DataSourceUnavailableException( e );
			}
		}
		#endregion ctor

		#region properties
		/// <summary>
		/// Underlying UserName
		/// </summary>
		public string UserName
		{
			get{return _userName;}
		}
		/// <summary>
		/// Returns the UserId corresponding to the UserName
		/// </summary>
		public int UserId
		{
			get{return _userId;}
		}
		#endregion properties

		#region public methods
		/// <summary>
		/// Method to create the Log used by derived classes.
		/// </summary>
		/// <param name="logEntry"></param>
		/// <returns></returns>
		public Log CreateLog( LogEntry logEntry )
		{
			logEntry.UserId = _userId;
			return new Log( logEntry );
		}
		#endregion public methods
	}

	/// <summary>
	/// This derived class adds the containment integration with the site
	/// infomration data layer into another base class.
	/// </summary>
	public class BllCustomer : BllBase
	{
		#region member variables
		/// <summary>
		/// Holds the site code after the populate function is called
		/// </summary>
		protected string _siteCode=null;
		/// <summary>
		/// Holds the site id after the populate function is called
		/// </summary>
		protected int _siteId=0;
		#endregion member variables

		#region ctor
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="userName"></param>
		public BllCustomer( string userName ):base( userName ){}
		#endregion ctor

		#region properties
		/// <summary>
		/// Return the site id member if populated. Otherwise, 0.
		/// </summary>
		protected int SiteId
		{
			get{return _siteId;}
		}
		/// <summary>
		/// Return the site code member if populated. Otherwise, null.
		/// </summary>
		protected string SiteCode
		{
			get{return _siteCode;}
		}
		#endregion properties

		#region protected methods
		/// <summary>
		/// This method is completely for convenience and to prevent the cluttering of
		/// code. 
		/// </summary>
		/// <param name="accountNumber">
		/// The site number from which to garner the site information.
		/// </param>
		/// <exception cref="InvalidAccountNumberException">Thrown when the accountNumber
		/// passed in does not exist in the system of record.
		/// </exception>
		/// <exception cref="DataSourceUnavailableException">Thrown when an underlying
		/// Datasource is unavailable or a the DataAccessLayer could not login to its
		/// underlying DataSource.
		/// </exception>
		/// <exception cref="UnexpectedSystemException">A catch all exception. If this error
		/// occurs a completely unknown or random problem just occurred. Further information
		/// can be garnered by looking at the innerException (if presented to you). If inner
		/// exceptions are not presented to you, then look in the EventViewer.
		/// </exception>
		protected void PopulateSiteInfo(CustomerAccountNumber accountNumber)
		{
			try
			{
				_siteCode = DalSiteCode.Instance.GetSiteCode( 
					accountNumber.CompanyNumber, accountNumber.DivisionNumber );
				_siteId = DalSiteCode.Instance.GetSiteId( accountNumber.AccountNumber16 );
			}
			catch( DataSourceException dse )
			{
				throw new DataSourceUnavailableException( dse );
			}
			catch( RecordDoesNotExistException rdnee )
			{
				throw new InvalidAccountNumberException( string.Format(
					"AccountNumber '{0}' does not exist in the system of record.", accountNumber.AccountNumber16 ), rdnee );
			}
			catch( Exception e )
			{
				throw new UnexpectedSystemException( e );
			}
		}
		/// <summary>
		/// Sets siteid/sitecode values for this class instance.
		/// </summary>
		/// <param name="siteId"></param>
		protected void PopulateSiteInfo(int siteId)
		{
			try
			{
				_siteCode=DalSiteCode.Instance.GetSiteCode(siteId);
				_siteId=siteId;
			}
			catch(DataSourceException dse)
			{
				throw new DataSourceUnavailableException(dse);
			}
			catch(RecordDoesNotExistException rdnee)
			{
				throw new InvalidSiteIdException(string.Format("Invalid SiteId '{0}'.",siteId),rdnee);
			}
			catch( Exception e )
			{
				throw new UnexpectedSystemException( e );
			}
		}
		#endregion protected methods
		
	}
}
