using System;
using System.Collections.Generic;
using Boongaloo.DTO.Enums;

namespace Boongaloo.DTO.BoongalooWebApiDto
{
    public class EditUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string About { get; set; }
        public GenderEnum Gender { get; set; }
        public IEnumerable<LanguagesEnum> Langugages { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
