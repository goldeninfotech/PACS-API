
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SoftEngine.Interface.IADM;
using SoftEngine.Interface.ITRD;
using SoftEngine.TRDCore.ADM;
using SoftEngine.TRDCore.Configurations;
using SoftEngine.TRDCore.TRD;
using SoftEngine.TRDModels.Models.TRD;
using System.Text;
using GDNTRDSolution_API.Service;
using GDNTRDSolution_API.Models;
using GDNTRDSolution_API.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionStrings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
builder.Services.AddSingleton(connectionStrings);

// Add token 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CrosPolicy", opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TRD_API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();


// Register the service implementation and interface
builder.Services.AddScoped<IUserLogin, UserLoginBLL>();
builder.Services.AddScoped<TokenVerify, TokenVerify>();
builder.Services.AddScoped<IUserPermisson, UserPermissonBLL>();
builder.Services.AddScoped<IDepartment, DepartmentBLL>();
builder.Services.AddScoped<IUsers, UsersBLL>();
builder.Services.AddScoped<IRoleInfo, RoleInfoBLL>();
builder.Services.AddScoped<IHospital, HospitalBLL>();
builder.Services.AddScoped<IHospitalCategory, HospitalCategoryBLL>();
builder.Services.AddScoped<IDoctorCategory, DoctorCategoryBLL>();
builder.Services.AddScoped<IDoctor, DoctorBLL>();
builder.Services.AddScoped<IDesignations, DesignationsBLL>();
builder.Services.AddScoped<IFileUpload, FileUploadBLL>();
builder.Services.AddScoped<IFilesRecored, FilesRecoredBLL>();
builder.Services.AddScoped<IHospitalDevice, HospitalDeviceBLL>();
builder.Services.AddScoped<IDeviceType, DeviceTypeBLL>();
builder.Services.AddScoped<IReportTemplate, ReportTemplateBLL>();
builder.Services.AddScoped<IDoctorAssign, DoctorAssignBLL>();
builder.Services.AddScoped<IDoctorPool, DoctorPoolBLL>();
builder.Services.AddScoped<ISettings, SettingsBLL>();
builder.Services.AddScoped<IDoctorAccount, DoctorAccountBLL>();
builder.Services.AddScoped<IBillingSettings, BillingSettingsBLL>();
builder.Services.AddScoped<IInvoice, InvoiceBLL>();
builder.Services.AddScoped<IInvoicePayments, InvoicePaymentsBLL>();
builder.Services.AddScoped<IPatientReports, PatientReportsBLL>();
builder.Services.AddScoped<IReportsDescriptipns, ReportsDescriptipnsBLL>();
builder.Services.AddScoped<IDoctorSchedule, DoctorScheduleBLL>();
builder.Services.AddScoped<IDoctorPaymentRecored, DoctorPaymentRecoredBLL>();
builder.Services.AddScoped<IPermission, PermissionBLL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CrosPolicy");

//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
