Imports System
Imports System.Data
Imports System.Data.SqlClient

Module ModulKoneksi

    Public Masuk As New SqlConnection
    Public DAData As SqlDataAdapter
    Public DRData As SqlDataReader
    Public DsData As New DataSet
    Public DtData As New DataTable
    Public Cmd As New SqlCommand
    Public STR As String, Tanya As String
    Public gs_UserId As String

    Public gs_FormatAmount As String = "###,##0.00"
    Public gs_FormatAmount2 As String = "#,##0.00"
    Public gs_FormatAmountUSD As String = "###,##0.000"
    Public gs_FormatQty As String = "###,##0"
    Public gs_FormatDate As String = "dd.MM.yyyy"
    Public gs_FormatDate2 As String = "dd MM yyyy"
    Public gs_FormatWeight As String = "###,##0.00"
    Public gs_FormatWeightMeter As String = "###,##0.000"
    Public gs_FormatPercentage As String = "#,##0.00"
    Public SoundFolder As String = "\Sound\"

    Public gd_MaxPrice As Double = 99999.99
    Public gd_MaxQty As Double = 99999999
    Public gd_MaxFreight As Double = 99999.99
    Public gd_MaxQtyBundle As Double = 9999
    Public gd_MaxQtyKg As Double = 99999999
    Public gd_MaxQtyPcs As Double = 999999
    Public gd_MaxWeightMatrix As Double = 99999999
    Public gd_MaxTotalAmount As Double = 9200000000000
    Public gd_MaxTotalWeight As Double = 999999999
    Public gd_MaxTotalWeightMatrix As Double = 99999
    Public gd_MaxPercentValue As Integer = 100

    Public gs_Menu As String
    Public gs_Title As String
    Public gs_Upload As String

    Public Sub BukaKoneksi()

        STR = "Data Source=.\SQL2014;Initial Catalog=TEST;User ID=sa;pwd=Innotek2018 "
        Masuk = New SqlConnection(STR)
        Masuk.Open()

        Exit Sub

Cek:
        MsgBox("Koneksi server Terputus...!!!", MsgBoxStyle.Information, "Informasi")
        End

    End Sub



End Module
