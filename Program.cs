using Microsoft.AspNetCore.Http.Extensions;

using ManeKani.Auth.Ory;
using ManeKani.DB;
using ManeKani.Auth.Policies;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opt =>
{
    var port = builder.Configuration.GetValue<int>("APP_PORT", 5001);
    opt.ListenAnyIP(port);
});

// Add DB handler
builder.Services.AddTransient(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    return new ManeKaniDatabase(connectionString);
});

// register Ory authentication
builder.Services.AddAuthentication(
    OryAuthenticationDefaults.AuthenticationScheme
).AddOry(
    OryAuthenticationDefaults.AuthenticationScheme,
    options =>
    {
        options.SessionKey = "manekani_session";
        options.OryBasePath = "http://localhost:4433";
    }
);
// set default auth provider
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OryAuthenticationDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization(options =>
    {
        options.AddLoginPolicy();
    }
);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorPages(
    options =>
    {
        options.Conventions.AuthorizeFolder("/settings", LoginPolicy.LoginIncomplete);
    }
);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAntiforgery(o => o.HeaderName = "CSRF-TOKEN");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePages(context =>
{
    // if we get a 401, redirect to login
    if (context.HttpContext.Response.StatusCode == 401)
    {
        var returnTo = context.HttpContext.Request.GetEncodedUrl();
        Console.WriteLine(returnTo);
        context.HttpContext.Response.Redirect($"/login?returnTo={returnTo}", true);
    }

    return Task.CompletedTask;
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
