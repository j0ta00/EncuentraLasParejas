
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
        public clsPuntuacion QuerySelectWithValueString(String query, string value)
        {
            clsPuntuacion clsPuntuacion=new clsPuntuacion();
            myCommand.Parameters.AddWithValue("nombreJugador", value);
            myCommand.CommandText = query;
            myCommand.Connection = myConnection;
            clsPuntuacion = (clsPuntuacion)myCommand.ExecuteScalar();
            return clsPuntuacion;
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
        ///// <summary>
        ///// Util method that allow to us execute a sql select instruction delete
        ///// </summary>
        ///// <param name="query">String that contains the query to execute</param>
        ///// <param name="person">person object that contains the differents parameters of the query</param>
        ///// <returns></returns>
        //public int QueryUpdateAndAddPerson(String query, clsPerson person)
        //{
        //    int result = 0;
        //    if (person.PersonId != 0)
        //    {
        //        myCommand.Parameters.Add("@idPersona", System.Data.SqlDbType.VarChar).Value = person.PersonId;
        //    }
        //    if (person.Name is null) { myCommand.Parameters.Add("@nombrePersona", System.Data.SqlDbType.VarChar).Value = DBNull.Value; } else { myCommand.Parameters.Add("@nombrePersona", System.Data.SqlDbType.VarChar).Value = person.Name; }
        //    if (person.Surname is null) { myCommand.Parameters.Add("@apellidosPersona", System.Data.SqlDbType.VarChar).Value = DBNull.Value; } else { myCommand.Parameters.Add("@apellidosPersona", System.Data.SqlDbType.VarChar).Value = person.Surname; }
        //    myCommand.Parameters.Add("@fechaNacimiento", System.Data.SqlDbType.Date).Value = person.BornDate;//No habría que validar la fecha ya que el campo de la base de datos es tipo date y acepta el valor mínimo 01-01-0001
        //    if (person.PhoneNumber is null) { myCommand.Parameters.Add("@telefono", System.Data.SqlDbType.VarChar).Value = DBNull.Value; } else { myCommand.Parameters.Add("@telefono", System.Data.SqlDbType.VarChar).Value = person.PhoneNumber; }
        //    if (person.Address is null) { myCommand.Parameters.Add("@direccion", System.Data.SqlDbType.VarChar).Value = DBNull.Value; } else { myCommand.Parameters.Add("@direccion", System.Data.SqlDbType.VarChar).Value = person.Address; };
        //    myCommand.Parameters.Add("@idDepartamento", System.Data.SqlDbType.Int).Value = person.DepartmentId;
        //    if (person.Photo is null) { myCommand.Parameters.Add("@foto", System.Data.SqlDbType.VarChar).Value = DBNull.Value; } else { myCommand.Parameters.Add("@foto", System.Data.SqlDbType.VarChar).Value = person.Photo; };
        //    myCommand.CommandText = query;
        //    myCommand.Connection = myConnection;
        //    result = myCommand.ExecuteNonQuery();
        //    return result;
        //}
        ///// <summary>
        ///// Util method that allow to us execute a sql select instructions update and insert
        ///// </summary>
        ///// <param name="query">String that contains the query to execute</param>
        ///// <param name="department">department object that contains the differents parameters of the query</param>
        ///// <returns></returns>
        //public int QueryUpdateAndAddDepartment(String query, clsDepartment department)
        //{
        //    int result = 0;
        //    myCommand.Parameters.Add("@idDepartamento", System.Data.SqlDbType.Int).Value = department.DepartmentId;
        //     if (department.DepartmentName is null) { myCommand.Parameters.Add("@nombreDepartamento", System.Data.SqlDbType.VarChar).Value=DBNull.Value; } else { myCommand.Parameters.Add("@nombreDepartamento", System.Data.SqlDbType.VarChar).Value = department.DepartmentName; }
        //    myCommand.CommandText = query;
        //    myCommand.Connection = myConnection;
        //    result = myCommand.ExecuteNonQuery();
        //    return result;


        public int QueryActualizarOInsertarPuntuacion(String query,clsPuntuacion puntuacion){
            myCommand.Parameters.Add("@nombreJugador",System.Data.SqlDbType.VarChar).Value=puntuacion.NombreJugador;
            myCommand.Parameters.Add("@tiempoRealizado", System.Data.SqlDbType.Time).Value = puntuacion.Tiempo;
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

        //public void editarPlanta(int id,double precio){
        //    myCommand.Parameters.Add("@idPlanta",System.Data.SqlDbType.Int).Value=id;
        //    myCommand.Parameters.Add("@precio",System.Data.SqlDbType.Float).Value=(float)precio;
        //    myCommand.CommandText = "Update Plantas Set precio=@precio  Where idPlanta=@idPlanta";
        //    myCommand.Connection = myConnection;
        //    myCommand.ExecuteNonQuery();
        //}

    }
        
}

