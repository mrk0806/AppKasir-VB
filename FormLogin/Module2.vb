Imports System.Data.SqlClient
Module Module2
    Public Conn2 As SqlConnection
    Public Da2 As SqlDataAdapter
    Public Ds2 As DataSet
    Public CMd2 As SqlCommand
    Public Rd2 As SqlDataReader
    Public MyDB As String

    Public Sub koneksi2()
        MyDB = "Data Source=DESKTOP-H5C25HB; initial catalog=db_aplikasi; integrated security=true"
        Conn2 = New SqlConnection(MyDB)
        If Conn2.State = ConnectionState.Closed Then Conn2.Open()
    End Sub
    
End Module
