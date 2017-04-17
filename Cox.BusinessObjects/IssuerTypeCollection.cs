using System;
using System.Collections.Generic;
using System.Collections;

namespace Cox.BusinessObjects
{
	/// <summary>
	///		A strongly-typed collection of <see cref="IssuerType"/> objects.
	/// </summary>
	[Serializable]
	public class IssuerTypeCollection : List<IssuerType>
	{
        #region ctors
        /// <summary>
        /// Default ctor.
        /// </summary>
        public IssuerTypeCollection() : base() { }
        /// <summary>
        /// Ctor taking another collection.
        /// </summary>
        /// <param name="collection"></param>
        public IssuerTypeCollection(IEnumerable<IssuerType> collection) : base(collection) { }
        /// <summary>
        /// Collection denoting capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public IssuerTypeCollection(int capacity) : base(capacity) { }
        #endregion ctors

        #region Convert
        /// <summary>
		///		Converts the array of integer values to an array of <c>IssuerType</c>.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static IssuerType[] Convert(int[] x)
		{
            return Array.ConvertAll(x, new Converter<int, IssuerType>(IntToIssuerType));
		}
		#endregion Convert

        #region IssuerTypeToInt
        /// <summary>
        /// Method to convert a IssuerType to an int.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int IssuerTypeToInt(IssuerType type)
        {
            return (int)type;
        }
        #endregion IssuerTypeToInt

        #region IntToIssuerType
        /// <summary>
        /// Method to convert a IssuerType to an int.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IssuerType IntToIssuerType(int x)
        {
            return (IssuerType)x;
        }
        #endregion IntToIssuerType
    }
}
