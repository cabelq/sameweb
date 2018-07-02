Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Http

Imports System.Web.Http

Namespace Controllers
    <Cors.EnableCors("http://localhost:4200", "*", "*")>
    Public Class LoginController
        Inherits ApiController

        Private Function lfunGenerarToken() As String

            Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&()=*abcdefghijklmnopqrstuvwxyz"
            Dim r As New Random
            Dim sb As New StringBuilder



            For i As Integer = 1 To 128
                Dim idx As Integer = r.Next(0, 71)
                sb.Append(s.Substring(idx, 1))
            Next
            Return sb.ToString()
        End Function


        ' POST api/login
        <HttpPost>
        <Cors.EnableCors("http://localhost:4200", "*", "*")>
        Public Function Login(<FromBody()> ByVal aoLogin As LoginBody) As HttpResponseMessage  'As IEnumerable(Of String) 
            Dim response As HttpResponseMessage
            Dim ls_sql As String = ""
            Dim lo_DataTable As New DataTable
            Dim lo_DataRow As DataRow
            Dim lo_usuario As Login
            Dim lo_error As Mensaje
            Dim lo_contexto As System.Web.HttpContext
            Dim lo_headers As Headers.HttpRequestHeaders
            Dim ls_organigrama As String
            Dim lo_organigrama As DependenciaOrganigrama

            Try
                lo_headers = Me.Request.Headers

                '-----------------------------------------------------------------------------------------------
                ' Valido que exista el usuario y la contraseña sea correcta 
                '-----------------------------------------------------------------------------------------------

                Using sqlConn As New SqlConnection()
                    ';User Id=sa;Password=ml
                    sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAME").ConnectionString & ";User Id=" & aoLogin.usuario & ";Password=" & aoLogin.password
                    sqlConn.Open()
                    sqlConn.Close()
                    sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAME").ConnectionString & ";User Id=sa;Password=ml"
                    sqlConn.Open()
                    Dim sqlComm As SqlCommand = New SqlCommand
                    sqlComm.Connection = sqlConn

                    sqlComm.CommandText = "SELECT * FROM ml.Usuarios WHERE uid = '" & aoLogin.usuario & "'"
                    Dim sqlDataAdapt As SqlDataAdapter = New SqlDataAdapter(sqlComm)
                    sqlDataAdapt.Fill(lo_DataTable)
                    If lo_DataTable.Rows.Count > 0 Then
                        lo_DataRow = lo_DataTable.Rows(0)
                    Else
                        lo_DataRow = Nothing
                    End If


                    If lo_DataRow Is Nothing Then
                        lo_error = New Mensaje
                        lo_error.codigo = "1"
                        lo_error.mensaje = "No existe el usuario ingresado"
                        response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                        response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                        Return response
                    End If

                    'If lo_DataRow("usu_password") <> aoLogin.password Then
                    '  lo_error = New Mensaje
                    '  lo_error.codigo = "2"
                    '  lo_error.mensaje = "La contraseña es incorrecta"
                    '  response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                    '  response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                    '  Return response       

                    'End If
                    lo_usuario = New Login()
                    lo_usuario.usuario = aoLogin.usuario
                    lo_usuario.nombre = lo_DataRow("apellido") + ", " + lo_DataRow("nombre")
                    lo_usuario.dependencia = lo_DataRow("ofi_nombre")
                    lo_usuario.token = lfunGenerarToken()
                    lo_usuario.expiracion = DateTime.Now.AddHours(4).ToString("dd/MM/yyyy HH:mm:ss")
                    'lo_usuario.expiracion = Now().AddHours(Convert.ToDouble(ConfigurationManager.AppSettings("JwtHorasExpritacion")))




                    'Elimino el token en la base si existe para el usuario
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn

                    sqlComm.CommandText = "DELETE FROM ml.WEB_Usuarios_Tokens WHERE uid = '" & aoLogin.usuario & "'"
                    If sqlComm.ExecuteNonQuery() = -1 Then
                        lo_error = New Mensaje
                        lo_error.codigo = "-2"
                        lo_error.mensaje = "No se puede eliminar el token de la base"
                        response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                        response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                    End If

                    'Inserto el token en la base
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn

                    sqlComm.CommandText = "INSERT INTO ml.WEB_Usuarios_Tokens (Token,uid, Fecha_expiracion) VALUES ('" & lo_usuario.token & "','" & lo_usuario.usuario & "',CAST( '" & lo_usuario.expiracion & "' as datetime))"
                    ls_sql = sqlComm.CommandText
                    If sqlComm.ExecuteNonQuery() = -1 Then
                        lo_error = New Mensaje
                        lo_error.codigo = "-2"
                        lo_error.mensaje = "No se puede insertar el token en la base"
                        response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                        response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                    End If

                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn
                    sqlComm.CommandText = "SELECT parametro, valor_s, valor_n FROM ml.USUARIOS_PARAMETROS Where sis_nombre = 'SAME' AND uid = '" & aoLogin.usuario & "'"
                    sqlDataAdapt = New SqlDataAdapter(sqlComm)
                    lo_DataTable = New DataTable
                    sqlDataAdapt.Fill(lo_DataTable)
                    For Each lo_DataRow In lo_DataTable.Rows
                        Select Case lo_DataRow("parametro")
                            Case "CARATULAR"
                                lo_usuario.parametros.caratular = lo_DataRow("valor_s")
                            Case "EXPORTAR DATOS"
                                lo_usuario.parametros.exportar_datos = lo_DataRow("valor_s")
                            Case "HABILITAR ORIGEN"
                                lo_usuario.parametros.habilitar_origen = lo_DataRow("valor_s")
                            Case "ID ORGANIGRAMA"
                                lo_usuario.parametros.organigrama_default.id = lo_DataRow("valor_n")
                                'lo_usuario.parametros.id_organigrama = lo_DataRow("valor_n")
                                ls_organigrama = Str(lo_DataRow("valor_n")) + "-" + IIf(IsDBNull(lo_DataRow("valor_s")), "", lo_DataRow("valor_s"))
                                'lo_usuario.parametros.id_organigrama_otros = Str(lo_DataRow("valor_n")) + "-" + IIf(IsDBNull(lo_DataRow("valor_s")), "", lo_DataRow("valor_s"))
                            Case "PREFIJO"
                                lo_usuario.parametros.prefijo = lo_DataRow("valor_s")
                        End Select
                    Next

                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn
                    sqlComm.CommandText = "SELECT ORG_Descripcion FROM ml.ORGANIGRAMA Where ORG_Id_Organigrama = " & lo_usuario.parametros.organigrama_default.id
                    sqlDataAdapt = New SqlDataAdapter(sqlComm)
                    lo_DataTable = New DataTable
                    sqlDataAdapt.Fill(lo_DataTable)
                    For Each lo_DataRow In lo_DataTable.Rows
                        lo_usuario.parametros.organigrama_default.descripcion = lo_DataRow("ORG_Descripcion")
                    Next
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn
                    sqlComm.CommandText = "SELECT ORG_Id_Organigrama,ORG_Descripcion FROM ml.ORGANIGRAMA Where ORG_Id_Organigrama in (" & ls_organigrama.ToString().Replace("-", ",") & ")"
                    sqlDataAdapt = New SqlDataAdapter(sqlComm)
                    lo_DataTable = New DataTable
                    sqlDataAdapt.Fill(lo_DataTable)
                    'Dim i = 0
                    'lo_usuario.parametros.organigrama_descripcion_otros = ""
                    For Each lo_DataRow In lo_DataTable.Rows
                        'If i <> 0 Then
                        '    lo_usuario.parametros.organigrama_descripcion_otros += ","
                        'End If
                        lo_organigrama = New DependenciaOrganigrama
                        lo_organigrama.id = lo_DataRow("ORG_Id_Organigrama")
                        lo_organigrama.descripcion = lo_DataRow("ORG_Descripcion")
                        lo_usuario.parametros.organigramas.Add(lo_organigrama)
                        'lo_usuario.parametros.organigrama_descripcion_otros += lo_DataRow("ORG_Descripcion")
                        'i = 1
                    Next

                    'ds.DataSetName = "CapasData"
                    'ds.Tables(0).TableName = "Capas"

                    'If System.Web.HttpContext.Current.Session("usuario") Is Nothing Then
                    '    System.Web.HttpContext.Current.Session.Add("usuario", lo_usuario.usuario)
                    'Else
                    '    System.Web.HttpContext.Current.Session("usuario") = lo_usuario.usuario
                    'End If

                    'If System.Web.HttpContext.Current.Session("token") Is Nothing Then
                    '    System.Web.HttpContext.Current.Session.Add("token", lo_usuario.token)
                    'Else
                    '    System.Web.HttpContext.Current.Session("token") = lo_usuario.token
                    'End If

                    'If System.Web.HttpContext.Current.Session("expira") Is Nothing Then
                    '    System.Web.HttpContext.Current.Session.Add("expira", lo_usuario.expiracion.ToString())
                    'Else
                    '    System.Web.HttpContext.Current.Session("expira") = lo_usuario.expiracion.ToString()
                    'End If

                    sqlDataAdapt.Dispose() : sqlComm.Dispose() : sqlConn.Close() : sqlConn.Dispose()

                    response = Request.CreateResponse(HttpStatusCode.OK)
                    response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_usuario), Encoding.UTF8, "application/json")
                    Return response 'Request.CreateResponse(HttpStatusCode.OK, Newtonsoft.Json.JsonConvert.SerializeObject(ds), Encoding.UTF8, "application/json")
                End Using

            Catch ex As Exception

                'Return Nothing
                lo_error = New Mensaje
                lo_error.codigo = "-1"
                lo_error.mensaje = ex.Message & ls_sql
                response = Request.CreateResponse(HttpStatusCode.Unauthorized)
                response.Content = New StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lo_error), Encoding.UTF8, "application/json")
                Return response

            End Try
        End Function
    End Class
End Namespace
