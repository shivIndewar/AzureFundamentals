📦 Azure File Processing Pipeline (.NET + Azure Functions)

This repository contains a scalable, event-driven file processing system built using Azure Functions, Azure Blob Storage, Azure Queue Storage, and SQL Server.

🧭 Solution Structure

Your solution AzureFundamentals consists of:

<img width="449" height="155" alt="image" src="https://github.com/user-attachments/assets/4b189f53-00b5-4d9f-b0bd-1c42ed8eae4b" />

🔁 Workflow Overview

<img width="452" height="188" alt="image" src="https://github.com/user-attachments/assets/a69eef41-0fe5-4174-97dd-64cb50b9d9e7" />



🌐 AzureFunctionWeb

Handles file uploads

Sends files to Azure Blob Storage

Pushes a message to Azure Queue

Provides APIs/UI to fetch processed data

⚡ AzureFunc

Azure Function App

Triggered by Queue

Reads file from Blob Storage

Processes file content

Stores structured data into SQL Server

🧠 AzureBobProject

Shared business logic

Data models

Database interaction (Entity Framework / ADO.NET)

Helper services (Blob, Queue, parsing, etc.)


☁️ Azure Services Used

Azure Blob Storage → File storage

Azure Queue Storage → Message queue

Azure Functions → Background processing

SQL Server → Data persistence

✅ Prerequisites

.NET SDK
 (6 or later)

Azure Subscription

Azure Storage Account

SQL Server (local or Azure SQL)

🔧 Configuration

Update configuration in:
<img width="452" height="193" alt="image" src="https://github.com/user-attachments/assets/00dab761-32bc-4a8d-92f1-9d453c197802" />
<img width="457" height="224" alt="image" src="https://github.com/user-attachments/assets/bc4ca103-d6b2-42b2-a245-b8aff3c102b6" />
<img width="438" height="233" alt="image" src="https://github.com/user-attachments/assets/54607ba8-173c-4948-a40c-cb8e1cff1306" />

📌 Key Features

✅ Event-driven architecture

✅ Decoupled services using queues

✅ Scalable background processing

✅ Clean separation of concerns

✅ Reusable core logic

📊 Architecture Diagram
<img width="707" height="218" alt="image" src="https://github.com/user-attachments/assets/37799a57-a371-40c1-8106-e98b915dd785" />

<img width="602" height="374" alt="image" src="https://github.com/user-attachments/assets/fec432fa-9eb7-4cc6-9b14-46ccc6dfd1a1" />

Result - 

<img width="519" height="389" alt="Screenshot 2026-02-27 154615" src="https://github.com/user-attachments/assets/9dcc424f-a805-480f-9b61-bb45f3f85ba4" />
