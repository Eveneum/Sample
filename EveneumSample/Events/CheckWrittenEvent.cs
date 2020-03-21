using System;
using System.ComponentModel.DataAnnotations;

namespace EveneumSample.Events
{
    public class CheckWrittenEvent
    {
        public string AccountNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int Number { get; set; }

        public decimal Amount { get; set; }

        public string PayTo { get; set; }
    }
}
