$listener = [System.Net.HttpListener]::new()
$listener.Prefixes.Add('http://localhost:8765/')
$listener.Start()
Write-Host 'Serving on http://localhost:8765/'
while ($true) {
    $ctx = $listener.GetContext()
    $reqPath = $ctx.Request.Url.LocalPath.TrimStart('/')
    if ($reqPath -eq '' -or $reqPath -eq '/') { $reqPath = 'erd_professional.html' }
    $filePath = Join-Path 'c:\Users\Brian\OOP-TaurusBikeShop\Documentation' $reqPath
    if (Test-Path $filePath) {
        $content = [System.IO.File]::ReadAllBytes($filePath)
        $ctx.Response.ContentType = 'text/html; charset=utf-8'
        $ctx.Response.ContentLength64 = $content.Length
        $ctx.Response.OutputStream.Write($content, 0, $content.Length)
    } else {
        $ctx.Response.StatusCode = 404
    }
    $ctx.Response.Close()
}
