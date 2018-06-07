using issc.MTOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

/// <summary>
/// Summary description for Contratos
/// </summary>
[ServiceContract]
public interface ControladorUploader {
    [OperationContract]
    long ArchivarDocumento(ElementoUploader Elemento);
    [OperationContract]
    ElementoUploader BuscarDocumento(long Identificador);

}
public interface ContratoMTOM
{
    [OperationContract(Name = "Pdf")]
    [ContentType("application/pdf")]
    [WebGet(UriTemplate = "{Token}")]
    Stream ObtenerPdf(string Token); 
}