Use ZIGPAY
Go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
Go
Begin Tran

   Insert Into [BANCOZIPAGO](      
      NombreLargo, NombreCorto, Activo, FechaCreacion
   )
   Values
      ('Banco de Comercio'                , Null      , 'S', getdate()),
      ('Banco de Crédito del Perú'        , 'BCP'     , 'S', getdate()),
      ('Banco Interamericano de Finanzas' , 'BANBIF'  , 'S', getdate()),
      ('Banco Pichincha'                  , Null      , 'S', getdate()),
      ('BBVA Continental'                 , Null      , 'S', getdate()),
      ('Citibank Peru'                    , Null      , 'S', getdate()),
      ('Interbank'                        , Null      , 'S', getdate()),
      ('MiBanco'                          , Null      , 'S', getdate()),
      ('Scotiabank'                       , Null      , 'S', getdate()),
      ('Banco GNB Peru'                   , Null      , 'S', getdate()),
      ('Banco Falabella'                  , Null      , 'S', getdate()),
      ('Banco Ripley'                     , Null      , 'S', getdate()),
      ('Banco Santander Peru'             , Null      , 'S', getdate()),
      ('Banco Azteca'                     , Null      , 'S', getdate()),
      ('Banco Cencosud'                   , Null      , 'S', getdate()),
      ('ICBC Peru Bank'                   , Null      , 'S', getdate())
   
   
   Insert Into [UBIGEOZIPAGO](      
      CodigoUbigeo, CodigoUbigeoPadre, Nombre, Activo, FechaCreacion
   )
   Values
      ('01', '00', 'Amazonas'       , 'S', getdate()),
      ('02', '00', 'Ancash'         , 'S', getdate()),
      ('03', '00', 'Apurimac'       , 'S', getdate()),
      ('04', '00', 'Arequipa'       , 'S', getdate()),
      ('05', '00', 'Ayacucho'       , 'S', getdate()),
      ('06', '00', 'Cajamarca'      , 'S', getdate()),
      ('07', '00', 'Cusco'          , 'S', getdate()),
      ('08', '00', 'Huancavelica'   , 'S', getdate()),
      ('09', '00', 'Huanuco'        , 'S', getdate()),
      ('10', '00', 'Ica'            , 'S', getdate()),
      ('11', '00', 'Junin'          , 'S', getdate()),
      ('12', '00', 'La Libertad'    , 'S', getdate()),
      ('13', '00', 'Lambayeque'     , 'S', getdate()),
      ('14', '00', 'Lima'           , 'S', getdate()),
      ('15', '00', 'Loreto'         , 'S', getdate()),
      ('16', '00', 'Madre de Dios'  , 'S', getdate()),
      ('17', '00', 'Moquegua'       , 'S', getdate()),
      ('18', '00', 'Pasco'          , 'S', getdate()),
      ('19', '00', 'Piura'          , 'S', getdate()),
      ('20', '00', 'Puno'           , 'S', getdate()),
      ('21', '00', 'San Martin'     , 'S', getdate()),
      ('22', '00', 'Tacna'          , 'S', getdate()),
      ('23', '00', 'Tumbes'         , 'S', getdate()),
      ('24', '00', 'Callao'         , 'S', getdate()),
      ('25', '00', 'Ucayali'        , 'S', getdate()),
      ------------------------------------------------------------------------
      ('1401', '14', 'Lima'      , 'S', getdate()),
      ('1402', '14', 'Cajatambo' , 'S', getdate()),
      ('1403', '14', 'Canta'     , 'S', getdate()),
      ('1404', '14', 'Canete'    , 'S', getdate()),
      ('1405', '14', 'Huaura'    , 'S', getdate()),
      ('1406', '14', 'Huarochiri', 'S', getdate()),
      ('1407', '14', 'Yauyos'    , 'S', getdate()),
      ('1408', '14', 'Huaral'    , 'S', getdate()),
      ('1409', '14', 'Barranca'  , 'S', getdate()),
      ('1410', '14', 'Oyon'      , 'S', getdate()),
      ------------------------------------------------------------------------
      ('140101', '1401', 'Lima'                    , 'S', getdate()),
      ('140102', '1401', 'Ancon'                   , 'S', getdate()),
      ('140103', '1401', 'Ate'                     , 'S', getdate()),
      ('140104', '1401', 'Breña'                   , 'S', getdate()),
      ('140105', '1401', 'Carabayllo'              , 'S', getdate()),
      ('140106', '1401', 'Comas'                   , 'S', getdate()),
      ('140107', '1401', 'Chaclacayo'              , 'S', getdate()),
      ('140108', '1401', 'Chorrillos'              , 'S', getdate()),
      ('140109', '1401', 'La Victoria'             , 'S', getdate()),
      ('140110', '1401', 'La Molina'               , 'S', getdate()),
      ('140111', '1401', 'Lince'                   , 'S', getdate()),
      ('140112', '1401', 'Lurigancho'              , 'S', getdate()),
      ('140113', '1401', 'Lurin'                   , 'S', getdate()),
      ('140114', '1401', 'Magdalena del Mar'       , 'S', getdate()),
      ('140115', '1401', 'Miraflores'              , 'S', getdate()),
      ('140116', '1401', 'Pachacamac'              , 'S', getdate()),
      ('140117', '1401', 'Pueblo Libre'            , 'S', getdate()),
      ('140118', '1401', 'Pucusana'                , 'S', getdate()),
      ('140119', '1401', 'Puente Piedra'           , 'S', getdate()),
      ('140120', '1401', 'Punta Hermosa'           , 'S', getdate()),
      ('140121', '1401', 'Punta Negra'             , 'S', getdate()),
      ('140122', '1401', 'Rimac'                   , 'S', getdate()),
      ('140123', '1401', 'San Bartolo'             , 'S', getdate()),
      ('140124', '1401', 'San Isidro'              , 'S', getdate()),
      ('140125', '1401', 'Barranco'                , 'S', getdate()),
      ('140126', '1401', 'San Martin de Porres'    , 'S', getdate()),
      ('140127', '1401', 'San Miguel'              , 'S', getdate()),
      ('140128', '1401', 'Santa Maria del Mar'     , 'S', getdate()),
      ('140129', '1401', 'Santa Rosa'              , 'S', getdate()),
      ('140130', '1401', 'Santiago de Surco'       , 'S', getdate()),
      ('140131', '1401', 'Surquillo'               , 'S', getdate()),
      ('140132', '1401', 'Villa Maria del Triunfo' , 'S', getdate()),
      ('140133', '1401', 'Jesus Maria'             , 'S', getdate()),
      ('140134', '1401', 'Independencia'           , 'S', getdate()),
      ('140135', '1401', 'El Agustino'             , 'S', getdate()),
      ('140136', '1401', 'San Juan de Miraflores'  , 'S', getdate()),
      ('140137', '1401', 'San Juan de Lurigancho'  , 'S', getdate()),
      ('140138', '1401', 'San Luis'                , 'S', getdate()),
      ('140139', '1401', 'Cieneguilla'             , 'S', getdate()),
      ('140140', '1401', 'San Borja'               , 'S', getdate()),
      ('140141', '1401', 'Villa El Salvador'       , 'S', getdate()),
      ('140142', '1401', 'Los Olivos'              , 'S', getdate()),
      ('140143', '1401', 'Santa Anita'             , 'S', getdate())



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
      -------------------------------------------------------------------------      
      ('TIPO_MONEDA', '01', 'Soles (S/)'),
      ('TIPO_MONEDA', '02', 'Dolares ($)'),
      -------------------------------------------------------------------------
      -------------------------------------------------------------------------
      ('TIPO_DOC_ID', '01', 'DNI - Documento Nacional de Identidad'),
      ('TIPO_DOC_ID', '02', 'RUC - Registro Unico de Contribuyente'),      
      ('TIPO_DOC_ID', '03', 'CE - Carnet de Extranjeria'),
      ('TIPO_DOC_ID', '04', 'OTR - Otros'),
      -------------------------------------------------------------------------
      -------------------------------------------------------------------------
      ('TIPO_CUENTA', '01', 'Cuenta Corriente'),
      ('TIPO_CUENTA', '02', 'Cuenta de Ahorros'),
      -------------------------------------------------------------------------
      -------------------------------------------------------------------------
      ('RUBRO_NEGOCIO', '001', 'Autos'),
      ('RUBRO_NEGOCIO', '002', 'Arquitectura'),
      ('RUBRO_NEGOCIO', '003', 'Boutique'),
      ('RUBRO_NEGOCIO', '004', 'Café'),
      ('RUBRO_NEGOCIO', '005', 'Calzado'),
      ('RUBRO_NEGOCIO', '006', 'Carpintería'),
      ('RUBRO_NEGOCIO', '007', 'Cerámicos'),
      ('RUBRO_NEGOCIO', '008', 'Cirujía Estética'),
      ('RUBRO_NEGOCIO', '009', 'Colegios'),
      ('RUBRO_NEGOCIO', '010', 'Comida'),
      ('RUBRO_NEGOCIO', '011', 'Consultorio Médico'),
      ('RUBRO_NEGOCIO', '012', 'Ferretería'),
      ('RUBRO_NEGOCIO', '013', 'Gimnasios'),
      ('RUBRO_NEGOCIO', '014', 'Hotelería'),
      ('RUBRO_NEGOCIO', '015', 'Ingenieria Civil'),
      ('RUBRO_NEGOCIO', '016', 'Institutos'),
      ('RUBRO_NEGOCIO', '017', 'Mecánica'),
      ('RUBRO_NEGOCIO', '018', 'Movilidad'),
      ('RUBRO_NEGOCIO', '019', 'Muebles'),
      ('RUBRO_NEGOCIO', '020', 'Odontología'),
      ('RUBRO_NEGOCIO', '021', 'Peluquería'),
      ('RUBRO_NEGOCIO', '022', 'Repuestos'),
      ('RUBRO_NEGOCIO', '023', 'Ropa'),
      ('RUBRO_NEGOCIO', '024', 'Telas')
      -------------------------------------------------------------------------
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