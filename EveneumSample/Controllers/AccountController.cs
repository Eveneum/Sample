using System.Threading.Tasks;
using EveneumSample.BusinessLogic;
using EveneumSample.Generators;
using EveneumSample.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EveneumSample.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEventStoreRepository _eventStoreRepository;

        public AccountController(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            ViewData["StreamHeaders"] = await _eventStoreRepository.GetStreamHeaders();

            return View();
        }

        [ActionName("Details")]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            var stream = await _eventStoreRepository.GetStream(id);
            ViewData["Stream"] = stream;
            ViewData["Total"] = EventSummarizer.FindTotal(stream);

            return View();
        }

        [ActionName("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            var openEvent = EventGenerator.BankAccountOpened();
            await _eventStoreRepository.AddEventAsync(openEvent.AccountNumber, openEvent);

            return RedirectToAction("Index");
        }

        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _eventStoreRepository.DeleteStream(id);

            return RedirectToAction("Index");
        }

        [Route("/account/add_check/{id}", Name = "AddCheck")]
        public async Task<IActionResult> AddCheck(string id)
        {
            var check = EventGenerator.CheckWritten(id);
            await _eventStoreRepository.AddEventAsync(id, check);

            return RedirectToAction("Details", new { id = id });
        }

        [Route("/account/add_deposit/{id}", Name = "AddDeposit")]
        public async Task<IActionResult> AddDeposit(string id)
        {
            var deposit = EventGenerator.DepositMoney(id);
            await _eventStoreRepository.AddEventAsync(id, deposit);

            return RedirectToAction("Details", new { id = id });
        }

        [Route("/account/close_account/{id}", Name="CloseAccount")]
        public async Task<IActionResult> CloseAccount(string id)
        {
            var close = EventGenerator.BankAccountClosed(id);
            await _eventStoreRepository.AddEventAsync(id, close);

            return RedirectToAction("Details", new { id = id } );
        }

        [Route("/account/make_snapshot", Name="MakeSnapshot")]
        public async Task<IActionResult> MakeSnapshot(string id, ulong version)
        {
            var stream = await _eventStoreRepository.GetStream(id);

            if (stream.HasValue)
            {
                var summary = EventGenerator.MakeSummary(stream.Value, version);
                await _eventStoreRepository.AddSnapshot(id, version, summary);
            }

            return RedirectToAction("Details", new { id = id } );
        }

        [Route("/account/delete_snapshots", Name="DeleteSnapshots")]
        public async Task<IActionResult> DeleteSnapshots(string id, ulong version)
        {
            await _eventStoreRepository.DeleteSnapshots(id, version + 1);

            return RedirectToAction("Details", new { id = id } );
        }
    }
}