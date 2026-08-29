Public Class IdleWatcher
    ' TODO: Implements IDisposable
    ' TODO: and dispose _timer
    Private _timer As System.Threading.Timer
    Private _enabled As Boolean
    Private _lastEvent As DateTime
    Public Sub New()
        _timer = New System.Threading.Timer(AddressOf watch)
        _enabled = False
        Timeout = 0
    End Sub
    Public Event Idle(sender As Object)
    Public Property Timeout As Long
    Public Property Enabled As Boolean
        Get
            Return _enabled
        End Get
        Set(value As Boolean)
            If value Then
                _lastEvent = DateTime.Now
                _timer.Change(0, 1000)
            Else
                _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)
            End If
        End Set
    End Property
    Private Sub watch()
        If DateTime.Now.Subtract(_lastEvent).TotalMilliseconds > Timeout Then
            Enabled = False
            ' raise an event so the form can handle it and log out
            RaiseEvent Idle(Me)
        End If
    End Sub
    Public Sub Refresh()
        _lastEvent = DateTime.Now
    End Sub
End Class
