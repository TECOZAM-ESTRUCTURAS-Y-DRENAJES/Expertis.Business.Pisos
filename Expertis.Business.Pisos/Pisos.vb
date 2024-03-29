﻿Imports Solmicro.Expertis
Imports Solmicro.Expertis.Engine.BE.BusinessProcesses
Imports Solmicro.Expertis.Engine.DAL
Imports Solmicro.Expertis.Engine.BE
Imports Solmicro.Expertis.Engine

Public Class Pisos

    Inherits Solmicro.Expertis.Engine.BE.BusinessHelper

    Public Sub New()
        MyBase.New(cnEntidad)
    End Sub

    Private Const cnEntidad As String = "tbMaestroPisos"

    Protected Overrides Sub RegisterAddnewTasks(ByVal addnewProcess As Engine.BE.BusinessProcesses.Process)
        MyBase.RegisterAddnewTasks(addnewProcess)
        addnewProcess.AddTask(Of DataRow)(AddressOf FillDefaultValues)
    End Sub

    <Task()> Public Shared Sub FillDefaultValues(ByVal data As DataRow, ByVal services As ServiceProvider)
        ProcessServer.ExecuteTask(Of DataRow)(AddressOf AsignarValoresPredeterminados, data, services)
    End Sub

    <Task()> Public Shared Sub AsignarValoresPredeterminados(ByVal data As DataRow, ByVal services As ServiceProvider)
        data("IDPiso") = AdminData.GetAutoNumeric
        'data("IDContrato") = AdminData.GetAutoNumeric
        data("Activo") = True
        data("Bloqueado") = False
    End Sub
    <Task()> Public Shared Sub checkObligaciones(ByVal data As DataRow, ByVal services As ServiceProvider)
        If data("IVAs").ToString.Length = 0 Then
            ApplicationService.GenerateError("El IVA es obligatorio, 0 o 21.")
        End If
        If data("Vencimiento").ToString.Length = 0 Then
            ApplicationService.GenerateError("Seleccione el tipo de mes, vencido o adelantado")
        End If

    End Sub
    <Task()> Public Shared Sub checkObligaciones2(ByVal data As DataRow, ByVal services As ServiceProvider)
        Try
            Dim cadena As DataTable
            Dim sql As String
            Dim cod As String
            cod = data("Codigo")
            sql = "select * from tbMaestroPisos where Codigo='" & cod & "'"
            cadena = AdminData.GetData(sql)
            If cadena.Rows.Count <> 0 Then
                ApplicationService.GenerateError("Ya existe un piso con este nombre.")
            End If
        Catch ex As Exception
            ApplicationService.GenerateError("Ya existe un piso con este codigo.")
        End Try
    End Sub

    Protected Overrides Sub RegisterUpdateTasks(ByVal updateProcess As Engine.BE.BusinessProcesses.Process)
        MyBase.RegisterUpdateTasks(updateProcess)
        updateProcess.AddTask(Of DataRow)(AddressOf checkObligaciones)
        'updateProcess.AddTask(Of DataRow)(AddressOf checkObligaciones2)

        'updateProcess.AddTask(Of DataRow)(AddressOf AsignarValoresPredeterminados)
    End Sub

    Public Sub EjecutarSql(ByVal sql As String)
        AdminData.Execute(sql)
    End Sub

    Function EjecutarSqlSelect(ByVal sql As String)

        Dim cadena As DataTable
        cadena = AdminData.GetData(sql)
        Return cadena

    End Function

    Protected Overrides Sub RegisterValidateTasks(ByVal validateProcess As Engine.BE.BusinessProcesses.Process)
        MyBase.RegisterValidateTasks(validateProcess)
        validateProcess.AddTask(Of DataRow)(AddressOf ValidarClaveDuplicada)
    End Sub

    <Task()> Public Shared Sub ValidarClaveDuplicada(ByVal data As DataRow, ByVal services As ServiceProvider)
        If data.RowState = DataRowState.Added Then
            Dim ClsPisos As New Pisos
            Dim filtro As New Engine.Filter
            filtro.Add("Codigo", FilterOperator.Equal, data("Codigo"))

            Dim dt As DataTable = ClsPisos.Filter(filtro)
            If dt.Rows.Count > 0 Then
                ApplicationService.GenerateError("Ya existe un piso con este codigo.")
            End If
        End If
    End Sub
End Class
