# Crestron Dsan Limitimer - Configuration Guide

Comprehensive timer control and program management for Dsan Limitimer devices. Integrates with Crestron Essentials for LED feedback, time adjustment, program selection, and communication monitoring.

### ⚠️ IMPORTANT: Device Configuration Requirements

The configuration properties must accurately reflect the communication parameters and timeout values required for proper device communication. Incorrect timeout values may result in communication errors or device initialization failures. All timeouts must be set appropriately for your network environment and device responsiveness requirements.

---
<!-- START Minimum Essentials Framework Versions -->
### Minimum Essentials Framework Versions

- 3.0.0-dev-v3-routing.63
<!-- END Minimum Essentials Framework Versions -->

<!-- START Supported Types -->
### Supported Types

- limitimer
<!-- END Supported Types -->

---

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

---

### Device Characteristics

| Feature | Support | Notes |
|---------|---------|-------|
| Communication | RS-232, TCP | Primary: RS-232 at 9600 baud |
| LED Indicators | 7 total | Program 1-3, Session, Beep, Blink, Seconds |
| Time Display | 3 zones | Total, Sum-Up, Remaining (MM:SS format; includes stop-at-zero variant) |
| Programs | 4 | Program 1-3 + Session mode |
| Controls | 6 | Start/Stop, Repeat, Clear, +/-, Seconds |
| Feedback Status | Online, Communication Health | Real-time monitoring |

---

<!-- START Core Properties -->
### Core Properties

| Property | Type | Required | Default | Description |
|----------|------|----------|---------|-------------|
| `key` | string | ✓ | - | Unique device identifier |
| `uid` | integer | ✓ | - | Essentials system UID (must be unique) |
| `name` | string | ✓ | - | Device display name |
| `type` | string | ✓ | - | Device type (always "limitimer") |
| `group` | string | ✓ | - | Device grouping (e.g., "timers") |
| `control` | object | ✓ | - | Communication control configuration |
| `pollTimeMs` | long | ✗ | 5000 | Status poll interval in milliseconds |
| `warningTimeoutMs` | long | ✗ | 45000 | Communication warning threshold in ms |
| `errorTimeoutMs` | long | ✗ | 90000 | Communication error threshold in ms |

<!-- END Core Properties -->

---

### Property Details

**Core Configuration:**

- **`key`:** Unique device identifier within the system. Used for device reference in routing and control logic. Example: `"limitimer-main"`, `"limitimer-backup"`.

- **`uid`:** Essentials system UID. Must be unique across all devices. Critical for device communication and feedback routing. Range: 1-65535.

- **`name`:** Device name displayed in the control system UI and used internally. Should be descriptive (e.g., `"MainTimer"`, `"ConferenceRoomTimer"`).

- **`type`:** Device type identifier. Must be set to `"limitimer"` for all Limitimer devices.

- **`group`:** Logical grouping category for device organization. Recommended: `"timers"` or `"presentation"`.

**Communication Configuration:**

- **`control`:** Communication control object. Contains either:
  - **COM (Serial):** `comParams` with baudRate (9600), dataBits (8), parity (None), stopBits (1), protocol (RS232)
  - **TCP (Network):** `tcpSshProperties` with IP address and port (5000)

- **`control.method`:** Communication method:
  - `"com"` for RS-232 serial connection
  - `"tcp"` for TCP/Network connection

- **`control.controlPortNumber`:** (COM only) Control processor's serial port number (typically 1-3 for CP4).

- **`control.comParams.baudRate`:** Serial communication speed. Must be 9600 for Limitimer devices (fixed by device firmware).

- **`control.comParams.dataBits`:** Serial data bits. Must be 8 for Limitimer.

- **`control.comParams.stopBits`:** Serial stop bits. Must be 1 for Limitimer.

- **`control.comParams.parity`:** Serial parity. Must be "None" for Limitimer.

- **`control.tcpSshProperties.address`:** (TCP only) Device IP address. Must be static for reliable connectivity.

- **`control.tcpSshProperties.port`:** (TCP only) Device port. Standard Limitimer TCP port is 5000.

**Monitoring & Timeout:**

- **`pollTimeMs`:** How often (in ms) the system queries device status. 
  - **Recommended:** 5000 ms (5 seconds) for normal operation
  - **Range:** 1000-60000 ms
  - **Lower values:** More responsive but higher network traffic
  - **Higher values:** Less traffic but slower status updates
  - **Multi-zone:** Use 3000 ms for high-traffic environments

- **`warningTimeoutMs`:** Time (in ms) before communication warning is triggered. 
  - **Recommended:** 45000 ms (45 seconds) for standard networks
  - **Calculation:** Should be at least 3x pollTimeMs + network latency
  - **Purpose:** Early warning of communication degradation
  - **Example:** 5000 ms polling × 3 + 30 ms latency = ~15 s minimum (45 s recommended)

- **`errorTimeoutMs`:** Time (in ms) before communication error is triggered. 
  - **Recommended:** 90000 ms (90 seconds) for standard networks
  - **Calculation:** Should be at least 2x warningTimeoutMs
  - **Purpose:** Device marked offline and removed from control
  - **Example:** 45000 ms warning × 2 = 90000 ms error threshold
  - **Note:** Should be significantly larger than warningTimeoutMs to prevent false errors

---
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
| 5 | R | Remaining Time String Stop at Zero (MM:SS format) |
<!-- END Join Maps -->

---

### Join Details

**Digital Joins - Status & Feedback:**

- **1 (Is Online):** Device online status feedback. True = online/responding, False = offline/no communication.

**Digital Joins - Program Control:**

- **11-12 (Program 1):** Program 1 selection feedback and LED states (on/dim). Bidirectional press control.
- **13-14 (Program 2):** Program 2 selection feedback and LED states (on/dim). Bidirectional press control.
- **15-16 (Program 3):** Program 3 selection feedback and LED states (on/dim). Bidirectional press control.
- **17-18 (Session):** Session mode selection feedback and LED states (on/dim). Bidirectional press control.

**Digital Joins - Control Functions:**

- **21 (Beep):** Beep button press and LED feedback. Toggles beep function on/off.
- **22 (Blink):** Blink button press and LED feedback. Toggles blink function on/off.
- **23 (Seconds Mode):** Seconds mode toggle and indicator feedback. Changes time display format.
- **27 (Start/Stop):** Timer start/stop control (write-only). High pulse starts timer, low pulse stops timer.
- **28 (Repeat):** Repeat function control (write-only). Re-runs current program.
- **29 (Clear):** Clear timer control (write-only). Resets all timer values.

**Digital Joins - Time Adjustment:**

- **30-31 (Total Time):** Increment/decrement total time. Joins 30 (plus) and 31 (minus) adjust total time value.
- **32-33 (Sum Time):** Increment/decrement sum-up time. Joins 32 (plus) and 33 (minus) adjust sum time value.

**Digital Joins - LED Status:**

- **24 (Green LED):** Green LED state feedback (read-only). Indicates normal/ready status.
- **25 (Red LED):** Red LED state feedback (read-only). Indicates warning/error status.
- **26 (Yellow LED):** Yellow LED state feedback (read-only). Indicates caution/attention needed.

**Analog Joins - State Tracking:**

- **1 (Socket Status):** Communication socket status (read-only). Values: 0=Ok, 1=CompromisedCommunication (warning), 2=CommunicationError (offline).
- **2-5 (LED States):** LED state values (read-only). Values: 0=off, 1=on, 2=dim. One join per program LED.

**Serial Joins - Data Feedback:**

- **1 (Device Name):** Device name string feedback (read-only). Returns configured device name.
- **2 (Total Time String):** Total elapsed time in MM:SS format (read-only). Format: "HH:MM:SS".
- **3 (Sum-Up Time String):** Sum-up time in MM:SS format (read-only). Format: "HH:MM:SS".
- **4 (Remaining Time String):** Remaining time in MM:SS format (read-only). Format: "HH:MM:SS".

---
<!-- START Interfaces Implemented -->
### Interfaces Implemented

- IOnline
- ICommunicationMonitor
<!-- END Interfaces Implemented -->

---

<!-- START Base Classes -->
### Base Classes

- MessengerBase
- JoinMapBaseAdvanced
- EssentialsBridgeableDevice
<!-- END Base Classes -->

---

<!-- START Routing Framework -->
### Routing Framework & Architecture

The Dsan Limitimer plugin follows the PepperDash Essentials architecture pattern:

**Device Lifecycle:**
- Inherits from `EssentialsBridgeableDevice` for core device management
- Implements `IOnline` interface for online/offline status tracking
- Implements `ICommunicationMonitor` for communication health monitoring

**Communication Architecture:**
- Uses `IBasicCommunication` for command/response handling
- Employs `GenericCommunicationMonitor` for connection state tracking
- Supports both RS-232 and TCP communication protocols

**Bridge Integration:**
- Integrates with Essentials Bridge framework via `EiscApiAdvanced`
- Join mapping handled through `LimitimerBridgeJoinMap`
- Bidirectional join support (read/write) for real-time synchronization
- Messenger-based communication pattern for decoupled device control

**Data Flow:**
1. Configuration loaded from JSON via `LimitimerPropertiesConfig`
2. Device initializes communication channel (RS-232 or TCP)
3. Join map establishes bridge connections
4. Commands sent through public methods
5. Device responses processed through feedback handlers
6. Status updates propagated via Bool/Int/String feedbacks

**Poll Architecture:**
- Configurable polling interval (`pollTimeMs`, default 5000ms)
- Warning timeout for degraded communication (`warningTimeoutMs`, default 45000ms)
- Error timeout for communication failure (`errorTimeoutMs`, default 90000ms)

<!-- END Routing Framework -->

---

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
- public void Factory_Source_Sets_MinimumEssentialsFrameworkVersion()
- public void Factory_Source_Sets_TypeNames()
- public void Factory_Source_Contains_TypeName(string factoryClassName, string typeName)
- public void Assembly_Loads_Successfully()
- public void Assembly_Name_Matches_Expected()
- public void Factory_Count_Matches_Expected()
- public void Factory_Exists_ByName(string factoryClassName)
- public void All_Factories_Have_Parameterless_Constructor()
- public void Config_Class_Exists()
- public void Config_Has_Parameterless_Constructor()
- public void Config_Property_Has_JsonPropertyAttribute(string propertyName, string jsonName)
<!-- END Public Methods -->
---

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

---

<!-- START Int Feedbacks -->
### Int Feedbacks

- StatusFeedback
- Program1LedStateFeedback
- Program2LedStateFeedback
- Program3LedStateFeedback
- SessionLedStateFeedback
<!-- END Int Feedbacks -->

---

<!-- START String Feedbacks -->
### String Feedbacks

- TotalTimeFeedback
- SumUpTimeFeedback
- RemainingTimeFeedback
- RemainingTimeSZFeedback
<!-- END String Feedbacks -->

---

<!-- START Configuration Best Practices -->
### Configuration Best Practices

**Communication Setup:**
- Verify Limitimer device is powered and initialized before deployment
- For RS-232: Test COM port connectivity using terminal utility before final config
- For RS-232: Ensure baud rate is set to 9600 on device (device firmware fixed)
- For TCP: Verify device IP address is static and documented
- For TCP: Verify firewall rules allow port 5000 access from control processor
- Test ping/connectivity from CP4 to device before deployment

**Timeout Configuration:**
- **Standard Network (5000ms polling):** 
  - `warningTimeoutMs`: 45000 (45 seconds)
  - `errorTimeoutMs`: 90000 (90 seconds)
- **High-Traffic Environments (3000ms polling):**
  - `warningTimeoutMs`: 30000 (30 seconds)
  - `errorTimeoutMs`: 60000 (60 seconds)
- **Low-Latency Requirements (2000ms polling):**
  - `warningTimeoutMs`: 15000 (15 seconds)
  - `errorTimeoutMs`: 30000 (30 seconds)
- Always ensure: `errorTimeoutMs` ≥ 2 × `warningTimeoutMs`
- Never set `pollTimeMs` lower than 1000 ms (1 second)

**Device Management:**
- Use unique, descriptive device keys (e.g., "limitimer-main", "limitimer-backup")
- Assign UIDs sequentially for easy tracking (101, 102, 103, etc.)
- Group devices logically using the `group` property (e.g., "timers", "presentation", "conference")
- Document device location and purpose for maintenance
- Maintain IP address documentation for TCP configurations

**Monitoring & Debugging:**
- Enable communication monitoring to track device status
- Check `StatusFeedback` (Analog Join 1) for real-time connection health
- Monitor `IsOnline` feedback for device availability
- Review logs if devices frequently enter warning or error state
- Verify timeout values if communication issues occur
- Use Serial Join 1 (Device Name) to confirm device identity
- Monitor Serial Joins 2-4 for time display accuracy

**Production Deployment:**
- Test configuration in staging environment first
- Verify all program buttons (1-3, Session) trigger expected actions
- Confirm LED feedback states (green, red, yellow) update correctly
- Test time adjustment (plus/minus) for both total and sum time
- Verify start/stop, repeat, and clear functions work reliably
- Confirm communication status feedback updates appropriately
- Perform full system test with network load before go-live
- Document all configuration values (UIDs, IPIDs, IP addresses)

**Troubleshooting:**
- If device goes offline: Check network connectivity, firewall rules, and timeout settings
- If feedback delays: Lower polling interval (3000ms or less) or increase timeouts
- If LED states inconsistent: Verify correct join numbers are mapped in control system
- If time strings malformed: Ensure device is in MM:SS format mode (check Seconds Mode indicator)
- If programs don't trigger: Verify bidirectional joins (11, 13, 15, 17) are mapped correctly
- If communication warnings: Increase `warningTimeoutMs` gradually until stable
<!-- END Configuration Best Practices -->

---

**Generated:** January 27, 2026  
**Framework Version:** PepperDash Essentials 2.8.0  
**Plugin Version:** 1.0.0-local  
**Execution:** COPILOT_README_PROMPTS.md - Source-First Extraction with NVX Reference Standard Enforcement
