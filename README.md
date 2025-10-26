# LumiNote

LumiNote is an AI-powered learning platform that automatically generates summaries and interactive quizzes from PDF documents using Google's Gemini AI.

## Features

- 📄 **PDF Upload & Processing** - Upload any PDF document
- 🤖 **AI-Powered Summaries** - Automatic generation of concise, bullet-point summaries
- 📝 **Smart Quiz Generation** - Creates 5-20 multiple-choice questions based on content depth
- ✅ **Interactive Quiz Taking** - Answer questions and track your progress
- 📊 **Score Tracking** - Review your answers with detailed explanations
- 🔐 **User Authentication** - Secure JWT-based authentication

## Tech Stack

### Backend
- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core** - ORM with SQL Server
- **Google Gemini AI** - AI-powered text processing
- **BCrypt.NET** - Password hashing
- **iText7** - PDF text extraction
- **JWT** - Authentication

### Frontend
- **Next.js** - React framework
- **React** - UI library
- **Tailwind CSS** - Styling

## Prerequisites

Before running the application locally, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 18+](https://nodejs.org/) and npm
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer Edition)
- A [Google Gemini API Key](https://ai.google.dev/)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/heir04/LumiNote.git
cd LumiNote
```

### 2. Backend Setup (API)

#### 2.1 Configure Database Connection

Navigate to the API directory and update the connection string in `appsettings.json`:

```bash
cd api
```

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=LumiNoteDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY_HERE"
  },
  "Jwt": {
    "Key": "your-super-secret-key-here-minimum-32-characters",
    "Issuer": "LumiNote",
    "Audience": "LumiNoteUsers"
  }
}
```

**Important:** 
- Replace `YOUR_GEMINI_API_KEY_HERE` with your actual Gemini API key
- Update the database connection string if needed
- Generate a secure JWT key (minimum 32 characters)

#### 2.2 Apply Database Migrations

Run the following commands to create the database:

```bash
dotnet ef database update
```

If you don't have the EF Core tools installed:

```bash
dotnet tool install --global dotnet-ef
```

#### 2.3 Restore NuGet Packages

```bash
dotnet restore
```

#### 2.4 Run the API

```bash
dotnet run
```

The API will start on `https://localhost:5001` (HTTPS) and `http://localhost:5000` (HTTP).

You should see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

### 3. Frontend Setup (Web)

Open a new terminal window and navigate to the web directory:

```bash
cd web
```

#### 3.1 Install Dependencies

```bash
npm install
```

#### 3.2 Configure API Endpoint (Optional)

If your API is running on a different port, create/update `.env.local`:

```bash
NEXT_PUBLIC_API_URL=http://localhost:5000
```

#### 3.3 Run the Development Server

```bash
npm run dev
```

The frontend will start on `http://localhost:3000`.

### 4. Access the Application

Open your browser and navigate to:
- **Frontend:** http://localhost:3000
- **API Swagger Documentation:** http://localhost:5000/swagger (if Swagger is enabled)

## Usage Flow

### 1. Register/Login
- Create a new account or login with existing credentials
- Store the JWT token (the frontend handles this automatically)

### 2. Upload PDF
- Navigate to the upload page
- Select a PDF document
- The system will:
  - Extract text from the PDF
  - Generate an AI summary (5-8 concise bullet points)
  - Create 5-20 quiz questions automatically

### 3. Review Summary
- View the AI-generated summary
- Save the Summary ID for taking the quiz

### 4. Take Quiz
- Use the Summary ID to load quiz questions
- Answer questions one by one
- Submit your answers

### 5. View Results
- Check your quiz score
- Review correct answers with explanations
- See which questions you got right/wrong

## API Endpoints

For detailed API documentation, see [API_DOCUMENTATION.md](./API_DOCUMENTATION.md)

### Quick Reference

**Authentication:**
- `POST /api/user/register` - Register new user
- `POST /api/user/login` - Login

**Summaries:** (All require authentication)
- `POST /api/summary/create` - Upload PDF and generate summary + quiz
- `GET /api/summary/{id}` - Get summary by ID
- `GET /api/summary/getall` - Get all user's summaries

**Quizzes:** (All require authentication)
- `GET /api/quiz/get/quiz-questions/{summaryId}` - Get quiz questions by summary ID
- `POST /api/quiz/question/answer/{questionId}` - Submit answer to question
- `GET /api/quiz/get/quiz-summary/{quizId}` - Get quiz score/summary
- `GET /api/quiz/get/questions/{quizId}` - Get questions with answers (for review)

## Project Structure

```
LumiNote/
├── api/                          # Backend API (.NET)
│   ├── Controllers/              # API Controllers
│   ├── Application/              # Business logic layer
│   │   ├── DTOs/                 # Data Transfer Objects
│   │   ├── Interface/            # Service interfaces
│   │   └── Service/              # Service implementations
│   ├── Entities/                 # Database entities
│   ├── Infrastructure/           # Infrastructure layer
│   │   ├── Context/              # Database context
│   │   ├── Security/             # Auth & JWT services
│   │   └── Service/              # External services (Gemini)
│   └── Program.cs                # Application entry point
├── web/                          # Frontend (Next.js)
│   ├── app/                      # Next.js app directory
│   ├── public/                   # Static assets
│   └── package.json              # npm dependencies
├── API_DOCUMENTATION.md          # Detailed API documentation
└── README.md                     # This file
```

## Environment Variables

### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=LumiNoteDb;..."
  },
  "Gemini": {
    "ApiKey": "your-gemini-api-key"
  },
  "Jwt": {
    "Key": "your-jwt-secret-key",
    "Issuer": "LumiNote",
    "Audience": "LumiNoteUsers"
  }
}
```

### Frontend (.env.local)
```bash
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server is running
- Verify connection string in `appsettings.json`
- Check firewall settings
- Try using SQL Server Authentication instead of Windows Authentication

### Gemini API Errors
- Verify your API key is valid
- Check your Gemini API quota/limits
- Ensure you have internet connectivity

### Port Already in Use
- Change the port in `Properties/launchSettings.json` for the backend
- Change the port in `package.json` for the frontend

### Migration Issues
```bash
# Reset migrations
dotnet ef database drop
dotnet ef database update
```

## Development

### Adding New Migrations

```bash
cd api
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Building for Production

#### Backend
```bash
cd api
dotnet publish -c Release -o ./publish
```

#### Frontend
```bash
cd web
npm run build
npm start
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please open an issue on GitHub.

## Acknowledgments

- [Google Gemini AI](https://ai.google.dev/) - AI-powered text processing
- [iText7](https://itextpdf.com/) - PDF processing
- [Next.js](https://nextjs.org/) - React framework
- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) - Web framework

---

Made with ❤️ by the LumiNote Team
