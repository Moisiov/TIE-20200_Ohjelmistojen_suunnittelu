using FinlandiaHiihtoAPI.Utils;

namespace FinlandiaHiihtoAPI.Enums
{
    public enum FinlandiaAgeGroup
    {
        [RequestValue("alle35")]
        LessThan35 = 0,
    
        [RequestValue("35")]
        ThirtyFive = 35,
    
        [RequestValue("40")]
        Forty = 40,
    
        [RequestValue("45")]
        FortyFive = 45,
    
        [RequestValue("50")]
        Fifty = 50,
    
        [RequestValue("55")]
        FiftyFive = 55,
    
        [RequestValue("60")]
        Sixty = 60,
    
        [RequestValue("65")]
        SixtyFive = 65,
    
        [RequestValue("70")]
        Seventy = 70,
    
        [RequestValue("75")]
        SeventyFive = 75,
    
        [RequestValue("yli80")]
        OverEighty = 1000
    }
}
