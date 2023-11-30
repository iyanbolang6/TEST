'******************************************************************************************************************
'Program       :   TEST

'Created by    :   Iyan 
'Created Date  :   28 Nop 2023
'******************************************************************************************************************
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Drawing
Imports Microsoft.Win32
Imports System.IO

Public Class FORMEMPLOYEES
    Protected IsLoading As Boolean
    Protected MainForm As Form
    Protected ConStr As String
    Protected UserLogin As String
    Protected MenuID As String
    Protected AllowRead As Integer
    Protected AllowWrite As Integer
    Protected AllowDelete As Integer
    Protected AllowApprove As Integer
    Private ls_Msg As String
    Dim ls_Gender As String
    Dim lb_Search, lb_grid As Boolean
    Dim pItem As String
    Dim oDTP As New DateTimePicker
    Dim ls_SQL As String
    Dim dt As New DataTable
    Dim cmd As New SqlCommand
    Dim da As SqlDataAdapter
    Dim li_Row As Integer

    Private Enum GridHeader
        ColEmployeeID = 0
        ColFullName = 1
        ColBirthday = 2
        Count = 3
    End Enum

#Region "PROCEDURE"

    Private Sub P_ClearScreen()
        Call up_GridLoad()
        txtFPencarian.Text = ""
        txtMsg.Text = ""
        txtEmployeeID.Enabled = True
        txtEmployeeID.Text = ""
        txtFullName.Text = ""
        DTP.Text = Format(Now)
        txtEmployeeID.Focus()
    End Sub

    Private Sub up_GridHeader(ByVal pBoolean As Boolean)
        Dim IRow As Integer = 1
        Dim ICol As Integer

        With Grid

            If pBoolean = True Then
                .Rows.Fixed = 1
                .Rows.Count = 1
                .Cols.Fixed = 0
            End If
            .Cols.Count = GridHeader.Count

            .Item(0, GridHeader.ColEmployeeID) = "Employee ID"
            .Item(0, GridHeader.ColFullName) = "Full Name"
            .Item(0, GridHeader.ColBirthday) = "Birth Day"

            .Cols(GridHeader.ColEmployeeID).Width = 150
            .Cols(GridHeader.ColFullName).Width = 300
            .Cols(GridHeader.ColBirthday).Width = 100

            .Rows(0).Height = 30
            .GetCellRange(0, GridHeader.ColEmployeeID, 0, GridHeader.ColBirthday).StyleNew.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Nodes
            .BackColor = Drawing.Color.LemonChiffon
            .GetCellRange(0, GridHeader.ColEmployeeID, 0, GridHeader.ColBirthday).StyleNew.BackColor = Color.PeachPuff
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Default


        End With
    End Sub

    Private Sub up_GridLoad()
        dt = New DataTable
        li_Row = 1
        Try

            Me.Cursor = Cursors.WaitCursor

            With Grid
                dt.Clear()
                dt = uf_GetList()
                If dt.Rows.Count > 0 Then
                    Grid.DataSource = dt
                    Call up_GridHeader("False")
                    .GetCellRange(1, GridHeader.ColEmployeeID, dt.Rows.Count, GridHeader.ColBirthday).StyleNew.BackColor = Color.LemonChiffon
                    .BackColor = Drawing.Color.LemonChiffon
                Else
                    Grid.DataSource = Nothing
                    Call up_GridHeader("True")
                End If

                lblTot.Text = Microsoft.VisualBasic.Format(CDbl(dt.Rows.Count), gs_FormatQty)
                Me.Cursor = Cursors.Default

            End With

        Catch ex As Exception

            Me.Cursor = Cursors.Default
        End Try

    End Sub

#End Region

#Region "FUNCTION"
    Private Function uf_validateInput()
        Dim tanggalLahir As DateTime
        tanggalLahir = DTP.Value
        Dim tanggalSekarang As DateTime = DateTime.Now ' Tanggal saat ini

        Dim umur As Integer = tanggalSekarang.Year - tanggalLahir.Year

        If txtEmployeeID.Text = "" Then
            txtEmployeeID.BackColor = Color.Red
            txtEmployeeID.Focus()
            txtMsg.Text = "Please input Employee ID !!!"
            Return False
        ElseIf txtFullName.Text = "" Then
            txtFullName.BackColor = Color.Red
            txtFullName.Focus()
            txtMsg.Text = "Please input Employee Fulname !!!"
            Return False
        ElseIf CDate(DTP.Value) = CDate(Format(Now)) Then
            DTP.BackColor = Color.Red
            DTP.Focus()
            txtMsg.Text = "The date cannot be the same as today !!!"
            Return False
        ElseIf umur < 17 Then
            DTP.BackColor = Color.Red
            DTP.Focus()
            txtMsg.Text = "you are less than 17 years old !!!"
            Return False
        End If

        Return True
    End Function

    Private Sub DeleteData()
        If txtEmployeeID.Enabled = False Then
            Call BukaKoneksi()

            STR = "Delete [TEST].[dbo].[EMPLOYEES]  Where [EmployeeId] = '" & txtEmployeeID.Text & "'"
            cmd = New SqlCommand(STR, Masuk)
            cmd.ExecuteNonQuery()
            Call up_GridLoad()
            Call P_ClearScreen()
            txtMsg.Text = "Data Deleted Successfully !"
        Else
            txtMsg.Text = "No Data to Delete !!!"
        End If
    End Sub
    Private Sub SimpanData()
        Call BukaKoneksi()

        STR = "Select * From [TEST].[dbo].[EMPLOYEES] Where EmployeeId ='" & Trim(txtEmployeeID.Text) & "'"
        cmd = New SqlCommand(STR, Masuk)
        DRData = cmd.ExecuteReader
        DRData.Read()

        If Not DRData.HasRows Then
            Call BukaKoneksi()

            STR = "Insert Into [TEST].[dbo].[EMPLOYEES]([EmployeeId],[FullName],[BIrthDate],[RegisterDate],[UpdateDate]) Values " & vbCrLf &
                  " ('" & UCase(txtEmployeeID.Text) & "','" & UCase(txtFullName.Text) & "'," & vbCrLf &
                  "'" & DTP.Value & "',GetDate(),GetDate())"
            cmd = New SqlCommand(STR, Masuk)
            cmd.ExecuteNonQuery()
            Call P_ClearScreen()
            txtMsg.Text = "Data saved successfully !"
        Else
            If txtEmployeeID.Enabled = True Then
                txtMsg.Text = "There is no data to change !"
            Else
                Call BukaKoneksi()

                STR = " Update  [TEST].[dbo].[EMPLOYEES] set " & vbCrLf &
                     " FullName = '" & UCase(txtFullName.Text) & "', BIrthDate='" & DTP.Value & "'," & vbCrLf &
                     " UpdateDate = GetDate() Where EmployeeId ='" & Trim(txtEmployeeID.Text) & "'"
                cmd = New SqlCommand(STR, Masuk)
                cmd.ExecuteNonQuery()
                Call P_ClearScreen()
                txtMsg.Text = "Data Changed Successfully !"
            End If
        End If
    End Sub

    Function uf_GetList() As DataTable
        Dim ld_ds As New DataTable
        BukaKoneksi()

        Dim ls_SQL As String = ""

        ls_SQL = " SELECT ISNULL([EmployeeId],'') EmployeeId,ISNULL([FullName],'') FullName, CONVERT(varchar, [BIrthDate], 106) BIrthDate  " & vbCrLf &
                 " FROM [TEST].[dbo].[EMPLOYEES]  "
        If txtFPencarian.Text <> "" Then
            ls_SQL = ls_SQL + " WHERE ISNULL([EmployeeId],'') like '" & txtFPencarian.Text & "' OR " & vbCrLf &
                         "      ISNULL([FullName],'') like '" & txtFPencarian.Text & "'"
        End If
        ls_SQL = ls_SQL + "  ORDER BY EmployeeId DESC " & vbCrLf &
                          "  "

        cmd = New SqlCommand(ls_SQL, Masuk)
        da = New SqlDataAdapter(cmd)
        da.Fill(ld_ds)
        Return ld_ds
    End Function

#End Region

#Region "EVENT"
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Tanya = MsgBox("Are you sure you want to Delete data ? ", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Informasi")
        Select Case Tanya
            Case vbCancel
                Call up_GridLoad()
                Call P_ClearScreen()
            Case vbOK
                Call DeleteData()
        End Select
    End Sub
    Private Sub txtEmployeeID_TextChanged(sender As Object, e As EventArgs) Handles txtEmployeeID.TextChanged
        txtMsg.Text = ""
        If txtEmployeeID.Text <> "" Then
            txtEmployeeID.BackColor = Color.White
        End If
    End Sub
    Private Sub txtFullName_TextChanged(sender As Object, e As EventArgs) Handles txtFullName.TextChanged
        txtMsg.Text = ""
        If txtFullName.Text <> "" Then
            txtFullName.BackColor = Color.White
        End If
    End Sub
    Private Sub DTP_ValueChanged(sender As Object, e As EventArgs) Handles DTP.ValueChanged
        DTP.BackColor = Color.Red
    End Sub
    Private Sub Grid_DoubleClick(sender As Object, e As System.EventArgs) Handles Grid.DoubleClick
        Try
            If Grid.Rows.Count = 1 Then
                txtMsg.Text = "there is no data to select !!!"
                Exit Sub
            End If

            txtEmployeeID.Enabled = False
            txtEmployeeID.Text = Grid.Item(Grid.RowSel, GridHeader.ColEmployeeID).ToString.Trim
            txtFullName.Text = Grid.Item(Grid.RowSel, GridHeader.ColFullName).ToString.Trim
            DTP.Text = Grid.Item(Grid.RowSel, GridHeader.ColBirthday).ToString.Trim

        Catch ex As Exception

            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If uf_validateInput() = True Then
            Tanya = MsgBox("Are you sure you want to save the data ? ", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Informasi")
            Select Case Tanya
                Case vbCancel
                    Call up_GridLoad()
                    Call P_ClearScreen()
                Case vbOK
                    Call SimpanData()
            End Select
        End If
    End Sub
    Private Sub FORMEMPLOYEES_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtMsg.Text = ""
        Call P_ClearScreen()
        txtEmployeeID.Select()
    End Sub
    Private Sub BtnCari_Click(sender As Object, e As EventArgs) Handles BtnCari.Click
        txtMsg.Text = ""
        Call up_GridLoad()
    End Sub
    Private Sub BtnLogout_Click(sender As System.Object, e As System.EventArgs) Handles BtnLogout.Click
        Me.Close()
    End Sub
    Private Sub btnMenu_Click(sender As System.Object, e As System.EventArgs) Handles btnMenu.Click
        Me.Close()
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Tanya = MsgBox("Are you sure you want to Cancel data ? ", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "Informasi")
        Select Case Tanya
            Case vbOK
                Call P_ClearScreen()
                Call up_GridLoad()
                txtEmployeeID.Select()
                txtMsg.Text = "Data cleared successfully !!!"
        End Select
    End Sub

#End Region

End Class