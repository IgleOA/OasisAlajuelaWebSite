CREATE TABLE [config].[utbServices] (
    [ServiceID]          INT           IDENTITY (1, 1) NOT NULL,
    [ServiceName]        VARCHAR (50)  NOT NULL,
    [ServiceDescription] VARCHAR (MAX) NOT NULL,
    [ServiceIcon]        VARCHAR (50)  NOT NULL,
    [Order]              INT           NOT NULL,
    [ActiveFlag]         BIT           CONSTRAINT [utbServicesDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [InsertDate]         DATETIME      CONSTRAINT [utbServicesDefaultInsertDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [InsertUser]         VARCHAR (100) CONSTRAINT [utbServicesDefaultInsertUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate]     DATETIME      CONSTRAINT [utbServicesDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser]     VARCHAR (100) CONSTRAINT [utbServicesDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbServicesID] PRIMARY KEY CLUSTERED ([ServiceID] ASC)
);


GO
CREATE TRIGGER [config].[utrServices] ON [config].[utbServices]
FOR INSERT,UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT * FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT * FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

		CREATE TABLE #DBCC (EventType varchar(50), Parameters varchar(50), EventInfo nvarchar(max))

		INSERT INTO #DBCC
		EXEC ('DBCC INPUTBUFFER(@@SPID)')

		--Assume it is an insert
		SET @INSERTUPDATE ='INSERT'
		--If there's data in deleted, it's an update
		IF EXISTS(SELECT * FROM Deleted)
		  SET @INSERTUPDATE='UPDATE'

		INSERT INTO [adm].[utbLogActivities] ([ActivityType],[TargetTable],[SQLStatement],[StartValues],[EndValues],[User],[LogActivityDate])
		SELECT	@INSERTUPDATE
				,'[config].[utbServices]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM Inserted
	END;