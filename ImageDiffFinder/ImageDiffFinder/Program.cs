using ImageDiffFinder.Client.Pages;
using ImageDiffFinder.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using ImageDiffFinder.Models.Other;
using WebUtils;
using ImageDiffFinder.Data;
using Microsoft.Extensions.Options;
using System.Text;
using WebUtils.Enums;
using System.Net;
using ImageDiffFinder;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ImageDiffFinder.Services;

var builder = WebApplication.CreateBuilder(args);


var Config = builder.Configuration;
builder.Services.Configure<JWTConfig>(Config.GetSection("Jwt"));


// Add services to the container.
builder.Services.AddRazorComponents(options =>
{

    // It's for razor pages, can't be used with razor components in this way.
    // [Authorize] attribute should be added directly on razor component
    #region Commented filter pages with Auth
    //options.Conventions.AuthorizeFolder("/", nameof(AllowedRoles.Admin_Manager_Instructor_Student));



    //// * availlable for Admin and Manager
    //options.Conventions.AuthorizeFolder("/Departments", nameof(AllowedRoles.Admin_Manager));
    //options.Conventions.AuthorizeFolder("/Instructors", nameof(AllowedRoles.Admin_Manager));
    //options.Conventions.AuthorizeFolder("/Students", nameof(AllowedRoles.Admin_Manager));

    ////options.Conventions.AuthorizeFolder("/Courses", nameof(AllowedRoles.Admin_Manager));
    //options.Conventions.AuthorizePage("/Courses/Edit", nameof(AllowedRoles.Admin_Manager));
    //options.Conventions.AuthorizePage("/Courses/Delete", nameof(AllowedRoles.Admin_Manager));
    //options.Conventions.AuthorizePage("/Courses/Create", nameof(AllowedRoles.Admin_Manager));





    //// * availlable for Admin 
    //options.Conventions.AuthorizeFolder("/Managers", nameof(AllowedRoles.Admin));
    //// Below two pages aren't used the app and aren't shown anywhere
    //options.Conventions.AuthorizePage("/Enrollments/Create", nameof(AllowedRoles.Admin)); // This page isn't used anymore
    //options.Conventions.AuthorizePage("/Enrollments/Edit", nameof(AllowedRoles.Admin)); // This page isn't used anymore

    //options.Conventions.AuthorizePage("/Statistics", nameof(AllowedRoles.Admin)); // This page isn't used anymore

    //options.Conventions.AuthorizePage("/Instructors/AddCourse", nameof(AllowedRoles.Admin)); // This page isn't used anymore 





    //// * availlable for Admin, Manager and Student
    //options.Conventions.AuthorizePage("/Enrollments/Delete", nameof(AllowedRoles.Admin_Manager_Student));
    ////options.Conventions.AuthorizePage("/Courses", nameof(AllowedRoles.Admin_Manager_Student));
    //options.Conventions.AuthorizePage("/Courses/Details", nameof(AllowedRoles.Admin_Manager_Student));
    //options.Conventions.AuthorizePage("/Courses/Index", nameof(AllowedRoles.Admin_Manager_Student));




    ////options.Conventions.AuthorizePage("/Statistics", nameof(AllowedRoles.Admin_Manager_Instructor));




    //options.Conventions.AllowAnonymousToPage("/Auth/SignIn");
    //options.Conventions.AllowAnonymousToPage("/Index");
    //options.Conventions.AllowAnonymousToPage("/Students/Create");
    //options.Conventions.AllowAnonymousToFolder("/Guide");
    //options.Conventions.AllowAnonymousToFolder("/Stats"); 
    #endregion
})
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();




if (Environment.GetEnvironmentVariable("ImgDiffApp_Domain") == "localhost")
    builder.Services.AddDbContext<ImageDiffContext>(options =>
        options.UseSqlServer(Config.GetConnectionString("ImgDiff")));
else
    builder.Services.AddDbContext<ImageDiffContext>(options => options.UseSqlServer(Config.GetConnectionString("ImgDiff_Remote")
            .Replace("****", Environment.GetEnvironmentVariable("SA_PASSWORD"))));
//.Replace("db", Environment.GetEnvironmentVariable("SQL_CONFIG_SERVER_NAME")))); //SQL_CONFIG_SERVER_NAME will come from Kubernetes config file (.yaml) env variable



#region Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // To be able to pass JWT token in URL in addition to Authorization Header
                // (Meaning you can pass either in URL or in Authorization Header) 
                options.Events = new JwtBearerEvents
                {

                    #region Commented OnMessageReceived
                    //OnMessageReceived = context => // access_token should be passed in url query everytime in ever request
                    //{
                    //    var accessToken = context.Request.Query["access_token"];

                    //    if (string.IsNullOrEmpty(accessToken) == false)
                    //    {
                    //        context.Token = accessToken;
                    //    }
                    //    return Task.CompletedTask;
                    //} 
                    #endregion
                    
                    OnMessageReceived = async context =>
                    {
                        //var accessToken = context.Request.Query["access_token"];
                        ////AppState AppState = context.HttpContext.RequestServices.GetRequiredService<AppState>();
                        // Cookies will be sent with each request automatically
                        //var appState = await context.HttpContext.Request.Cookies.GetComplexCookie<AppState>(nameof(AppState));
                        //var appStateScoped = context.HttpContext.RequestServices.GetRequiredService<AppStateScoped>();


                        //AppState appState = new();
                        //try
                        //{
                        //    var protectedLocalStore = context.HttpContext.RequestServices.GetRequiredService<ProtectedLocalStorage>();
                        //    appState = await BlazorUtils.GetStateAsync<AppState>(protectedLocalStore);

                        //}
                        //catch (Exception)
                        //{

                        //    //throw;
                        //}
                        var appState = context.HttpContext.RequestServices.GetRequiredService<AppState>();

                        if (appState != null && !string.IsNullOrEmpty(appState.Token))
                        {
                            string accessToken = appState.Token.Substring(2, appState.Token.Length - 5);

                            context.Token = accessToken;
                        }
                        //return Task.CompletedTask;
                    },

                    OnTokenValidated = async context =>
                    {
                        //AppState AppState = await context.HttpContext.Request.Cookies.GetComplexCookie<AppState>(nameof(AppState));
                        var protectedLocalStore = context.HttpContext.RequestServices.GetRequiredService<ProtectedLocalStorage>();
                        var appState = await BlazorUtils.GetStateAsync<AppState>(protectedLocalStore);



                        // Checking user with db and logout them if needed(For more security and having more control on signed in users)
                        #region Commented loggin out hackers
                        //if (AppState.PersonRole != nameof(Roles.Admin)) // this condition will be removed when app is published
                        //{
                        //    var db = context.HttpContext.RequestServices.GetRequiredService<UniversityContext>();
                        //    var user = await db.Credentials.FirstOrDefaultAsync(c => c.PersonID == AppState.Person.ID);
                        //    // logout the user
                        //    if (user == null || String.IsNullOrEmpty(user.AccessToken)) // there is no token in srver-side
                        //    {
                        //        // Remove token from client-side
                        //        context.HttpContext.Response.Cookies.SetComplexCookie<AppState>(nameof(AppState), new AppState { Language = AppState.Language });

                        //        //return Task.CompletedTask;
                        //        return;
                        //    }
                        //    if (user.AccessToken != AppState.Token) // token is changed by client
                        //    {
                        //        // Remove token from client-side
                        //        context.HttpContext.Response.Cookies.SetComplexCookie<AppState>(nameof(AppState), new AppState { Language = AppState.Language });

                        //        // Remove token from  server-side
                        //        user.AccessToken = null;
                        //        await db.SaveChangesAsync();

                        //        //return Task.CompletedTask;
                        //        return;
                        //    }
                        //} 
                        #endregion


                        // Renew token every 4 minutes in client-side and server-side
                        // (Cookie expires and is deleted in client browser after 5 min
                        // (JWT expire time is also reached), so after 5 min here isn't reached)
                        double minElapsed = (DateTime.Now - appState.CreationTime).TotalMinutes;
                        if (minElapsed > 2)
                        {
                            // Token renew client-side
                            var newToken = await Utils.GenerateJWT(appState.PersonRole
                                , context.HttpContext.RequestServices.GetRequiredService<IOptions<JWTConfig>>().Value);

                            appState.Token = $"{new Random().Next(11, 99)}{newToken}{new Random().Next(111, 999)}";
                            appState.CreationTime = DateTime.Now;
                            //await context.HttpContext.Response.Cookies.SetComplexCookie<AppState>(nameof(appState), appState);
                            await BlazorUtils.SetStateAsync(protectedLocalStore,appState);



                            // Token renew server-side
                            var db = context.HttpContext.RequestServices.GetRequiredService<ImageDiffContext>();
                            var user = await db.Credentials.FirstOrDefaultAsync(c => c.PersonID == appState.PersonId);
                            user.AccessToken = appState.Token;
                            await db.SaveChangesAsync();
                        }





                        //return Task.CompletedTask;
                    }

                };

                options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ClockSkew = TimeSpan.Zero,
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = Config["Jwt:Issuer"],
                                ValidAudience = Config["Jwt:Audience"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.
                                UTF8.GetBytes(Config["Jwt:Key"]))
                            };
            });


#region Adding Policies for Policy-Based Authorization  
// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-6.0#policy-based-role-checks
// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-6.0
// https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-6.0
//
// * If you apply multiple policies to a controller or action, then all policies must pass before access is granted. For example:
//    https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-6.0#multiple-policy-evaluation

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("RequireAdminRole", policy =>
    //{
    //    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
    //    policy.RequireAuthenticatedUser();
    //    //policy.Requirements.Add(new MinimumAgeRequirement(18));
    //    policy.RequireRole("Admin");
    //});
    //
    //options.AddPolicy("RequireAdminRole",
    //     policy => policy.RequireRole("Student")); 
    //
    // options.AddPolicy("RequireStudentRole",
    //     policy.RequireRole("Admin", "Manager", "Student")) // To allow multple roles // "," means "or"

    foreach (string policyName in Enum.GetNames(typeof(AllowedRoles)))
    {
        options.AddPolicy(policyName, policy =>
        {
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();

            //policy.Requirements.Add(new MinimumAgeRequirement(18));
            //policy.RequireRole("Admin", "Manager", "Student"); // "," means "or"
            string[] allowedRoles = policyName.Split("_");
            policy.RequireRole(allowedRoles);
        });
    }
});
#endregion
#endregion

// *** Scoped and Transient liftimes behave the same in MVC and Razor Pages
//      because http is a stateless protocol and to persist-state in http based apps
//      you should use Cookies or Cookies combined with Session and/or Database or...
//      see: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0
// *** Scoped and Transient liftimes behave differently in Blazor-Server because
//      SignalR is not disconnected immidiatly. 
//
// * Scoped: These commented line are not ok here. Scoped is shorter liftime than Singleton
//    scoped means lifetime is scope of current request and during 
//    scope will be changed and values in these classes will be lost.
// 
// * Transient: Transient is shorter liftime than scope which is, even if in current request,
//    whenever this service is injected as a dependency to somwhere, a new fresh instance of
//    this service is created and nothing is persisted 
// 
// * Singleton: Singleton lifetime means, state of this sercive is persisted for lifetime of
//    service container or lifetime the app and the state will be share across all the client
//builder.Services.AddTransient<AppState>();
//builder.Services.AddScoped<AppStateScoped>();
builder.Services.AddScoped<AppState>();

// *** Provides helpful error information in the development environment for EF migrations errors.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// We need it in _Layout.cshtml file to access HttpContext, microsoft docs doesn't work
//builder.Services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.TryAddScoped<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient();

// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0#expose-the-authentication-state-as-a-cascading-parameter
builder.Services.AddCascadingAuthenticationState();

// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/?view=aspnetcore-8.0&source=recommendations&tabs=visual-studio#implement-a-custom-authenticationstateprovider
builder.Services.AddScoped<AuthenticationStateProvider,
    CustomAuthenticationStateProvider>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
else
{
    //app.UseExceptionHandler("/Error", createScopeForErrors: true);

    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

#region Create db
//using (var scope = app.ApplicationServices.CreateScope())
using (var scope = app.Services.CreateScope())
{ // *** Just for development when no db exist

    // The EnsureCreated method takes no action if a database for the context exists.
    // If no database exists, it creates the database and schema.

    // This workflow works early in development when the schema is rapidly evolving,
    // as long as data doesn't need to be preserved.
    // The situation is different when data that has been entered into the database needs to be preserved.
    // When that is the case, use migrations.
    // A database that is created by EnsureCreated can't be updated by using migrations.

    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ImageDiffContext>();
    context.Database.EnsureCreated();
    ////DbInitializer.Initialize(context);
}
#endregion

// Call UseStatusCodePages before request handling middleware. 
// For example, call UseStatusCodePages before the Static File Middleware and the Endpoints Middleware.
//app.UseStatusCodePages();
app.UseStatusCodePages(async context =>
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    // you may also check requests path to do this only for specific methods       
    // && request.Path.Value.StartsWith("/specificPath")

    {
        //response.Redirect("/signin");
        response.Redirect("/");
    }
});


//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();



//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapRazorPages();
//    //endpoints.MapControllerRoute(
//    //    name: "default",
//    //    pattern: "{controller=Home}/{action=Index}/{id?}");

//    endpoints.MapControllers();
//});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);


app.Run();
