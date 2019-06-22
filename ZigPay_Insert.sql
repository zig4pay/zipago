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
      ('01', '00', 'Amazonas'       , 'A', getdate()),
      ('02', '00', 'Ancash'         , 'A', getdate()),
      ('03', '00', 'Apurimac'       , 'A', getdate()),
      ('04', '00', 'Arequipa'       , 'A', getdate()),
      ('05', '00', 'Ayacucho'       , 'A', getdate()),
      ('06', '00', 'Cajamarca'      , 'A', getdate()),
      ('07', '00', 'Cusco'          , 'A', getdate()),
      ('08', '00', 'Huancavelica'   , 'A', getdate()),
      ('09', '00', 'Huanuco'        , 'A', getdate()),
      ('10', '00', 'Ica'            , 'A', getdate()),
      ('11', '00', 'Junin'          , 'A', getdate()),
      ('12', '00', 'La Libertad'    , 'A', getdate()),
      ('13', '00', 'Lambayeque'     , 'A', getdate()),
      ('14', '00', 'Lima'           , 'A', getdate()),
      ('15', '00', 'Loreto'         , 'A', getdate()),
      ('16', '00', 'Madre de Dios'  , 'A', getdate()),
      ('17', '00', 'Moquegua'       , 'A', getdate()),
      ('18', '00', 'Pasco'          , 'A', getdate()),
      ('19', '00', 'Piura'          , 'A', getdate()),
      ('20', '00', 'Puno'           , 'A', getdate()),
      ('21', '00', 'San Martin'     , 'A', getdate()),
      ('22', '00', 'Tacna'          , 'A', getdate()),
      ('23', '00', 'Tumbes'         , 'A', getdate()),
      ('24', '00', 'Callao'         , 'A', getdate()),
      ('25', '00', 'Ucayali'        , 'A', getdate()),
      ------------------------------------------------------------------------
      ('1401', '14', 'Lima'      , 'A', getdate()),
      ('1402', '14', 'Cajatambo' , 'A', getdate()),
      ('1403', '14', 'Canta'     , 'A', getdate()),
      ('1404', '14', 'Canete'    , 'A', getdate()),
      ('1405', '14', 'Huaura'    , 'A', getdate()),
      ('1406', '14', 'Huarochiri', 'A', getdate()),
      ('1407', '14', 'Yauyos'    , 'A', getdate()),
      ('1408', '14', 'Huaral'    , 'A', getdate()),
      ('1409', '14', 'Barranca'  , 'A', getdate()),
      ('1410', '14', 'Oyon'      , 'A', getdate()),
      ------------------------------------------------------------------------
      ('140101', '1401', 'Lima'                    , 'A', getdate()),
      ('140102', '1401', 'Ancon'                   , 'A', getdate()),
      ('140103', '1401', 'Ate'                     , 'A', getdate()),
      ('140104', '1401', 'Breña'                   , 'A', getdate()),
      ('140105', '1401', 'Carabayllo'              , 'A', getdate()),
      ('140106', '1401', 'Comas'                   , 'A', getdate()),
      ('140107', '1401', 'Chaclacayo'              , 'A', getdate()),
      ('140108', '1401', 'Chorrillos'              , 'A', getdate()),
      ('140109', '1401', 'La Victoria'             , 'A', getdate()),
      ('140110', '1401', 'La Molina'               , 'A', getdate()),
      ('140111', '1401', 'Lince'                   , 'A', getdate()),
      ('140112', '1401', 'Lurigancho'              , 'A', getdate()),
      ('140113', '1401', 'Lurin'                   , 'A', getdate()),
      ('140114', '1401', 'Magdalena del Mar'       , 'A', getdate()),
      ('140115', '1401', 'Miraflores'              , 'A', getdate()),
      ('140116', '1401', 'Pachacamac'              , 'A', getdate()),
      ('140117', '1401', 'Pueblo Libre'            , 'A', getdate()),
      ('140118', '1401', 'Pucusana'                , 'A', getdate()),
      ('140119', '1401', 'Puente Piedra'           , 'A', getdate()),
      ('140120', '1401', 'Punta Hermosa'           , 'A', getdate()),
      ('140121', '1401', 'Punta Negra'             , 'A', getdate()),
      ('140122', '1401', 'Rimac'                   , 'A', getdate()),
      ('140123', '1401', 'San Bartolo'             , 'A', getdate()),
      ('140124', '1401', 'San Isidro'              , 'A', getdate()),
      ('140125', '1401', 'Barranco'                , 'A', getdate()),
      ('140126', '1401', 'San Martin de Porres'    , 'A', getdate()),
      ('140127', '1401', 'San Miguel'              , 'A', getdate()),
      ('140128', '1401', 'Santa Maria del Mar'     , 'A', getdate()),
      ('140129', '1401', 'Santa Rosa'              , 'A', getdate()),
      ('140130', '1401', 'Santiago de Surco'       , 'A', getdate()),
      ('140131', '1401', 'Surquillo'               , 'A', getdate()),
      ('140132', '1401', 'Villa Maria del Triunfo' , 'A', getdate()),
      ('140133', '1401', 'Jesus Maria'             , 'A', getdate()),
      ('140134', '1401', 'Independencia'           , 'A', getdate()),
      ('140135', '1401', 'El Agustino'             , 'A', getdate()),
      ('140136', '1401', 'San Juan de Miraflores'  , 'A', getdate()),
      ('140137', '1401', 'San Juan de Lurigancho'  , 'A', getdate()),
      ('140138', '1401', 'San Luis'                , 'A', getdate()),
      ('140139', '1401', 'Cieneguilla'             , 'A', getdate()),
      ('140140', '1401', 'San Borja'               , 'A', getdate()),
      ('140141', '1401', 'Villa El Salvador'       , 'A', getdate()),
      ('140142', '1401', 'Los Olivos'              , 'A', getdate()),
      ('140143', '1401', 'Santa Anita'             , 'A', getdate())



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