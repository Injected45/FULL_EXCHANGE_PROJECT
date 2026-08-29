Public Class CoBranchGene
	Private _Code As String
	Public Property Code() As Integer
		Get
			Return _Code
		End Get
		Set(ByVal value As Integer)
			_Code = value
		End Set
	End Property

	Private _BName As String
	Public Property BName() As String
		Get
			Return _BName
		End Get
		Set(ByVal value As String)
			_BName = value
		End Set
	End Property

	Private _Country As String
	Public Property Country() As String
		Get
			Return _Country
		End Get
		Set(ByVal value As String)
			_Country = value
		End Set
	End Property

	Private _BAddress As String
	Public Property BAddress() As String
		Get
			Return _BAddress
		End Get
		Set(ByVal value As String)
			_BAddress = value
		End Set
	End Property

	Private _Mobile1 As String
	Public Property Mobile1() As String
		Get
			Return _Mobile1
		End Get
		Set(ByVal value As String)
			_Mobile1 = value
		End Set
	End Property
	Private _Mobile2 As String
	Public Property Mobile2() As String
		Get
			Return _Mobile2
		End Get
		Set(ByVal value As String)
			_Mobile2 = value
		End Set
	End Property
	Private _Notes As String
	Public Property Notes() As String
		Get
			Return _Notes
		End Get
		Set(ByVal value As String)
			_Notes = value
		End Set
	End Property
	Private _IsActive As Boolean
    Public Property IsActive() As Boolean
        Get
            Return _IsActive
        End Get
        Set(ByVal value As Boolean)
            _IsActive = value
        End Set
    End Property
End Class
