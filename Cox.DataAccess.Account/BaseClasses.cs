using System;

using Cox.DataAccess;

namespace Cox.DataAccess.Account
{
	/// <summary>
	/// Base class that all Dal* should derive from within this assembly.
	/// </summary>
	public abstract class DalAccountBase : Dal
	{
		#region constants
		/// <summary>
		/// String constant containing the connection key to the configuration block.
		/// </summary>
		protected const string __accountConnectionKey="accountConnectionString";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DalAccountBase():base(__accountConnectionKey){}
		
		/// <summary>
		/// Constructor enabling you to override the connectionKey.
		/// </summary>
		/// <param name="connectionKey">Connection key to get connection info from the config file</param>
		public DalAccountBase(string connectionKey):base(connectionKey){}
		#endregion ctors
	}
	/// <summary>
	/// This class provides basic siteid/sitecode translation used by
	/// DataAccess Layer assemblies.
	/// 
	/// *******************************************************************
	/// At some point we may want to move this class to a location so it can 
	/// be used by multiple assemblies; however, at this time it is not used
	/// elsewhere. Therefore it is not practical to create another assembly
	/// to contain this one class.
	/// *******************************************************************
	/// </summary>
	public abstract class DalCustomer : DalAccountBase
	{
		#region member variables
		/// <summary>
		/// member variable containing siteId value.
		/// </summary>
		protected int _siteId=0;
		/// <summary>
		/// member variable containing siteCode value.
		/// </summary>
		protected string _siteCode=null;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		public DalCustomer(int siteId,string siteCode)
			:this(__accountConnectionKey,siteId,siteCode){}
		/// <summary>
		/// The constructor allowing you to set the connectionKey
		/// </summary>
		/// <param name="connectionKey"></param>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		public DalCustomer(string connectionKey,int siteId,string siteCode)
			:base(connectionKey)
		{
			_siteId=siteId;
			_siteCode=siteCode!=null?siteCode.Trim():null;
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Property containing SiteId value.
		/// </summary>
		public int SiteId
		{
			get{return _siteId;}
		}
		/// <summary>
		/// Property containing SiteCode value.
		/// </summary>
		public string SiteCode
		{
			get{return _siteCode;}
		}
		#endregion properties
	}
}
