using System;
using System.Collections.Generic;

namespace DatingCode.Config
{
    public class ConfigValuesCollection 
    {
        public ConfigValuesCollection()
        {
            _valuesDic = new Dictionary<string, string>();
        }

        private Dictionary<string, string> _valuesDic;

        public void AddValue(string key, string value)
        {
            _valuesDic.Add(key, value);
        }

        public string GetValue(string key)
        {
            var result = _valuesDic[key];
            return result;
        }

        public string GetPgConnectionString()
        {
            return GetValue("PgConnectionString");
        }

        public string GetUserFilesDirectory()
        {
            return GetValue("UserFiles:Directory");
        }

        public string GetUserFilesUrlPrefix()
        {
            return GetValue("UserFiles:AccessUrlPrefix");
        }
    }
}
