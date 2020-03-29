Pro Flight Panel SCJoyServer Client V 1.3 - Build 8 BETA
(c) Cassini - 29-Mar-2020

Contains 6 files:

PFPanelClient.exe		     The program (V1.3)

- All libraries below MUST be in the same folder as the Exe file
vjMapper.dll                 Command Mapping Library
PFSP_HID.dll                 HID Support Wrapper 
  HidLibrary.dll             HID Access Library 
  Theraot.Core.dll           HID Access Support Library 

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
V 1.3-B8
- add Macro capability for commands - see reference
- changed Config file syntax for Key - VKcode (int), VKcodeEx(string) - see reference
- refact using SC_Toolbox libraries and some more improvements
V 1.0 initial 
- Derived from ProFlightPanelSupport, to integrate with SCJoyServer
