# Customer Management REST API

## Requirements
- Customer data management with CRUD operations
- Data persistence using JSON file
- Input validation
- Sorted storage of customers without using builtin functionalities (by LastName, FirstName)

## What I added
 - I added Unit test for the project.

## Project Structure
```
RestAPI/
├── Controllers/
│   └── CustomersController.cs      # Core API logic
├── DTOs/
│   ├── Customer.cs                 # Customer data model
│   └── CustomerResponse.cs         # API response model
├── Program.cs                      # API configuration
└── customers.json                  # Data storage
```

## Technical Details

### 1. Endpoints

#### GET /customers
- Returns all customers
- No parameters required
- Returns 200 OK with customer list

#### POST /customers
- Adds multiple customers
- Request body: Array of customer objects
- Validates each customer:
  - Required fields check
  - Age > 18
  - Unique ID
- Returns success/error response

### 2. Data Management
- **Sorted Insertion**: Custom implementation without using built-in sorting
- **File Persistence**: Saves changes to `customers.json`
- **Thread Safety**: Uses static list for in-memory storage

### 3. Validation Rules
- All fields (FirstName, LastName, Age, Id) must be present
- Age must be greater than 18
- IDs must be unique
- Data is automatically sorted by LastName, then FirstName

## How to Run

1. **Clone the Repository**
   ```bash
   git clone https://github.com/ronalison1009/RestAPI.git
   ```

2. **Navigate to Project Directory**
   ```bash
   cd RestAPI
   ```

3. **Run the Application**
   ```bash
   dotnet run
   ```

4. **Access the API**
   - HTTP: http://localhost:5000
   - HTTPS: https://localhost:7000

## Testing
- Unit tests implemented using xUnit
- Test cases cover:
  - Getting all customers
  - Adding valid customers
  - Validating invalid customer data
```
