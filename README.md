# RanorexReport

## Overview 

This solution is creating __*allure-results*__ folder with all artefacts like *.-results.json, *.-container.json, attachments. 

Any Allure version (2 or 3) can be used for report generation.

## Setup
### Java Installation

1. Go to the official page: 👉 https://learn.microsoft.com/en-us/java/openjdk/download
2. Scroll to the Windows section.
3. Download either: MSI Installer (x64/x86/ARM) – easiest way.
4. Run the installer and follow the steps.
5. Verify installation:
   ```cmd 
      java -version
   ```

### Environment variables _(if needed)_

If java isn’t recognized, add it to PATH manually:

   #### For Windows:

   1. Open __System Properties → Environment Variables.__
   2. Under __System variables__, edit `Path`.
   3. Add the `bin` folder path, e.g.:
      ```cmd 
         C:\Program Files\Microsoft\jdk-17\bin
      ``` 
   4. Click OK and restart your terminal.

       #### Verify the installation
       If unsure, you can confirm the actual path:
       ```cmd 
          where java
       ```
       It will output something like:
       ```
          C:\Program Files\Microsoft\jdk-17\bin\java.exe
       ```
       Then the JDK home directory is:
       ```
          C:\Program Files\Microsoft\jdk-17
       ```

#### Set the `JAVA_HOME` Environment Variable

1. Press __Win + R__, type `sysdm.cpl`, and press __Enter__
2. Go to __Advanced → Environment Variables__
3. Under __System variables__, click __New__
   + __Variable name__: `JAVA_HOME`
   + __Variable value__: (your JDK path, for example)
      ```
      C:\Program Files\Microsoft\jdk-17
      ```
4. Click __OK__
5. Still in “System variables,” find __Path → Edit__
6. Add a new line:
   ```
      %JAVA_HOME%\bin
   ```
7. Save → OK → OK → Close

### Allure Installation

Install any Allure version. Follow the official instructions: 👉 https://allurereport.org/docs/install-for-windows/

## How to use RanorexReport

* For report generation needed folder with the following results from Ranorex:
   + *.rxlog.data
   + images/videos folder with artefacts for failed tests
   + *.pdf  (optional)
   + *.zip with full Ranorex report (optional)

Open command line and run the following CLI command

```cmd
   dotnet run -- create "RanorexReportsPath" "AllureFolderPath" --buildId <buildId> --runName "<runName>" --zipName "<zipName>.zip"
```
   + ___RanorexReportsPath__ - put here path to Ranorex logs. __*.rxlog.data__ files are used to create alure-results_
   + ___AllureFolderPath__ - path to folder where allure-results folder will be created_
   + ___buildId__ - build id which is used in executor.json file_
   + ___runName__ - run name which is used in executor.json file_
   + ___zipName__ - *.zip file name with extention (it will be attached to test cases)_

### How RanorexReport works

* __Generate allure results__. All allure results are generated and placed into __*allure-results*__ folder.
   + __*.rxlog.data__ files are parsed into objects;
   + __*-result.json__, __*-attachment.[html/jpg/pdf/png/txt]__, __*-container.json__ allure artifacts are created
   + Creates __environment.json__ file - this might be updated in accordance with your requirements
   + __executor.json__ - file is copied to allure-results folder, executor_template.json might be updated with your refs
   + __categories.json__ - must be updated in solution with your regular expressions in solution

