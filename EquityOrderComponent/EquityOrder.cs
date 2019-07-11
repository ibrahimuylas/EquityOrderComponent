using System;

namespace EquityOrderComponent
{
    public class EquityOrder : IEquityOrder
    {
        private readonly OrderParameters Parameters;
        private readonly IOrderService OrderService;

        private bool ShutDown;
        private readonly object LockObject = new object();

        public EquityOrder(IOrderService orderService, OrderParameters parameters)
        {
            OrderService = orderService;
            Parameters = parameters;
        }

        public event OrderPlacedEventHandler OrderPlaced;
        public event OrderErroredEventHandler OrderErrored;

        public void ReceiveTick(string equityCode, decimal price)
        {
            if (CanReceiveTick(price))
            {
                lock (LockObject)
                {
                    try
                    {
                        OrderService.Buy(equityCode, Parameters.Quantity, price);

                        OrderPlaced?.Invoke(new OrderPlacedEventArgs(equityCode, price));
                    }
                    catch (Exception ex)
                    {
                        OrderErrored?.Invoke(new OrderErroredEventArgs(equityCode, price, ex));
                    }
                    finally
                    {
                        ShutDown = true;
                    }
                }
            }
        }

        private bool CanReceiveTick(decimal price)
        {
            if (ShutDown || CheckPriceThreshold(price))
            {
                return false;
            }
            return true;
        }

        private bool CheckPriceThreshold(decimal price)
        {
            return price >= Parameters.PriceThreshold;
        }
    }
}
