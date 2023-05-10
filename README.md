
# Web-App zur Verlaufsbefragung von Notfallpatient:innen (UKN Inselspital)
### General info
This is the Code for the Living Case 1 at the Bern university of applied sciences. The Living Case 1 project is an opportunity for students to work with real Stakeholders and develop real-world applications. For this project our Stakeholder was [Prof. Dr. med. Wolf Hautz](http://www.notfallmedizin.insel.ch/de/ueber-uns/details/personlist/46/person/detail/wolf-hautz-mme/) from the [UKN Inselspital in Bern](https://www.insel.ch/de/). 

The developers of this Project are
 - Jorma Steiner [(StoneyJr)](https://github.com/StoneyJr)
 - Nicolas Gujer [(AngryBacteria)](https://github.com/AngryBacteria)

### Background
Diagnostic errors are a problem for the safety of the patient. Especially in the environment of an emergency station they occur more often than regular hospital stations. Diagnostic errors seem to increase the mortality and hospital duration [[1]](https://pubmed.ncbi.nlm.nih.gov/31068188/). There is the need of identifying such errors and find out why they happen.
### Goals
Generally the goal of this project was to implement a working prototype for a Questionnaire-Website with which the UKN Bern can identify cases where diagnostic errors were present. For this the idea is to send patients an invitation for a questionnaire approximately two weeks after their stay in the emergency station. Our specific goals for this project were the following
 1. Create a web application for patients to complete a questionnaire
 2. Save the answers in a structured form in a Database
 3. Provide the possibility to the UKN to download the Data either via an Admin-Portal or files on the server
 
## Project structure
The Project is structured into a frontend built with VueJS 3 and a backend built with .NET 7.0. To use this project copy the source files into a local folder. 
```sh
https://github.com/AngryBacteria/lc1_diagerror.git
```
Additionally some further steps are needed for the project to work. The backend uses Firebase for Auth, so you need a Firebase project and a service account JSON file. For quickstart and documentation for Firebase please visit their official [website](https://firebase.google.com/).
Once you have the file put it into this folder with the following name

    Data/firebaseServiceAccount.json

### How to run the backend
First build the Database
```sh
cd backend
dotnet  ef  migrations  add  InitialCreate
dotnet  ef  database  update
```
Then run the Project with either the http or https profile.
```sh
dotnet run --launch-profile "https"
```
The API-Documentation should now be available with this link. Slight changes in port and https or http are possible, in the terminal the exact path will be specified
https://localhost:7184/swagger/index.html
### How to run the frontend
To run the frontend run these following commands in a seperate window
```sh
cd frontend
npm install
npm run dev
```
