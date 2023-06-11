param (
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$projectPath,

    [Parameter(Mandatory = $false, Position = 1)]
    [string]$localRepoPath = "E:\CSharp\LocalRepo"
  )

# D�finir le chemin vers nuget.exe
$nugetExec = "E:\CSharp\Projects\nuget.exe";
$dotnetExec = "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\MSBuild.exe"
$t4Transform = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\TextTransform.exe";

# D�finir le chemin d'acc�s au projet
# $projectPath = "C:\Users\z017855\source\repos\UsefulCsharpCommonsUtils"

# Acc�der au r�pertoire du projet
Set-Location $projectPath

if (Test-Path -Path "Assembly.tt" -PathType Leaf) {
    write-host "Transformation T4 Assembly...";
    Start-Process -FilePath $t4Transform  -ArgumentList "-out Assembly.cs Assembly.tt" -NoNewWindow -wait -PassThru;
    $p = Start-Process -FilePath $dotnetExec -ArgumentList $("$projectPath") -Wait -NoNewWindow -PassThru;
    if ($p.ExitCode -ne 0) {
        write-host "ExitCode != 0 : arr�t"
        return;
    }
}

write-host "Compile MSBUILD...";
$p = Start-Process -FilePath $dotnetExec -ArgumentList $("$projectPath") -Wait -NoNewWindow -PassThru;
if ($p.ExitCode -ne 0) {
	write-host "ExitCode != 0 : arr�t"
    return;
}

# Obtenir le nom du projet et la version de l'assembly
$projectName = [System.IO.Path]::GetFileNameWithoutExtension((Get-ChildItem "$projectPath\*.csproj").Name)
write-host "Nom du projet : $projectName";

$projectVersion = (Get-Item "$projectPath\bin\Debug\$projectName.dll").VersionInfo.FileVersion
write-host "Nom du projet : $projectVersion";

# D�finir le chemin d'acc�s au dossier de sortie du package NuGet
$nupkgOutputPath = $projectPath

# Cr�er le dossier de sortie s'il n'existe pas encore
if (!(Test-Path $nupkgOutputPath)) {
    New-Item -ItemType Directory -Path $nupkgOutputPath
}

write-host "G�n�rer le package NuGet � partir du projet...";
# G�n�rer le package NuGet � partir du projet
$p = Start-process -FilePath $nugetExec -ArgumentList $("pack `"$projectPath\$projectName.csproj`" -Version $projectVersion -OutputDirectory $nupkgOutputPath") -Wait -NoNewWindow -PassThru;
if ($p.ExitCode -ne 0) {
	write-host "ExitCode != 0 : arr�t"
    return;
}

# Renommer le fichier nupkg g�n�r� en ajoutant le nom du projet et la version de l'assembly
$nupkgFileName = "$projectName.$projectVersion.nupkg"
Rename-Item "$nupkgOutputPath\$projectName.$projectVersion.nupkg" $nupkgFileName

# Afficher un message indiquant que le package NuGet a �t� g�n�r�
Write-Host "Le package NuGet $nupkgFileName a �t� g�n�r� avec succ�s."

write-host "Ajouter le package NuGet au r�f�rentiel local...";
# Ajouter le package NuGet au r�f�rentiel local
$p = Start-process -FilePath $nugetExec -ArgumentList $("add $nupkgFileName -Source $localRepoPath") -Wait -NoNewWindow -PassThru;
if ($p.ExitCode -ne 0) {
	write-host "ExitCode != 0 : arr�t"
    return;
}

# Afficher un message indiquant que le package NuGet a �t� publi� dans le r�f�rentiel local
Write-Host "Le package NuGet $nupkgFileName a �t� publi� dans le r�f�rentiel local avec succ�s."
