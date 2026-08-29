Module ModuleAnalyze
    Public Sub GetInternalExToConfirm()
        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_TXT("InternalEx_LOADTOCONFIRM")
        'If DT.Rows.Count > 0 Then
        '    FRMMAIN.Timer2.Start()
        '    'FRMMAIN.ConfirmInternalEx.Text = DT.Rows.Count
        '    FRMMESSAGENEWTRANSFER.LBL1.Text = "لديك حوالة داخلية مطلوب اعتمادها وعددها:" & vbNewLine & DT.Rows.Count
        '    My.Computer.Audio.Play(System.IO.Path.GetFullPath(Application.StartupPath & "\Message Tone.wav"))
        'End If
        'FRMMAIN.Timer2.Stop()
    End Sub
    Public Sub GetInternalExToConfirmCancel()

        'Dim DT As New DataTable
        'DT.Clear()
        'DT = RUN_QUARY_TXT("InternalEx_LOADTOCONFIRMCANCEL")
        'If DT.Rows.Count > 0 Then
        '    FRMMAIN.ConfirmInternalExCancel.Text = DT.Rows.Count
        '    FRMMAIN.Timer2.Start()
        '    'FRMMESSAGENEWTRANSFER.LBL1.Text = "لديك حوالة داخلية مطلوب اعتمادها وعددها:" & vbNewLine & DT.Rows.Count
        '    My.Computer.Audio.Play(System.IO.Path.GetFullPath(Application.StartupPath & "\Message Tone.wav"))
        'End If
        'FRMMAIN.Timer2.Stop()
    End Sub
End Module
