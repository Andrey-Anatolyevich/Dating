using System.Diagnostics;

namespace DatingCode.BusinessModels.Core
{
    [DebuggerDisplay("ID: '{Id}' Code: {Code}")]
    public class LocaleInfo
    {
        public int Id;
        public string Code;
        public string Comment;
    }
}
