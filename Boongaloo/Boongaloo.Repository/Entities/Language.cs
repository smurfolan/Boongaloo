﻿namespace Boongaloo.Repository.Entities
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Language(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
