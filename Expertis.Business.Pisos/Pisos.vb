Imports Solmicro.Expertis
Imports Solmicro.Expertis.Engine.BE.BusinessProcesses
Imports Solmicro.Expertis.Engine.DAL

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
        'ProcessServer.ExecuteTask(Of DataRow)(AddressOf AsignarCentroGestion, data, services)
        'ProcessServer.ExecuteTask(Of DataRow)(AddressOf AsignarContador, data, services)
    End Sub

    <Task()> Public Shared Sub AsignarValoresPredeterminados(ByVal data As DataRow, ByVal services As ServiceProvider)
        'Dim dt As New DataTable
        'dt = AdminData.GetData("SELECT MAX(IDContrato) As ID FROM tbVehiculoContrato")

        'data("IDContrato") = dt(0)(0) + 1
        data("IDPiso") = AdminData.GetAutoNumeric
    End Sub

    Protected Overrides Sub RegisterUpdateTasks(ByVal updateProcess As Engine.BE.BusinessProcesses.Process)
        MyBase.RegisterUpdateTasks(updateProcess)
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

End Class
