using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace PruebaFederacion.Controllers
{
    [AllowAnonymous]
    [Route("Metadata")]
    public class MetadataController : Controller
    {
        private readonly Saml2Configuration config;

        public MetadataController(Saml2Configuration config)
        {
            this.config = config;
        }

    
        public IActionResult Index()
        {
            var defaultSite = new Uri($"{Request.Scheme}://{Request.Host.ToUriComponent()}/");

            var entityDescriptor = new EntityDescriptor(config);
            entityDescriptor.ValidUntil = 365;
                        

            entityDescriptor.SPSsoDescriptor = new SPSsoDescriptor
            {
                WantAssertionsSigned = true,
                SigningCertificates = new X509Certificate2[]
                {
                    config.SigningCertificate
                },
                

                EncryptionCertificates = config.DecryptionCertificates,

                SingleLogoutServices = new SingleLogoutService[]
                {
                    new SingleLogoutService { Binding = ProtocolBindings.HttpPost, Location = new Uri(defaultSite, "Auth/SingleLogout"), ResponseLocation = new Uri(defaultSite, "Auth/LoggedOut") }
                },
                NameIDFormats = new Uri[] { NameIdentifierFormats.X509SubjectName },
                AssertionConsumerServices = new AssertionConsumerService[]
                {
                    new AssertionConsumerService { Binding = ProtocolBindings.HttpPost, Location = new Uri(defaultSite, "Auth/AssertionConsumerService") },
                },
                AttributeConsumingServices = new AttributeConsumingService[]
                {
                    //new AttributeConsumingService { ServiceNames.add = new ServiceNames("Sistema", "en"), RequestedAttributes = CreateRequestedAttributes() }
                },
            };
            entityDescriptor.ContactPersons = new[] {
                new ContactPerson(ContactTypes.Administrative)
                {
                    Company = "Universidad de Colima",
                    GivenName = "Administración Escolar",
                    SurName = "SICEUC",
                    EmailAddress = "dgae@ucol.mx",
                    TelephoneNumber = "3161100"
                }
            };
            return new Saml2Metadata(entityDescriptor).CreateMetadata()
                .ToActionResult();
        }



        private IEnumerable<RequestedAttribute> CreateRequestedAttributes()
        {

            yield return new RequestedAttribute("urn:oid:2.5.4.4");
            yield return new RequestedAttribute("urn:oid:2.5.4.3", false);
            yield return new RequestedAttribute("urn:xxx", "Prueba-Federacion");
            yield return new RequestedAttribute("urn:yyy", "123") { AttributeValueType = "xs:integer" };
        }
    
    }
}
