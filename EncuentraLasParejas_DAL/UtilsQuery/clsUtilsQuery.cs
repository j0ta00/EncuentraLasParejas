﻿
using EncuentraLasParejas_DAL.MyConnection;
using EncuentraLasParejas_Entities;
using System;
using System.Data.SqlClient;

namespace EncuentraLasParejas_DAL.UtilsQuery
{
    public class clsUtilsQuery
    {
        SqlConnection myConnection;
        SqlDataReader myReader;
        SqlCommand myCommand;

        public SqlDataReader MyReader { get => myReader; set => myReader = value; }
        public clsUtilsQuery()
        {
            myConnection = new clsMyConnection().getConnection();
            myCommand = new SqlCommand();
        }

        /// <summary>
        /// Util method that allow to us execute a sql select instruction
        /// </summary>
        /// <param name="query">String that contains the query to execute</param>
        public void QuerySelect(String query)
        {
            myCommand.CommandText = query;
            myCommand.Connection = myConnection;
            myReader = myCommand.ExecuteReader();
        }
        /// <summary>
        /// Util method that allow to us execute a sql select instruction with a where condition
        /// </summary>
        /// <param name="query">String that contains the query to execute</param>
        /// <param name="value">int parameter that is th id of a object and represent a parameter in the query</param>
        public void QuerySelectWithValueString(String query, string value)
        {
            clsPuntuacion clsPuntuacion=new clsPuntuacion();
            myCommand.Parameters.AddWithValue("nombreJugador", value);
            myCommand.CommandText = query;
            myCommand.Connection = myConnection;
            myReader = myCommand.ExecuteReader();

        }
        /// <summary>
        /// Util method that allow to us execute a sql select instruction delete
        /// </summary>
        /// <param name="query">String that contains the query to execute</param>
        /// <param name="value">int parameter that is th id of a object and represent a parameter in the query</param>
        /// <returns></returns>
        public int QueryDelete(String query, int value)
        {
            int result = 0;
            myCommand.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = value;
            myCommand.CommandText = query;
            myCommand.Connection = myConnection;
            result = myCommand.ExecuteNonQuery();
            return result;
        }

        public int QueryActualizarOInsertarPuntuacion(String query,clsPuntuacion puntuacion){
            myCommand.Parameters.Add("@nombreJugador",System.Data.SqlDbType.VarChar).Value=puntuacion.NombreJugador;
            myCommand.Parameters.Add("@tiempoRealizado", System.Data.SqlDbType.Time).Value = TimeSpan.FromSeconds(double.Parse(puntuacion.Tiempo)).ToString();
            myCommand.CommandText =query;
            myCommand.Connection = myConnection;
            return myCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Util method that close the connection with the database
        /// </summary>      
        public void closeConnection()
        {
            myReader.Close();
            myConnection.Close();
        }
    }
        
}

