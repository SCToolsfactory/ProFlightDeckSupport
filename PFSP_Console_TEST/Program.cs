using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PFSP_HID;

namespace PFSP_Console_TEST
{
  /// <summary>
  /// Checks to see if a Griffin Pro Flight Switch Panel device is connected. If it is, 
  /// read rotation and button press events and control the LED brightness
  /// and pulse speed.
  /// 
  /// This code has not been tested with multiple Pro Flight Switch Panels connected at the same time.
  /// </summary>
  class Program
  {
    static PFSwPanelManager pfspManager = new PFSwPanelManager( );


    static void Main( string[] args )
    {
      if ( pfspManager.OpenDevice( ) ) {
        pfspManager.DeviceAttached += new EventHandler( pfspManager_DeviceAttached );
        pfspManager.DeviceRemoved += new EventHandler( pfspManager_DeviceRemoved );
        pfspManager.ButtonUp += PfspManager_ButtonUp;
        pfspManager.ButtonDown += PfspManager_ButtonDown;
        pfspManager.RotaryChanged += PfspManager_RotaryChanged;

        Console.WriteLine( "Pro Flight Switch Panel found, press any key to exit." );
        Console.ReadKey( );
        pfspManager.CloseDevice( );
      }
      else {
        Console.WriteLine( "Could not find a Pro Flight Switch Panel." );
        Console.ReadKey( );
      }
    }

    private static void PfspManager_ButtonUp( object sender, PFSwPanelSwitchEventArgs e )
    {
      Console.WriteLine( $"Pro Flight Switch Panel Switch Up event for: {e.Switch.ToString( )}" );
      string l1 = "  ";
      string l2 = "  ";
      foreach ( PFSwPanelSwitches s in Consts.ESwitchesValues ) {
        l1 += $"{s.ToString( ).PadRight( 15 )}|";
        l2 += $"{e.State.SwitchState( s ).ToString( ).PadRight( 15 )}|";
        if ( s == PFSwPanelSwitches.GEAR ) {
          Console.WriteLine( l1 );
          Console.WriteLine( l2 );
          l1 = $"  ";
          l2 = $"  ";
        }
      }
      Console.WriteLine( l1 );
      Console.WriteLine( l2 );
      pfspManager.SetLed( PFSwPanelLeds.GEAR_L, PFSwPanelLedState.Led_Green );
      pfspManager.SetLed( PFSwPanelLeds.GEAR_R, PFSwPanelLedState.Led_Red );
    }


    private static void PfspManager_ButtonDown( object sender, PFSwPanelSwitchEventArgs e )
    {
      Console.WriteLine( $"Pro Flight Switch Panel Switch Down event for: {e.Switch.ToString( )}" );
      string l1 = "  ";
      string l2 = "  ";
      foreach ( PFSwPanelSwitches s in Consts.ESwitchesValues ) {
        l1 += $"{s.ToString( ).PadRight( 15 )}|";
        l2 += $"{e.State.SwitchState( s ).ToString( ).PadRight( 15 )}|";
        if ( s == PFSwPanelSwitches.GEAR ) {
          Console.WriteLine( l1 );
          Console.WriteLine( l2 );
          l1 = $"  ";
          l2 = $"  ";
        }
      }
      Console.WriteLine( l1 );
      Console.WriteLine( l2 );
      pfspManager.SetLed( PFSwPanelLeds.GEAR_R, PFSwPanelLedState.Led_Green );
      pfspManager.SetLed( PFSwPanelLeds.GEAR_L, PFSwPanelLedState.Led_Red );
    }

    private static void PfspManager_RotaryChanged( object sender, PFSwPanelRotaryEventArgs e )
    {
      Console.WriteLine( $"Pro Flight Switch Panel Rotary Changed event" );
      Console.WriteLine( $"  Rotary State: {e.State.RotaryState( ).ToString( )}" );
      pfspManager.SetLed( PFSwPanelLeds.GEAR_N, (PFSwPanelLedState)((int)e.State.RotaryState()%4) );
    }

    static void pfspManager_DeviceRemoved( object sender, EventArgs e )
    {
      Console.WriteLine( "Pro Flight Switch Panel removed." );
    }

    static void pfspManager_DeviceAttached( object sender, EventArgs e )
    {
      Console.WriteLine( "Pro Flight Switch Panel attached." );
    }


  }
}
