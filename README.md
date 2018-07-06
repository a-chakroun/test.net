# .Net Core Web API project template
A basic project template to kick start your .Net Core Web API application.

The project is configured with the following tools:

- StyleCop to keep a consist coding style throughout your project.
- Swagger to document your API.
- XUnit for writing unit tests.
- Shouldly to write easy to understand tests.
- TestServer to write functional tests.


In addition to configuring the above tools this project demonstrates the following key concepts:

- How to write a CRUD API.
- How to validate a data model.
- How to use application settings.
- How to test a CRUD API.


Looking for a template with `Azure B2C Authentication` or `Azure Table Storage`? Check out the `addon` branches for additional templates.


## Get Started
These instructions are for `Visual Studio 2017` but it should be a similar process for `JetBrains Rider` and `Visual Studio Code`.

1. Fork or Clone the project.
	`git clone https://github.com/ianc1/DotNetCoreWebApiTemplate.git`

2. Verify that you can build the solution and run the tests in Visual Studio before making any changes.

3. Rename the solution folder in Windows Explorer.

4. Open the solution in Visual Studio and rename the solution using the Solution Explorer.

5. Rename each of the projects using the Solution Explorer. Keep the `.Tests` suffix on the tests project.

6. Open each of the project's properties and update the Assembly Name, XML Documentation File and Project Id to match the new project name.

7. Remove all projects using the Solution Explorer. Save when prompted.

8. Rename all project folders using Windows Explorer to match the new project names.

9. Add back in the renamed projects using the Solution Explorer in Visual Studio. Make sure you add the projects back into the same src and test folders they were removed from.

10. Add a reference to the test project to reference the main API project.

11. Right click each project and choose Refactor -> Adjustnamespaces to update the C# files with the new namespace. Accept the default options. (This may require the ReSharper plugin)

12. Save the solution and close and reopen it. You should now be able to build and run the tests.

13. If you have cloned rather than forked the project you will need to update the Git origin to your own repo.
    `git remote set-url origin https://new.url.here`

14. Start coding.