# ListList
ListList is a user authenticated ASP.NET web application for maintaining lists.
There's a video [demo](https://youtu.be/rzk04myaZ78) and [documentation](https://www.dropbox.com/s/snsytx89gu8gfl3/ListList%20Documentation.pdf?dl=0).

## Database Setup
ListList will use Entity Framework to create the needed database tables for you but you will need to have [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) installed. You will also need to have a database created called "ListListDev". If you want to use another name for the database, you will have to go into web.config and change `initial catalog=ListListDev` in both connection strings to whatever name you want to use.
ListList was created using the code first approach so it's suggested that you make any schema changes in the code and let Entity Framework handle it. <br/><br />
(Optional) You may also want to install [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) if you haven't already.

## User Authentication
The user authentication is built on the [ASP.NET Identity system](https://www.asp.net/identity).

## Styling
The user interface is styled with [Bootstrap 3](http://getbootstrap.com).


