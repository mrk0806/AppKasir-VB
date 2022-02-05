Imports System.Data.Odbc

Module Module1
    Public Conn As OdbcConnection
    Public Da As OdbcDataAdapter
    Public Ds As DataSet
    Public CMd As OdbcCommand
    Public Rd As OdbcDataReader

    Public MyDB As String
    Public Sub koneksi()

        MyDB = "Driver={MySQL ODBC 3.51 Driver};database=db_aplikasi;server=localhost;uid=root"

        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then Conn.Open()
    End Sub
End Module
