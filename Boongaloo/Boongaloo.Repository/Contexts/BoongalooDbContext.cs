using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boongaloo.Repository.Entities;
using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;

namespace Boongaloo.Repository.Contexts
{
    public class BoongalooDbContext : IDisposable
    {
        private readonly string _groupStoreFileName = "DataStorage/groupStore.json";
        private readonly string _areaStoreFileName = "DataStorage/areaStore.json";
        private readonly string _userStoreFileName = "DataStorage/userStore.json";

        private readonly string _areaToGroupStoreFileName = "DataStorage/areaGroupBridgeStore.json";
        private readonly string _groupToUserStoreFileName = "DataStorage/groupUserBridgeStore.json";

        public BoongalooDbContext()
        {
            this.ExtractGroupsFromFile(this._groupStoreFileName);
            this.ExtractAreasFromFile(this._areaStoreFileName);
            this.ExtractUsersFromFile(this._userStoreFileName);

            this.ExtractAreaToGroupsFromFile(this._areaToGroupStoreFileName);
            this.ExtractGroupToUsersFromFile(this._groupToUserStoreFileName);
        }

        public IList<Group> Groups { get; set; }
        public IList<Area> Areas { get; set; }
        public IList<User> Users { get; set; }

        // Bridges. Never have to be present in the UOW object. Access them only through the repositories.
        public IList<AreaToGroup> AreaToGroup { get; set; }
        public IList<GroupToUser> GroupToUser { get; set; }

        public bool SaveChanges()
        {
            // TODO: Could be made async since there's no dependency between the files being saved.
            // write trips to json file, overwriting the old one
            var groupsSaved = this.SaveEntities(_groupStoreFileName);
            var areasSaved = this.SaveEntities(_areaStoreFileName);
            var usersSaved = this.SaveEntities(_userStoreFileName);

            var areaToGroupSaved = this.SaveEntities(_areaToGroupStoreFileName);
            var groupToUserSaved = this.SaveEntities(_groupToUserStoreFileName);
            
            return groupsSaved 
                && areasSaved 
                && usersSaved
                && areaToGroupSaved
                && groupToUserSaved;
        }

        public void Dispose()
        {
            // cleanup
        }

        #region Helper methods
        private void ExtractUsersFromFile(string userStoreFileName)
        {
            var fi = this.GetStoreFileInfo(userStoreFileName);

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<User>>(json);

            Users = result.ToList();
        }

        private void ExtractAreasFromFile(string areaStoreFileName)
        {
            var fi = this.GetStoreFileInfo(areaStoreFileName);

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<Area>>(json);

            Areas = result.ToList();
        }

        private void ExtractGroupsFromFile(string groupStoreFileName)
        {
            var fi = this.GetStoreFileInfo(groupStoreFileName);

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<Group>>(json);

            Groups = result.ToList();
        }

        private void ExtractGroupToUsersFromFile(string groupToUserStoreFileName)
        {
            var fi = this.GetStoreFileInfo(groupToUserStoreFileName);

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<AreaToGroup>>(json);

            AreaToGroup = result.ToList();
        }

        private void ExtractAreaToGroupsFromFile(string areaToGroupStoreFileName)
        {
            var fi = this.GetStoreFileInfo(areaToGroupStoreFileName);

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<GroupToUser>>(json);

            GroupToUser = result.ToList();
        }

        private IFileInfo GetStoreFileInfo(string resourceFileLocation)
        {
            var resFileLocation = resourceFileLocation;

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (!fileSystem.TryGetFileInfo(resFileLocation, out fi))
                throw new FileNotFoundException("File:" + resourceFileLocation + " is not available.");

            return fi;
        }

        private bool SaveEntities(string storeFileName)
        {
            var json = "";

            if (storeFileName == _groupStoreFileName)
                json = JsonConvert.SerializeObject(Groups);
            if (storeFileName == _areaStoreFileName)
                json = JsonConvert.SerializeObject(Areas);
            if (storeFileName == _userStoreFileName)
                json = JsonConvert.SerializeObject(Users);

            if(storeFileName == _areaToGroupStoreFileName)
                json = JsonConvert.SerializeObject(AreaToGroup);
            if (storeFileName == _groupToUserStoreFileName)
                json = JsonConvert.SerializeObject(GroupToUser);

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (!fileSystem.TryGetFileInfo(storeFileName, out fi))
                return false;

            File.WriteAllText(fi.PhysicalPath, json);
            return true;
        }
        #endregion
    }
}
