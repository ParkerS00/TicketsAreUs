using RazorClassLib.Services;

namespace WebApp.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public bool IsWebApp => true;
    }
}
