tranquire
=========

[![Build status](https://ci.appveyor.com/api/projects/status/qmv6y5576jyvra1q/branch/master?svg=true)](https://ci.appveyor.com/project/Galad/tranquire/branch/master)
[![Quality gate](https://sonarcloud.io/api/project_badges/measure?project=Tranquire&metric=alert_status)](https://sonarcloud.io/dashboard?id=Tranquire)
[![NuGet](https://img.shields.io/nuget/v/Tranquire.svg)](https://www.nuget.org/packages/Tranquire/)



Introduction
------------
Tranquire brings the [screenplay pattern](http://www.infoq.com/articles/Beyond-Page-Objects-Test-Automation-Serenity-Screenplay) to .NET. It is highly inspired by the implementation of [Serenity BDD](http://www.serenity-bdd.info). You can refer to the Serenity documentation to understand how Tranquire works.
Its goal is to better organize UI acceptance tests and make them more maintainable and readable, by applying the single responsability and open-closed principles to UI actions.

Tranquire enables UI actions by provinding an abstraction layer on top of [Selenium](http://www.seleniumhq.org/) in a form of a independant composable actions.

Documentation
---------------
See the [wiki](https://github.com/Galad/tranquire/wiki)

Solution overview 
--------
The solution is composed by Tranquire as well as test projects.

### Tranquire
* Tranquire : the core library
* Tranquire.Selenium : contains the ui actions and questions used to automate a web UI with Selenium.

### Demo project
* ToDoList : a [todomvc](http://todomvc.com/) web project used to demonstrate tranquire. 
* ToDoList.Automation : the automation framework for the todomvc app.
* ToDoList.Specifications : the acceptance tests for the todomvc app. Please run the ToDoList web project before executing the tests, so that IIS express can start.

### Tests projects
* Tranquire.Tests
* Tranquire.Selenium.Tests
