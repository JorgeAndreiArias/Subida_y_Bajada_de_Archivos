using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;

/// <summary>
/// Summary description for Metodos
/// </summary>
public static class Metodos
{
    static string Repositorio = ConfigurationManager.ConnectionStrings["Repositorio"].ToString();
    static string URL = ConfigurationManager.ConnectionStrings["URL"].ToString();
    static string Temp = ConfigurationManager.ConnectionStrings["Temporal"].ToString();
    public static string ObtenerConnectionString() {
        return ConfigurationManager.ConnectionStrings["MiBD"].ToString();
    }
    public static long ArchivarDocumento(ElementoUploader Elemento) {
        SqlConexion _conexion = new SqlConexion();
        List<SqlParameter> _Parametros = new List<SqlParameter>();
        DataTableReader _dtr = null;
        long Identificador = 0;
        string Token = Elemento.Token + DateTime.Now.Ticks.ToString();//Otro token con mayor seguridad
        try
        {
            var RutaRepositorio = Path.Combine(Repositorio, Token);
            Elemento.Documento = File.ReadAllBytes(Temp + "\\" + Elemento.Token);
            File.WriteAllBytes(RutaRepositorio, Elemento.Documento);
            if (File.Exists(RutaRepositorio))
            {
                _conexion.Conectar(ObtenerConnectionString());
                _Parametros.Add(new SqlParameter("@Nombre", Elemento.NombreOriginal));
                _Parametros.Add(new SqlParameter("@Token", Token));
                _conexion.PrepararProcedimiento("dbo.GuardarDocumento", _Parametros);
                _dtr = _conexion.EjecutarTableReader();
                while (_dtr.Read())
                {
                    Identificador = long.Parse(_dtr["ID"].ToString() == "" ? "0" : _dtr["ID"].ToString());
                }
                //Aquí se integra la lógica de guardado a base de datos
            }
            return Identificador;
        }
        catch (SqlException sqlEx)
        {
            throw new FaultException(sqlEx.Message, new FaultCode("Error SQL", "50"));
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message, new FaultCode("Error en método", "100"));
        }
        finally {
            _conexion.Desconectar();
            _conexion = null;
            if (_dtr != null)
            {
                _dtr.Close();
                _dtr.Dispose();
            }
        }
    }
    public static ElementoUploader BuscarDocumento(long Identificador) {
        ElementoUploader _elemento = new ElementoUploader();
        SqlConexion _conexion = new SqlConexion();
        DataTableReader dtr = null;
        List<SqlParameter> _Parametros = new List<SqlParameter>();
        try
        {
            _conexion.Conectar(ObtenerConnectionString());
            _Parametros.Add(new SqlParameter("@Identificador", Identificador));
            _conexion.PrepararProcedimiento("dbo.BuscarDocumento", _Parametros);
            dtr = _conexion.EjecutarTableReader();
            while (dtr.Read())
            {
                _elemento.Identificador = Identificador;
                _elemento.NombreOriginal = dtr["Nombre"].ToString();
                _elemento.Token = dtr["Token"].ToString();
                _elemento.URL = URL + "/" + _elemento.Token;
            }
            return _elemento;

        }
        catch (SqlException sqlEx)
        {
            throw new FaultException(sqlEx.Message, new FaultCode("Error SQL", "50"));
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.Message, new FaultCode("Error en método", "100"));
        }
        finally {
            _conexion.Desconectar();
            _conexion = null;
            if (dtr != null) { dtr.Close(); dtr.Dispose(); }
        }
    }
    public static Stream ObtenerPdf(string Token) {
        Stream temp = null;
        var Archivo = Path.Combine(ConfigurationManager.ConnectionStrings["Repositorio"].ToString(), Token);
        temp = File.OpenRead(Archivo);
        return temp;
    }
}