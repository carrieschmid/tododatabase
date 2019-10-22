using BasicAuthentication.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Models;

namespace ToDoList {

  public class Startup {
    public Startup (IHostingEnvironment env) {
      var builder = new ConfigurationBuilder ()
        .SetBasePath (env.ContentRootPath)
        .AddJsonFile ("appsettings.json");
      Configuration = builder.Build ();
    }

    public IConfigurationRoot Configuration { get; set; }

    public void ConfigureServices (IServiceCollection services) {
      services.AddMvc ();

      services.AddEntityFrameworkMySql ()
        .AddDbContext<ToDoListContext> (options => options
          .UseMySql (Configuration["ConnectionStrings:DefaultConnection"]));

      services.AddIdentity<ApplicationUser, IdentityRole> ()
        .AddEntityFrameworkStores<ToDoListContext> ()
        .AddDefaultTokenProviders ();

      services.Configure<IdentityOptions> (options => {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 0;
      });
    }

    public void Configure (IApplicationBuilder app) {
      app.UseStaticFiles ();

      app.UseDeveloperExceptionPage ();

      app.UseAuthentication ();

      app.UseMvc (routes => {
        routes.MapRoute (
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
        //The template option tells .NET how to treat routes. This configuration, known as conventional routing, matches a path like /Items/Details/6 to its specific controller action by looking for the Details action route in the Items controller. Then it binds the value of id to 6. We won't change routes in this class, but be aware that .NET routing conventions can be configured
      });

      app.Run (async (context) => {
        await context.Response.WriteAsync ("Something went wrong!");
      });
    }
  }
}