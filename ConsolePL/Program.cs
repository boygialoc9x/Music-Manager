using ConsolePL.UI;

namespace MusicManager
{
    class Program
    {
        private static LoginMenu logMenu = new LoginMenu();
        static void Main(string[] args)
        {
            logMenu.option_loginMenu();
        }
    }
}
