using Bowling.Rest.Service.Interface.Services;
using Bowling.Rest.Service.Interface.Validation;
using Funq;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface
{
    /// <summary>
    /// Create your ServiceStack web service application with a singleton AppHost.
    /// </summary>
    public class AppHost : AppHostHttpListenerBase
    {
        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHost()
            : base("Bowling Web Services", typeof(MembersService).Assembly) { }

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            // enable fluent validation
            Plugins.Add(new ValidationFeature());

            //This method scans the assembly for validators
            container.RegisterValidators(typeof(MembersValidator).Assembly);

            // Register our own custom validators
            container.Register<IEmailValidator>(new EmailValidator());

            //Using an in-memory cache
            container.Register<ICacheClient>(new MemoryCacheClient());
        }
    }
}
