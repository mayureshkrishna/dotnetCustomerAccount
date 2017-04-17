using System;
using System.Collections;
using Cox.Validation;

namespace Cox.BusinessLogic
{
	/// <summary>
	/// Summary description for ExceptionFormatter.
	/// </summary>
	public class ExceptionFormatter
	{
		#region constants
		/// <summary>
		/// header for message to be returned.
		/// </summary>
		private const string __header="The following errors occurred:\r\n";
		#endregion constants

		#region member variables
		/// <summary>
		/// Exception we are formatting.
		/// </summary>
		protected Exception _exception=null;
		/// <summary>
		/// The formatter.
		/// </summary>
		protected IValidatorFormatProvider _formatProvider=null;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		/// <param name="exception"></param>
		public ExceptionFormatter(Exception exception)
		{
			_exception=exception;
			MultiItemFormatProvider provider=new MultiItemFormatProvider();
			provider.HeaderText=__header;
			_formatProvider=(IValidatorFormatProvider)provider;
		}
		/// <summary>
		/// Constructor taking a different format provider.
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="formatProvider"></param>
		public ExceptionFormatter(Exception exception,IValidatorFormatProvider formatProvider)
		{
			_exception=exception;
			_formatProvider=formatProvider;
		}
		#endregion ctors

		#region methods
		/// <summary>
		/// Workhorse of class. This methods performs the actual formatting
		/// of the exception information.
		/// </summary>
		/// <returns></returns>
		public string Format()
		{
			ArrayList arrList=new ArrayList();
			Exception exc=_exception;
			while(exc!=null)
			{
				arrList.Add(exc.Message);
				exc=exc.InnerException;
			}
			return _formatProvider.Format(arrList);
		}
		#endregion methods
	}
}
