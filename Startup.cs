using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using ITfoxtec.Identity.Saml2.Util;

namespace PruebaFederacion
{
    public class Startup
    {
        public static IWebHostEnvironment AppEnvironment { get; private set; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            AppEnvironment = env;

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.BindConfig<Saml2Configuration>(Configuration, "Saml2", (serviceProvider, saml2Configuration) =>
            {
                // habilitado para el Metadata
                saml2Configuration.SigningCertificate = CertificateUtil.Load(AppEnvironment.MapToPhysicalFilePath(Configuration["Saml2:SigningCertificate"]), Configuration["Saml2:SigningCertificatePassword"]) ;               

                saml2Configuration.AllowedAudienceUris.Add(saml2Configuration.Issuer);
                
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                var entityDescriptor = new EntityDescriptor();
                entityDescriptor.SPSsoDescriptor = new SPSsoDescriptor();

                // la siguiente dos líneas son para encriptar/desencriptar el Response del IDP (encriptado con el certificado indicado en el metadata del IDP)
                saml2Configuration.EncryptionCertificate = CertificateUtil.Load(AppEnvironment.MapToPhysicalFilePath(Configuration["Saml2:EncryptionCertificate"]), Configuration["Saml2:CertificatePassword"]);
                saml2Configuration.DecryptionCertificates.Add(CertificateUtil.Load(AppEnvironment.MapToPhysicalFilePath(Configuration["Saml2:EncryptionCertificate"]), Configuration["Saml2:CertificatePassword"]));

                entityDescriptor.ReadIdPSsoDescriptorFromUrlAsync(httpClientFactory, new Uri(Configuration["Saml2:IdPMetadata"])).GetAwaiter().GetResult();
                if (entityDescriptor.IdPSsoDescriptor != null)
                {
                    saml2Configuration.AllowedIssuer = entityDescriptor.EntityId;
                    saml2Configuration.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;
                    saml2Configuration.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;

                    saml2Configuration.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);
                }
                else
                {
                    throw new Exception("IdPSsoDescriptor not loaded from metadata.");
                }

                return saml2Configuration;
            });
            

            services.AddSaml2(slidingExpiration: true);
            services.AddHttpClient();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSaml2();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

