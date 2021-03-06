--------------------------------------------------------------------------------------------
--  CONTROLES DEL LOGIN --
--------------------------------------------------------------------------------------------

-- Sacar si es un usuario --
SELECT * FROM ml.USUARIOS WHERE uid = 'mgp'  AND ((Fecha_baja IS NULL) OR (Fecha_Baja > GETDATE()))

-- Sacar si tiene acceso al modulo --
SELECT * FROM ml.USUARIOS_SISTEMAS WHERE sis_nombre = 'SAME' AND uid = 'mgp' AND ((usi_Fecha_baja IS NULL) OR (usi_Fecha_baja > GETDATE()))

-- Sacar parametros del SAME --
SELECT * FROM ml.USUARIOS_PARAMETROS WHERE sis_nombre = 'SAME' AND uid = 'mgp'



--------------------------------------------------------------------------------------------
--  Para Armar la Ventana de Info Gral --
--------------------------------------------------------------------------------------------

-- Lugar Actual del Expediente --
SELECT * FROM ml.SA_V_Lugar_Actual_Expte WHERE ID_Org_Destino = 261
SELECT * FROM ml.SA_V_Ultimo_Pase_por_Expediente WHERE (ID_Destino = 261)

-- Cuantos Expedientes Hay en una Oficina --
SELECT Count(*) FROM ml.SA_V_Ultimo_Pase_por_Expediente WHERE (ID_Destino = 261)

-- Pases Emitidos sin recepcion de la oficina destino --
SELECT Count(*) FROM  ml.SA_V_Ultimo_Pase_por_Expediente WHERE ( ID_Origen = 261 ) AND  ( Fecha_Recepcion IS NULL )    

-- Pases Emitidos a MI oficina y aun no los recepcione --
SELECT Count(*) FROM  ml.SA_V_Ultimo_Pase_por_Expediente WHERE ( ID_Destino = 261 ) AND  ( Fecha_Recepcion IS NULL )    

-- Expedientes que tengo YO solicitados por otra oficina --
SELECT Count(*) FROM ml.sa_registro_solicitudes_exptes WHERE rse_id_org_actual = 261


--------------------------------------------------------------------------------------------
--  Para la Grilla de los Expedientes que estan en nuestra Oficina --
--------------------------------------------------------------------------------------------

SELECT	prefijo, numero, anio, extension, fecha_ingreso, estracto, iniciador, id_organigrama, id_ultimo_pase, 
		remito, fecha_pase, fecha_recepcion, codigo_origen, origen, codigo_destino, destino, dias_oficina,
		ml.SA_fun_Q_adjuntos (prefijo, numero, anio, extension) as Adjuntos,
		ml.SA_fun_Q_rtas (id_ultimo_pase) as Rtas, Texto 
FROM ml.sa_v_lugar_actual_expte  
WHERE ( ml.sa_v_lugar_actual_expte.id_organigrama = 261 )
AND  ( ml.sa_v_lugar_actual_expte.prefijo LIKE '4069' )   
AND  ( ml.sa_v_lugar_actual_expte.numero LIKE '%' )   
AND  ( ml.sa_v_lugar_actual_expte.anio_texto LIKE '2018' )   
AND  ( ml.sa_v_lugar_actual_expte.estracto LIKE '%' )   
AND  ( ml.sa_v_lugar_actual_expte.iniciador LIKE '%' )

SELECT TOP 1000 [Estracto]
      ,[Iniciador]
      ,[Calle]
      ,[Altura]
      ,[Localidad]
      ,[CP]
      ,[TE]
      ,[Tipo_Doc]
      ,[Nro_Doc]
      ,[Mail]
      ,[Fecha_Inicio]
      ,[Fecha_Pase]
      ,[Fecha_Recepcion]
      ,[Texto]
      ,[Codigo_Organigrama]
      ,[Desc_Organigrama]
      ,[Codigo_Tipo]
      ,[Desc_Tipo]
      ,[Prefijo]
      ,[Nro]
      ,[Anio]
      ,[Extension]
      ,[ID_Ultimo_Pase]
      ,[Fecha_Ultimo_Pase]
      ,[Remito_Ultimo_Pase]
      ,[Adjuntos]
      ,[Respuestas]
  FROM [LUJANSAMEDESA].[ml].[SA_V_Expedientes]

--------------------------------------------------------------------------------------------
--  Pases Emitidos SIN RECEPCION --
--------------------------------------------------------------------------------------------

SELECT	prefijo, numero,anio, extension, id_ultimo_pase, remito, fecha_pase, codigo_origen, origen, 
		codigo_destino, destino, estracto, iniciador     
FROM ml.sa_v_ultimo_pase_por_expediente
WHERE (ID_Origen = 261) 
and   (fecha_recepcion is NULL )   


--------------------------------------------------------------------------------------------
--  Pases Emitidos SIN RECIBIR (yo no los recepcione) --
--------------------------------------------------------------------------------------------

SELECT	prefijo, numero, anio, extension, id_ultimo_pase, remito,
		fecha_pase, codigo_origen, origen, codigo_destino, destino, estracto, iniciador     
FROM ml.sa_v_ultimo_pase_por_expediente
WHERE ( ID_Destino = 261 ) 
and   (fecha_recepcion is NULL )   


--------------------------------------------------------------------------------------------
--  Pases Solicitados --
--------------------------------------------------------------------------------------------

SELECT  rse_prefijo_de_expediente, rse_numero_de_expediente, rse_anio_de_expediente, rse_id_extension,
		rse_fecha_de_solicitud, rse_id_org_solicita, org_codigo, org_descripcion     
FROM ml.sa_registro_solicitudes_exptes
INNER JOIN ml.organigrama  ON ( ml.organigrama.org_id_organigrama = ml.sa_registro_solicitudes_exptes.rse_id_org_solicita)   
WHERE ( ( ml.sa_registro_solicitudes_exptes.rse_id_org_actual = 63 ) )  

