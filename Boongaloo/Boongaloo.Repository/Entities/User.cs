﻿using System;
using Boongaloo.DTO.Enums;

namespace Boongaloo.Repository.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string IdsrvUniqueId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string About { get; set; }
        public GenderEnum Gender { get; set; }

        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
