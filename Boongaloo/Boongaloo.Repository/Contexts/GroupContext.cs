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
    public class GroupContext : IDisposable
    {
        private readonly string _resourceFileLocation;

        public GroupContext(string resourceFileLocation)
        {
            _resourceFileLocation = resourceFileLocation;

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (!fileSystem.TryGetFileInfo(_resourceFileLocation, out fi))
                return;

            var json = File.ReadAllText(fi.PhysicalPath);
            var result = JsonConvert.DeserializeObject<List<Group>>(json);

            Groups = result.ToList();
        }

        public IList<Group> Groups { get; set; }

        public bool SaveChanges()
        {
            // write trips to json file, overwriting the old one

            var json = JsonConvert.SerializeObject(Groups);

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (!fileSystem.TryGetFileInfo(_resourceFileLocation, out fi))
                return false;

            File.WriteAllText(fi.PhysicalPath, json);
            return true;
        }

        public void Dispose()
        {
            // cleanup
        }
    }
}
