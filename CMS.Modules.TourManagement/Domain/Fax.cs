using System;
using System.Collections;

namespace CMS.Modules.TourManagement.Domain
{

    #region Fax

    /// <summary>
    /// Fax object for NHibernate mapped table 'tmh_Fax'.
    /// </summary>
    public class Fax
    {
        #region Member Variables

        protected int _id;
        protected string _name;
        protected string _number;
        protected int _owner;
        protected string _ownerType;
        protected string _type;

        #endregion

        #region Constructors

        public Fax()
        {
            _id = -1;
        }

        public Fax(string name, string type, string number, int owner, string ownerType)
        {
            _name = name;
            _type = type;
            _number = number;
            _owner = owner;
            _ownerType = ownerType;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value);
                _name = value;
            }
        }

        public virtual string Type
        {
            get { return _type; }
            set
            {
                if (value != null && value.Length > 10)
                    throw new ArgumentOutOfRangeException("Invalid value for Type", value, value);
                _type = value;
            }
        }

        public virtual string OwnerType
        {
            get { return _ownerType; }
            set { _ownerType = value; }
        }

        public virtual string Number
        {
            get { return _number; }
            set
            {
                if (value != null && value.Length > 20)
                    throw new ArgumentOutOfRangeException("Invalid value for Number", value, value);
                _number = value;
            }
        }

        public virtual int Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        #endregion

        #region -- Methods --

        public static Fax GetFaxFromString(string value, int ownerid, Type ownerType)
        {
            IList splited = value.Split('\\');
            if (splited.Count == 3)
            {
                Fax fax = new Fax();
                if ((string) splited[2] != "-1")
                {
                    fax.Id = Convert.ToInt32(splited[2]);
                }
                fax.Name = (string) splited[0];
                fax.Number = (string) splited[1];
                fax.Owner = ownerid;
                fax.OwnerType = ownerType.Name;
                fax.Type = "Fax";
                return fax;
            }
            return null;
        }

        public static Fax GetFaxFromString(string value, int ownerid)
        {
            IList splited = value.Split('\\');
            if (splited.Count == 3)
            {
                Fax fax = new Fax();
                if ((string)splited[2] != "-1")
                {
                    fax.Id = Convert.ToInt32(splited[2]);
                }
                fax.Name = (string)splited[0];
                fax.Number = (string)splited[1];
                fax.Owner = ownerid;
                fax.OwnerType = "Hotel";
                fax.Type = "Fax";
                return fax;
            }
            return null;
        }

        #endregion
    }

    #endregion
}