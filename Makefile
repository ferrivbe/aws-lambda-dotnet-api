.DEFAULT_GOAL := help

.PHONY: help
help:
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)

.PHONY: install
install: ## install api service dependencies.
	@npm install --omit="dev"

.PHONY: api
api: ## build api service.
	@dotnet restore --disable-parallel --verbosity minimal functions/api/Serverless.Api.sln

	@dotnet build functions/api/Serverless.Api.sln --no-restore --configuration Release --verbosity minimal
	@cd functions/api && dotnet publish -c Release --self-contained false -r linux-arm64 -o publish
	@cd functions/api/publish && zip -r ../../../api-artifact.zip *

.PHONY: dev
dev: ## run the project with development tools and configurations.
	@serverless offline start --allowCache --config serverless.yml

.PHONY: local
local: install api dev