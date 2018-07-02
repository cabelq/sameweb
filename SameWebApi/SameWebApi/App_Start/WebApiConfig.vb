Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http

Public Class WebApiConfig
    Public Shared Sub Register(ByVal config As HttpConfiguration)
        ' Configuración y servicios de API web



        config.Routes.MapHttpRoute( _
            name:="DefaultApi", _
            routeTemplate:="api/{controller}/{id}", _
            defaults:=New With {.id = RouteParameter.Optional} _
        )

        config.Routes.MapHttpRoute( _
            name:="ActionApi", _
            routeTemplate:="api/{controller}/{action}/{id}", _
            defaults:=New With {.id = RouteParameter.Optional}
        )

        'Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable(Of T) return type.
        'To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
        'For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
        'config.EnableQuerySupport()

        'To disable tracing in your application, please comment out or remove the following line of code
        'For more information, refer to: http://www.asp.net/web-api
        config.EnableSystemDiagnosticsTracing()
    End Sub
End Class