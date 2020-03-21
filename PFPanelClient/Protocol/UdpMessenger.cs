using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace PFPanelClient.Protocol
{
  /// <summary>
  /// Sending UDP messages over the line
  /// </summary>
  class UdpMessenger
  {

    #region Static Class
    /// <summary>
    /// Try to get the best IP address for this machine...
    /// ignores virtual and loopback adapters 
    /// </summary>
    /// <returns></returns>
    static public string GetLocalIP()
    {
      string localIP = "";

      foreach ( NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces( ) ) {
        if ( nic.OperationalStatus == OperationalStatus.Up ) {
          // must be up..
          if ( nic.Description.ToLowerInvariant( ).Contains( "virtual" ) ) continue; // not with virtual interfaces
          if ( nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ) continue;

          IPInterfaceProperties ipProps = nic.GetIPProperties( );
          foreach ( var ips in ipProps.UnicastAddresses ) {
            if ( ips.Address.AddressFamily.ToString( ) == "InterNetwork" ) {
              // that would be a IpV4 address..
              localIP = ips.Address.ToString( );
              return localIP;
            }
          }
        }
        // check if localAddr is in ipProps.UnicastAddresses
      }
      return localIP;
    }

    /// <summary>
    /// Checks for a valid IP and one that the server owns i.e. can be used to receive connections
    /// </summary>
    /// <param name="ipAddr">The IP address string</param>
    /// <returns>True if it can be used</returns>
    static public bool CheckIP( string ipAddr )
    {
      if ( !string.IsNullOrEmpty( ipAddr ) ) {
        if ( IPAddress.TryParse( ipAddr, out IPAddress lAddr ) ) {
          // seems to be a valid IP
          if ( Equals( lAddr, IPAddress.Loopback ) ) return true;
          // check if we own such an IP
          IPHostEntry host = Dns.GetHostEntry( Dns.GetHostName( ) );
          foreach ( IPAddress ip in host.AddressList ) {
            if ( Equals( lAddr, ip ) ) return true;
          }
        }
      }
      return false; // nope
    }

#endregion



    private string m_host = "";
    private IPAddress m_ipAddress = null;
    private int m_port = 0;
    private UdpClient m_udpClient = null;
    private IPEndPoint m_endpoint = null;

    public UdpMessenger( string remoteHost, int remotePort )
    {
      m_host = remoteHost;
      m_port = remotePort;

      if ( IPAddress.TryParse( remoteHost, out IPAddress lAddr ) ) {
        m_ipAddress = lAddr;
        m_endpoint = new IPEndPoint( m_ipAddress, m_port );
        m_udpClient = new UdpClient( );
      }
    }

    public bool SendMsg( string msg )
    {
      if ( m_endpoint == null ) return false;
      if ( m_udpClient == null ) return false;

      var ascii = new ASCIIEncoding( );

      byte[] buffer = ascii.GetBytes( msg );
      int bytes = buffer.Length;

      int howMuch = m_udpClient.Send( buffer, bytes, m_endpoint );
      if ( howMuch != bytes ) {
        // failed .. expect to send it in one call, no buffer manager here..
        return false;
      }

      return true;
    }

  }
}
