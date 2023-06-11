param (
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$projectPath,

    [Parameter(Mandatory = $false, Position = 1)]
    [string]$localRepoPath = "E:\CSharp\LocalRepo"
  )

# Définir le chemin vers nuget.exe
$nugetExec = "E:\CSharp\Projects\nuget.exe";
$dotnetExec = "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\MSBuild.exe"
$t4Transform = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\TextTransform.exe";

# Définir le chemin d'accès au projet
# $projectPath = "C:\Users\z017855\source\repos\UsefulCsharpCommonsUtils"

# Accéder au répertoire du projet
Set-Location $projectPath

if (Test-Path -Path "Assembly.tt" -PathType Leaf) {
    write-host "Transformation T4 Assembly...";
    Start-Process -FilePath $t4Transform  -ArgumentList "-out Assembly.cs Assembly.tt" -NoNewWindow -wait -PassThru;
    $p = Start-Process -FilePath $dotnetExec -ArgumentList $("$projectPath") -Wait -NoNewWindow -PassThru;
    if ($p.ExitCode -ne 0) {
        write-host "ExitCode != 0 : arrêt"
        return;
    }
}

write-host "Compile MSBUILD...";
$p = Start-Process -FilePath $dotnetExec -ArgumentList $("$projectPath") -Wait -NoNewWindow -PassThru;
if ($p.ExitCode -ne 0) {
	write-host "ExitCode != 0 : arrêt"
    return;
}

# Obtenir le nom du projet et la version de l'assembly
$projectName = [System.IO.Path]::GetFileNameWithoutExtension((Get-ChildItem "$projectPath\*.csproj").Name)
write-host "Nom du projet : $projectName";

$projectVersion = (Get-Item "$projectPath\bin\Debug\$projectName.dll").VersionInfo.FileVersion
write-host "Nom du projet : $projectVersion";

# Définir le chemin d'accès au dossier de sortie du package NuGet
$nupkgOutputPath = $projectPath

# Créer le dossier de sortie s'il n'existe pas encore
if (!(Test-Path $nupkgOutputPath)) {
    New-Item -ItemType Directory -Path $nupkgOutputPath
}

write-host "Générer le package NuGet à partir du projet...";
# Générer le package NuGet à partir du projet
$p = Start-process -FilePath $nugetExec -ArgumentList $("pack `"$projectPath\$projectName.csproj`" -Version $projectVersion -OutputDirectory $nupkgOutputPath") -Wait -NoNewWindow -PassThru;
if ($p.ExitCode -ne 0) {
	write-host "ExitCode != 0 : arrêt"
    return;
}

# Renommer le fichier nupkg généré en ajoutant le nom du projet et la version de l'assembly
$nupkgFileName = "$projectName.$projectVersion.nupkg"
Rename-Item "$nupkgOutputPath\$projectName.$projectVersion.nupkg" $nupkgFileName

# Afficher un message indiquant que le package NuGet a été généré
Write-Host "Le package NuGet $nupkgFileName a été généré avec succès."

write-host "Ajouter le package NuGet au référentiel local...";
# Ajouter le package NuGet au référentiel local
$p = Start-process -FilePath $nugetExec -ArgumentList $("add $nupkgFileName -Source $localRepoPath") -Wait -NoNewWindow -PassThru;
if ($p.ExitCode -ne 0) {
	write-host "ExitCode != 0 : arrêt"
    return;
}

# Afficher un message indiquant que le package NuGet a été publié dans le référentiel local
Write-Host "Le package NuGet $nupkgFileName a été publié dans le référentiel local avec succès."
