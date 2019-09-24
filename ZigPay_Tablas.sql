Use ZIGPAY
Go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
Go
Begin Tran
   
   CREATE TABLE [dbo].[USUARIOZIPAGO](
	   [IdUsuarioZiPago]	            Int Identity(1,1) Not Null,
      [Clave1]	                     varchar(100)	   Not Null,
	   [Clave2]	                     varchar(500)	   Not Null,
      [ApellidosUsuario]	         varchar(200)	   Not Null,
      [NombresUsuario]	            varchar(100)	   Not Null,
      [CodigoRubroNegocio]	         varchar(20)       Null,
	   [CodigoTipoPersona]	         varchar(20)       Null,
	   [CodigoTipoDocumento]         varchar(20)       Null,
	   [NumeroDocumento]	            varchar(11)	      Null,
	   [RazonSocial]	               varchar(100)	   Null,
      [CodigoTipoDocumentoContacto] varchar(20)       Null,
	   [NumeroDocumentoContacto]     varchar(11)	      Null,
	   [ApellidoPaterno]	            varchar(100)	   Null,
	   [ApellidoMaterno]	            varchar(100)	   Null,
      [Nombres]	                  varchar(100)	   Null,
	   [Sexo]	                     char(1)	         Null,
	   [FechaNacimiento]	            smalldatetime	   Null,
	   [TelefonoMovil]	            varchar(20)	      Null,
	   [TelefonoFijo]	               varchar(15)	      Null,
      [AceptoTerminos]              char(1)           Not Null,
      [EstadoRegistro]              char(1)           Not Null,
	   [Activo]	                     char(1)	         Not Null,
      [ClaveRecuperacion]           varchar(500)	   Null,
      [FechaGeneracionClave]        datetime	         Null,
	   [FechaCreacion]	            datetime	         Not Null,
	   [FechaActualizacion]          datetime	         Null
      
      CONSTRAINT [PK_USUARIOZIPAGO] PRIMARY KEY CLUSTERED 
      (
	      [IdUsuarioZiPago]
      )
   )
   
   CREATE TABLE [dbo].[DOMICILIOZIPAGO](
	   [IdDomicilioZiPago]	   Int Identity(1,1) Not Null,
      [IdUsuarioZiPago]	      Int               Not Null,      
      [CodigoDepartamento]	   varchar(20)       Not Null,
      [CodigoProvincia]	      varchar(20)       Not Null,
      [CodigoDistrito]	      varchar(20)       Not Null,
      [Via]	                  varchar(80)	      Not Null,
      [DireccionFacturacion]	varchar(150)      Null,
      [Referencia]	         varchar(200)	   Null,
      [Activo]	               char(1)	         Not Null,
      [FechaCreacion]	      datetime	         Not Null,
      [FechaActualizacion]	   datetime	         Null
      	   
      CONSTRAINT [PK_DOMICILIOZIPAGO] PRIMARY KEY CLUSTERED 
      (
	      [IdDomicilioZiPago]
      )
   )
   
   CREATE TABLE [dbo].[BANCOZIPAGO](
	   [IdBancoZiPago]      Int Identity(1,1) Not Null,
      [NombreLargo]        varchar(60)       Not Null,
      [NombreCorto]        varchar(20)       Null,
      [Activo]	            char(1)	         Not Null,
      [FechaCreacion]	   datetime	         Not Null,
      [FechaActualizacion]	datetime	         Null
            	   
      CONSTRAINT [PK_BANCOSZIPAGO] PRIMARY KEY CLUSTERED 
      (
	      [IdBancoZiPago]
      )
   )

   CREATE TABLE [dbo].[CUENTABANCARIAZIPAGO](
	   [IdCuentaBancaria]   Int Identity(1,1) Not Null,
      [IdUsuarioZiPago]	   Int               Not Null,
      [IdBancoZiPago]	   Int               Not Null,
      [NumeroCuenta]	      varchar(20)	      Not Null,
      [CodigoTipoCuenta]	varchar(20)	      Not Null,
      [CodigoTipoMoneda]	varchar(20)	      Not Null,
      [CCI]	               char(20)	         Null,
      [Activo]	            char(1)	         Not Null,
      [FechaCreacion]	   datetime	         Not Null,
      [FechaActualizacion]	datetime	         Null
      
      CONSTRAINT [PK_CUENTABANCARIAZIPAGO] PRIMARY KEY CLUSTERED 
      (
	      [IdCuentaBancaria]
      )
   )
   
   CREATE TABLE [dbo].[COMERCIOZIPAGO](
	   [IdComercioZiPago]   Int Identity(1,1) Not Null,
      [CodigoComercio]     varchar(14)       Not Null,
      [IdUsuarioZiPago]	   int	            Not Null,
      [Descripcion]	      varchar(30)	      Not Null,      
      [CorreoNotificacion] varchar(100)	   Not Null,
      [Confirmado]	      char(1)	         Null,
      [Activo]	            char(1)	         Not Null,
      [FechaCreacion]	   datetime	         Not Null,
      [FechaActualizacion]	datetime	         Null
            	   
      CONSTRAINT [PK_COMERCIOZIPAGO] PRIMARY KEY CLUSTERED 
      (
	      [IdComercioZiPago]
      )
   )

   CREATE TABLE [dbo].[COMERCIOCUENTAZIPAGO](
	   [IdComercioZiPago]   Int      Not Null,
      [IdCuentaBancaria]   Int      Not Null,
      [Activo]	            char(1)	Not Null,
      [FechaCreacion]	   datetime	Not Null,
      [FechaActualizacion]	datetime	Null
            	   
      CONSTRAINT [PK_COMERCIOCUENTAZIPAGO] PRIMARY KEY CLUSTERED 
      (
	      [IdComercioZiPago], [IdCuentaBancaria]
      )
   )
   
   CREATE TABLE [dbo].[UBIGEOZIPAGO](
	   [IdUbigeoZiPago]     Int Identity(1,1) Not Null,
      [CodigoUbigeo]       varchar(6)        Not Null,
      [CodigoUbigeoPadre]  varchar(6)        Null,
      [Nombre]             varchar(50)       Not Null,
      [Activo]	            char(1)	         Not Null,
      [FechaCreacion]	   datetime	         Not Null,
      [FechaActualizacion]	datetime	         Null
            	   
      CONSTRAINT [PK_UBIGEOZIPAGO] PRIMARY KEY CLUSTERED 
      (
	      [IdUbigeoZiPago]
      )
   )

   CREATE UNIQUE INDEX IX_USUARIOZIPAGO_01
      ON [USUARIOZIPAGO] (Clave1)
   
   CREATE UNIQUE INDEX IX_CUENTABANCARIAZIPAGO_01
      ON [CUENTABANCARIAZIPAGO] (IdBancoZiPago, NumeroCuenta)
   
   CREATE UNIQUE INDEX IX_COMERCIOZIPAGO_01
      ON [COMERCIOZIPAGO] (CodigoComercio)
   
   ALTER TABLE [DOMICILIOZIPAGO]
      ADD CONSTRAINT [FK_USUARIOZIPAGO_DOMICILIOZIPAGO]
         FOREIGN KEY ([IdUsuarioZiPago]) REFERENCES [USUARIOZIPAGO]([IdUsuarioZiPago])

   ALTER TABLE [COMERCIOZIPAGO]
      ADD CONSTRAINT [FK_USUARIOZIPAGO_COMERCIOZIPAGO]
         FOREIGN KEY ([IdUsuarioZiPago]) REFERENCES [USUARIOZIPAGO]([IdUsuarioZiPago])
   
   ALTER TABLE [CUENTABANCARIAZIPAGO]
      ADD CONSTRAINT [FK_USUARIOZIPAGO_CUENTABANCARIAZIPAGO]
         FOREIGN KEY ([IdUsuarioZiPago]) REFERENCES [USUARIOZIPAGO]([IdUsuarioZiPago])

   ALTER TABLE [CUENTABANCARIAZIPAGO]
      ADD CONSTRAINT [FK_BANCOZIPAGO_CUENTABANCARIAZIPAGO]
         FOREIGN KEY ([IdBancoZiPago]) REFERENCES [BANCOZIPAGO]([IdBancoZiPago])

   ALTER TABLE [COMERCIOCUENTAZIPAGO]
      ADD CONSTRAINT [FK_CUENTABANCARIAZIPAGO_COMERCIOCUENTAZIPAGO]
         FOREIGN KEY ([IdCuentaBancaria]) REFERENCES [CUENTABANCARIAZIPAGO]([IdCuentaBancaria])
   
   ALTER TABLE [COMERCIOCUENTAZIPAGO]
      ADD CONSTRAINT [FK_COMERCIOZIPAGO_COMERCIOCUENTAZIPAGO]
         FOREIGN KEY ([IdComercioZiPago]) REFERENCES [COMERCIOZIPAGO]([IdComercioZiPago])


Commit Tran
Go
SET ANSI_NULLS OFF
SET QUOTED_IDENTIFIER OFF