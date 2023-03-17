using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sm.Query.Api.Queries;
using Sm.Query.Domain.Entities;
using Sm.Query.Domain.Repositories;
using Sm.Query.Infrastructure.Consumers;
using Sm.Query.Infrastructure.DataContext;
using Sm.Query.Infrastructure.Dispatchers;
using Sm.Query.Infrastructure.Handlers;
using Sm.Query.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Action<DbContextOptionsBuilder> configureDbContext = (o => o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SocialMediaDb")));
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();

builder.Services.AddScoped<IEventHandler, Sm.Query.Infrastructure.Handlers.EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

//Register query handler methods  //TODO use MediatR package
var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
var dispatcher = new QueryDispatcher();
dispatcher.RegisterHandler<FindAllPostsQuery>(queryHandler.HandlerAsync);
dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandlerAsync);
dispatcher.RegisterHandler<FindPostByAuthorQuery>(queryHandler.HandlerAsync);
dispatcher.RegisterHandler<FindPostsWithCommentsQuery>(queryHandler.HandlerAsync);
dispatcher.RegisterHandler<FindPostsWithLikesQuery>(queryHandler.HandlerAsync);
builder.Services.AddSingleton<IQueryDispatcher<PostEntity>>(_ => dispatcher);

builder.Services.AddHostedService<ConsumerHostedService>();

var databaseContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
databaseContext.Database.EnsureCreated();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
