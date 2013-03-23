# Files in the order they must be combined
$files = 
    "jquery.signalR.compression.core.js",
    "jquery.signalR.compression.utilities.js",
	"jquery.signalR.compression.compressor.js",
	"jquery.signalR.compression.decompressor.js",
	"jquery.signalR.compression.contractRetriever.js",
	"jquery.signalR.compression.methodInvoker.js",
	"jquery.signalR.compression.receiveHandler.js"

# Run JSHint against files
Write-Host "Running JSHint..." -ForegroundColor Yellow
foreach ($file in $files) {
    Write-Host "$file... " -NoNewline
    $output = "build-output.txt"
    & "cscript.exe" ..\tools\jshint\env\wsh.js "$file" > $output

    if (Select-String $output -SimpleMatch -Pattern "[$file]" -Quiet) {
        Write-Host 
		Write-Host (Get-Content $output) | Select -Skip 4 | Write-Host -ForegroundColor Red        
        Remove-Item $output
        exit 1
    }
    Write-Host "no issues found" -ForegroundColor Green
}

# Combine all files into jquery.signalR.compression.js
if (!(Test-Path -path "bin")) {
	New-Item "bin" -Type Directory | Out-Null
}

Write-Host "Building bin\jquery.signalR.compression.js... " -NoNewline -ForegroundColor Yellow
$filePath = "bin\jquery.signalR.compression.js"
Remove-Item $filePath -Force -ErrorAction SilentlyContinue
foreach ($file in $files) {
    Add-Content -Path $filePath -Value "/* $file */"
    Get-Content -Path $file | %{ $_.replace($versionHolder,$version) } | Add-Content -Path $filePath
}
Write-Host "done" -ForegroundColor Green

# Minify to jquery.signalR.min.js
Write-Host "Building bin\jquery.signalR.min.js... " -NoNewline -ForegroundColor Yellow
& "..\tools\ajaxmin\AjaxMin.exe" bin\jquery.signalR.compression.js -out bin\jquery.signalR.compression.min.js -clobber > $output
(Get-Content $output)[6] | Write-Host -ForegroundColor Green

Remove-Item $output -Force