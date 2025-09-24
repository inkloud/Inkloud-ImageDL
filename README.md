# Inkloud-ImageDL

Applicazione console .NET Core 3.1 per il download di immagini di prodotti da Life365.eu.

## Prerequisiti

- .NET Core 3.1 SDK o versione superiore
- Scaricabile da: https://dotnet.microsoft.com/download/dotnet/3.1

## Compilazione

```powershell
dotnet build Life365-download.csproj
```

## Utilizzo

### Esecuzione con dotnet run
```powershell
dotnet run -- -p "C:\temp\output" -c "14,27" -u "username" -k "password"
```

### Esecuzione dell'executable compilato
```powershell
.\bin\Debug\netcoreapp3.1\Life365-download.exe -p "C:\temp\output" -c "14,27" -u "username" -k "password"
```

## Parametri

- `-p, --path`: Percorso dove scaricare le immagini (obbligatorio)
- `-c, --cats`: ID delle categorie separate da virgola (obbligatorio)
- `-u, --user`: Username per l'API Life365.eu (obbligatorio)
- `-k, --password`: Password per l'API Life365.eu (obbligatorio)

## Esempio

```powershell
dotnet run -- -p "C:\Downloads\Life365Images" -c "1,14,27" -u "mio_username" -k "mia_password"
```

Questo comando scaricherà le immagini delle categorie 1, 14 e 27 nella cartella specificata.