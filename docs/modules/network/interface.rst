Interface selection
===================

Desomnia can be configured to monitor one or more network interfaces installed in your system. Upon service startup and everytime your network configuration changes, the configuration of all connected interfaces are compared for a match with one of your ``<NetworkMonitor>`` configurations. To determine which configuration should be used for which interface, there are several options:

Automatic selection
-------------------

.. code:: xml

    <NetworkMonitor>
        <!-- hosts, etc. -->
    </NetworkMonitor>

If you do not specify a value for either ``interface`` or ``network``, Desomnia will automatically monitor all interfaces configured with a standard gateway. This is usually the interface connected to your local network, and, unless you have multiple interfaces up at the same time, it is typically the only interface with this description. In that case, all interfaces will be monitored using the same configuration. This could be exactly what you want if you have wired and wireless links to the same network, at the same time.

By network
----------

.. code:: xml

    <NetworkMonitor network="192.168.178.0/24">
        <!-- hosts, etc. -->
    </NetworkMonitor>

When you specify a network in CIDR notation, the ``<NetworkMonitor>`` will only be configured for interfaces that have joined that particular network. This is determined by the IP address and netmask, or prefix length, of the interface itself, and applies to both IPv4 and IPv6 networks. Alternatively, you can use a single concrete IP address to configure it to bind to a very specific link only.

By interface name
-----------------

.. code:: xml

    <NetworkMonitor interface="eth0">
        <!-- hosts, etc. -->
    </NetworkMonitor>

When you specify an interface name, the ``<NetworkMonitor>`` will only be configured for interfaces with a matching name. The name will be compared for equality with the name of the interface or if the ID of the interface contains the specified string. There are differences, in how network interfaces are identified between the different operating systems:

Windows
+++++++

:OS: 🪟

On Windows every network interfaces is assigned a unique GUID and a human readable name. The name can actually be changed inside the system settings, but the GUID always stays unambiguously the same. You can query this information yourself, using PowerShell:

.. code:: PowerShell

    Get-NetAdapter | Select-Object Name, InterfaceDescription, InterfaceGuid

::

    Name                         InterfaceDescription                                      InterfaceGuid
    ----                         --------------------                                      -------------
    Bluetooth-Netzwerkverbindung Bluetooth Device (Personal Area Network)                  {6912B25F-0702-4ACC-AB22-82B3157A89FB}
    Netzwerkbrücke               Hyper-V Virtual Ethernet Adapter                          {5334C77C-2E13-4005-A7CE-C6889A312B5F}
    WLAN                         MediaTek Wi-Fi 6E MT7922 (RZ616) 160MHz Wireless LAN Card {44ECA482-24F2-4362-99F8-88A17A450B45}
    Ethernet                     Intel(R) Ethernet Controller (3) I225-V                   {1BD73899-523C-4911-967A-FE797ACF6C44}

So when you want to use the name of the interface, you have to specify the exact same string. If you want to use the ID instead, you can choose to include or omit the curly braces.

Linux and macOS
+++++++++++++++

:OS: 🐧 🍎

On Linux and macOS you always use the device or BSD name of the interface, which will usually be something like ``eth0`` or ``eth1``, depending on the number of installed interfaces in your system. Depending on the actual OS and distribution it can also be something like ``en12`` or ``wlan0``. Check ``ifconfig`` for a enumeration of all interfaces by their name.

By interface name and network
-----------------------------

.. code:: xml

    <NetworkMonitor interface="eth0" network="192.168.178.20">
        <!-- hosts, etc. -->
    </NetworkMonitor>

It is also allowed to combine both ways of identification to monitor the interface only in the most specific situation. If multiple configurations match the same interface, the first one will be used to initialize the ``<NetworkMonitor>``. Each interface will be monitored no more than once.

Hot plugging
------------

You can add, remove, connect or disconnect network interfaces at runtime without the need to restart Desomnia.