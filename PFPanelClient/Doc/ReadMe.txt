Pro Flight Panel SCJoyServer Client V 1.2 - Build 7 BETA
(c) Cassini - 26-Mar-2020

Contains 6 files:

PFPanelClient.exe		     The program (V1.2)
PFSP_HID.dll                 HID Support DLL		- MUST be in the same folder as the Exe file
HidLibrary.dll               HID Access Library     - MUST be in the same folder as the Exe file

ReadMe.txt                   This file

PFPanelClient (.Net 4.7.2)

Put all files into one folder and hit PFPanelClient.exe to run it
This program expects a running SCJoyServer to receive UDP commands
see: https://github.com/SCToolsfactory/SCJoyServer

For Updates and information visit:
https://github.com/SCToolsfactory/ProFlightDeckSupport

Scanned for viruses before packing... 
cassini@burri-web.org

Changelog:
V 1.2-B7
- add Macro capability for commands - see reference
- changed Config file syntax for Key - VKcode (int), VKcodeEx(string) - see reference
V 1.0 initial 
- Derived from ProFlightPanelSupport, to integrate with SCJoyServer
