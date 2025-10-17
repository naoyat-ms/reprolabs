var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ルート：プレーンテキストで応答
app.MapGet("/", () => Results.Text(
    "Hello from ASP.NET Core on IIS (net8.0)!", "text/plain"));

// 動作確認用：200 を返す
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

// 可変秒スリープ (/sleep/{sec}) 例: /sleep/3
app.MapGet("/sleep/{sec:int}", async (int sec, HttpContext ctx) =>
{
    if (sec < 0 || sec > 300) return Results.BadRequest("sec must be 0-300");

    try
    {
        await Task.Delay(TimeSpan.FromSeconds(sec), ctx.RequestAborted);
    }
    catch (TaskCanceledException)
    {
        return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
    }

    return Results.Text($"Slept {sec} seconds.", "text/plain; charset=utf-8");
});

// 20秒スリープして例外がそのまま飛ぶ (/sleepnotrycatch)
app.MapGet("/sleepnotrycatch", async (HttpContext ctx) =>
{
    // キャンセル検知を付けたい場合は ctx.RequestAborted を渡すが、
    // ここでは try/catch なしでキャンセル例外をそのまま外に出す
    await Task.Delay(TimeSpan.FromSeconds(20), ctx.RequestAborted);

    return Results.Text("Slept 20 seconds (no try/catch).", "text/plain; charset=utf-8");
});

// --- 例外をわざと投げる: /throw ---
app.MapGet("/throw", () =>
{
    throw new Exception("Test exception from /throw (expected 500).");
});

// 外向きHTTPテスト: /httpcall
// 公開テストサイト https://httpbin.org/delay/10 にアクセスして10秒待たせる
app.MapGet("/httpcall", async () =>
{
    using var http = new HttpClient();
    var start = DateTime.UtcNow;

    // httpbin の delay/10 は10秒スリープしてから200 OKを返す
    var response = await http.GetAsync("https://httpbin.org/delay/10");
    var body = await response.Content.ReadAsStringAsync();

    var elapsed = DateTime.UtcNow - start;

    return Results.Text(
        $"Target: https://httpbin.org/delay/10\nStatus: {(int)response.StatusCode}\nElapsed: {elapsed.TotalSeconds} sec\nBody: {body[..Math.Min(body.Length, 200)]}...",
        "text/plain; charset=utf-8"
    );
});

app.Run();
