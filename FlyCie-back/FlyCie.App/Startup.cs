using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using FlyCie.App.Services;
using FlyCie.App.Abstractions;

namespace FlyCie.App
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddControllers();
            services.AddHostedService<FlightService>();
            services.AddHostedService<ExternalTicketHandler>();
            services.AddSingleton<ExternalService>();
            services.AddSingleton<ITicketService, TicketService>();
            services.Configure<ExternalApiOptions>( o =>
            {
                o.ExternalApiUrl = Configuration[ "ExternalApiUrl" ];
            } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors( c =>
                    c.SetIsOriginAllowed( host => true )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials() );

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllers();
            } );
        }
    }
}
