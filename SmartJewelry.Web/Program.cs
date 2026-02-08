var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure HttpClient for API calls
builder.Services.AddHttpClient("SmartJewelryAPI", client =>
{
    var baseUrl = builder.Configuration["AppSettings:ApiBaseUrl"] ?? "http://localhost:5000";
    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Configure Authentication
var authBuilder = builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.Cookie.Name = "SmartJewelry.Auth";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// Add Google Authentication if configured
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
if (!string.IsNullOrEmpty(googleClientId) && !googleClientId.Contains("YOUR_"))
{
    authBuilder.AddGoogle("Google", options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret ?? "";
        options.CallbackPath = "/signin-google";
        options.SaveTokens = true; // Save access_token to authentication properties
    });
}

// Add Facebook Authentication if configured
var facebookAppId = builder.Configuration["Authentication:Facebook:AppId"];
var facebookAppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
if (!string.IsNullOrEmpty(facebookAppId) && !facebookAppId.Contains("YOUR_"))
{
    authBuilder.AddFacebook("Facebook", options =>
    {
        options.AppId = facebookAppId;
        options.AppSecret = facebookAppSecret ?? "";
        options.CallbackPath = "/signin-facebook";
        options.SaveTokens = true; // Save access_token to authentication properties
    });
}

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
