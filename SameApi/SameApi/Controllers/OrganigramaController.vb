Imports System.Net
Imports System.Web.Http
Imports System.Net.Http
Imports System.Data.SqlClient

Namespace Controllers
    <Cors.EnableCors("http://localhost:4200", "*", "*")>
    Public Class OrganigramaController
        Inherits ApiController

        ' GET: api/Organigrama
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


                    ls_sql = "  SELECT ORG_Id_Organigrama , ORG_Enlace_P_H , ORG_Nivel ,ORG_Descripcion"
                    ls_sql &= " ,ORG_Codigo,ORG_Senial_de_Expansion,ORG_Titulo_de_Cargo,ORG_Legajo "
                    ls_sql &= " ,ORG_uid,ORG_est_trabajo, ORG_Fecha_Alta,ORG_Fecha_Baja ,ORG_Usar_CallCenter"
                    ls_sql &= " ,per_legajo_responsable, P.per_apellido + ' ' + P.per_nombre as Responsable, ORG_resp_f_desde, org_bloqueado"
                    ls_sql &= " ,org_archivo, org_externo, org_origen_escanea,org_destino_escanea"
                    ls_sql &= " FROM ml.ORGANIGRAMA O "
                    ls_sql &= " inner join ml.PE_Personal P ON (O.per_legajo_responsable = P.per_legajo) "
                    ls_sql &= " where org_bloqueado = 'N'"

                    ls_sql &= " order by [ORG_Nivel] ,[ORG_Enlace_P_H],[ORG_Id_Organigrama]"
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

        '' GET: api/Organigrama/5
        'Public Function GetValue(ByVal id As Integer) As String
        '    Return "value"
        'End Function

        '' POST: api/Organigrama
        'Public Sub PostValue(<FromBody()> ByVal value As String)

        'End Sub

        '' PUT: api/Organigrama/5
        'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        'End Sub

        '' DELETE: api/Organigrama/5
        'Public Sub DeleteValue(ByVal id As Integer)

        'End Sub
    End Class
End Namespace