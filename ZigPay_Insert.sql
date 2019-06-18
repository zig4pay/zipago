Use ZIGPAY
Go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
Go
Begin Tran

   Insert Into [BANCOZIPAGO](      
      NombreLargo, NombreCorto, Activo, FechaCreacion, FechaActualizacion
   )
   Values
      ('Banco de Comercio', Null, 'A', getdate(), getdate()),
      ('Banco de Crédito del Perú', 'BCP', 'A', getdate(), getdate()),
      ('Banco Interamericano de Finanzas', 'BANBIF', 'A', getdate(), getdate()),
      ('Banco Pichincha', Null, 'A', getdate(), getdate()),
      ('BBVA Continental', Null, 'A', getdate(), getdate()),
      ('Citibank Peru', Null, 'A', getdate(), getdate()),
      ('Interbank', Null, 'A', getdate(), getdate()),
      ('MiBanco', Null, 'A', getdate(), getdate()),
      ('Scotiabank', Null, 'A', getdate(), getdate()),
      ('Banco GNB Peru', Null, 'A', getdate(), getdate()),
      ('Banco Falabella', Null, 'A', getdate(), getdate()),
      ('Banco Ripley', Null, 'A', getdate(), getdate()),
      ('Banco Santander Peru', Null, 'A', getdate(), getdate()),
      ('Banco Azteca', Null, 'A', getdate(), getdate()),
      ('Banco Cencosud', Null, 'A', getdate(), getdate()),
      ('ICBC Peru Bank', Null, 'A', getdate(), getdate())

   Insert Into [TP_TABLA_TABLAS](
      COD_TABLA, NOMBRE_TABLA
      )
   Values
      ('TIPO_PERSONA', 'TIPOS DE PERSONA (ZIPAGO)'),
      ('TIPO_MONEDA', 'TIPOS DE MONEDAS (ZIPAGO)'),
      ('TIPO_DOC_ID', 'TIPOS DE DOCUMENTO DE IDENTIDAD (ZIPAGO)'),      
      ('RUBRO_NEGOCIO', 'LISTA DE RUBROS DE NEGOCIO (ZIPAGO)'),
      ('DEPARTAMENTO', 'LISTA DE DEPARTAMENTOS DEL PERU (ZIPAGO)'),
      ('PROVINCIA', 'LISTA DE PROVINCIAS DEL PERU (ZIPAGO)'),
      ('DISTRITO', 'LISTA DE DISTRITOS DEL PERU (ZIPAGO)')
         
   Insert Into [TD_TABLA_TABLAS](
      COD_TABLA, VALOR, DESCR_VALOR
      )
   Values
      ('TIPO_PERSONA', '01', 'Persona Natural'),
      ('TIPO_PERSONA', '02', 'Persona Juridica'),
      ('TIPO_MONEDA', '01', 'Soles S/'),
      ('TIPO_MONEDA', '02', 'Dolares $'),
      ('TIPO_DOC_ID', '01', 'DNI - Documento Nacional de Identidad'),
      ('TIPO_DOC_ID', '02', 'RUC - Registro Unico de Contribuyente'),      
      ('RUBRO_NEGOCIO', '01', 'Restaurantes'),
      ('RUBRO_NEGOCIO', '02', 'Educacion')

Commit Tran
Go
SET ANSI_NULLS OFF
SET QUOTED_IDENTIFIER OFF
/*
Select * From TP_TABLA_TABLAS
Select * From TD_TABLA_TABLAS
Select * From [BANCOZIPAGO]
*/