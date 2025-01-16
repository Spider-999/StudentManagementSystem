# StudentManagementSystem
# Used technologies
C# with ASP.NET Core with EntityFramework for Database management and IdentityFramework for 
User management.
Separation of logic using the MVC software architectural pattern.

# Features

### Log in and registration
User can log in/ register, select their role(professor or student) and choose
the year of study as a student or the department as a professor.

### Students
Students can solve homeworks, quizzes and project(with file uploads).

### Professors
Professors can create different types of homeworks(basic homework assignments, quizzes and projects),
they can grade homeworks, change the formula for calculating the grade for the discipline they're teaching.

### Quiz
Time based quiz with questions and the student gets the grade after submitting the questions.

### Assignment
Students type the solution in a text box and send it to the professor to be graded.

### Gamification elements.
For gamification there is a leaderboard that shows students with the most completed homeworks in
descending order.

### Handling of exceptions
The handling of exceptions is done using mostly custom exceptions.

### Project
Students can upload one or more files and the professor can download them as a ZIP archive.

### Export grades to CSV
Professors can download a CSV file with all of the students grades for each discipline and the final grade.

