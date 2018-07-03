Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Microsoft.Reporting.WebForms

Namespace Controllers
    <Cors.EnableCors("http://localhost:4200", "*", "*")>
    Public Class ExpedientesController
        Inherits ApiController

        <Cors.EnableCors("http://localhost:4200", "*", "*")>
        <HttpGet>
        Public Function Antiguos(ByVal id As Integer)
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

                    'ls_sql = "Select Tipo_Sistema, Codigo_Sistema, Nivel_Tension, Capa,Elemento, Usuario,Nivel,Insertar,Modificar,Eliminar "

                    ls_sql = "Select  prefijo, numero, anio, extension, fecha_ingreso, estracto, iniciador, id_organigrama, id_ultimo_pase, "
                    ls_sql &= "remito, fecha_pase, fecha_recepcion, codigo_origen, origen, codigo_destino, destino, dias_oficina,"
                    ls_sql &= "ml.SA_fun_Q_adjuntos(prefijo, numero, anio, extension) As Adjuntos,"
                    ls_sql &= "ml.SA_fun_Q_rtas(id_ultimo_pase) As Rtas, Texto "
                    ls_sql &= "From ml.sa_v_lugar_actual_expte  "
                    ls_sql &= "Where ml.sa_v_lugar_actual_expte.id_organigrama = " & id
                    ls_sql &= " Order by dias_oficina desc"
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

        <Cors.EnableCors("http://localhost:4200", "*", "*")>
        <HttpPost>
        Public Function Filtro(<FromBody()> ByVal aoExpediente As ExpedienteBody)
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

                    ls_sql = "SELECT	prefijo, numero, anio, extension, fecha_ingreso, estracto, iniciador, id_organigrama, id_ultimo_pase, "
                    ls_sql &= "remito, fecha_pase, fecha_recepcion, codigo_origen, origen, codigo_destino, destino, dias_oficina,"
                    ls_sql &= "ml.SA_fun_Q_adjuntos (prefijo, numero, anio, extension) as Adjuntos,"
                    ls_sql &= "ml.SA_fun_Q_rtas (id_ultimo_pase) as Rtas, Texto "
                    ls_sql &= "FROM ml.sa_v_lugar_actual_expte"
                    ls_sql &= " WHERE  prefijo LIKE '%" & IIf(Not aoExpediente.prefijo Is Nothing, aoExpediente.prefijo, "4069") & "%'"
                    If aoExpediente.organigrama.ToString <> "" And aoExpediente.organigrama > 0 Then
                        ls_sql &= " AND   id_organigrama =  " & aoExpediente.organigrama.ToString
                    End If
                    If Not aoExpediente.numero Is Nothing Then
                        ls_sql &= " AND   numero LIKE '%" & aoExpediente.numero.ToString & "%'"
                    End If
                    If aoExpediente.anio.ToString <> 0 Then
                        ls_sql &= " AND   anio_texto = '" & aoExpediente.anio & "'"
                    End If
                    If Not aoExpediente.extracto Is Nothing Then
                        ls_sql &= " AND   estracto LIKE '%" & aoExpediente.extracto.ToString & "%'"
                    End If
                    If Not aoExpediente.iniciador Is Nothing Then
                        ls_sql &= " AND   iniciador LIKE '%" & aoExpediente.iniciador.ToString & "%' "
                    End If

                    ls_sql &= " Order by prefijo, numero, anio, extension"
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

        <Cors.EnableCors("http://localhost:4200", "*", "*")>
<HttpPost>
        Public Function Expediente(<FromBody()> ByVal aoExpediente As ExpedienteBody)
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

                    ls_sql = "SELECT estracto ,iniciador,Calle,Altura,Localidad,CP,TE,Tipo_Doc,Nro_Doc,Mail"
                    ls_sql &= ",Fecha_Inicio as fecha_ingreso,Fecha_Pase,fecha_recepcion,Texto,Codigo_Organigrama as id_organigrama"
                    ls_sql &= ",Desc_Organigrama,Codigo_Tipo,Desc_Tipo,prefijo,Nro as numero,anio,extension,id_ultimo_pase"
                    ls_sql &= ",Fecha_Ultimo_Pase as fecha_pase,remito_Ultimo_Pase,Adjuntos,Respuestas"
                    ls_sql &= " FROM ml.SA_V_Expedientes "
                    ls_sql &= " WHERE  prefijo LIKE '%" & IIf(Not aoExpediente.prefijo Is Nothing, aoExpediente.prefijo, "4069") & "%'"
                    If aoExpediente.organigrama.ToString <> "" And aoExpediente.organigrama > 0 Then
                        ls_sql &= " AND   Codigo_Organigrama =  " & aoExpediente.organigrama.ToString
                    End If
                    If Not aoExpediente.numero Is Nothing Then
                        ls_sql &= " AND   nro = " & aoExpediente.numero.ToString & ""
                    End If
                    If aoExpediente.anio.ToString <> 0 Then
                        ls_sql &= " AND   anio = " & aoExpediente.anio
                    End If
                    If Not aoExpediente.extracto Is Nothing Then
                        ls_sql &= " AND   estracto LIKE '%" & aoExpediente.extracto.ToString & "%'"
                    End If
                    If Not aoExpediente.iniciador Is Nothing Then
                        ls_sql &= " AND   iniciador LIKE '%" & aoExpediente.iniciador.ToString & "%' "
                    End If

                    ls_sql &= " Order by prefijo, nro, anio, extension"
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



        <Cors.EnableCors("http://localhost:4200", "*", "*")>
<HttpPost>
        Public Function imprimirExpediente(<FromBody()> ByVal aoExpediente As ExpedienteBody)
            Dim response As HttpResponseMessage

            Dim ls_sql As String
            Dim lo_headers As Headers.HttpRequestHeaders
            Dim lo_DataTable As New DataTable
            Dim lo_DataAdapt As SqlDataAdapter
            Dim lo_error As Mensaje
            Dim lo_usuario As Login
            Dim localReport As New Microsoft.Reporting.WebForms.LocalReport

            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            Dim deviceInfo As String
            Dim bytes As Byte()


            localReport.ReportPath = HttpContext.Current.Server.MapPath("/Reportes/rptDetalleExpediente.rdlc")



            Try
                lo_headers = Me.Request.Headers
                lo_usuario = gfun_autorizacion(lo_headers.GetValues("token").First())

                If lo_usuario Is Nothing Then
                    lo_error = New Mensaje
                    lo_error.codigo = "-3"
                    lo_error.mensaje = "No está autorizado a urilizar este servicio"
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                    response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), System.Text.Encoding.UTF8, "application/json")
                    Return response
                End If

                Using sqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("SAME").ConnectionString)
                    sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAME").ConnectionString & ";User Id=sa;Password=ml"
                    sqlConn.Open()
                    Dim sqlComm As SqlCommand = New SqlCommand
                    sqlComm.Connection = sqlConn

                    ls_sql = "SELECT estracto ,iniciador,Calle,Altura,Localidad,CP,TE,Tipo_Doc,Nro_Doc,Mail"
                    ls_sql &= ",Fecha_Inicio,Fecha_Pase,fecha_recepcion,Texto,Codigo_Organigrama "
                    ls_sql &= ",Desc_Organigrama,Codigo_Tipo,Desc_Tipo,prefijo,Nro,anio,extension,id_ultimo_pase"
                    ls_sql &= ",Fecha_Ultimo_Pase,remito_Ultimo_Pase,Adjuntos,Respuestas"
                    ls_sql &= " FROM ml.SA_V_Expedientes "
                    ls_sql &= " WHERE  prefijo LIKE '%" & IIf(Not aoExpediente.prefijo Is Nothing, aoExpediente.prefijo, "4069") & "%'"
                    If aoExpediente.organigrama.ToString <> "" And aoExpediente.organigrama > 0 Then
                        ls_sql &= " AND   Codigo_Organigrama =  " & aoExpediente.organigrama.ToString
                    End If
                    If Not aoExpediente.numero Is Nothing Then
                        ls_sql &= " AND   nro = " & aoExpediente.numero.ToString & ""
                    End If
                    If aoExpediente.anio.ToString <> 0 Then
                        ls_sql &= " AND   anio = " & aoExpediente.anio
                    End If
                    If Not aoExpediente.extracto Is Nothing Then
                        ls_sql &= " AND   estracto LIKE '%" & aoExpediente.extracto.ToString & "%'"
                    End If
                    If Not aoExpediente.iniciador Is Nothing Then
                        ls_sql &= " AND   iniciador LIKE '%" & aoExpediente.iniciador.ToString & "%' "
                    End If

                    ls_sql &= " Order by prefijo, nro, anio, extension"
                    sqlComm.CommandText = ls_sql

                    lo_DataAdapt = New SqlDataAdapter(sqlComm)
                    'lo_DataTable = New DataSet("dsExpediente")
                    lo_DataTable.BeginLoadData()
                    lo_DataAdapt.Fill(lo_DataTable)
                    lo_DataTable.EndLoadData()
                    'ds.Tables(0).TableName = "dsExpediente"
                    Dim loReportDataSource As ReportDataSource = New ReportDataSource()
                    loReportDataSource.Name = "dsExpediente"
                    loReportDataSource.Value = lo_DataTable
                    localReport.DataSources.Clear()
                    localReport.DataSources.Add(loReportDataSource)

                    localReport.ReportPath = HttpContext.Current.Server.MapPath("..\..\Reportes\\rptDetalleExpediente.rdlc")
                    'ds.DataSetName = "TrafoData"
                    'ds.Tables(0).TableName = "Trafo"
                    deviceInfo = "<DeviceInfo><SimplePageHeaders>True</SimplePageHeaders></DeviceInfo>"
                    bytes = localReport.Render("PDF", deviceInfo, mimeType, encoding, extension, streamids, warnings)


                    ' Create response using the report bytes.
                    ' Response headers are set to return a PDF document.

                    response = Request.CreateResponse(HttpStatusCode.OK)
                    response.Content = New ByteArrayContent(bytes)

                    Dim contentValue As Headers.ContentDispositionHeaderValue = New Headers.ContentDispositionHeaderValue("attachment")
                    contentValue.FileName = "exp" & aoExpediente.numero.ToString.Trim() & "_" & aoExpediente.anio.ToString.Trim() & ".pdf"
                    response.Content.Headers.ContentDisposition = contentValue
                    response.Content.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/pdf")
                    lo_DataTable.Dispose() : lo_DataAdapt.Dispose() : sqlComm.Dispose() : sqlConn.Close() : sqlConn.Dispose()
                    'response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(ds), Encoding.UTF8, "application/json")
                    Return response
                End Using

            Catch ex As Exception
                lo_error = New Mensaje
                lo_error.codigo = "-1"
                lo_error.mensaje = ex.Message
                response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), System.Text.Encoding.UTF8, "application/json")
                Return response
            End Try
        End Function



        <Cors.EnableCors("http://localhost:4200", "*", "*")>
<HttpPost>
        Public Function Pases(<FromBody()> ByVal aoExpediente As ExpedienteBody)
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
                    ls_sql = " SELECT ID_Pase,Nro_Remito,Prefijo,Numero,Anio,Extension"
                    ls_sql &= ",Fecha_Pase,ID_Origen,ID_Destino,Origen,Desc_Origen"
                    ls_sql &= ",Destino,Desc_Destino,Codigo_Destino,Estracto,Iniciador,Fecha_Recepcion,Responsable_Recepcion"
                    ls_sql &= " FROM ml.SA_V_Pases_por_Expediente_Web"
                    ls_sql &= " WHERE  prefijo LIKE '%" & IIf(Not aoExpediente.prefijo Is Nothing, aoExpediente.prefijo, "4069") & "%'"
                    If Not aoExpediente.numero Is Nothing Then
                        ls_sql &= " AND   Numero = " & aoExpediente.numero.ToString & ""
                    End If
                    If aoExpediente.anio.ToString <> 0 Then
                        ls_sql &= " AND   anio = " & aoExpediente.anio
                    End If
                    If Not aoExpediente.extracto Is Nothing Then
                        ls_sql &= " AND   estracto LIKE '%" & aoExpediente.extracto.ToString & "%'"
                    End If
                    If Not aoExpediente.iniciador Is Nothing Then
                        ls_sql &= " AND   iniciador LIKE '%" & aoExpediente.iniciador.ToString & "%' "
                    End If

                    ls_sql &= " Order by Fecha_Pase"
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

        <Cors.EnableCors("http://localhost:4200", "*", "*")>
<HttpPost>
        Public Function Respuestas(<FromBody()> ByVal aoExpediente As ExpedienteBody)
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

                    ls_sql = " SELECT Prefijo,Nro as Numero,Anio,Extension,Fecha_Rta,Tipo_Rta"
                    ls_sql &= ",Descripcion, Estado, Texto, Rta_ID,Pase_ID, Remito, Usuario"
                    ls_sql &= " FROM ml.SA_V_Respuestas"
                    ls_sql &= " WHERE  prefijo LIKE '%" & IIf(Not aoExpediente.prefijo Is Nothing, aoExpediente.prefijo, "4069") & "%'"
                    If Not aoExpediente.numero Is Nothing Then
                        ls_sql &= " AND   Nro LIKE '%" & aoExpediente.numero.ToString & "%'"
                    End If
                    If aoExpediente.anio.ToString <> 0 Then
                        ls_sql &= " AND   anio = " & aoExpediente.anio
                    End If

                    ls_sql &= " Order by Fecha_Rta"
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

        <Cors.EnableCors("http://localhost:4200", "*", "*")>
<HttpGet>
        Public Function Estadistica(ByVal id As Integer) 'As String
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

        <Cors.EnableCors("http://localhost:4200", "*", "*")>
    <HttpGet>
        Public Function SinRecibir(ByVal id As Integer)
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

                    'ls_sql = "Select Tipo_Sistema, Codigo_Sistema, Nivel_Tension, Capa,Elemento, Usuario,Nivel,Insertar,Modificar,Eliminar "
                    ls_sql = "Select  prefijo, numero, anio, extension, id_ultimo_pase, remito,"
                    ls_sql &= "fecha_pase, codigo_origen, origen, codigo_destino, destino, estracto, iniciador     "
                    ls_sql &= "From ml.sa_v_ultimo_pase_por_expediente "
                    ls_sql &= " Where ID_Destino = " & id
                    ls_sql &= " And   fecha_recepcion Is NULL   "
                    ls_sql &= " Order by fecha_pase desc"
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

        <Cors.EnableCors("http://localhost:4200", "*", "*")>
        <HttpGet>
        Public Function SinRecepcion(ByVal id As Integer)
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

                    'ls_sql = "Select Tipo_Sistema, Codigo_Sistema, Nivel_Tension, Capa,Elemento, Usuario,Nivel,Insertar,Modificar,Eliminar "
                    ls_sql = "Select  prefijo, numero, anio, extension, id_ultimo_pase, remito,"
                    ls_sql &= "fecha_pase, codigo_origen, origen, codigo_destino, destino, estracto, iniciador     "
                    ls_sql &= "From ml.sa_v_ultimo_pase_por_expediente "
                    ls_sql &= " Where ID_Origen = " & id
                    ls_sql &= " And   fecha_recepcion Is NULL   "
                    ls_sql &= " Order by fecha_pase desc"
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

        <Cors.EnableCors("http://localhost:4200", "*", "*")>
        <HttpPost>
        Public Function insertPase(<FromBody()> ByVal aoPase As PaseBody)
            Dim response As HttpResponseMessage

            Dim ls_sql As String
            Dim lo_headers As Headers.HttpRequestHeaders
            Dim lo_DataTable As New DataTable
            Dim lo_DataAdapt As SqlDataAdapter
            Dim lo_error As Mensaje
            Dim lo_usuario As Login
            Dim li_id_pase As Integer
            Dim li_id_rta As Integer
            Dim li_id_remito As Integer
            Dim lo_transaction As SqlClient.SqlTransaction

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
                    lo_transaction = sqlConn.BeginTransaction()
                    Dim sqlComm As SqlCommand = New SqlCommand

                    sqlComm.Connection = sqlConn

                    Dim li_conexion_id = CInt(Math.Ceiling(Rnd() * 99999)) + 1
                    'Inserto secuencia pase
                    ls_sql = " INSERT INTO ml.SA_numeracion_remito (id_conexion,prefijo,numero,anio,extension)"
                    ls_sql &= " VALUES(" & li_conexion_id & ",'-',0,0,'-')"
                    sqlComm.CommandText = ls_sql
                    sqlComm.Transaction = lo_transaction
                    sqlComm.ExecuteNonQuery()

                    'Obtengo secuencia pase
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn
                    ls_sql = "SELECT	nro_remito  FROM ml.SA_numeracion_remito Where id_conexion = " & li_conexion_id
                    sqlComm.Transaction = lo_transaction
                    sqlComm.CommandText = ls_sql

                    lo_DataAdapt = New SqlDataAdapter(sqlComm)
                    lo_DataTable = New DataTable
                    lo_DataAdapt.Fill(lo_DataTable)
                    li_id_remito = lo_DataTable.Rows(0)(0)

                    'Borro secuencia pase
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn
                    ls_sql = " DELETE FROM ml.SA_numeracion_remito Where id_conexion = " & li_conexion_id
                    sqlComm.Transaction = lo_transaction
                    sqlComm.CommandText = ls_sql
                    sqlComm.ExecuteNonQuery()


                    For Each bo_exp As PaseExpediente In aoPase.expedientes
                        sqlComm = New SqlCommand
                        sqlComm.Connection = sqlConn
                        'Inserto secuencia pase
                        ls_sql = " INSERT INTO ml.SA_numeracion_id_pase (id_conexion,prefijo,numero,anio,extension)"
                        ls_sql &= " VALUES(" & li_conexion_id & ",'" & bo_exp.prefijo & "'," & bo_exp.numero & "," & bo_exp.anio & ",'" & bo_exp.extension & "')"
                        sqlComm.CommandText = ls_sql
                        sqlComm.Transaction = lo_transaction
                        sqlComm.ExecuteNonQuery()

                        'Obtengo secuencia pase
                        sqlComm = New SqlCommand
                        sqlComm.Connection = sqlConn
                        ls_sql = "SELECT	id_pase  FROM ml.SA_numeracion_id_pase Where id_conexion = " & li_conexion_id
                        sqlComm.Transaction = lo_transaction
                        sqlComm.CommandText = ls_sql

                        lo_DataAdapt = New SqlDataAdapter(sqlComm)
                        lo_DataTable = New DataTable
                        lo_DataAdapt.Fill(lo_DataTable)
                        li_id_pase = lo_DataTable.Rows(0)(0)

                        'Borro secuencia pase
                        sqlComm = New SqlCommand
                        sqlComm.Connection = sqlConn
                        ls_sql = " DELETE FROM ml.SA_numeracion_id_pase Where id_conexion = " & li_conexion_id
                        sqlComm.Transaction = lo_transaction
                        sqlComm.CommandText = ls_sql
                        sqlComm.ExecuteNonQuery()



                        'ls_sql = "Select Tipo_Sistema, Codigo_Sistema, Nivel_Tension, Capa,Elemento, Usuario,Nivel,Insertar,Modificar,Eliminar "
                        ls_sql = " INSERT INTO ml.SA_Registro_de_Pases"
                        ls_sql &= " (RDP_Id_Pase, RDP_Numero_de_remito,RDP_Prefijo_de_Expediente"
                        ls_sql &= " ,RDP_Numero_de_Expediente, RDP_Anio_de_Expediente,RDP_Id_Extension"
                        ls_sql &= " ,RDP_Fecha_de_pase, RDP_Id_Origen_de_Pase,RDP_Id_Destino_de_Pase"
                        ls_sql &= " ,RDP_uid, RDP_est_trabajo,RDP_responsable_emision)"
                        ls_sql &= " VALUES("
                        ls_sql &= li_id_pase
                        ls_sql &= ", " & li_id_remito
                        ls_sql &= ",'" & bo_exp.prefijo & "','" & bo_exp.numero & "'," & bo_exp.anio & ",'" & bo_exp.extension & "'"
                        ls_sql &= ", getdate()"
                        ls_sql &= "," & bo_exp.organigrama_origen.id
                        ls_sql &= ", " & aoPase.organigrama.id
                        ls_sql &= ",'" & lo_usuario.usuario & "'"
                        ls_sql &= ",'WEB'"
                        ls_sql &= ",'" & lo_usuario.usuario & "')"
                        sqlComm.CommandText = ls_sql
                        sqlComm.Transaction = lo_transaction
                        sqlComm.ExecuteNonQuery()

                        'Inserto secuencia respuesta
                        sqlComm = New SqlCommand
                        sqlComm.Connection = sqlConn
                        ls_sql = " INSERT INTO ml.SA_numeracion_id_rta (id_conexion)"
                        ls_sql &= " VALUES(" & li_conexion_id & ")"
                        sqlComm.CommandText = ls_sql
                        sqlComm.Transaction = lo_transaction
                        sqlComm.ExecuteNonQuery()

                        'Obtengo secuencia respuesta
                        sqlComm = New SqlCommand
                        sqlComm.Connection = sqlConn
                        ls_sql = "SELECT	id_rta  FROM ml.SA_numeracion_id_rta Where id_conexion = " & li_conexion_id
                        sqlComm.Transaction = lo_transaction
                        sqlComm.CommandText = ls_sql

                        lo_DataAdapt = New SqlDataAdapter(sqlComm)
                        lo_DataTable = New DataTable
                        lo_DataAdapt.Fill(lo_DataTable)
                        li_id_rta = lo_DataTable.Rows(0)(0)

                        'Borro secuencia respuesta
                        sqlComm = New SqlCommand
                        sqlComm.Connection = sqlConn
                        ls_sql = " DELETE FROM ml.SA_numeracion_id_rta Where id_conexion = " & li_conexion_id
                        sqlComm.Transaction = lo_transaction
                        sqlComm.CommandText = ls_sql
                        sqlComm.ExecuteNonQuery()

                        ls_sql = " INSERT INTO ml.SA_Respuestas "
                        ls_sql &= " (RTA_id_rta, RDP_Id_Pase,RDP_Numero_de_remito,RDP_Prefijo_de_Expediente"
                        ls_sql &= ", RDP_Numero_de_Expediente, RDP_Anio_de_Expediente,RDP_Id_Extension,RTA_Fecha_rta"
                        ls_sql &= ",RTA_f_alta"
                        ls_sql &= ", RTA_Tipo_rta, RTA_descripcion, RTA_texto,RTA_estado,RTA_texto_rtf, RTA_uid)"
                        ls_sql &= " VALUES("
                        ls_sql &= li_id_rta
                        ls_sql &= ", " & li_id_pase
                        ls_sql &= ", " & li_id_remito
                        ls_sql &= ",'" & bo_exp.prefijo & "','" & bo_exp.numero & "'," & bo_exp.anio & ",'" & bo_exp.extension & "'"
                        ls_sql &= ", getdate()"
                        ls_sql &= ", getdate()"
                        ls_sql &= ", 'WEB' "
                        ls_sql &= ", '" & bo_exp.respuesta & "'"
                        ls_sql &= ", '" & bo_exp.respuesta & "'"
                        ls_sql &= ",''"
                        ls_sql &= ", '" & bo_exp.respuesta & "'"
                        ls_sql &= ", '" & lo_usuario.usuario & "')"
                        sqlComm.CommandText = ls_sql
                        sqlComm.Transaction = lo_transaction
                        sqlComm.ExecuteNonQuery()



                    Next
                    Dim lo_respuesta As RespuestaInsertPase = New RespuestaInsertPase
                    lo_respuesta.remito = li_id_remito

                    lo_transaction.Commit()
                    lo_DataTable.Dispose() : lo_DataAdapt.Dispose() : sqlComm.Dispose() : sqlConn.Close() : sqlConn.Dispose()
                    'ds.DataSetName = "TrafoData"
                    'ds.Tables(0).TableName = "Trafo"
                    response = Request.CreateResponse(HttpStatusCode.OK)
                    response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_respuesta), Encoding.UTF8, "application/json")
                    Return response
                End Using

            Catch ex As Exception
                If Not lo_transaction Is Nothing Then
                    lo_transaction.Rollback()
                End If
                lo_error = New Mensaje
                lo_error.codigo = "-1"
                lo_error.mensaje = ex.Message
                response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                Return response
            End Try
        End Function

        <HttpGet>
        <Cors.EnableCors("http://localhost:4200", "*", "*")>
        Public Function recepcionPase(ByVal id As Integer)
            Dim response As HttpResponseMessage

            Dim ls_sql As String
            Dim lo_headers As Headers.HttpRequestHeaders
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


                    'ls_sql = "Select Tipo_Sistema, Codigo_Sistema, Nivel_Tension, Capa,Elemento, Usuario,Nivel,Insertar,Modificar,Eliminar "
                    ls_sql = " UPDATE ml.SA_Registro_de_Pases SET "
                    ls_sql &= "  RDP_Fecha_Recepcion = getdate()"
                    ls_sql &= "  ,RDP_uid_recepcion = '" & lo_usuario.usuario & "'"
                    ls_sql &= "  ,RDP_est_trabajo_recepcion = 'WEB'"
                    ls_sql &= "  ,RDP_responsable_recepcion = '" & lo_usuario.usuario & "'"
                    ls_sql &= " WHERE RDP_Id_Pase =  " & id
                    sqlComm.CommandText = ls_sql
                    sqlComm.ExecuteNonQuery()

                    response = Request.CreateResponse(HttpStatusCode.OK)
                    'response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_respuesta), Encoding.UTF8, "application/json")
                    Return response
                End Using

            Catch ex As Exception
                lo_error = New Mensaje
                lo_error.codigo = "-1"
                lo_error.mensaje = ex.Message
                response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                Return response
            Finally

            End Try
        End Function

    End Class
End Namespace
