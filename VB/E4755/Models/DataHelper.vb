Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Data
Imports System.Data.Odbc

''' <summary>
''' Summary description for DataHelper
''' </summary>
Public Class DataHelper
	Private Shared Function GetConnection() As OdbcConnection
		Try
			Dim connection As New OdbcConnection("Driver={Microsoft Access Driver (*.mdb)};Dbq=|DataDirectory|\nwind.mdb;Uid=;Pwd=;")
			connection.Open()
			Return connection
		Catch ex As Exception
			Throw New Exception(String.Format("Connection failed!: {0}", ex.Message), ex)
		End Try
	End Function

	Public Shared Function ExecuteCommand(ByVal commandText As String) As Object
		Try
			Using connection As OdbcConnection = GetConnection()
				Dim command As New OdbcCommand(commandText, connection)
				Dim result As Object = command.ExecuteScalar()
				connection.Close()
				Return result
			End Using
		Catch ex As Exception
			Throw New Exception(String.Format("Command executing failed!: {0}", ex.Message), ex)
		End Try
	End Function

	Public Shared Function ProcessSelectCommand(ByVal selectCommandText As String) As DataTable
		Try
			Using connection As OdbcConnection = GetConnection()
				Dim adapter As New OdbcDataAdapter(selectCommandText, connection)
				Dim table As New DataTable()
				adapter.Fill(table)
				Return table
			End Using
		Catch ex As Exception
			Throw New Exception(String.Format("Data selecting failed!: {0}", ex.Message), ex)
		End Try
	End Function
End Class