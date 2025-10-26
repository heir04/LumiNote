# LumiNote API Documentation

## Base URL
```
http://localhost:5000/api
```

## Authentication
Protected endpoints require a JWT token in the Authorization header:
```
Authorization: Bearer <your_jwt_token>
```

---

## 1. User Controller
Base route: `/api/user`

### 1.1 Register User
**Endpoint:** `POST /api/user/register`  
**Authentication:** ❌ Not Required  
**Description:** Create a new user account

**Request Body:**
```json
{
  "email": "user@example.com",
  "name": "John Doe",
  "password": "securePassword123"
}
```

**Success Response (200 OK):**
```json
{
  "message": "User registered successfully.",
  "status": true,
  "data": null
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Email already exists",
  "status": false,
  "data": null
}
```

---

### 1.2 Login
**Endpoint:** `POST /api/user/login`  
**Authentication:** ❌ Not Required  
**Description:** Login with email and password to receive JWT token

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "securePassword123"
}
```

**Success Response (200 OK):**
```json
{
  "message": "Welcome",
  "status": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com"
  }
}
```

**Error Response (401 Unauthorized):**
```json
{
  "message": "Incorrect Email or password!",
  "status": false,
  "data": null
}
```

---

## 2. Summary Controller
Base route: `/api/summary`

> ⚠️ **All Summary endpoints are protected and require authentication**

### 2.1 Create Summary
**Endpoint:** `POST /api/summary/create`  
**Authentication:** ✅ Required  
**Description:** Upload a PDF file to generate a summary and quiz questions

**Request Body:**
- **Content-Type:** `multipart/form-data`
- **Form Data:**
  - `file`: PDF file (required)

**Example (using FormData in JavaScript):**
```javascript
const formData = new FormData();
formData.append('file', pdfFile);

fetch('http://localhost:5000/api/summary/create', {
  method: 'POST',
  headers: {
    'Authorization': 'Bearer <your_jwt_token>'
  },
  body: formData
});
```

**Success Response (200 OK):**
```json
{
  "message": "Summary created successfully",
  "status": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Document Title",
    "userId": "user-id-here",
    "content": "This is the generated summary content...",
    "createdAt": "2025-10-26T10:30:00Z"
  }
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Invalid file format. Only PDF files are allowed",
  "status": false,
  "data": null
}
```

---

### 2.2 Get Summary by ID
**Endpoint:** `GET /api/summary/{id}`  
**Authentication:** ✅ Required  
**Description:** Retrieve a specific summary by its ID

**URL Parameters:**
- `id` (Guid): The unique identifier of the summary

**Example Request:**
```
GET /api/summary/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Success Response (200 OK):**
```json
{
  "message": "Summary retrieved successfully",
  "status": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Document Title",
    "userId": "user-id-here",
    "content": "This is the generated summary content...",
    "createdAt": "2025-10-26T10:30:00Z"
  }
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Summary not found",
  "status": false,
  "data": null
}
```

---

### 2.3 Get All Summaries for Current User
**Endpoint:** `GET /api/summary/getall`  
**Authentication:** ✅ Required  
**Description:** Retrieve all summaries created by the authenticated user

**Success Response (200 OK):**
```json
{
  "message": "Summaries retrieved successfully",
  "status": true,
  "data": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "title": "First Document",
      "userId": "user-id-here",
      "content": "Summary content 1...",
      "createdAt": "2025-10-26T10:30:00Z"
    },
    {
      "id": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
      "title": "Second Document",
      "userId": "user-id-here",
      "content": "Summary content 2...",
      "createdAt": "2025-10-25T09:15:00Z"
    }
  ]
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Failed to retrieve summaries",
  "status": false,
  "data": null
}
```

---

## 3. Quiz Controller
Base route: `/api/quiz`

> ⚠️ **All Quiz endpoints are protected and require authentication**

> 📌 **Important: Quiz Creation Flow**
> 
> Quizzes are **automatically created** when you upload a PDF file to `/api/summary/create`:
> 1. Upload PDF → System extracts text
> 2. AI (Gemini) generates a summary AND 5 quiz questions automatically
> 3. Both Summary and Quiz are saved with a linked relationship (`SummaryId` in Quiz)
> 4. To take the quiz, use the Summary ID you received when creating the summary
> 
> **You don't manually create quizzes** - they're automatically generated alongside summaries!

---

### 3.1 Get Quiz Questions for Display (by Summary ID)
**Endpoint:** `GET /api/quiz/get/quiz-questions/{summaryId}`  
**Authentication:** ✅ Required  
**Description:** Get quiz questions for taking the quiz. Uses Summary ID to find the associated quiz and returns questions WITHOUT correct answers (for quiz-taking mode).

**URL Parameters:**
- `summaryId` (Guid): The unique identifier of the summary (received when uploading PDF)

**Example Request:**
```
GET /api/quiz/get/quiz-questions/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Success Response (200 OK):**
```json
{
  "message": "Questions retrieved successfully",
  "status": true,
  "data": [
    {
      "quizId": "quiz-id-generated-by-system",
      "id": "question-id-1",
      "questionNumber": 1,
      "questionText": "What is the capital of France?",
      "options": ["London", "Paris", "Berlin", "Madrid"],
      "isAnswered": false,
      "userAnswer": null
    },
    {
      "quizId": "quiz-id-generated-by-system",
      "id": "question-id-2",
      "questionNumber": 2,
      "questionText": "What is 2 + 2?",
      "options": ["3", "4", "5", "6"],
      "isAnswered": false,
      "userAnswer": null
    }
  ]
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Quiz not found for the specified summary.",
  "status": false,
  "data": null
}
```

---

### 3.2 Get Quiz Questions (with answers)
**Endpoint:** `GET /api/quiz/get/questions/{quizId}`  
**Authentication:** ✅ Required  
**Description:** Get all questions for a quiz including answers and explanations (for review after completion)

**URL Parameters:**
- `quizId` (Guid): The unique identifier of the quiz

**Example Request:**
```
GET /api/quiz/get/questions/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Success Response (200 OK):**
```json
{
  "message": "Questions retrieved successfully",
  "status": true,
  "data": [
    {
      "quizId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "questionText": "What is the capital of France?",
      "options": ["London", "Paris", "Berlin", "Madrid"],
      "answer": "Paris",
      "isAnswered": true,
      "isAnsweredCorrectly": true,
      "explanation": "Paris is the capital and largest city of France.",
      "userAnswer": "Paris"
    },
    {
      "quizId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "questionText": "What is 2 + 2?",
      "options": ["3", "4", "5", "6"],
      "answer": "4",
      "isAnswered": true,
      "isAnsweredCorrectly": false,
      "explanation": "Basic arithmetic: 2 + 2 equals 4.",
      "userAnswer": "3"
    }
  ]
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Quiz not found",
  "status": false,
  "data": null
}
```

---

### 3.3 Get Quiz Summary
**Endpoint:** `GET /api/quiz/get/quiz-summary/{quizId}`  
**Authentication:** ✅ Required  
**Description:** Get quiz summary with score and progress information

**URL Parameters:**
- `quizId` (Guid): The unique identifier of the quiz

**Example Request:**
```
GET /api/quiz/get/quiz-summary/3fa85f64-5717-4562-b3fc-2c963f66afa6
```

**Success Response (200 OK):**
```json
{
  "message": "Quiz summary retrieved successfully",
  "status": true,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "totalQuestions": 10,
    "answeredCount": 7,
    "score": 5,
    "createdAt": "2025-10-26T10:30:00Z"
  }
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Quiz not found",
  "status": false,
  "data": null
}
```

---

### 3.4 Submit Answer to Question
**Endpoint:** `POST /api/quiz/question/answer/{questionId}`  
**Authentication:** ✅ Required  
**Description:** Submit an answer to a specific quiz question

**URL Parameters:**
- `questionId` (Guid): The unique identifier of the question

**Request Body:**
```json
{
  "userAnswer": "Paris"
}
```

**Example Request:**
```
POST /api/quiz/question/answer/question-id-1
```

**Success Response (200 OK):**
```json
{
  "message": "Answer submitted successfully.",
  "status": true,
  "data": null
}
```

> **Note:** The response does not include whether the answer was correct or the correct answer. To check if the answer was correct, you need to call the "Get Quiz Questions" endpoint after submitting.

**Error Response (400 Bad Request):**
```json
{
  "message": "Question not found or already answered",
  "status": false,
  "data": null
}
```

---

## Common Response Structure

All API responses follow this structure:

```json
{
  "message": "Description of the result",
  "status": true/false,
  "data": {} or [] or null
}
```

- **message**: Human-readable message describing the result
- **status**: Boolean indicating success (true) or failure (false)
- **data**: The actual response data (object, array, or null)

---

## Error Codes

| Status Code | Description |
|-------------|-------------|
| 200 | Success |
| 400 | Bad Request - Invalid input or business logic error |
| 401 | Unauthorized - Authentication required or failed |
| 404 | Not Found - Resource doesn't exist |
| 500 | Internal Server Error - Server-side error |

---

## Notes for Frontend Developers

1. **Authentication Flow:**
   - Call `/api/user/register` or `/api/user/login` first
   - Store the JWT token from the response
   - Include token in Authorization header for all protected endpoints
   - Token format: `Bearer <token>`

2. **PDF Upload & Automatic Quiz Generation:**
   - Upload PDF to `/api/summary/create` using `multipart/form-data`
   - Only PDF files are accepted
   - The system **automatically** generates:
     - A summary of the PDF content
     - A quiz with 5 questions based on the PDF
   - Response includes the Summary ID - **save this!**
   - Maximum file size may be limited (check with backend team)

3. **Complete Quiz Flow (IMPORTANT!):**
   ```
   Step 1: Upload PDF
   POST /api/summary/create
   → Returns: { data: { id: "summary-id", ... } }
   
   Step 2: Get Quiz Questions (using Summary ID from Step 1)
   GET /api/quiz/get/quiz-questions/{summary-id}
   → Returns: Array of questions WITHOUT answers
   
   Step 3: User answers questions one by one
   POST /api/quiz/question/answer/{question-id}
   Body: { "userAnswer": "Paris" }
   → Returns: Success confirmation (no answer feedback)
   
   Step 4: Get Quiz Summary (check score)
   GET /api/quiz/get/quiz-summary/{quiz-id}
   → Returns: Score, total questions, answered count
   
   Step 5: Review answers with explanations
   GET /api/quiz/get/questions/{quiz-id}
   → Returns: All questions WITH correct answers and explanations
   ```

4. **Key Points:**
   - **DO NOT** try to manually create quizzes via `/api/quiz/create` - quizzes are auto-generated with summaries
   - Use **Summary ID** to get quiz questions (Step 2)
   - Use **Question ID** to submit answers (Step 3)
   - Use **Quiz ID** (from questions response) to get summary and review (Steps 4-5)

5. **Guid Format:**
   - All IDs use Guid format: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`
   - Example: `3fa85f64-5717-4562-b3fc-2c963f66afa6`

6. **Date Format:**
   - All dates are in ISO 8601 format: `YYYY-MM-DDTHH:mm:ssZ`
   - Example: `2025-10-26T10:30:00Z`

7. **Protected Endpoints:**
   - Summary Controller: All endpoints require authentication
   - Quiz Controller: All endpoints require authentication
   - User Controller: No endpoints require authentication
