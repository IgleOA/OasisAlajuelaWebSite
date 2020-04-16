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

        public bool AddNewResource(Resources RT, string InsertUser)
        {
            return RDAL.AddNewResource(RT, InsertUser);
        }

        public List<ResourceTypes> TypeList(bool ActiveFlag)
        {
            return RDAL.TypeList(ActiveFlag);
        }

        public List<Resources> ResourceList(int ResourceTypeID, bool ActiveFlag)
        {
            return RDAL.ResourceList(ResourceTypeID, ActiveFlag);
        }
    }
}
