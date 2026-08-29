

Public Class ResizeControls

    Dim RatioTable As New Hashtable

    Private WindowHeight As Single
    Private WindowWidth As Single
    Private HeightRatio As Single
    Private WidthRatio As Single

    Private _Container As New Control

    Private Shared m_FormWidth As Long                          'Original form width.
    Private Shared m_FormHeight As Long

    Public Property Container() As Control
        Get
            Return _Container
        End Get
        Set(ByVal Ctrl As Control)
            _Container = Ctrl
            FullRatioTable()
        End Set

    End Property

    Private Structure SizeRatio
        Dim TopRatio As Single
        Dim LeftRatio As Single
        Dim HeightRatio As Single
        Dim WidthRatio As Single
    End Structure

    Private Sub FullRatioTable()
        WindowHeight = _Container.Height
        WindowWidth = _Container.Width
        RatioTable = New Hashtable
        AddChildrenToTable(_Container)

    End Sub

    Private Sub AddChildrenToTable(ByRef ChildContainer As Control)
        Dim R As New SizeRatio

        For Each C As Control In ChildContainer.Controls
            With C
                R.TopRatio = CSng(.Top / WindowHeight)
                R.LeftRatio = CSng(.Left / WindowWidth)
                R.HeightRatio = CSng(.Height / WindowHeight)
                R.WidthRatio = CSng(.Width / WindowWidth)
                RatioTable(.Name) = R
                If .HasChildren Then
                    AddChildrenToTable(C)
                End If
            End With
        Next

    End Sub

    Public Sub ResizeControls()


        HeightRatio = CSng(_Container.Height / WindowHeight)
        WidthRatio = CSng(_Container.Width / WindowWidth)

        WindowHeight = _Container.Height
        WindowWidth = _Container.Width

        ResizeChildren(_Container)



    End Sub

    Private Sub ResizeChildren(ByRef ChildContainer As Control)

        Dim R As New SizeRatio
        For Each C As Control In ChildContainer.Controls
            With C
                R = CType(RatioTable(.Name), SizeRatio)
                .Top = CInt(WindowHeight * R.TopRatio)
                .Left = CInt(WindowWidth * R.LeftRatio)
                .Height = CInt(WindowHeight * R.HeightRatio)
                .Width = CInt(WindowWidth * R.WidthRatio)

                If .HasChildren Then
                    ResizeChildren(C)

                End If

            End With


            Select Case True
                Case TypeOf C Is ListBox
                    Dim L As New ListBox
                    L = CType(C, ListBox)
                    L.IntegralHeight = False
            End Select

            ResizeControlFont(C, WidthRatio, HeightRatio)

        Next





    End Sub

    Public Shared Sub SubResize(ByVal F As Form, ByVal percentW As Single, ByVal percentH As Single)

        Dim FormHeight As Long
        Dim FormWidth As Long
        Dim HeightChange As Single, WidthChange As Single


        FormHeight = Int((Screen.PrimaryScreen.WorkingArea.Height) * (percentH / 100))
        FormWidth = Int((Screen.PrimaryScreen.WorkingArea.Width) * (percentW / 100))

        With F
            .Height = FormHeight
            .Width = FormWidth

            HeightChange = .ClientSize.Height / m_FormHeight
            WidthChange = .ClientSize.Width / m_FormWidth


        End With


    End Sub



    Private Sub ResizeControlFont(ByRef Ctrl As Control, ByVal RatioW As Single, ByVal RatioH As Single)

        Dim FSize As Single = Ctrl.Font.Size
        Dim FStyle As FontStyle = Ctrl.Font.Style
        Dim FNome As String = Ctrl.Font.Name
        Dim NewSize As Single = FSize

        NewSize = CSng(FSize * Math.Sqrt(RatioW * RatioH))
        Dim NFont As New Font(FNome, CSng(NewSize), FStyle)
        Ctrl.Font = NFont

    End Sub

End Class

