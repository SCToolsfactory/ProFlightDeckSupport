# Pro Flight Deck Support<br>

Support for HID devices of the Pro Flight Panel series<br>
  Currently supported: Pro Flight Switch Panel<br>
<br>
NOTE: THIS _ IS _ VERY _ EARLY _ WORK _ IN _ PROGRESS _ IT _ MAY _ JUST _ BREAK _ AT _ ANY _ TIME ;-)<br>

# PFPaneClient V1.0 an SCJoyServer Client

Intends to connect to an SCJoyServer 
see: https://github.com/SCToolsfactory/SCJoyServer

Connects the Pro Flight Switch Panel and sends configurable commands to the SCJoyServer.<br> 
The Configuration File contains the commands to be sent with each Switch or the Rotary Knob (look for Doc/Examples folder of this project for specification and examples)
SCJoyServer can integrate multiple clients sending joystick and keyboard commands.<br>
It then forwards such activation to the local vJoy (virtual Joystick) device and/or keyboard. <br>
Clients can be anything from this Panel to Tablets, RaspBerry PI etc.<br>
See: https://github.com/SCToolsfactory/SCJoyServer <br>
<br>
In order to use the program one has to add the driver DLLs <br>
<br>
Just within the application Exe folder:<br>
PFPanelClient.exe            The program (V1.10<br>
PFSP_HID.dll                 HID Support DLL<br>
HidLibrary.dll               HID Access Library<br>
<br>
For convenience find them in the SupportingBinaries as zip<br>
<br>

# ProFlightPanelSupport V 1.1 the standalone program

Provides a small standalone program to enable the Pro Flight Switch Panel acting as vJoy and/or Keyboard<br>
Actuates ONE vJoy virtual Joystick and supplies keystrokes to the active window<br>
 <br>
 <br>
 <br>
 A config file defines what command are sent with the Switches and the Rotary.<br>
<br>
For documentation of the Config File see the Doc Folder<br>
https://github.com/SCToolsfactory/ProFlightDeckSupport/tree/master/ProFlightPanelSupport/Doc <br>
For a sample Config File see the Example Folder<br>
https://github.com/SCToolsfactory/ProFlightDeckSupport/tree/master/ProFlightPanelSupport/Examples <br>
 <br>
Note: <br>
You have to install the vJoy (V 2.1) driver.<br>
The driver is not available here but use this link.<br>
http://vjoystick.sourceforge.net/site/index.php/download-a-install/download    <br>
<br>
You may map the Virtual Joystick like any real one into SC by using e.g. SCJMapper-V2<br>
<br>
In order to use the program one has to add the driver DLLs <br>
<br>
Just within the application Exe folder:<br>
ProFlightPanelSupport.exe    The program (V1.1)<br>
PFSP_HID.dll                 HID Support DLL<br>
HidLibrary.dll               HID Access Library<br>
x64\SCdxKeyboard.dll   (64bit)<br>
x64\vJoyInterface.dll  (from the vJoy218SDK-291116 - 64bit)<br>
x86\SCdxKeyboard.dll   (32bit)<br>
x86\vJoyInterface.dll  (from the vJoy218SDK-291116 - 32bit)<br>
<br>
For convenience find them in the SupportingBinaries as zip<br>
<br>


# Credits

-----------------------------------------------------------------------------------<br>
The vJoy wrapper contains code copy from the vJoy Wrapper for CS.<br>
 MIT License<br>
 Copyright( c) 2017 Shaul Eizikovich<br>
-----------------------------------------------------------------------------------<br>
HidLibrary.dll binary copy via NuGet from:<br>
https://github.com/mikeobrien/HidLibrary  <br>
The PFSP_HID.DLL contains some code copy and adoptions from:<br>
https://github.com/mikeobrien/HidLibrary/tree/master/examples/GriffinPowerMate/PowerMate <br>
 MIT License<br>
 Copyright(c) Mike O'Brien and contributors see GitHub Page<br>
-----------------------------------------------------------------------------------<br>
ProFlightPanel HID Reference  <br>
 Credit: https://github.com/saitek-xplane/saitek-plugin-xplane <br>
<br>
<br>