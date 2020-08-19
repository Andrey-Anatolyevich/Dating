using DatingCode.BusinessModels.Basics;
using DatingCode.Core;
using System.Collections.Generic;

namespace DatingCode.Business.Basics
{
    public interface IObjectTypesService
    {
        IEnumerable<ObjectType> GetAll();
        Maybe<ObjectType> Get(int id);
        Maybe<ObjectType> Get(string code);
        Result Update(ObjectType objectType);
        Result<int> Create(string code);
    }
}
