using DAL;
using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class GroupsBL
    {
        private GroupsDAL GDAL = new GroupsDAL();
        public List<Groups> List()
        {
            return GDAL.List();
        }

        public List<Groups> FullList()
        {
            return GDAL.FullList();
        }

        public List<Groups> ListbyRT(int ResourceTypeID)
        {
            return GDAL.ListbyRT(ResourceTypeID);
        }
        public List<Groups> ListbyUser(int UserID)
        {
            return GDAL.ListbyUser(UserID);
        }

        public bool AddUserGroup(UsersGroups UG, string InsertUser)
        {
            return GDAL.AddUserGroup(UG, InsertUser);
        }

        public bool AddNew(Groups GP, string InsertUser)
        {
            return GDAL.AddNew(GP, InsertUser);
        }

        public bool Update(Groups GP,string InsertUser)
        {
            return GDAL.Udpdate(GP, InsertUser);
        }

        public bool AddRTGroup(ResourcesGroups RG, string InsertUser)
        {
            return GDAL.AddRTGroup(RG, InsertUser);
        }

        public bool RemoveUG(UsersGroups UG, string InsertUser)
        {
            return GDAL.RemoveUG(UG, InsertUser);
        }

        public bool RemoveRG(ResourcesGroups RG, string InsertUser)
        {
            return GDAL.RemoveRG(RG, InsertUser);
        }

        public Groups Details(int GroupID)
        {
            return GDAL.Details(GroupID);
        }

        public List<UsersGroups> UserList(int id)
        {
            return GDAL.UserList(id);
        }

        public List<ResourcesGroups> RTList(int id)
        {
            return GDAL.RTList(id);
        }
    }
}
