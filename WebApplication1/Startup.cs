﻿using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SignalR.Hub;
using System.Text;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Repository.InheritanceRepo;
using WebApplication1.Repository.Interface;
using WebApplication1.Service;
using WebApplication1.Trigger;

namespace MyWebApiApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularOrigins",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true)
                   .AllowCredentials();
                });
            });

            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddDbContext<MyDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("AzureDb"));

                option.UseTriggers(triggerOptions =>
                {
                    triggerOptions.AddTrigger<NotificationConTrigger>();
                    triggerOptions.AddTrigger<MessageConTrigger>();
                });
            });

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IPublisherRepository, PublisherRepository>();
            services.AddScoped<IReceiptRepository, ReceiptRepository>();

            services.AddScoped<IBookStatusRepository, BookStatusRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped(x => new BlobServiceClient(Configuration.GetValue<string>("AzureBlobStorage")));
            services.AddScoped<IBlobService, BlobService>();

            services.AddSingleton<IUserIdProvider, IdBasedUserIdProvider>();

            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));

            var secretKey = Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tự cấp token
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        //ký vào token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Book permission
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateBookAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.CREATE_BOOK));
                options.AddPolicy("UpdateBookAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.UPDATE_BOOK));
                options.AddPolicy("DeleteBookAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.DELETE_BOOK));

                options.AddPolicy("CreateCategoryAccess", policy =>
                                 policy.RequireClaim("Permissions", PolicyTerm.CREATE_CATEGORY));
                options.AddPolicy("UpdateCategoryAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.UPDATE_CATEGORY));
                options.AddPolicy("DeleteCategoryAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.DELETE_CATEGORY));

                options.AddPolicy("CreateVoucherAccess", policy =>
                                policy.RequireClaim("Permissions", PolicyTerm.CREATE_VOUCHER));
                options.AddPolicy("UpdateVoucherAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.UPDATE_VOUCHER));
                options.AddPolicy("DeleteVoucherAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.DELETE_VOUCHER));

                options.AddPolicy("CreatePublisherAccess", policy =>
                               policy.RequireClaim("Permissions", PolicyTerm.CREATE_PUBLISHER));
                options.AddPolicy("UpdatePublisherAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.UPDATE_PUBLISHER));
                options.AddPolicy("DeletePublisherAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.DELETE_PUBLISHER));

                options.AddPolicy("CreateAuthorAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.CREATE_AUTHOR));
                options.AddPolicy("UpdateAuthorAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.UPDATE_AUTHOR));
                options.AddPolicy("DeleteAuthorAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.DELETE_AUTHOR));

                options.AddPolicy("CreateReceiptAccess", policy =>
                                 policy.RequireClaim("Permissions", PolicyTerm.CREATE_RECEIPT));
                options.AddPolicy("UpdateReceiptAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.UPDATE_RECEIPT));
                options.AddPolicy("DeleteReceiptAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.DELETE_RECEIPT));

                options.AddPolicy("CreateMessageAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.CREATE_MESSAGE));
                options.AddPolicy("UpdateMessageAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.UPDATE_MESSAGE));
                options.AddPolicy("DeleteMessageAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.DELETE_MESSAGE));

                options.AddPolicy("CreateNotificationAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.CREATE_NOTIFICATION));
                options.AddPolicy("UpdateNotificationAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.UPDATE_NOTIFICATION));
                options.AddPolicy("DeleteNotificationAccess", policy =>
                                  policy.RequireClaim("Permissions", PolicyTerm.DELETE_NOTIFICATION));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyWebApiApp", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebApiApp v1"));
            }

            if (!env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseDeveloperExceptionPage();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyWebApiApp v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("api/user/notify");
                endpoints.MapHub<MessageHub>("api/user/message");

                endpoints.MapControllers();
            });
        }
    }
}