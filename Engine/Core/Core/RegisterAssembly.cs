using Microsoft.Extensions.DependencyInjection;

namespace Nodegem.Engine.Core
{
    /// <summary>
    /// This exists purely because c# does something weird where it won't
    /// recognize an assembly unless it's called or something
    /// </summary>
    public static class RegisterAssembly
    {

        public static void RegisterMySelf(this IServiceCollection collection)
        {
            
        }
        
    }
}