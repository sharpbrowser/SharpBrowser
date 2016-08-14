using CefSharp;

namespace SharpBrowser
{
    public class SchemeHandlerFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "chrome";
        public const string SchemeNameTest = "test";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new SchemeHandler();
        }
    }
}