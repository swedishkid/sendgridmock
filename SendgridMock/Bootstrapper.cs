using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.ViewEngines;

namespace SendgridMock
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            var assembly = GetType().Assembly;
            ResourceViewLocationProvider.RootNamespaces.Add(assembly, "SendgridMock.Features");
        }

        protected override NancyInternalConfiguration InternalConfiguration
            => NancyInternalConfiguration.WithOverrides(ConfigurationBuilder);

        private void ConfigurationBuilder(NancyInternalConfiguration nancyInternalConfiguration)
        {
            nancyInternalConfiguration.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        }
    }
}