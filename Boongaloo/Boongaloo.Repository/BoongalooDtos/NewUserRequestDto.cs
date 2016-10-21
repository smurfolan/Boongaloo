using System;
using System.Collections.Generic;
using Boongaloo.DTO.Enums;
using System.ComponentModel.DataAnnotations;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class NewUserRequestDto
    {
        public int Id { get; set; }

        [Required]
        public string IdsrvUniqueId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string About { get; set; }
        public GenderEnum Gender { get; set; }

        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }

        public IEnumerable<int> LanguageIds { get; set; }
        public IEnumerable<int> GroupIds { get; set; } 
    }
}
