using System;
using System.ComponentModel.DataAnnotations;

namespace EveneumSample.Events
{
    public class BankAccountOpenedEvent
    {
        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
