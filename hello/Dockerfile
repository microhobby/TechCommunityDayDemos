# --------- BUILD
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS Build

COPY . /project
WORKDIR /project

# build
RUN dotnet restore && \
	dotnet build && \
	dotnet build -r linux-arm && \
	dotnet publish -r linux-arm

# --------- DEPLOY
FROM arm32v7/debian:buster-slim AS Deploy

# install dependencies
RUN apt-get -y update && apt-get install -y --no-install-recommends \
	libicu63 \
	&& apt-get clean && apt-get autoremove && rm -rf /var/lib/apt/lists/*

# copy project
COPY --from=Build /project/bin/Debug/netcoreapp3.1/linux-arm/publish /project

CMD ["./project/hello"]
