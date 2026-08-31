<#
.SYNOPSIS
    Adds a host to the debug cleartext allow-list.

.DESCRIPTION
    Android blocks cleartext HTTP unless the host is named in the network
    security config. Only the DEBUG source set carries a config that permits
    it at all, so this touches that file and nothing else - a release apk
    talking http:// cannot be made to work this way, and should not be: the
    fix there is TLS on the server, not an exception that would put balances
    and transfer codes on the wire in the clear.

    Called by build_apk.bat before a debug build. Lives in its own file
    rather than inline in the .bat because the escaping needed to embed this
    in a batch line is unreadable and easy to get subtly wrong.

    The file carries Arabic comments, so it is read and written as UTF-8
    without a BOM. Writing it back through the shell's default encoding
    would mangle them.

.PARAMETER Config
    Path to network_security_config.xml.

.PARAMETER HostName
    The host to allow, e.g. 192.168.1.10. Not named -Host: that collides
    with PowerShell's automatic $Host variable.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string]$Config,
    [Parameter(Mandatory = $true)][string]$HostName
)

$ErrorActionPreference = 'Stop'

try {
    if (-not (Test-Path -LiteralPath $Config)) {
        Write-Error "network security config not found: $Config"
        exit 1
    }

    $xml = Get-Content -LiteralPath $Config -Raw -Encoding UTF8

    # Match the element's text exactly, so 192.168.1.1 is not taken as a
    # match for 192.168.1.10.
    if ($xml -match [regex]::Escape('>' + $HostName + '<')) {
        Write-Output "Cleartext allow-list: $HostName already listed."
        exit 0
    }

    $entry = '        <domain includeSubdomains="false">' + $HostName + '</domain>'
    $updated = $xml -replace '(?m)^\s*</domain-config>', ($entry + "`r`n    </domain-config>")

    if ($updated -eq $xml) {
        Write-Error "could not find </domain-config> in $Config - left unchanged."
        exit 1
    }

    # Fail before writing rather than leaving a file Android cannot parse.
    try {
        [xml]$updated | Out-Null
    } catch {
        Write-Error "the edit would produce invalid XML - left unchanged. $($_.Exception.Message)"
        exit 1
    }

    [IO.File]::WriteAllText(
        (Resolve-Path -LiteralPath $Config).Path,
        $updated,
        (New-Object Text.UTF8Encoding($false))
    )
    Write-Output "Cleartext allow-list: added $HostName."
    exit 0
} catch {
    Write-Error $_.Exception.Message
    exit 1
}
