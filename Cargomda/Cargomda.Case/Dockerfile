#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.



# Temel imaj olarak Microsoft'un ASP.NET Core imaj�n� kullan
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80                                                           
EXPOSE 443                                                           #EXPOSE = Konteynerin hangi portlar� dinleyece�ini belirtir          #80 genellikle HTTP i�in, 443 HTTPS i�in kullan�l�r.


# Uygulamay� in�a etmek i�in Microsoft'un ASP.NET Core SDK's�n� kullan
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src                                                           #WORKDIR = Docker konteyneri i�indeki �al��ma dizinini ayarlar
COPY ["Cargomda.Case/Cargomda.UI.csproj", "Cargomda.Case/"]            # COPY = Dosyalar� Docker konteynerine kopyalamak i�in kullan�l�r. 
COPY ["Business/Business.csproj", "Business/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Entity/Entity.csproj", "Entity/"]
COPY ["Redis.Cache/Redis.Cache.csproj", "Redis.Cache/"]
RUN dotnet restore "Cargomda.Case/Cargomda.UI.csproj"                 # RUN = Komutlar� �al��t�rmak i�in kullan�l�r.
COPY . .
WORKDIR "/src/Cargomda.Case"
RUN dotnet build "Cargomda.UI.csproj" -c Release -o /app/build


# Uygulamay� yay�nlamak i�in �nceki a�amadan olu�turulan imaj� kullan
FROM build AS publish
RUN dotnet publish "Cargomda.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Temel imaja son yay�nlanan uygulamay� kopyala ve uygulama ba�lat�c�s�n� belirle
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cargomda.UI.dll"]                              #ENTRYPOINT = Konteyner ba�lat�ld���nda �al��t�r�lacak komutu belirtir