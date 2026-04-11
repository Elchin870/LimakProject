namespace Limak.Domain.Enums;

public enum PackageStatus
{
    WaitingForDeclaration,   
    Declared,                
    NeedCorrection,          
    ReadyForWarehouse,       
    InWarehouse,             
    InTransit,               
    ArrivedToCountry,       
    ReadyForDelivery,        
    Delivered,               
    Cancelled
}
