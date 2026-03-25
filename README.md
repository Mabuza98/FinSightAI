# 🚀 FinSightAI — AI Financial Intelligence Platform

> Transforming financial documents into actionable intelligence using AI.


🌍 Live Demo Frontend: https://finsight-ai-ruby.vercel.app

Backend API (Swagger): https://finsight-ai-api-gycpfaa4dpf3huan.southafricanorth-01.azurewebsites.net/swagger


This project showcases the ability to design, build, and deploy a real-world AI-powered SaaS application.

---

## 🌍 Overview

**FinSightAI** is a full-stack AI-powered platform that analyzes financial documents and extracts high-value insights such as:

- ⚠️ Risk exposure  
- 📈 Growth signals  
- 💡 Opportunities  
- 🧠 Strategic insights  

Built for modern businesses, analysts, and decision-makers who want **data-driven clarity** from complex documents.

---

## 🧠 What Makes This Powerful?

FinSightAI is not just a dashboard.

It’s an **AI intelligence layer** on top of your documents.

✔ Upload documents  
✔ AI processes & indexes them  
✔ Ask questions (ChatGPT-style)  
✔ Get structured insights instantly  

---

## ✨ Core Features

### 📊 Intelligent Dashboard
- KPI Cards (Documents, Risks, Growth, Opportunities)
- Visual analytics (Pie charts, breakdowns)
- Real-time insight aggregation

---

### 💬 AI Document Chat
- Ask questions about any document
- Context-aware responses
- Multi-document intelligence

---

### 📄 Document Management
- Upload & manage files
- Processing + indexing pipeline
- Clean UI with search capability

---

### 🧠 Insights Engine (🔥 Key Feature)
- AI detects:
  - Risk ⚠️
  - Growth 📈
  - Opportunities 💡
- Drill-down per document
- Structured insight classification

---

### 📈 Advanced Analytics
- Per-document charts
- Insight distribution visualization
- Financial signal breakdowns

---

## 🏗️ Architecture
Frontend (React + Vite)

↓

API Layer (ASP.NET Core)

↓

AI Services

├── Azure OpenAI

├── Azure AI Search (Vector DB)

└── Document Processing Pipeline

↓

Storage (Azure Blob)


---

## 🛠️ Tech Stack

### 🎨 Frontend
- React (Vite)
- Tailwind CSS
- Recharts

### ⚙️ Backend
- ASP.NET Core Web API
- C#
- Entity Framework Core
- JWT Authentication

### ☁️ Cloud & AI
- Azure OpenAI
- Azure Blob Storage
- Azure Cognitive Search (Vector Search)

### 🚀 Deployment
- Frontend: Vercel
- Backend: Azure App Services

---

## 🔐 Authentication Flow
Register → Login → JWT Token → Protected Routes


- Secure token-based authentication
- Auto-attached API headers

---

## 📡 API Endpoints

POST /api/Auth/register

POST /api/Auth/login

GET /api/Document/my-documents

POST /api/Document/upload-single

POST /api/Ai/query

GET /api/Insights


---

## ⚙️ Local Setup

### 1️⃣ Clone Repo

git clone https://github.com/Mabuza98/FinSightAI.git
cd FinSightAI

2️⃣ Frontend Setup
cd finsight-ui
npm install
npm run dev

Create .env:
VITE_API_URL=https://your-api-url/api

3️⃣ Backend Setup
cd FinSight.API
dotnet run

🚀 Live Demo
🌐 Frontend: (your Vercel link)

⚙️ API: (your Azure link)

📸 Screenshots (Add These 🔥)

![Backendswagger](./screenshots/backendswagger.png
![Register+login](./screenshots/register+login.png
![Home](./screenshots/home.png
![Dashboard](./screenshots/dashboard.png)
![Chat](./screenshots/chat.png)
![Documents](./screenshots/documents.png)
![Insights](./screenshots/insights.png)
![Settings](./screenshots/settings.png



🧭 Product Vision
FinSightAI is evolving into a next-gen financial intelligence platform:

📊 Predictive analytics

🧠 AI-driven decision support

🏢 Multi-tenant SaaS (companies & teams)

📤 Export insights (PDF / Excel)

🤝 Collaboration tools


💡 Why This Project Stands Out
Real-world AI integration (not demo-level)

Full-stack architecture (frontend + backend + cloud)

Uses modern AI stack (Azure OpenAI + vector search)

Solves an actual business problem

Scalable SaaS foundation


👨‍💻 Author
Justice Mabuza
AI Engineer | Full Stack Developer


⭐ Support
If you like this project:

👉 Star the repo
👉 Share it
👉 Build on top of it

📜 License
MIT License