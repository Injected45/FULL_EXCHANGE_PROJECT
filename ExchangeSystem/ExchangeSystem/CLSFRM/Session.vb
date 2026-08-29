Imports System.Data.SqlClient

Namespace ExchangeSystem.CLSFRM
    Public Class Session
        Private Shared _user As DAL.TB_User

        Public Shared ReadOnly Property User As DAL.TB_User
            Get
                Return _user
            End Get
        End Property

        Public Shared Sub SetUser(ByVal user As DAL.TB_User)
            _user = user
            Dim PR(0) As SqlParameter
            Using db As DAL.DataClasses1DataContext = New DAL.DataClasses1DataContext()
                _screensAccesses = (From s In [CLSFRM].Screens.GetScreens From d In db.UserAccessProfileDetails.Where(Function(x) x.ProfileID = user.USettingProfileID And x.ScreenID = s.ScreenID).DefaultIfEmpty() Select New [CLSFRM].ScreensAccessProfile(s.ScreenName) With {
                    .CanAdd = If((d Is Nothing), False, d.CanAdd),
                    .CanDelete = If((d Is Nothing), False, d.CanDelete),
                    .CanEdit = If((d Is Nothing), False, d.CanEdit),
                    .CanOpen = If((d Is Nothing), False, d.CanOpen),
                    .CanPrint = If((d Is Nothing), False, d.CanPrint),
                    .CanShow = If((d Is Nothing), False, d.CanShow),
                    .Actions = s.Actions,
                    .ScreenName = s.ScreenName,
                    .ScreenCaption = s.ScreenCaption,
                    .ScreenID = s.ScreenID,
                    .ParentScreenID = s.ParentScreenID
                }).ToList()
            End Using
        End Sub
        Private Shared _screensAccesses As List(Of ScreensAccessProfile)

        Public Shared ReadOnly Property ScreensAccesses As List(Of ScreensAccessProfile)
            Get
                Return _screensAccesses
            End Get
        End Property
    End Class
End Namespace
