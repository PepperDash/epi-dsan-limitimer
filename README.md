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
