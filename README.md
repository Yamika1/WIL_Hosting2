ReHubCenter

Re-Center Hub is a web based platform to support wellness and management to allow users to register, login securely, manage details and make individual or workshop bookings and view their booking information

The system also allows administrative dashboard where an authorised admin can view and manage individual or workshop bookings and update booking statuses 

The application uses ASP.NET Core MVC as the frontend to connect to a separate ASP.NET Core web API. Authentication and user management are handled using AP.NET Core Identity and Microsoft SQL Server used as the database 


Features 

User Registration

- new users can create an account using their email address and password
- registered users are automatically assigned the client role
- user account information is stored securely using ASP.NET Core Identity


User Login


users can login into the system using their registered credentials. after the login is successful the application would retrieve the users identity and role and create an authentication session within the MVC application


Role-based Access

there are two main roles : client and admin


Database storage 

uses Microsoft SQL Server to store information

such as :

-user accounts
- roles
- user role relationships
- individual bookings
- workshop bookings 





