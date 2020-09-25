# askLNU
The goal of the project is to provide a platform for asking questions on some studying materials and receiving answers to them. System allows to create a question in the chosen faculty, add tags, reply to created ones and mark the answers and questions based on how helpful they were.
### Description
To start with, user needs to go to the starting page to log in, also, if user doesn't have an account, he can register.

After log in user is navigated to page with all questions. In top of the page, user sees navbar with some buttons:

1. 'Main page' page, where user can see all question, add to favourites, look for details, filter them by tag names and the name of the faculty and also sort dy date, rate.
2. 'Create new' page, where user can create a new question.
3. 'Question details' page, where user can see the specific question, view and rate answers, write a new answer.
4. 'Profile' pages, where user can change personal information, view created questions and favourite questions.
5. 'Chat' page, where users can chat in real time (built using SignalR).
6. 'Logout' button, which helps the user to logout.

There are 4 roles in the project: Guest, User, Moderator, Admin.
1. Guest - user that isn’t logged in. Can sign up and sign in, view questions and answers, view user accounts.
2. User - can create a question, pick a faculty and add tags, search questions, answer them, rate answers and questions. 
Also can get and change info in his/her account and delete
his/her account.
3. Moderator - can view info about users and questions, block/unblock users, delete questions and answers.
4. Admin - can give/remove moderator rights, add/remove faculties, add/remove tags


## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development.
### Prerequisites
```
Visual Studio

```
### Installing

A step by step series of examples that tell you how to get a development env running

* Download the latest stable release from the download tab and unzip it to your web folder.
* Run the project from Visual Studio, test and develop.
* Enjoy ;-)

### System requirements

* ASP.NET 4.5+
* Visual Studio 2015+ 
* Full Trust

### Built With

* ASP.NET Core MVC Framework

