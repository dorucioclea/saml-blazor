using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SamlBlazorApp.Data;
using Sustainsys.Saml2;
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
    sharedOptions.DefaultChallengeScheme = "Saml2";
})
.AddCookie()
.AddSaml2(options =>
{
    options.SPOptions.EntityId = new EntityId("saml-poc"); // Your SP entity ID
    options.IdentityProviders.Add(
        new IdentityProvider(
            new EntityId("http://localhost:8080/simplesaml/saml2/idp/metadata.php"), options.SPOptions) // Your IdP entity ID
        {
            LoadMetadata = true,
            MetadataLocation = "http://localhost:8080/simplesaml/saml2/idp/metadata.php" // Your IdP metadata
        });

    options.SPOptions.ServiceCertificates.Add(new X509Certificate2("certificates/localhost.pfx", "YourSecurePassword"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Add this line
app.UseAuthorization(); // Add this line

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

