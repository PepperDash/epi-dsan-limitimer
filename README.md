![PepperDash Essentials Pluign Logo](/images/essentials-plugin-blue.png)

# Essentials Plugin Template (c) 2023

## License

Provided under MIT license

## Overview

Fork this repo when creating a new plugin for Essentials. For more information about plugins, refer to the Essentials Wiki [Plugins](https://github.com/PepperDash/Essentials/wiki/Plugins) article.

This repo contains example classes for the three main categories of devices:
* `EssentialsPluginTemplateDevice`: Used for most third party devices which require communication over a streaming mechanism such as a Com port, TCP/SSh/UDP socket, CEC, etc
* `EssentialsPluginTemplateLogicDevice`:  Used for devices that contain logic, but don't require any communication with third parties outside the program
* `EssentialsPluginTemplateCrestronDevice`:  Used for devices that represent a piece of Crestron hardware

There are matching factory classes for each of the three categories of devices.  The `EssentialsPluginTemplateConfigObject` should be used as a template and modified for any of the categories of device.  Same goes for the `EssentialsPluginTemplateBridgeJoinMap`.

This also illustrates how a plugin can contain multiple devices.

## Cloning Instructions

After forking this repository into your own GitHub space, you can create a new repository using this one as the template.  Then you must install the necessary dependencies as indicated below.

## Dependencies

The [Essentials](https://github.com/PepperDash/Essentials) libraries are required. They referenced via nuget. You must have nuget.exe installed and in the `PATH` environment variable to use the following command. Nuget.exe is available at [nuget.org](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe).

### Installing Dependencies

To install dependencies once nuget.exe is installed, run the following command from the root directory of your repository:
`nuget install .\packages.config -OutputDirectory .\packages -excludeVersion`.
Alternatively, you can simply run the `GetPackages.bat` file.
To verify that the packages installed correctly, open the plugin solution in your repo and make sure that all references are found, then try and build it.

### Installing Different versions of PepperDash Core

If you need a different version of PepperDash Core, use the command `nuget install .\packages.config -OutputDirectory .\packages -excludeVersion -Version {versionToGet}`. Omitting the `-Version` option will pull the version indicated in the packages.config file.

### Instructions for Renaming Solution and Files

See the Task List in Visual Studio for a guide on how to start using the template.  There is extensive inline documentation and examples as well.

For renaming instructions in particular, see the XML `remarks` tags on class definitions

## Build Instructions (PepperDash Internal) 

## Generating Nuget Package 

In the solution folder is a file named "PDT.EssentialsPluginTemplate.nuspec" 

1. Rename the file to match your plugin solution name 
2. Edit the file to include your project specifics including
    1. <id>PepperDash.Essentials.Plugin.MakeModel</id> Convention is to use the prefix "PepperDash.Essentials.Plugin" and include the MakeModel of the device. 
    2. <projectUrl>https://github.com/PepperDash/EssentialsPluginTemplate</projectUrl> Change to your url to the project repo

There is no longer a requirement to adjust workflow files for nuget generation for private and public repositories.  This is now handled automatically in the workflow.

__If you do not make these changes to the nuspec file, the project will not generate a nuget package__
<!-- START Minimum Essentials Framework Versions -->
### Minimum Essentials Framework Versions

- 2.8.0
<!-- END Minimum Essentials Framework Versions -->
<!-- START Config Example -->
### Config Example

```json
{
    "key": "GeneratedKey",
    "uid": 1,
    "name": "GeneratedName",
    "type": "limitimer",
    "group": "Group",
    "properties": {
        "control": "SampleValue",
        "pollTimeMs": 0,
        "warningTimeoutMs": 0,
        "errorTimeoutMs": 0
    }
}
```
<!-- END Config Example -->
<!-- START Supported Types -->
### Supported Types

- limitimer
<!-- END Supported Types -->
<!-- START Join Maps -->
### Join Maps

#### Digitals

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | R | Is Online |
| 11 | R | Program 1 Press / Program 1 LED On Feedback |
| 12 | R | Program 1 LED Dim Feedback |
| 13 | R | Program 2 Press / Program 2 LED On Feedback |
| 14 | R | Program 2 LED Dim Feedback |
| 15 | R | Program 3 Press / Program 3 LED On Feedback |
| 16 | R | Program 3 LED Dim Feedback |
| 17 | R | Session Press / Session LED On Feedback |
| 18 | R | Session LED Dim Feedback |
| 21 | R | Beep Press / Beep LED On Feedback |
| 22 | R | Blink Press / Blink LED On Feedback |
| 23 | R | Seconds Mode Press / Seconds Mode Indicator Feedback |
| 24 | R | Green LED On Feedback |
| 25 | R | Red LED On Feedback |
| 26 | R | Yellow LED On Feedback |
| 27 | R | Start/Stop Press |
| 28 | R | Repeat Press |
| 29 | R | Clear Press |
| 30 | R | Total Time Plus Press |
| 31 | R | Total Time Minus Press |
| 32 | R | Sum Time Plus Press |
| 33 | R | Sum Time Minus Press |

#### Analogs

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | R | Socket Status (0=IsOk, 1=CompromisedCommunication, 2=CommunicationError) |
| 2 | R | Program 1 LED State (0=off, 1=on, 2=dim) |
| 3 | R | Program 2 LED State (0=off, 1=on, 2=dim) |
| 4 | R | Program 3 LED State (0=off, 1=on, 2=dim) |
| 5 | R | Session LED State (0=off, 1=on, 2=dim) |

#### Serials

| Join | Type (RW) | Description |
| --- | --- | --- |
| 1 | R | Device Name |
| 2 | R | Total Time String (MM:SS format) |
| 3 | R | Sum-Up Time String (MM:SS format) |
| 4 | R | Remaining Time String (MM:SS format) |
<!-- END Join Maps -->
<!-- START Interfaces Implemented -->
### Interfaces Implemented

- IOnline
- ICommunicationMonitor
- IBridgeAdvanced
<!-- END Interfaces Implemented -->
<!-- START Base Classes -->
### Base Classes

- EssentialsDevice
- JoinMapBaseAdvanced
- MessengerBase
<!-- END Base Classes -->
<!-- START Public Methods -->
### Public Methods

- public void ProcessFeedbackMessage(string message)
- public void SendText(string text)
- public void Program1()
- public void Program2()
- public void Program3()
- public void Session4()
- public void Beep()
- public void Beep1()
- public void Blink()
- public void StartStop()
- public void Repeat()
- public void Clear()
- public void TotalTimePlus()
- public void TotalTimeMinus()
- public void SumTimePlus()
- public void SumTimeMinus()
- public void SetSeconds()
- public void LinkToApi(BasicTriList trilist, uint joinStart, string joinMapKey, EiscApiAdvanced bridge)
<!-- END Public Methods -->
<!-- START Bool Feedbacks -->
### Bool Feedbacks

- IsOnline
- BeepLedStateFeedback
- BlinkLedStateFeedback
- GreenLedStateFeedback
- RedLedStateFeedback
- YellowLedStateFeedback
- SecondsModeIndicatorStateFeedback
<!-- END Bool Feedbacks -->
<!-- START Int Feedbacks -->
### Int Feedbacks

- StatusFeedback
- Program1LedStateFeedback
- Program2LedStateFeedback
- Program3LedStateFeedback
- SessionLedStateFeedback
<!-- END Int Feedbacks -->
<!-- START String Feedbacks -->
### String Feedbacks

- TotalTimeFeedback
- SumUpTimeFeedback
- RemainingTimeFeedback
<!-- END String Feedbacks -->
