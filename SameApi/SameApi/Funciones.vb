Imports System.Data.SqlClient

Module Funciones

  Public Function gfun_autorizacion(ByVal ao_token As String) As Login
    Dim lo_DataAdapt As SqlDataAdapter
    Dim lo_DataTable As New DataTable
    Dim lo_DataRow As DataRow
    Dim lo_login As Login
    Try
      Using sqlConn As New SqlConnection(ConfigurationManager.ConnectionStrings("SAME").ConnectionString)
        sqlConn.ConnectionString = ConfigurationManager.ConnectionStrings("SAME").ConnectionString & ";User Id=sa;Password=ml"
        sqlConn.Open()
        Dim sqlComm As SqlCommand = New SqlCommand
        sqlComm.Connection = sqlConn

        sqlComm.CommandText = "Select Token, uid, Fecha_expiracion From ML.WEB_Usuarios_Tokens Where Token = '" & ao_token & "'"
        lo_DataAdapt = New SqlDataAdapter(sqlComm)
        lo_DataAdapt.Fill(lo_DataTable)
        If lo_DataTable.Rows.Count > 0 Then
          lo_DataRow = lo_DataTable.Rows(0)
          If CDate(lo_DataRow("Fecha_expiracion")) > Now() Then
            lo_login = New Login()
            lo_login.usuario = lo_DataRow("uid")
            lo_login.token = lo_DataRow("Token")
            lo_login.expiracion = lo_DataRow("Fecha_expiracion")

            lo_DataTable.Dispose() : lo_DataAdapt.Dispose() : sqlComm.Dispose() : sqlConn.Close() : sqlConn.Dispose()
            Return lo_login
          Else
            lo_DataTable.Dispose() : lo_DataAdapt.Dispose() : sqlComm.Dispose() : sqlConn.Close() : sqlConn.Dispose()
            Return Nothing
          End If
        Else
          lo_DataTable.Dispose() : lo_DataAdapt.Dispose() : sqlComm.Dispose() : sqlConn.Close() : sqlConn.Dispose()
          Return Nothing
        End If
      End Using
    Catch ex As Exception
      Return Nothing
    Finally

    End Try

  End Function


End Module
