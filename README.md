### Star Wars API 

------------


#### Important informations:

- **Clean architecture with implementation of CQRS + MediatR**

Despite the fact that this is a simple CRUD system I decided to implement CQRS pattern.
It gives a lot of possibilities to scale up our system with preserving of SOLID principles
as well as easy logging options on ORM or database level
- **ORM: Dapper**

I used Dapper for simple ORM operation becouse of simplicity of use and anemic data model.
It gives developer chance to write his own transaction script which
might be usefull in some cases
- **Database: MySql**

Data is stored in MySql databse. I had MySql Server already installed on my machine 
so it was easy to start with. Export of my databse can be found here: 
[swdb-dev](https://github.com/SzymonJarek/swdb-dev.git "swdb-dev").

After installation change connection string in
`appsettings.json`

Database diagram:

![swdb-dev relation diagram](https://i.ibb.co/M6T5R3F/swdb-dev-diagram.png "swdb-dev relation diagram")
- **Swagger support**

Application support Swagger OpenAPI documentation.
- **Unit test**

Sample unit tests can be found in test folder in solution tree


