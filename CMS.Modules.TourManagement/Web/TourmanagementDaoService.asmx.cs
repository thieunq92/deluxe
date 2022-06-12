using System.Collections;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Serialization;
using CMS.Modules.TourManagement.Domain;
using CMS.Web.UI;

namespace CMS.Modules.TourManagement.Web
{    
    public class GenericLocation
    {
        public int Id;
        public string Name;
        public GenericLocation()
        {
            
        }
        public GenericLocation(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    /// <summary>
    /// Summary description for TourmanagementDaoService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class TourmanagementDaoService : WebServiceBase
    {
        private readonly TourManagementModule TourModule;

        public TourmanagementDaoService()
            : base("CMS.Modules.TourManagement.TourManagementModule")
        {
            TourModule = (TourManagementModule) Module;
            // Default Constructor
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [XmlInclude(typeof(GenericLocation))]
        public IList LocationGetAll()
        {
            IList list = TourModule.LocationGetAll();
            IList result = new ArrayList();
            foreach (Location location in list)
            {
                result.Add(new GenericLocation(location.Id ,location.Name));
            }
            return result;
        }
        
        [WebMethod]
        [XmlInclude(typeof(GenericLocation))]
        public IList LocationGetAllByParentId(int i)
        {
            Location location = TourModule.LocationGetById(i);
            IList list = location.Children;
            IList result = new ArrayList();
            foreach (Location loc in list)
            {
                result.Add(new GenericLocation(loc.Id, loc.Name));
            }
            return result;
        }

        [WebMethod]
        [XmlInclude(typeof(GenericLocation))]
        public IList LocationGetAllByLevel(int i)
        {
            IList list = TourModule.LocationGetRoot();
            IList result = new ArrayList();
            foreach (Location loc in list)
            {
                result.Add(new GenericLocation(loc.Id, loc.Name));
            }
            return result;
        }
    }
}