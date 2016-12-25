select distinct(SchemaId),
                                 SchemaName, 
                                 TableId,
                                 TableName,
                                 ColumnId,
                                 ColumnName,
                                 IsComputed,
                                 Nullable,
                                 [MaxLength],
                                 DataType,
                                 IsForeignKey,
                                 ForeignKeyRefrenceTable,
                                 ForeignKeyRefrenceTableId,
                                 ForeignKeyRefrenceColumn,
                                 ForeignKeyName,
                                 [Default],
                                 ForeignKeyRefrenceSchema,
                                 ForeignKeyParentSchema,
								 PrimaryKeyName
                                 from (select	pks.COLUMN_NAME as PrimaryKeyName,
												schemaes.id as SchemaId,
                                 				schemaes.name as SchemaName,
                                 				tbls.object_id as TableId,
                                 				tbls.name as TableName,
                                 				cols.column_id as ColumnId,
                                 				cols.name as ColumnName,
                                 				cols.is_computed as IsComputed,
                                 				cols.is_nullable as Nullable,
                                 				cols.is_identity as IsIdentity,
                                 				cols.max_length as [MaxLength],
                                 				(select top 1 DATA_TYPE from INFORMATION_SCHEMA.COLUMNS as c where c.TABLE_NAME = tbls.name and c.COLUMN_NAME = cols.name) as DataType,
                                 				(select top 1 COLUMN_DEFAULT from INFORMATION_SCHEMA.COLUMNS as c where c.TABLE_NAME = tbls.name and c.COLUMN_NAME = cols.name) as [Default],
                                 				CAST(ISNULL((select CHARINDEX(fks.ColName,cols.name,0)),0) as bit) as IsForeignKey,
                                 				CAST(ISNULL((select case when ISNULL((select CHARINDEX(fks.ColName,cols.name,0)),0) = 1 then fks.RefrenceTableName end),N'') as nvarchar) as ForeignKeyRefrenceTable,
                                                CAST(ISNULL((select case when ISNULL((select CHARINDEX(fks.ColName,cols.name,0)),0) = 1 then fks.RefrenceTableId end),N'') as int) as ForeignKeyRefrenceTableId,
                                 				CAST(ISNULL((select case when ISNULL((select CHARINDEX(fks.ColName,cols.name,0)),0) = 1 then fks.RefrenceColName end),N'') as nvarchar) as ForeignKeyRefrenceColumn,
                                 				CAST(ISNULL((select case when ISNULL((select CHARINDEX(fks.ColName,cols.name,0)),0) = 1 then fks.name end),N'') as nvarchar) as ForeignKeyName,
                                 				CAST(ISNULL(fks_schema.UNIQUE_CONSTRAINT_SCHEMA,N'') as nvarchar) as ForeignKeyRefrenceSchema,
                                 				CAST(ISNULL(fks.FKSchemaName,N'') as nvarchar) as ForeignKeyParentSchema
                                 		from	   (select name,schema_id as id from sys.schemas where name not in(N'dbo',N'guest',N'sys',N'INFORMATION_SCHEMA') and CHARINDEX(N'db_',name,0) = 0) as schemaes
                                 		inner join (select * from sys.tables) as tbls 
                                 		on schemaes.id = tbls.schema_id
                                 		left outer join  (select * from sys.columns) as cols
                                 		on tbls.object_id = cols.object_id
                                 		left outer join  (select	
                                 									f.name,
                                 									SCHEMA_NAME(f.schema_id) as FKSchemaName,
                                 									f.referenced_object_id as RefrenceTableId,
                                 									OBJECT_NAME(f.referenced_object_id) as RefrenceTableName,
                                 									COL_NAME(fc.referenced_object_id,fc.referenced_column_id) RefrenceColName,
                                 									f.parent_object_id as TableId,
                                 									OBJECT_NAME(f.parent_object_id) TableName,
                                 									COL_NAME(fc.parent_object_id,fc.parent_column_id) ColName
                                 						 from sys.foreign_keys AS f
                                 						 inner join sys.foreign_key_columns AS fc 
                                 						 on f.OBJECT_ID = fc.constraint_object_id
                                 						 inner join sys.tables t 
                                 						 on t.OBJECT_ID = fc.referenced_object_id) as fks
                                 		on tbls.object_id = fks.TableId
										left outer join (select	COLUMN_NAME,
																tc.CONSTRAINT_SCHEMA,
																tc.CONSTRAINT_NAME,
																tc.TABLE_NAME
															from INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
															join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu 
															on tc.CONSTRAINT_NAME = ccu.Constraint_name
															and tc.CONSTRAINT_TYPE = 'Primary Key') as pks
										on pks.CONSTRAINT_SCHEMA = SCHEMA_NAME(tbls.schema_id) and pks.TABLE_NAME = tbls.name
                                 		left outer join (select UNIQUE_CONSTRAINT_SCHEMA,CONSTRAINT_NAME,CONSTRAINT_SCHEMA from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS) as fks_schema
                                 		on fks.name = fks_schema.CONSTRAINT_NAME and fks.FKSchemaName = fks_schema.CONSTRAINT_SCHEMA
                                 		) as R