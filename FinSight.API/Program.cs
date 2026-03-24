using FinSight.API.Services;
using FinSightAI.API.Services;
using FinSight.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ========================================
// 🔐 DATABASE (FOR USERS)
// ========================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("FinSightDb"));

// ========================================
// 🔐 AUTH SERVICE
// ========================================
builder.Services.AddScoped<AuthService>();

// ========================================
// 🔐 JWT AUTHENTICATION (FIXED)
// ========================================
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "default-secret-key")
            )
        };
    });

builder.Services.AddAuthorization();

// ========================================
// 💬 CHAT MEMORY
// ========================================
builder.Services.AddScoped<ChatMemoryService>();

// ========================================
// ☁️ AZURE BLOB STORAGE
// ========================================
var blobConnectionString = configuration["AzureBlob:ConnectionString"]
    ?? throw new InvalidOperationException("AzureBlob:ConnectionString is missing");

builder.Services.AddScoped<AzureBlobService>(sp =>
    new AzureBlobService(blobConnectionString));

// ========================================
// 📄 DOCUMENT ANALYSIS
// ========================================
var docEndpoint = configuration["DocumentAnalysis:Endpoint"]
    ?? throw new InvalidOperationException("DocumentAnalysis:Endpoint is missing");

var docKey = configuration["DocumentAnalysis:Key"]
    ?? throw new InvalidOperationException("DocumentAnalysis:Key is missing");

builder.Services.AddSingleton<DocumentAnalysisService>(sp =>
    new DocumentAnalysisService(docEndpoint, docKey));

// ========================================
// 🤖 AZURE OPENAI
// ========================================
var openAIEndpoint = configuration["AzureOpenAI:Endpoint"]
    ?? throw new InvalidOperationException("AzureOpenAI:Endpoint is missing");

var openAIKey = configuration["AzureOpenAI:Key"]
    ?? throw new InvalidOperationException("AzureOpenAI:Key is missing");

var chatDeployment = configuration["AzureOpenAI:ChatDeployment"]
    ?? throw new InvalidOperationException("AzureOpenAI:ChatDeployment is missing");

var embeddingDeployment = configuration["AzureOpenAI:EmbeddingDeployment"]
    ?? throw new InvalidOperationException("AzureOpenAI:EmbeddingDeployment is missing");

builder.Services.AddSingleton<OpenAIService>(sp =>
    new OpenAIService(openAIEndpoint, openAIKey, chatDeployment, embeddingDeployment));

// ========================================
// 🔍 AZURE SEARCH
// ========================================
var searchEndpoint = configuration["AzureSearch:Endpoint"]?.Trim();
var searchApiKey = configuration["AzureSearch:ApiKey"]?.Trim();
var searchIndexName = configuration["AzureSearch:IndexName"]?.Trim();

if (string.IsNullOrWhiteSpace(searchEndpoint) ||
    string.IsNullOrWhiteSpace(searchApiKey) ||
    string.IsNullOrWhiteSpace(searchIndexName))
{
    throw new InvalidOperationException("AzureSearch configuration is missing or incomplete");
}

builder.Services.AddScoped<AzureSearchService>(sp =>
    new AzureSearchService(searchEndpoint, searchApiKey, searchIndexName));

// ========================================
// 📄 DOCUMENT PROCESSOR
// ========================================
builder.Services.AddSingleton<DocumentProcessor>();

// ========================================
// 🌐 CORS
// ========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ========================================
// 📦 CONTROLLERS + SWAGGER
// ========================================
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer YOUR_TOKEN'",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ========================================
// 🚀 MIDDLEWARE
// ========================================

 app.UseSwagger();
 app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

// 🔐 IMPORTANT ORDER
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "FinSight AI API running 🚀");

app.Run();