using System;

namespace DatingCode.BusinessModels.Basics
{
    public struct ObjectType
    {
        public ObjectType(int id, string code)
        {
            if (id <= 0)
                throw new ArgumentException($"{id} <= 0", nameof(id));
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(code));


            Id = id;
            Code = code;
        } 

        public int Id;
        public string Code;
    }
}
