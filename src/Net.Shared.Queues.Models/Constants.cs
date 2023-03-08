namespace Net.Shared.Queues.Models;

public static class Constants
{
    public static class Enums
    {
        public static class RabbitMq
        {
            public enum ExchangeNames
            {
                In,
                Apc,
                Sync
            }
            public enum ExchangeTypes
            {
                Direct,
                Fannout,
                Topic,
                Headers
            }
        }
    }
}