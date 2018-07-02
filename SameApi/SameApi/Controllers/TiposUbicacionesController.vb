Imports System.Net
Imports System.Web.Http
Imports System.Net.Http
Imports System.Data.SqlClient

Namespace Controllers
    <Cors.EnableCors("http://localhost:4200", "*", "*")>
    Public Class TiposUbicacionesController
        Inherits ApiController

        ' GET: api/TiposUbicaciones
        Public Function GetValues() 'As IEnumerable(Of String)
            Dim response As HttpResponseMessage

            Dim ls_sql As String
            Dim lo_headers As Headers.HttpRequestHeaders
            Dim lo_DataTable As New DataTable
            Dim lo_DataAdapt As SqlDataAdapter
            Dim lo_error As Mensaje
            Dim lo_usuario As Login


            Try
                lo_headers = Me.Request.Headers
                lo_usuario = gfun_autorizacion(lo_headers.GetValues("token").First())

                If lo_usuario Is Nothing Then
                    lo_error = New Mensaje
                    lo_error.codigo = "-3"
                    lo_error.mensaje = "No está autorizado a urilizar este servicio"
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                    response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                    Return response
                End If

                Using sqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("SAME").ConnectionString)
                    sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAME").ConnectionString & ";User Id=sa;Password=ml"
                    sqlConn.Open()
                    Dim sqlComm As SqlCommand = New SqlCommand
                    sqlComm.Connection = sqlConn


                    ls_sql = " SELECT tiu_codigo as ubicacion from ml.SA_Tipos_Ubicacion"
                    ls_sql &= " Order by tiu_codigo desc"

                    sqlComm.CommandText = ls_sql

                    lo_DataAdapt = New SqlDataAdapter(sqlComm)
                    Dim ds As DataSet = New DataSet
                    lo_DataAdapt.Fill(ds)

                    lo_DataTable.Dispose() : lo_DataAdapt.Dispose() : sqlComm.Dispose() : sqlConn.Close() : sqlConn.Dispose()
                    'ds.DataSetName = "TrafoData"
                    'ds.Tables(0).TableName = "Trafo"
                    response = Request.CreateResponse(HttpStatusCode.OK)
                    response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(ds), Encoding.UTF8, "application/json")
                    Return response
                End Using

            Catch ex As Exception
                lo_error = New Mensaje
                lo_error.codigo = "-1"
                lo_error.mensaje = ex.Message
                response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                Return response
            End Try


        End Function

        '' GET: api/TiposUbicaciones/5
        'Public Function GetValue(ByVal id As Integer) As String
        '    Return "value"
        'End Function

        '' POST: api/TiposUbicaciones
        'Public Sub PostValue(<FromBody()> ByVal value As String)

        'End Sub

        '' PUT: api/TiposUbicaciones/5
        'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        'End Sub

        '' DELETE: api/TiposUbicaciones/5
        'Public Sub DeleteValue(ByVal id As Integer)

        'End Sub
    End Class
End Namespace