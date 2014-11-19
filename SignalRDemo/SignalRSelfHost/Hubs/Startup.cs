using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using SignalRSelfHost.IOC;

namespace SignalRSelfHost.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //worsks to client
            //app.UseCors(CorsOptions.AllowAll);
            //app.MapSignalR();




            //not working
            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    Resolver = new AutofacSignalRDependencyResolver(App.Container),

                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}