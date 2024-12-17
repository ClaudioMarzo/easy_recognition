// Program.cs

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao container de injeção de dependência

// Habilita o suporte para controllers (API MVC)
builder.Services.AddControllers();

// Configuração de CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()  // Permite qualquer origem
              .AllowAnyMethod()  // Permite qualquer método HTTP
              .AllowAnyHeader()); // Permite qualquer cabeçalho
});

// Geração de documentação Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurações do pipeline HTTP

// Ativa o Swagger apenas no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona automaticamente requisições HTTP para HTTPS
app.UseHttpsRedirection();

// Aplica a política de CORS configurada anteriormente
app.UseCors();

// Habilita a autorização (padrão, caso venha a ser configurada posteriormente)
app.UseAuthorization();

// Mapeia os endpoints dos controllers
app.MapControllers();

// Executa a aplicação
app.Run();