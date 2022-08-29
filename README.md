<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
# Simple REST API 
📚 Practice | Docker | .NET 6 | DAPPER  

This project is my practice repository to get familiar with .net 6 and C# 
Goal of this repo is to spend time on the code and lear how to write efficient code and how to work with variaty different databases. 
I wanted to implement JWT token auth where generated token can be used as bearer token for role-based end-points.
Learning objective was to learn SQL Database / code seperation / clean architecture

<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#">About The Project</a>
      <ul>
        <li><a href="#tech">Tech stack</a></li>
      </ul>
    </li>
    <li><a href="#features">Features</a></li>
    <li><a href="#under-constraction">Under Constraction</a></li>
    <li><a href="#licence">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>


## Tech
* [.NET 6](https://github.com/vuejs/vue)
* [ASP.NET CORE](https://github.com/vuejs/vuex)
* [DAPPER](https://github.com/vuejs/vue-router)
* [MSSQL](https://firebase.google.com)
* [DOCKER](https://firebase.google.com)
* [SWAGGER](https://firebase.google.com)

<!-- ABOUT THE PROJECT -->
## Swagger end-points

![Screenshot 2022-08-29 at 15 43 28](https://user-images.githubusercontent.com/72190589/187215472-fa958517-ca07-465c-90f2-9e3e748e6808.png)


## Features
- [x] Authentication
  - [x] Sign up
  - [x] Authentication
  - [x] Email verification
     - [x] FluentEmail
     - [x] Tested with ethereal.email
     - [x] Custom templates with wwwroot
  - [x] JWT auth
  - [x] Role-based auth
    - [x] User
    - [x] Admin
- [x] User
  - [x] Basic crud
- [x] Roles 
  - [x] Basic crud
- [x] Passsword hashing
  - [x] CryptoHasher
- [x] Docker 
  - [x] Docker compose up (db-mssql)
- [x] Validation
  - [x] Fluent validation with custom validators 
  
  

<!-- GETTING STARTED -->

## Under Constraction

- [x] Refresh token needs better generation and testing
- [x] Dapper mapping from user repositories is not async
- [x] Docker compose up has only mssql need to run api seperately
- [x] Want to fix throwing exceptions for better formatting 
- [x] Want to build some product end-points and one-to-one address 

## Licence 

Distributed under the MIT License. See LICENSE for more information.

TTT :black_nib:
## Contact

Tomas Simko - [@twitter](https://twitter.com/TomasSimko_) - simko.t@email.cz - [@linkedIn](https://www.linkedin.com/in/tomas-simko/)

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo.svg?style=for-the-badge
