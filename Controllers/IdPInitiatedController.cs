using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.Util;
using ITfoxtec.Identity.Saml2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens.Saml2;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PruebaFederacion.Controllers
{
    [AllowAnonymous]
    [Route("IdPInitiated")]
    public class IdPInitiatedController : Controller
    {
        public IActionResult Initiate()
        {
            var serviceProviderRealm = "https://www.ucol.mx/";

            var binding = new Saml2PostBinding();
            binding.RelayState = $"RPID={Uri.EscapeDataString(serviceProviderRealm)}";

            var config = new Saml2Configuration();

            config.Issuer = "https://idp.ucol.mx/";
            config.SingleSignOnDestination = new Uri("https://idp.ucol.mx/saml2/idp/SSOService.php");
            config.SigningCertificate = CertificateUtil.Load(Startup.AppEnvironment.MapToPhysicalFilePath("FederacionPrueba.pfx"));
            config.SignatureAlgorithm = Saml2SecurityAlgorithms.RsaSha256Signature;

            var appliesToAddress = "https://www.ucol.mx/";

            var response = new Saml2AuthnResponse(config);
            response.Status = Saml2StatusCodes.Success;

            var claimsIdentity = new ClaimsIdentity(CreateClaims());
            response.NameId = new Saml2NameIdentifier(claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).Single(), NameIdentifierFormats.Persistent);
            response.ClaimsIdentity = claimsIdentity;
            var token = response.CreateSecurityToken(appliesToAddress);

            return binding.Bind(response).ToActionResult();
        }

        private IEnumerable<Claim> CreateClaims()
        {
            yield return new Claim(ClaimTypes.NameIdentifier, "some-user-identity");
            yield return new Claim(ClaimTypes.Email, "some-user@domain.com");
        }
    }
}
