param (
    [Parameter(Mandatory = $true, Position = 0)]
    [string]$projectPath,

    [Parameter(Mandatory = $false, Position = 1)]
    [string]$localRepoPath = "C:\Users\z017855\source\repos\packageRepo"
  )

# Définir le chemin vers nuget.exe
$nugetExec = "C:\D\csharpSources\AryxDevLibrary\nuget.exe";
$dotnetExec = "C:\Program Files\Microsoft Visual Studio\2022\Professional\Msbuild\Current\Bin\amd64\MSBuild.exe"

# Définir le chemin d'accès au projet
# $projectPath = "C:\Users\z017855\source\repos\UsefulCsharpCommonsUtils"

# Accéder au répertoire du projet
Set-Location $projectPath

$p = Start-Process -FilePath $dotnetExec -ArgumentList $("$projectPath") -Wait -NoNewWindow -PassThru;
if ($p.ExitCode -ne 0) {
    return;
}

# Obtenir le nom du projet et la version de l'assembly
$projectName = [System.IO.Path]::GetFileNameWithoutExtension((Get-ChildItem "$projectPath\*.csproj").Name)
$projectVersion = (Get-Item "$projectPath\bin\Debug\$projectName.dll").VersionInfo.FileVersion

# Définir le chemin d'accès au dossier de sortie du package NuGet
$nupkgOutputPath = $projectPath

# Créer le dossier de sortie s'il n'existe pas encore
if (!(Test-Path $nupkgOutputPath)) {
    New-Item -ItemType Directory -Path $nupkgOutputPath
}

# Générer le package NuGet à partir du projet
Start-process -FilePath $nugetExec -ArgumentList $("pack `"$projectPath\$projectName.csproj`" -Version $projectVersion -OutputDirectory $nupkgOutputPath") -Wait -NoNewWindow;

# Renommer le fichier nupkg généré en ajoutant le nom du projet et la version de l'assembly
$nupkgFileName = "$projectName.$projectVersion.nupkg"
Rename-Item "$nupkgOutputPath\$projectName.$projectVersion.nupkg" $nupkgFileName

# Afficher un message indiquant que le package NuGet a été généré
Write-Host "Le package NuGet $nupkgFileName a été généré avec succès."

# Ajouter le package NuGet au référentiel local
Start-process -FilePath $nugetExec -ArgumentList $("add $nupkgFileName -Source $localRepoPath") -Wait -NoNewWindow;

# Afficher un message indiquant que le package NuGet a été publié dans le référentiel local
Write-Host "Le package NuGet $nupkgFileName a été publié dans le référentiel local avec succès."
