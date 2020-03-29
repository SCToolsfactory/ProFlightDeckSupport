# Pro Flight Deck Support
Support for HID devices of the Pro Flight Panel series  
  Currently supported: Pro Flight Switch Panel

### General note to builders
The Project files expect referenced Libraries which have no NuGet package reference in a Solution directory  ´ExtLibraries´  
Those external libraries can built from the project SC_Toolbox:  
https://github.com/SCToolsfactory/SC_Toolbox

NOTE: THIS _ IS _ VERY _ EARLY _ WORK _ IN _ PROGRESS _ IT _ MAY _ JUST _ BREAK _ AT _ ANY _ TIME ;-)  

# PFPanelClient V1.2 an SCJoyServer Client
Intends to connect to an SCJoyServer  

_
Connects the Pro Flight Switch Panel and sends configurable commands to the SCJoyServer.  
The Configuration File contains the commands to be sent with each Switch or the Rotary Knob (look for Doc/Examples folder of this project for specification and examples)  
SCJoyServer can integrate multiple clients sending joystick and keyboard commands.  
It then forwards such activation to the local vJoy (virtual Joystick) device and/or keyboard.   
Clients can be anything from this Panel to Tablets, RaspBerry PI etc.  
_
see: https://github.com/SCToolsfactory/SCJoyServer  

In order to use the program one has to add the library DLLs   

Just within the application Exe folder:  
*PFPanelClient.exe            The program  
*vjMapper.dll                 Command Mapping Library  
*PFSP_HID.dll                 HID Support Wrapper 
*  HidLibrary.dll             HID Access Library 
*  Theraot.Core.dll           HID Access Support Library 

# ProFlightPanelSupport V 1.2 the standalone program
Provides a small standalone program to enable the Pro Flight Switch Panel acting as vJoy and/or Keyboard.  
Actuates ONE vJoy virtual Joystick and supplies keystrokes to the active window 
 
 
 A config file defines what command are sent with the Switches and the Rotary.

For documentation of the Config File see the Doc Folder  
https://github.com/SCToolsfactory/ProFlightDeckSupport/tree/master/ProFlightPanelSupport/Doc  
For a sample Config File see the Example Folder  
https://github.com/SCToolsfactory/ProFlightDeckSupport/tree/master/ProFlightPanelSupport/Examples 
 
Note: 
You have to install the vJoy (V 2.1) driver.  
The driver is not available here but use this link.  
http://vjoystick.sourceforge.net/site/index.php/download-a-install/download   

You may map the Virtual Joystick like any real one into SC by using e.g. SCJMapper-V2

In order to use the program one has to add the driver/library DLLs 

Just within the application Exe folder:  
*ProFlightPanelSupport.exe    The program
*vjMapper.dll                 Command Mapping Library
*dxKbdInterfaceWrap.dll       application keyboard typing support
*  x64\SCdxKeyboard.dll       (64bit version)
*  x86\SCdxKeyboard.dll       (32bit version)
*vJoy_csWrapper.dll           vJoy Access Library
*  x64\vJoyInterface.dll      (from vJoy218SDK-291116 - 64bit)
*  x86\vJoyInterface.dll      (from vJoy218SDK-291116 - 32bit)
*PFSP_HID.dll                 HID Support Wrapper 
*  HidLibrary.dll             HID Access Library 
*  Theraot.Core.dll           HID Access Support Library 


# Credits
The vJoy wrapper contains code copy from the vJoy Wrapper for CS.  
 MIT License  
 Copyright( c) 2017 Shaul Eizikovich  
-----------------------------------------------------------------------------------  
HidLibrary.dll  via NuGet from:  
https://github.com/mikeobrien/HidLibrary    
The PFSP_HID.DLL contains some code copy and adoptions from:  
https://github.com/mikeobrien/HidLibrary/tree/master/examples/GriffinPowerMate/PowerMate   
 MIT License  
 Copyright(c) Mike O'Brien and contributors see GitHub Page  
-----------------------------------------------------------------------------------  
ProFlightPanel HID Reference    
 Credit: https://github.com/saitek-xplane/saitek-plugin-xplane   

