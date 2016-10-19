using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Boongaloo.Repository.Entities;
using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;

namespace Boongaloo.Repository.Contexts
{
    public class BoongalooDbContext : IDisposable
    {
        private static readonly string _groupStoreFileName = "DataStorage/groupStore.json";
        private static readonly string _areaStoreFileName = "DataStorage/areaStore.json";
        private static readonly string _userStoreFileName = "DataStorage/userStore.json";
                 
        private static readonly string _areaToGroupStoreFileName = "DataStorage/areaGroupBridgeStore.json";
        private static readonly string _groupToUserStoreFileName = "DataStorage/groupUserBridgeStore.json";
        private static readonly string _groupToTagStoreFileName = "DataStorage/groupTagBridgeStore.json";
        private static readonly string _userToLanguagesStoreFileName = "DataStorage/userLanguageBridgeStore.json";

        public BoongalooDbContext()
            :this(
                 _groupStoreFileName, 
                 _areaStoreFileName, 
                 _userStoreFileName, 
                 _areaToGroupStoreFileName, 
                 _groupToUserStoreFileName, 
                 _groupToTagStoreFileName,
                 _userToLanguagesStoreFileName){}

        public BoongalooDbContext(
            string groupStoreFile, 
            string areaStoreFile, 
            string userStoreFile,
            string areaToGroupStoreFile,
            string groupToUserStoreFile,
            string groupToTagStoreFile,
            string userToLanguageStoreFile)
        {
            this.ExtractGroupsFromFile(groupStoreFile);
            this.ExtractAreasFromFile(areaStoreFile);
            this.ExtractUsersFromFile(userStoreFile);

            this.ExtractAreaToGroupsFromFile(areaToGroupStoreFile);
            this.ExtractGroupToUsersFromFile(groupToUserStoreFile);
            this.ExtractGroupToTagsFromFile(groupToTagStoreFile);
            this.ExtractUserToLanguagesFromFile(userToLanguageStoreFile);
        }
        

        public IList<Group> Groups { get; set; }
        public IList<Area> Areas { get; set; }
        public IList<User> Users { get; set; }

        public IList<Language> Languages { get
            {
                return new List<Language>()
                {
                    new Language (1, "Bulgarian"),
                    new Language (1, "English"),
                    new Language (1, "French"),
                    new Language (1, "German"),
                    new Language (1, "Russian")
                };
            }
        }
        public IList<Tag> Tags { get {
                return new List<Tag>()
                {
                    new Tag(1, "Help"),
                    new Tag(2, "School"),
                    new Tag(3, "Fun"),
                    new Tag(4, "Sport"),
                    new Tag(5, "University"),
                    new Tag(6, "Other")
                };
            }
        }

        // Bridges. Never have to be present in the UOW object. Access them only through the repositories.
        public IList<AreaToGroup> AreaToGroup { get; set; }
        public IList<GroupToUser> GroupToUser { get; set; }
        public IList<GroupToTag> GroupToTag { get; set; }
        public IList<UserToLanguage> UserToLangauge { get; set; }

        public bool SaveChanges()
        {
            // TODO: Could be made async since there's no dependency between the files being saved.
            // write trips to json file, overwriting the old one
            var groupsSaved = this.SaveEntities(_groupStoreFileName);
            var areasSaved = this.SaveEntities(_areaStoreFileName);
            var usersSaved = this.SaveEntities(_userStoreFileName);

            var areaToGroupSaved = this.SaveEntities(_areaToGroupStoreFileName);
            var groupToUserSaved = this.SaveEntities(_groupToUserStoreFileName);
            var groupToTagSaved = this.SaveEntities(_groupToTagStoreFileName);
            var userToLanguagesSaved = this.SaveEntities(_userToLanguagesStoreFileName);
            
            return groupsSaved 
                && areasSaved 
                && usersSaved
                && areaToGroupSaved
                && groupToUserSaved
                && groupToTagSaved
                && userToLanguagesSaved;
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
            var result = JsonConvert.DeserializeObject<List<GroupToUser>>(json);

            GroupToUser = result.ToList();
        }

        private void ExtractAreaToGroupsFromFile(string areaToGroupStoreFileName)
        {
            var fi = this.GetStoreFileInfo(areaToGroupStoreFileName);

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<AreaToGroup>>(json);

            AreaToGroup = result.ToList();
        }

        private void ExtractGroupToTagsFromFile(string groupToTagStoreFileName)
        {
            var fi = this.GetStoreFileInfo(groupToTagStoreFileName);

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<GroupToTag>>(json);

            GroupToTag = result.ToList();
        }

        private void ExtractUserToLanguagesFromFile(string userToLanguagesStoreFileName)
        {
            var fi = this.GetStoreFileInfo(userToLanguagesStoreFileName);

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<UserToLanguage>>(json);

            UserToLangauge = result.ToList();
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
            if (storeFileName == _groupToTagStoreFileName)
                json = JsonConvert.SerializeObject(GroupToTag);
            if (storeFileName == _userToLanguagesStoreFileName)
                json = JsonConvert.SerializeObject(UserToLangauge);

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
