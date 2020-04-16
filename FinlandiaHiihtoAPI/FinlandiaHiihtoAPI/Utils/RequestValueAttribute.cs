using System;

namespace FinlandiaHiihtoAPI.Utils
{
    public class RequestValue : Attribute
    {
        public readonly string Value;
        
        public RequestValue(string str)
        {
            Value = str;
        }
    }
}
