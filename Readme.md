# 🏥 eHealthFolder

**eHealthFolder** is a digital health record management app designed for patients. It provides secure access to  medical history, documents, medications, and visits through **QR codes** and **biometrics**.

---

## ✨ Features
- 🔐 **QR Code & Biometrics Access** – Scan patient QR codes or use biometrics to retrieve profiles.  
- 📂 **Patient Folder System** – Organize lab results, prescriptions, discharge summaries, and more.  
- ⚡ **Emergency Mode** – Quickly display critical info (blood type, allergies, chronic conditions).  
- 💊 **Medication Tracker** – Track current & past medications with warnings for interactions.  
- 📑 **Secure Document Storage** – Upload, view, and annotate medical documents.    
- 🔒 **Access Control** – Role-based permissions (Doctor, Nurse, Admin) with audit logging.  

---

## 🛠️ Tech Stack
**Frontend:** .NET MAUI (C# + XAML)  
**Backend:** ASP.NET Core Web API  
**Database:** SQLite (development/offline), SQL Server (production)  
**File Storage:** Azure Blob Storage  
**Authentication:** Firebase (development), OAuth2/OpenID Connect (future)  
**Logging & Monitoring:** Serilog + Azure Application Insights  
**Deployment:** Docker + Azure App Service  

---

## 📂 Information Architecture
- **Patient Profile** → Emergency info, demographics, history.  
- **Documents** → Lab results, prescriptions, discharge notes.  
- **Medications** → Active + past treatments.  
- **Visits & Appointments** → History + upcoming visits.  
- **Audit Logs** → Track who accessed/modified records.  

---

## 🚀 Getting Started
### Prerequisites
- .NET 8 SDK  
- Visual Studio 2022 (with MAUI workload)  
- Azure account (for storage & hosting)  

### Setup
```bash
# Clone the repository
git clone https://github.com/yourusername/ehealthfolder.git
cd ehealthfolder

# Restore dependencies
dotnet restore

# Run the backend
cd eHealthFolder.API
dotnet run

# Run the mobile app
cd ../eHealthFolder.App
dotnet build
dotnet run
