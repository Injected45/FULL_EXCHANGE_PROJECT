<#
.SYNOPSIS
    Prints this machine's LAN IPv4 address, or nothing.

.DESCRIPTION
    Used by build_apk.bat to bake the right backend address into a debug
    apk, and it is a separate file for a reason: as a PowerShell one-liner
    inside a batch for-loop it needs "^|" for every pipe and breaks outright
    inside an if-block, where cmd ends the block at the first unescaped ")".
    That failure is silent - the address just comes back empty.

    Skips loopback (127.*), APIPA (169.254.*, what Windows assigns when DHCP
    fails - it reaches nothing), and WellKnown origins, so what is left is a
    real DHCP or manually configured address a phone on the same Wi-Fi can
    reach. Virtual adapters (Hyper-V, WSL, VirtualBox) are excluded too: they
    are real addresses that no phone can route to, and picking one produces
    an apk that installs and then cannot reach the backend.

    Prints nothing and exits 1 when there is no usable address, so the
    caller can fail loudly instead of building an apk pointed at nowhere.
#>
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

try {
    $candidates = Get-NetIPAddress -AddressFamily IPv4 |
        Where-Object {
            $_.IPAddress -notlike '127.*' -and
            $_.IPAddress -notlike '169.254.*' -and
            $_.PrefixOrigin -ne 'WellKnown'
        }

    # Prefer a physically connected adapter; a phone cannot route to a
    # host-only virtual switch even though its address looks ordinary.
    $preferred = $candidates | Where-Object {
        $alias = $_.InterfaceAlias
        $alias -notmatch 'vEthernet|Hyper-V|WSL|VirtualBox|VMware|Loopback'
    }

    $pick = @($preferred)[0]
    if ($null -eq $pick) { $pick = @($candidates)[0] }

    if ($null -eq $pick) { exit 1 }

    Write-Output $pick.IPAddress
    exit 0
} catch {
    exit 1
}
