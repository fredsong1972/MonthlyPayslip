# MonthlyPayslip

When call MonthlyPayslip with two arguments which are employee name and annual salary, the program should geneate monthly pay slip.

Here is example console input:
MonthlyPayslip "Mary Song" 60000


## Assumptions

We assume the gross monthly income, monthly income tax, and net monthly income are round to dollar.

## Solution Deatils
This soultion is built by Visual Studio 2019 with .NetCore 3.1.

## Application Settings
In application settings, configured the tax rates file which is CSV format.

## How to run
Buid the solution with Visual Studio First. You can run it from Visual Studio or command line. 
For command line, please go to the bin folder, PayslipApp\bin\[Debug | Release]\netcoreapp3.1.

This is a cross-platform application. Run it from command line as the below

donet MonthlyPayslip.dll

For Windows platform, you can run MonthlyPayslip.exe.

## Input
This solution has a default taxrate.csv. You can replace it with your own csv file in the data folder of the runing directory.
Ensure the file name is the same, otherwise need change appsettings.json.

## Ouput
After run this application, the monthly payslip will be print to console.

Here is exmaple output:

Monthly Payslip for: "Mary Song"
Gross Monthly Income: $5000
Monthly Income Tax: $500
Net Monthly Income: $4500

## Change log
