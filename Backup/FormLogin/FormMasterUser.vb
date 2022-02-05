Imports System.Data.SqlClient



Public Class FormMasterUser
    Sub kondisiAwal()
        TextBox1.Text = ""
        TextBox1.Enabled = True
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox3.PasswordChar = "*"
        TextBox4.Text = ""
        ComboBox1.Text = "Select From.."
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("ADMIN")
        ComboBox1.Items.Add("USER")
        Button1.Text = "Input"
        Button1.Enabled = True
        Button2.Text = "Edit"
        Button2.Enabled = False
        Button3.Text = "Hapus"
        Button3.Enabled = False
        Button4.Text = "Tutup"

        Call MunculGrid()
    End Sub
    Private Sub FormMasterUser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call kondisiAwal()

    End Sub
    Sub MunculGrid()
        Call koneksi2()
        Da2 = New SqlDataAdapter("select kodeadmin,username,level from tbl_admin  ", Conn2)
        Ds2 = New DataSet
        Da2.Fill(Ds2, "tbl_admin")
        DataGridView1.DataSource = Ds2.Tables("tbl_admin")
        DataGridView1.Columns(0).HeaderText = "Kode Admin"
        DataGridView1.Columns(1).HeaderText = "Username"
        DataGridView1.Columns(2).HeaderText = "Level"
        DataGridView1.Columns(0).Width = 90
        DataGridView1.Columns(1).Width = 100
        DataGridView1.Columns(2).Width = 70
        DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Silver


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button1.Text = "Input" Then
            Button1.Text = "Simpan"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call koneksi2()
            CMd2 = New SqlCommand("select * from tbl_admin where kodeadmin in (select max(kodeadmin) from tbl_admin)", Conn2)
            Dim urutanKode As String
            Dim hitung As Long
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            If Not Rd2.HasRows Then
                urutanKode = "ADM" + "01"
            Else
                hitung = Microsoft.VisualBasic.Right(Rd2.GetString(0), 2) + 1
                urutanKode = "ADM" + Microsoft.VisualBasic.Right("00" & hitung, 2)
            End If
            TextBox1.Text = urutanKode
            TextBox1.Enabled = False

        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Data Tidak Boleh Kosong!", MsgBoxStyle.Information, "Information")
            ElseIf ComboBox1.Text <> "ADMIN" And ComboBox1.Text <> "USER" Then
                MsgBox("Silahkan pilih Level ADMIN/USER", MsgBoxStyle.Information, "Information")
            Else
                Call koneksi2()
                Dim simpan As String
                simpan = "insert into tbl_admin values ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & ComboBox1.Text & "')"
                CMd2 = New SqlCommand(simpan, Conn2)
                CMd2.ExecuteNonQuery()
                MsgBox("Input Data Berhasil!", MsgBoxStyle.Information, "Information")
                Call kondisiAwal()
            End If
            
        End If

    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Call koneksi2()
            CMd2 = New SqlCommand("select * from tbl_admin where kodeadmin='" & TextBox1.Text & "' ", Conn2)
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            If Not Rd2.HasRows Then
                MsgBox("Kode Admin Tidak Ada!", MsgBoxStyle.Information, "Information")
            Else
                TextBox1.Text = Rd2.Item("kodeadmin")
                TextBox1.Enabled = False
                TextBox2.Text = Rd2.Item("username")
                TextBox3.Text = Rd2.Item("pwd")
                ComboBox1.Text = Rd2.Item("level")
                Button1.Enabled = False
                Button2.Enabled = True
                Button3.Enabled = True

            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Button2.Text = "Edit" Then
            Button2.Text = "Simpan"
            Button1.Enabled = False
            Button3.Enabled = False
        Else

            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Data Tidak Boleh Kosong!", MsgBoxStyle.Information, "Information")
            ElseIf ComboBox1.Text <> "ADMIN" And ComboBox1.Text <> "USER" Then
                MsgBox("Silahkan pilih Level ADMIN/USER", MsgBoxStyle.Information, "Information")
            Else
                Call koneksi2()
                Dim edit As String
                edit = "update tbl_admin set username='" & TextBox2.Text & "', pwd='" & TextBox3.Text & "', level='" & ComboBox1.Text & "' where kodeadmin='" & TextBox1.Text & "'"
                CMd2 = New SqlCommand(edit, Conn2)
                CMd2.ExecuteNonQuery()
                MsgBox("Edit data Berhasil!", MsgBoxStyle.Information, "Information")
                Call kondisiAwal()
            End If
        End If


            
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim pesan As String
        pesan = MsgBox("Yakin hapus Data?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Peringatan")
        If pesan = vbYes Then
            Call koneksi2()
            Dim hapus As String
            hapus = "delete from tbl_admin where kodeadmin='" & TextBox1.Text & "'"
            CMd2 = New SqlCommand(hapus, Conn2)
            CMd2.ExecuteNonQuery()
            MsgBox("Data Berhasil di Hapus!", MsgBoxStyle.Information, "Information")
            Call kondisiAwal()
        End If
        

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

    End Sub

    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        If e.KeyChar = Chr(13) Then
            Call koneksi2()
            CMd2 = New SqlCommand("select * from tbl_admin where username LIKE '%" & TextBox4.Text & "%' ", Conn2)
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            If Not Rd2.HasRows Then
                MsgBox("Username Tidak ada Tidak Ada!", MsgBoxStyle.Information, "Information")
            Else
                Call koneksi2()
                Da2 = New SqlDataAdapter("select kodeadmin,username,level from tbl_admin where username LIKE '%" & TextBox4.Text & "%' ", Conn2)
                Ds2 = New DataSet
                'Da2.Fill(Ds2, "cari")
                'DataGridView1.DataSource = Ds2.Tables("cari")
                'DataGridView1.ReadOnly = True
                Da2.Fill(Ds2, "tbl_admin")
                DataGridView1.DataSource = Ds2.Tables("tbl_admin")
                DataGridView1.Columns(0).HeaderText = "Kode Admin"
                DataGridView1.Columns(1).HeaderText = "Username"
                DataGridView1.Columns(2).HeaderText = "Level"
                DataGridView1.Columns(0).Width = 90
                DataGridView1.Columns(1).Width = 100
                DataGridView1.Columns(2).Width = 70
                DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Silver

            End If
        End If
    End Sub

    
  
    Private Sub DataGridView1_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        TextBox1.Enabled = False
        Call koneksi2()
        CMd2 = New SqlCommand("select * from tbl_admin where kodeadmin='" & TextBox1.Text & "' ", Conn2)
        Rd2 = CMd2.ExecuteReader
        Rd2.Read()
        TextBox1.Text = Rd2.Item("kodeadmin")
        TextBox1.Enabled = False
        TextBox2.Text = Rd2.Item("username")
        TextBox3.Text = Rd2.Item("pwd")
        ComboBox1.Text = Rd2.Item("level")
        Button1.Enabled = False
        Button2.Enabled = True
        Button3.Enabled = True

    End Sub

   
End Class