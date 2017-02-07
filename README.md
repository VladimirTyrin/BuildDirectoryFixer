# Build directory fixer

This is a test project for solution binary output directories change. Any customizations and improvements are welcome :simple_smile:

## Purpose

By default Visual studio stores binary build output in `bin` directory relative to project folder. 
This is unhandy for large solutions. This program looks for project files (`.csproj`) and fixes them to store build output in `build\PROJECT_NAME` directory relative to solution root  

## Warning

**DO NOT** use this if solution is not a **clean** git repository root. Changes cannot be undone using only this program.  

## Assumtions and limitations

* Windows OS (some backslashes are hardcoded)  
* Script is used in fork (solution path is hardcoded)  
* All projects are stored in `SOLUTION_ROOT\src\PROJECT_NAME` folders  
* As a result of the previous item: Xamarin and ASP.Net Core projects are not supported now :disappointed:    
* There exists a `build` directory in solution root directory.

## Build directory

Build directory shouldn't be gitignored completely. The easiest way to preserve it existing and clear is to put the following `.gitignore` in it:

```
# Ignore everything in this directory
*
# Except this file
!.gitignore
```

## Result

All project will output their build results to `SOLUTION_ROOT\build\PROJECT_NAME`.

If BUILD_TYPE is one of {Debug, Release} and ARCHITECTURE is one of {AnyCPU, x86, x64, ARM} then build folder is 

```
SOLUTION_ROOT\build\PROJECT_NAME\BUILD_TYPE\ARCHITECTURE\
```