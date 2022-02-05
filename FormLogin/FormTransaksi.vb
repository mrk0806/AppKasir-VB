Imports System.Data.SqlClient
Public Class FormTransaksi

    'rumus hitung item
    Sub hitungItem()
        Dim hitung As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            hitung = hitung + DataGridView1.Rows(i).Cells(3).Value
            LItem.Text = hitung
        Next
    End Sub
    'rumushitung
    Sub hitungsub()
        Dim hitung As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            hitung = hitung + DataGridView1.Rows(i).Cells(4).Value
            Label2.Text = hitung
        Next
    End Sub
    'input data di dgv
    Sub buatKolom()
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("kode", "KODE")
        DataGridView1.Columns.Add("nama", "NAMA BARANG")
        DataGridView1.Columns.Add("harga", "HARGA")
        DataGridView1.Columns.Add("jumlah", "JUMLAH")
        DataGridView1.Columns.Add("subTotal", "SUB TOTAL")
    End Sub
    'untuk kosongkan semua properti
    Sub kondisiAwal()
        LItem.Text = ""
        Button1.Enabled = False
        TextBox2.Text = ""
        Label30.Text = ""
        Label24.Text = ""
        Label32.Text = ""
        TextBox3.Text = ""
        Label2.Text = ""
        Label9.Text = ""
        Label10.Text = ""
        Label11.Text = ""
        ComboBox1.Text = ""
        ComboBox1.Items.Clear() 'untuk kosongkan combobox from db
        TextBox1.Text = ""
        TextBox1.Enabled = False
        combobox()
        Call buatKolom()
        If Label9.Text = "" Then
            TextBox2.Enabled = False
            TextBox3.Enabled = False
        End If
        Label13.Text = FormMenuUtama.Slabel8.Text 'menampilkan tanggal dari slabel8 yaitu tanggal
        Label15.Text = FormMenuUtama.Slabel4.Text 'menampilkan nilai yg ada pada lebel 4 yaitu username
        noOtomatis() 'untuk struk otomatis

    End Sub
    Private Sub FormTransaksi_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        kondisiAwal()

    End Sub
    'struk otomatis
    Sub noOtomatis()
        Call koneksi2()
        CMd2 = New SqlCommand("select * from tbl_jual1 where nojual in (select max(nojual) from tbl_jual1)", Conn2)
        Dim urutanKode As String
        Dim hitung As Long
        Rd2 = CMd2.ExecuteReader
        Rd2.Read()
        If Not Rd2.HasRows Then
            urutanKode = "BON-" + "000001"
        Else
            hitung = Microsoft.VisualBasic.Right(Rd2.GetString(0), 5) + 1
            urutanKode = "BON-" + Microsoft.VisualBasic.Right("00000" & hitung, 5)
        End If
        Label7.Text = urutanKode
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call koneksi2()
        Dim simpanJual As String = "Insert into tbl_jual1 values ('" & Label7.Text & "','" & Label13.Text & "','" & Label14.Text & "','" & LItem.Text & "','" & Label2.Text & "','" & TextBox1.Text & "','" & Label24.Text & "','" & ComboBox1.Text & "','" & Label15.Text & "')"
        CMd2 = New SqlCommand(simpanJual, Conn2)
        CMd2.ExecuteNonQuery()
        For baris As Integer = 0 To DataGridView1.Rows.Count - 2
            Dim simpanDetail As String = "insert into tbl_detailjual values('" & Label7.Text & "','" & DataGridView1.Rows(baris).Cells(0).Value & "','" & DataGridView1.Rows(baris).Cells(1).Value & "','" & DataGridView1.Rows(baris).Cells(2).Value & "','" & DataGridView1.Rows(baris).Cells(3).Value & "','" & DataGridView1.Rows(baris).Cells(4).Value & "' )"
            CMd2 = New SqlCommand(simpanDetail, Conn2)
            CMd2.ExecuteNonQuery()

            CMd2 = New SqlCommand("select * from tbl_barang1 where kodebarang='" & DataGridView1.Rows(baris).Cells(0).Value & "' ", Conn2)
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            Call koneksi2()
            Dim kurang As String = "update tbl_barang1 set jumlahbarang='" & Rd2.Item("jumlahbarang") - DataGridView1.Rows(baris).Cells(3).Value & "' where kodebarang= '" & DataGridView1.Rows(baris).Cells(0).Value & "'"
            CMd2 = New SqlCommand(kurang, Conn2)
            CMd2.ExecuteNonQuery()

        Next
        MsgBox("Data Berhasil Disimpan!", MsgBoxStyle.Information, "Information")
        If MessageBox.Show("Apakah ingin cetak nota ", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            'Form1.ShowDialog()
            AxCrystalReport1.SelectionFormula = "totext({tbl_jual1.nojual})='" & Label7.Text & "'"
            AxCrystalReport1.ReportFileName = "nota.rpt"
            AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
            AxCrystalReport1.RetrieveDataFiles()
            AxCrystalReport1.Action = 1
            Call kondisiAwal()
        Else
            Call kondisiAwal()
        End If

    End Sub
    'ambil data dari db kirim ke combobox
    Sub combobox()
        Call koneksi2()
        CMd2 = New SqlCommand("select * from tbl_pelanggan  ", Conn2)
        Rd2 = CMd2.ExecuteReader
        ComboBox1.Text = ""
        While Rd2.Read
            ComboBox1.Items.Add(Rd2("kodepelanggan"))
        End While

    End Sub
    'rubah label dari data yg di pilih combobox => rubah sesuai value databse
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Call koneksi2()
        CMd2 = New SqlCommand("select * from tbl_pelanggan where kodepelanggan='" & ComboBox1.Text & "' ", Conn2)
        Rd2 = CMd2.ExecuteReader
        Rd2.Read()

        If Rd2.HasRows Then
            Label9.Text = Rd2.Item("namapelanggan")
            Label10.Text = Rd2.Item("alamatpelanggan")
            Label11.Text = Rd2.Item("telppelanggan")
        End If
        TextBox2.Enabled = True
        TextBox2.Focus()

    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Label14.Text = TimeOfDay
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress


        If e.KeyChar = Chr(13) Then
            Call koneksi2()
            CMd2 = New SqlCommand("select * from tbl_barang1 where kodebarang='" & TextBox2.Text & "' ", Conn2)
            Rd2 = CMd2.ExecuteReader
            Rd2.Read()
            If Not Rd2.HasRows Then
                MsgBox("Kode Barang Tidak Ada!", MsgBoxStyle.Information, "Information")
            Else
                TextBox2.Text = Rd2.Item("kodebarang")
                Label30.Text = Rd2.Item("namabarang")
                Label32.Text = Rd2.Item("hargabarang")
                Lblstok.Text = Rd2.Item("jumlahbarang")
                TextBox3.Enabled = True
                TextBox3.Focus()
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Label30.Text = "" Then
            MsgBox("Silahakan masukan kode barang kemudian tekan enter!", MsgBoxStyle.Information, "Information")

        Else

            If TextBox3.Text = "" Then
                MsgBox("Silahakan masukan jumlah barang kemudian tekan enter!", MsgBoxStyle.Information, "Information")
            Else

                If Lblstok.Text < TextBox3.Text Then
                    MsgBox("Stok barang kurang dari '" & TextBox3.Text & "'", MsgBoxStyle.Information, "Information")
                    TextBox3.Focus()
                Else
                    DataGridView1.Rows.Add(New String() {TextBox2.Text, Label30.Text, Label32.Text, TextBox3.Text, Val(Label32.Text) * (TextBox3.Text)})
                    Call hitungsub()
                    Call hitungItem()
                    TextBox2.Text = ""
                    Label30.Text = ""
                    Label32.Text = ""
                    TextBox3.Text = ""
                    TextBox3.Enabled = False
                    TextBox2.Focus()
                    TextBox1.Enabled = True
                End If

                
            End If


        End If

    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            If Val(TextBox1.Text) < Val(Label2.Text) Then
                MsgBox("Jumlah uang yang anda masukan kurang dari total belanja!", MsgBoxStyle.Information, "Information")
            ElseIf Val(TextBox1.Text) = Val(Label2.Text) Then
                Label24.Text = 0
                Button1.Enabled = True

            ElseIf Val(TextBox1.Text) > Val(Label2.Text) Then
                Label24.Text = Val(TextBox1.Text) - Val(Label2.Text)
                Button1.Enabled = True

            End If
        End If
    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Call kondisiAwal()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

End Class