using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Odbc;

/// <summary>
/// Summary description for DataHelper
/// </summary>
public class DataHelper {
    static OdbcConnection GetConnection() {
        try {
            OdbcConnection connection = new OdbcConnection("Driver={Microsoft Access Driver (*.mdb)};Dbq=|DataDirectory|\\nwind.mdb;Uid=;Pwd=;");
            connection.Open();
            return connection;
        }
        catch (Exception ex) {
            throw new Exception(String.Format("Connection failed!: {0}", ex.Message), ex);
        }
    }

    public static object ExecuteCommand(string commandText) {
        try {
            using (OdbcConnection connection = GetConnection()) {
                OdbcCommand command = new OdbcCommand(commandText, connection);
                object result = command.ExecuteScalar();
                connection.Close();
                return result;
            }
        }
        catch (Exception ex) {
            throw new Exception(String.Format("Command executing failed!: {0}", ex.Message), ex);
        }
    }

    public static DataTable ProcessSelectCommand(string selectCommandText) {
        try {
            using (OdbcConnection connection = GetConnection()) {
                OdbcDataAdapter adapter = new OdbcDataAdapter(selectCommandText, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);                
                return table;
            }
        }
        catch (Exception ex) {
            throw new Exception(String.Format("Data selecting failed!: {0}", ex.Message), ex);
        }
    }
}