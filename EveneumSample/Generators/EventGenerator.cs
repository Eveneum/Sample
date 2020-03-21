using Eveneum;
using EveneumSample.BusinessLogic;
using EveneumSample.Events;
using System;
using System.Collections.Generic;

namespace EveneumSample.Generators
{
    public static class EventGenerator
    {
        private static Random _random = new Random();

        public static BankAccountOpenedEvent BankAccountOpened()
        {
            return new BankAccountOpenedEvent
            {
                AccountNumber = RandomAccountNumber(),
                AccountName = RandomAccountName(),
                Date = DateTime.Today
            };
        }

        public static BankAccountClosedEvent BankAccountClosed(string AccountNumber = null)
        {
            return new BankAccountClosedEvent
            {
                AccountNumber = AccountNumber == null ? RandomAccountNumber() : AccountNumber,
                Date = DateTime.Today
            };
        }

        public static CheckWrittenEvent CheckWritten(string AccountNumber = null)
        {
            return new CheckWrittenEvent
            {
                AccountNumber = AccountNumber == null ? RandomAccountNumber() : AccountNumber,
                Date = DateTime.Today,
                Number = RandomCheckNumber(),
                Amount = RandomAmount(),
                PayTo = RandomCheckPayee()
            };
        }

        public static MoneyDepositedEvent DepositMoney(string AccountNumber = null)
        {
            return new MoneyDepositedEvent
            {
                AccountNumber = AccountNumber == null ? RandomAccountNumber() : AccountNumber,
                Date = DateTime.Today,
                Amount = RandomAmount(),
                Source = RandomDepositSource()
            };
        }

        public static AccountSummary MakeSummary(string AccountNumber = null)
        {
            return new AccountSummary
            {
                AccountNumber = AccountNumber == null ? RandomAccountNumber() : AccountNumber,
                Date = DateTime.Today,
                Amount = RandomAmount(),
            };
        }

        public static AccountSummary MakeSummary(Stream stream, ulong? version = null)
        {
            return new AccountSummary
            {
                AccountNumber = stream.StreamId,
                Date = DateTime.Today,
                Amount = EventSummarizer.FindTotal(stream, version)
            };
        }

        private static string RandomAccountNumber()
        {
            return _random.Next(100000000, 999999999).ToString();
        }

        private static int RandomCheckNumber()
        {
            return _random.Next(1000, 9999);
        }
        private static decimal RandomAmount()
        {
            return new Decimal(_random.Next(10000, 99999)) / 100;
        }

        private static string RandomAccountName()
        {
            var account = new List<string> { "Good", "Great", "Local", "Online" };
            var accountIndex = _random.Next(account.Count);

            return $"{account[accountIndex]} Checking";
        }

        private static string RandomCheckPayee()
        {
            var payees = new List<string> { "Coffee shop", "Grocery store", "Favorite charity", "Rent" };
            var index = _random.Next(payees.Count);

            return payees[index];
        }

        private static string RandomDepositSource()
        {
            var sources = new List<string> { "Paycheck", "Investments", "Dividends", "Gift from relative" };
            var index = _random.Next(sources.Count);

            return sources[index];
        }
    }
}
