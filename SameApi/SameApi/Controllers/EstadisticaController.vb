Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http

Namespace Controllers
  <Cors.EnableCors("http://localhost:4200", "*", "*")>
  Public Class EstadisticaController
    Inherits ApiController

    ' GET: api/Estadistica
    'Public Function GetValues() 'As IEnumerable(Of String)

    '  'Return New String() {"value1", "value2"}
    'End Function

    '' GET: api/Estadistica/5
    Public Function GetValue(ByVal id As Integer) 'As String
      Dim response As HttpResponseMessage
      Dim ls_sql As String
      Dim lo_DataTable As New DataTable
      Dim lo_DataRow As DataRow
      Dim lo_estadistica As Estadistica
      Dim lo_error As Mensaje
      Dim lo_contexto As System.Web.HttpContext
      Dim lo_headers As Headers.HttpRequestHeaders

      Try
        lo_headers = Me.Request.Headers

        '-----------------------------------------------------------------------------------------------
        ' Valido que exista el usuario y la contraseña sea correcta 
        '-----------------------------------------------------------------------------------------------

        Using sqlConn As New SqlConnection()
          ';User Id=sa;Password=ml
          lo_estadistica = New Estadistica()

          sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAME").ConnectionString & ";User Id=sa;Password=ml"
          sqlConn.Open()

          Dim sqlComm As SqlCommand = New SqlCommand
          sqlComm.Connection = sqlConn
          sqlComm.CommandText = "Select  Count(*) as Q FROM ml.SA_V_Ultimo_Pase_por_Expediente WHERE ID_Destino = " & id
          Dim sqlDataAdapt As SqlDataAdapter = New SqlDataAdapter(sqlComm)
          lo_DataTable = New DataTable
          sqlDataAdapt.Fill(lo_DataTable)
          lo_DataRow = lo_DataTable.Rows(0)
          lo_estadistica.expedientes_en_oficina = lo_DataRow(0)


          '-- Pases Emitidos sin recepcion de la oficina destino --
          sqlComm = New SqlCommand
          sqlComm.Connection = sqlConn
          sqlComm.CommandText = "Select Count(*) as Q FROM  ml.SA_V_Ultimo_Pase_por_Expediente WHERE  ID_Origen = " & id & "  And   Fecha_Recepcion Is NULL "
          sqlDataAdapt = New SqlDataAdapter(sqlComm)
          lo_DataTable = New DataTable
          sqlDataAdapt.Fill(lo_DataTable)
          lo_DataRow = lo_DataTable.Rows(0)
          lo_estadistica.exp_sin_rec_en_otra_ofi = lo_DataRow(0)

          '-- Pases Emitidos a MI oficina y aun no los recepcione --
          sqlComm = New SqlCommand
          sqlComm.Connection = sqlConn
          sqlComm.CommandText = "Select Count(*) as Q FROM  ml.SA_V_Ultimo_Pase_por_Expediente WHERE ( ID_Destino = " & id & " ) And  ( Fecha_Recepcion Is NULL )    "
          sqlDataAdapt = New SqlDataAdapter(sqlComm)
          lo_DataTable = New DataTable
          sqlDataAdapt.Fill(lo_DataTable)
          lo_DataRow = lo_DataTable.Rows(0)
          lo_estadistica.exp_sin_rec_en_mi_ofi = lo_DataRow(0)


          '-- Expedientes que tengo YO solicitados por otra oficina --
          sqlComm = New SqlCommand
          sqlComm.Connection = sqlConn
          sqlComm.CommandText = "Select Count(*) as Q  FROM ml.sa_registro_solicitudes_exptes WHERE rse_id_org_actual = " & id
          sqlDataAdapt = New SqlDataAdapter(sqlComm)
          lo_DataTable = New DataTable
          sqlDataAdapt.Fill(lo_DataTable)
          lo_DataRow = lo_DataTable.Rows(0)
          lo_estadistica.exp_sol_por_otra_ofi = lo_DataRow(0)


          sqlDataAdapt.Dispose() : sqlComm.Dispose() : sqlConn.Close() : sqlConn.Dispose()

          response = Request.CreateResponse(HttpStatusCode.OK)
          response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_estadistica), Encoding.UTF8, "application/json")
          Return response 'Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(ds), Encoding.UTF8, "application/json")
        End Using

      Catch ex As Exception

        'Return Nothing
        lo_error = New Mensaje
        lo_error.codigo = "-1"
        lo_error.mensaje = ex.Message
        response = Request.CreateResponse(HttpStatusCode.Unauthorized)
        response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
        Return response

      End Try
    End Function

    '' POST: api/Estadistica
    'Public Sub PostValue(<FromBody()> ByVal value As String)

    'End Sub

    '' PUT: api/Estadistica/5
    'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    'End Sub

    '' DELETE: api/Estadistica/5
    'Public Sub DeleteValue(ByVal id As Integer)

    'End Sub
  End Class
End Namespace
