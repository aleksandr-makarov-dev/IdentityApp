# AspNetCore Identity Application

Identity app is a implementation of different authentication and authorization tasks with AspNetCore Identity, EntityFramework, Razor Pages, API Controllers and React. The project is based on Adam Freeman's book [Pro ASP.NET CoreIdentityUnder the Hood with Authenticationand Authorization in ASP.NET Core Applications](https://www.amazon.com/ASP-NET-Core-Identity-Authentication-Authorization/dp/1484268571) and my own implementation of JWT + Refresh Token and React application. Completed 

## Done
- [x] Password authentication
- [x] Lockouts
- [x] Two factor authentication (Google Authenticator)
- [x] External authentication providers (Google, Github)
- [x] Email verification, password reset 
- [x] Role based authorization
- [x] Cookie based authentication
- [x] JsonWebToken + Refresh Token in Cookie authentication + React app
      
## Goals
- [ ] Keyclock authentication provider (in progress...)
- [ ] Role based authentication in React app (Role Guards)
- [ ] Magic links
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
![image](https://github.com/aleksandrmakarov-dev/IdentityApp/assets/128474912/2e1fb329-7897-42c2-ba30-cd77bd1fbee5)

