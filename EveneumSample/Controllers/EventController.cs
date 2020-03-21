using System.Threading.Tasks;
using EveneumSample.Events;
using EveneumSample.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EveneumSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventStoreRepository _eventStoreRepo;

        public EventController(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepo = eventStoreRepository;
        }

        [HttpPost("OpenAccount")]
        public async Task<ActionResult> OpenBankAccount(BankAccountOpenedEvent openEvent)
        {
            var response = await _eventStoreRepo.AddEventAsync(openEvent.AccountNumber, openEvent);
            return CreatedAtAction("created", response);
        }

        [HttpPost("CloseAccount")]
        public async Task<ActionResult> CloseBankAccount(BankAccountClosedEvent closeEvent)
        {
            var response = await _eventStoreRepo.AddEventAsync(closeEvent.AccountNumber, closeEvent);
            return CreatedAtAction("created", response);
        }

        [HttpPost("DepositMoney")]
        public async Task<ActionResult> DepositMoney(MoneyDepositedEvent depositEvent)
        {
            var response = await _eventStoreRepo.AddEventAsync(depositEvent.AccountNumber, depositEvent);
            return CreatedAtAction("created", response);
        }

        [HttpPost("WriteCheck")]
        public async Task<ActionResult> WriteCheck(CheckWrittenEvent checkEvent)
        {
            var response = await _eventStoreRepo.AddEventAsync(checkEvent.AccountNumber, checkEvent);
            return CreatedAtAction("created", response);
        }
    }
}