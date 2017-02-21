using System.Collections.Generic;
using Nancy.ViewEngines.Razor;

namespace SendgridMock
{
    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "SendgridMock";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "SendgridMock";
        }

        public bool AutoIncludeModelNamespace => true;
    }
}