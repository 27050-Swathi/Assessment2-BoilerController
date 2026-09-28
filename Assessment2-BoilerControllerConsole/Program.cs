using Assessment2_BoilerControllerConsole.Controller;
using Assessment2_BoilerControllerConsole.Persistence;
using Assessment2_BoilerControllerConsole.Service;
using Assessment2_BoilerControllerConsole.View;

namespace Assessment2_BoilerControllerConsole
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            // Persistence
            LoggerRepository loggerRepository = new LoggerRepository();

            // Services
            SwitchService switchService = new SwitchService(loggerRepository);
            using BoilerService boilerService = new BoilerService(switchService, loggerRepository);

            // View
            ConsoleView consoleView = new ConsoleView();

            // Controller
            BoilerController controller = new BoilerController(switchService, boilerService, loggerRepository, consoleView);
            await controller.Start();
        }
    }
}
