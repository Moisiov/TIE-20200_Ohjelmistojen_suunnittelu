using System.Diagnostics.CodeAnalysis;
using FinlandiaHiihtoAPI.Utils;

namespace FinlandiaHiihtoAPI.Enums
{
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used dynamically")]
    public enum FinlandiaCompetitionType
    { 
        [RequestValue("P50")]
        P50, 
        
        [RequestValue("V50")]
        V50, 
        
        [RequestValue("P100")]
        P100,  
        
        [RequestValue("P32")]
        P32, 
        
        [RequestValue("P20")]
        P20, 
        
        [RequestValue("V20")]
        V20, 
        
        [RequestValue("V32")]
        V32, 
        
        [RequestValue("V20jun")]
        // ReSharper disable once InconsistentNaming
        V20jun, 
        
        [RequestValue("P42")]
        P42, 
        
        [RequestValue("V42")]
        V42, 
        
        [RequestValue("P30")]
        P30, 
        
        [RequestValue("P44")]
        P44, 
        
        [RequestValue("P60")]
        P60, 
        
        [RequestValue("P62")]
        P62, 
        
        [RequestValue("P25")]
        P25, 
        
        [RequestValue("P35")]
        P35, 
        
        [RequestValue("P45")]
        P45,
        
        [RequestValue("P52")]
        P52, 
        
        [RequestValue("P53")]
        P53, 
        
        [RequestValue("P75")]
        P75, 
        
        [RequestValue("V30")]
        V30, 
        
        [RequestValue("V45")]
        V45, 
        
        [RequestValue("V53")]
        V53
    }
}
