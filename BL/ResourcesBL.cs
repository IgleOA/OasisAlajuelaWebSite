using ET;
using DAL;
using System.Collections.Generic;

namespace BL
{
    public class ResourcesBL
    {
        private ResourcesDAL RDAL = new ResourcesDAL();

        public bool AddNewResourceType(ResourceTypes RT, string InsertUser)
        {
            return RDAL.AddNewResourceType(RT, InsertUser);
        }

        public int AddNewResource(Resources RT, string InsertUser)
        {
            return RDAL.AddNewResource(RT, InsertUser);
        }

        public List<ResourceTypes> TypeList(string UserName)
        {
            return RDAL.TypeList(UserName);
        }

        public List<Resources> ResourceList(int ResourceTypeID, bool ActiveFlag)
        {
            return RDAL.ResourceList(ResourceTypeID, ActiveFlag);
        }

        public Resources ResourceDetails(int ResourceID)
        {
            return RDAL.ResourceDetails(ResourceID);
        }

        public bool Update(Resources MS, string InsertUser)
        {
            return RDAL.Update(MS, InsertUser);
        }
    }
}
