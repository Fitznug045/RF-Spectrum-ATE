# Description
This project is an Automated Test Equipment (ATE) application designed to automate measurements, monitoring, and testing for Spectrum Analyzer devices, specifically for **Siglent SSA3021X Plus** & **Siglent SHA851A**.<br/> 

It is built using **C# Windows Forms** and includes custom UI components such as real-time graph visualization, custom notifications, Excel integration, and Crystal Reports for automated report generation.<br/>

It uses **SCPI (Standard Commands for Programmable Instruments)** over TCP/IP to communicate with the analyzer, retrieve trace data, and perform automated frequency testing with PASS/FAIL evaluation.
# Installation
### Requirements
- Windows 10/11
- .NET Desktop Runtime 
- Visual Studio 2022 (for option 2)

## Option 1 : Install prebuilt application
- Download from [releases](https://github.com/Fitznug045/RF-Spectrum-ATE/releases/tag/v1.0.0) and unpack the zip file
- Open Release folder and run SpectrumAnalyzerGUI.exe

## Option 2 : Build from source code
- Clone this repository
```
git clone https://github.com/Fitznug045/RF-Spectrum-ATE.git
cd RF-Spectrum-ATE
```
- Open solution file in Visual Studio 2022
- Install required NuGet packages:<br/>
  1. EPPlus (Excel reading)
  2. OpenTK
  3. System.Data.DataSetExtensions
- Set configuration from **Debug** to **Release**
- Ensure Crystal Reports Runtime is installed
- Build the project :
```
Build -> Build Solution
```
- Executible will be located in :
```
/bin/Release/
```

# User Guide
### 1. Connecting to Spectrum Analyzer
- Open the application and turn on Spectrum Analyzer
- Ensure it is connected to the same network as your PC
- Get the analyzer’s IP address and type it into **Resource Address** field
- Click **Connect**<br/>

If the connection is successfull :
- Device name will be shown
- Notification will appear
- Measurement controls will be enabled

### 2. Starting Measurement
- After connecting, click **Start** to measure
- The graph will begin updating in real time
- The following values will update automatically :<br/>
  - Start Frequency
  - Stop Frequency
  - Peak Frequency
  - Peak Amplitude
- To stop measurement, click **Stop**

### 3. Adjusting Frequency Settings
You can manually control the analyzer's center frequency and span.<br/>
To apply, you must :
1. Stop the measurement
2. Enter values
3. click **Apply**

### 4. Importing Test List
This allows automatic PASS/FAIL testing using a frequency list from Excel.<br/>
**Excel Format**<br/>
Your Excel must have this format:
| No | Freq (MHz) | Limit (dBm) |
| -- | ---------- | ----------- |
| 1  | 100        | -30         |
| 2  | 150        | -28         |

To import :
1. Click **Import**
2. Select .xlsx file
3. The excel data will be inserted in the table

### 5. Running Automated Test (Batch Test)
Once the Excel list is imported:<br/>
1. Ensure the analyzer is connected
2. Click **Run Test**

For each frequency :<br/>
- The analyzer is tuned
- A trace is acquired
- The nearest frequency sample is evaluated
- PASS/FAIL is computed
- Table row is colored :
  - Green = PASS
  - Red = FAIL
You will receive notification when the test is completed.

### 6. Saving Results & Creating Reports
After test execution, click **Save**.

A PDF-style report is generated through Crystal Reports, containing :<br/>
- Device Name
- Test Date & Time
- PASS/FAIL Summary
- Pass Percentage
- Full test table

In the report viewer you can :
- Export to PDF
- Print
- Save to file

### 7. Clearing the Table
Click **Clear** to remove all rows from the test table.<br/>
This does not affect graph or connection status.

### 8. Disconnecting
To safely disconnected:
- Click **Disconnect**
- Active measurement loops will stop
- UI returns to idle state 
