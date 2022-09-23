![workflow](https://github.com/ferrivbe/aws-lambda-dotnet-api/actions/workflows/master.yml/badge.svg)

# Serverless Express service

This repository contains the source code of a serverless dotnet 6 service.

## Usage

### Deployment

This source should be deployed by triggering the ```github workflow``` for each branch after a PR merge or commit.

### Invocation

After successful deployment, you can call the created application via HTTP:

```bash
curl https://xxxxxxx.execute-api.us-east-1.amazonaws.com/
```

### Local deployment.

```
$ make local
```

After deploying, you should see output similar to:

```bash
Starting Offline at stage dev (us-east-1)

Offline [http for lambda] listening on http://localhost:3002
Function names exposed for local invocation by aws-sdk:
           * api: serverless-infrastructure-dev-api

   ┌───────────────────────────────────────────────────────────────────────┐
   │                                                                       │
   │   ANY | http://localhost:3000/{proxy*}                                │
   │   POST | http://localhost:3000/2015-03-31/functions/api/invocations   │
   │                                                                       │
   └───────────────────────────────────────────────────────────────────────┘

Server ready: http://localhost:3000 🚀
```

_Note_: In current form, some AWS configurations will not be available, such as authorizers.

### Loal invocation

After successful deployment, you can call the created application via HTTP:

```bash
curl http://localhost:3000/
```
