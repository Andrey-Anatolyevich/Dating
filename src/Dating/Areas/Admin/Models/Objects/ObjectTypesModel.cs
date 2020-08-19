using Dating.Models;
using DatingCode.BusinessModels.Basics;
using System.Collections.Generic;

namespace Dating.Areas.Admin.Models.Objects
{
    public class ObjectTypesModel : BaseViewModel
    {
        public IEnumerable<ObjectType> ObjectTypes;
    }
}
