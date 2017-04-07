using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin.FileSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoongalooCompany.Repository.Entities
{
    // TODO: Extract generic out of this one and UserContext
    public class RefreshTokenContext
    {
        private string _fileDBLocation;

        public RefreshTokenContext(string fileDBLocation)
        {
            _fileDBLocation = fileDBLocation;

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (fileSystem.TryGetFileInfo(_fileDBLocation, out fi))
            {

                var json = File.ReadAllText(fi.PhysicalPath);
                var result = JsonConvert.DeserializeObject<List<RefreshToken>>(json, new ClaimConverter());

                RefreshTokens = result.ToList();
            }
        }

        public IList<RefreshToken> RefreshTokens { get; set; }

        public bool SaveChanges()
        {
            // write trips to json file, overwriting the old one

            var json = JsonConvert.SerializeObject(RefreshTokens, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

            var fileSystem = new Microsoft.Owin.FileSystems.PhysicalFileSystem("");

            IFileInfo fi;
            if (fileSystem.TryGetFileInfo(_fileDBLocation, out fi))
            {
                File.WriteAllText(fi.PhysicalPath, json);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            // cleanup
        }
    }

    class ClaimConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(System.Security.Claims.Claim));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string type = (string)jo["Type"];
            string value = (string)jo["Value"];
            string valueType = (string)jo["ValueType"];
            string issuer = (string)jo["Issuer"];
            string originalIssuer = (string)jo["OriginalIssuer"];
            return new Claim(type, value, valueType, issuer, originalIssuer);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
