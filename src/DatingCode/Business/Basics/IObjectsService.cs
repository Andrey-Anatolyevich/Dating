using DatingCode.BusinessModels.Basics;
using DatingCode.Core;
using System.Collections.Generic;

namespace DatingCode.Business.Basics
{
    public interface IObjectsService
    {
        IEnumerable<ObjectItem> AllOfType(int typeId);
        Result Create(ObjectCreateInfo objCreate);
        Result Delete(int objectId);
        Result SetCode(int id, string code);
        IEnumerable<ObjectItem> ActiveOfType(int typeId);
        Maybe<ObjectItem> GetById(int objectId);
        Maybe<ObjectItem> GetByTypeIdAndCode(int typeId, string code);
    }
}
