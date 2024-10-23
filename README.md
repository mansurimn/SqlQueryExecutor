C# console application for .NET Framework 4.8, 
It will execute the SQL query at specified intervals, log results, and handle errors gracefully.

Interval Time interval (in seconds) at which the SQL query is executed.
ConnectionString: SQL Server connection string.
SqlQuery: The query to execute at each interval.
LogFilePath: Path of the log file to store query results or errors.
Program.cs:

A Timer is set up to execute the SQL query at regular intervals.
The ExecuteSqlQuery method runs the query, logs the results, and handles any exceptions.
Results and errors are logged in the specified log file.

Install log4net NuGet Package:
Add configuration for log4net in the App.config.
Other configuration in the App.config.

**How It Works:**
The application will run a SQL query at intervals specified in the App.config file.
The results will be logged to the specified log file.
If an error occurs (e.g., SQL connection failure or query error), it will be caught and logged to the same file.

**Notes:**
SQL Connection: Make sure to provide a valid connection string in App.config.
Logging: The log file is appended with each new query execution or error, so all results/errors are kept for later review.
Error Handling: Any exceptions encountered during query execution will be gracefully logged.

