using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;

namespace Boongaloo.DTO.Applicant
{
    public class UserDataContext : IDisposable
    {
        private string _fileDbLocation;

        public List<UserData> Users { get; set; } 

        public UserDataContext(string fileLocation)
        {
            _fileDbLocation = fileLocation;

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (fileSystem.TryGetFileInfo(_fileDbLocation, out fi))
            {

                var json = File.ReadAllText(fi.PhysicalPath);
                var result = JsonConvert.DeserializeObject<List<UserData>>(json);

                Users = result.ToList();
            }
        }

        public bool SaveChanges()
        {
            // TODO: No matter this is temporary, we could optimize it so we can update but not recreate it every time.
            // write trips to json file, overwriting the old one

            var json = JsonConvert.SerializeObject(Users);

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (fileSystem.TryGetFileInfo(_fileDbLocation, out fi))
            {
                File.WriteAllText(fi.PhysicalPath, json);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            // Do a cleanup
        }
    }
}
