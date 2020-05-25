FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

RUN mkdir /sharetheshop
RUN mkdir /sharetheshop/api

WORKDIR /sharetheshop/api

RUN curl -L https://github.com/goeaway/shared-list/releases/download/api/shared-list-api.tar.gz --output shared-list-api.tar.gz
RUN tar -xzf shared-list-api.tar.gz

EXPOSE 80
EXPOSE 443
ENTRYPOINT [ "dotnet", "/sharetheshop/api/SharedList.API.Presentation.dll" ]