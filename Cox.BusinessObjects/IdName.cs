using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Cox.BusinessObjects
{
    /// <summary>
    /// Abstract class for id/name code type classes.
    /// </summary>
    [Serializable()]
    public abstract class IdName
    {
        #region member variables
        /// <summary>
        /// Member variable for AffiliateClassificationId
        /// </summary>
        protected int? _id=null;       
        /// <summary>
        /// Member variable for Name
        /// </summary>
        protected string _name = null;
        #endregion member variables

        #region ctors
        /// <summary>
        /// Default constructor
        /// </summary>
        public IdName(){}
        /// <summary>
        /// Initialization constructor to populate member variables
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public IdName(int? id, string name)
        {
            _id = id;
            _name = name;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// Get / Set Id
        /// </summary>
        [XmlElement("Id")]
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Get / Set Name
        /// </summary>
        [XmlElement("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion properties
    }
}
