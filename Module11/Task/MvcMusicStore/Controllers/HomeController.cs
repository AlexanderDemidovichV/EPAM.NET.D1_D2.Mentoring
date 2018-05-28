using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using LogAdapter;
using LogProvider;
using MvcMusicStore.Infastructure;
using MvcMusicStore.Models;
using PerformanceCounterHelper;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();
        private readonly ILogger logger;

        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }
        // GET: /Home/
        public async Task<ActionResult> Index()
        {
            logger.Info("Go to Home");
            CounterHelperService.Instance.Increment(Counters.GoToHome);
            return View(await _storeContext.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(6)
                .ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}