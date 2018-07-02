
Public Class UsuarioParametros
    Public caratular As String
    Public exportar_datos As String
    Public habilitar_origen As String
    Public organigrama_default As New DependenciaOrganigrama
    Public organigramas As New List(Of DependenciaOrganigrama)
    'Public id_organigrama As Integer
    'Public id_organigrama_otros As String
    'Public organigrama_descripcion As String
    'Public organigrama_descripcion_otros As String
    Public prefijo As String
End Class