Imports System.Data.SqlClient
Public Class FormMasterBarang
    'untuk menampilkan data dari database
    Sub MunculGrid()
        Call koneksi2()
        Da2 = New SqlDataAdapter("select kodebarang,namabarang,hargabarang,jumlahbarang,satuanbarang from tbl_barang1  ", Conn2)
        Ds2 = New DataSet
        Da2.Fill(Ds2, "tbl_barang1")
        DataGridView1.DataSource = Ds2.Tables("tbl_barang1")
        DataGridView1.Columns(0).HeaderText = "Kode Barang"
        DataGridView1.Columns(1).HeaderText = "Nama Barang"
        DataGridView1.Columns(2).HeaderText = "Harga Barang"
        DataGridView1.Columns(3).HeaderText = "Jumlah Barang"
        DataGridView1.Columns(4).HeaderText = "Satuan Barang"
        DataGridView1.Columns(0).Width = 80
        DataGridView1.Columns(1).Width = 150
        DataGridView1.Columns(2).Width = 80
        DataGridView1.Columns(3).Width = 80
        DataGridView1.Columns(4).Width = 80
        DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Silver


    End Sub
    'untuk mengkosongkan semua textbox
    Sub kondisiAwal()
        TextBox1.Text = ""
        TextBox1.Enabled = True
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        Button1.Text = "Input"
        Button1.Enabled = True
        Button2.Text = "Edit"
        Button2.Enabled = False
        Button3.Text = "Hapus"
        Button3.Enabled = False
        Button4.Text = "Tutup"

        Call MunculGrid()
    End Sub
    Private Sub FormMasterBarang_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'untuk load saat pertama kali di buka
        Call kondisiAwal()
    End Sub
    'button untuk input data baru
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button1.Text = "Input" Then
            Button1.Text = "Simpan"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call koneksi2()
            CMd2 = New SqlCommand("select * from tbl_barang1 where kodebarang in (select max(kodebarang) from tbl_barang1)", Conn2)
            Dim urutanKode As String
            Dim hitung As Long
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            If Not Rd2.HasRows Then
                urutanKode = "BRG" + "01"
            Else
                hitung = Microsoft.VisualBasic.Right(Rd2.GetString(0), 2) + 1
                urutanKode = "BRG" + Microsoft.VisualBasic.Right("00" & hitung, 2)
            End If
            TextBox1.Text = urutanKode
            TextBox1.Enabled = False
            TextBox2.Select()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Then
                MsgBox("Data Tidak Boleh Kosong!", MsgBoxStyle.Information, "Information")
            Else
                Call koneksi2()
                Dim simpan As String
                simpan = "insert into tbl_barang1 values ('" & TextBox1.Text & "','" & TextBox2.Text & "'," & TextBox3.Text & "," & TextBox4.Text & ",'" & TextBox5.Text & "')"
                CMd2 = New SqlCommand(simpan, Conn2)
                CMd2.ExecuteNonQuery()
                MsgBox("Input Data Berhasil!", MsgBoxStyle.Information, "Information")
                Call kondisiAwal()
            End If

        End If

    End Sub

    'hanya bisa input angka pada textbox3
    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True
    End Sub
    'hanya bisa input angka pada textbox4
    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True
    End Sub
    'button untuk edit
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Button2.Text = "Edit" Then
            Button2.Text = "Simpan"
            Button1.Enabled = False
            Button3.Enabled = False
        Else

            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Then
                MsgBox("Data Tidak Boleh Kosong!", MsgBoxStyle.Information, "Information")

            Else
                Call koneksi2()
                Dim edit As String
                edit = "update tbl_barang1 set namabarang='" & TextBox2.Text & "', hargabarang='" & TextBox3.Text & "', jumlahbarang='" & TextBox4.Text & "', satuanbarang='" & TextBox5.Text & "' where kodebarang='" & TextBox1.Text & "'"
                CMd2 = New SqlCommand(edit, Conn2)
                CMd2.ExecuteNonQuery()
                MsgBox("Edit data Berhasil!", MsgBoxStyle.Information, "Information")
                Call kondisiAwal()
            End If
        End If
    End Sub
    'button untuk close
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Button4.Text = "Tutup" Then
            Me.Close()
        Else
            kondisiAwal()
        End If
    End Sub
    'button untuk hapus
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim pesan As String
        pesan = MsgBox("Yakin hapus Data?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Peringatan")
        If pesan = vbYes Then
            Call koneksi2()
            Dim hapus As String
            hapus = "delete from tbl_barang1 where kodebarang='" & TextBox1.Text & "'"
            CMd2 = New SqlCommand(hapus, Conn2)
            CMd2.ExecuteNonQuery()
            MsgBox("Data Berhasil di Hapus!", MsgBoxStyle.Information, "Information")
            Call kondisiAwal()
        End If

    End Sub
    'ambil data dari gird kirim ke textbox
    Private Sub DataGridView1_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick

        TextBox1.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString()
        If TextBox1.Text = "" Then
            kondisiAwal()
        Else
            TextBox1.Enabled = False
            Call koneksi2()
            CMd2 = New SqlCommand("select * from tbl_barang1 where kodebarang='" & TextBox1.Text & "' ", Conn2)
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            TextBox1.Text = Rd2.Item("kodebarang")
            TextBox1.Enabled = False
            TextBox2.Text = Rd2.Item("namabarang")
            TextBox3.Text = Rd2.Item("hargabarang")
            TextBox4.Text = Rd2.Item("jumlahbarang")
            TextBox5.Text = Rd2.Item("satuanbarang")
            Button1.Enabled = False
            Button2.Enabled = True
            Button3.Enabled = True

        End If
    End Sub
    'btn cari
    Private Sub TextBox6_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox6.KeyPress
        If e.KeyChar = Chr(13) Then
            Call koneksi2()
            CMd2 = New SqlCommand("select * from tbl_barang1 where namabarang LIKE '%" & TextBox6.Text & "%' ", Conn2)
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            If Not Rd2.HasRows Then
                MsgBox("Nama Barang Tidak ada Tidak Ada!", MsgBoxStyle.Information, "Information")
            Else
                Call koneksi2()
                Da2 = New SqlDataAdapter("select kodebarang,namabarang,hargabarang,jumlahbarang,satuanbarang from tbl_barang1 where namabarang LIKE '%" & TextBox6.Text & "%' ", Conn2)
                Ds2 = New DataSet
                Da2.Fill(Ds2, "tbl_barang1")
                DataGridView1.DataSource = Ds2.Tables("tbl_barang1")
                DataGridView1.Columns(0).HeaderText = "Kode Barang"
                DataGridView1.Columns(1).HeaderText = "Nama Barang"
                DataGridView1.Columns(2).HeaderText = "Harga Barang"
                DataGridView1.Columns(3).HeaderText = "Jumlah Barang"
                DataGridView1.Columns(4).HeaderText = "Satuan Barang"
                DataGridView1.Columns(0).Width = 80
                DataGridView1.Columns(1).Width = 150
                DataGridView1.Columns(2).Width = 80
                DataGridView1.Columns(3).Width = 80
                DataGridView1.Columns(4).Width = 80
                DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Silver

            End If
        End If
    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged

    End Sub
End Class