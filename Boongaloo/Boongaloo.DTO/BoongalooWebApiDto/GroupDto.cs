using System.Collections.Generic;
using Boongaloo.DTO.Enums;

namespace Boongaloo.DTO.BoongalooWebApiDto
{
    public class GroupDto
    {
        public bool NewAreaGroup { get; set; }

        public string Name { get; set; }

        public IEnumerable<TagEnum> Tags { get; set; }

        #region These should not be null if NewAreaGroup is true
        public decimal? Latitutude { get; set; }
        public decimal? Longtitude { get; set; }
        public RadiusEnum? Radius { get; set; }
        #endregion

        #region This should not be null if NewAreGroup is false
        public IEnumerable<int> AreaIds { get; set; }
        #endregion
    }
}
