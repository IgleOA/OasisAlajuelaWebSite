using ET;
using DAL;
using System.Collections.Generic;
using System;

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

        public ResourceTypes ResourceTypeDetail(int TypeID)
        {
            return RDAL.TypeDetail(TypeID);
        }

        public List<ResourceTypes> TypeList(string UserName)
        {
            return RDAL.TypeList(UserName);
        }

        public List<Resources> ResourceList(int ResourceTypeID, DateTime Date)
        {
            return RDAL.ResourceList(ResourceTypeID, Date);
        }

        public List<Resources> History(int ResourceTypeID)
        {
            return RDAL.History(ResourceTypeID);
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
