using Assessment2_BoilerControllerConsole.Controller;
using Assessment2_BoilerControllerConsole.Model;
using Assessment2_BoilerControllerConsole.Persistence;
using Assessment2_BoilerControllerConsole.Service;
using Assessment2_BoilerControllerConsole.View;

namespace Assessment2_BoilerControllerConsole
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            LoggerRepository loggerRepository = new LoggerRepository();
            SwitchService switchService = new SwitchService();
            BoilerService boilerService = new BoilerService(switchService, loggerRepository);
            ConsoleView consoleView = new ConsoleView();
            BoilerController controller = new BoilerController(switchService, boilerService, loggerRepository, consoleView);
             await controller.Start();
        }
    }
}
