# RanorexReportToAllure

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

Install Allure from archive. Follow the official instructions: 👉 https://allurereport.org/docs/install-for-windows/

## How to use RanorexReportToAllure

Open command line and run the following command

```cmd
   RanorexReport.exe -RanorexReportsPath "pathToRanorexArtifacts" -AllureFolderPath "pathToFolderToCreateAllureReports"
```
+ ___RanorexReportsPath__ - put here path to Ranorex logs. __*.rxlog.data__ files are used to create Allure results_
+ ___AllureFolderPath__ - path to folder where allure-results and allure-report will be created_

### How RanorexReportToAllure works

1. __Generate allure results__. All Allure results are generated and placed into __*allure-results*__ folder.
   + __*.rxlog.data__ files are parsed into objects;
   + *-result.json, *-attachment.[html/jpg/pdf/png/txt], *-container.json Allure artifacts are created
2. __Copy history folder__ into __allure-results__ folder, if exists (path to __*allure-report*__ folder must be provided)
3. __Build Allure report__. 
   + following Powershell command is running
   ``` pwsh
      allure generate "allureResultsPath" --clean -o "allureReportPath"
   ```
   + Create __environments.json__ file

