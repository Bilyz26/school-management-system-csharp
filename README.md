# School Management System (prj_ForYou) 🏫

![C#](https://img.shields.io/badge/Language-C%23%207.0-blue.svg)
![Framework](https://img.shields.io/badge/Framework-.NET%20Framework%204.6.1-purple.svg)
![Database](https://img.shields.io/badge/Database-SQL%20Server-red.svg)
![Security](https://img.shields.io/badge/Security-Parameterized%20SQL-green.svg)
![Reports](https://img.shields.io/badge/Reports-Crystal%20Reports-orange.svg)
![UI](https://img.shields.io/badge/UI-Windows%20Forms-green.svg)

## 📌 Project Overview
**prj_ForYou** is a production-ready Windows Desktop Application built in C# (.NET Framework 4.6.1) and SQL Server for managing private support schools and educational centers (*Centre de Soutien Scolaire*). 

The system automates student registration, academic timetable generation, teacher assignment, group management, monthly payment tracking, financial revenue splitting, and Crystal Reports printing.

---

## ✨ Features & Functional Modules

### 🔐 1. Authentication & Security
- User login authentication for administration staff (`emp` table).
- Parameterized SQL queries preventing SQL Injection vulnerabilities across all form operations.
- Dynamic `App.config` database connection resolution with fallback support.
- Role-based UI navigation sidebar with dynamic panel switching.

### 👨‍🎓 2. Student Management (`frm_Inscription_dun_eleve`)
- Register new students with CIN (national ID) validation.
- Handles CIN ownership differentiation (Student vs. Parent).
- Registration fee (`frinsc`) tracking and registration date logging.

### 👨‍🏫 3. Staff & Subject Management (`frmGestiondesProfs`, `frmGestiondesEmployees`, `frmGestiondesMatier`, `frmGestionNiveaumatier`)
- Management of subjects (*Matière*) and education levels (*Niveau*).
- Instructor (*Professeur*) profiles linked to specific teaching subjects.
- Employee account administration with parameterized update/delete handlers.

### 👥 4. Group Assignment & Student Allocation (`frmraaf`)
- Multi-tier cascading dropdowns for Subject ➔ Level ➔ Instructor ➔ Group filtering.
- Enrolls students into specific study groups (*Rattachement*).
- Live counter displaying group capacity and current student headcount.

### 📅 5. Weekly Timetable Generator & Scheduling (`frmGestiondes_seances`, `frmModif_Delete_Seance`)
- Interactive day-by-day weekly session scheduling (Monday through Sunday).
- Automated conflict checking per group, instructor, academic year, and day.
- Transactional rollback algorithm to clean up incomplete batch insertions if scheduling fails.

### 💰 6. Payment & Financial Management (`frmPaiement`, `frmExplorerpaiment`, `frmfinannce`)
- Monthly tuition fee payment entry with duplicate month prevention.
- Unpaid student discovery algorithm: Filters students enrolled in groups who haven't paid for a target month.
- Financial revenue module: Calculates total earnings per group or per teacher, with customizable percentage commission splits between the center and instructors.

### 🖨️ 7. Report Generation & Printing (`frmImprimer`, `frmAbsence`, Crystal Reports `CR.rpt`)
- Attendance & absence sheet generator.
- Dynamic data binding using SAP Crystal Reports engine and typed DataSets (`DS.xsd`).

---

## 🛠️ Project Structure

```
school-management-system-csharp/
├── prj_ForYou.sln                    # Visual Studio Solution File
├── README.md                         # Project Documentation
├── Database_Setup.sql                # Complete SQL Server Database Setup Script
├── .gitignore                        # Git Version Control Exclusion Rules
└── prj_ForYou/                       # Main C# WinForms Project Folder
    ├── App.config                    # Connection Strings & App Configuration
    ├── prj_ForYou.csproj             # C# Project File
    ├── MemberGlobal.cs               # Central Database Helper Utility & Parameterized Query Handlers
    ├── Program.cs                    # Application Entry Point
    ├── DS.xsd / DS.Designer.cs       # Typed DataSet Schema for Database Binding
    ├── CR.rpt / CR.cs                # Crystal Reports Attendance Template
    ├── FrmMenu.cs                    # Main Modern Dashboard Navigation Form
    ├── frmLogIn.cs                   # Parameterized Login Window
    ├── frm_Inscription_dun_eleve.cs  # Student Inscription Form
    ├── frmraaf.cs                    # Student-Group Assignment Form
    ├── frmGestiondesProfs.cs         # Teacher Management Form
    ├── frmGestiondesEmployees.cs     # Employee Management Form
    ├── frmGestiondesMatier.cs        # Subject Management Form
    ├── frmGestionNiveaumatier.cs     # Subject Level Management Form
    ├── frmCreationNouveauGroupe.cs   # Group Creation Form
    ├── frmGestiondes_seances.cs      # Session & Timetable Scheduling Form
    ├── frmModif_Delete_Seance.cs     # Session Edit/Delete Form
    ├── frmPaiement.cs                # Payment Transaction Form
    ├── frmExplorerpaiment.cs         # Unpaid Student Explorer Form
    ├── frmfinannce.cs                # Financial Revenue & Calculation Form
    ├── frmImprimer.cs                # Report Parameters Selection Form
    └── frmAbsence.cs                 # Crystal Reports Viewer Form
```

---

## 🗄️ Database Architecture

The application relies on Microsoft SQL Server (`DB_Support_School`). Main relational tables:

| Table | Description |
| :--- | :--- |
| `emp` | Employee & admin accounts (username, password, role) |
| `prof` | Instructors linked to subjects (`#idmat`) |
| `matier` | Subjects taught at the center |
| `niveauMat` | Educational levels linked to subjects |
| `grp` | Student groups mapped to subjects and levels |
| `Annee` | Academic school years |
| `inscStd` | Registered student master directory |
| `Raff` | Junction table mapping students to groups, teachers, and academic years |
| `seance` | Timetable sessions (group, year, teacher, day, start time, end time) |
| `pay` | Payment records (student, group, teacher, subject, month, year, amount, date) |

---

## 🚀 Getting Started

### Prerequisites
- **IDE:** Visual Studio 2017 or newer (.NET Desktop Development workload installed).
- **Runtime:** .NET Framework 4.6.1.
- **Database Engine:** Microsoft SQL Server / SQL Express.
- **Reporting:** SAP Crystal Reports runtime for Visual Studio.

### Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Bilyz26/school-management-system-csharp.git
   ```

2. **Database Initialization:**
   - Run **`Database_Setup.sql`** in SQL Server Management Studio (SSMS) or via command line:
     ```bash
     sqlcmd -S . -i Database_Setup.sql
     ```
   - This automatically creates the `DB_Support_School` database, all 10 relational tables, foreign key constraints, and seeds default admin credentials (`admin` / `admin123`).

3. **Database Connection Configuration:**
   - Update the connection string in `prj_ForYou/App.config` if your SQL Server instance uses a custom server name:
     ```xml
     <connectionStrings>
         <add name="prj_ForYou.Properties.Settings.DB_Support_SchoolConnectionString"
             connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=DB_Support_School;Integrated Security=True"
             providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```

4. **Build & Run:**
   - Open `prj_ForYou.sln` in Visual Studio.
   - Build the solution (`Ctrl + Shift + B`).
   - Run the project (`F5`).

---

## 📄 License
This project is open-source and available for educational and administrative development purposes.
