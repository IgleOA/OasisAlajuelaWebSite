CREATE TABLE [adm].[utbUsersProfile] (
    [UserID]         INT             NOT NULL,
    [Photo]          VARBINARY (MAX) NULL,
    [PhotoExt]       VARCHAR (10)    NULL,
    [Phone]          VARCHAR (50)    NULL,
    [Mobile]         VARCHAR (50)    NULL,
    [Facebook]       VARCHAR (100)   NULL,
    [Twitter]        VARCHAR (100)   NULL,
    [Snapchat]       VARCHAR (100)   NULL,
    [Instragram]     VARCHAR (100)   NULL,
    [Country]        VARCHAR (100)   NULL,
    [State]          VARCHAR (100)   NULL,
    [City]           VARCHAR (100)   NULL,
    [ActiveFlag]     BIT             CONSTRAINT [utbUsersProfileDefaultActiveFlagTrue] DEFAULT ((1)) NOT NULL,
    [CreationDate]   DATETIME        CONSTRAINT [utbUsersProfileDefaultCreationDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [CreationUser]   VARCHAR (100)   CONSTRAINT [utbUsersProfileDefaultCreationUserSuser_sSame] DEFAULT (suser_sname()) NOT NULL,
    [LastModifyDate] DATETIME        CONSTRAINT [utbUsersProfileDefaultLastModifyDateSysDateTime] DEFAULT (sysdatetime()) NOT NULL,
    [LastModifyUser] VARCHAR (100)   CONSTRAINT [utbUsersProfileDefaultLastModifyUserSuser_Sname] DEFAULT (suser_sname()) NOT NULL,
    CONSTRAINT [utbProfileUserID] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK.adm.utbUsers.adm.utbUsersProfile.UserID] FOREIGN KEY ([UserID]) REFERENCES [adm].[utbUsers] ([UserID])
);


GO
CREATE TRIGGER [adm].[utrLogUsersProfile] ON [adm].[utbUsersProfile]
FOR INSERT,UPDATE
AS
	BEGIN
		DECLARE @INSERTUPDATE VARCHAR(30)
		DECLARE @StartValues	XML = (SELECT [UserID],[PhotoExt],[Phone],[Mobile],[Facebook],[Twitter],[Snapchat],[Instragram],[Country],[State],[City],[ActiveFlag],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] FROM Deleted [Values] for xml AUTO, ELEMENTS XSINIL)
		DECLARE @EndValues		XML = (SELECT [UserID],[PhotoExt],[Phone],[Mobile],[Facebook],[Twitter],[Snapchat],[Instragram],[Country],[State],[City],[ActiveFlag],[CreationDate],[CreationUser],[LastModifyDate],[LastModifyUser] FROM Inserted [Values] for xml AUTO, ELEMENTS XSINIL)

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
				,'[adm].[utbUsersProfile]'
				,(SELECT EventInfo FROM #DBCC)
				,@StartValues
				,@EndValues
				,[LastModifyUser]
				,GETDATE()
		FROM	Inserted
	END;
