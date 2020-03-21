using System;
using System.ComponentModel.DataAnnotations;

namespace EveneumSample.Events
{
    public class MoneyDepositedEvent
    {
        public string AccountNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Source { get; set; }
    }
}
