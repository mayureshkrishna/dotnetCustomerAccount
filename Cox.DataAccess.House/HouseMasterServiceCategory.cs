using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cox.DataAccess.House
{
    public class HouseMasterServiceCategory
    {

        #region Member Variables

        protected int _siteId;
        protected int _houseNumber ;
        protected string _serviceCategoryCode = string.Empty;
        protected string _serviceableStatusCode = string.Empty;
        protected DateTime _snapShotDate ;

        #endregion Member Variables

        #region Ctors
        /// <summary>
		/// Default ctor
		/// </summary>
        public HouseMasterServiceCategory() { }
        
        #endregion Ctors

        /// <summary>
        /// Site Id
        /// </summary>
        [XmlElement("SiteId")]
        public int SiteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

        /// <summary>
        /// house number
        /// </summary>
        [XmlElement("HouseNumber")]
        public int HouseNumber
        {
            get { return _houseNumber; }
            set { _houseNumber = value; }
        }

        /// <summary>
        /// Service Category Code
        /// </summary>
        [XmlElement("ServiceCategoryCode")]
        public string ServiceCategoryCode
        {
            get { return _serviceCategoryCode; }
            set { _serviceCategoryCode = value; }
        }

        /// <summary>
        /// Serviceable Status Code
        /// </summary>
        [XmlElement("ServiceableStatusCode")]
        public string ServiceableStatusCode
        {
            get { return _serviceableStatusCode; }
            set { _serviceableStatusCode = value; }
        }
    }
}
