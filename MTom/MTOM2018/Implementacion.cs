using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;

[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class ImplementacionUploader : ControladorUploader
{
    public long ArchivarDocumento(ElementoUploader Elemento)
    {
        long Identificador = Metodos.ArchivarDocumento(Elemento);
        return Identificador;
    }

    public ElementoUploader BuscarDocumento(long Identificador)
    {
        ElementoUploader elemento = Metodos.BuscarDocumento(Identificador);
        return elemento;
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ImplementacionMTOM : ContratoMTOM
    {
        public Stream ObtenerPdf(string Token)
        {
            //return Metodos.ObtenerPdf(Token);}
            Stream temp = null;
            var Archivo = Path.Combine(ConfigurationManager.ConnectionStrings["Repositorio"].ToString(), Token);
            temp = File.OpenRead(Archivo);
            return temp;
        }
    }
}   