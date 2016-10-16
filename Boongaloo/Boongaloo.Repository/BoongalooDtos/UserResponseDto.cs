using System;
using System.Collections.Generic;
using Boongaloo.Repository.Enums;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string IdsrvUniqueId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string About { get; set; }
        public GenderEnum Gender { get; set; }

        public IEnumerable<LanguageDto> Langugages { get; set; }
        public IEnumerable<GroupDto> Groups { get; set; }
         
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
