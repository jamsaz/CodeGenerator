# JamsazCodeGenerator

Code Generator Project is a Template based code generator plugin for Visual Studio and a Visual Studio Project Template for generating WCF data applications.

It helps you to start a well designed project based on a well designed boiler plate. It's specifically designed for making different type of forms for data entities based on Mustache Templates. you can customize the templates to generate different forms or classes for your Data aware projects.

We already made templates for generating WCF data application based on Code First (Actually Code Second, because we need database tables created first) Entity Framework model. also we assume your database server is any version of Microsoft SQL Server.

All you need to start is use it's Visual Studio Project template to new a project then, you can provide extra metadata which CodeGenerator needs to create forms and classes through Visual Studio plugins. Based on entities you have in SQL Server DB and extra meta data you provide for each Table/Entity, you can generate the rest of the project using this Visual Studio Plugin and provided template. You can also customize the Mustache templates and initial visual studio project template to generate completely different type of projects.

This project is still in development phase and we will improve different part of it gradually. however the code is usable with minor tweaks.
