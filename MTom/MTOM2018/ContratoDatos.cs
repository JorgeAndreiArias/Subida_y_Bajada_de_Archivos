using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for ContratoDatos
/// </summary>
public class ElementoUploader
{
    [DataMember]
    public long Identificador { get; set; }
    [DataMember]
    public string NombreOriginal { get; set; }
    [DataMember]
    public byte[] Documento { get; set; }
    [DataMember]
    public string Token { get; set; }
    [DataMember]
    public string URL { get; set; }


}