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
   
   
   Insert Into [UBIGEOZIPAGO](      
      CodigoUbigeo, CodigoUbigeoPadre, Nombre, Activo, FechaCreacion
   )
   Values
      ('01', Null, 'Amazonas'       , 'A', getdate()),
      ('02', Null, 'Ancash'         , 'A', getdate()),
      ('03', Null, 'Apurimac'       , 'A', getdate()),
      ('04', Null, 'Arequipa'       , 'A', getdate()),
      ('05', Null, 'Ayacucho'       , 'A', getdate()),
      ('06', Null, 'Cajamarca'      , 'A', getdate()),
      ('07', Null, 'Cusco'          , 'A', getdate()),
      ('08', Null, 'Huancavelica'   , 'A', getdate()),
      ('09', Null, 'Huanuco'        , 'A', getdate()),
      ('10', Null, 'Ica'            , 'A', getdate()),
      ('11', Null, 'Junin'          , 'A', getdate()),
      ('12', Null, 'La Libertad'    , 'A', getdate()),
      ('13', Null, 'Lambayeque'     , 'A', getdate()),
      ('14', Null, 'Lima'           , 'A', getdate()),
      ('15', Null, 'Loreto'         , 'A', getdate()),
      ('16', Null, 'Madre de Dios'  , 'A', getdate()),
      ('17', Null, 'Moquegua'       , 'A', getdate()),
      ('18', Null, 'Pasco'          , 'A', getdate()),
      ('19', Null, 'Piura'          , 'A', getdate()),
      ('20', Null, 'Puno'           , 'A', getdate()),
      ('21', Null, 'San Martin'     , 'A', getdate()),
      ('22', Null, 'Tacna'          , 'A', getdate()),
      ('23', Null, 'Tumbes'         , 'A', getdate()),
      ('24', Null, 'Callao'         , 'A', getdate()),
      ('25', Null, 'Ucayali'        , 'A', getdate())


   Insert Into [TP_TABLA_TABLAS](
      COD_TABLA, NOMBRE_TABLA
      )
   Values
      ('TIPO_PERSONA', 'TIPOS DE PERSONA (ZIPAGO)'),
      ('TIPO_MONEDA', 'TIPOS DE MONEDAS (ZIPAGO)'),
      ('TIPO_DOC_ID', 'TIPOS DE DOCUMENTO DE IDENTIDAD (ZIPAGO)'),      
      ('TIPO_CUENTA', 'TIPOS DE CUENTAS BANCARIAS (ZIPAGO)'),      
      ('RUBRO_NEGOCIO', 'LISTA DE RUBROS DE NEGOCIO (ZIPAGO)'),
      ('DEPARTAMENTO', 'LISTA DE DEPARTAMENTOS DEL PERU (ZIPAGO)'),
      ('PROVINCIA', 'LISTA DE PROVINCIAS DEL PERU (ZIPAGO)'),
      ('DISTRITO', 'LISTA DE DISTRITOS DEL PERU (ZIPAGO)')
         
   Insert Into [TD_TABLA_TABLAS](
      COD_TABLA, VALOR, DESCR_VALOR
      )
   Values
      ('TIPO_PERSONA', '01', 'Juridica'),
      ('TIPO_PERSONA', '02', 'Natural'),
      -------------------------------------------------------------------------
      ('TIPO_MONEDA', '01', 'Soles S/'),
      ('TIPO_MONEDA', '02', 'Dolares $'),
      -------------------------------------------------------------------------
      ('TIPO_DOC_ID', '01', 'DNI - Documento Nacional de Identidad'),
      ('TIPO_DOC_ID', '02', 'RUC - Registro Unico de Contribuyente'),      
      -------------------------------------------------------------------------
      ('TIPO_CUENTA', '01', 'Cuenta Corriente'),
      ('TIPO_CUENTA', '02', 'Cuenta de Ahorros'),
      -------------------------------------------------------------------------
      ('RUBRO_NEGOCIO', '01', 'Restaurantes'),
      ('RUBRO_NEGOCIO', '02', 'Educacion')
      -------------------------------------------------------------------------
      

Commit Tran
Go
SET ANSI_NULLS OFF
SET QUOTED_IDENTIFIER OFF
/*
Select * From TP_TABLA_TABLAS
Select * From TD_TABLA_TABLAS
Select * From [BANCOZIPAGO]
Select * From [UBIGEOZIPAGO]
*/