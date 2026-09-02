# =============================================================
#  Emulator setup - re-applied on every launch, idempotent.
# =============================================================
#  Called by run_app_emulator.bat once the device is booted.
#  Everything here is cheap and safe to repeat; the point is that a
#  fresh emulator, a wiped AVD or a machine restart can never again
#  cost an hour of re-configuring by hand.
#
#  Three things get restored:
#
#   1. Android's ARABIC PHYSICAL KEYBOARD LAYOUT.
#      The emulator forwards scancodes, so Gboard's language is
#      irrelevant to hardware keys: Android maps them through the
#      layout set PER INPUT DEVICE. Default is English (US), which
#      yields Latin letters no matter what the on-screen keyboard says.
#      This normally persists in /data/system/input-manager-state.xml;
#      the check below is for when it does not (a wiped AVD).
#
#   2. The WINDOWS INPUT LANGUAGE OF THE EMULATOR WINDOW.
#      This is the one that actually bites, and it looks like Android
#      losing its settings when it is not. If Windows is on Arabic, the
#      host sends an Arabic CHARACTER, which the emulator cannot map to
#      an Android keycode - so NOTHING types, letters or digits.
#      Windows supports a per-window input language, so the emulator
#      window is switched to English while every other window on the
#      machine stays Arabic. Nothing is taken away from the user.
#
#   3. ANIMATION SCALES set to 0 - the emulator renders in software
#      (no Vulkan on this GPU), so system transitions are expensive.
#
#  Note the login screens no longer depend on any of this: phone_screen
#  and otp_screen draw their own NumericKeypad and had no TextField at
#  all, so no keyboard setting could ever have reached them. That is
#  fixed in the app itself - see the HardwareDigits mixin in
#  lib/ui/widgets/controls.dart.
# =============================================================

param(
    [string]$Adb = "$env:LOCALAPPDATA\Android\Sdk\platform-tools\adb.exe",
    # en-GB. The pair installed on this machine is ar-LY + en-GB.
    [string]$EnglishLayout = '00000809'
)

$ErrorActionPreference = 'Continue'

function Say($msg) { Write-Host "  [setup] $msg" }

if (-not (Test-Path $Adb)) {
    Say "adb not found at $Adb - skipping."
    exit 0
}

# ---------------------------------------------------------------
#  1) Animation scales
# ---------------------------------------------------------------
foreach ($k in 'window_animation_scale', 'transition_animation_scale', 'animator_duration_scale') {
    & $Adb shell settings put global $k 0 2>$null | Out-Null
}
Say 'animations off'

# ---------------------------------------------------------------
#  2) Arabic physical keyboard layout
# ---------------------------------------------------------------
#  Two input devices exist and the layout is enabled PER DEVICE:
#  "AT Translated Set 2 keyboard" (vendor:1,product:1) and "qwerty2"
#  (vendor:1575,product:1). Setting only one leaves half the keys
#  English, which is confusing enough to look like a different bug.
$overlays = (& $Adb shell dumpsys input 2>$null | Select-String -SimpleMatch 'HaveKeyboardLayoutOverlay: true').Count

if ($overlays -ge 2) {
    Say "arabic layout ok ($overlays devices)"
} else {
    Say "arabic layout MISSING (only $overlays of 2) - repairing"

    # The layout picker is a UI-only setting on API 30, so the repair
    # writes the state file directly and restarts the framework. Needs
    # root, which google_apis images allow (a Play Store image does not).
    & $Adb root 2>$null | Out-Null
    Start-Sleep -Seconds 3
    & $Adb wait-for-device 2>$null | Out-Null

    $layout = 'com.android.inputdevices/com.android.inputdevices.InputDeviceReceiver/keyboard_layout_arabic'
    $xml = @"
<?xml version='1.0' encoding='utf-8' standalone='yes' ?>
<input-manager-state>
    <input-devices>
        <input-device descriptor="vendor:1,product:1">
            <keyboard-layout descriptor="$layout" current="true" />
        </input-device>
        <input-device descriptor="vendor:1575,product:1">
            <keyboard-layout descriptor="$layout" current="true" />
        </input-device>
    </input-devices>
</input-manager-state>
"@
    $tmp = Join-Path $env:TEMP 'input-manager-state.xml'
    # LF endings and no BOM: the parser on the device is not Windows.
    [IO.File]::WriteAllText($tmp, ($xml -replace "`r`n", "`n"), (New-Object Text.UTF8Encoding $false))

    & $Adb push $tmp /data/system/input-manager-state.xml 2>$null | Out-Null
    & $Adb shell chown system:system /data/system/input-manager-state.xml 2>$null | Out-Null
    & $Adb shell chmod 600 /data/system/input-manager-state.xml 2>$null | Out-Null
    Remove-Item $tmp -Force -ErrorAction SilentlyContinue

    # InputManagerService reads this at startup only.
    Say 'restarting android framework (about a minute)'
    & $Adb shell stop 2>$null | Out-Null
    & $Adb shell start 2>$null | Out-Null

    for ($i = 0; $i -lt 60; $i++) {
        Start-Sleep -Seconds 3
        if ((& $Adb shell getprop sys.boot_completed 2>$null) -match '1') { break }
    }
    & $Adb unroot 2>$null | Out-Null
    Start-Sleep -Seconds 3
    & $Adb wait-for-device 2>$null | Out-Null

    $overlays = (& $Adb shell dumpsys input 2>$null | Select-String -SimpleMatch 'HaveKeyboardLayoutOverlay: true').Count
    if ($overlays -ge 2) { Say 'arabic layout repaired' }
    else { Say "arabic layout STILL missing ($overlays of 2) - set it by hand: Settings > System > Languages and input > Physical keyboard" }
}

# ---------------------------------------------------------------
#  3) English input language for the emulator window only
# ---------------------------------------------------------------
Add-Type @"
using System;
using System.Text;
using System.Runtime.InteropServices;
public class EmuWin {
  [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
  [DllImport("user32.dll")] public static extern IntPtr GetKeyboardLayout(uint thread);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] public static extern IntPtr LoadKeyboardLayout(string id, uint flags);
  [DllImport("user32.dll")] public static extern IntPtr PostMessage(IntPtr h, uint msg, IntPtr wp, IntPtr lp);
  [DllImport("user32.dll")] public static extern bool EnumWindows(EnumProc cb, IntPtr p);
  [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr h);
  public delegate bool EnumProc(IntPtr h, IntPtr p);
}
"@ -ErrorAction SilentlyContinue

# Match on the OWNING PROCESS, not the window title: a terminal running
# this project is titled "... Android emulator" too and would be caught
# by a title match, silently switching the shell the user types Arabic in.
$emuPids = (Get-Process -Name 'qemu-system-x86_64', 'emulator' -ErrorAction SilentlyContinue).Id
if (-not $emuPids) {
    Say 'emulator window not found - windows input language left alone'
    exit 0
}

$targets = @()
$cb = [EmuWin+EnumProc] {
    param($h, $p)
    if ([EmuWin]::IsWindowVisible($h)) {
        $owner = 0
        $tid = [EmuWin]::GetWindowThreadProcessId($h, [ref]$owner)
        if ($emuPids -contains $owner) { $script:targets += [PSCustomObject]@{ Hwnd = $h; Tid = $tid } }
    }
    return $true
}
[void][EmuWin]::EnumWindows($cb, [IntPtr]::Zero)

if ($targets.Count -eq 0) { Say 'emulator has no visible window yet'; exit 0 }

# flags 0, not KLF_ACTIVATE: load the layout without activating it for
# this shell. Only the emulator window should change.
$hkl = [EmuWin]::LoadKeyboardLayout($EnglishLayout, 0)
$WM_INPUTLANGCHANGEREQUEST = 0x0050
foreach ($t in $targets) {
    [void][EmuWin]::PostMessage($t.Hwnd, $WM_INPUTLANGCHANGEREQUEST, [IntPtr]1, $hkl)
}
Start-Sleep -Milliseconds 700

$now = [EmuWin]::GetKeyboardLayout($targets[0].Tid)
if ($now -eq $hkl) {
    Say 'emulator window switched to English (the rest of Windows stays Arabic)'
} else {
    Say ("emulator window layout is 0x{0:X8}, expected 0x{1:X8} - type with Windows on English" -f $now.ToInt64(), $hkl.ToInt64())
}
