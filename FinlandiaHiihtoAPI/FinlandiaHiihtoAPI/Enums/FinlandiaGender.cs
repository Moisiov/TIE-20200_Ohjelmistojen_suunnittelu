using FinlandiaHiihtoAPI.Utils;

namespace FinlandiaHiihtoAPI.Enums
{
    public enum FinlandiaGender
    {
        [RequestValue("M")]
        Male = 0,
        
        [RequestValue("N")]
        Female = 1,
    }
}
