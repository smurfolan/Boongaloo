using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BoongalooCompany.Repository.Entities
{
    // TODO: Extract generic out of this one and RefreshTokenContext
    public class UserContext : IDisposable
    {
        private string _fileDBLocation;

        private Object readWriteFileLock = new Object();

        public UserContext(string fileDBLocation)
        {
            _fileDBLocation = fileDBLocation;
            this.InitializeUsersContext();
        }

        public IList<User> Users { get; set; }

        public bool SaveChanges()
        {
            lock (readWriteFileLock)
            {
                var json = JsonConvert.SerializeObject(Users);

                var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

                IFileInfo fi;
                if (fileSystem.TryGetFileInfo(_fileDBLocation, out fi))
                {
                    File.WriteAllText(fi.PhysicalPath, json);
                    return true;
                }

                return false;
            }
        }

        public void Dispose()
        {
            // cleanup
        }

        private void InitializeUsersContext()
        {
            lock (readWriteFileLock)
            {
                var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

                IFileInfo fi;
                if (fileSystem.TryGetFileInfo(_fileDBLocation, out fi))
                {

                    var json = File.ReadAllText(fi.PhysicalPath);
                    var result = JsonConvert.DeserializeObject<List<User>>(json);

                    Users = result.ToList();
                }
            }
        }
    }
}
