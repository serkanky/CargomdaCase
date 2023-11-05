#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.



# Temel imaj olarak Microsoft'un ASP.NET Core imajýný kullan
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80                                                           
EXPOSE 443                                                           #EXPOSE = Konteynerin hangi portlarý dinleyeceðini belirtir          #80 genellikle HTTP için, 443 HTTPS için kullanýlýr.


# Uygulamayý inþa etmek için Microsoft'un ASP.NET Core SDK'sýný kullan
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src                                                           #WORKDIR = Docker konteyneri içindeki çalýþma dizinini ayarlar
COPY ["Cargomda.Case/Cargomda.UI.csproj", "Cargomda.Case/"]            # COPY = Dosyalarý Docker konteynerine kopyalamak için kullanýlýr. 
COPY ["Business/Business.csproj", "Business/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Entity/Entity.csproj", "Entity/"]
COPY ["Redis.Cache/Redis.Cache.csproj", "Redis.Cache/"]
RUN dotnet restore "Cargomda.Case/Cargomda.UI.csproj"                 # RUN = Komutlarý çalýþtýrmak için kullanýlýr.
COPY . .
WORKDIR "/src/Cargomda.Case"
RUN dotnet build "Cargomda.UI.csproj" -c Release -o /app/build


# Uygulamayý yayýnlamak için önceki aþamadan oluþturulan imajý kullan
FROM build AS publish
RUN dotnet publish "Cargomda.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Temel imaja son yayýnlanan uygulamayý kopyala ve uygulama baþlatýcýsýný belirle
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cargomda.UI.dll"]                              #ENTRYPOINT = Konteyner baþlatýldýðýnda çalýþtýrýlacak komutu belirtir