# demo-federacion-net-core
proyecto demo de la implementación de la federación en .NET 6 

## Dependencias 
ITfoxtec Saml2
https://github.com/ITfoxtec/ITfoxtec.Identity.Saml2
https://www.itfoxtec.com/identitysaml2

## Implementación
### Generación de certificado .pfx
se generó un certificado con contraseña para tener la respuesta del IDP encriptada
https://www.foxids.com/tools/Certificate 



### Configuración
1.-Modificar informacion del appsettings.json 
2.-Agregar el certificado correspondiente 
3.-Modificar la información del controlador MetadataController
4.-Generar y enviar el xml de los metadatos la dirección de sistemas
