CREATE TABLE [config].[utbContactTypes]
(
	[ContactTypeID]	INT IDENTITY	(1, 1) NOT NULL,
	[Type]				VARCHAR(50)		NOT NULL,
	[TypeName]			VARCHAR(100)	NOT NULL,
	[TypeTitle]			VARCHAR(100)	NOT NULL,
	[TypeSubtitle]		VARCHAR(100)	NOT NULL,
	[Order]				INT				NOT NULL,
	[ActiveFlag]		BIT             CONSTRAINT [utbContactTypesDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
	[InsertDate]		DATETIME        CONSTRAINT [utbContactTypesDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]		VARCHAR (100)   CONSTRAINT [utbContactTypesDefaultInsertUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]	DATETIME        CONSTRAINT [utbContactTypesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]	VARCHAR (100)   CONSTRAINT [utbContactTypesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
	CONSTRAINT [utbContactTypeID] PRIMARY KEY CLUSTERED ([ContactTypeID] ASC)
)
