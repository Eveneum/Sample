using Eveneum;
using EveneumSample.Events;

namespace EveneumSample.BusinessLogic
{
    public class EventSummarizer
    {
        public static decimal FindTotal(Stream? stream, ulong? version = null)
        {
            decimal total = 0;

            if (!stream.HasValue)
                return total;

            if (stream.Value.Snapshot.HasValue)
            {
                total = ((AccountSummary)stream.Value.Snapshot.Value.Data).Amount;
            }

            total += FindTotal(stream.Value.Events, version);

            return total;
        }

        public static decimal FindTotal(EventData[] array, ulong? version = null)
        {
            decimal total = 0;

            foreach (var data in array)
            {
                if (version.HasValue && data.Version > version)
                    return total;

                var body = data.Body;

                if (body.GetType().Equals(typeof(CheckWrittenEvent)))
                    total -= ((CheckWrittenEvent)body).Amount;
                else if (body.GetType().Equals(typeof(MoneyDepositedEvent)))
                    total += ((MoneyDepositedEvent)body).Amount;
            }

            return total;
        }
    }
}
