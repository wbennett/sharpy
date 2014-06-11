sharpy
==========================

A web-based (r)ead (e)val (p)rint (l)oop (REPL) for the C# language. This tool was created for evaluating LINQ queries/closures on arbitrary data sets, and playing with Roslyn is fun.

Requirements
------------

### To download source
- git

###For the execution service:

- Windows
- .NET Framework 4.5 or higher
- Visual Studio 2012  or higher with Nuget support

###For the web application:

- Node JS (latest)

Getting started
---------------

###Get latest sources

    cd path/to/desired/home
    git clone git@github.com:wbennett/sharpy.git


### Execution Service

1. Build solution (solution will automatically pull required packages)
2. Navigate to target directory
3. Run with desired options

* * *
    cd (project directory)/sharpy/bin/Release/
    #yay server is now running
    ./sharpy -s http://localhost:4001/

The web service is running in apphost mode, ready to accept requests.

### Web Application
1. Get latest packages using `npm`
2. Configure the settings to point where the Execution Service is running.
3. Run web application with node

* * *
 
    cd (project directory)/SharpNet.Presentation/sharpy
    # need to edit (project directory)/config/environment.js to point to the web service
    npm update
    node app.js

##Usage 
####(after execution service and web application are running)
1. Go to http://localhost:4000/

     ![alt launch instance](https://github.com/wbennett/sharpy/raw/master/docs/images/launch.png)

2. Create a user (**yes it supports multiple users simultaneously**!)

     ![alt create](https://github.com/wbennett/sharpy/raw/master/docs/images/create.png)

3. Login in with user

    ![alt login](https://github.com/wbennett/sharpy/raw/master/docs/images/login.png)

4. Use the help to show some more features

    ![alt help](https://github.com/wbennett/sharpy/raw/master/docs/images/help.png)

5. Assign variables

    ![alt assign](https://github.com/wbennett/sharpy/raw/master/docs/images/assign.png)

6. Offend the compiler

    ![alt offend](https://github.com/wbennett/sharpy/raw/master/docs/images/offend.png)

7. Try doing something silly

    ![alt silly](https://github.com/wbennett/sharpy/raw/master/docs/images/silly.png)

8. Create an arbitrary collection

    ![alt collection](https://github.com/wbennett/sharpy/raw/master/docs/images/collection.png)

9. Do something LINQy with the collection

    ![alt linq](https://github.om/wbennett/sharpy/raw/master/docs/images/linq.png)

10. Create a complex type

    ![alt complex](https://github.com/wbennett/sharpy/raw/master/docs/images/complex.png)

11. Logout

    ![alt logout](https://github.com/wbennett/sharpy/raw/master/docs/images/logout.png)


    

### TODO

- Package everything nicely
- Make portable with mono (roslyn is open source)
- syntax highlighting
- tab completion
- syntax completion
- semantic completion
- support re-sharper (JK :P )


### SHOUT OUT

- Web Framework: [geddyjs.org](http://geddyjs.org)
- Compiler Framework: [Roslyn](http://msdn.microsoft.com/en-us/vstudio/roslyn.aspx)
- jQuery terminal plugin: [jq-console](https://github.com/replit/jq-console)
- Service Framework: [ServiceStack](https://github.com/ServiceStackV3/ServiceStackV3)
