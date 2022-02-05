'Imports System.Data.Odbc
Imports System.Data.SqlClient

Public Class FormLogin
    Sub kondisiAwal()
        TextBox1.Text = "ADM01"
        TextBox2.Text = "ADMIN"
    End Sub
    Sub terbuka()
        FormMenuUtama.LoginToolStripMenuItem.Visible = False
        FormMenuUtama.LogoutToolStripMenuItem.Visible = True
        FormMenuUtama.MasterToolStripMenuItem.Visible = True
        FormMenuUtama.TransaksiToolStripMenuItem.Visible = True
        FormMenuUtama.LaporanToolStripMenuItem.Visible = True
        FormMenuUtama.UtilityToolStripMenuItem.Visible = True
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            MsgBox("Kode Admin Atau Password Tidak Boleh Kosong!")
        Else
            'Call koneksi()
            Call koneksi2()
            'CMd = New OdbcCommand("Select * from tbl_admin where kodeadmin='" & TextBox1.Text & "' and pwd='" & TextBox2.Text & "'", Conn)
            CMd2 = New SqlCommand("Select * from tbl_admin where kodeadmin='" & TextBox1.Text & "' and pwd='" & TextBox2.Text & "'", Conn2)
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            If Rd2.HasRows Then
                'If TextBox1.Text = "AA" And TextBox2.Text = "AA" Then
                Me.Close()
                Call terbuka()
                FormMenuUtama.Slabel2.Text = Rd2.Item("kodeadmin")
                FormMenuUtama.Slabel4.Text = Rd2.Item("username")
                FormMenuUtama.Slabel6.Text = Rd2.Item("level")
            Else
                MsgBox("Kode Admin Atau Password Salah!")
            End If
        End If
    End Sub

  
    Private Sub FormLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call kondisiAwal()
        TextBox2.PasswordChar = "*"

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            TextBox2.PasswordChar = ""
        Else
            TextBox2.PasswordChar = "*"
        End If
    End Sub
End Class