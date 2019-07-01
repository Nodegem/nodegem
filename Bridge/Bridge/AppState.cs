using Bridge.Data.Dtos;

namespace Nodester.Bridge
{
    public class AppState
    {

        private static AppState _instance;

        public static AppState Instance => _instance ?? (_instance = new AppState());

        public TokenDto Token { get; set; }
        
        public string Environment { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}