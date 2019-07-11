using System;
namespace EquityOrderComponent
{
    public interface IOrderPlaced
    {
        event OrderPlacedEventHandler OrderPlaced;
    }
}
