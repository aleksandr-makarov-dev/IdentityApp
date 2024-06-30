# AspNetCore Identity Application

Identity app is a implementation of different authentication and authorization tasks with AspNetCore Identity, EntityFramework, Razor Pages, API Controllers and React. The project is based on Adam Freeman's book [Pro ASP.NET CoreIdentityUnder the Hood with Authenticationand Authorization in ASP.NET Core Applications](https://www.amazon.com/ASP-NET-Core-Identity-Authentication-Authorization/dp/1484268571) and my own implementation of JWT + Refresh Token and React application.

## Done
- [x] Password authentication
- [x] Lockouts
- [x] Two factor authentication (Google Authenticator)
- [x] External authentication providers (Google, Github)
- [x] Email verification, password reset 
- [x] Role based authorization
- [x] Cookie based authentication
- [x] JsonWebToken + Refresh Token in Cookie authentication + React app
- [x] Magic links
      
## Goals
- [ ] Keyclock authentication provider
- [ ] Role based authentication in React app, role Guards
- [ ] Two factor authentication (Email, SMS)
- [ ] and more...


## appsettings.json Sample
```json
{
  "ConnectionStrings": {
    "SqliteConnection": "Data Source=local.db"
  },
  "App": {
    "AppName": "IdentityApp"
  },
  "Identity": {
    "Role": "SuperAdmin",
    "Email": "superadmin@example.com",
    "Password": "superadmin123"
  },
  "Google": {
    "ClientId": "YOUR-GOOGLE-CLIENT-ID",
    "ClientSecret": "YOUR-GOOGLE-CLIENT-SECRET"
  },
  "Github": {
    "ClientId": "YOUR-GITHUB-CLIENT-ID",
    "ClientSecret": "YOUR-GITHUB-CLIENT-SECRET",
    "Scopes": [
      "user:email"
    ]
  },
  "JsonWebToken": {
    "Expires": 2,
    "KeySecret": "NobodyWillGuessMySuperPuperSecretKey"
  },
  "RefreshToken": {
    "Expires": 1440,
    "Name": "RefreshToken"
  }
}
```
## Screenshots

### Razor pages

![image](https://github.com/aleksandrmakarov-dev/IdentityApp/assets/128474912/2e1fb329-7897-42c2-ba30-cd77bd1fbee5)

![image](https://github.com/aleksandrmakarov-dev/IdentityApp/assets/128474912/a063c982-066e-4062-815b-0251503fba63)

![image](https://github.com/aleksandrmakarov-dev/IdentityApp/assets/128474912/ca4f0159-567a-4b98-88b2-158b8a669359)

### React

![image](https://github.com/aleksandrmakarov-dev/IdentityApp/assets/128474912/2848a254-5685-4e64-b577-21fae76ffd3d)

![image](https://github.com/aleksandrmakarov-dev/IdentityApp/assets/128474912/0575323c-662a-4326-989c-2789d58dd300)

![image](https://github.com/aleksandrmakarov-dev/IdentityApp/assets/128474912/d23e95d8-33a7-4e79-a3d5-6379473a6b13)

