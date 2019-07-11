using System;
namespace EquityOrderComponent
{
    public interface IOrderErrored
    {
        event OrderErroredEventHandler OrderErrored;
    }
}
