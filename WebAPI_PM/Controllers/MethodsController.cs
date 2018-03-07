using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAPI_PM.Controllers
{
    public class MethodsController : ApiController
    {
        public class HelpMethod
        {
            public string Method { get; set; }
            public string RelativePath { get; set; }
            public string ReturnType { get; set; }
            public IEnumerable<HelpParameter> Parameters { get; set; }
        }

        public class HelpParameter
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public bool IsOptional { get; set; }
        }

        public IEnumerable<HelpMethod> GetMethods()
        {
            // get the IApiExplorer registered automatically
            IApiExplorer ex = this.Configuration.Services.GetApiExplorer();

            // loop, convert and return all descriptions 
            return ex.ApiDescriptions
                // ignore self
                .Where(D => D.ActionDescriptor.ControllerDescriptor.ControllerName != "ApiMethod")
                .Select(D =>
                {
                    // convert to a serializable structure
                    return new HelpMethod
                    {
                        Parameters = D.ParameterDescriptions.Select(P => new HelpParameter
                        {
                            Name = P.Name,
                            Type = P.ParameterDescriptor.ParameterType.FullName,
                            IsOptional = P.ParameterDescriptor.IsOptional
                        }).ToArray(),
                        Method = D.HttpMethod.ToString(),
                        RelativePath = D.RelativePath,
                        ReturnType = D.ResponseDescription.DeclaredType == null ?
                            null : D.ResponseDescription.DeclaredType.ToString()
                    };
                });
        }
    }
}
