# Simplified Transaction System
## Introduction
STF is my solution for a bank transfer system as part of a challenge for a job (I wasn't one of the candidates, I just thought it was interesting to do it) as a backend developer (no level was mentioned).

## Table of Contents

- [Overview](#overview)
  - [Tasks](#tasks)
  - [User Endpoints](#user-endpoints)
  - [Transaction Endpoints](#transaction-endpoints)
  - [Built With](#built-with)
- [How to use](#how-to-use)
- [Author](#author)

## Overview
We have 2 types of users, common users and shopkeepers, both have wallets with money and transfer money between them.

### Tasks
- [x] For both types of user, we need the Full Name, Document Number, email and Password. Document Number and e-mails must be unique in the system. Therefore, your system should allow only one registration with the same Document Number or email address.

- [x] Users can send money (make transfers) to merchants and between users.

- [x] Shopkeepers only receive transfers, they do not send money to anyone.

- [x] Validate that the user has a balance before the transfer.

- [x] Before finalizing the transfer, you must consult an external authorizing service, use this mock to simulate (https://run.mocky.io/v3/8fafdd68-a090-496f-8c9a-3442cf30dae6).

- [x] The transfer operation must be a transaction (i.e. reversed in any case of inconsistency) and the money must be returned to the sending user's wallet.

- [x] Upon receipt of payment, the user or merchant needs to receive notification (email, sms) sent by a third-party service and eventually this service may be unavailable/unstable. Use this mockup to simulate sending (http://o4d9z.mocklab.io/notify).

- [x] This service must be RESTFul.

### User Endpoints
![User-Endpoints](https://github.com/d3Jesus/SimplifiedTransactionSystem/blob/main/assets/User-Endpoints.PNG)

### Transaction Endpoints
![Transaction-Endpoints](https://github.com/d3Jesus/SimplifiedTransactionSystem/blob/main/assets/Transaction-Endpoints.PNG)

### Built with

* [.Net Core v7.0](https://dotnet.microsoft.com/en-us/download)
* [Entity Framework Core v7.0.6](https://docs.microsoft.com/en-us/ef/core/get-started/overview/install)
* [InMemory Database v7.0.10](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/)

## How to use
1. Fork this repository(Top right side)
2. Clone your forked repository
   <br />For example, run this command in your terminal/command prompt:
   ```
   git clone https://github.com/<YOUR_GITHUB_USERNAME>/SimplifiedTransactionSystem.git
   ```
3. Install all dependencies(You will need .Net SDK installed in your machine).

## Author
- [Yuran de Jesus](https://github.com/d3Jesus)

