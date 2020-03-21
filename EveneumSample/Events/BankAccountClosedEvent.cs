using System;
using System.ComponentModel.DataAnnotations;

namespace EveneumSample.Events
{
    public class BankAccountClosedEvent
    {
        public string AccountNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
