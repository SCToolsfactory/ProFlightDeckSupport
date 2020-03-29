Pro Flight Deck Support V 1.3 - Build 8 BETA
(c) Cassini - 29-Mar-2020

Contains 6 files:

ProFlightPanelSupport.exe    The program (V1.3)

- All libraries below MUST be in the same folder as the Exe file
vjMapper.dll                 Command Mapping Library
dxKbdInterfaceWrap.dll       application keyboard typing support
  x64\SCdxKeyboard.dll       (64bit version)
  x86\SCdxKeyboard.dll       (32bit version)
vJoy_csWrapper.dll           vJoy Access Library
  x64\vJoyInterface.dll      (from vJoy218SDK-291116 - 64bit)
  x86\vJoyInterface.dll      (from vJoy218SDK-291116 - 32bit)
PFSP_HID.dll                 HID Support Wrapper 
  HidLibrary.dll             HID Access Library 
  Theraot.Core.dll           HID Access Support Library 

ReadMe.txt                   This file

Pro Flight Deck Support (.Net 4.7.2)

Put all files into one folder and hit ProFlightPanelSupport.exe to run it

For Updates and information visit:
https://github.com/SCToolsfactory/ProFlightDeckSupport

Scanned for viruses before packing... 
cassini@burri-web.org

Changelog:
V 1.3-B8
- add Macro capability for commands - see reference
- changed Config file syntax for Key - VKcode (int), VKcodeEx(string) - see reference
- refact using SC_Toolbox libraries and some more improvements
V 1.1-B6
- refact. some items 
V 1.0 initial 
