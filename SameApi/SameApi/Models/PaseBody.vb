Public Class PaseBody
    Public numero As Integer
    Public organigrama As DependenciaOrganigrama = New DependenciaOrganigrama
    Public expedientes As List(Of PaseExpediente) = New List(Of PaseExpediente)
End Class
