using WebUrlCutterApp.Classes;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


//https://localhost:5001/swagger/index.html 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

ApplicationContext appContext = new ApplicationContext();
//задаем имя сервера коротких имен
appContext.ShortHostName = "https://my.syte/";


//запрос по объекта Link по короткой ссылке
//пример запроса: http://localhost:5001/getfull/https://my.syte/1abbbHHbas
app.MapGet("/getfull/{*link}", (string link) =>
{
    Link? newLink = appContext.Links.Where(x=> x.ShortUrl.ToUpper()== link.ToUpper()).FirstOrDefault();
    if (newLink != null)
        return Results.Ok(newLink);
    else
        return Results.NotFound();
});

//запрос по объекта Link по длинной ссылке
//если такая ссылка есть уже и время её жизни не истекло - то возвращается Link
//если такой ссылки нет, то создается и возвращается Link
//если есть и время истекло, то возвращается ошибка
//пример запроса: https://localhost:5001/getshort?link=https://myvideo.hello.ru
//пример с указанием времени жизни в минутах: https://localhost:5001/getshort?link=https://myvideo.hello.ru&minutes=10
app.MapGet("/getshort", (HttpContext context) =>
{
    Console.WriteLine(context.Request.Query);

   
    if (context.Request.Query.Keys.Contains("link"))
    {
        string link = context.Request.Query["link"];
        int minutes = 2;
        if (context.Request.Query.Keys.Contains("minutes"))
            int.TryParse(context.Request.Query["minutes"], out minutes);

        Link? newLink = appContext.CreateShortLink(link,minutes);
        return Results.Ok(newLink);

    }
    else return Results.BadRequest("Неправильные параметры");
});

//вывод списка всех ссылок на сервере
app.MapGet("/links", () => Results.Ok(appContext.Links));


app.Run();
