using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SamlBlazorApp.Data;
using Sustainsys.Saml2;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Add SAML SSO services
builder.Services.AddAuthentication(sharedOptions =>
{
    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    sharedOptions.DefaultChallengeScheme = Saml2Defaults.Scheme;
})
.AddCookie()
.AddSaml2(options =>
{
    options.SPOptions.EntityId = new EntityId("saml-poc"); // Your SP entity ID

    options.IdentityProviders.Add(
        new IdentityProvider(
            new EntityId("http://localhost:8080/simplesaml/saml2/idp/metadata.php"), options.SPOptions)
        {
            LoadMetadata = true,

            SingleSignOnServiceUrl = new Uri("http://localhost:8080/simplesaml/saml2/idp/SSOService.php"),
            SingleLogoutServiceUrl = new Uri("http://localhost:8080/simplesaml/saml2/idp/SingleLogoutService.php"),
            AllowUnsolicitedAuthnResponse = true,
            WantAuthnRequestsSigned = false
        }
    );

    // options.SPOptions.AuthenticateRequestSigningBehavior = SigningBehavior.IfIdpWantAuthnRequestsSigned;
    options.SPOptions.ServiceCertificates.Add(new X509Certificate2("certificates/localhost.pfx", "YourSecurePassword"));
    // Allow weaker signing algorithm
    options.SPOptions.MinIncomingSigningAlgorithm = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

    
});

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddSession();
var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions 
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    Secure = CookieSecurePolicy.Always
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();  // This line should be before UseRouting()
app.UseRouting();
app.UseAuthorization();  // This line should be after UseRouting()
// app.UseSession();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

